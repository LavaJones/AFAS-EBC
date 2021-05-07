USE [air]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [appr].[spUpdateAIR-Import_MissedEmployees]
	@employerId INT,
	@yearId SMALLINT,
	@employeeId INT
AS

	-- now into appr.
	-- We let the Line15, InsuranceChangeEvents and MonthlyHoursAndStatus clean things up.
	INSERT INTO [air].[appr].[employee_monthly_detail] (
		employee_id,
		time_frame_id,
		employer_id,
		monthly_hours,
		offer_of_coverage_code,
		mec_offered,
		share_lowest_cost_monthly_premium,
		safe_harbor_code,
		enrolled,
		monthly_status_id,
		insurance_type_id,
		hra_flex_contribution
	)
	SELECT
		EmployeeId,
		TimeFrameId,
		EmployerId,
		-2 AS monthly_hours,
		NULL AS offer_of_coverage_code,
		0 AS mec_offered,
		NULL AS share_lowest_cost_monthly_premium,
		NULL AS safe_harbor_code,
		0 AS enrolled,
		7 AS monthly_status_id,
		NULL AS insurance_type_id,
		NULL AS hra_flex_contribution
	FROM
		[aca].[dbo].[EmployeeInsuranceOfferEditable] eioe WITH (NOLOCK)
		INNER JOIN [air].[emp].[employee] ee WITH (NOLOCK) ON (ee.employee_id = eioe.EmployeeId)
		INNER JOIN [aca].[dbo].[employee] adee WITH (NOLOCK) ON (adee.employee_id = ee.employee_id)
	WHERE
		eioe.EmployerId = @employerId
			AND
		adee.employee_id = @employeeId
			AND
		eioe.EmployeeId NOT IN (
			SELECT DISTINCT
				emd.employee_id
			FROM
				[air].[appr].[employee_monthly_detail] emd WITH (NOLOCK)
				INNER JOIN [air].[gen].[time_frame] tf WITH (NOLOCK) ON (tf.time_frame_id = emd.time_frame_id)
			WHERE
				emd.employer_id = @employerId
					AND
				tf.year_id = @yearId
		)

GO

GRANT EXECUTE ON [appr].[spUpdateAIR-Import_MissedEmployees] TO [air-user] AS DBO
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <2/17/2017>
-- Description:	<This stored procedure is meant to return a row count of records found by employeeID and Tax Year>
-- Modifications:
-- =============================================
CREATE PROCEDURE [dbo].[spSelect_does_employee_exist_taxyear] (
	@employeeID int,
	@taxYear int,
	@rowCount int output
	)
AS
BEGIN
	SELECT
		@rowCount = COUNT(eyd.employee_id)
	FROM
		[air].[appr].[employee_yearly_detail] eyd WITH (NOLOCK)
	WHERE
		eyd.employee_id = @employeeID
			AND
		eyd.year_id=@taxYear;
END

RETURN @rowCount;

GO

GRANT EXECUTE ON [dbo].[spSelect_does_employee_exist_taxyear] TO [air-user] AS DBO
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/********************************************************
Removed NULL value for employeeID, this needs to recieve an employeeID. 
Added the EmployeeID in the first Where Clause so only a specific individual is updated. 
Removed a bunch of the WHERE clause restrictions so anyone can be brought into the AIR db. 
********************************************************/
CREATE PROCEDURE [dbo].[spUpdateAIR-Import_MissedEmployees]
	@employerId INT,
	@yearId SMALLINT,
	@employeeId INT
