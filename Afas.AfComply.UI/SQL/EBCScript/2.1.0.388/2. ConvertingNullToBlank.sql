 USE aca
 GO
 ALTER TABLE dbo.UserEditPart2 DISABLE TRIGGER UserEditTrigger
 GO
 UPDATE dbo.UserEditPart2 SET NewValue = ISNULL(NewValue, '') WHERE NewValue is null
 GO
 ALTER TABLE dbo.UserEditPart2 ENABLE TRIGGER UserEditTrigger