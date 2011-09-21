CREATE TABLE [dbo].[User]
(
	ID char(36) PRIMARY KEY NOT NULL, 
	Email nvarchar(50) NOT NULL,
	PasswordHash char(64) NOT NULL,
	PasswordSalt int NOT NULL,
	AccountTypeID char(36) NOT NULL,
	LastLoginDate datetime NULL,
	ModifiedDate datetime NOT NULL,
	CONSTRAINT FK_User_AccountType 
	FOREIGN KEY (AccountTypeID)
	REFERENCES [User.AccountType](ID)
)
