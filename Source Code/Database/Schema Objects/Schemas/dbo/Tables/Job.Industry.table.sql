CREATE TABLE [dbo].[Job.Industry]
(
	ID char(36) PRIMARY KEY NOT NULL,
	JobID char(36) NOT NULL, 
	IndustryID int NOT NULL,
	ModifiedDate datetime NOT NULL,
	CONSTRAINT FK_Job_Industry_Job
	FOREIGN KEY (JobID)
	REFERENCES [Job](ID),
	CONSTRAINT FK_Job_Industry_Industry
	FOREIGN KEY (IndustryID)
	REFERENCES [Industry](ID),
)
