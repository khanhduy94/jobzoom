CREATE PROCEDURE [dbo].[GetPivotJobRequirement]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @listCol VARCHAR(2000)
	DECLARE @SQLString NVARCHAR(1000)
	SELECT  @listCol = STUFF(( SELECT DISTINCT '],[' + ltrim(TagName)
							FROM Tag
							INNER JOIN Job on Job.ID = Tag.ObjectID
							ORDER BY '],[' + ltrim(TagName)
							FOR XML PATH('')), 1, 2, '') + ']'
	--DROP VIEW PivotJobRequirement
	SET @SQLString = N'ALTER VIEW PivotJobRequirement
						AS
						SELECT * FROM (
							SELECT Job.ID, Job.JobTitle, JobApproval.IsApply, Tag.TagName 
							FROM Job
							INNER JOIN Tag on Job.ID = Tag.ObjectID
							INNER JOIN JobApproval ON JobApproval.JobID = Job.ID
							Group by Job.ID,  Job.JobTitle, Tag.TagName, JobApproval.IsApply
						)t
						PIVOT (COUNT(TagName) FOR TagName
						IN ('+@listCol+')) AS pvt'						
	EXECUTE (@SQLString)

END

