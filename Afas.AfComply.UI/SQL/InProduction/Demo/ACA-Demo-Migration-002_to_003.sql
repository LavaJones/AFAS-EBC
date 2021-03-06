USE [aca-demo]
GO
SET IDENTITY_INSERT [dbo].[employer] ON 
GO
INSERT [dbo].[employer] ([employer_id], [name], [address], [city], [state_id], [zip], [img_logo], [bill_address], [bill_city], [bill_state], [bill_zip], [employer_type_id], [ein], [initial_measurement_id], [import_demo], [import_payroll], [iei], [iec], [ftpei], [ftpec], [ipi], [ipc], [ftppi], [ftppc], [importProcess], [vendor_id], [autoUpload], [autoBill], [suBilled], [import_gp], [import_hr], [import_ec], [import_io], [import_ic], [import_pay_mod]) VALUES (1, N'American Fidelity Administrative Services', N'9000 Cameron Parkway', N'Oklahoma City', 36, N'73114', N'../images/logos/EBC_logo.gif', N'9000 Cameron Parkway', N'Oklahoma City', 36, N'73114', 3, N'00-1234567', 9, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, 0, 0, NULL, NULL, NULL, NULL, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[employer] OFF
GO

SET IDENTITY_INSERT [dbo].[plan_year] ON 
GO
INSERT [dbo].[plan_year] ([plan_year_id], [employer_id], [description], [startDate], [endDate], [notes], [history], [modOn], [modBy]) VALUES (1, 1, N'Must Have A Value', CAST(N'2015-01-01 00:00:00.000' AS DateTime), CAST(N'2015-12-31 00:00:00.000' AS DateTime), N'', N'Plan created on: Jun  3 2016  3:25PM', CAST(N'2016-06-03 15:25:16.957' AS DateTime), N'Registration')
GO
SET IDENTITY_INSERT [dbo].[plan_year] OFF
GO

SET IDENTITY_INSERT [dbo].[user] ON 
GO
INSERT [dbo].[user] ([user_id], [fname], [lname], [email], [phone], [username], [password], [employer_id], [active], [poweruser], [last_mod_by], [last_mod], [reset_pwd], [billing], [irsContact], [floater]) VALUES (1, N'AFcomply', N'Adminstrator', N'wg-technical@af-group.com', N'405-523-5962', N'afcomply-admin', N'FP8uEaxKDE6/mx2f9GldgO4w6ALhOlFCyw==', 1, 1, 1, N'Registration', CAST(N'2016-06-03 15:25:16.957' AS DateTime), 0, 1, 1, 0)
GO
INSERT [dbo].[user] ([user_id], [fname], [lname], [email], [phone], [username], [password], [employer_id], [active], [poweruser], [last_mod_by], [last_mod], [reset_pwd], [billing], [irsContact], [floater]) VALUES (2, N'AFcomply', N'Developer', N'wg-technical@americanfidelity.com', N'405-523-5962', N'afcomply-developer', N'i2hIKAuW2lvA09lCswOKQEub4aeMYW10jQ==', 1, 1, 1, N'afcomply-admin', CAST(N'2016-06-03 15:28:57.000' AS DateTime), 0, 1, 1, 0)
GO
SET IDENTITY_INSERT [dbo].[user] OFF
GO
