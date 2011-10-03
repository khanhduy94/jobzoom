CREATE TABLE [dbo].[City]
(
	ID int PRIMARY KEY NOT NULL IDENTITY(1,1),
	Name nvarchar(50) NOT NULL,
	CountryID int NOT NULL,
	CONSTRAINT FK_City_Country
	FOREIGN KEY (CountryID) 
	REFERENCES Country(ID)
)
