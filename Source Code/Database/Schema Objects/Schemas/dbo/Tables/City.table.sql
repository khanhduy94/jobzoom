CREATE TABLE [dbo].[City]
(
	ID char(36) PRIMARY KEY NOT NULL, 
	Name nvarchar(50) NOT NULL,
	CountryID char(36) NOT NULL,
	CONSTRAINT FK_City_Country
	FOREIGN KEY (CountryID) 
	REFERENCES Country(ID)
)
