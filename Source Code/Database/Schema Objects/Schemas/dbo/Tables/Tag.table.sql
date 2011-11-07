CREATE TABLE [dbo].[Tag] (
    [ID]           CHAR (36)     NOT NULL,
    [ObjectID]     CHAR (36)     NULL,
    [TagName]      NVARCHAR (50) NOT NULL,
    [ParentID]     CHAR (36)     NULL,
    [ModifiedDate] DATETIME      NOT NULL
);

