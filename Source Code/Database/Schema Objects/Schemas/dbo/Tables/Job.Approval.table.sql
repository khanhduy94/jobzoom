CREATE TABLE [dbo].[Job.Approval] (
    [JobApprovalId]		CHAR(36)	NOT NULL,
	[JobPostingId]		CHAR(36)	NOT NULL,
    [ProfileID]			CHAR(36)	NOT NULL,    
	[UserId]			NVARCHAR(128)		NULL,    
	[IsApplied]			BIT					NULL,
    [IsApproved]		BIT					NULL,
);
