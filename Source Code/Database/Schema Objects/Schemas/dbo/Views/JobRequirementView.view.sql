CREATE VIEW [dbo].[JobRequirementView]
	AS
	SELECT W.JobWorkExpRequirementId Id, P.JobPostingId, P.UserId, P.CompanyId, P.CompanyName, P.JobTitle, W.JobAttributeName, W.JobAttributeValue
	FROM [Job.Posting] P
	INNER JOIN [Job.WorkExpRequirement] W
	ON P.JobPostingId = W.JobPostingId
	UNION
	SELECT E.JobEducationExpRequirementId Id,P.JobPostingId, P.UserId, P.CompanyId, P.CompanyName, P.JobTitle, E.JobAttributeName, E.JobAttributeValue
	FROM [Job.Posting] P
	INNER JOIN [Job.EducationExpRequirement] E
	ON P.JobPostingId = E.JobPostingId
	UNION
	SELECT O.JobOtherRequirementId Id ,P.JobPostingId, P.UserId, P.CompanyId, P.CompanyName, P.JobTitle, O.JobAttributeName, O.JobAttributeValue
	FROM [Job.Posting] P
	INNER JOIN [Job.OtherRequirement] O
	ON P.JobPostingId = O.JobPostingId