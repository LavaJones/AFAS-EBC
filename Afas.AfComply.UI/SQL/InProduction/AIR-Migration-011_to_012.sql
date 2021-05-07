USE [air]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spChangeEvent]
	@employeeId int,
	@yearId int
AS
BEGIN
	
	SET NOCOUNT ON;

    DECLARE @itemCount INT
	DECLARE @temp TABLE
		  (
				rowId Int, 
				offeredOn datetime, 
				terminationDate datetime, 
				employee_id int,
				employer_id int,
				offered bit,
				accepted bit
		  )
	DECLARE @timeFrame TABLE 
		(
			timeFrame int,
			employerId int,
			employeeId int,
			startMonths datetime,
			endMonths datetime,
			month_id int,
			notWaved bit,
			useDate datetime,
			noChange bit
		 );
	WITH insuranceCombine
	AS (
		SELECT  [rowid]
			  ,[employee_id]
			  ,[plan_year_id]
			  ,[employer_id]
			  ,[insurance_id]
			  ,[ins_cont_id]
			  ,[avg_hours_month]
			  ,[offered]
			  ,[offeredOn]
			  ,[accepted]
			  ,[acceptedOn]
			  ,[modOn]
			  ,[modBy]
			  ,[notes]
			  ,[history]
			  ,[effectiveDate]
			  ,[hra_flex_contribution]
			  ,[ResourceId]
		  FROM [aca].[dbo].[employee_insurance_offer_archive]
		  UNION 
		  SELECT  [rowid]
			  ,[employee_id]
			  ,[plan_year_id]
			  ,[employer_id]
			  ,[insurance_id]
			  ,[ins_cont_id]
			  ,[avg_hours_month]
			  ,[offered]
			  ,[offeredOn]
			  ,[accepted]
			  ,[acceptedOn]
			  ,[modOn]
			  ,[modBy]
			  ,[notes]
			  ,[history]
			  ,[effectiveDate]
			  ,[hra_flex_contribution]
			  ,[ResourceId]
		  FROM [aca].[dbo].[employee_insurance_offer]
		  ), 
		  getDateData AS 
		  (
			  SELECT 
				ROW_NUMBER() OVER(ORDER BY ic.offeredOn) AS rowId , 
				CASE  
					WHEN ic.effectiveDate is null AND ic.acceptedOn is null THEN offeredOn 
					WHEN ic.effectiveDate is null THEN ic.acceptedOn
					ELSE ic.effectiveDate
					END AS CorrectedDate , 
					em.terminationDate, 
					ic.employee_id, 
					ic.employer_id, 
					ic.offered, 
					ic.accepted 
			  FROM 
				insuranceCombine ic 
					INNER JOIN 
				aca.dbo.employee em 
					ON ic.employee_id = em.employee_id 
			  WHERE ic.employee_id = @employeeId
		  )
		  
		  INSERT INTO 
			@temp 
				(rowId,
				offeredOn,
				terminationDate, 
				employee_id, 
				employer_id, 
				offered, 
				accepted )
		  SELECT 
			rowId, 
			CorrectedDate, 
			terminationDate, 
			employee_id, 
			employer_id, 
			offered, 
			accepted 
		  FROM 
			getDateData
	  
		  SELECT @itemCount =  COUNT(employer_id) FROM @temp
		  IF(@itemCount <> 0 )
		  DECLARE @inc int = 1
		  DECLARE @setOne datetime
		  DECLARE @setTwo datetime
		  DECLARE @num int
		  DECLARE @notWaved bit = 1
		  DECLARE @useDate datetime
		  DECLARE @noChange bit
		  SET @itemCount = @itemCount - 1
			WHILE(@itemCount <> 0)
			BEGIN
				SELECT @setOne = OfferedOn FROM @temp WHERE rowId = @inc
				SET @inc = @inc + 1
				SELECT @setTwo = OfferedOn FROM @temp WHERE rowId = @inc
				SELECT @notWaved = CASE
					WHEN (t.offered is null OR t.offered = 0) and (t.accepted is null OR t.accepted = 0) THEN 0
					WHEN (t.offered is null OR t.offered = 0) and (t.accepted = 1) THEN 0
					ELSE  1
					END
				FROM @temp t WHERE rowId = @inc
				IF(@notWaved = 0)
				SELECT @noChange = CASE
					WHEN EOMONTH(@setTwo) > EOMONTH(t.terminationDate) THEN 0
					ELSE 1
					END
				FROM @temp t WHERE rowId = @inc
				SELECT @useDate = CASE
					WHEN @setTwo > t.terminationDate THEN @setTwo
					ELSE t.terminationDate
					END
				FROM @temp t WHERE rowId = @inc

				INSERT INTO 
					@timeFrame 
						(timeFrame, 
						employerId, 
						employeeId, 
						startMonths, 
						endMonths, 
						month_id, 
						notWaved, 
						useDate,
						noChange)
				SELECT 
					CONVERT(int,DATEDIFF(Month, @setOne, @setTwo)), 
					employer_id, 
					employee_id, 
					@setOne, 
					@setTwo, 
					air.etl.ufnGetMonthFromTimeFrame(DATEPART(m, @setOne)), 
					@notWaved, 
					@useDate,
					@noChange
				FROM @temp
				SET @itemCount = @itemCount - 1
			END
	  
		  DECLARE @dupPrevious TABLE (
			employerId int,
			employeeId int,
			monthly_hours decimal(18,2),
			offer_of_coverage_code varchar(10),
			mec_offered int,
			share_lowest_cost_monthly_premium decimal(18,2),
			safe_harbor_code varchar(10),
			enrolled int,
			insurance_type_id int,
			hra_flex_contribution decimal(10,2),
			timeFrame int
		  )

		  INSERT INTO 
			@dupPrevious 
				(employerId,
				employeeId, 
				monthly_hours, 
				offer_of_coverage_code, 
				mec_offered, 
				share_lowest_cost_monthly_premium, 
				safe_harbor_code, enrolled, 
				insurance_type_id, 
				hra_flex_contribution,
				timeFrame)
			SELECT
				employer_id,
				employee_id, 
				monthly_hours, 
				offer_of_coverage_code, 
				mec_offered, 
				share_lowest_cost_monthly_premium, 
				safe_harbor_code, enrolled, 
				insurance_type_id, 
				hra_flex_contribution,
				time_frame_id
			FROM air.appr.employee_monthly_detail
			WHERE time_frame_id in (SELECT air.etl.ufnGetTimeFrameID(YEAR(useDate), MONTH(useDate)) FROM @timeFrame WHERE noChange = 0 AND notWaved = 0)
			
			UPDATE 
				air.appr.employee_monthly_detail 
					SET monthly_hours = ch.monthly_hours, 
						offer_of_coverage_code = ch.offer_of_coverage_code,
						mec_offered = ch.mec_offered,
						share_lowest_cost_monthly_premium = ch.share_lowest_cost_monthly_premium,
						safe_harbor_code = ch.safe_harbor_code,
						enrolled = ch.enrolled,
						insurance_type_id = ch.insurance_type_id,
						hra_flex_contribution = ch.hra_flex_contribution
			FROM air.appr.employee_monthly_detail emd INNER JOIN @dupPrevious ch ON ch.employeeId = emd.employee_id WHERE emd.time_frame_id = (ch.timeFrame + 1) 
