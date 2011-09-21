CREATE TABLE [dbo].[Job.Ability]
(
	ID char(36) PRIMARY KEY NOT NULL,
	JobID char(36) NOT NULL,
	AbilityName nvarchar(50) NOT NULL, 
	ModifiedDate datetime NOT NULL,
	CONSTRAINT FK_Job_Ability_Job
	FOREIGN KEY (JobID)
	REFERENCES [Job](ID)
)