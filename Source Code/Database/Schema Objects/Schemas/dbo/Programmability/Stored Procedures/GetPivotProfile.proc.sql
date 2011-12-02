CREATE PROCEDURE [dbo].[GetPivotProfile]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @listCol VARCHAR(MAX)
	DECLARE @SQLString NVARCHAR(MAX)
	SELECT  @listCol = STUFF(( SELECT DISTINCT '],[' + ltrim(AttributeTagName)
							FROM AttributeTag
							INNER JOIN [Profile.Basic] on [Profile.Basic].ProfileBasicId = AttributeTag.ObjectId
							ORDER BY '],[' + ltrim(AttributeTagName)
							FOR XML PATH('')), 1, 2, '') + ']'
	IF EXISTS(SELECT TABLE_NAME FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_NAME = 'PivotProfile')
	DROP VIEW PivotProfile

	SET @SQLString = N'CREATE VIEW PivotProfileView
					AS
					SELECT row_number() over (order by pvt.ProfileBasicId desc) as Id, pvt.* FROM (
						SELECT [Profile.Basic].ProfileBasicId, [Profile.Basic].UserId, [Job.Approval].JobPostingId, [Job.Posting].JobTitle, [Job.Approval].IsApproved, AttributeTag.AttributeTagName
						FROM [Profile.Basic]
						INNER JOIN AttributeTag on [Profile.Basic].ProfileBasicId = AttributeTag.ObjectId
						LEFT JOIN [Job.Approval] ON [Job.Approval].ProfileID = [Profile.Basic].ProfileBasicId
						INNER JOIN [Job.Posting] ON [Job.Posting].JobPostingId = [Job.Approval].JobPostingId
						GROUP BY [Profile.Basic].ProfileBasicId, [Profile.Basic].UserId, [Job.Approval].JobPostingId, [Job.Posting].JobTitle, [Job.Approval].IsApproved, AttributeTag.AttributeTagName					
					)t
					PIVOT (COUNT(AttributeTagName) FOR AttributeTagName
					IN ('+@listCol+')) AS pvt'
						
	EXECUTE (@SQLString)
END