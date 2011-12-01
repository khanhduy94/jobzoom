CREATE TABLE [dbo].[Job.Approval] (
    [JobApprovalId]		UNIQUEIDENTIFIER	NOT NULL,
	[JobPostingId]		UNIQUEIDENTIFIER	NOT NULL,
    [ProfileID]			UNIQUEIDENTIFIER	NOT NULL,    
	[UserId]			NVARCHAR(128)		NULL,    
	[IsApplied]			BIT					NULL,
    [IsApproved]		BIT					NULL,
);
