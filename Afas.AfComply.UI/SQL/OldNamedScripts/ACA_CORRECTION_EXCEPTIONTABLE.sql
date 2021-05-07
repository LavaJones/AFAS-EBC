USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tax_year_1095c_correction_exception](
        [TaxYear1095cCorrectionExceptionId] [int] IDENTITY(1,1) NOT NULL,
        [tax_year] [int] NOT NULL,
        [employer_id] [int] NOT NULL,
        [employee_id] [int] NOT NULL,
        [Justification] [varchar](2048) NULL,
        [CreatedBy] [nvarchar](50) NOT NULL,
        [CreatedDate] [datetime2](7) NOT NULL,
        [ModifiedBy] [nvarchar](50) NOT NULL,
        [ModifiedDate] [datetime2](7) NOT NULL,
        [EntityStatusId] [int] NOT NULL,
        [ResourceId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_tax_year_1095c_correction_exception] PRIMARY KEY CLUSTERED 
(
        [TaxYear1095cCorrectionExceptionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

-- employer and employee keys omitted since they have to be removable at the application level to prevent additional transmissions
-- or logins.

ALTER TABLE [dbo].[tax_year_1095c_correction_exception]  WITH CHECK ADD  CONSTRAINT [FK_tax_year_1095c_correction_exception] FOREIGN KEY([tax_year])
REFERENCES [dbo].[tax_year] ([tax_year])
GO

ALTER TABLE [dbo].[tax_year_1095c_correction_exception] CHECK CONSTRAINT [FK_tax_year_1095c_correction_exception]
GO

-- needs the entity status constraint moved over. gc5

-- needs the resource id unique constraint and default moved over. gc5

