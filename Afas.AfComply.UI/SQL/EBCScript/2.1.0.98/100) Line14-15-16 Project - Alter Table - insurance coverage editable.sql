ALTER TABLE aca.dbo.insurance_coverage_editable
ADD CreatedBy varchar(50) NULL;
GO

ALTER TABLE aca.dbo.insurance_coverage_editable
ADD CreatedDate datetime NULL;
GO

ALTER TABLE aca.dbo.insurance_coverage_editable
ADD ModifiedBy varchar(50) NULL;
GO

ALTER TABLE aca.dbo.insurance_coverage_editable
ADD ModifiedDate datetime NULL;
GO

ALTER TABLE aca.dbo.insurance_coverage_editable
ADD EntityStatusID int NULL;
GO

ALTER TABLE aca.dbo.insurance_coverage_editable
ADD CONSTRAINT FK_iceesid FOREIGN KEY (EntityStatusID)
    REFERENCES aca.dbo.EntityStatus(EntityStatusID);
GO

UPDATE aca.dbo.insurance_coverage_editable
SET CreatedBy='System Update',
	CreatedDate=GETDATE(),
	ModifiedBy='System Update',
	ModifiedDate=GETDATE(),
	EntityStatusID=1;

GO

ALTER TABLE aca.dbo.insurance_coverage_editable
ALTER COLUMN CreatedBy varchar(50) NOT NULL;
GO

ALTER TABLE aca.dbo.insurance_coverage_editable
ALTER COLUMN CreatedDate datetime NOT NULL;
GO

ALTER TABLE aca.dbo.insurance_coverage_editable
ALTER COLUMN ModifiedBy varchar(50) NOT NULL;
GO

ALTER TABLE aca.dbo.insurance_coverage_editable
ALTER COLUMN ModifiedDate datetime NOT NULL;
GO

ALTER TABLE aca.dbo.insurance_coverage_editable
ALTER COLUMN EntityStatusID int NOT NULL;
GO