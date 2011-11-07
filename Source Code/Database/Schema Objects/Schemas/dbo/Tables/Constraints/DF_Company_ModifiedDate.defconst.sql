ALTER TABLE [dbo].[Company]
    ADD CONSTRAINT [DF_Company_ModifiedDate] DEFAULT (getdate()) FOR [ModifiedDate];

