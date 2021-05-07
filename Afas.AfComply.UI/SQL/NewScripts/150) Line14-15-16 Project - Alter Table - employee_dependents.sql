ALTER TABLE aca.dbo.employee_dependents
ADD CreatedBy varchar(50) NULL;
GO

ALTER TABLE aca.dbo.employee_dependents
ADD CreatedDate datetime NULL;
GO

ALTER TABLE aca.dbo.employee_dependents
ADD ModifiedBy varchar(50) NULL;
GO

ALTER TABLE aca.dbo.employee_dependents
ADD ModifiedDate datetime NULL;
GO

ALTER TABLE aca.dbo.employee_dependents
ADD EntityStatusID int NULL;
GO

ALTER TABLE aca.dbo.employee_dependents
ADD CONSTRAINT FK_edesid FOREIGN KEY (EntityStatusID)
    REFERENCES aca.dbo.EntityStatus(EntityStatusID);
GO

UPDATE aca.dbo.employee_dependents
SET CreatedBy='System Update',
	CreatedDate=GETDATE(),
	ModifiedBy='System Update',
	ModifiedDate=GETDATE(),
	EntityStatusID=1;

GO



ALTER TABLE aca.dbo.employee_dependents
ALTER COLUMN CreatedBy varchar(50) NOT NULL;
GO

ALTER TABLE aca.dbo.employee_dependents
ALTER COLUMN CreatedDate datetime NOT NULL;
GO

ALTER TABLE aca.dbo.employee_dependents
ALTER COLUMN ModifiedBy varchar(50) NOT NULL;
GO

ALTER TABLE aca.dbo.employee_dependents
ALTER COLUMN ModifiedDate datetime NOT NULL;
GO

ALTER TABLE aca.dbo.employee_dependents
ALTER COLUMN EntityStatusID int NOT NULL;
GO