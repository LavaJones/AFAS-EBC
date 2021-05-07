USE [air]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER TABLE [br].[status_code] ALTER COLUMN [status_code] [nchar](25)

ALTER TABLE [br].[status_code] DROP CONSTRAINT [CHK_status_code]
GO

ALTER TABLE [br].[status_code] DROP CONSTRAINT [CHK_status_code_id]
GO

ALTER TABLE [br].[status_code]  WITH CHECK ADD  CONSTRAINT [CHK_status_code] CHECK  (([status_code]='Rejected' OR [status_code]='Processing' OR [status_code]='Accepted w/Errors' OR [status_code]='Accepted'))
GO

ALTER TABLE [br].[status_code] CHECK CONSTRAINT [CHK_status_code]
GO

ALTER TABLE [br].[status_code]  WITH CHECK ADD  CONSTRAINT [CHK_status_code_id] CHECK  (([status_code_id]=(4) OR [status_code_id]=(3) OR [status_code_id]=(2) OR [status_code_id]=(1)))
GO

ALTER TABLE [br].[status_code] CHECK CONSTRAINT [CHK_status_code_id]
GO

INSERT INTO [br].[status_code] ([status_code_id], [status_code]) VALUES (3, 'Accepted w/Errors')
GO

INSERT INTO [br].[status_code] ([status_code_id], [status_code]) VALUES (4, 'Processing')
GO

