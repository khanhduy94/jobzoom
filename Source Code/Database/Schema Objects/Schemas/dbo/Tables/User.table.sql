CREATE TABLE [dbo].[User]
(
	ID char(36) PRIMARY KEY NOT NULL, 
	Email nvarchar(50) NOT NULL,
	[Password] varchar(50) NOT NULL,
	AccountTypeID int NOT NULL,
	LastLoginDate datetime NULL,
	ModifiedDate datetime NOT NULL,
	CONSTRAINT FK_User_AccountType 
	FOREIGN KEY (AccountTypeID)
	REFERENCES [User.AccountType](ID)
)
