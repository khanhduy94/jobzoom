ALTER TABLE [dbo].[TagAttribute]
	ADD CONSTRAINT [FK_TagAttribute_TagAttribute] 
	FOREIGN KEY (ParentId)
	REFERENCES TagAttribute (TagId)	

