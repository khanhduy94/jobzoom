ALTER TABLE [dbo].[Job.OtherRequirement]
	ADD CONSTRAINT [FK_JobOtherRequirement_JobPosting] 
	FOREIGN KEY (JobPostingId)
	REFERENCES [Job.Posting] (JobPostingId)	

