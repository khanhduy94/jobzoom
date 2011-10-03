CREATE TABLE [dbo].[Job.Function]
(
	ID char(36) PRIMARY KEY NOT NULL,
	JobID char(36) NOT NULL, 
	FunctionID char(5) NOT NULL,
	ModifiedDate datetime NOT NULL,
	CONSTRAINT FK_Job_Function_Job
	FOREIGN KEY (JobID)
	REFERENCES [Job](ID),
	CONSTRAINT FK_Job_Function_Function
	FOREIGN KEY (FunctionID)
	REFERENCES [Function](ID),
)
