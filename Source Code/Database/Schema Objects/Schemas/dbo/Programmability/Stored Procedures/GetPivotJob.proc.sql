CREATE PROCEDURE [dbo].[GetPivotJob]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @listCol VARCHAR(MAX)
	DECLARE @SQLString NVARCHAR(MAX)
	SELECT  @listCol = STUFF(( SELECT DISTINCT '],[' + ltrim(AttributeTagName)
							FROM AttributeTag
							INNER JOIN [Job.Posting] on [Job.Posting].JobPostingId = AttributeTag.ObjectId
							ORDER BY '],[' + ltrim(AttributeTagName)
							FOR XML PATH('')), 1, 2, '') + ']'

	--DROP VIEW PivotJob
	IF EXISTS(SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_NAME = 'PivotJob')
	DROP VIEW PivotJob

	SET @SQLString = N'CREATE VIEW PivotJob
						AS
						SELECT * FROM (
							SELECT [Job.Posting].JobPostingId, [Job.Posting].UserId, [Job.Posting].CompanyId, [Job.Posting].CompanyName, [Job.Posting].JobTitle, [Job.Approval].IsApproved, AttributeTag.AttributeTagName 
							FROM [Job.Posting]
							INNER JOIN AttributeTag on [Job.Posting].JobPostingId = AttributeTag.ObjectId
							LEFT JOIN [Job.Approval] ON [Job.Approval].JobPostingId = [Job.Posting].JobPostingId
							Group by [Job.Posting].JobPostingId, [Job.Posting].UserId, [Job.Posting].CompanyId, [Job.Posting].CompanyName, [Job.Posting].JobTitle, [Job.Approval].IsApproved, AttributeTag.AttributeTagName 
						)t
						PIVOT (COUNT(AttributeTagName) FOR AttributeTagName
						IN ('+@listCol+')) AS pvt'						
	EXECUTE (@SQLString)

END