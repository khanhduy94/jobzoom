ALTER TABLE [dbo].[Profile.Work]
    ADD CONSTRAINT [FK_ProfileWorkUser] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

