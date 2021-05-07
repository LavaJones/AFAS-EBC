USE [aca]
GO

ALTER TABLE [dbo].[employer] ADD [fee_id] [int] NOT NULL DEFAULT ((1))
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
ALTER TABLE [dbo].[fee] ADD [cap] [money] NOT NULL
GO
ALTER TABLE [dbo].[fee] ADD [billing_cycle_length_months] [int] NOT NULL
GO
ALTER TABLE [dbo].[fee] ADD [current_discount_percentage] [int] NOT NULL
GO

SET ANSI_PADDING OFF
GO

SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[fee] ON 
INSERT [dbo].[fee] ([id], [su_fee], [base_fee], [employee_fee], [cap], [history], [modOn], [modBy], [billing_cycle_length_months], [current_discount_percentage]) VALUES (1, 1000.0000, 100.0000, 0.2000, 12000.0000, N'Initial Settings', CAST(N'2015-01-26 00:00:00.000' AS DateTime), N'SYSTEM', 12, 0)
SET IDENTITY_INSERT [dbo].[fee] OFF
GO

ALTER TABLE [dbo].[employer]  WITH NOCHECK ADD  CONSTRAINT [FK_employer_fee] FOREIGN KEY([fee_id])
REFERENCES [dbo].[fee] ([id])
GO

ALTER TABLE [dbo].[employer] CHECK CONSTRAINT [FK_employer_fee]
GO
