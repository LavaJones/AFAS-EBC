USE [aca]
GO

ALTER TABLE [dbo].[TransmissionLinking] DROP CONSTRAINT [DF_TransmissionLinking_ResourceId]
GO

/****** Object:  Table [dbo].[TransmissionEx]    Script Date: 4/3/2018 1:30:26 PM ******/
DROP TABLE [dbo].[TransmissionLinking]
GO

/****** Object:  Table [dbo].[TransmissionEx]    Script Date: 4/3/2018 1:30:26 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[TransmissionLinking](
       [TransmissionId] [int] IDENTITY(1,1) NOT NULL,
       [Approved1095FinalId] [int] NOT NULL,
       [EmployerId] [int] NOT NULL,
       [EmployeeId] [int] NOT NULL,
       [RecordId][int] NOT NULL,
       [TaxYear] [int] NOT NULL,
       [TaxYearEmployerTransmissionId] [int] NOT NULL,
       [TaxYearEmployeeTransmissionId] [int] NOT NULL,
       [EntityStatusId] [int] NOT NULL,
       [ResourceId] [uniqueidentifier] ROWGUIDCOL  NOT NULL,
       [CreatedBy] [varchar](50) NULL,
       [CreatedDate] [datetime2](7) NULL,
       [ModifiedBy] [varchar](50) NULL,
       [ModifiedDate] [datetime2](7) NULL,
CONSTRAINT [PK_TransmissionLinking] PRIMARY KEY CLUSTERED 
(
       [TransmissionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[TransmissionLinking] ADD  CONSTRAINT [DF_TransmissionLinking_ResourceId]  DEFAULT (newid()) FOR [ResourceId]
GO