END
GO

GRANT EXECUTE ON [dbo].[spChangeEvent] TO [air-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spUpdateAIR-InsuranceChangeEvents]
	@employerId int,
	@yearId int
	
AS
BEGIN

	SET NOCOUNT ON;
	
	DECLARE @employeeChangeEvent INT
	DECLARE changeEvent CURSOR FOR
	SELECT DISTINCT combinedOffer.employee_id FROM (SELECT  [rowid]
			  ,[employee_id]
			  ,[plan_year_id]
			  ,[employer_id]
			  ,[insurance_id]
			  ,[ins_cont_id]
			  ,[avg_hours_month]
			  ,[offered]
			  ,[offeredOn]
			  ,[accepted]
			  ,[acceptedOn]
			  ,[modOn]
			  ,[modBy]
			  ,[notes]
			  ,[history]
			  ,[effectiveDate]
			  ,[hra_flex_contribution]
			  ,[ResourceId]
		  FROM [aca].[dbo].[employee_insurance_offer_archive]
		  UNION 
		  SELECT  [rowid]
			  ,[employee_id]
			  ,[plan_year_id]
			  ,[employer_id]
			  ,[insurance_id]
			  ,[ins_cont_id]
			  ,[avg_hours_month]
			  ,[offered]
			  ,[offeredOn]
			  ,[accepted]
			  ,[acceptedOn]
			  ,[modOn]
			  ,[modBy]
			  ,[notes]
			  ,[history]
			  ,[effectiveDate]
			  ,[hra_flex_contribution]
			  ,[ResourceId]
		  FROM [aca].[dbo].[employee_insurance_offer]) AS combinedOffer
		OPEN changeEvent
		FETCH NEXT FROM changeEvent
		INTO @employeeChangeEvent

		WHILE @@FETCH_STATUS = 0
		BEGIN
		EXEC [air].[dbo].[spChangeEvent] @employeeChangeEvent, @yearId
		FETCH NEXT FROM changeEvent
		INTO @employeeChangeEvent

		END
		CLOSE changeEvent
		DEALLOCATE changeEvent
END

GO

GRANT EXECUTE ON [dbo].[spUpdateAIR-InsuranceChangeEvents] TO [air-user] AS [dbo]
GO

