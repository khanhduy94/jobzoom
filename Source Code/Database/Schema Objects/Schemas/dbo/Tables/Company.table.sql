CREATE TABLE [dbo].[Company]
(
	CompanyId		uniqueidentifier	NOT NULL, 
	Name			nvarchar(128)		NOT NULL,
	Industry		nvarchar(256)		NULL,
	CompanySize		nvarchar(128)		NULL,
	AddressLine1	nvarchar(255)		NULL,
	AddressLine2	nvarchar(255)		NULL,
	Country			nvarchar(64)		NULL,
	City			nvarchar(64)		NULL,
	[State]			nvarchar(64)		NULL,
	Phone			nvarchar(64)		NULL,
	MobilePhone		nvarchar(64)		NULL,
	Fax				nvarchar(64)		NULL,
	Website			nvarchar(255)		NULL,
	Faceboook		nvarchar(255)		NULL,
	Twitter			nvarchar(255)		NULL,
	Linkedin		nvarchar(255)		NULL,
	Logo			nvarchar(255)		NULL,
	Email			nvarchar(255)		NULL
)
