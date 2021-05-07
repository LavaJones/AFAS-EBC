use aca;

GO
ALTER TABLE aca.dbo.affordability_safe_harbor
ADD CreatedBy nvarchar(50);

GO
ALTER TABLE aca.dbo.affordability_safe_harbor
ADD CreatedDate datetime2(7);

GO
ALTER TABLE aca.dbo.affordability_safe_harbor
ADD ModifiedBy nvarchar(50);

GO
ALTER TABLE aca.dbo.affordability_safe_harbor
ADD ModifiedDate datetime2(7);

GO
ALTER TABLE aca.dbo.affordability_safe_harbor
ADD EntityStatusID int;

GO
ALTER TABLE aca.dbo.affordability_safe_harbor
ADD ResourceID uniqueidentifier;

GO
ALTER TABLE aca.dbo.affordability_safe_harbor  WITH CHECK ADD  CONSTRAINT [FK_employee_ash_EntityStatus] FOREIGN KEY([EntityStatusID])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO

ALTER TABLE aca.dbo.affordability_safe_harbor CHECK CONSTRAINT [FK_employee_ash_EntityStatus]
GO


ALTER TABLE aca.dbo.affordability_safe_harbor ADD  DEFAULT (newid()) FOR [ResourceId]
GO

--Populate the new columns as they will be set to NOT NULL below. 
UPDATE aca.dbo.affordability_safe_harbor
SET
	CreatedBy='System Update',
	CreatedDate=GETDATE(),
	ModifiedBy='System Update',
	ModifiedDate=GETDATE(),
	EntityStatusID=1,
	ResourceID=NEWID()


--Set the NOT NULL property of each column. 
ALTER TABLE aca.dbo.affordability_safe_harbor ALTER COLUMN [CreatedBy] nvarchar(50) NOT NULL
ALTER TABLE aca.dbo.affordability_safe_harbor ALTER COLUMN [CreatedDate] datetime2(7) NOT NULL
ALTER TABLE aca.dbo.affordability_safe_harbor ALTER COLUMN [ModifiedBy] nvarchar(50) NOT NULL
ALTER TABLE aca.dbo.affordability_safe_harbor ALTER COLUMN [ModifiedDate] datetime2(7) NOT NULL
ALTER TABLE aca.dbo.affordability_safe_harbor ALTER COLUMN [EntityStatusID] INTEGER NOT NULL
ALTER TABLE aca.dbo.affordability_safe_harbor ALTER COLUMN [ResourceID] uniqueidentifier NOT NULL

