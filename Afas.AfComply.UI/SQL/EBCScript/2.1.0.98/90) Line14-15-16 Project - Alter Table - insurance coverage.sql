ALTER TABLE aca.dbo.insurance_coverage
ADD CreatedBy varchar(50) NULL;
GO

ALTER TABLE aca.dbo.insurance_coverage
ADD CreatedDate datetime NULL;
GO

ALTER TABLE aca.dbo.insurance_coverage
ADD ModifiedBy varchar(50) NULL;
GO

ALTER TABLE aca.dbo.insurance_coverage
ADD ModifiedDate datetime NULL;
GO

ALTER TABLE aca.dbo.insurance_coverage
ADD EntityStatusID int NULL;
GO

ALTER TABLE aca.dbo.insurance_coverage
ADD CONSTRAINT FK_icesid FOREIGN KEY (EntityStatusID)
    REFERENCES aca.dbo.EntityStatus(EntityStatusID);
GO

UPDATE aca.dbo.insurance_coverage
SET CreatedBy='System Update',
	CreatedDate=GETDATE(),
	ModifiedBy='System Update',
	ModifiedDate=GETDATE(),
	EntityStatusID=1;

GO



ALTER TABLE aca.dbo.insurance_coverage
ALTER COLUMN CreatedBy varchar(50) NOT NULL;
GO

ALTER TABLE aca.dbo.insurance_coverage
ALTER COLUMN CreatedDate datetime NOT NULL;
GO

ALTER TABLE aca.dbo.insurance_coverage
ALTER COLUMN ModifiedBy varchar(50) NOT NULL;
GO

ALTER TABLE aca.dbo.insurance_coverage
ALTER COLUMN ModifiedDate datetime NOT NULL;
GO

ALTER TABLE aca.dbo.insurance_coverage
ALTER COLUMN EntityStatusID int NOT NULL;
GO