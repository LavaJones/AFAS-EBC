use aca;

GO
ALTER TABLE aca.dbo.employee_classification
ADD WaitingPeriodID int  NULL;

GO

ALTER TABLE [dbo].[employee_classification]  WITH NOCHECK ADD  CONSTRAINT [FK_employee_classification_waitingPeriod] FOREIGN KEY([waitingPeriodID])
REFERENCES [dbo].[waitingPeriod] ([WaitingPeriodID])
GO

ALTER TABLE [dbo].[employee_classification] CHECK CONSTRAINT [FK_employee_classification_waitingPeriod]
GO

GO
ALTER TABLE aca.dbo.employee_classification
ADD CreatedBy nvarchar(50);

GO

ALTER TABLE aca.dbo.employee_classification
ADD CreatedDate datetime2(7);

GO
ALTER TABLE aca.dbo.employee_classification
ADD EntityStatusID int;

GO
ALTER TABLE aca.dbo.employee_classification
ADD Ooc varchar(2)

GO
ALTER TABLE [dbo].[employee_classification]  WITH CHECK ADD  CONSTRAINT [FK_employee_classification_EntityStatus] FOREIGN KEY([EntityStatusID])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO

ALTER TABLE [dbo].[employee_classification] CHECK CONSTRAINT [FK_employee_classification_EntityStatus]
GO

/*Rename the modified columns to match up with the AF format*/
use aca;
EXEC sp_rename 'aca.dbo.employee_classification.modOn', 'ModifiedDate', 'COLUMN';  
GO 

use aca;
EXEC sp_rename 'aca.dbo.employee_classification.modBy', 'ModifiedBy', 'COLUMN';  
GO 



/* SET all insurance plans to ACTIVE with the new Entity Status ID column. */
UPDATE aca.dbo.employee_classification
	SET
		CreatedBy=ModifiedBy,
		CreatedDate=ModifiedDate,
		EntityStatusID=1;

ALTER TABLE aca.dbo.employee_classification ALTER COLUMN [CreatedBy] nvarchar(50) NOT NULL
ALTER TABLE aca.dbo.employee_classification ALTER COLUMN [CreatedDate] datetime2(7) NOT NULL
ALTER TABLE aca.dbo.employee_classification ALTER COLUMN [EntityStatusID] int NOT NULL
