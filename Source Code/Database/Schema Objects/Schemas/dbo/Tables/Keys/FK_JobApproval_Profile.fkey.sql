ALTER TABLE [dbo].[JobApproval]
    ADD CONSTRAINT [FK_JobApproval_Profile] FOREIGN KEY ([ProfileID]) REFERENCES [dbo].[Profile] ([ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;

