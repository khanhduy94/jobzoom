CREATE TABLE [dbo].[Jobseeker.Language]
(
	ID char(36) PRIMARY KEY NOT NULL, 
	UserID char(36) NOT NULL,
	LanguageID int NOT NULL,
	Proficiency nvarchar(50) NOT NULL,
	ModifiedDate datetime NOT NULL,
	CONSTRAINT FK_Jobseeker_Language_User
	FOREIGN KEY (UserID)
	REFERENCES [User](ID),
	CONSTRAINT FK_Jobseeker_Language_Language
	FOREIGN KEY (LanguageID)
	REFERENCES [Language](ID)
)
