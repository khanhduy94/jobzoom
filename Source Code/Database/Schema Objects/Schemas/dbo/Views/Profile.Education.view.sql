CREATE VIEW [dbo].[Profile.Education]
AS
	SELECT [Profile].ID, Tag.TagName, TagDetail.Location, TagDetail.StartDate, TagDetail.EndDate, TagDetail.[Description], TagDetail.Attachment  
	FROM [Profile]
	INNER JOIN Tag ON [Profile].ID = Tag.ObjectID
	INNER JOIN TagDetail ON Tag.ID = TagDetail.TagID
	WHERE Tag.ParentID = '3B0312C3-E588-4AB3-A396-B8E328A0B312'
