CREATE TABLE [dbo].[Profile.Basic] (
    [ProfileBasicId]    CHAR(36) NOT NULL,
    [UserId]            NVARCHAR (128)   NOT NULL,
    [FirstName]         NVARCHAR (50)    NOT NULL,
    [LastName]          NVARCHAR (50)    NOT NULL,
    [Gender]            NVARCHAR (10)    NOT NULL,
    [Birthdate]         DATETIME         NOT NULL,
    [MaritalStatus]     NVARCHAR (50)    NOT NULL,
    [ProfilePictureUrl] NVARCHAR (255)   NULL,
    [AddressLine1]      NVARCHAR (255)   NULL,
    [AddressLine2]      NVARCHAR (255)   NULL,
    [Country]           NVARCHAR (128)   NULL,
    [City]              NVARCHAR (128)   NULL,
    [ZipCode]           INT              NULL,
    [Phone]             NVARCHAR (50)    NULL,
    [MobilePhone]       NVARCHAR (50)    NULL,
    [Website]           NVARCHAR (50)    NULL,
    [Facebook]          NVARCHAR (128)   NULL,
    [Twitter]           NVARCHAR (128)   NULL
);

