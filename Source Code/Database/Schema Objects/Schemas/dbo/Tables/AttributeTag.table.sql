CREATE TABLE [dbo].[AttributeTag]
(
	AttributeTagId		VARCHAR(36)	NOT NULL,
	AttributeTagName	NVARCHAR(256)		NOT NULL,
	[Weight]			SMALLINT			NULL,
	[Level]				SMALLINT			NULL,
	[Required]			BIT					NULL,
	TableReference		VARCHAR(128)		NULL,
	ObjectId			VARCHAR(36)			NULL,
	ParentId			VARCHAR(36)			NULL,
	ParentName			NVARCHAR(256)		NULL,
	Attachment			NVARCHAR(200)		NULL,
	ModifiedDate		DATETIME			NULL,
	[Status]			VARCHAR(128)		NULL
)