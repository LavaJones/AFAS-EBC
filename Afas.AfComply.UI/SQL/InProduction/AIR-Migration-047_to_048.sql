USE [air]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ______________________________________________________________________________________________________________________________________________________
-- Author:		Travis Wells
-- Create date: 02/18/2017
-- Description:	<Description,,>
-- ______________________________________________________________________________________________________________________________________________________
ALTER PROCEDURE [etl].[spETL_Build_MissingEmployee]
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
--:Parameters
@employer_id INT,
@year_id SMALLINT,
@employee_id INT

-- ______________________________________________________________________________________________________________________________________________________
AS
-- ______________________________________________________________________________________________________________________________________________________
--:Variables
DECLARE @ErrorMessage NVARCHAR(125)
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
BEGIN TRY
	BEGIN TRAN ETL_BUILD
		BEGIN

			/*****     INSERT the employee into the AIR DB regardless of the employees status ************/
			EXECUTE [air].[dbo].[spUpdateAIR-InsertMissingEmployee] @employer_id, @employee_id

			/*  INSERT THE DETAILED records here.*/
			EXECUTE [air].[dbo].[spUpdateAIR-Import_MissedEmployees] @employer_id, @year_id, @employee_id

			/********************* Update LINE 15 ****************************************************/
			EXECUTE [air].[dbo].[spUpdateAIR-SetLine15] @employer_id, @year_id, @employee_id

			EXECUTE [air].[appr].[spUpdateAIR-SetLine15] @employer_id, @year_id, @employee_id

			/********************* Monthly Hours and Status  *****************************************/
			EXECUTE [air].[dbo].[spUpdateAIR-MonthlyHoursAndStatus] @employer_id, @year_id, @employee_id

			EXECUTE [air].[appr].[spUpdateAIR-MonthlyHoursAndStatus] @employer_id, @year_id, @employee_id

			-- delegate some heavy lifting back to the original procedures.
			EXECUTE air.etl.spInsert_employee_yearly_detail @employer_id, @year_id, @employee_id

			-- delegate some heavy lifting back to the original procedures.
			EXECUTE air.appr.spInsert_employee_yearly_detail_init @employer_id, @year_id, @employee_id, 'IRSTransmissionMEE'

			EXECUTE [air].[dbo].[spUpdateAir-Set1GCodes] @employer_id, @year_id, @employee_id

			EXECUTE [air].[appr].[spUpdateAir-Set1GCodes] @employer_id, @year_id, @employee_id

			EXECUTE [air].[appr].[spUpdate_1095C_status] @employer_id, @year_id, @employee_id

		END	
	COMMIT TRAN BuildFormsTables;
	SELECT 'Successful';
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRAN ETL_BUILD
	EXEC aca.dbo.INSERT_ErrorLogging
	SELECT ERROR_PROCEDURE() AS ErrorProcedure, ERROR_MESSAGE() AS ErrorMessage;
END CATCH					
GO
