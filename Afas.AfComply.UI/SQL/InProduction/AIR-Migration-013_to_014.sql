USE [air]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spUpdateAIR-Set1GCodes]
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
--:Parameters 
@employer_id INT,
@year_id SMALLINT,
@employee_id INT = NULL,
@user_name NVARCHAR(100)=NULL
-- ______________________________________________________________________________________________________________________________________________________
AS
-- ______________________________________________________________________________________________________________________________________________________
--:Variables
--:None Currently
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________

	-- used to be part 12 in the employer yearly detail but it was removing values it should not be
	-- based on outdated information. gc5

	UPDATE air.appr.employee_yearly_detail
	SET
		annual_offer_of_coverage_code = '1G',
		annual_share_lowest_cost_monthly_premium = NULL,
		annual_safe_harbor_code = NULL,
		_1095C = 1,
		is_1G = 1
	FROM
		air.appr.employee_yearly_detail eyd
	WHERE
		eyd.employee_id NOT IN (
			SELECT DISTINCT
				emd.employee_id
			FROM
				air.appr.employee_monthly_detail emd 
			WHERE
				(monthly_status_id IN (1))
					AND
				(emd.employer_id = @employer_id)
			)
			AND
		eyd.employee_id IN (
			SELECT DISTINCT
				emd.employee_id
			FROM
				air.appr.employee_monthly_detail emd 
			WHERE
				(insurance_type_id =2)
					AND
				(emd.enrolled = 1)
					AND
				(emd.employer_id = @employer_id)
			)
			AND
		(eyd.employer_id = @employer_id)
			AND
		(eyd.year_id = @year_id)
			AND 
		-- note: this will break when there is more than 10 million lives in the database.
		(eyd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

GO

GRANT EXECUTE ON [dbo].[spUpdateAIR-Set1GCodes] TO [air-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ______________________________________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- Create date: 03/28/2016
-- Description:	<Description,,>
-- ______________________________________________________________________________________________________________________________________________________
ALTER PROCEDURE [appr].[spInsert_employee_yearly_detail]
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
--:Parameters 
@employer_id INT,
@year_id SMALLINT,
@employee_id INT = NULL,
@user_name NVARCHAR(100)=NULL
-- ______________________________________________________________________________________________________________________________________________________
AS
-- ______________________________________________________________________________________________________________________________________________________
--:Variables
--:None Currently
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
PRINT '12: Delete Appr Employee Yearly Detail';
DELETE air.appr.employee_yearly_detail 
WHERE
	(employer_id = @employer_id)
		AND
	(year_id = @year_id)
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));
-- ______________________________________________________________________________________________________________________________________________________
PRINT '12: Insert Appr Employee Yearly Detail';
INSERT INTO air.appr.employee_yearly_detail (
		employee_id,
		year_id,
		employer_id,
		annual_offer_of_coverage_code,
		annual_share_lowest_cost_monthly_premium,
		annual_safe_harbor_code,
		enrolled,
		insurance_type_id,
		must_supply_ci_info
	)
SELECT DISTINCT
	emd.employee_id,
	t.year_id,
	emd.employer_id,
	cc.offer_of_coverage_code,
	slcmp.share_lowest_cost_monthly_premium,
	sh.safe_harbor_code,
	ISNULL(enr.enrolled, 0) AS enrolled, 
	ISNULL(it1.insurance_type_id, 0) + ISNULL(it2.insurance_type_id, 0) AS insurance_type_id,
	IIF(
			(it2.employee_id IS NOT NULL) OR (it3.employee_id IS NOT NULL),
			1,
			0
		) AS must_supply_ci_info
FROM
	air.appr.employee_monthly_detail emd 
	INNER JOIN air.gen.time_frame t ON (emd.time_frame_id = t.time_frame_id)
	LEFT OUTER JOIN (
			SELECT DISTINCT
				emd.employee_id,
				MAX(emd.offer_of_coverage_code) AS offer_of_coverage_code,
				t.year_id
			FROM
				air.appr.employee_monthly_detail emd 
				INNER JOIN air.gen.time_frame t ON (emd.time_frame_id = t.time_frame_id)
			GROUP BY
				emd.employee_id,
				t.year_id
			HAVING
				(COUNT(emd.offer_of_coverage_code) = 12)
					AND
				(COUNT(DISTINCT emd.offer_of_coverage_code) = 1)
			) cc ON (emd.employee_id = cc.employee_id)
	LEFT OUTER JOIN (
			SELECT DISTINCT
				emd.employee_id,
				MAX(emd.share_lowest_cost_monthly_premium) AS share_lowest_cost_monthly_premium,
				t.year_id
			FROM
				air.appr.employee_monthly_detail emd 
				INNER JOIN air.gen.time_frame t ON (emd.time_frame_id = t.time_frame_id)
			GROUP BY
				emd.employee_id,
				t.year_id
			HAVING
				(COUNT(emd.share_lowest_cost_monthly_premium) = 12)
					AND 
				(COUNT(DISTINCT emd.share_lowest_cost_monthly_premium) = 1)
			) slcmp ON (emd.employee_id = slcmp.employee_id)
	LEFT OUTER JOIN (
			SELECT DISTINCT
				emd.employee_id,
				MAX(emd.safe_harbor_code) AS safe_harbor_code,
				t.year_id
			FROM
				air.appr.employee_monthly_detail emd 
				INNER JOIN air.gen.time_frame t ON (emd.time_frame_id = t.time_frame_id)
			GROUP BY
				emd.employee_id,
				t.year_id
			HAVING
				(COUNT(emd.safe_harbor_code) = 12)
					AND
				(COUNT(DISTINCT emd.safe_harbor_code) = 1)
			) sh ON (emd.employee_id = sh.employee_id)
	LEFT OUTER JOIN (
			SELECT DISTINCT
				employee_id,
				enrolled 
			FROM
				air.appr.employee_monthly_detail 
			WHERE
				enrolled = 1
			) enr ON (emd.employee_id = enr.employee_id)
	LEFT OUTER JOIN (
			SELECT DISTINCT
				employee_id,
				insurance_type_id 
			FROM
				air.appr.employee_monthly_detail
			WHERE
				(insurance_type_id = 1)
					AND
				(enrolled = 1)
			) it1 ON (emd.employee_id = it1.employee_id)
	LEFT OUTER JOIN (
			SELECT DISTINCT
				employee_id,
				insurance_type_id 
			FROM
				air.appr.employee_monthly_detail 
			WHERE
				(insurance_type_id = 2)
					AND
				(enrolled = 1)
			) it2 ON (emd.employee_id = it2.employee_id)
	LEFT OUTER JOIN (
			SELECT DISTINCT
				employee_id,
				insurance_type_id 
			FROM
				air.appr.employee_monthly_detail
			WHERE
				(insurance_type_id IN (1,2))
					AND
				(enrolled = 0)
					AND 
				(employee_id IN(SELECT DISTINCT employee_id FROM air.emp.covered_individual))
			) it3 ON (emd.employee_id = it3.employee_id)
