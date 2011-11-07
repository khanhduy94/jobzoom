ALTER TABLE [dbo].[TagDetail]
    ADD CONSTRAINT [DF_TagDetail_ModifiedDate] DEFAULT (getdate()) FOR [ModifiedDate];

