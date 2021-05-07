use aca;

GO
ALTER TABLE aca.dbo.insurance
ADD Mec bit;

GO
ALTER TABLE aca.dbo.insurance
ADD CreatedBy nvarchar(50);

GO
use aca;
ALTER TABLE aca.dbo.insurance
ADD CreatedDate datetime2(7);

GO
ALTER TABLE aca.dbo.insurance
ADD EntityStatusID int;

GO
ALTER TABLE aca.dbo.insurance
ADD fullyPlusSelfInsured bit;

GO
ALTER TABLE [dbo].[insurance]  WITH CHECK ADD  CONSTRAINT [FK_insurance_EntityStatus] FOREIGN KEY([EntityStatusID])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO

ALTER TABLE [dbo].[insurance] CHECK CONSTRAINT [FK_insurance_EntityStatus]
GO

/*Rename the modified columns to match up with the AF format*/
use aca;
EXEC sp_rename 'aca.dbo.insurance.modOn', 'ModifiedDate', 'COLUMN';  
GO 

use aca;
EXEC sp_rename 'aca.dbo.insurance.modBy', 'ModifiedBy', 'COLUMN';  
GO 


/* SET all insurance plans to ACTIVE with the new Entity Status ID column. */
UPDATE insurance
	SET
		EntityStatusID=1,
		CreatedBy=ModifiedBy,
		CreatedDate=ModifiedDate,
		Mec=1;

ALTER TABLE aca.dbo.insurance ALTER COLUMN [minValue] bit NOT NULL;
ALTER TABLE aca.dbo.insurance ALTER COLUMN [offSpouse] bit NOT NULL;
ALTER TABLE aca.dbo.insurance ALTER COLUMN [offDependent] bit NOT NULL;
ALTER TABLE aca.dbo.insurance ALTER COLUMN [ModifiedDate] datetime NOT NULL;
ALTER TABLE aca.dbo.insurance ALTER COLUMN [ModifiedBy] varchar(50) NOT NULL;
ALTER TABLE aca.dbo.insurance ALTER COLUMN [insurance_type_id] int NOT NULL;
--ALTER TABLE aca.dbo.insurance ALTER COLUMN [SpouseConditional] bit NOT NULL;
ALTER TABLE aca.dbo.insurance ALTER COLUMN [Mec] bit NOT NULL;
ALTER TABLE aca.dbo.insurance ALTER COLUMN [CreatedBy] nvarchar(50) NOT NULL;
ALTER TABLE aca.dbo.insurance ALTER COLUMN [CreatedDate] datetime2(7) NOT NULL;
ALTER TABLE aca.dbo.insurance ALTER COLUMN [EntityStatusID] int NOT NULL;
ALTER TABLE aca.dbo.insurance ALTER COLUMN [fullyPlusSelfInsured] int NULL;