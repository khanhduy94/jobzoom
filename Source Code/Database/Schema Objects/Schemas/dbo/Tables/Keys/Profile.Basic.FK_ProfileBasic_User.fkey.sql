ALTER TABLE [dbo].[Profile.Basic]
	ADD CONSTRAINT [FK_ProfileBasic_User] 
	FOREIGN KEY (UserId)
	REFERENCES [User] (UserId)	

