ALTER TABLE [dbo].[Job.EducationExpRequirement]
	ADD CONSTRAINT [FK_JobEducationExpRequirement_JobPosting] 
	FOREIGN KEY (JobPostingId)
	REFERENCES [Job.Posting] (JobPostingId)	

