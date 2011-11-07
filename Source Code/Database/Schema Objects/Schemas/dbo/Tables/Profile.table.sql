CREATE TABLE [dbo].[Profile] (
    [ID]             CHAR (36)      NOT NULL,
    [UserID]         CHAR (36)      NOT NULL,
    [FirstName]      NVARCHAR (50)  NOT NULL,
    [LastName]       NVARCHAR (50)  NOT NULL,
    [Gender]         NCHAR (1)      NOT NULL,
    [Birthdate]      DATETIME       NOT NULL,
    [MaritalStatus]  NCHAR (1)      NOT NULL,
    [Citizenship]    CHAR (36)      NOT NULL,
    [Picture]        IMAGE          NULL,
    [AddressLine1]   VARCHAR (50)   NOT NULL,
    [AddressLine2]   VARCHAR (50)   NULL,
    [CityID]         CHAR (36)      NOT NULL,
    [Phone]          NVARCHAR (50)  NULL,
    [Mobile]         NVARCHAR (50)  NULL,
    [AdditionalInfo] NVARCHAR (100) NULL,
    [Website]        NVARCHAR (50)  NULL,
    [ModifiedDate]   DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF)
);