CREATE PROCEDURE [dbo].[spUpdateAIR-MonthlyHoursAndStatus]
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

	DECLARE @tempCode TABLE (
	employee_id int,
	time_frame_id int,
	offer_of_coverage_code varchar(10),
	safe_harbor_code varchar(10)
);
WITH			
employees_in_payroll_during_year AS
(
	SELECT DISTINCT
			p.employee_id,
			p.employer_id,
			t.time_frame_id,
			t.month_id,
			t.year_id,
			ee.hireDate,
			ee.terminationDate, 
			ee.initialMeasurmentEnd,
			classification_id,
			1 AS on_payroll,
			ee.aca_status_id
		FROM
			aca.dbo.payroll p
			CROSS JOIN air.gen.time_frame  t
			INNER JOIN aca.dbo.employee ee ON (p.employee_id = ee.employee_id)
		WHERE 
			-- to reduce confusion we are setting all measurements up for 2016 reporting, 2015 measuring. gc5
			(p.edate > CONVERT(DATETIME,'2014-12-31 00:00:00', 102)) 
				AND
			-- to reduce confusion we are setting all measurements up for 2016 reporting, 2015 measuring. gc5
			(p.sdate < CONVERT(DATETIME,'2016-01-01 00:00:00', 102)) 
				AND
			(p.employer_id = @employerId)
				AND
			(t.year_id = @yearId)
				AND
			-- note: this will break when there is more than 10 million lives in the database.
			(ee.employee_id BETWEEN ISNULL(@employerId, 0) AND ISNULL(@employerId, 10000000))
),
	employees_in_ci_not_in_payroll_in_year AS
	(
		SELECT DISTINCT
			ee.employee_id,
			ee.employer_id,
			t.time_frame_id,
			t.month_id,
			t.year_id,
			ee.hireDate,
			ee.terminationDate, 
			ee.initialMeasurmentEnd,
			ee.classification_id,
			0 AS on_payroll,
			ee.aca_status_id
		FROM
			aca.dbo.employee ee
			INNER JOIN air.emp.covered_individual ci ON (ee.employee_id = ci.employee_id)
			CROSS JOIN air.gen.time_frame  t
			LEFT OUTER JOIN employees_in_payroll_during_year p ON (ee.employee_id = p.employee_id) 
		WHERE 
			(ee.employer_id = @employerId)
				AND
			(t.year_id = @yearId)
				AND
			(p.employee_id IS NULL)
				AND
			-- note: this will break when there is more than 10 million lives in the database.
			(ee.employee_id BETWEEN ISNULL(@employerId, 0) AND ISNULL(@employerId, 10000000))
	),
	employees_in_year AS
	(
		SELECT DISTINCT
			employee_id,
			employer_id,
			time_frame_id,
			month_id,
			year_id,
			hireDate,
			terminationDate, 
			initialMeasurmentEnd,
			classification_id,
			on_payroll,
			aca_status_id
		FROM
			employees_in_payroll_during_year

		UNION

		SELECT DISTINCT
			employee_id,
			employer_id,
			time_frame_id,
			month_id,
			year_id,
			hireDate,
			terminationDate, 
			initialMeasurmentEnd,
			classification_id,
			on_payroll,
			aca_status_id
		FROM
			employees_in_ci_not_in_payroll_in_year
	),
	employees_monthly_aggregates AS
	(
		SELECT
			ey.employee_id,
			ey.time_frame_id,
			ey.employer_id,
			mh.monthly_hours,
			emd.monthly_status_id, 
			ey.hireDate,
			ey.terminationDate,
			ey.initialMeasurmentEnd,
			mi.slcmp AS share_lowest_cost_monthly_premium, 
			CASE
				WHEN mi.offeredOn IS NOT NULL THEN 1
				ELSE 0 
			END AS offered_coverage, 
			ISNULL(accepted,0) AS enrolled,
			mi.offeredOn,
			mi.acceptedOn,
			mi.effectiveDate,
			mi.minValue,
			mi.offSpouse,
			mi.offDependent,
			mi.insurance_type_id, 
			mi.contribution_id,
			ec.ash_code,
			on_payroll,
			ey.aca_status_id,
			mi.monthly_flex,
			SpouseConditional
		FROM
			employees_in_year ey
			LEFT OUTER JOIN air.etl.ufnEmployeeMonthlyHours(@employerId, @yearId) mh ON (ey.employee_id = mh.employee_id) AND (ey.time_frame_id = mh.time_frame_id)
			LEFT OUTER JOIN air.etl.ufnEmployeeMonthlyInsurance(@employerId, @yearId) mi ON (ey.employee_id = mi.employee_id) AND ((ey.time_frame_id = mi.time_frame_id))
			LEFT OUTER JOIN aca.dbo.employee_classification	ec ON (ey.classification_id = ec.classification_id)
			INNER JOIN air.appr.employee_monthly_detail emd ON (ey.employee_id = emd.employee_id)
		GROUP BY
			ey.employee_id,
			ey.time_frame_id,
			ey.employer_id,
			mh.monthly_hours,
			calculated_sumer_hours,
			mh.time_frame_id,
			hireDate,
			ey.initialMeasurmentEnd,
			mi.slcmp,
			ey.terminationDate,
			offeredOn,
			acceptedOn,
			accepted,
			effectiveDate,
			mi.minValue,
			mi.offSpouse, 
			mi.offDependent,
			mi.insurance_type_id,
			mi.contribution_id,
			ec.ash_code,
			on_payroll,
			aca_status_id,
			mi.monthly_flex,
			emd.monthly_status_id,
			SpouseConditional
	)
	INSERT INTO 
		@tempCode (
			employee_id, 
			time_frame_id, 
			offer_of_coverage_code, 
			safe_harbor_code)
	SELECT
		employee_id,
		time_frame_id,
		air.etl.ufnGetMecCode(time_frame_id, offered_coverage, offSpouse, offDependent, minValue, IIF(contribution_id = '%', 1, 0), effectiveDate, terminationDate, aca_status_id, SpouseConditional) AS offer_of_coverage_code,
		air.etl.ufnGetSafeHarborCode(monthly_status_id, offered_coverage, enrolled, terminationDate, aca_status_id, ash_code) AS safe_harbor_code
	FROM
		employees_monthly_aggregates ema

	UPDATE air.appr.employee_monthly_detail
	SET offer_of_coverage_code = tc.offer_of_coverage_code, safe_harbor_code = tc.safe_harbor_code
	FROM air.appr.employee_monthly_detail emd INNER JOIN @tempCode tc
		ON emd.employee_id = tc.employee_id AND emd.time_frame_id = tc.time_frame_id

END

GO

