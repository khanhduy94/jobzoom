ALTER TABLE [dbo].[Job.WorkExpRequirement]
	ADD CONSTRAINT [FK_JobWorkExRequirement_JobPosting] 
	FOREIGN KEY (JobPostingId)
	REFERENCES [Job.Posting] (JobPostingId)	

