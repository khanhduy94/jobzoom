CREATE TABLE [dbo].[Job.Approval] (
    [JobApprovalId]		UNIQUEIDENTIFIER	NOT NULL,
	[JobPostingId]		UNIQUEIDENTIFIER	NOT NULL,
    [ProfileID]			UNIQUEIDENTIFIER	NOT NULL,        
    [IsApproved]		BIT					NOT NULL,
);