WHERE
	(emd.employer_id = @employer_id)
		AND
	(t.year_id = @year_id)
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(emd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

	-- old 1G logic is now in its own procedure. gc5

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spUpdateAIR]
	@employerId int,
	@yearId int
	
AS
BEGIN

	SET NOCOUNT ON;
	
	-- needs feature toggles on the database side. gc5

	-- TODO: Grab all ACA Full time not in AIR.

			EXECUTE [air].[dbo].[spUpdateAIR-InsuranceChangeEvents] @employerId, @yearId
				PRINT '1 *** Updating the insurance information based on change events. ***'
				PRINT ''
				PRINT ''

			-- order is important, this procedure must be the last one ran for the data corrections. gc5
			EXECUTE [air].[dbo].[spUpdateAIR-MonthlyHoursAndStatus] @employerId, @yearId
				PRINT '2 *** Updating hours from the MP/IMP and redetermining the monthly status. ***'
				PRINT ''
				PRINT ''

			--EXECUTE [air].dbo.[spUpdateAir-Set1GCodes] @employerId, @yearId
			--	PRINT '3 *** Setting 1G flags now that all of the data is ready. ***'
			--	PRINT ''
			--	PRINT ''

			EXECUTE [air].[appr].[spUpdate_1095C_status] @employerId, @yearId
				PRINT '4 *** Flagging the forms that should be presented on the 1095 screens. ***'
				PRINT ''
				PRINT ''

END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spUpdateAIR-MonthlyHoursAndStatus]
	@employerId int,
	@yearId int
	
