CREATE TABLE [Jobseeker.Project]
(
	ID char(36) PRIMARY KEY NOT NULL, 
	UserID char(36) NOT NULL,
	ProjectName nvarchar(50) NOT NULL,
	Occupation nvarchar(50) NULL,
	ProjectURL nvarchar(50) NULL,
	[Description] nvarchar(100) NOT NULL,
	ModifiedDate datetime NOT NULL,
	CONSTRAINT FK_Jobseeker_Project_User
	FOREIGN KEY (UserID)
	REFERENCES [User](ID)
)