GRANT EXECUTE ON [dbo].[spUpdateAIR-MonthlyHoursAndStatus] TO [air-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ______________________________________________________________________________________________________________________________________________________
-- Author:		Scott Harvey
-- Create date: 4/17/2016
-- Description:	<Description,,>
-- ______________________________________________________________________________________________________________________________________________________
ALTER PROCEDURE [appr].[spUpdate_1095C_status]
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
--:Parameters
@employer_id INT, 
@year_id INT,
@employee_id INT = NULL

-- ______________________________________________________________________________________________________________________________________________________
AS
-- ______________________________________________________________________________________________________________________________________________________
--:Variables
--:None presently
-- ______________________________________________________________________________________________________________________________________________________
-- ______________________________________________________________________________________________________________________________________________________
UPDATE air.appr.employee_yearly_detail
SET
	submittal_ready = 0,
	_1095C = 0
FROM
	air.appr.employee_yearly_detail yd
WHERE
	(yd.employer_id = @employer_id)
		AND
	(yd.year_id = @year_id)
		AND 
	-- note: this will break when there is more than 10 million lives in the database.
	(yd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

UPDATE air.appr.employee_yearly_detail
SET
	submittal_ready = 1,
	_1095C = 1
FROM
	air.appr.employee_yearly_detail yd
WHERE
	yd.employee_id IN (
			SELECT DISTINCT employee_id
			FROM air.appr.employee_monthly_detail md 
			WHERE
				((monthly_status_id = 1) OR (insurance_type_id = 2 AND enrolled = 1))
					AND
				(md.employee_id = employee_id)
	)
		OR
	yd.employee_id IN (
		SELECT DISTINCT employee_id
		FROM air.appr.employee_monthly_detail
		WHERE
			(enrolled = 0)
				AND
			(employee_id IN(SELECT DISTINCT employee_id FROM air.emp.covered_individual))
	)
		AND 
	(yd.employer_id = @employer_id)
		AND
	(yd.year_id = @year_id)
		AND 
	-- note: this will break when there is more than 10 million lives in the database.
	(yd.employee_id BETWEEN ISNULL(@employee_id, 0) AND ISNULL(@employee_id, 10000000));

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
	
			EXECUTE [air].[dbo].[spUpdateAIR-MonthlyHoursAndStatus] @employerId, @yearId
				PRINT '1 *** Updating hours from the MP/IMP and redetermining the monthly status. ***'
				PRINT ''
				PRINT ''
			EXECUTE [air].[dbo].[spUpdateAIR-InsuranceChangeEvents] @employerId, @yearId
				PRINT '2 *** Updating the insurance information based on change events. ***'
				PRINT ''
				PRINT ''
			EXECUTE [air].[appr].[spUpdate_1095C_status] @employerId, @yearId
				PRINT '3 *** Flagging the forms that should be presented on the 1095 screens. ***'
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

	-- start with the emp table first. gc5

	UPDATE 
		air.emp.monthly_detail
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

	DECLARE @tempCodeEmp TABLE (
	employee_id int,
	time_frame_id int,
	offer_of_coverage_code varchar(10),
	safe_harbor_code varchar(10)
);
WITH			
employees_in_payroll_during_year AS
(
	SELECT DISTINCT
			p.employee_id,
			p.employer_id,
			t.time_frame_id,
			t.month_id,
			t.year_id,
			ee.hireDate,
			ee.terminationDate, 
			ee.initialMeasurmentEnd,
			classification_id,
			1 AS on_payroll,
			ee.aca_status_id
		FROM
			aca.dbo.payroll p
			CROSS JOIN air.gen.time_frame  t
			INNER JOIN aca.dbo.employee ee ON (p.employee_id = ee.employee_id)
		WHERE 
			-- to reduce confusion we are setting all measurements up for 2016 reporting, 2015 measuring. gc5
			(p.edate > CONVERT(DATETIME,'2014-12-31 00:00:00', 102)) 
				AND
			-- to reduce confusion we are setting all measurements up for 2016 reporting, 2015 measuring. gc5
			(p.sdate < CONVERT(DATETIME,'2016-01-01 00:00:00', 102)) 
				AND
			(p.employer_id = @employerId)
				AND
			(t.year_id = @yearId)
				AND
			-- note: this will break when there is more than 10 million lives in the database.
			(ee.employee_id BETWEEN ISNULL(@employerId, 0) AND ISNULL(@employerId, 10000000))
),
	employees_in_ci_not_in_payroll_in_year AS
	(
		SELECT DISTINCT
			ee.employee_id,
			ee.employer_id,
			t.time_frame_id,
			t.month_id,
			t.year_id,
			ee.hireDate,
			ee.terminationDate, 
			ee.initialMeasurmentEnd,
			ee.classification_id,
			0 AS on_payroll,
			ee.aca_status_id
		FROM
			aca.dbo.employee ee
			INNER JOIN air.emp.covered_individual ci ON (ee.employee_id = ci.employee_id)
			CROSS JOIN air.gen.time_frame  t
			LEFT OUTER JOIN employees_in_payroll_during_year p ON (ee.employee_id = p.employee_id) 
		WHERE 
			(ee.employer_id = @employerId)
				AND
			(t.year_id = @yearId)
				AND
			(p.employee_id IS NULL)
				AND
			-- note: this will break when there is more than 10 million lives in the database.
			(ee.employee_id BETWEEN ISNULL(@employerId, 0) AND ISNULL(@employerId, 10000000))
	),
	employees_in_year AS
	(
		SELECT DISTINCT
			employee_id,
			employer_id,
			time_frame_id,
			month_id,
			year_id,
			hireDate,
			terminationDate, 
			initialMeasurmentEnd,
			classification_id,
			on_payroll,
			aca_status_id
		FROM
			employees_in_payroll_during_year

		UNION

		SELECT DISTINCT
			employee_id,
			employer_id,
			time_frame_id,
			month_id,
			year_id,
			hireDate,
			terminationDate, 
			initialMeasurmentEnd,
			classification_id,
			on_payroll,
			aca_status_id
		FROM
			employees_in_ci_not_in_payroll_in_year
	),
	employees_monthly_aggregates AS
	(
		SELECT
			ey.employee_id,
			ey.time_frame_id,
			ey.employer_id,
			mh.monthly_hours,
			emd.monthly_status_id, 
			ey.hireDate,
			ey.terminationDate,
			ey.initialMeasurmentEnd,
			mi.slcmp AS share_lowest_cost_monthly_premium, 
			CASE
				WHEN mi.offeredOn IS NOT NULL THEN 1
				ELSE 0 
			END AS offered_coverage, 
			ISNULL(accepted,0) AS enrolled,
			mi.offeredOn,
			mi.acceptedOn,
			mi.effectiveDate,
			mi.minValue,
			mi.offSpouse,
			mi.offDependent,
			mi.insurance_type_id, 
			mi.contribution_id,
			ec.ash_code,
			on_payroll,
			ey.aca_status_id,
			mi.monthly_flex,
			SpouseConditional
		FROM
			employees_in_year ey
			LEFT OUTER JOIN air.etl.ufnEmployeeMonthlyHours(@employerId, @yearId) mh ON (ey.employee_id = mh.employee_id) AND (ey.time_frame_id = mh.time_frame_id)
			LEFT OUTER JOIN air.etl.ufnEmployeeMonthlyInsurance(@employerId, @yearId) mi ON (ey.employee_id = mi.employee_id) AND ((ey.time_frame_id = mi.time_frame_id))
			LEFT OUTER JOIN aca.dbo.employee_classification	ec ON (ey.classification_id = ec.classification_id)
			INNER JOIN air.appr.employee_monthly_detail emd ON (ey.employee_id = emd.employee_id)
		GROUP BY
			ey.employee_id,
			ey.time_frame_id,
			ey.employer_id,
			mh.monthly_hours,
			calculated_sumer_hours,
			mh.time_frame_id,
			hireDate,
			ey.initialMeasurmentEnd,
			mi.slcmp,
			ey.terminationDate,
			offeredOn,
			acceptedOn,
			accepted,
			effectiveDate,
			mi.minValue,
			mi.offSpouse, 
			mi.offDependent,
			mi.insurance_type_id,
			mi.contribution_id,
			ec.ash_code,
			on_payroll,
			aca_status_id,
			mi.monthly_flex,
			emd.monthly_status_id,
			SpouseConditional
	)
	INSERT INTO 
		@tempCodeEmp (
			employee_id, 
			time_frame_id, 
			offer_of_coverage_code, 
			safe_harbor_code)
	SELECT
		employee_id,
		time_frame_id,
		air.etl.ufnGetMecCode(time_frame_id, offered_coverage, offSpouse, offDependent, minValue, IIF(contribution_id = '%', 1, 0), effectiveDate, terminationDate, aca_status_id, SpouseConditional) AS offer_of_coverage_code,
		air.etl.ufnGetSafeHarborCode(monthly_status_id, offered_coverage, enrolled, terminationDate, aca_status_id, ash_code) AS safe_harbor_code
	FROM
		employees_monthly_aggregates ema

	UPDATE
		air.emp.monthly_detail
	SET
		offer_of_coverage_code = tc.offer_of_coverage_code,
		safe_harbor_code = tc.safe_harbor_code
	FROM
		air.appr.employee_monthly_detail emd
		INNER JOIN @tempCodeEmp tc ON (emd.employee_id = tc.employee_id AND emd.time_frame_id = tc.time_frame_id)

-- appr table now. gc5, c&p for the time being.

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

	DECLARE @tempCodeAppr TABLE (
	employee_id int,
	time_frame_id int,
	offer_of_coverage_code varchar(10),
	safe_harbor_code varchar(10)
);
WITH			
employees_in_payroll_during_year AS
(
	SELECT DISTINCT
			p.employee_id,
			p.employer_id,
			t.time_frame_id,
			t.month_id,
			t.year_id,
			ee.hireDate,
			ee.terminationDate, 
			ee.initialMeasurmentEnd,
			classification_id,
			1 AS on_payroll,
			ee.aca_status_id
		FROM
			aca.dbo.payroll p
			CROSS JOIN air.gen.time_frame  t
			INNER JOIN aca.dbo.employee ee ON (p.employee_id = ee.employee_id)
		WHERE 
			-- to reduce confusion we are setting all measurements up for 2016 reporting, 2015 measuring. gc5
			(p.edate > CONVERT(DATETIME,'2014-12-31 00:00:00', 102)) 
				AND
			-- to reduce confusion we are setting all measurements up for 2016 reporting, 2015 measuring. gc5
			(p.sdate < CONVERT(DATETIME,'2016-01-01 00:00:00', 102)) 
				AND
			(p.employer_id = @employerId)
				AND
			(t.year_id = @yearId)
				AND
			-- note: this will break when there is more than 10 million lives in the database.
			(ee.employee_id BETWEEN ISNULL(@employerId, 0) AND ISNULL(@employerId, 10000000))
),
	employees_in_ci_not_in_payroll_in_year AS
	(
		SELECT DISTINCT
			ee.employee_id,
			ee.employer_id,
			t.time_frame_id,
			t.month_id,
			t.year_id,
			ee.hireDate,
			ee.terminationDate, 
			ee.initialMeasurmentEnd,
			ee.classification_id,
			0 AS on_payroll,
			ee.aca_status_id
		FROM
			aca.dbo.employee ee
			INNER JOIN air.emp.covered_individual ci ON (ee.employee_id = ci.employee_id)
			CROSS JOIN air.gen.time_frame  t
			LEFT OUTER JOIN employees_in_payroll_during_year p ON (ee.employee_id = p.employee_id) 
		WHERE 
			(ee.employer_id = @employerId)
				AND
			(t.year_id = @yearId)
				AND
			(p.employee_id IS NULL)
				AND
			-- note: this will break when there is more than 10 million lives in the database.
			(ee.employee_id BETWEEN ISNULL(@employerId, 0) AND ISNULL(@employerId, 10000000))
	),
	employees_in_year AS
	(
		SELECT DISTINCT
			employee_id,
			employer_id,
			time_frame_id,
			month_id,
			year_id,
			hireDate,
			terminationDate, 
			initialMeasurmentEnd,
			classification_id,
			on_payroll,
			aca_status_id
		FROM
			employees_in_payroll_during_year

		UNION

		SELECT DISTINCT
			employee_id,
			employer_id,
			time_frame_id,
			month_id,
			year_id,
			hireDate,
			terminationDate, 
			initialMeasurmentEnd,
			classification_id,
			on_payroll,
			aca_status_id
		FROM
			employees_in_ci_not_in_payroll_in_year
	),
	employees_monthly_aggregates AS
	(
		SELECT
			ey.employee_id,
			ey.time_frame_id,
			ey.employer_id,
			mh.monthly_hours,
			emd.monthly_status_id, 
			ey.hireDate,
			ey.terminationDate,
			ey.initialMeasurmentEnd,
			mi.slcmp AS share_lowest_cost_monthly_premium, 
			CASE
				WHEN mi.offeredOn IS NOT NULL THEN 1
				ELSE 0 
			END AS offered_coverage, 
			ISNULL(accepted,0) AS enrolled,
			mi.offeredOn,
			mi.acceptedOn,
			mi.effectiveDate,
			mi.minValue,
			mi.offSpouse,
			mi.offDependent,
			mi.insurance_type_id, 
			mi.contribution_id,
			ec.ash_code,
			on_payroll,
			ey.aca_status_id,
			mi.monthly_flex,
			SpouseConditional
		FROM
			employees_in_year ey
			LEFT OUTER JOIN air.etl.ufnEmployeeMonthlyHours(@employerId, @yearId) mh ON (ey.employee_id = mh.employee_id) AND (ey.time_frame_id = mh.time_frame_id)
			LEFT OUTER JOIN air.etl.ufnEmployeeMonthlyInsurance(@employerId, @yearId) mi ON (ey.employee_id = mi.employee_id) AND ((ey.time_frame_id = mi.time_frame_id))
			LEFT OUTER JOIN aca.dbo.employee_classification	ec ON (ey.classification_id = ec.classification_id)
			INNER JOIN air.appr.employee_monthly_detail emd ON (ey.employee_id = emd.employee_id)
		GROUP BY
			ey.employee_id,
			ey.time_frame_id,
			ey.employer_id,
			mh.monthly_hours,
			calculated_sumer_hours,
			mh.time_frame_id,
			hireDate,
			ey.initialMeasurmentEnd,
			mi.slcmp,
			ey.terminationDate,
			offeredOn,
			acceptedOn,
			accepted,
			effectiveDate,
			mi.minValue,
			mi.offSpouse, 
			mi.offDependent,
			mi.insurance_type_id,
			mi.contribution_id,
			ec.ash_code,
			on_payroll,
			aca_status_id,
			mi.monthly_flex,
			emd.monthly_status_id,
			SpouseConditional
	)
	INSERT INTO 
		@tempCodeAppr (
			employee_id, 
			time_frame_id, 
			offer_of_coverage_code, 
			safe_harbor_code)
	SELECT
		employee_id,
		time_frame_id,
		air.etl.ufnGetMecCode(time_frame_id, offered_coverage, offSpouse, offDependent, minValue, IIF(contribution_id = '%', 1, 0), effectiveDate, terminationDate, aca_status_id, SpouseConditional) AS offer_of_coverage_code,
		air.etl.ufnGetSafeHarborCode(monthly_status_id, offered_coverage, enrolled, terminationDate, aca_status_id, ash_code) AS safe_harbor_code
	FROM
		employees_monthly_aggregates ema

	UPDATE
		air.appr.employee_monthly_detail
	SET
		offer_of_coverage_code = tc.offer_of_coverage_code,
		safe_harbor_code = tc.safe_harbor_code
	FROM
		air.appr.employee_monthly_detail emd
		INNER JOIN @tempCodeAppr tc ON (emd.employee_id = tc.employee_id AND emd.time_frame_id = tc.time_frame_id)

END

GO

-- adding back the imp/mp deduplication logic. gc5

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

	-- start with the emp table first. gc5

	UPDATE 
		air.emp.monthly_detail
	SET monthly_hours = ad.MonthlyAverageHours, 
		monthly_status_id = CASE
								WHEN ISNULL(ad.MonthlyAverageHours, 0) = 0 THEN 7
								WHEN ad.MonthlyAverageHours > 129.99 THEN 1
								WHEN ad.MonthlyAverageHours < 130 THEN 2
							END
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

	DECLARE @tempCodeEmp TABLE (
	employee_id int,
	time_frame_id int,
	offer_of_coverage_code varchar(10),
	safe_harbor_code varchar(10)
);
WITH			
employees_in_payroll_during_year AS
(
	SELECT DISTINCT
			p.employee_id,
			p.employer_id,
			t.time_frame_id,
			t.month_id,
			t.year_id,
			ee.hireDate,
			ee.terminationDate, 
			ee.initialMeasurmentEnd,
			classification_id,
			1 AS on_payroll,
			ee.aca_status_id
		FROM
			aca.dbo.payroll p
			CROSS JOIN air.gen.time_frame  t
			INNER JOIN aca.dbo.employee ee ON (p.employee_id = ee.employee_id)
		WHERE 
			-- to reduce confusion we are setting all measurements up for 2016 reporting, 2015 measuring. gc5
			(p.edate > CONVERT(DATETIME,'2014-12-31 00:00:00', 102)) 
				AND
			-- to reduce confusion we are setting all measurements up for 2016 reporting, 2015 measuring. gc5
			(p.sdate < CONVERT(DATETIME,'2016-01-01 00:00:00', 102)) 
				AND
			(p.employer_id = @employerId)
				AND
			(t.year_id = @yearId)
				AND
			-- note: this will break when there is more than 10 million lives in the database.
			(ee.employee_id BETWEEN ISNULL(@employerId, 0) AND ISNULL(@employerId, 10000000))
),
	employees_in_ci_not_in_payroll_in_year AS
	(
		SELECT DISTINCT
			ee.employee_id,
			ee.employer_id,
			t.time_frame_id,
			t.month_id,
			t.year_id,
			ee.hireDate,
			ee.terminationDate, 
			ee.initialMeasurmentEnd,
			ee.classification_id,
			0 AS on_payroll,
			ee.aca_status_id
		FROM
			aca.dbo.employee ee
			INNER JOIN air.emp.covered_individual ci ON (ee.employee_id = ci.employee_id)
			CROSS JOIN air.gen.time_frame  t
			LEFT OUTER JOIN employees_in_payroll_during_year p ON (ee.employee_id = p.employee_id) 
		WHERE 
			(ee.employer_id = @employerId)
				AND
			(t.year_id = @yearId)
				AND
			(p.employee_id IS NULL)
				AND
			-- note: this will break when there is more than 10 million lives in the database.
			(ee.employee_id BETWEEN ISNULL(@employerId, 0) AND ISNULL(@employerId, 10000000))
	),
	employees_in_year AS
	(
		SELECT DISTINCT
			employee_id,
			employer_id,
			time_frame_id,
			month_id,
			year_id,
			hireDate,
			terminationDate, 
			initialMeasurmentEnd,
			classification_id,
			on_payroll,
			aca_status_id
		FROM
			employees_in_payroll_during_year

		UNION

		SELECT DISTINCT
			employee_id,
			employer_id,
			time_frame_id,
			month_id,
			year_id,
			hireDate,
			terminationDate, 
			initialMeasurmentEnd,
			classification_id,
			on_payroll,
			aca_status_id
		FROM
			employees_in_ci_not_in_payroll_in_year
	),
	employees_monthly_aggregates AS
	(
		SELECT
			ey.employee_id,
			ey.time_frame_id,
			ey.employer_id,
			mh.monthly_hours,
			emd.monthly_status_id, 
			ey.hireDate,
			ey.terminationDate,
			ey.initialMeasurmentEnd,
			mi.slcmp AS share_lowest_cost_monthly_premium, 
			CASE
				WHEN mi.offeredOn IS NOT NULL THEN 1
				ELSE 0 
			END AS offered_coverage, 
			ISNULL(accepted,0) AS enrolled,
			mi.offeredOn,
			mi.acceptedOn,
			mi.effectiveDate,
			mi.minValue,
			mi.offSpouse,
			mi.offDependent,
			mi.insurance_type_id, 
			mi.contribution_id,
			ec.ash_code,
			on_payroll,
			ey.aca_status_id,
			mi.monthly_flex,
			SpouseConditional
		FROM
			employees_in_year ey
			LEFT OUTER JOIN air.etl.ufnEmployeeMonthlyHours(@employerId, @yearId) mh ON (ey.employee_id = mh.employee_id) AND (ey.time_frame_id = mh.time_frame_id)
			LEFT OUTER JOIN air.etl.ufnEmployeeMonthlyInsurance(@employerId, @yearId) mi ON (ey.employee_id = mi.employee_id) AND ((ey.time_frame_id = mi.time_frame_id))
			LEFT OUTER JOIN aca.dbo.employee_classification	ec ON (ey.classification_id = ec.classification_id)
			INNER JOIN air.emp.monthly_detail emd ON (ey.employee_id = emd.employee_id)
		GROUP BY
			ey.employee_id,
			ey.time_frame_id,
			ey.employer_id,
			mh.monthly_hours,
			calculated_sumer_hours,
			mh.time_frame_id,
			hireDate,
			ey.initialMeasurmentEnd,
			mi.slcmp,
			ey.terminationDate,
			offeredOn,
			acceptedOn,
			accepted,
			effectiveDate,
			mi.minValue,
			mi.offSpouse, 
			mi.offDependent,
			mi.insurance_type_id,
			mi.contribution_id,
			ec.ash_code,
			on_payroll,
			aca_status_id,
			mi.monthly_flex,
			emd.monthly_status_id,
			SpouseConditional
	)
	INSERT INTO 
		@tempCodeEmp (
			employee_id, 
			time_frame_id, 
			offer_of_coverage_code, 
			safe_harbor_code)
	SELECT
		employee_id,
		time_frame_id,
		air.etl.ufnGetMecCode(time_frame_id, offered_coverage, offSpouse, offDependent, minValue, IIF(contribution_id = '%', 1, 0), effectiveDate, terminationDate, aca_status_id, SpouseConditional) AS offer_of_coverage_code,
		air.etl.ufnGetSafeHarborCode(monthly_status_id, offered_coverage, enrolled, terminationDate, aca_status_id, ash_code) AS safe_harbor_code
	FROM
		employees_monthly_aggregates ema

	UPDATE
		air.emp.monthly_detail
	SET
		offer_of_coverage_code = tc.offer_of_coverage_code,
		safe_harbor_code = tc.safe_harbor_code
	FROM
		air.emp.monthly_detail emd
		INNER JOIN @tempCodeEmp tc ON (emd.employee_id = tc.employee_id AND emd.time_frame_id = tc.time_frame_id)

-- appr table now. gc5, c&p for the time being.

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

			) ad ON ad.EmployeeId = emd.employee_id 
		INNER JOIN air.gen.time_frame tf ON emd.time_frame_id = tf.time_frame_id
	WHERE
		tf.year_id = @yearId
			AND
		emd.employer_id =  @employerId

	DECLARE @tempCodeAppr TABLE (
	employee_id int,
	time_frame_id int,
	offer_of_coverage_code varchar(10),
	safe_harbor_code varchar(10)
);
WITH			
employees_in_payroll_during_year AS
(
	SELECT DISTINCT
			p.employee_id,
			p.employer_id,
			t.time_frame_id,
			t.month_id,
			t.year_id,
			ee.hireDate,
			ee.terminationDate, 
			ee.initialMeasurmentEnd,
			classification_id,
			1 AS on_payroll,
			ee.aca_status_id
		FROM
			aca.dbo.payroll p
			CROSS JOIN air.gen.time_frame  t
			INNER JOIN aca.dbo.employee ee ON (p.employee_id = ee.employee_id)
		WHERE 
			-- to reduce confusion we are setting all measurements up for 2016 reporting, 2015 measuring. gc5
			(p.edate > CONVERT(DATETIME,'2014-12-31 00:00:00', 102)) 
				AND
			-- to reduce confusion we are setting all measurements up for 2016 reporting, 2015 measuring. gc5
			(p.sdate < CONVERT(DATETIME,'2016-01-01 00:00:00', 102)) 
				AND
			(p.employer_id = @employerId)
				AND
			(t.year_id = @yearId)
				AND
			-- note: this will break when there is more than 10 million lives in the database.
			(ee.employee_id BETWEEN ISNULL(@employerId, 0) AND ISNULL(@employerId, 10000000))
),
	employees_in_ci_not_in_payroll_in_year AS
	(
		SELECT DISTINCT
			ee.employee_id,
			ee.employer_id,
			t.time_frame_id,
			t.month_id,
			t.year_id,
			ee.hireDate,
			ee.terminationDate, 
			ee.initialMeasurmentEnd,
			ee.classification_id,
			0 AS on_payroll,
			ee.aca_status_id
		FROM
			aca.dbo.employee ee
			INNER JOIN air.emp.covered_individual ci ON (ee.employee_id = ci.employee_id)
			CROSS JOIN air.gen.time_frame  t
			LEFT OUTER JOIN employees_in_payroll_during_year p ON (ee.employee_id = p.employee_id) 
		WHERE 
			(ee.employer_id = @employerId)
				AND
			(t.year_id = @yearId)
				AND
			(p.employee_id IS NULL)
				AND
			-- note: this will break when there is more than 10 million lives in the database.
			(ee.employee_id BETWEEN ISNULL(@employerId, 0) AND ISNULL(@employerId, 10000000))
	),
	employees_in_year AS
	(
		SELECT DISTINCT
			employee_id,
			employer_id,
			time_frame_id,
			month_id,
			year_id,
			hireDate,
			terminationDate, 
			initialMeasurmentEnd,
			classification_id,
			on_payroll,
			aca_status_id
		FROM
			employees_in_payroll_during_year

		UNION

		SELECT DISTINCT
			employee_id,
			employer_id,
			time_frame_id,
			month_id,
			year_id,
			hireDate,
			terminationDate, 
			initialMeasurmentEnd,
			classification_id,
			on_payroll,
			aca_status_id
		FROM
			employees_in_ci_not_in_payroll_in_year
	),
	employees_monthly_aggregates AS
	(
		SELECT
			ey.employee_id,
			ey.time_frame_id,
			ey.employer_id,
			mh.monthly_hours,
			emd.monthly_status_id, 
			ey.hireDate,
			ey.terminationDate,
			ey.initialMeasurmentEnd,
			mi.slcmp AS share_lowest_cost_monthly_premium, 
			CASE
				WHEN mi.offeredOn IS NOT NULL THEN 1
				ELSE 0 
			END AS offered_coverage, 
			ISNULL(accepted,0) AS enrolled,
			mi.offeredOn,
			mi.acceptedOn,
			mi.effectiveDate,
			mi.minValue,
			mi.offSpouse,
			mi.offDependent,
			mi.insurance_type_id, 
			mi.contribution_id,
			ec.ash_code,
			on_payroll,
			ey.aca_status_id,
			mi.monthly_flex,
			SpouseConditional
		FROM
			employees_in_year ey
			LEFT OUTER JOIN air.etl.ufnEmployeeMonthlyHours(@employerId, @yearId) mh ON (ey.employee_id = mh.employee_id) AND (ey.time_frame_id = mh.time_frame_id)
			LEFT OUTER JOIN air.etl.ufnEmployeeMonthlyInsurance(@employerId, @yearId) mi ON (ey.employee_id = mi.employee_id) AND ((ey.time_frame_id = mi.time_frame_id))
			LEFT OUTER JOIN aca.dbo.employee_classification	ec ON (ey.classification_id = ec.classification_id)
			INNER JOIN air.appr.employee_monthly_detail emd ON (ey.employee_id = emd.employee_id)
		GROUP BY
			ey.employee_id,
			ey.time_frame_id,
			ey.employer_id,
			mh.monthly_hours,
			calculated_sumer_hours,
			mh.time_frame_id,
			hireDate,
			ey.initialMeasurmentEnd,
			mi.slcmp,
			ey.terminationDate,
			offeredOn,
			acceptedOn,
			accepted,
			effectiveDate,
			mi.minValue,
			mi.offSpouse, 
			mi.offDependent,
			mi.insurance_type_id,
			mi.contribution_id,
			ec.ash_code,
			on_payroll,
			aca_status_id,
			mi.monthly_flex,
			emd.monthly_status_id,
			SpouseConditional
	)
	INSERT INTO 
		@tempCodeAppr (
			employee_id, 
			time_frame_id, 
			offer_of_coverage_code, 
			safe_harbor_code)
	SELECT
		employee_id,
		time_frame_id,
		air.etl.ufnGetMecCode(time_frame_id, offered_coverage, offSpouse, offDependent, minValue, IIF(contribution_id = '%', 1, 0), effectiveDate, terminationDate, aca_status_id, SpouseConditional) AS offer_of_coverage_code,
		air.etl.ufnGetSafeHarborCode(monthly_status_id, offered_coverage, enrolled, terminationDate, aca_status_id, ash_code) AS safe_harbor_code
	FROM
		employees_monthly_aggregates ema

	UPDATE
		air.appr.employee_monthly_detail
	SET
		offer_of_coverage_code = tc.offer_of_coverage_code,
		safe_harbor_code = tc.safe_harbor_code
	FROM
		air.appr.employee_monthly_detail emd
		INNER JOIN @tempCodeAppr tc ON (emd.employee_id = tc.employee_id AND emd.time_frame_id = tc.time_frame_id)

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
	
			EXECUTE [air].[dbo].[spUpdateAIR-MonthlyHoursAndStatus] @employerId, @yearId
				PRINT '1 *** Updating hours from the MP/IMP and redetermining the monthly status. ***'
				PRINT ''
				PRINT ''
			--EXECUTE [air].[dbo].[spUpdateAIR-InsuranceChangeEvents] @employerId, @yearId
			--	PRINT '2 *** Updating the insurance information based on change events. ***'
			--	PRINT ''
			--	PRINT ''
			EXECUTE [air].[appr].[spUpdate_1095C_status] @employerId, @yearId
				PRINT '3 *** Flagging the forms that should be presented on the 1095 screens. ***'
				PRINT ''
				PRINT ''


END

GO

