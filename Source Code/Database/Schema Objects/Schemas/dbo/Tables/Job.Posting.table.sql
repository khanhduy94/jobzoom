CREATE TABLE [dbo].[Job.Posting]
(
	JobPostingId		CHAR(36)	NOT NULL, 
	UserId				NVARCHAR(128)		NOT NULL,
	CompanyId			CHAR(36)	NULL,
	CompanyName			NVARCHAR (128)		NULL,
	JobTitle			NVARCHAR(128)		NOT NULL
);
