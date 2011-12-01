CREATE TABLE [dbo].[Tag] (
    [ID]			CHAR(36)	NOT NULL,
	[TableName]		CHAR(128)			NULL,
    [ObjectID]		CHAR(128)			NULL,	
    [TagName]		NVARCHAR(256)		NOT NULL,
    [ParentId]		CHAR(36)	NULL,
	[ParentName]	NVARCHAR(256)		NULL,
    [ModifiedDate]	DATETIME			NOT NULL,
	[IsUpToDate]	BIT					NULL,
);

