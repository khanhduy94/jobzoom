ALTER TABLE [dbo].[Profile.Basic]
    ADD CONSTRAINT [FK_ProfileBasic_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION;