USE [aca-demo]
GO
SET IDENTITY_INSERT [dbo].[payroll_vendor] ON 
GO
INSERT [dbo].[payroll_vendor] ([vendor_id], [name], [autoUpload]) VALUES (1, N'Ties', 1)
GO
INSERT [dbo].[payroll_vendor] ([vendor_id], [name], [autoUpload]) VALUES (2, N'Skyward', 0)
GO
INSERT [dbo].[payroll_vendor] ([vendor_id], [name], [autoUpload]) VALUES (3, N'Smart HR', 0)
GO
INSERT [dbo].[payroll_vendor] ([vendor_id], [name], [autoUpload]) VALUES (4, N'ADP', 0)
GO
INSERT [dbo].[payroll_vendor] ([vendor_id], [name], [autoUpload]) VALUES (5, N'Software Unlimited', 1)
GO
INSERT [dbo].[payroll_vendor] ([vendor_id], [name], [autoUpload]) VALUES (6, N'Unknown', 0)
GO
SET IDENTITY_INSERT [dbo].[payroll_vendor] OFF
GO