AS
BEGIN

	SET NOCOUNT ON;

	UPDATE 
		air.emp.monthly_detail
	SET
		monthly_hours = ad.MonthlyAverageHours 
	FROM 
		air.emp.monthly_detail emd 
		INNER JOIN (
			SELECT
				   MAX(MonthlyAverageHours) as MonthlyAverageHours,
				   EmployeeId,
				   employer_id
			FROM (
				SELECT
					   MonthlyAverageHours,
					   eah.EmployeeId,
					   mea.employer_id
				FROM
					   aca.dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN aca.dbo.measurement mea ON (eah.MeasurementId = mea.measurement_id)
					   INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = eah.EmployeeId)
				WHERE
					   (eah.EntityStatusId = 1)
							 AND
					   eah.EmployeeId IN (select ee.employee_id from [aca].[dbo].[employee] ee where ee.employer_id = @employerId)
							 AND
					   eah.IsNewHire = 0
 
				UNION ALL
 
				SELECT
					   ee.imp_plan_year_avg_hours as MonthlyAverageHours,
					   eah.EmployeeId,
					   mea.employer_id
				FROM
					   aca.dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN aca.dbo.measurement mea ON (eah.MeasurementId = mea.measurement_id)
					   INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = eah.EmployeeId)
				WHERE
					   (eah.EntityStatusId = 1)
							 AND
					   eah.EmployeeId IN (select ee.employee_id from [aca].[dbo].[employee] ee where ee.employer_id = @employerId)
							 AND
					   eah.IsNewHire = 1
					   ) a
				GROUP BY EmployeeId, employer_id

			) ad ON (ad.EmployeeId = emd.employee_id) 
		INNER JOIN air.gen.time_frame tf ON (emd.time_frame_id = tf.time_frame_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId

-- now that we have real hours, reset the monthly_status_id
	UPDATE 
		air.emp.monthly_detail
	SET
		monthly_status_id =
			CASE
				--: Is employee In Termination Month?
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.terminationDate), MONTH(ee.terminationDate)),0) = emd.time_frame_id THEN 4
				
				--: Is employee Terminated and not in Termination Month? -- note rolls over at time_frame_id 1000. gc5
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.terminationDate), MONTH(ee.terminationDate)), 1000) < emd.time_frame_id THEN 5 
		
				--: Is employee Not Yet Hired?
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.hireDate), MONTH(ee.hireDate)),0) > emd.time_frame_id THEN 5 
		
				--: Is employee In Initial Measurement Period?
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.initialMeasurmentEnd), MONTH(ee.initialMeasurmentEnd)), 0) >= emd.time_frame_id THEN 3 
				
				--: Is employee In Administrative Period? -- this math looks right, compare logic looks wrong. gc5
				WHEN ISNULL(
						air.etl.ufnGetTimeFrameID(
									IIF(
											YEAR(ee.hireDate) = @yearId,
											YEAR(ee.hireDate),
											@yearId + 1
										), 
									MONTH(ee.hireDate)
								),
							0) + 3 BETWEEN air.etl.ufnGetTimeFrameID(YEAR(@yearId), MONTH(ee.hireDate)) AND emd.time_frame_id THEN 6
		
				--: Are there no monthly hours?
				WHEN ISNULL(emd.monthly_hours,0) = 0 THEN 7
		
				--: Is full-time according to hours?
				WHEN emd.monthly_hours > 129.99 THEN 1
		
				--: Is part-time according to hours? 
				WHEN emd.monthly_hours < 130 THEN 2 

			END
	FROM 
		air.emp.monthly_detail emd
		INNER JOIN [air].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId

	-- now appr.

	UPDATE 
		air.appr.employee_monthly_detail
	SET
		monthly_hours = ad.MonthlyAverageHours 
	FROM 
		air.appr.employee_monthly_detail emd 
		INNER JOIN (
			SELECT
				   MAX(MonthlyAverageHours) as MonthlyAverageHours,
				   EmployeeId,
				   employer_id
			FROM (
				SELECT
					   MonthlyAverageHours,
					   eah.EmployeeId,
					   mea.employer_id
				FROM
					   aca.dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN aca.dbo.measurement mea ON (eah.MeasurementId = mea.measurement_id)
					   INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = eah.EmployeeId)
				WHERE
					   (eah.EntityStatusId = 1)
							 AND
					   eah.EmployeeId IN (select ee.employee_id from [aca].[dbo].[employee] ee where ee.employer_id = @employerId)
							 AND
					   eah.IsNewHire = 0
 
				UNION ALL
 
				SELECT
					   ee.imp_plan_year_avg_hours as MonthlyAverageHours,
					   eah.EmployeeId,
					   mea.employer_id
				FROM
					   aca.dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN aca.dbo.measurement mea ON (eah.MeasurementId = mea.measurement_id)
					   INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = eah.EmployeeId)
				WHERE
					   (eah.EntityStatusId = 1)
							 AND
					   eah.EmployeeId IN (select ee.employee_id from [aca].[dbo].[employee] ee where ee.employer_id = @employerId)
							 AND
					   eah.IsNewHire = 1
					   ) a
				GROUP BY EmployeeId, employer_id

			) ad ON (ad.EmployeeId = emd.employee_id) 
		INNER JOIN air.gen.time_frame tf ON (emd.time_frame_id = tf.time_frame_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId

-- now that we have real hours, reset the monthly_status_id
	UPDATE 
		air.appr.employee_monthly_detail
	SET
		monthly_status_id =
			CASE
				--: Is employee In Termination Month?
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.terminationDate), MONTH(ee.terminationDate)),0) = emd.time_frame_id THEN 4
				
				--: Is employee Terminated and not in Termination Month? -- note rolls over at time_frame_id 1000. gc5
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.terminationDate), MONTH(ee.terminationDate)), 1000) < emd.time_frame_id THEN 5 
		
				--: Is employee Not Yet Hired?
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.hireDate), MONTH(ee.hireDate)),0) > emd.time_frame_id THEN 5 
		
				--: Is employee In Initial Measurement Period?
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.initialMeasurmentEnd), MONTH(ee.initialMeasurmentEnd)), 0) >= emd.time_frame_id THEN 3 
				
				--: Is employee In Administrative Period? -- see note above.
				WHEN ISNULL(
						air.etl.ufnGetTimeFrameID(
									IIF(
											YEAR(ee.hireDate) = @yearId,
											YEAR(ee.hireDate),
											@yearId + 1
										), 
									MONTH(ee.hireDate)
								),
							0) + 3 BETWEEN air.etl.ufnGetTimeFrameID(YEAR(@yearId), MONTH(ee.hireDate)) AND emd.time_frame_id THEN 6
		
				--: Are there no monthly hours?
				WHEN ISNULL(emd.monthly_hours,0) = 0 THEN 7
		
				--: Is full-time according to hours?
				WHEN emd.monthly_hours > 129.99 THEN 1
		
				--: Is part-time according to hours? 
				WHEN emd.monthly_hours < 130 THEN 2 

			END
	FROM 
		air.appr.employee_monthly_detail emd
		INNER JOIN [air].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId

	-- now reset the 1 codes.
	UPDATE [appr].[employee_monthly_detail]
	SET
		offer_of_coverage_code = air.etl.ufnGetMecCode(
				emd.time_frame_id,
				emd.mec_offered,
				offSpouse,
				offDependent,
				minValue,
				IIF(contribution_id = '%', 1, 0),
				CASE
					-- The ufn does a date compare on the effective date to determine values, do the math here. gc5
					WHEN eioe.CoverageInForce = 1 THEN DateAdd(month, -1, DATEFROMPARTS(tf.year_id, tf.month_id, 1))
					ELSE NULL
				END,
				terminationDate,
				aca_status_id,
				SpouseConditional
			)
	FROM
		[appr].[employee_monthly_detail] emd
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
		INNER JOIN [aca].[dbo].[insurance_contribution] ic ON (ic.classification_id = ee.classification_id)
		INNER JOIN [aca].[dbo].[insurance] i ON (i.insurance_id = ic.insurance_id)
		INNER JOIN [aca].[dbo].[EmployeeInsuranceOfferEditable] eioe ON (eioe.TimeFrameId = emd.time_frame_id AND eioe.EmployeeId = emd.employee_id)
		INNER JOIN [air].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @yearId
	
	UPDATE [emp].[monthly_detail]
	SET
		offer_of_coverage_code = air.etl.ufnGetMecCode(
				emd.time_frame_id,
				emd.mec_offered,
				offSpouse,
				offDependent,
				minValue,
				IIF(contribution_id = '%', 1, 0),
				CASE
					WHEN eioe.CoverageInForce = 1 THEN DATEFROMPARTS(tf.year_id, tf.month_id, 1)
					ELSE NULL
				END,
				terminationDate,
				aca_status_id,
				SpouseConditional
			)
	FROM
		 [emp].[monthly_detail] emd
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
		INNER JOIN [aca].[dbo].[insurance_contribution] ic ON (ic.classification_id = ee.classification_id)
		INNER JOIN [aca].[dbo].[insurance] i ON (i.insurance_id = ic.insurance_id)
		INNER JOIN [aca].[dbo].[EmployeeInsuranceOfferEditable] eioe ON (eioe.TimeFrameId = emd.time_frame_id AND eioe.EmployeeId = emd.employee_id)
		INNER JOIN [air].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @yearId

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ______________________________________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- Create date: 03/28/2016
-- Description:	<Description,,>
-- ______________________________________________________________________________________________________________________________________________________
ALTER PROCEDURE [appr].[spInsert_employee_yearly_detail_init]
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
--:Parameters 
@employer_id INT,
@year_id SMALLINT,
@employee_id INT,
@user_name NVARCHAR(100)=NULL
-- ______________________________________________________________________________________________________________________________________________________
AS
-- ______________________________________________________________________________________________________________________________________________________
--:Variables
--:None Currently
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
PRINT '12: Delete Appr Employee Yearly Detail';
DELETE air.appr.employee_yearly_detail 
WHERE
	(employer_id = @employer_id)
		AND
	(year_id = @year_id)
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));
-- ______________________________________________________________________________________________________________________________________________________
PRINT '12: Insert Appr Employee Yearly Detail';
INSERT INTO air.appr.employee_yearly_detail (
	employee_id,
	year_id,
	employer_id,
	annual_offer_of_coverage_code,
	annual_share_lowest_cost_monthly_premium,
	annual_safe_harbor_code,
	enrolled,
	_1095C
)
SELECT DISTINCT
	employee_id,
	year_id,
	employer_id,
	annual_offer_of_coverage_code,
	annual_share_lowest_cost_monthly_premium,
	annual_safe_harbor_code,
	enrolled,
	_1095C
