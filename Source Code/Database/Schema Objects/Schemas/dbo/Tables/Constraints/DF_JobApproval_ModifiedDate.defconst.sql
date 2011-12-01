ALTER TABLE [dbo].[JobApproval]
    ADD CONSTRAINT [DF_JobApproval_ModifiedDate] DEFAULT (getdate()) FOR [ModifiedDate];

