USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Approved1094FinalPart1](
	[Approved1094FinalId] [bigint] IDENTITY(1,1) NOT NULL,
	[EmployerId] [int] NOT NULL,
	[TaxYearId] [int] NOT NULL,
	[EmployerResourceId] [uniqueidentifier] NOT NULL,
	[Ein] [int] NOT NULL,
	[EmployerName] [nvarchar](max) NOT NULL,
	[EmployerDBAName] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NOT NULL,
	[City] [nvarchar](max) NOT NULL,
	[State] [nvarchar](max) NOT NULL,
	[Zip] [nvarchar](max) NOT NULL,
	[IrsContactName] [nvarchar](max) NOT NULL,
	[IrsContactPhoneNumber] [nvarchar](max) NOT NULL,
	[IsDge] [bit] NOT NULL,
	[DgeName] [nvarchar](max) NULL,
	[DgeAddress] [nvarchar](max) NULL,
	[DgeCity] [nvarchar](max) NULL,
	[DgeState] [nvarchar](max) NULL,
	[DgeZip] [nvarchar](max) NULL,
	[DgeContactName] [nvarchar](max) NULL,
	[DgeContactPhoneNumber] [nvarchar](max) NULL,
	[TransmissionTotal1095Forms] [int] NOT NULL,
	[IsAuthoritiveTransmission] [bit] NOT NULL,
	[ResourceId] [uniqueidentifier] NOT NULL,
	[EntityStatusId] [int] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](50) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Approved1094FinalPart1] PRIMARY KEY CLUSTERED 
(
	[Approved1094FinalId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


