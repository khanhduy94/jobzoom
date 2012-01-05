CREATE PROCEDURE [dbo].[GetPivotProfileByCompany]
	@Prefix NVARCHAR(50),
	@JobTitle	NVARCHAR(128),
	@CompanyName NVARCHAR(128)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @listCol VARCHAR(MAX),
			@SQLString NVARCHAR(MAX),
			@viewName NVARCHAR(128)

	SET @viewName = @Prefix + @JobTitle + @CompanyName
	SET @viewName = REPLACE(@viewName,' ','')

	SELECT  @listCol = STUFF(( SELECT DISTINCT '],[' + ltrim(TagName)
							FROM TagAttribute
							INNER JOIN [Profile.Basic] on [Profile.Basic].ProfileBasicId = TagAttribute.ObjectId
							LEFT JOIN [Job.Approval] ON [Job.Approval].ProfileID = [Profile.Basic].ProfileBasicId
							INNER JOIN [Job.Posting] ON [Job.Posting].JobPostingId = [Job.Approval].JobPostingId
							WHERE [Job.Posting].JobTitle = @JobTitle
							AND [Job.Posting].CompanyName = @CompanyName
							ORDER BY '],[' + ltrim(TagName)
							FOR XML PATH('')), 1, 2, '') + ']'

	SET @SQLString = N'IF EXISTS(SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS 
						WHERE TABLE_NAME =''' + @viewName +  ''')
						DROP VIEW ['+  @viewName + ']'
	EXECUTE (@SQLString)

	SET @SQLString = N'CREATE VIEW [' + @viewName + '] AS
					SELECT row_number() over (order by pvt.ProfileBasicId desc) as ID, pvt.* FROM (
						SELECT [Profile.Basic].ProfileBasicId, [Profile.Basic].UserId, [Job.Approval].JobPostingId, [Job.Posting].JobTitle, [Job.Approval].IsApproved, TagAttribute.TagName
						FROM [Profile.Basic]
						INNER JOIN TagAttribute on [Profile.Basic].ProfileBasicId = TagAttribute.ObjectId
						LEFT JOIN [Job.Approval] ON [Job.Approval].ProfileID = [Profile.Basic].ProfileBasicId
						INNER JOIN [Job.Posting] ON [Job.Posting].JobPostingId = [Job.Approval].JobPostingId
						WHERE [Job.Posting].JobTitle = '''+ @JobTitle + '''
						AND [Job.Posting].CompanyName = '''+ @CompanyName + '''
						GROUP BY [Profile.Basic].ProfileBasicId, [Profile.Basic].UserId, [Job.Approval].JobPostingId, [Job.Posting].JobTitle, [Job.Approval].IsApproved, TagAttribute.TagName					
					)t
					PIVOT (COUNT(TagName) FOR TagName
					IN ('+@listCol+')) AS pvt'
				
	EXECUTE (@SQLString)
END
