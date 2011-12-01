CREATE TABLE [dbo].[Job.EducationExpRequirement]
(
	JobEducationExpRequirementId	CHAR(36)	NOT NULL, 
	JobPostingId					CHAR(36)	NOT NULL,
	JobAttributeName				NVARCHAR(256)		NOT NULL,
	JobAttributeValue				NVARCHAR(256)		NOT NULL,
);
