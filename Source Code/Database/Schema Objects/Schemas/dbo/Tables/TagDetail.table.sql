CREATE TABLE [dbo].[TagDetail] (
    [ID]           CHAR (36)      NOT NULL,
    [TagID]        CHAR (36)      NOT NULL,
    [Location]     NVARCHAR (50)  NULL,
    [StartDate]    DATE           NULL,
    [EndDate]      DATE           NULL,
    [Description]  NVARCHAR (100) NULL,
    [Attachment]   NVARCHAR (255) NULL,
    [ModifiedDate] DATETIME       NOT NULL
);

