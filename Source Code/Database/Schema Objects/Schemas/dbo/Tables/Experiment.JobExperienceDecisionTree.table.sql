CREATE TABLE [dbo].[Experiment.JobExperienceDecisionTree]
(
	ID					int NOT NULL PRIMARY KEY, 
	Job					int	NOT NULL,
	DotNetProgramming	bit NOT NULL,
	WebProgramming		bit NOT NULL,
	AspNET				bit NOT NULL,
	EmloyerApproved		bit NOT NULL
)
