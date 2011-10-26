CREATE TABLE [Jobseeker.ProjectMember]
(
	ID char(36) PRIMARY KEY NOT NULL, 
	ProjectID char(36) NOT NULL,
	MemberID char(36) NOT NULL,
	ModifiedDate datetime NOT NULL,
	CONSTRAINT FK_Jobseeker_ProjectMember_Project
	FOREIGN KEY (ProjectID)
	REFERENCES [Jobseeker.Project](ID),
	CONSTRAINT FK_Jobseeker_ProjectMember_User
	FOREIGN KEY (MemberID)
	REFERENCES [User](ID)
)
