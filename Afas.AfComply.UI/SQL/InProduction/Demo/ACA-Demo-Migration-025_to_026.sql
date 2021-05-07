USE [aca-demo]
GO
ALTER TABLE [dbo].[employee] ALTER COLUMN [state_id] integer NOT NULL
GO
ALTER TABLE [dbo].[employee]  WITH CHECK ADD  CONSTRAINT [fk_employee_stateID] FOREIGN KEY([state_id])
REFERENCES [dbo].[state] ([state_id])
GO
ALTER TABLE [dbo].[employee] CHECK CONSTRAINT [fk_employee_stateID]
GO
GRANT EXECUTE ON [dbo].[SELECT_EmployeeCount] TO [aca-user] AS [dbo]
GO