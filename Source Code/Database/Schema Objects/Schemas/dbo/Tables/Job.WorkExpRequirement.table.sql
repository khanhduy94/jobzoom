CREATE TABLE [dbo].[Job.WorkExpRequirement]
(
	JobWorkExpRequirementId		UNIQUEIDENTIFIER	NOT NULL, 
	JobPostingId				UNIQUEIDENTIFIER	NOT NULL,
	JobAttributeName			NVARCHAR(256)		NOT NULL,
	JobAttributeValue			NVARCHAR(256)		NOT NULL,
);
