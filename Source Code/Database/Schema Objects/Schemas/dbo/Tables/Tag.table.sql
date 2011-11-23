CREATE TABLE [dbo].[Tag] (
    [ID]			UNIQUEIDENTIFIER	NOT NULL,
	[TableName]		CHAR(128)			NULL,
    [ObjectID]		CHAR(128)			NULL,	
    [TagName]		NVARCHAR(256)		NOT NULL,
    [ParentId]		UNIQUEIDENTIFIER	NULL,
	[ParentName]	NVARCHAR(256)		NULL,
    [ModifiedDate]	DATETIME			NOT NULL,
	[IsUpToDate]	BIT					NULL,
);

