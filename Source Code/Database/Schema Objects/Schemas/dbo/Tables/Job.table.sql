CREATE TABLE [dbo].[Job]
(
	ID char(36) PRIMARY KEY NOT NULL,
	UserID char(36) NOT NULL, 
	JobTitle nvarchar(50) NOT NULL,
	CompanyName nvarchar(50) NOT NULL,
	CityID int NOT NULL,
	CountryID int NOT NULL,
	[Type] nvarchar(50) NOT NULL, 
	Experience nvarchar(50) NOT NULL,
	Compensation nvarchar(50) NULL,
	ModifiedDate datetime NOT NULL,
	CONSTRAINT FK_Job_User
	FOREIGN KEY (UserID)
	REFERENCES [User](ID),
)
