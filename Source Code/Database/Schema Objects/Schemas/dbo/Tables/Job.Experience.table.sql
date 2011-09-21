CREATE TABLE [dbo].[Job.Experience]
(
	ID char(36) PRIMARY KEY NOT NULL,
	JobID char(36) NOT NULL,
	JobTitle nvarchar(50) NOT NULL,
	YearOfExperience int NULL,
	ModifiedDate datetime NOT NULL,
	CONSTRAINT FK_Job_Experience_Job
	FOREIGN KEY (JobID)
	REFERENCES [Job](ID)
)