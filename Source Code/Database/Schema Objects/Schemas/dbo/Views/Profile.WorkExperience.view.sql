CREATE VIEW [dbo].[Profile.WorkExperience]
AS
	SELECT [Profile].ID, Tag.TagName, TagDetail.Location, TagDetail.StartDate, TagDetail.EndDate, TagDetail.[Description], TagDetail.Attachment  
	FROM [Profile]
	INNER JOIN Tag ON [Profile].ID = Tag.ObjectID
	INNER JOIN TagDetail ON Tag.ID = TagDetail.TagID
	WHERE Tag.ParentID = 'BDD12E21-6DDB-41B8-891E-30FF2D25EE62'
