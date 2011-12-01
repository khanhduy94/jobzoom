ALTER TABLE [dbo].[Tag]
    ADD CONSTRAINT [DF_Tag_ModifiedDate] DEFAULT (getdate()) FOR [ModifiedDate];

