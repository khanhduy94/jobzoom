EXEC sp_addlinkedserver

@server='TRUNGHIEU-PC',    -- local SQL name given to the linked server

@srvproduct='',         -- not used (any value will do)

@provider='MSOLAP',     -- Analysis Services OLE DB provider

@datasrc='localhost',   -- Analysis Server name (machine name)

@catalog='MovieClick'   -- default catalog/database




SELECT *
INTO DecisionTreeNode FROM
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
FROM [Pivot Job Requirement].CONTENT')


SELECT *
INTO DecisionTreeNodeDistribution FROM
OPENQUERY(LINKED_AS,
'SELECT FLATTENED
[NODE_UNIQUE_NAME] AS [NODEID],
[NODE_DISTRIBUTION]
FROM [Pivot Job Requirement].CONTENT')

CREATE PROCEDURE [GenTree]
AS
  BEGIN
    SET NOCOUNT ON;
    WITH TREE
         AS (SELECT CAST(1 AS INT) AS LEVEL,
                    ID,
                    NAME,
                    CAST(RIGHT(' '
                                 + CONVERT(VARCHAR(11),ID),11) AS VARCHAR(120)) AS HIERARCHY
             FROM   Node
             WHERE  PARENTID IS NULL
             UNION ALL
             SELECT LEVEL
                      + 1,
                    B.ID,
                    B.NAME,
                    CAST(A.HIERARCHY
                           + '/'
                           + RIGHT(' '
                                     + CONVERT(VARCHAR(11),B.ID),11) AS VARCHAR(120))
             FROM   TREE A
                    JOIN Node B
                      ON A.ID = B.PARENTID),
         SEQTREE
         AS (SELECT LEVEL,
                    Row_number()
                      OVER(ORDER BY HIERARCHY) AS SEQ,
                    ID,
                    NAME,
                    HIERARCHY
             FROM   TREE)
    SELECT '<Root>'
             + REPLACE(REPLACE((SELECT   '`tree id="'
                                           + CONVERT(VARCHAR(11),A.ID)
                                           + '" PageName="'
                                           + A.NAME
                                           + '" ~'
                                           + CASE
                                               WHEN A.LEVEL < Isnull(B.LEVEL,1) THEN ''
                                               ELSE Replicate('`/tree~',1
                                                                          + A.LEVEL
                                                                          - Isnull(B.LEVEL,1))
                                             END
                                FROM     SEQTREE A
                                         LEFT JOIN SEQTREE B
                                           ON A.SEQ
                                                + 1 = B.SEQ
                                ORDER BY A.SEQ
                                for xml path('')
                                                               
                               ),'`','<'),'~','>')
             + '</Root>' AS MYXMLEXPORT
  END


  exec [GenTree]