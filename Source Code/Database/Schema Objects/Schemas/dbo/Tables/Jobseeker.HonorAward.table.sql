CREATE TABLE [dbo].[Jobseeker.HonorAward]
(
	ID char(36) PRIMARY KEY NOT NULL, 
	UserID char(36) NOT NULL,
	Title nvarchar(50) NOT NULL,
	Issuer nvarchar(50) NULL,
	Occupation nvarchar(50) NULL,
	IssueDate datetime NULL,
	[Description] nvarchar(100) NULL,
	ModifiedDate datetime NOT NULL,
	CONSTRAINT FK_Jobseeker_HonorAward_User
	FOREIGN KEY (UserID)
	REFERENCES [User](ID)
)
