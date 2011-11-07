CREATE TABLE [dbo].[User] (
    [ID]            CHAR (36)     NOT NULL,
    [Email]         NVARCHAR (50) NOT NULL,
    [Password]      VARCHAR (50)  NOT NULL,
    [LastLoginDate] DATETIME      NULL,
    [ModifiedDate]  DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF)
);

