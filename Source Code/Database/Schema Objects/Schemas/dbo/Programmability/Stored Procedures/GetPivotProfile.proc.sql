-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[GetPivotProfile]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @listCol VARCHAR(2000)
	DECLARE @SQLString NVARCHAR(1000)
	SELECT  @listCol = STUFF(( SELECT DISTINCT '],[' + ltrim(TagName)
							FROM Tag
							INNER JOIN [Profile] on [Profile].ID = Tag.ObjectID
							ORDER BY '],[' + ltrim(TagName)
							FOR XML PATH('')), 1, 2, '') + ']'
	 
	SET @SQLString = N'ALTER VIEW PivotProfile
					AS
					SELECT * FROM (
						SELECT [Profile].ID, JobApproval.IsApproved, Tag.TagName
						FROM [Profile]
						INNER JOIN Tag on [Profile].ID = Tag.ObjectID
						INNER JOIN JobApproval ON JobApproval.ProfileID = [Profile].ID
						Group by [Profile].ID, JobApproval.IsApproved, Tag.TagName
					)t
					PIVOT (COUNT(TagName) FOR TagName
					IN ('+@listCol+')) AS pvt'
					
						
	EXECUTE (@SQLString)
END

