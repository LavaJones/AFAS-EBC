USE [air]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spUpdateAIR]
	@employerId int,
	@yearId int
	
AS
BEGIN

	SET NOCOUNT ON;

	UPDATE 
		air.appr.employee_monthly_detail 
	SET monthly_hours = ad.MonthlyAverageHours, 
		monthly_status_id = CASE
								WHEN ISNULL(ad.MonthlyAverageHours, 0) = 0 THEN 7
								WHEN ad.MonthlyAverageHours > 129.99 THEN 1
								WHEN ad.MonthlyAverageHours < 130 THEN 2
							END
	FROM 
		air.appr.employee_monthly_detail emd 
			INNER JOIN (
				SELECT
					MonthlyAverageHours, 
					eah.EmployeeId, 
					mea.employer_id
				FROM aca.dbo.EmployeeMeasurementAverageHours eah 
					INNER JOIN aca.dbo.measurement mea 
						ON eah.MeasurementId = mea.measurement_id
				WHERE  ((stability_start >= '2016-01-01 00:00:00.000' AND stability_start <= '2016-12-31 00:00:00.000')) AND (eah.EntityStatusId = 1)) ad 
					ON ad.EmployeeId = emd.employee_id 
						INNER JOIN air.gen.time_frame tf 
							ON emd.time_frame_id = tf.time_frame_id
	WHERE tf.year_id = @yearId AND emd.employer_id =  @employerId

END
GO
-- ______________________________________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- Create date: 1/23/2016
-- Description:	<Description,,>
-- ______________________________________________________________________________________________________________________________________________________
ALTER PROCEDURE [etl].[spETL_Build]
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
--:Parameters
@employer_id INT,
@year_id SMALLINT,
@employee_id INT = NULL,
@aag_indicator BIT = 0,
@aag_code TINYINT = 2
--:WHEN NEEDED FOR TESTING
--DECLARE @employer_id INT=11
--DECLARE @year_id SMALLINT = 2015
--DECLARE @employee_id INT = NULL
--DECLARE @aag_indicator BIT = 0
--DECLARE @aag_code TINYINT = 2
-- ______________________________________________________________________________________________________________________________________________________
AS
-- ______________________________________________________________________________________________________________________________________________________
--:Variables
DECLARE @ErrorMessage NVARCHAR(125)
DECLARE @dge_ein NCHAR(10)
DECLARE @_4980H_transition_relief_indicator BIT = 1
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
SELECT  @dge_ein = dge_ein, @_4980H_transition_relief_indicator = safeHarbor 
FROM	aca.dbo.tax_year_approval 
WHERE	(employer_id = @employer_id) AND (tax_year = @year_id)
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
BEGIN TRY
	BEGIN TRAN ETL_BUILD
		BEGIN
			EXECUTE air.etl.spInsert_ale_employer @employer_id, @year_id
				PRINT '1*** End Insert Employer***'
				PRINT ''
				PRINT ''
			EXECUTE air.etl.spInsert_ale_dge @employer_id, @year_id
				PRINT '1A*** End Insert Dge***'
				PRINT ''
				PRINT ''
			EXECUTE air.etl.spUpdate_employer @employer_id, @year_id
				PRINT '2*** End Update Employer***'
				PRINT ''
				PRINT ''
			EXECUTE air.etl.spUpdate_ale_dge @employer_id, @year_id
				PRINT '2A*** End Update Employer***'
				PRINT ''
				PRINT ''
			EXECUTE air.etl.spInsert_employee @employer_id, @employee_id
				PRINT '3*** End Insert Employee***'
				PRINT ''
				PRINT ''
			EXECUTE air.etl.spUpdate_employee @employer_id, @employee_id
				PRINT '4*** End Update Employee***'
				PRINT ''
				PRINT ''
			EXECUTE air.etl.spInsert_covered_individuals @employer_id, @year_id, @employee_id
				PRINT '5*** End Insert Covered Individuals***'
				PRINT ''
				PRINT ''
			EXECUTE air.etl.spInsert_covered_individuals_monthly_detail @employer_id, @year_id, @employee_id
				PRINT '6*** End Insert Covered Individuals Monthly***'
				PRINT ''
				PRINT ''
			EXECUTE air.etl.spInsert_employee_monthly_detail @employer_id, @year_id, @employee_id
				PRINT '7*** End Insert Employee Monthly Detail***'
				PRINT ''
				PRINT ''
			EXECUTE air.etl.spInsert_employee_yearly_detail @employer_id, @year_id, @employee_id
				PRINT '8*** End Insert Insert Employee Yearly Detail***'
				PRINT ''
				PRINT ''
			EXECUTE air.etl.spInsert_ale_monthly_detail @employer_id, @year_id, @aag_indicator, @_4980H_transition_relief_indicator
				PRINT '9*** End Insert Ale Monthly Detail***'
				PRINT ''
				PRINT ''
			EXECUTE air.etl.spInsert_ale_yearly_detail @employer_id, @year_id, @aag_code, @_4980H_transition_relief_indicator
				PRINT '10*** End Insert Ale Yearly Detail***'
				PRINT ''
				PRINT ''
			EXECUTE air.appr.spInsert_employee_monthly_detail @employer_id, @year_id, @employee_id, 'IRSTransmissionETL'
				PRINT '11*** End Insert Appr Employee Monthly Detail***'
				PRINT ''
				PRINT ''
			EXECUTE air.appr.spInsert_employee_yearly_detail @employer_id, @year_id, @employee_id, 'IRSTransmissionETL'
				PRINT '12*** End Insert Appr Employee Yearly Detail***'
				PRINT ''
				PRINT ''
			EXECUTE air.dbo.spUpdateAIR @employer_id, @year_id
				PRINT '13*** Update Hours and Monthly Status to be inline with ACA db.***'
				PRINT ''
				PRINT ''
		END	
	COMMIT TRAN BuildFormsTables;
	SELECT 'Successful';
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRAN ETL_BUILD
	SELECT ERROR_PROCEDURE() AS ErrorProcedure, ERROR_MESSAGE() AS ErrorMessage;
END CATCH
GO

	