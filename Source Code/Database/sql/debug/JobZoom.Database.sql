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
PRINT N'Starting rebuilding table [dbo].[Job.Approval]...';


GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

SET XACT_ABORT ON;

BEGIN TRANSACTION;

CREATE TABLE [dbo].[tmp_ms_xx_Job.Approval] (
    [JobApprovalId] UNIQUEIDENTIFIER NOT NULL,
    [JobPostingId]  UNIQUEIDENTIFIER NOT NULL,
    [ProfileID]     UNIQUEIDENTIFIER NOT NULL,
    [UserId]        NVARCHAR (128)   NULL,
    [IsApplied]     BIT              NULL,
    [IsApproved]    BIT              NULL
);

ALTER TABLE [dbo].[tmp_ms_xx_Job.Approval]
    ADD CONSTRAINT [tmp_ms_xx_clusteredindex_PK_JobApproval] PRIMARY KEY CLUSTERED ([JobApprovalId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

IF EXISTS (SELECT TOP 1 1
           FROM   [dbo].[Job.Approval])
    BEGIN
        INSERT INTO [dbo].[tmp_ms_xx_Job.Approval] ([JobApprovalId], [JobPostingId], [ProfileID], [IsApplied], [IsApproved])
        SELECT   [JobApprovalId],
                 [JobPostingId],
                 [ProfileID],
                 [IsApplied],
                 [IsApproved]
        FROM     [dbo].[Job.Approval]
        ORDER BY [JobApprovalId] ASC;
    END

DROP TABLE [dbo].[Job.Approval];

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_Job.Approval]', N'Job.Approval';

EXECUTE sp_rename N'[dbo].[tmp_ms_xx_clusteredindex_PK_JobApproval]', N'PK_JobApproval', N'OBJECT';

COMMIT TRANSACTION;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;


GO
PRINT N'Altering [dbo].[GetPivotJobRequirement]...';


GO
ALTER PROCEDURE [dbo].[GetPivotJobRequirement]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @listCol VARCHAR(2000)
	DECLARE @SQLString NVARCHAR(1000)
	SELECT  @listCol = STUFF(( SELECT DISTINCT '],[' + ltrim(TagName)
							FROM Tag
							INNER JOIN Job on Job.ID = Tag.ObjectID
							ORDER BY '],[' + ltrim(TagName)
							FOR XML PATH('')), 1, 2, '') + ']'
	--DROP VIEW PivotJobRequirement
	SET @SQLString = N'ALTER VIEW PivotJobRequirement
						AS
						SELECT * FROM (
							SELECT Job.ID, Job.JobTitle, JobApproval.IsApply, Tag.TagName 
							FROM Job
							INNER JOIN Tag on Job.ID = Tag.ObjectID
							INNER JOIN JobApproval ON JobApproval.JobID = Job.ID
							Group by Job.ID,  Job.JobTitle, Tag.TagName, JobApproval.IsApply
						)t
						PIVOT (COUNT(TagName) FOR TagName
						IN ('+@listCol+')) AS pvt'						
	EXECUTE (@SQLString)

END
GO
PRINT N'Altering [dbo].[GetPivotProfile]...';


GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[GetPivotProfile]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @listCol VARCHAR(2000)
	DECLARE @SQLString NVARCHAR(1000)
	SELECT  @listCol = STUFF(( SELECT DISTINCT '],[' + ltrim(TagName)
							FROM Tag
							INNER JOIN [Profile] on [Profile].ID = Tag.ObjectID
							ORDER BY '],[' + ltrim(TagName)
							FOR XML PATH('')), 1, 2, '') + ']'
	 
	SET @SQLString = N'ALTER VIEW PivotProfile
					AS
					SELECT * FROM (
						SELECT [Profile].ID, JobApproval.IsApproved, Tag.TagName
						FROM [Profile]
						INNER JOIN Tag on [Profile].ID = Tag.ObjectID
						INNER JOIN JobApproval ON JobApproval.ProfileID = [Profile].ID
						Group by [Profile].ID, JobApproval.IsApproved, Tag.TagName
					)t
					PIVOT (COUNT(TagName) FOR TagName
					IN ('+@listCol+')) AS pvt'
					
						
	EXECUTE (@SQLString)
END
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