FROM
	air.emp.yearly_detail  eyd 
WHERE
	(employer_id = @employer_id)
		AND
	(year_id = @year_id)
		AND
	-- note: this will break when there is more than 10 million lives in the database.
	(employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

-- ______________________________________________________________________________________________________________________________________________________

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
-- ______________________________________________________________________________________________________________________________________________________
AS
-- ______________________________________________________________________________________________________________________________________________________
--:Variables
DECLARE @ErrorMessage NVARCHAR(125)
DECLARE @dge_ein NCHAR(10)
DECLARE @_4980H_transition_relief_indicator BIT = 1
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
SELECT 
	@dge_ein = dge_ein,
	@_4980H_transition_relief_indicator = safeHarbor 
FROM
	aca.dbo.tax_year_approval 
WHERE
	(employer_id = @employer_id)
		AND
	(tax_year = @year_id)
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
			EXECUTE air.appr.spInsert_employee_yearly_detail_init @employer_id, @year_id, @employee_id, 'IRSTransmissionETL'
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

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spUpdateAIR-MonthlyHoursAndStatus]
	@employerId int,
	@yearId int
	
AS
BEGIN

	SET NOCOUNT ON;

	UPDATE 
		air.emp.monthly_detail
	SET
		monthly_hours = ad.MonthlyAverageHours 
	FROM 
		air.emp.monthly_detail emd 
		INNER JOIN (
			SELECT
				   MAX(MonthlyAverageHours) as MonthlyAverageHours,
				   EmployeeId,
				   employer_id
			FROM (
				SELECT
					   MonthlyAverageHours,
					   eah.EmployeeId,
					   mea.employer_id
				FROM
					   aca.dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN aca.dbo.measurement mea ON (eah.MeasurementId = mea.measurement_id)
					   INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = eah.EmployeeId)
				WHERE
					   (eah.EntityStatusId = 1)
							 AND
					   eah.EmployeeId IN (select ee.employee_id from [aca].[dbo].[employee] ee where ee.employer_id = @employerId)
							 AND
					   eah.IsNewHire = 0
 
				UNION ALL
 
				SELECT
					   ee.imp_plan_year_avg_hours as MonthlyAverageHours,
					   eah.EmployeeId,
					   mea.employer_id
				FROM
					   aca.dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN aca.dbo.measurement mea ON (eah.MeasurementId = mea.measurement_id)
					   INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = eah.EmployeeId)
				WHERE
					   (eah.EntityStatusId = 1)
							 AND
					   eah.EmployeeId IN (select ee.employee_id from [aca].[dbo].[employee] ee where ee.employer_id = @employerId)
							 AND
					   eah.IsNewHire = 1
					   ) a
				GROUP BY EmployeeId, employer_id

			) ad ON (ad.EmployeeId = emd.employee_id) 
		INNER JOIN air.gen.time_frame tf ON (emd.time_frame_id = tf.time_frame_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId

-- now that we have real hours, reset the monthly_status_id
	UPDATE 
		air.emp.monthly_detail
	SET
		monthly_status_id =
			CASE

				--: Is employee In Termination Month?
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.terminationDate), MONTH(ee.terminationDate)),0) = emd.time_frame_id THEN 4
				
				--: Is employee Terminated and not in Termination Month? -- note rolls over at time_frame_id 1000. gc5
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.terminationDate), MONTH(ee.terminationDate)), 1000) < emd.time_frame_id THEN 5 
		
				--: Is employee Not Yet Hired?
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.hireDate), MONTH(ee.hireDate)),0) > emd.time_frame_id THEN 5 
		
				--: Is employee In Initial Measurement Period?
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.initialMeasurmentEnd), MONTH(ee.initialMeasurmentEnd)), 0) >= emd.time_frame_id THEN 3 
				
				--: Is employee In Administrative Period?
				WHEN ISNULL(
						air.etl.ufnGetTimeFrameID(
									IIF(
											YEAR(ee.hireDate) = @yearId,
											YEAR(ee.hireDate),
											@yearId + 1
										), 
									MONTH(ee.hireDate)
								),
							0) + 3 BETWEEN air.etl.ufnGetTimeFrameID(YEAR(@yearId), MONTH(ee.hireDate)) AND emd.time_frame_id THEN 6
		
				--: Are there no monthly hours?
				WHEN ISNULL(emd.monthly_hours,0) = 0 THEN 7
		
				--: Is full-time according to hours?
				WHEN emd.monthly_hours > 129.99 THEN 1
		
				--: Is part-time according to hours? 
				WHEN emd.monthly_hours < 130 THEN 2 

			END
	FROM 
		air.emp.monthly_detail emd
		INNER JOIN [air].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId

	-- now appr.

	UPDATE 
		air.appr.employee_monthly_detail
	SET
		monthly_hours = ad.MonthlyAverageHours 
	FROM 
		air.appr.employee_monthly_detail emd 
		INNER JOIN (
			SELECT
				   MAX(MonthlyAverageHours) as MonthlyAverageHours,
				   EmployeeId,
				   employer_id
			FROM (
				SELECT
					   MonthlyAverageHours,
					   eah.EmployeeId,
					   mea.employer_id
				FROM
					   aca.dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN aca.dbo.measurement mea ON (eah.MeasurementId = mea.measurement_id)
					   INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = eah.EmployeeId)
				WHERE
					   (eah.EntityStatusId = 1)
							 AND
					   eah.EmployeeId IN (select ee.employee_id from [aca].[dbo].[employee] ee where ee.employer_id = @employerId)
							 AND
					   eah.IsNewHire = 0
 
				UNION ALL
 
				SELECT
					   ee.imp_plan_year_avg_hours as MonthlyAverageHours,
					   eah.EmployeeId,
					   mea.employer_id
				FROM
					   aca.dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN aca.dbo.measurement mea ON (eah.MeasurementId = mea.measurement_id)
					   INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = eah.EmployeeId)
				WHERE
					   (eah.EntityStatusId = 1)
							 AND
					   eah.EmployeeId IN (select ee.employee_id from [aca].[dbo].[employee] ee where ee.employer_id = @employerId)
							 AND
					   eah.IsNewHire = 1
					   ) a
				GROUP BY EmployeeId, employer_id

			) ad ON (ad.EmployeeId = emd.employee_id) 
		INNER JOIN air.gen.time_frame tf ON (emd.time_frame_id = tf.time_frame_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId

-- now that we have real hours, reset the monthly_status_id
	UPDATE 
		air.appr.employee_monthly_detail
	SET
		monthly_status_id =
			CASE
				--: Is employee In Termination Month?
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.terminationDate), MONTH(ee.terminationDate)),0) = emd.time_frame_id THEN 4
				
				--: Is employee Terminated and not in Termination Month? -- note rolls over at time_frame_id 1000. gc5
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.terminationDate), MONTH(ee.terminationDate)), 1000) < emd.time_frame_id THEN 5 
		
				--: Is employee Not Yet Hired?
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.hireDate), MONTH(ee.hireDate)),0) > emd.time_frame_id THEN 5 
		
				--: Is employee In Initial Measurement Period?
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.initialMeasurmentEnd), MONTH(ee.initialMeasurmentEnd)), 0) >= emd.time_frame_id THEN 3 
				
				--: Is employee In Administrative Period?
				WHEN ISNULL(
						air.etl.ufnGetTimeFrameID(
									IIF(
											YEAR(ee.hireDate) = @yearId,
											YEAR(ee.hireDate),
											@yearId + 1
										), 
									MONTH(ee.hireDate)
								),
							0) + 3 BETWEEN air.etl.ufnGetTimeFrameID(YEAR(@yearId), MONTH(ee.hireDate)) AND emd.time_frame_id THEN 6
		
				--: Are there no monthly hours?
				WHEN ISNULL(emd.monthly_hours,0) = 0 THEN 7
		
				--: Is full-time according to hours?
				WHEN emd.monthly_hours > 129.99 THEN 1
		
				--: Is part-time according to hours? 
				WHEN emd.monthly_hours < 130 THEN 2 

			END
	FROM 
		air.appr.employee_monthly_detail emd
		INNER JOIN [air].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId

	-- now reset the 1 codes.
	UPDATE [appr].[employee_monthly_detail]
	SET
		offer_of_coverage_code = air.etl.ufnGetMecCode(
				emd.time_frame_id,
				emd.mec_offered,
				offSpouse,
				offDependent,
				minValue,
				IIF(contribution_id = '%', 1, 0),
				CASE
					-- The ufn does a date compare on the effective date to determine values, do the math here. gc5
					WHEN eioe.CoverageInForce = 1 THEN DateAdd(month, -1, DATEFROMPARTS(tf.year_id, tf.month_id, 1))
					ELSE NULL
				END,
				terminationDate,
				aca_status_id,
				SpouseConditional
			)
	FROM
		[appr].[employee_monthly_detail] emd
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
		INNER JOIN [aca].[dbo].[insurance_contribution] ic ON (ic.classification_id = ee.classification_id)
		INNER JOIN [aca].[dbo].[insurance] i ON (i.insurance_id = ic.insurance_id)
		INNER JOIN [aca].[dbo].[EmployeeInsuranceOfferEditable] eioe ON (eioe.TimeFrameId = emd.time_frame_id AND eioe.EmployeeId = emd.employee_id)
		INNER JOIN [air].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @yearId
	
	-- now the emp ones.
	UPDATE [emp].[monthly_detail]
	SET
		offer_of_coverage_code = air.etl.ufnGetMecCode(
				emd.time_frame_id,
				emd.mec_offered,
				offSpouse,
				offDependent,
				minValue,
				IIF(contribution_id = '%', 1, 0),
				CASE
					-- The ufn does a date compare on the effective date to determine values, do the math here. gc5
					WHEN eioe.CoverageInForce = 1 THEN DateAdd(month, -1, DATEFROMPARTS(tf.year_id, tf.month_id, 1))
					ELSE NULL
				END,
				terminationDate,
				aca_status_id,
				SpouseConditional
			)
	FROM
		 [emp].[monthly_detail] emd
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
		INNER JOIN [aca].[dbo].[insurance_contribution] ic ON (ic.classification_id = ee.classification_id)
		INNER JOIN [aca].[dbo].[insurance] i ON (i.insurance_id = ic.insurance_id)
		INNER JOIN [aca].[dbo].[EmployeeInsuranceOfferEditable] eioe ON (eioe.TimeFrameId = emd.time_frame_id AND eioe.EmployeeId = emd.employee_id)
		INNER JOIN [air].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @yearId

	-- now reset the 2 codes.
	UPDATE [appr].[employee_monthly_detail]
	SET
		safe_harbor_code = [etl].[ufnGetSafeHarborCode](
				emd.monthly_status_id,
				ISNULL(emd.mec_offered, 0),
				emd.enrolled,
				ee.terminationDate,
				ee.aca_status_id,
				ec.ash_code
			)
	FROM
		[appr].[employee_monthly_detail] emd
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
		LEFT OUTER JOIN aca.dbo.employee_classification	ec ON (ec.classification_id = ee.classification_id)
	WHERE
		emd.employer_id = @employerId

	-- now emp.
	UPDATE [emp].[monthly_detail]
	SET
		safe_harbor_code = [etl].[ufnGetSafeHarborCode](
				emd.monthly_status_id,
				ISNULL(emd.mec_offered, 0),
				emd.enrolled,
				ee.terminationDate,
				ee.aca_status_id,
				ec.ash_code
			)
	FROM
		[emp].[monthly_detail] emd
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
		LEFT OUTER JOIN aca.dbo.employee_classification	ec ON (ec.classification_id = ee.classification_id)
	WHERE
		emd.employer_id = @employerId

