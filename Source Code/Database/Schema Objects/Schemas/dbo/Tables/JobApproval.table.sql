CREATE TABLE [dbo].[JobApproval] (
    [ID]           CHAR (36) NOT NULL,
    [ProfileID]    CHAR (36) NOT NULL,
    [JobID]        CHAR (36) NOT NULL,
    [IsApply]      BIT       NULL,
    [IsApproved]   BIT       NULL,
    [ModifiedDate] DATETIME  NOT NULL
);

