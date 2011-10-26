ALTER TABLE [dbo].[Profile.Work]
	ADD CONSTRAINT [FK_ProfileWorkUser] 
	FOREIGN KEY (UserId)
	REFERENCES [User] (UserId)	

