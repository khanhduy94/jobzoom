CREATE TABLE [dbo].[Job] (
    [ID]           CHAR (36)     NOT NULL,
    [UserID]       CHAR (36)     NOT NULL,
    [JobTitle]     NVARCHAR (50) NOT NULL,
    [CompanyName]  NVARCHAR (50) NULL,
    [ModifiedDate] DATETIME      NOT NULL
);

