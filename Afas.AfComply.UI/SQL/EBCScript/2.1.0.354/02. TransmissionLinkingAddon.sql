USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[BULK_INSERT_tax_year_employer_transmission]    Script Date: 4/6/2018 3:01:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:            Travis Wells
-- Create date: 07/10/2017
-- Description:       insert and update tax_year_employer_transmission
-- =============================================
ALTER PROCEDURE [dbo].[BULK_INSERT_tax_year_employer_transmission]
       -- Add the parameters for the stored procedure here
       @tyet TYET readonly
AS
BEGIN
       DECLARE @tyetID int;
       SET @tyetID = 0;

       INSERT INTO aca.dbo.tax_year_employer_transmission(
              tax_year,
              employer_id,
              TransmissionType,
              UniqueTransmissionId,                      
              UniqueSubmissionId,
              transmission_status_code_id,
              OriginalReceiptId,
              OriginalUniqueSubmissionId,
              EntityStatusId,
              CreatedBy,
              CreatedDate,
              ModifiedBy,
              ModifiedDate,
              BulkFile,
              ManifestFile
              )
       SELECT DISTINCT 
              tax_year, 
              employer_id, 
              TransmissionType, 
              UniqueTransmissionId, 
              UniqueSubmissionId, 
              transmission_status_code_id,
              OriginalReceiptId,
              OriginalUniqueSubmissionId,
              EntityStatusId,
              CreatedBy,
              CreatedDate,
              ModifiedBy,
              ModifiedDate,
              BulkFile,
              ManifestFile
       FROM @tyet;

       SELECT @tyetID=SCOPE_IDENTITY();

       INSERT INTO aca.dbo.tax_year_employee_transmission(
              tax_year_employer_transmissionId,
              tax_year,
              employee_id,
              employer_id,
              RecordID,
              transmission_status_code_id,
              EntityStatusId,
              CreatedBy,
              CreatedDate,
              ModifiedBy,
              ModifiedDate
                      )
       SELECT 
              @tyetID,
              tax_year, 
              employee_id, 
              employer_id,  
              RecordID,
              transmission_status_code_id,
              EntityStatusId,
              CreatedBy,
              CreatedDate,
              ModifiedBy,
              ModifiedDate 
       FROM @tyet;

       INSERT INTO 
              [aca].[dbo].[TransmissionLinking]
         (
      [Approved1095FinalId]
      ,[EmployerId]
      ,[EmployeeId]
         ,[RecordId]
      ,[TaxYear]
      ,[TaxYearEmployerTransmissionId]
      ,[TaxYearEmployeeTransmissionId]
      ,[EntityStatusId]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[ModifiedBy]
      ,[ModifiedDate]
         )    
       Select        
              af.Approved1095FinalId
              ,tyet.[employer_id]
              ,tyet.[employee_id]
                        ,tyet.[RecordId]
              ,tyet.[tax_year]
              ,tyet.tax_year_employer_transmissionId
              ,tyet.[tax_year_employee_transmissionId]
              ,1
              ,tyet.CreatedBy
              ,tyet.CreatedDate
              ,tyet.ModifiedBy
              ,tyet.ModifiedDate 
       from 
              [aca].[dbo].[tax_year_employee_transmission] tyet 
                      Join 
              [aca].[dbo].[Approved1095Final] af 
                      on 
              af.employeeID = tyet.employee_id 
                      AND 
              af.EntityStatusId = 1 
       where 
              tyet.tax_year_employer_transmissionId = @tyetID;

END




GO
/****** Object:  StoredProcedure [dbo].[UnApproveAllErrant1095ForTransmission]    Script Date: 4/6/2018 3:02:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:            <Ryan McCully>
-- Create date: <4/4/2018>
-- Description:       <.>
-- Changes:
--                    
-- =============================================
CREATE PROCEDURE [dbo].[UnApproveAllErrant1095ForTransmission]
       @transmissionID int
AS

BEGIN

       Declare @TEMP Table ( 
              [tax_year_employee_transmissionId] [int] NOT NULL,
              [tax_year_employer_transmissionId] [int] NULL,
              [tax_year] [int] NOT NULL,
              [employee_id] [int] NOT NULL,
              [employer_id] [int] NOT NULL,
              [ResourceId] [uniqueidentifier] NOT NULL ,
              [RecordID] [int] NOT NULL,
              [transmission_status_code_id] [int] NULL,
              [EntityStatusId] [int] NOT NULL,
              [CreatedBy] [nvarchar](50) NOT NULL,
              [CreatedDate] [datetime2](7) NOT NULL,
              [ModifiedBy] [nvarchar](50) NOT NULL,
              [ModifiedDate] [datetime2](7) NOT NULL
       )

       Insert @TEMP EXEC SELECT_tax_year_submission_errant_employees @transmissionID 
            
       update [aca].[dbo].[Approved1095Final] set EntityStatusId = 2 where [Approved1095FinalId] in
       (
              SELECT distinct [Approved1095FinalId]
              from dbo.TransmissionLinking 
              where TaxYearEmployeeTransmissionId in ( select tax_year_employee_transmissionId FROM @TEMP)
       )

END



GO
/****** Object:  StoredProcedure [dbo].[SELECT_Transmitted_EmployeeIds]    Script Date: 4/6/2018 3:03:15 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:    Ryan McCully
-- Create date: 4/4/2018
-- Description:       Selects the Id's of all approved 1095s that have been transmitted.
-- Modifications : 
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_Transmitted_EmployeeIds]
      @EmployerId int
AS
BEGIN TRY
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

       SELECT DISTINCT 
              [Approved1095FinalId] 
       FROM 
              [aca].[dbo].[TransmissionLinking] a 
				inner join aca.dbo.tax_year_employer_transmission b 
			ON a.TaxYearEmployerTransmissionId = b.tax_year_employer_transmissionId
       where
              a.[EntityStatusId] = 1 
                      AND 
              a.[EmployerId] = @EmployerId
					AND
			b.ReceiptId is not null

END TRY
BEGIN CATCH
       EXEC INSERT_ErrorLogging
END CATCH
GO
GRANT EXEC ON [dbo].[SELECT_Transmitted_EmployeeIds] TO [aca-user]
GO
GRANT EXEC ON [dbo].[UnApproveAllErrant1095ForTransmission] TO [aca-user]
GO
GRANT EXEC ON [dbo].[BULK_INSERT_tax_year_employer_transmission] TO [aca-user]