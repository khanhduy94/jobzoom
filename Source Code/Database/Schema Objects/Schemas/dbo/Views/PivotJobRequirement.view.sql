CREATE VIEW [dbo].[PivotJobRequirement]
						AS
						SELECT * FROM (
							SELECT Job.ID, Job.JobTitle, JobApproval.IsApply, Tag.TagName 
							FROM Job
							INNER JOIN Tag on Job.ID = Tag.ObjectID
							INNER JOIN JobApproval ON JobApproval.JobID = Job.ID
							Group by Job.ID,  Job.JobTitle, Tag.TagName, JobApproval.IsApply
						)t
						PIVOT (COUNT(TagName) FOR TagName
						IN ([.NET Framework],[C/C++],[Computer Software],[DBMS],[English],[Full time],[Java],[Software development])) AS pvt
