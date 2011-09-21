CREATE TABLE [dbo].[Company]
(
	ID char(36) PRIMARY KEY NOT NULL, 
	UserID char(36) NOT NULL,
	Name nvarchar(10) NOT NULL,
	IndustryID char(36) NOT NULL,
	CompanyType nvarchar(50) NULL,
	CompanySize nvarchar(50) NULL,
	AddressLine1 varchar(50) NOT NULL,
	AddressLine2 varchar(50) NULL,
	CityID char(36) NOT NULL,
	Phone nvarchar(50) NULL,
	Fax nvarchar(50) NULL,
	Founded datetime NULL,
	Website nvarchar(50) NULL,
	Logo image NULL,
	ModifiedDate datetime NOT NULL,
	CONSTRAINT FK_Company_User
	FOREIGN KEY (UserID)
	REFERENCES [User](ID),
	CONSTRAINT FK_Company_Industry
	FOREIGN KEY (IndustryID)
	REFERENCES [Industry](ID),
	CONSTRAINT FK_Company_Country
	FOREIGN KEY (CityID)
	REFERENCES Country(ID),
)