END

GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [etl].[ufnDoesTerminationMonthHaveCoverage] (
	@employeeId INT,
	@timeFrameId INT,
	@terminationDate DATETIME
)
RETURNS BIT
AS
BEGIN

	IF (@terminationDate IS NULL)
		-- short circuit and return
		RETURN NULL

	IF (air.etl.ufnGetTimeFrameID(YEAR(@terminationDate), MONTH(@terminationDate)) = @timeFrameId)

		-- more work is needed to determine the outcome of the coverage
		DECLARE @coverageInForce BIT
		SELECT
			@coverageInForce = eioe.CoverageInForce
		FROM
			[aca].[dbo].[EmployeeInsuranceOfferEditable] eioe
		WHERE
			eioe.EmployeeId = @employeeId
				AND
			eioe.TimeFrameId = @timeFrameId

		return @coverageInForce

	-- default answer
	RETURN NULL

END

GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Returns 1 when the termination month has coverage and the coverage should override the termination month determination.' , @level0type=N'SCHEMA',@level0name=N'etl', @level1type=N'FUNCTION',@level1name=N'ufnDoesTerminationMonthHaveCoverage'
GO

GRANT EXECUTE ON [etl].[ufnDoesTerminationMonthHaveCoverage] TO  [air-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spUpdateAIR-MonthlyHoursAndStatus]
	@employerId int,
	@yearId int
	
