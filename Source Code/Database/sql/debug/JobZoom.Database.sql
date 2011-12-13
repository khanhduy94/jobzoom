/*
Deployment script for JobZoom
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "JobZoom"
:setvar DefaultDataPath "C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\"
:setvar DefaultLogPath "C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSQLSERVER\MSSQL\DATA\"

GO
:on error exit
GO
USE [master]
GO
IF (DB_ID(N'$(DatabaseName)') IS NOT NULL
    AND DATABASEPROPERTYEX(N'$(DatabaseName)','Status') <> N'ONLINE')
BEGIN
    RAISERROR(N'The state of the target database, %s, is not set to ONLINE. To deploy to this database, its state must be set to ONLINE.', 16, 127,N'$(DatabaseName)') WITH NOWAIT
    RETURN
END

GO

IF NOT EXISTS (SELECT 1 FROM [master].[dbo].[sysdatabases] WHERE [name] = N'$(DatabaseName)')
BEGIN
    RAISERROR(N'You cannot deploy this update script to target CONGPHUCLE-MSFT. The database for which this script was built, JobZoom, does not exist on this server.', 16, 127) WITH NOWAIT
    RETURN
END

GO

IF (@@servername != 'CONGPHUCLE-MSFT')
BEGIN
    RAISERROR(N'The server name in the build script %s does not match the name of the target server %s. Verify whether your database project settings are correct and whether your build script is up to date.', 16, 127,N'CONGPHUCLE-MSFT',@@servername) WITH NOWAIT
    RETURN
END

GO

IF CAST(DATABASEPROPERTY(N'$(DatabaseName)','IsReadOnly') as bit) = 1
BEGIN
    RAISERROR(N'You cannot deploy this update script because the database for which it was built, %s , is set to READ_ONLY.', 16, 127, N'$(DatabaseName)') WITH NOWAIT
    RETURN
END

GO
USE [$(DatabaseName)]
GO
/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

GO
PRINT N'Dropping FK_TagAttribute_TagAttribute...';


GO
ALTER TABLE [dbo].[TagAttribute] DROP CONSTRAINT [FK_TagAttribute_TagAttribute];


GO
PRINT N'Starting rebuilding table [dbo].[TagAttribute]...';


GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

BEGIN TRANSACTION;

CREATE TABLE [dbo].[tmp_ms_xx_TagAttribute] (
    [TagId]          UNIQUEIDENTIFIER NOT NULL,
    [TagName]        NVARCHAR (256)   NOT NULL,
    [TagValue]       NVARCHAR (256)   NULL,
    [Weight]         SMALLINT         NULL,
    [Level]          SMALLINT         NULL,
    [Required]       BIT              NULL,
    [TableReference] VARCHAR (128)    NULL,
    [ObjectId]       UNIQUEIDENTIFIER NULL,
    [ParentId]       UNIQUEIDENTIFIER NULL,
    [ParentName]     NVARCHAR (256)   NULL,
    [Attachment]     NVARCHAR (200)   NULL,
    [ModifiedDate]   DATETIME         NULL,
    [Status]         VARCHAR (128)    NULL
);

ALTER TABLE [dbo].[tmp_ms_xx_TagAttribute]
    ADD CONSTRAINT [tmp_ms_xx_clusteredindex_PK_TagAttribute] PRIMARY KEY CLUSTERED ([TagId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

IF EXISTS (SELECT TOP 1 1
           FROM   [dbo].[TagAttribute])
    BEGIN
        INSERT INTO [dbo].[tmp_ms_xx_TagAttribute] ([TagId], [TagName], [Weight], [Level], [Required], [TableReference], [ObjectId], [ParentId], [ParentName], [Attachment], [ModifiedDate], [Status])
        SELECT   [TagId],
                 [TagName],
                 [Weight],
                 [Level],
                 [Required],
                 [TableReference],
                 [ObjectId],
                 [ParentId],
                 [ParentName],
                 [Attachment],
                 [ModifiedDate],
                 [Status]
        FROM     [dbo].[TagAttribute]
        ORDER BY [TagId] ASC;
    END

DROP TABLE [dbo].[TagAttribute];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_TagAttribute]', N'TagAttribute';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_clusteredindex_PK_TagAttribute]', N'PK_TagAttribute', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Creating FK_TagAttribute_TagAttribute...';


GO
ALTER TABLE [dbo].[TagAttribute] WITH NOCHECK
    ADD CONSTRAINT [FK_TagAttribute_TagAttribute] FOREIGN KEY ([ParentId]) REFERENCES [dbo].[TagAttribute] ([TagId]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

GO
PRINT N'Checking existing data against newly created constraints';


GO
USE [$(DatabaseName)];


GO
ALTER TABLE [dbo].[TagAttribute] WITH CHECK CHECK CONSTRAINT [FK_TagAttribute_TagAttribute];


GO
