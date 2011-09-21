CREATE TABLE [Jobseeker.Skill]
(
	ID char(36) PRIMARY KEY NOT NULL, 
	UserID char(36) NOT NULL,
	SkillName nvarchar(50) NOT NULL,
	CONSTRAINT FK_Jobseeker.Skill
	FOREIGN KEY (UserID)
	REFERENCES [User](ID)
)
