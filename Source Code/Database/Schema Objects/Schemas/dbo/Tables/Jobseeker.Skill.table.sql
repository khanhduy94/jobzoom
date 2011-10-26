CREATE TABLE [Jobseeker.Skill]
(
	ID char(36) PRIMARY KEY NOT NULL, 
	UserID char(36) NOT NULL,
	SkillName nvarchar(50) NOT NULL,
	ModifiedDate datetime NOT NULL,
	CONSTRAINT FK_Jobseeker_Skill_User
	FOREIGN KEY (UserID)
	REFERENCES [User](ID)
)
