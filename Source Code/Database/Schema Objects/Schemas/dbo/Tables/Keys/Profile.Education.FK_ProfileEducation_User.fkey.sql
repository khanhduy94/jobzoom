ALTER TABLE [dbo].[Profile.Education]
    ADD CONSTRAINT [FK_ProfileEducation_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

