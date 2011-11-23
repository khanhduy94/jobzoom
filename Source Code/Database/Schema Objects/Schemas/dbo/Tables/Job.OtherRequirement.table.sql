CREATE TABLE [dbo].[Job.OtherRequirement]
(
	JobOtherRequirementId	UNIQUEIDENTIFIER	NOT NULL, 
	JobPostingId			UNIQUEIDENTIFIER	NOT NULL,
	JobAttributeName		NVARCHAR(256)		NOT NULL,
	JobAttributeValue		NVARCHAR(256)		NOT NULL,
)
