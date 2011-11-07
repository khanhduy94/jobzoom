﻿CREATE TABLE [dbo].[Company] (
    [CompanyId]    UNIQUEIDENTIFIER NOT NULL,
    [Name]         NVARCHAR (128)   NOT NULL,
    [Industry]     NVARCHAR (256)   NULL,
    [CompanySize]  NVARCHAR (128)   NULL,
    [AddressLine1] NVARCHAR (255)   NULL,
    [AddressLine2] NVARCHAR (255)   NULL,
    [Country]      NVARCHAR (64)    NULL,
    [City]         NVARCHAR (64)    NULL,
    [State]        NVARCHAR (64)    NULL,
    [Phone]        NVARCHAR (64)    NULL,
    [MobilePhone]  NVARCHAR (64)    NULL,
    [Fax]          NVARCHAR (64)    NULL,
    [Website]      NVARCHAR (255)   NULL,
    [Faceboook]    NVARCHAR (255)   NULL,
    [Twitter]      NVARCHAR (255)   NULL,
    [Linkedin]     NVARCHAR (255)   NULL,
    [Logo]         NVARCHAR (255)   NULL,
    [Email]        NVARCHAR (255)   NULL
);

