ALTER TABLE [dbo].[Profile.Work]
    ADD CONSTRAINT [FK_ProfileWork_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

