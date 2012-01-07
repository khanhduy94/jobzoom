CREATE PROCEDURE [dbo].[GetPivotJob]
	@Prefix NVARCHAR(50),
	@JobTitle	NVARCHAR(128)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @listCol VARCHAR(MAX),
			@SQLString NVARCHAR(MAX),
			@viewName NVARCHAR(128)

	SET @viewName = @Prefix + @JobTitle
	SET @viewName = REPLACE(@viewName,' ','')

	SELECT  @listCol = STUFF(( SELECT DISTINCT '],[' + ltrim(TagName)
							FROM TagAttribute
							INNER JOIN [Job.Posting] on [Job.Posting].JobPostingId = TagAttribute.ObjectId
							WHERE [Job.Posting].JobTitle= @JobTitle
							ORDER BY '],[' + ltrim(TagName)							
							FOR XML PATH('')), 1, 2, '') + ']'

	--DROP VIEW PivotJob
	SET @SQLString = N'IF EXISTS(SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS 
						WHERE TABLE_NAME =''' + @viewName +  ''')
						DROP VIEW ['+  @viewName +']'
	EXECUTE (@SQLString)

	SET @SQLString = N'CREATE VIEW [' + @viewName + '] AS
						SELECT row_number() over (order by pvt.JobPostingId desc) as ID, pvt.* FROM (
							SELECT [Job.Posting].JobPostingId, [Job.Posting].UserId, [Job.Posting].CompanyId, [Job.Posting].CompanyName, [Job.Posting].JobTitle, [Job.Approval].IsApproved, TagAttribute.TagName 
							FROM [Job.Posting]
							INNER JOIN TagAttribute on [Job.Posting].JobPostingId = TagAttribute.ObjectId
							LEFT JOIN [Job.Approval] ON [Job.Approval].JobPostingId = [Job.Posting].JobPostingId
							WHERE [Job.Posting].JobTitle = '''+ @JobTitle + '''
							Group by [Job.Posting].JobPostingId, [Job.Posting].UserId, [Job.Posting].CompanyId, [Job.Posting].CompanyName, [Job.Posting].JobTitle, [Job.Approval].IsApproved, TagAttribute.TagName 
						)t
						PIVOT (COUNT(TagName) FOR TagName
						IN ('+@listCol+')) AS pvt'						
	EXECUTE (@SQLString)
END