AS

	-- insert these folks into emp first.
	-- We let the Line15, InsuranceChangeEvents and MonthlyHoursAndStatus clean things up.
	INSERT INTO [air].[emp].[monthly_detail] (
		employee_id,
		time_frame_id,
		employer_id,
		monthly_hours,
		offer_of_coverage_code,
		mec_offered,
		share_lowest_cost_monthly_premium,
		safe_harbor_code,
		enrolled,
		monthly_status_id,
		insurance_type_id,
		hra_flex_contribution
	)
	SELECT
		EmployeeId,
		TimeFrameId,
		EmployerId,
		-2 AS monthly_hours,
		NULL AS offer_of_coverage_code,
		0 AS mec_offered,
		NULL AS share_lowest_cost_monthly_premium,
		NULL AS safe_harbor_code,
		0 AS enrolled,
		7 AS monthly_status_id,
		NULL AS insurance_type_id,
		NULL AS hra_flex_contribution
	FROM
		[aca].[dbo].[EmployeeInsuranceOfferEditable] eioe WITH (NOLOCK)
		INNER JOIN [air].[emp].[employee] ee WITH (NOLOCK) ON (ee.employee_id = eioe.EmployeeId)
		INNER JOIN [aca].[dbo].[employee] adee WITH (NOLOCK) ON (adee.employee_id = ee.employee_id)
	WHERE
		eioe.EmployerId = @employerId
			AND
		adee.employee_id = @employeeId
			AND
		eioe.EmployeeId NOT IN (
			SELECT DISTINCT
				emd.employee_id
			FROM
				[air].[emp].[monthly_detail] emd WITH (NOLOCK)
				INNER JOIN [air].[gen].[time_frame] tf WITH (NOLOCK) ON (tf.time_frame_id = emd.time_frame_id)
			WHERE
				emd.employer_id = @employerId
					AND
				tf.year_id = @yearId
		)

GO

GRANT EXECUTE ON [dbo].[spUpdateAIR-Import_MissedEmployees] TO [air-user] AS DBO
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
CREATE PROCEDURE [etl].[spETL_Build_MissingEmployee]
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
			EXECUTE [air].[dbo].[spUpdateAIR-Import_MissedEmployees] @employer_id, @year_id, @employee_id
				PRINT '11A *** Grab any potential full time coded employees that did not finish their IMP. (emp) ***'
				PRINT ''
				PRINT ''
			EXECUTE [air].[appr].[spUpdateAIR-Import_MissedEmployees] @employer_id, @year_id, @employee_id
				PRINT '11B *** Grab any potential full time coded employees that did not finish their IMP. (appr) ***'
				PRINT ''
				PRINT ''

			/********************* Update LINE 15 ****************************************************/
			EXECUTE [air].[dbo].[spUpdateAIR-SetLine15] @employer_id, @year_id, @employee_id
				PRINT '12A *** Cleanup the overwritten line 15 entries. (emp) ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[appr].[spUpdateAIR-SetLine15] @employer_id, @year_id, @employee_id
				PRINT '12B *** Cleanup the overwritten line 15 entries. (appr) ***'
				PRINT ''
				PRINT ''

			/********************* Monthly Hours and Status  *****************************************/
			EXECUTE [air].[dbo].[spUpdateAIR-MonthlyHoursAndStatus] @employer_id, @year_id, @employee_id
				PRINT '14A *** Updating hours from the MP/IMP and redetermining the monthly status. (emp) ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[appr].[spUpdateAIR-MonthlyHoursAndStatus] @employer_id, @year_id, @employee_id
				PRINT '14B *** Updating hours from the MP/IMP and redetermining the monthly status. (appr) ***'
				PRINT ''
				PRINT ''

			-- delegate some heavy lifting back to the original procedures.
			EXECUTE air.etl.spInsert_employee_yearly_detail @employer_id, @year_id, @employee_id
				PRINT '15 *** End Insert Insert Employee Yearly Detail ***'
				PRINT ''
				PRINT ''

			-- delegate some heavy lifting back to the original procedures.
			EXECUTE air.appr.spInsert_employee_yearly_detail_init @employer_id, @year_id, @employee_id, 'IRSTransmissionETL'
				PRINT '16 *** End Insert Appr Employee Yearly Detail ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[dbo].[spUpdateAir-Set1GCodes] @employer_id, @year_id, @employee_id
				PRINT '17A *** Setting 1G flags now that all of the data is ready. (emp) ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[appr].[spUpdateAir-Set1GCodes] @employer_id, @year_id, @employee_id
				PRINT '17B *** Setting 1G flags now that all of the data is ready. (appr) ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[appr].[spUpdate_1095C_status] @employer_id, @year_id, @employee_id
				PRINT '18 *** Flagging the forms that should be presented on the 1095 screens. ***'
				PRINT ''
				PRINT ''
		END	
	COMMIT TRAN BuildFormsTables;
	SELECT 'Successful';
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRAN ETL_BUILD
	EXEC aca.dbo.INSERT_ErrorLogging
	SELECT ERROR_PROCEDURE() AS ErrorProcedure, ERROR_MESSAGE() AS ErrorMessage;
END CATCH					
-- ______________________________________________________________________________________________________________________________________________________

GO

GRANT EXECUTE ON [etl].[spETL_Build_MissingEmployee] TO [air-user] AS DBO
GO
