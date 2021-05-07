USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[Select_tax_year_submission_errant_employees]    Script Date: 4/16/2018 1:56:01 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:            <Ryan McCully, Travis Wells>
-- Create date: <4/16/2018>
-- Description:       <This stored procedure is meant to return all taxYearEmployeeSubmissions by tax_year_employer_transmissionID.>
-- Changes:
--                    
-- =============================================
CREATE PROCEDURE [dbo].[Select_tax_year_submission_all_employees]
       @transmissionID int
AS

BEGIN
       SELECT 
         [tax_year_employee_transmissionId]
      ,[tax_year_employer_transmissionId]
      ,[tax_year]
      ,[employee_id]
      ,[employer_id]
      ,[ResourceId]
      ,[RecordID]
      ,[transmission_status_code_id]
      ,[EntityStatusId]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[ModifiedBy]
      ,[ModifiedDate]
       FROM [aca].[dbo].[tax_year_employee_transmission]
       WHERE tax_year_employer_transmissionId=@transmissionID;
END
GO
GRANT EXEC ON dbo.Select_tax_year_submission_all_employees TO [aca-user]
GO
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
ALTER PROCEDURE [dbo].[SELECT_Transmitted_EmployeeIds]
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
                        inner join
                        [aca].[dbo].[tax_year_employer_transmission] b
                        on a.TaxYearEmployerTransmissionId = b.tax_year_employer_transmissionId
       where
              a.[EntityStatusId] = 1 AND b.[EntityStatusId] = 1 
                                      AND
                        b.[ReceiptId] IS NOT NULL
                      AND 
              a.[EmployerId] = @EmployerId;

END TRY
BEGIN CATCH
       EXEC INSERT_ErrorLogging
END CATCH

