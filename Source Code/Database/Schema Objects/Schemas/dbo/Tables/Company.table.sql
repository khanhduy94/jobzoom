CREATE TABLE [dbo].[Company] (
    [ID]           CHAR (36)     NOT NULL,
    [UserID]       CHAR (36)     NOT NULL,
    [Name]         NVARCHAR (10) NOT NULL,
    [IndustryID]   INT           NOT NULL,
    [CompanyType]  NVARCHAR (50) NULL,
    [CompanySize]  NVARCHAR (50) NULL,
    [AddressLine1] VARCHAR (50)  NOT NULL,
    [AddressLine2] VARCHAR (50)  NULL,
    [CityID]       INT           NOT NULL,
    [Phone]        NVARCHAR (50) NULL,
    [Fax]          NVARCHAR (50) NULL,
    [Founded]      DATETIME      NULL,
    [Website]      NVARCHAR (50) NULL,
    [Logo]         IMAGE         NULL,
    [ModifiedDate] DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF)
);

