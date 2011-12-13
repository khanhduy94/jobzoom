CREATE TABLE [dbo].[TagAttribute]
(
	[TagId]				UNIQUEIDENTIFIER	NOT NULL,
	[TagName]			NVARCHAR(256)		NOT NULL,
	[TagValue]			NVARCHAR(256)		NULL,
	[Weight]			SMALLINT			NULL,
	[Level]				SMALLINT			NULL,
	[Required]			BIT					NULL,
	[TableReference]	VARCHAR(128)		NULL,
	[ObjectId]			UNIQUEIDENTIFIER	NULL,
	[ParentId]			UNIQUEIDENTIFIER	NULL,
	[ParentName]		NVARCHAR(256)		NULL,
	[Attachment]		NVARCHAR(200)		NULL,
	[ModifiedDate]		DATETIME			NULL,
	[Status]			VARCHAR(128)		NULL
)
