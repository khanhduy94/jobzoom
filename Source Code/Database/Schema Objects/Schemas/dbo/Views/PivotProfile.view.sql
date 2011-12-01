CREATE VIEW [dbo].[PivotProfile]
					AS
					SELECT * FROM (
						SELECT [Profile].ID, Tag.TagName
						FROM [Profile]
						INNER JOIN Tag on [Profile].ID = Tag.ObjectID
						Group by [Profile].ID, Tag.TagName
					)t
					PIVOT (COUNT(TagName) FOR TagName
					IN ([Developer],[Graphic design],[HTML/CSS],[Tester],[Testing],[Web Programming])) AS pvt
