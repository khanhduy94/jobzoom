CREATE TABLE [dbo].[DecisionTreeNodeDistribution] (
    [NODEID]          VARCHAR (100)  NOT NULL,
    [ATTRIBUTE_NAME]  NVARCHAR (256) NULL,
    [ATTRIBUTE_VALUE] NVARCHAR (7)   NULL,
    [SUPPORT]         FLOAT          NULL,
    [PROBABILITY]     FLOAT          NULL,
    [VARIANCE]        FLOAT          NULL,
    [VALUETYPE]       INT            NULL,
    FOREIGN KEY ([NODEID]) REFERENCES [dbo].[DecisionTreeNode] ([NODEID]) ON DELETE NO ACTION ON UPDATE NO ACTION
);

