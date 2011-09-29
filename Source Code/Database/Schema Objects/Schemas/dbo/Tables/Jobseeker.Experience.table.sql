CREATE TABLE [Jobseeker.Experience]
(
	ID char(36) PRIMARY KEY NOT NULL, 
	UserID char(36) NOT NULL,
	Name nvarchar(50) NOT NULL,
	Title nvarchar(50) NULL,
	Industry nvarchar(50) NULL,
	Location nvarchar(50) NULL,
	StartDate datetime NULL,
	EndDate datetime NULL,
	[Description] nvarchar(100) NULL,
	ExperienceType int NOT NULL,
	ModifiedDate datetime NOT NULL,
	CONSTRAINT FK_Jobseeker_Exprience_User
	FOREIGN KEY (UserID)
	REFERENCES [User](ID)
)
