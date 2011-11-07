ALTER TABLE [dbo].[Profile.Education]
    ADD CONSTRAINT [FK_ProfileEducationUser] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

