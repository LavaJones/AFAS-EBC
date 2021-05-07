USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[ufnGetPlanYearIdForTimeFrame] (
		@employerId INT,
		@yearId SMALLINT,
		@monthId SMALLINT
	)
RETURNS INT
AS
BEGIN

	DECLARE @planYearId INT

	SELECT
		@planYearId = py.plan_year_id
	FROM
		[dbo].[plan_year] py
		INNER JOIN [dbo].[employer] er ON (er.employer_id = py.employer_id)
	WHERE
		DATEFROMPARTS(@yearId, @monthId, 1) BETWEEN py.startDate AND py.endDate
			AND
		er.employer_id = @employerId

	RETURN @planYearId

END

GO

GRANT EXECUTE ON  [dbo].[ufnGetPlanYearIdForTimeFrame] TO [aca-user] AS DBO
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spInsertEmployeeInsuranceOfferEditable]
	@employerId int,
	@yearId int
AS
BEGIN

	SET NOCOUNT ON

	DELETE [dbo].[EmployeeInsuranceOfferEditable]
	WHERE
		EmployerId = @employerId
			AND
		TaxYearId = @yearId;

	-- holds the union of the offer and offer_archive tables so we can rebuild the history of the offers.
	DECLARE @insuranceStateCombined TABLE
	(
		dateRowId int NOT NULL,
		employer_id int NOT NULL,
		employee_id int NOT NULL,
		plan_year_id INT NOT NULL,
		offered bit NOT NULL,
		offeredOn datetime NOT NULL,
		accepted bit NOT NULL,
		acceptedOn datetime NOT NULL,
		insurance_id INT NULL,
		ins_cont_id INT NULL,
		inEffect bit NOT NULL,
		effectiveDate datetime NOT NULL,
		hireDate datetime NOT NULL,
		terminationDate datetime NULL,
		hra_flex_contribution INT NULL
	);

	WITH insuranceCombined
	AS (
		SELECT
			rowid,
			a.employer_id,
			employee_id,
			a.plan_year_id,
			offered,
			offeredOn,
			accepted,
			acceptedOn,
			effectiveDate,
			insurance_id,
			ins_cont_id,
			avg_hours_month,
			hireDate,
			terminationDate,
			hra_flex_contribution,
			InOffer,
			InOfferArchive
		FROM (
			SELECT
				eioa.rowid,
				eioa.employer_id,
				eioa.employee_id,
				eioa.plan_year_id,
				eioa.offered,
				eioa.offeredOn,
				ISNULL(eioa.accepted, 0) AS accepted,
				ISNULL(eioa.acceptedOn, eioa.offeredOn) AS acceptedOn,
				ISNULL(eioa.effectiveDate, ISNULL(eioa.acceptedOn, eioa.offeredOn)) AS effectiveDate,
				eioa.insurance_id,
				eioa.ins_cont_id,
				eioa.avg_hours_month,
				ee.hireDate,
				ee.terminationDate,
				eioa.hra_flex_contribution,
				0 AS [InOffer],
				1 AS [InOfferArchive]
			FROM
				[dbo].[employee_insurance_offer_archive] eioa
				INNER JOIN [dbo].[employee] ee ON (ee.employee_id = eioa.employee_id)
			WHERE
				eioa.employer_id = @employerId

			UNION

			SELECT
				eio.rowid,
				eio.employer_id,
				eio.employee_id,
				eio.plan_year_id,
				eio.offered,
				eio.offeredOn,
				ISNULL(eio.accepted, 0) AS accepted,
				ISNULL(eio.acceptedOn, eio.offeredOn) AS acceptedOn,
				ISNULL(eio.effectiveDate, ISNULL(eio.acceptedOn, eio.offeredOn)) AS effectiveDate,
				eio.insurance_id,
				eio.ins_cont_id,
				eio.avg_hours_month,
				ee.hireDate,
				ee.terminationDate,
				eio.hra_flex_contribution,
				1 AS [InOffer],
				0 AS [InOfferArchive]
			FROM
				[dbo].[employee_insurance_offer] eio
				INNER JOIN [dbo].[employee] ee ON (ee.employee_id = eio.employee_id)
			WHERE
				eio.employer_id = @employerId
			) AS A
			INNER JOIN [dbo].[plan_year] py ON (py.plan_year_id = a.plan_year_id)
		WHERE
			a.employer_id = @employerId
				AND
			(
				-- these may not be needed anymore, gc5
				py.endDate <= '2016-12-31'
					OR
				py.startDate >= '2015-01-01'
			)
				AND
			(
				-- getting weird dates here from near future events, removing to be safe. gc5
				offeredOn <= '2016-12-31'
					AND
				acceptedOn <= '2016-12-31'
			)
	),

	getCombinedDateData
	AS (

		SELECT 
			ROW_NUMBER() OVER(ORDER BY ic.offeredOn) as dateRowId,
			ic.employer_id,
			ic.employee_id,
			ic.plan_year_id,
			ic.offered,
			ic.offeredOn,
			ic.accepted,
			ic.acceptedOn,
			ic.insurance_id,
			ic.ins_cont_id,
			0 AS inEffect,		 -- TODO: determine later when peeking at the term date and effective date. gc5
			ic.effectiveDate,
			ee.hireDate,
			ee.terminationDate,
			ic.hra_flex_contribution
		FROM
			insuranceCombined ic
			INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = ic.employee_id)
		WHERE
			ic.employer_id = @employerId
	)

	INSERT INTO @insuranceStateCombined (
		dateRowId,
		employer_id,
		employee_id,
		plan_year_id,
		offered,
		offeredOn,
		accepted,
		acceptedOn,
		insurance_id,
		ins_cont_id,
		inEffect,
		effectiveDate,
		hireDate,
		terminationDate,
		hra_flex_contribution
	)
	SELECT
		gcdd.dateRowId,
		gcdd.employer_id,
		gcdd.employee_id,
		gcdd.plan_year_id,
		gcdd.offered,
		gcdd.offeredOn,
		gcdd.accepted,
		gcdd.acceptedOn,
		gcdd.insurance_id,
		gcdd.ins_cont_id,
		gcdd.inEffect,
		gcdd.effectiveDate,
		gcdd.hireDate,
		gcdd.terminationDate,
		gcdd.hra_flex_contribution
	FROM
		getCombinedDateData gcdd
	WHERE 
		gcdd.offeredOn IS NOT NULL

	-- our state of offer table pivoted by month, default is everyone has no offer/coverage/acceptance.
	DECLARE @offerByMonth TABLE(
		[TaxYearId] [int] NOT NULL,
		[MonthId] [int] NOT NULL,
		[TimeFrameId] [int] NOT NULL,
		[EmployerId] [int] NOT NULL,
		[EmployeeId] [int] NOT NULL,
		[PlanYearId] [int] NOT NULL,
		[InsuranceId] [int] NULL,
		[InsuranceContributionId] [int] NULL,
		[OfferInForce] [bit] NOT NULL,
		[AcceptedInForce] [bit] NOT NULL,
		[CoverageInForce] [bit] NOT NULL,
		[HraFlexContribution] [int] NULL
	);

	-- ensure we create one row, per month for the whole tax year by employee
	WITH [months] AS
	(
		  SELECT 1 as [month_number]
		  UNION all
		  SELECT
			[month_number] + 1 as [month_number]
		  from
			[months]
		  where
			[month_number] < 12
	),
	[tax_year_data] AS
	(
		SELECT
			@yearId as tax_year_id,
			m.month_number as [month_id]
		FROM
			[months] m
	)

	-- insert a row for every employee regardless of status for every month during the tax year
	-- with all of the bits toggled off.
	INSERT INTO @offerByMonth (
		[TaxYearId],
		[MonthId],
		[TimeFrameId],
		[EmployerId],
		[EmployeeId],
		[PlanYearId],
		[InsuranceId],
		[InsuranceContributionId],
		[OfferInForce],
		[AcceptedInForce],
		[CoverageInForce],
		[HraFlexContribution]
	)
	SELECT
		tf.year_id,
		tf.month_id,
		tf.time_frame_id,
		ee.employer_id,
		ee.employee_id,
		[dbo].[ufnGetPlanYearIdForTimeFrame](@employerId, tf.year_id, tf.month_id), -- not ideal but the best I had at the moment. gc5
		NULL,	-- by default there is no offer/coverage present.
		NULL,	-- by default there is no offer/coverage present.
		0,		-- by default everyone is going to be false.
		0,		-- by default everyone is going to be false.
		0,		-- by default everyone is going to be false.
		NULL	-- safe value based on our previous understandings.
	FROM
		[air].[gen].[time_frame] tf
		INNER JOIN [tax_year_data] tyd ON (tyd.tax_year_id = tf.year_id AND tyd.month_id = tf.month_id)
		CROSS JOIN [aca].dbo.employee ee
	WHERE
		ee.employer_id = @employerId;

	DECLARE @distinctEmployeesWithOfferEntries TABLE (
		EmployeeId INT NOT NULL
	)

	INSERT INTO @distinctEmployeesWithOfferEntries (
		EmployeeId
	)
	SELECT DISTINCT
		isc.employee_id
	FROM
		@insuranceStateCombined isc;

	---- verify first with upstream.
	DECLARE @rowCount INT

	DECLARE @currentEmployeeId INT

	-- I hate to use this, but what choice is there? gc5
	DECLARE insuranceEvent CURSOR FOR
		SELECT EmployeeId FROM @distinctEmployeesWithOfferEntries

	OPEN insuranceEvent

	FETCH NEXT FROM insuranceEvent INTO @currentEmployeeId

	WHILE @@FETCH_STATUS = 0
	BEGIN

		SELECT
			@rowCount = COUNT(isc.employee_id)
		FROM
			@insuranceStateCombined isc
		WHERE isc.employee_id = @currentEmployeeId;

		-- looping in SQL, probably against a rule somewhere. gc5
		IF (@rowCount > 1)

			-- looping variables.
			DECLARE @minRow INT
			DECLARE @currentRow INT
			DECLARE @startingTimeFrame INT
			DECLARE @endingTimeFrame INT
			DECLARE @offered BIT
			DECLARE @accepted BIT
			DECLARE @inForce BIT
			DECLARE @offeredDateStart datetime
			DECLARE @offeredDateEnd datetime
			DECLARE @acceptedDateStart datetime
			DECLARE @acceptedDateEnd datetime
			DECLARE @coverageDateStart datetime
			DECLARE @coverageDateEnd datetime
			DECLARE @terminationDate datetime

			DECLARE @insuranceId INT
			DECLARE @insuranceContributionId INT

			DECLARE @hraFlexContribution INT

			SELECT @minRow = MIN(dateRowId) - 1 FROM @insuranceStateCombined WHERE employee_id = @currentEmployeeId;

			SELECT @rowCount = @rowCount - 1
			WHILE (@rowCount >= 0)
			BEGIN

				-- lets grab our current row key, we need it for every fetch from here on out. gc5
				SELECT
					@currentRow = MIN(isc.dateRowId)
				FROM
					@insuranceStateCombined isc
				WHERE
					isc.employee_id = @currentEmployeeId
						AND
					isc.dateRowId > @minRow;

				-----------------------------------------------------------------
				-- safe to grab data now since we know the exact row we want. gc5
				-----------------------------------------------------------------

				-- we need the termination date to determine the end of the bit flipping.
				SELECT
					@terminationDate = isc.terminationDate
				FROM
					@insuranceStateCombined isc
				WHERE
					isc.employee_id = @currentEmployeeId
						AND
					dateRowId = @currentRow;

				-- should be data driven, somehow. Sticking with 2016 reporting year until we pull everything from the @yearId gc5
				SELECT @offeredDateEnd = ISNULL(@terminationDate, '2016-12-31')
				SELECT @acceptedDateEnd = ISNULL(@terminationDate, '2016-12-31')
				SELECT @coverageDateEnd = ISNULL(@terminationDate, '2016-12-31')

				-- TODO: Be more nuianced on the term data handling.

				-- short circuit to the end of the reporting period or the termination date.
				SELECT @endingTimeFrame = [air].[etl].[ufnGetTimeFrameID](YEAR(@offeredDateEnd), MONTH(@offeredDateEnd))
		
				-- lets get the values so we toggle some offered bits later
				SELECT
					@offered = isc.offered,
					@offeredDateStart = isc.offeredOn
				FROM
					@insuranceStateCombined isc
				WHERE
					isc.employee_id = @currentEmployeeId
						AND
					dateRowId = @currentRow;

				SELECT @startingTimeFrame = [air].[etl].ufnGetTimeFrameID(YEAR(@offeredDateStart), MONTH(@offeredDateStart));
				IF (DAY(@offeredDateStart) <> 1) SELECT @startingTimeFrame = @startingTimeFrame + 1;		-- move to the next month.

				WITH [months] AS
				(
					  SELECT 1 as [month_number]
					  UNION all
					  SELECT
						[month_number] + 1 as [month_number]
					  from
						[months]
					  where
						[month_number] < 12
				),
				[tax_year_data] AS
				(
					SELECT
						@yearId as tax_year_id,
						m.month_number as [month_id]
					FROM
						[months] m
				)
				UPDATE @offerByMonth
				SET
					OfferInForce = @offered
				FROM
					@offerByMonth obm
					INNER JOIN tax_year_data tyd ON (obm.MonthId = tyd.month_id AND obm.TaxYearId = tyd.tax_year_id)
				WHERE
					EmployeeId = @currentEmployeeId
						AND
					(obm.TimeFrameId BETWEEN @startingTimeFrame AND @endingTimeFrame);

				-- TODO: Be more nuianced on the term data handling.

				-- short circuit to the end of the reporting period or the termination date.
				SELECT @endingTimeFrame = [air].[etl].[ufnGetTimeFrameID](YEAR(@acceptedDateEnd), MONTH(@acceptedDateEnd))
		
				-- lets toggle some accepted bits
				SELECT
					@accepted = isc.accepted,
					@acceptedDateStart = isc.acceptedOn
				FROM
					@insuranceStateCombined isc
				WHERE
					isc.employee_id = @currentEmployeeId
						AND
					dateRowId = @currentRow;

				SELECT @startingTimeFrame = [air].[etl].ufnGetTimeFrameID(YEAR(@acceptedDateStart), MONTH(@acceptedDateStart));
				IF (DAY(@acceptedDateStart) <> 1) SELECT @startingTimeFrame = @startingTimeFrame + 1;		-- move to the next month.

				WITH [months] AS
				(
					  SELECT 1 as [month_number]
					  UNION all
					  SELECT
						[month_number] + 1 as [month_number]
					  from
						[months]
					  where
						[month_number] < 12
				),
				[tax_year_data] AS
				(
					SELECT
						@yearId as tax_year_id,
						m.month_number as [month_id]
					FROM
						[months] m
				)
				UPDATE @offerByMonth
				SET
					AcceptedInForce = @accepted
				FROM
					@offerByMonth obm
					INNER JOIN tax_year_data tyd ON (obm.MonthId = tyd.month_id AND obm.TaxYearId = tyd.tax_year_id)
				WHERE
					EmployeeId = @currentEmployeeId
						AND
					(obm.TimeFrameId BETWEEN @startingTimeFrame AND @endingTimeFrame);

				-- TODO: Be more nuianced on the term data handling.

				-- short circuit to the end of the reporting period or the termination date.
				SELECT @endingTimeFrame = [air].[etl].[ufnGetTimeFrameID](YEAR(@coverageDateEnd), MONTH(@coverageDateEnd))
		
				-- lets toggle some offer coverage bits
				SELECT
					@coverageDateStart = isc.effectiveDate
				FROM
					@insuranceStateCombined isc
				WHERE
					isc.employee_id = @currentEmployeeId
						AND
					dateRowId = @currentRow;

				SELECT @startingTimeFrame = [air].[etl].ufnGetTimeFrameID(YEAR(@coverageDateStart), MONTH(@coverageDateStart));
				IF (DAY(@coverageDateStart) <> 1) SELECT @startingTimeFrame = @startingTimeFrame + 1;		-- move to the next month.

				WITH [months] AS
				(
					  SELECT 1 as [month_number]
					  UNION all
					  SELECT
						[month_number] + 1 as [month_number]
					  from
						[months]
					  where
						[month_number] < 12
				),
				[tax_year_data] AS
				(
					SELECT
						@yearId as tax_year_id,
						m.month_number as [month_id]
					FROM
						[months] m
				)
				UPDATE @offerByMonth
				SET
					-- we reuse the accepted toggle to handle waives and accepts. gc5
					CoverageInForce = @accepted
				FROM
					@offerByMonth obm
					INNER JOIN tax_year_data tyd ON (obm.MonthId = tyd.month_id AND obm.TaxYearId = tyd.tax_year_id)
				WHERE
					EmployeeId = @currentEmployeeId
						AND
					(obm.TimeFrameId BETWEEN @startingTimeFrame AND @endingTimeFrame);

				-- safe default answers.
				SELECT
					@insuranceId = NULL,
					@insuranceContributionId = NULL

				SELECT
					@hraFlexContribution = isc.hra_flex_contribution
				FROM
					@insuranceStateCombined isc
				WHERE
					isc.employee_id = @currentEmployeeId
						AND
					dateRowId = @currentRow;

				IF (@offered = 1)

					-- employee accepted something so we want real values.
					SELECT
						@insuranceId = isc.insurance_id,
						@insuranceContributionId = isc.ins_cont_id
					FROM
						@insuranceStateCombined isc
					WHERE
						isc.employee_id = @currentEmployeeId
							AND
						dateRowId = @currentRow;

				WITH [months] AS
				(
					SELECT 1 AS [month_number]
					UNION all
					SELECT
						[month_number] + 1 AS [month_number]
					FROM
						[months]
					WHERE
						[month_number] < 12
					),
				[tax_year_data] AS
				(
					SELECT
						@yearId AS tax_year_id,
						m.month_number as [month_id]
					FROM
						[months] m
				)
				UPDATE @offerByMonth
				SET
					InsuranceId = @insuranceId,
					InsuranceContributionId = @insuranceContributionId,
					HraFlexContribution = @hraFlexContribution
				FROM
					@offerByMonth obm
					INNER JOIN tax_year_data tyd ON (obm.MonthId = tyd.month_id AND obm.TaxYearId = tyd.tax_year_id)
				WHERE
					obm.EmployeeId = @currentEmployeeId
						AND
					(obm.TimeFrameId BETWEEN @startingTimeFrame AND @endingTimeFrame);
						
				-- now we move our loop counter up
				SELECT @minRow = @currentRow;

				-- do not forget to decrement the while counter, do not ask how I know. gc5
				SELECT @rowCount = @rowCount - 1

		END
		-- end of the many rows loop.
			
		-- done, cursor on
		FETCH NEXT FROM insuranceEvent INTO @currentEmployeeId

	END

	CLOSE insuranceEvent
	DEALLOCATE insuranceEvent

	INSERT INTO [dbo].[EmployeeInsuranceOfferEditable] (
		[TaxYearId],
		[MonthId],
		[TimeFrameId],
		[EmployerId],
		[EmployeeId],
		[PlanYearId],
		[InsuranceId],
		[InsuranceContributionId],
		[OfferInForce],
		[AcceptedInForce],
		[CoverageInForce],
		[HraFlexContribution]
	)
	SELECT 
		obm.[TaxYearId],
		obm.[MonthId],
		obm.[TimeFrameId],
		obm.[EmployerId],
		obm.[EmployeeId],
		obm.[PlanYearId],
		obm.[InsuranceId],
		obm.[InsuranceContributionId],
		obm.[OfferInForce],
		obm.[AcceptedInForce],
		obm.[CoverageInForce],
		obm.[HraFlexContribution]
	FROM	
		@offerByMonth obm
	WHERE
		obm.EmployerId = @employerId;

	-- the AIR procedure has the responsiblity to pick this data up and do the right thing over there. gc5

	SET NOCOUNT OFF

