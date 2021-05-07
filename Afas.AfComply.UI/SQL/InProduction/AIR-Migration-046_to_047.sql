USE [air]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spUpdateAIR-InsertMissingEmployee]
@employer_id INT,
@employee_id INT
AS

	INSERT INTO [air].[emp].[employee] ( 
		employee_id,
		employer_id,
		first_name,
		last_name,
		[address], 
		city,
		state_code,
		zipcode,
		telephone,
		ssn
	)
	SELECT DISTINCT	
			ee.employee_id,
			ee.employer_id, 
			UPPER(ee.fName), 
			UPPER(ee.lName),
			UPPER(ee.[address]),
			UPPER(ee.city),
			UPPER(s.abbreviation), 
			UPPER(SUBSTRING(ee.zip,1,5)), 
			UPPER(contact_telephone), 
			ee.ssn
	FROM
		[aca].[dbo].[employee] ee
		INNER JOIN [air].[ale].[employer] er ON (ee.employer_id = er.employer_id)
		INNER JOIN [aca].[dbo].[state] s ON (ee.state_id = s.state_id)
		LEFT OUTER JOIN [air].[emp].[employee] emp ON (ee.employee_id = emp.employee_id)
	WHERE
		(er.employer_id = @employer_id)
			AND
		(ee.employee_id = @employee_id)
			AND
		(emp.employee_id IS NULL) 

GO

GRANT EXECUTE ON [air].[dbo].[spUpdateAIR-InsertMissingEmployee] TO [air-user] AS DBO
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
			EXECUTE [air].[dbo].[spUpdateAIR-InsertMissingEmployee] @employer_id, @year_id, @employee_id

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
			EXECUTE air.appr.spInsert_employee_yearly_detail_init @employer_id, @year_id, @employee_id, 'IRSTransmissionETLMEE'

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