AS
BEGIN

	SET NOCOUNT ON;

	UPDATE 
		air.emp.monthly_detail
	SET
		monthly_hours = ad.MonthlyAverageHours 
	FROM 
		air.emp.monthly_detail emd 
		INNER JOIN (
			SELECT
				   MAX(MonthlyAverageHours) as MonthlyAverageHours,
				   EmployeeId,
				   employer_id
			FROM (
				SELECT
					   MonthlyAverageHours,
					   eah.EmployeeId,
					   mea.employer_id
				FROM
					   aca.dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN aca.dbo.measurement mea ON (eah.MeasurementId = mea.measurement_id)
					   INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = eah.EmployeeId)
				WHERE
					   (eah.EntityStatusId = 1)
							 AND
					   eah.EmployeeId IN (select ee.employee_id from [aca].[dbo].[employee] ee where ee.employer_id = @employerId)
							 AND
					   eah.IsNewHire = 0
 
				UNION ALL
 
				SELECT
					   ee.imp_plan_year_avg_hours as MonthlyAverageHours,
					   eah.EmployeeId,
					   mea.employer_id
				FROM
					   aca.dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN aca.dbo.measurement mea ON (eah.MeasurementId = mea.measurement_id)
					   INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = eah.EmployeeId)
				WHERE
					   (eah.EntityStatusId = 1)
							 AND
					   eah.EmployeeId IN (select ee.employee_id from [aca].[dbo].[employee] ee where ee.employer_id = @employerId)
							 AND
					   eah.IsNewHire = 1
					   ) a
				GROUP BY EmployeeId, employer_id

			) ad ON (ad.EmployeeId = emd.employee_id) 
		INNER JOIN air.gen.time_frame tf ON (emd.time_frame_id = tf.time_frame_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId

-- now that we have real hours, reset the monthly_status_id
	UPDATE 
		air.emp.monthly_detail
	SET
		monthly_status_id =
			CASE

				--: Is employee In Termination Month and not covered?
				--WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.terminationDate), MONTH(ee.terminationDate)),0) = emd.time_frame_id THEN 4
				WHEN 
					ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.terminationDate), MONTH(ee.terminationDate)),0) = emd.time_frame_id
						AND
					[air].[etl].[ufnDoesTerminationMonthHaveCoverage](ee.employee_id, emd.time_frame_id, ee.terminationDate) = 0
				THEN 4
								
				--: Is employee Terminated and not in Termination Month? -- note rolls over at time_frame_id 1000. gc5
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.terminationDate), MONTH(ee.terminationDate)), 1000) < emd.time_frame_id THEN 5 
		
				--: Is employee Not Yet Hired?
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.hireDate), MONTH(ee.hireDate)),0) > emd.time_frame_id THEN 5 
		
				--: Is employee In Initial Measurement Period?
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.initialMeasurmentEnd), MONTH(ee.initialMeasurmentEnd)), 0) >= emd.time_frame_id THEN 3 
				
				--: Is employee In Administrative Period?
				WHEN ISNULL(
						air.etl.ufnGetTimeFrameID(
									IIF(
											YEAR(ee.hireDate) = @yearId,
											YEAR(ee.hireDate),
											@yearId + 1
										), 
									MONTH(ee.hireDate)
								),
							0) + 3 BETWEEN air.etl.ufnGetTimeFrameID(YEAR(@yearId), MONTH(ee.hireDate)) AND emd.time_frame_id THEN 6
		
				--: Are there no monthly hours?
				WHEN ISNULL(emd.monthly_hours,0) = 0 THEN 7
		
				--: Is full-time according to hours?
				WHEN emd.monthly_hours > 129.99 THEN 1
		
				--: Is part-time according to hours? 
				WHEN emd.monthly_hours < 130 THEN 2 

			END
	FROM 
		air.emp.monthly_detail emd
		INNER JOIN [air].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId

	-- now appr.

	UPDATE 
		air.appr.employee_monthly_detail
	SET
		monthly_hours = ad.MonthlyAverageHours 
	FROM 
		air.appr.employee_monthly_detail emd 
		INNER JOIN (
			SELECT
				   MAX(MonthlyAverageHours) as MonthlyAverageHours,
				   EmployeeId,
				   employer_id
			FROM (
				SELECT
					   MonthlyAverageHours,
					   eah.EmployeeId,
					   mea.employer_id
				FROM
					   aca.dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN aca.dbo.measurement mea ON (eah.MeasurementId = mea.measurement_id)
					   INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = eah.EmployeeId)
				WHERE
					   (eah.EntityStatusId = 1)
							 AND
					   eah.EmployeeId IN (select ee.employee_id from [aca].[dbo].[employee] ee where ee.employer_id = @employerId)
							 AND
					   eah.IsNewHire = 0
 
				UNION ALL
 
				SELECT
					   ee.imp_plan_year_avg_hours as MonthlyAverageHours,
					   eah.EmployeeId,
					   mea.employer_id
				FROM
					   aca.dbo.EmployeeMeasurementAverageHours eah
					   INNER JOIN aca.dbo.measurement mea ON (eah.MeasurementId = mea.measurement_id)
					   INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = eah.EmployeeId)
				WHERE
					   (eah.EntityStatusId = 1)
							 AND
					   eah.EmployeeId IN (select ee.employee_id from [aca].[dbo].[employee] ee where ee.employer_id = @employerId)
							 AND
					   eah.IsNewHire = 1
					   ) a
				GROUP BY EmployeeId, employer_id

			) ad ON (ad.EmployeeId = emd.employee_id) 
		INNER JOIN air.gen.time_frame tf ON (emd.time_frame_id = tf.time_frame_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId

-- now that we have real hours, reset the monthly_status_id
	UPDATE 
		air.appr.employee_monthly_detail
	SET
		monthly_status_id =
			CASE

				--: Is employee In Termination Month and not covered?
				--WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.terminationDate), MONTH(ee.terminationDate)),0) = emd.time_frame_id THEN 4
				WHEN 
					ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.terminationDate), MONTH(ee.terminationDate)),0) = emd.time_frame_id
						AND
					[air].[etl].[ufnDoesTerminationMonthHaveCoverage](ee.employee_id, emd.time_frame_id, ee.terminationDate) = 0
				THEN 4
				
				--: Is employee Terminated and not in Termination Month? -- note rolls over at time_frame_id 1000. gc5
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.terminationDate), MONTH(ee.terminationDate)), 1000) < emd.time_frame_id THEN 5 
		
				--: Is employee Not Yet Hired?
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.hireDate), MONTH(ee.hireDate)),0) > emd.time_frame_id THEN 5 
		
				--: Is employee In Initial Measurement Period?
				WHEN ISNULL(air.etl.ufnGetTimeFrameID(YEAR(ee.initialMeasurmentEnd), MONTH(ee.initialMeasurmentEnd)), 0) >= emd.time_frame_id THEN 3 
				
				--: Is employee In Administrative Period?
				WHEN ISNULL(
						air.etl.ufnGetTimeFrameID(
									IIF(
											YEAR(ee.hireDate) = @yearId,
											YEAR(ee.hireDate),
											@yearId + 1
										), 
									MONTH(ee.hireDate)
								),
							0) + 3 BETWEEN air.etl.ufnGetTimeFrameID(YEAR(@yearId), MONTH(ee.hireDate)) AND emd.time_frame_id THEN 6
		
				--: Are there no monthly hours?
				WHEN ISNULL(emd.monthly_hours,0) = 0 THEN 7
		
				--: Is full-time according to hours?
				WHEN emd.monthly_hours > 129.99 THEN 1
		
				--: Is part-time according to hours? 
				WHEN emd.monthly_hours < 130 THEN 2 

			END
	FROM 
		air.appr.employee_monthly_detail emd
		INNER JOIN [air].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId

	-- now reset the 1 codes.
	UPDATE [appr].[employee_monthly_detail]
	SET
		offer_of_coverage_code = air.etl.ufnGetMecCode(
				emd.time_frame_id,
				emd.mec_offered,
				offSpouse,
				offDependent,
				minValue,
				IIF(contribution_id = '%', 1, 0),
				CASE
					-- The ufn does a date compare on the effective date to determine values, do the math here. gc5
					WHEN eioe.CoverageInForce = 1 THEN DateAdd(month, -1, DATEFROMPARTS(tf.year_id, tf.month_id, 1))
					ELSE NULL
				END,
				terminationDate,
				aca_status_id,
				SpouseConditional
			)
	FROM
		[appr].[employee_monthly_detail] emd
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
		INNER JOIN [aca].[dbo].[insurance_contribution] ic ON (ic.classification_id = ee.classification_id)
		INNER JOIN [aca].[dbo].[insurance] i ON (i.insurance_id = ic.insurance_id)
		INNER JOIN [aca].[dbo].[EmployeeInsuranceOfferEditable] eioe ON (eioe.TimeFrameId = emd.time_frame_id AND eioe.EmployeeId = emd.employee_id)
		INNER JOIN [air].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @yearId
	
	-- now the emp ones.
	UPDATE [emp].[monthly_detail]
	SET
		offer_of_coverage_code = air.etl.ufnGetMecCode(
				emd.time_frame_id,
				emd.mec_offered,
				offSpouse,
				offDependent,
				minValue,
				IIF(contribution_id = '%', 1, 0),
				CASE
					-- The ufn does a date compare on the effective date to determine values, do the math here. gc5
					WHEN eioe.CoverageInForce = 1 THEN DateAdd(month, -1, DATEFROMPARTS(tf.year_id, tf.month_id, 1))
					ELSE NULL
				END,
				terminationDate,
				aca_status_id,
				SpouseConditional
			)
	FROM
		 [emp].[monthly_detail] emd
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
		INNER JOIN [aca].[dbo].[insurance_contribution] ic ON (ic.classification_id = ee.classification_id)
		INNER JOIN [aca].[dbo].[insurance] i ON (i.insurance_id = ic.insurance_id)
		INNER JOIN [aca].[dbo].[EmployeeInsuranceOfferEditable] eioe ON (eioe.TimeFrameId = emd.time_frame_id AND eioe.EmployeeId = emd.employee_id)
		INNER JOIN [air].[gen].[time_frame] tf ON (tf.time_frame_id = emd.time_frame_id)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @yearId

	-- now reset the 2 codes.
	UPDATE [appr].[employee_monthly_detail]
	SET
		safe_harbor_code = [etl].[ufnGetSafeHarborCode](
				emd.monthly_status_id,
				ISNULL(emd.mec_offered, 0),
				emd.enrolled,
				ee.terminationDate,
				ee.aca_status_id,
				ec.ash_code
			)
	FROM
		[appr].[employee_monthly_detail] emd
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
		LEFT OUTER JOIN aca.dbo.employee_classification	ec ON (ec.classification_id = ee.classification_id)
	WHERE
		emd.employer_id = @employerId

	-- now emp.
	UPDATE [emp].[monthly_detail]
	SET
		safe_harbor_code = [etl].[ufnGetSafeHarborCode](
				emd.monthly_status_id,
				ISNULL(emd.mec_offered, 0),
				emd.enrolled,
				ee.terminationDate,
				ee.aca_status_id,
				ec.ash_code
			)
	FROM
		[emp].[monthly_detail] emd
		INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = emd.employee_id)
		LEFT OUTER JOIN aca.dbo.employee_classification	ec ON (ec.classification_id = ee.classification_id)
	WHERE
		emd.employer_id = @employerId

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spUpdateAIR]
	@employerId int,
	@yearId int
	
AS
BEGIN

	SET NOCOUNT ON;
	
	-- needs feature toggles on the database side. gc5

	-- TODO: Grab all ACA Full time not in AIR.

			EXECUTE [air].[dbo].[spUpdateAIR-InsuranceChangeEvents] @employerId, @yearId
				PRINT '1 *** Updating the insurance information based on change events. ***'
				PRINT ''
				PRINT ''

			-- order is important, this procedure must be the last one ran for the data corrections. gc5
			EXECUTE [air].[dbo].[spUpdateAIR-MonthlyHoursAndStatus] @employerId, @yearId
				PRINT '2 *** Updating hours from the MP/IMP and redetermining the monthly status. ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].dbo.[spUpdateAir-Set1GCodes] @employerId, @yearId
				PRINT '3 *** Setting 1G flags now that all of the data is ready. ***'
				PRINT ''
				PRINT ''

			EXECUTE [air].[appr].[spUpdate_1095C_status] @employerId, @yearId
				PRINT '4 *** Flagging the forms that should be presented on the 1095 screens. ***'
				PRINT ''
				PRINT ''

END

GO
