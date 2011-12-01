CREATE TABLE [dbo].[Profile.Work] (
    [ProfileWorkId] UNIQUEIDENTIFIER NOT NULL,
    [UserId]        NVARCHAR (128)   NOT NULL,
    [CompanyId]     UNIQUEIDENTIFIER NULL,
    [CompanyName]   NVARCHAR (128)   NOT NULL,
    [JobTitle]      NVARCHAR (128)   NOT NULL,
    [StartDate]     DATE             NULL,
    [EndDate]       DATE             NULL,
    [CurrentlyWork] BIT              NULL,
    [Description]   NVARCHAR (512)   NULL
);

