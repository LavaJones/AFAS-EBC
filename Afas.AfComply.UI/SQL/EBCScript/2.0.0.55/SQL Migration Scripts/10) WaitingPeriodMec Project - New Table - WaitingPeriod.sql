USE [aca]
GO
/****** Object:  Table [dbo].[WaitingPeriod]    Script Date: 9/6/2017 8:17:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WaitingPeriod](
	[WaitingPeriodID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](100) NOT NULL,
	[ResourceID] [uniqueidentifier] ROWGUIDCOL  NOT NULL CONSTRAINT [DF_WaitingPeriod_ResourceID]  DEFAULT (newid()),
	[EntityStatusID] [int] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime2](7) NOT NULL,
	[ModifiedBy] [nvarchar](50) NOT NULL,
	[ModfiedDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_WaitingPeriod] PRIMARY KEY CLUSTERED 
(
	[WaitingPeriodID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
SET IDENTITY_INSERT [dbo].[WaitingPeriod] ON 

GO
INSERT [dbo].[WaitingPeriod] ([WaitingPeriodID], [Description], [ResourceID], [EntityStatusID], [CreatedBy], [CreatedDate], [ModifiedBy], [ModfiedDate]) VALUES (1, N'Date of Hire', N'f5dca681-43f0-42da-964f-09c10eb198fd', 1, N'System Update', CAST(N'2017-08-30 00:00:00.0000000' AS DateTime2), N'System Update', CAST(N'2017-08-30 00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[WaitingPeriod] ([WaitingPeriodID], [Description], [ResourceID], [EntityStatusID], [CreatedBy], [CreatedDate], [ModifiedBy], [ModfiedDate]) VALUES (2, N'1st of Month following Date of Hire', N'e5ac0da0-95db-4bde-8862-c5ef86b81182', 1, N'System Update 8/30/2017', CAST(N'2017-08-30 00:00:00.0000000' AS DateTime2), N'System Update', CAST(N'2017-08-30 00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[WaitingPeriod] ([WaitingPeriodID], [Description], [ResourceID], [EntityStatusID], [CreatedBy], [CreatedDate], [ModifiedBy], [ModfiedDate]) VALUES (3, N'1st of Month following 30 days', N'fb8a5809-a9eb-47df-9b35-09d5899e0cc5', 1, N'System Update', CAST(N'2017-08-30 00:00:00.0000000' AS DateTime2), N'System Update', CAST(N'2017-08-30 00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[WaitingPeriod] ([WaitingPeriodID], [Description], [ResourceID], [EntityStatusID], [CreatedBy], [CreatedDate], [ModifiedBy], [ModfiedDate]) VALUES (4, N'1st of Month following 60 days', N'fdd3dfc5-1a92-4299-8af7-e11ca7c0d094', 1, N'System Update', CAST(N'2017-08-30 00:00:00.0000000' AS DateTime2), N'System Update', CAST(N'2017-08-30 00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[WaitingPeriod] ([WaitingPeriodID], [Description], [ResourceID], [EntityStatusID], [CreatedBy], [CreatedDate], [ModifiedBy], [ModfiedDate]) VALUES (5, N'90 days from Date of Hire', N'6cd4a576-d4c6-4df8-847e-a39ec6a5126e', 1, N'System Update', CAST(N'2017-08-30 00:00:00.0000000' AS DateTime2), N'System Update', CAST(N'2017-08-30 00:00:00.0000000' AS DateTime2))
GO
INSERT [dbo].[WaitingPeriod] ([WaitingPeriodID], [Description], [ResourceID], [EntityStatusID], [CreatedBy], [CreatedDate], [ModifiedBy], [ModfiedDate]) VALUES (6, N'Other', N'e41a255a-d8fb-4b6b-a43d-49c771b1e717', 1, N'System Update', CAST(N'2017-09-06 00:00:00.0000000' AS DateTime2), N'System Update', CAST(N'2017-09-06 00:00:00.0000000' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[WaitingPeriod] OFF
GO
ALTER TABLE [dbo].[WaitingPeriod]  WITH CHECK ADD  CONSTRAINT [FK_WaitingPeriod_EntityStatus] FOREIGN KEY([EntityStatusID])
REFERENCES [dbo].[EntityStatus] ([EntityStatusId])
GO
ALTER TABLE [dbo].[WaitingPeriod] CHECK CONSTRAINT [FK_WaitingPeriod_EntityStatus]
GO
