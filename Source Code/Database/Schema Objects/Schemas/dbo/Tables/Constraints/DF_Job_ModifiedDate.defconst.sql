ALTER TABLE [dbo].[Job]
    ADD CONSTRAINT [DF_Job_ModifiedDate] DEFAULT (getdate()) FOR [ModifiedDate];

