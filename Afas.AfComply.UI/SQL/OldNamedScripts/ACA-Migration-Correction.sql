USE [aca]
GO

/****** Object:  Table [dbo].[tax_year_1095c_correction]    Script Date: 3/30/2017 8:53:16 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[tax_year_1095c_correction](
	[tax_yearCorrectionId] [int] IDENTITY(1,1) NOT NULL,
	[tax_year] [int] NOT NULL,
	[employee_id] [int] NOT NULL,
	[employer_id] [int] NOT NULL,
	[approvedBy] [varchar](50) NOT NULL,
	[approvedOn] [datetime2](7) NOT NULL,
	[get1095C] [bit] NOT NULL,
	[ResourceId] [uniqueidentifier] NOT NULL,
	[Corrected] [bit] NOT NULL,
	[OriginalUniqueSubmissionId] [varchar](100) NULL,
	[CorrectedUniqueSubmissionId] [varchar](100) NULL,
	[CorrectedUniqueRecordId] [varchar](100) NULL,
	[Transmitted] [bit] NOT NULL,
	[EntityStatusId] [int] NOT NULL,
	[ModifiedBy] [nvarchar](50) NOT NULL,
	[ModifiedDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_tax_year_1095c_correction] PRIMARY KEY CLUSTERED 
(
	[tax_yearCorrectionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[tax_year_1095c_correction]  WITH CHECK ADD  CONSTRAINT [FK_tax_year_1095c_correction] FOREIGN KEY([tax_year])
REFERENCES [dbo].[tax_year] ([tax_year])
GO

ALTER TABLE [dbo].[tax_year_1095c_correction] CHECK CONSTRAINT [FK_tax_year_1095c_correction]
GO


CREATE PROCEDURE [dbo].[INSERT_taxYear1095cCorrection]
	@corrected bit,
	@employerId int,
	@employeeId int

AS
BEGIN TRY
INSERT INTO [dbo].[tax_year_1095c_correction] ([tax_year],
		[employee_id],
		[employer_id],
		[approvedBy],
		[approvedOn],
		[get1095C],
		[ResourceId],
		[Corrected],
		[OriginalUniqueSubmissionId],
		[CorrectedUniqueSubmissionId],
		[CorrectedUniqueRecordId],
		[Transmitted],
		[EntityStatusId],
		[ModifiedBy],
		[ModifiedDate])
SELECT [tax_year]
      ,[employee_id]
      ,[employer_id]
      ,[approvedBy]
      ,[approvedOn]
      ,[get1095C]
      ,[ResourceId]
      ,@corrected
	  ,null
	  ,null
	  ,null
	  ,0
	  ,1
	  ,'SYSTEM'
	  ,GETDATE()
  FROM [aca].[dbo].[tax_year_1095c_approval]
  WHERE employer_id = @employerId AND employee_id = @employeeId
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH