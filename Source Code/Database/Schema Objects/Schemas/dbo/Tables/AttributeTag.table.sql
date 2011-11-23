CREATE TABLE [dbo].[AttributeTag]
(
	AttributeTagId		UNIQUEIDENTIFIER	NOT NULL,
	AttributeTagName	NVARCHAR(256)		NOT NULL,
	TableReference		VARCHAR(128)		NULL,
	ObjectId			UNIQUEIDENTIFIER	NULL,
	ParentId			UNIQUEIDENTIFIER	NULL,
	ParentName			NVARCHAR(256)		NULL,
	ModifiedDate		DATETIME			NULL,
	[Status]			VARCHAR(128)		NULL
)