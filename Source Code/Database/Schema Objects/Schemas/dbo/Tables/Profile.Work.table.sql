CREATE TABLE [dbo].[Profile.Work]
(
	ProfileWorkId		uniqueidentifier	NOT NULL,
	UserId				nvarchar(128)		NOT NULL,	
	CompanyId			uniqueidentifier	NULL,
	CompanyName			nvarchar(128)		NOT NULL,
	JobTitle			nvarchar(128)		NOT NULL,
	StartDate			date				NULL,
	EndDate				date				NULL,
	CurrentlyWork		bit					NULL,
	[Description]		nvarchar(512)		NULL,
)