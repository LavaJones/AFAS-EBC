USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Employee1094CountOverride]
(
	[Employee1094CountOverrideId] [int] IDENTITY(1,1) NOT NULL,
	[TaxYearId] [int] NOT NULL,
	[EmployerId] [int] NOT NULL,
	[TimeFrameId] [int] NOT NULL,
	[TotalEmployeeCnt] [int] NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Employee1094CountOverride] ADD CONSTRAINT FK_Employee1094CountOverride_TaxYear FOREIGN KEY (TaxYearId) REFERENCES [dbo].[tax_year] (tax_year)    
GO

ALTER TABLE [dbo].[Employee1094CountOverride] ADD CONSTRAINT FK_Employee1094CountOverride_Employer FOREIGN KEY (EmployerId) REFERENCES [dbo].[employer] (employer_id)    
GO

CREATE INDEX [IDX_Employee1094CountOverride_TaxYearId] ON [aca].[dbo].[Employee1094CountOverride] ([TaxYearId])
GO

CREATE INDEX [IDX_Employee1094CountOverride_EmployerId] ON [aca].[dbo].[Employee1094CountOverride] ([EmployerId])
GO

CREATE INDEX [IDX_Employee1094CountOverride_TimeFrameId] ON [aca].[dbo].[Employee1094CountOverride] ([TimeFrameId])
GO

CREATE INDEX [IDX_Employee1094CountOverride_EmployerId_TimeFrameId_TotalEmployeeCnt] ON [aca].[dbo].[Employee1094CountOverride] ([EmployerId], [TimeFrameId]) INCLUDE ([TotalEmployeeCnt])
GO

GRANT SELECT ON [dbo].[Employee1094CountOverride] TO [aca-user] AS DBO
GO
