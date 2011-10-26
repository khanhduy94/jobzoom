CREATE TABLE [dbo].[Job.Applicant]
(
	ID char(36) PRIMARY KEY NOT NULL,
	JobID char(36) NOT NULL,
	UserID char(36) NOT NULL,
	Result nvarchar(50) NULL,
	ModifiedDate datetime NOT NULL,
	CONSTRAINT FK_Job_Applicant_Job
	FOREIGN KEY (JobID)
	REFERENCES [Job](ID),
	CONSTRAINT FK_Job_Applicant_User
	FOREIGN KEY (UserID)
	REFERENCES [User](ID)
)