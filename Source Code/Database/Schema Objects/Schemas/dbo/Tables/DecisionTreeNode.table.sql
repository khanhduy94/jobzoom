CREATE TABLE [dbo].[DecisionTreeNode] (
    [NODEID]                    VARCHAR (100)  NOT NULL,
    [NODE_TYPE]                 INT            NULL,
    [NODE_CAPTION]              NVARCHAR (256) NULL,
    [CHILDREN_CARDINALITY]      INT            NULL,
    [PARENTID]                  VARCHAR (100)  NULL,
    [NODE_DESCRIPTION]          NTEXT          NULL,
    [NODE_RULE]                 NTEXT          NULL,
    [MARGINAL_RULE]             NTEXT          NULL,
    [NODE_PROBABILITY]          FLOAT          NULL,
    [MARGINAL_PROBABILITY]      FLOAT          NULL,
    [NODE_SUPPORT]              FLOAT          NULL,
    [MSOLAP_MODEL_COLUMN]       NVARCHAR (256) NULL,
    [MSOLAP_NODE_SCORE]         FLOAT          NULL,
    [MSOLAP_NODE_SHORT_CAPTION] NVARCHAR (256) NULL,
    [ATTRIBUTE_NAME]            NVARCHAR (256) NULL,
    PRIMARY KEY CLUSTERED ([NODEID] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF)
);

