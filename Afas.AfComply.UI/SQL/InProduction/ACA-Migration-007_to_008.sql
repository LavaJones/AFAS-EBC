USE [aca]
GO

ALTER TABLE [dbo].[employee] ALTER COLUMN [dob] [datetime] NULL
GO

SET IDENTITY_INSERT [dbo].[insurance_carrier] ON 
GO
INSERT [dbo].[insurance_carrier] ([carrier_id], [name], [import_approved], [hra_flex]) VALUES (13, N'AFcomply', 0, 0)
SET IDENTITY_INSERT [dbo].[insurance_carrier] OFF
GO

SET IDENTITY_INSERT [dbo].[insurance_carrier_import_template] ON 
GO
INSERT [dbo].[insurance_carrier_import_template] ([template_id], [carrier_id], [columns], [employee_dependent_link], [fname], [mname], [lname], [ssn], [dob], [all12], [jan], [feb], [march], [april], [may], [june], [july], [august], [september], [october], [november], [december], [subscriber], [trueFormat], [nameFormat], [all12trueFormat], [subscriberFormat], [address], [city], [state], [zip]) VALUES (1, 5, 34, 3, 19, 19, 18, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 3, N'1', N'lcfm', N'Y', N'ssn', 4, 5, 6, 7)
GO
INSERT [dbo].[insurance_carrier_import_template] ([template_id], [carrier_id], [columns], [employee_dependent_link], [fname], [mname], [lname], [ssn], [dob], [all12], [jan], [feb], [march], [april], [may], [june], [july], [august], [september], [october], [november], [december], [subscriber], [trueFormat], [nameFormat], [all12trueFormat], [subscriberFormat], [address], [city], [state], [zip]) VALUES (2, 3, 28, 5, 13, 14, 12, 15, 16, 0, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 5, N'Y', N'seperated', NULL, N'ssn', 6, 9, 10, 11)
GO
INSERT [dbo].[insurance_carrier_import_template] ([template_id], [carrier_id], [columns], [employee_dependent_link], [fname], [mname], [lname], [ssn], [dob], [all12], [jan], [feb], [march], [april], [may], [june], [july], [august], [september], [october], [november], [december], [subscriber], [trueFormat], [nameFormat], [all12trueFormat], [subscriberFormat], [address], [city], [state], [zip]) VALUES (3, 2, 62, 10, 45, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 10, N'x', N'seperated', N'X', N'ssn', 12, 14, 16, 17)
GO
INSERT [dbo].[insurance_carrier_import_template] ([template_id], [carrier_id], [columns], [employee_dependent_link], [fname], [mname], [lname], [ssn], [dob], [all12], [jan], [feb], [march], [april], [may], [june], [july], [august], [september], [october], [november], [december], [subscriber], [trueFormat], [nameFormat], [all12trueFormat], [subscriberFormat], [address], [city], [state], [zip]) VALUES (4, 6, 20, 4, 3, 0, 2, 4, 0, 0, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 4, N'X', N'seperated', NULL, N'ssn', 5, 6, 7, 8)
GO
INSERT [dbo].[insurance_carrier_import_template] ([template_id], [carrier_id], [columns], [employee_dependent_link], [fname], [mname], [lname], [ssn], [dob], [all12], [jan], [feb], [march], [april], [may], [june], [july], [august], [september], [october], [november], [december], [subscriber], [trueFormat], [nameFormat], [all12trueFormat], [subscriberFormat], [address], [city], [state], [zip]) VALUES (5, 7, 26, 3, 5, 0, 4, 3, 6, 0, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 3, N'X', N'seperated', NULL, N'ssn', 8, 10, 11, 12)
GO
INSERT [dbo].[insurance_carrier_import_template] ([template_id], [carrier_id], [columns], [employee_dependent_link], [fname], [mname], [lname], [ssn], [dob], [all12], [jan], [feb], [march], [april], [may], [june], [july], [august], [september], [october], [november], [december], [subscriber], [trueFormat], [nameFormat], [all12trueFormat], [subscriberFormat], [address], [city], [state], [zip]) VALUES (8, 4, 26, 10, 5, 5, 4, 7, 9, 0, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 6, N'Y', N'lcfm', NULL, N'primary', 10, 12, 13, 14)
GO
INSERT [dbo].[insurance_carrier_import_template] ([template_id], [carrier_id], [columns], [employee_dependent_link], [fname], [mname], [lname], [ssn], [dob], [all12], [jan], [feb], [march], [april], [may], [june], [july], [august], [september], [october], [november], [december], [subscriber], [trueFormat], [nameFormat], [all12trueFormat], [subscriberFormat], [address], [city], [state], [zip]) VALUES (9, 1, 25, 6, 10, 0, 9, 8, 11, 25, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 7, N'1', N'seperated', N'12', N'00', 0, 0, 0, 0)
GO
INSERT [dbo].[insurance_carrier_import_template] ([template_id], [carrier_id], [columns], [employee_dependent_link], [fname], [mname], [lname], [ssn], [dob], [all12], [jan], [feb], [march], [april], [may], [june], [july], [august], [september], [october], [november], [december], [subscriber], [trueFormat], [nameFormat], [all12trueFormat], [subscriberFormat], [address], [city], [state], [zip]) VALUES (10, 8, 29, 9, 10, 0, 11, 9, 0, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 9, N'X', N'seperated', N'X', N'ssn', 12, 14, 15, 16)
GO
INSERT [dbo].[insurance_carrier_import_template] ([template_id], [carrier_id], [columns], [employee_dependent_link], [fname], [mname], [lname], [ssn], [dob], [all12], [jan], [feb], [march], [april], [may], [june], [july], [august], [september], [october], [november], [december], [subscriber], [trueFormat], [nameFormat], [all12trueFormat], [subscriberFormat], [address], [city], [state], [zip]) VALUES (11, 9, 16, 3, 2, 2, 1, 3, 4, 0, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 3, N'1', N'lcfm', NULL, N'ssn', 0, 0, 0, 0)
GO
INSERT [dbo].[insurance_carrier_import_template] ([template_id], [carrier_id], [columns], [employee_dependent_link], [fname], [mname], [lname], [ssn], [dob], [all12], [jan], [feb], [march], [april], [may], [june], [july], [august], [september], [october], [november], [december], [subscriber], [trueFormat], [nameFormat], [all12trueFormat], [subscriberFormat], [address], [city], [state], [zip]) VALUES (12, 10, 34, 3, 18, 19, 20, 15, 21, 34, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 3, N'1', N'seperated', N'12', N'ssn', 8, 9, 10, 11)
GO
INSERT [dbo].[insurance_carrier_import_template] ([template_id], [carrier_id], [columns], [employee_dependent_link], [fname], [mname], [lname], [ssn], [dob], [all12], [jan], [feb], [march], [april], [may], [june], [july], [august], [september], [october], [november], [december], [subscriber], [trueFormat], [nameFormat], [all12trueFormat], [subscriberFormat], [address], [city], [state], [zip]) VALUES (13, 13, 19, 3, 5, 0, 4, 1, 6, 19, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 3, N'1', N'seperated', NULL, N'ssn', 0, 0, 0, 0)
GO
INSERT [dbo].[insurance_carrier_import_template] ([template_id], [carrier_id], [columns], [employee_dependent_link], [fname], [mname], [lname], [ssn], [dob], [all12], [jan], [feb], [march], [april], [may], [june], [july], [august], [september], [october], [november], [december], [subscriber], [trueFormat], [nameFormat], [all12trueFormat], [subscriberFormat], [address], [city], [state], [zip]) VALUES (1013, 1010, 26, 10, 5, 5, 4, 8, 9, 0, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 6, N'Y', N'lcfm', NULL, N'primary', 10, 12, 13, 14)
GO
INSERT [dbo].[insurance_carrier_import_template] ([template_id], [carrier_id], [columns], [employee_dependent_link], [fname], [mname], [lname], [ssn], [dob], [all12], [jan], [feb], [march], [april], [may], [june], [july], [august], [september], [october], [november], [december], [subscriber], [trueFormat], [nameFormat], [all12trueFormat], [subscriberFormat], [address], [city], [state], [zip]) VALUES (1014, 1011, 26, 10, 5, 5, 4, 8, 9, 0, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 6, N'Y', N'lcfm', NULL, N'primary', 10, 12, 13, 14)
GO
SET IDENTITY_INSERT [dbo].[insurance_carrier_import_template] OFF
GO
