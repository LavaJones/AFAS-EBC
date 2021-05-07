USE [aca]
GO
CREATE TYPE BULK_Tax_year_1095c_correction AS TABLE
(
	[tax_yearCorrectionId] [int] NOT NULL,
	[tax_year] [int] NOT NULL,
	[employee_id] [int] NOT NULL,
	[employer_id] [int] NOT NULL,
	[ResourceId] [uniqueidentifier] NOT NULL,
	[Corrected] [bit] NOT NULL,
	[OriginalUniqueSubmissionId] [varchar](100) NULL,
    [CorrectedUniqueSubmissionId] [varchar](100) NULL,
    [CorrectedUniqueRecordId] [varchar](100) NULL,
	[Transmitted] [bit] NOT NULL,
	[ModifiedBy] [nvarchar](50) NOT NULL,
	[ModifiedDate] [datetime2](7) NOT NULL
)
GO
GRANT SELECT ON [dbo].[BULK_Tax_year_1095c_correction] TO [aca-user] as [dbo]
GO