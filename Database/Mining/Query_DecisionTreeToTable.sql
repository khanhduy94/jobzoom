
CREATE TABLE [dbo].[DecisionTreeNode](
	[NODEID] [varchar](100) PRIMARY KEY,
	[NODE_TYPE] [int] NULL,
	[NODE_CAPTION] [nvarchar](256) NULL,
	[CHILDREN_CARDINALITY] int NULL,
	[PARENTID] [varchar](100) NULL,
	[NODE_DESCRIPTION] [ntext] NULL,
	[NODE_RULE] [ntext] NULL,
	[MARGINAL_RULE] [ntext] NULL,
	[NODE_PROBABILITY] [float] NULL,
	[MARGINAL_PROBABILITY] [float] NULL,
	[NODE_SUPPORT] [float] NULL,
	[MSOLAP_MODEL_COLUMN] [nvarchar](256) NULL,
	[MSOLAP_NODE_SCORE] [float] NULL,
	[MSOLAP_NODE_SHORT_CAPTION] [nvarchar](256) NULL,
	[ATTRIBUTE_NAME] [nvarchar](256) NULL
)
GO
CREATE TABLE [dbo].[DecisionTreeNodeDistribution](
	[NODEID] [varchar](100) NOT NULL 
		REFERENCES [dbo].[DecisionTreeNode](NODEID),
	[ATTRIBUTE_NAME] [nvarchar](256) NULL,
	[ATTRIBUTE_VALUE] [nvarchar](7) NULL,
	[SUPPORT] [float] NULL,
	[PROBABILITY] [float] NULL,
	[VARIANCE] [float] NULL,
	[VALUETYPE] [int] NULL
)
GO

INSERT INTO DecisionTreeNode
SELECT * FROM 
OPENQUERY(LINKED_AS,
'SELECT FLATTENED
[NODE_UNIQUE_NAME] AS [NODEID],
[NODE_TYPE], 
[NODE_CAPTION], 
[CHILDREN_CARDINALITY], 
[PARENT_UNIQUE_NAME] AS [PARENTID],
[NODE_DESCRIPTION], 
[NODE_RULE], 
[MARGINAL_RULE], 
[NODE_PROBABILITY], 
[MARGINAL_PROBABILITY],
[NODE_SUPPORT], 
[MSOLAP_MODEL_COLUMN], 
[MSOLAP_NODE_SCORE], 
[MSOLAP_NODE_SHORT_CAPTION],
[ATTRIBUTE_NAME]
FROM [PivotDeveloper].CONTENT
WHERE [NODE_UNIQUE_NAME] <> ''0''')


INSERT INTO DecisionTreeNodeDistribution
SELECT * FROM 
OPENQUERY(LINKED_AS,
'SELECT FLATTENED
[NODE_UNIQUE_NAME] AS [NODEID],
[NODE_DISTRIBUTION]
FROM [Pivot Job Requirement].CONTENT')


SELECT NODEID, NODE_CAPTION
FROM dbo.DecisionTreeNode
ORDER BY Coalesce(PARENTID, NODEID)


SELECT 
	dbo.DecisionTreeNode.NODEID, 
	NODE_CAPTION, NODE_DESCRIPTION,
	NODE_SUPPORT,
	PROBABILITY, 
	dbo.DecisionTreeNodeDistribution.ATTRIBUTE_NAME, 
	ATTRIBUTE_VALUE	
FROM
	dbo.DecisionTreeNode inner join dbo.DecisionTreeNodeDistribution
	ON dbo.DecisionTreeNode.NODEID = dbo.DecisionTreeNodeDistribution.NODEID
WHERE
	NODE_DESCRIPTION like '%C#%'
	AND dbo.DecisionTreeNodeDistribution.ATTRIBUTE_NAME = 'Is Apply'
	AND ATTRIBUTE_VALUE = 'True'
	AND PROBABILITY > 0.5


EXEC sp_linkedservers

Select 1 Where Exists (Select [SRVID] From master..sysservers Where [srvName]='JobZoomMiningLinkedServer') 

select TABLE_NAME as Name from INFORMATION_SCHEMA.Tables where TABLE_TYPE = 'VIEW' AND [TABLE_NAME] like 'Pivot%'

SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.Columns where TABLE_NAME = 'PivotProfile' 

select TABLE_NAME as Name from INFORMATION_SCHEMA.Tables where TABLE_TYPE = 'VIEW' AND [TABLE_NAME] like 'Pivot%'

select 1 from INFORMATION_SCHEMA.Tables where TABLE_TYPE = 'BASE TABLE' AND [TABLE_NAME] = 'DecisionTreeNode'

select 1 from INFORMATION_SCHEMA.Tables where TABLE_TYPE = 'BASE TABLE' AND [TABLE_NAME] = 'DecisionTreeNodeDistribution'

select TABLE_NAME as Name from INFORMATION_SCHEMA.Views


SELECT FLATTENED
[NODE_UNIQUE_NAME] AS [NODEID],
[NODE_TYPE], 
[NODE_CAPTION], 
[CHILDREN_CARDINALITY], 
[PARENT_UNIQUE_NAME] AS [PARENTID],
[NODE_DESCRIPTION], 
[NODE_RULE], 
[MARGINAL_RULE], 
[NODE_PROBABILITY], 
[MARGINAL_PROBABILITY],
[NODE_SUPPORT], 
[MSOLAP_MODEL_COLUMN], 
[MSOLAP_NODE_SCORE], 
[MSOLAP_NODE_SHORT_CAPTION],
[ATTRIBUTE_NAME]
FROM [PivotDeveloper].CONTENT
WHERE [NODE_UNIQUE_NAME] <> '0'