CREATE TABLE [dbo].[SimilarityTerm](
	[ID] [char](36) PRIMARY KEY NOT NULL,
	[Keyword1] [nvarchar](128) NOT NULL,
	[Keyword2] [nvarchar](128) NOT NULL,
	[Rate] [float] NOT NULL
)