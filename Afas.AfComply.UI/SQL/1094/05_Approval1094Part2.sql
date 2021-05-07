USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Approved1094FinalPart2](
	[Approved1094FinalPart2Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Approved1094FinalPart1Id] [int] NOT NULL,
	[Total1095Forms] int NOT NULL,
	[IsAggregatedAleGroup] bit NOT NULL,
	[ResourceId] [uniqueidentifier] NOT NULL,
	[EntityStatusId] [int] NOT NULL,
	[CreatedBy] [nvarchar](50) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](50) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Approved1094FinalPart2] PRIMARY KEY CLUSTERED 
(
	[Approved1094FinalPart2Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


