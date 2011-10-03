CREATE TABLE [dbo].[Country]
(
	ID int PRIMARY KEY NOT NULL IDENTITY(1,1), 
	Name nvarchar(50) NOT NULL,
	Flag image NULL
)