END

GO

GRANT EXECUTE ON  [dbo].[spInsertEmployeeInsuranceOfferEditable] TO [aca-user] AS DBO
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[spGetFieldForIRS]
	 @employerID int,
	 @taxYear int
AS
BEGIN TRY

	SET NOCOUNT ON;

WITH empA AS
	(
	SELECT
		ed.employer_id,
		ed.employee_id,
		ed.time_frame_id,
		eyd._1095C,
		ed.offer_of_coverage_code,
		ed.share_lowest_cost_monthly_premium,
		ed.safe_harbor_code,
		ed.monthly_status_id,
		ed.monthly_hours,
		ed.mec_offered,
		ed.enrolled
	FROM
		air.appr.employee_monthly_detail ed
		INNER JOIN air.gen.time_frame tf ON (tf.time_frame_id = ed.time_frame_id)
		INNER JOIN air.appr.employee_yearly_detail eyd ON (eyd.employee_id = ed.employee_id)
	WHERE
		ed.employer_id = @employerID
			AND
		tf.year_id = @taxYear
	),
	empB AS
	(
	SELECT
		eea.employee_id,
		eea.first_name,
		eea.middle_name,
		eea.last_name,
		eea.name_suffix,
		eeb.hireDate,
		eeb.terminationDate,
		eeb.dob,
		eeb.ext_emp_id,
		hr.ext_id AS HRStatusCode,
		hr.[name] AS HRStatusDescription,
		acas.[name] AS ACAStatus,
		ec.[description] AS EmployeeClass,
		et.[name] AS EmployeeType
	FROM
		air.emp.employee eea
		INNER JOIN empA em ON (eea.employee_id = em.employee_id) 
		INNER JOIN dbo.employee eeb ON (eea.employee_id = eeb.employee_id)
		INNER JOIN dbo.aca_status acas ON (acas.aca_status_id = eeb.aca_status_id)
		INNER JOIN dbo.hr_status hr ON (hr.HR_status_id = eeb.HR_status_id)
		INNER JOIN dbo.employee_classification ec ON (ec.classification_id = eeb.classification_id)
		INNER JOIN dbo.employee_type et ON (et.employee_type_id = eeb.employee_type_id)
	)
	SELECT DISTINCT
		er.ein AS [FEIN],
		emb.employee_id AS [EmployeeId],
		ISNULL(emb.last_name, '') + ', ' + ISNULL(emb.first_name, '') + ' ' + ISNULL(emb.name_suffix, '') AS [Name],
		CONVERT(varchar(8), hireDate, 1) AS [HireDate],
		CONVERT(varchar(8), terminationDate, 1) AS [TerminationDate],
		CONVERT(varchar(8), dob, 1) AS [DateOfBirth],
		ema._1095C AS [GettingA1095],
		tf.year_id as [Year], 
		m.month_id as [Month],
		ema.offer_of_coverage_code AS [Line14OfferOfCoverageCode],
		ema.share_lowest_cost_monthly_premium AS [Line15Premium],
		ema.safe_harbor_code AS [Line16SafeHarborCode],
		ema.monthly_hours AS [Measured Monthly Hours],
		ms.status_description AS [Measured ACA Status],
		ema.mec_offered AS [HasAnOfferThisMonth],
		ema.enrolled AS [ShowsEnrolledThisMonth],
		emb.ext_emp_id AS [Employee #],
		HRStatusCode AS [HR Status Code],
		HRStatusDescription AS [HR Status Description],
		emb.ACAStatus AS [ACA Status],
		EmployeeClass AS [Employee Class],
		EmployeeType AS [Employee Type]
	FROM
		empA ema
		INNER JOIN empB emb ON (ema.employee_id = emb.employee_id) 
		INNER JOIN air.emp.monthly_status ms ON (ema.monthly_status_id = ms.monthly_status_id)
		INNER JOIN air.ale.employer er ON (er.employer_id = ema.employer_id) 
		INNER JOIN air.gen.time_frame tf ON (ema.time_frame_id = tf.time_frame_id)
		INNER JOIN air.gen.[month] m ON (tf.month_id = m.month_id)
	ORDER BY
		[Name], [Year], [Month]

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH

GO

GRANT EXECUTE ON [dbo].[spGetFieldForIRS] TO [aca-user] AS DBO
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:<Travis Wells>
-- Create date: <4/9/2016>
-- Description: <This function will transpose all insurance carriers rows for a single individual. If they have multiple rows, 
--the TRUE value will always show. >
-- EXAMPLE:
--		Jan	Feb	Mar	Apr	May	Jun	Jul	Aug	Sep	Oct	Nov	Dec
--Bob	1	1	1	0	0	0	1	1	1	1	1	1
--Bob	0	0	0	0	0	1	1	1	1	1	0	0
--____________________________________________________
--Bob	1	1	1	0	0	1	1	1	1	1	1	1	Final Results for AIR system.
-- =============================================
ALTER FUNCTION [dbo].[ufnGetConsolidatedEmployeeInsurance](@employer_id int, @tax_year SMALLINT)
RETURNS TABLE 
AS
RETURN 
SELECT DISTINCT
              MIN(ic.row_id) AS first_row_id,
              ic.employee_id,
              ee.employer_id, 
              ic.dependent_id, 
              ic.tax_year,
              MAX(CASE WHEN ic.dependent_id IS NULL THEN ee.fName ELSE ed.fName END) AS fName,
              MAX(CASE WHEN ic.dependent_id IS NULL THEN ee.mName ELSE ed.mName END) AS mName,
              MAX(CASE WHEN ic.dependent_id IS NULL THEN ee.lName ELSE ed.lName END) AS lName,
              MAX(CASE WHEN ic.dependent_id IS NULL THEN ee.ssn ELSE ed.ssn END) AS ssn,
              MAX(CASE WHEN ic.dependent_id IS NULL THEN ee.dob ELSE ed.dob END) AS dob,
              MAX(CONVERT(INTEGER,ic.jan)) as Jan,
              MAX(CONVERT(INTEGER,ic.feb))as Feb,
              MAX(CONVERT(INTEGER,ic.mar)) as Mar,
              MAX(CONVERT(INTEGER,ic.apr)) as Apr,
              MAX(CONVERT(INTEGER,ic.may)) as May,
              MAX(CONVERT(INTEGER,ic.jun)) as Jun,
              MAX(CONVERT(INTEGER,ic.jul)) as Jul,
              MAX(CONVERT(INTEGER,ic.aug)) as Aug,
              MAX(CONVERT(INTEGER,ic.sep)) as Sept,
              MAX(CONVERT(INTEGER,ic.oct)) as Oct,
              MAX(CONVERT(INTEGER,ic.nov)) as Nov,
              MAX(CONVERT(INTEGER,ic.[dec])) as [Dec]
FROM
	aca.dbo.insurance_coverage ic
    INNER JOIN aca.dbo.employee ee ON (ic.employee_id = ee.employee_id)
    LEFT OUTER JOIN aca.dbo.employee_dependents ed ON (ic.dependent_id = ed.dependent_id)
WHERE
	ic.tax_year = @tax_year
		AND
	ee.employer_id = @employer_id
GROUP BY
	ic.employee_id,
	ee.employer_id,
	ic.dependent_id,
	tax_year

GO

GRANT SELECT ON [dbo].[ufnGetConsolidatedEmployeeInsurance] TO [aca-user] AS DBO
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[Migrate_Coverage] 
	@employer_id INT, 
	@year_id SMALLINT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE [dbo].[insurance_coverage_editable]
	WHERE
		tax_year = @year_id
			AND
		employer_id = @employer_id;

    INSERT INTO [dbo].[insurance_coverage_editable]
	(
		employee_id,
		employer_id,
		dependent_id,
		tax_year,
		jan,
		feb,
		Mar,
		Apr,
		May,
		Jun,
		Jul,
		Aug,
		Sept,
		Oct,
		Nov,
		[Dec]
	)
	SELECT
		employee_id,
		employer_id,
		dependent_id,
		tax_year,
		jan,
		feb,
		Mar,
		Apr,
		May,
		Jun,
		Jul,
		Aug,
		Sept,
		Oct,
		Nov, 
		[Dec]
	FROM
		dbo.[ufnGetConsolidatedEmployeeInsurance](@employer_id, @year_id) 
	WHERE
		employer_id = @employer_id
END

GO

GRANT EXECUTE ON [dbo].[Migrate_Coverage] TO [aca-user] AS DBO
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[PrepareAcaForIRSStaging] 
	@employer_id INT,
	@year_id SMALLINT
AS
BEGIN TRY
	BEGIN TRAN PrepareAcaForIRSStaging
		BEGIN
		EXEC dbo.Migrate_Coverage @employer_id, @year_id
		EXEC [dbo].[spInsertEmployeeInsuranceOfferEditable] @employer_id, @year_id
		END
	COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRAN PrepareAcaForIRSStaging
	EXEC dbo.INSERT_ErrorLogging
END CATCH

GO

GRANT EXECUTE ON [dbo].[PrepareAcaForIRSStaging] TO [aca-user] AS DBO
GO
