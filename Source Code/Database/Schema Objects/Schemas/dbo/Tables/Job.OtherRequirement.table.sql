CREATE TABLE [dbo].[Job.OtherRequirement]
(
	JobOtherRequirementId	CHAR(36)	NOT NULL, 
	JobPostingId			CHAR(36)	NOT NULL,
	JobAttributeName		NVARCHAR(256)		NOT NULL,
	JobAttributeValue		NVARCHAR(256)		NOT NULL,
)
