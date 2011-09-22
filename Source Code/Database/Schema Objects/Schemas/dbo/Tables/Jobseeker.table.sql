CREATE TABLE [dbo].[Jobseeker]
(
	ID char(36) PRIMARY KEY NOT NULL, 
	UserID char(36) NOT NULL,
	Title nvarchar(10) NOT NULL,
	Suffix nvarchar(10) NULL,
	FirtName nvarchar(50) NOT NULL,
	LastName nvarchar(50) NOT NULL,
	MiddleName nvarchar(50) NULL,
	Gender nchar(1) NOT NULL,
	Birthdate datetime NOT NULL,
	MaritalStatus nchar(1) NOT NULL,
	Citizenship char(36) NOT NULL,
	Picture image NULL,
	AddressLine1 varchar(50) NOT NULL,
	AddressLine2 varchar(50) NULL,
	CityID char(36) NOT NULL,
	Phone nvarchar(50) NULL,
	Mobile nvarchar(50) NULL,
	AdditionalInfo nvarchar(100) NULL,
	Website nvarchar(50) NULL,
	ModifiedDate datetime NOT NULL,
	CONSTRAINT FK_Jobseeker_User
	FOREIGN KEY (UserID)
	REFERENCES [User](ID),
	CONSTRAINT FK_Jobseeker_Country
	FOREIGN KEY (Citizenship)
	REFERENCES Country(ID),
	CONSTRAINT FK_JobseekerCity_Country
	FOREIGN KEY (CityID)
	REFERENCES City(ID)
)
