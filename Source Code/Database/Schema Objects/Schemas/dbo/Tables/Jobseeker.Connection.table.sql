CREATE TABLE [dbo].[Jobseeker.Connection]
(
	ID char(36) PRIMARY KEY NOT NULL, 
	FromUserID char(36) NOT NULL,
	ToUserID char(36) NOT NULL,
	[Status] int NOT NULL,
	ModifiedDate datetime NOT NULL,
	CONSTRAINT FK_Jobseeker_Connection1_User
	FOREIGN KEY (FromUserID)
	REFERENCES [User](ID),
	CONSTRAINT FK_Jobseeker_Connection2_User
	FOREIGN KEY (ToUserID)
	REFERENCES [User](ID)
)
