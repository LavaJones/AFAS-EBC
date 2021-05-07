USE [aca]
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

	-- priming the cursor pump
	FETCH NEXT FROM insuranceEvent INTO @currentEmployeeId

	-- for each looping.
	WHILE @@FETCH_STATUS = 0
	BEGIN

		SELECT
			@rowCount = COUNT(isc.employee_id)
		FROM
			@insuranceStateCombined isc
		WHERE
			isc.employee_id = @currentEmployeeId;

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

			-- prime the pump
			SELECT @rowCount = @rowCount - 1

			-- looping
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

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/17/2016>
-- Description:	<This stored procedure is meant to generate Insurance Alerts for any missing Employees.>	
-- Modifications:

-- =============================================
ALTER PROCEDURE [dbo].[INSERT_PlanYear_Missing_insurance_offers]
	@employerID int,
	@planYearEndDate datetime, --Stability Period Start Date
	@missingPlanYearID int,
	@currPlanYearID int,
	@hours int,
	@modBy varchar(50),
	@modOn datetime, 
	@history varchar(max)
AS

BEGIN TRY

	INSERT INTO [dbo].[employee_insurance_offer] (
		employee_id,
		plan_year_id,
		employer_id,
		avg_hours_month,
		modOn,
		modBy,
		history
	)
	SELECT
		employee_id,
		@missingPlanYearID,
		@employerID,
		@hours,
		@modOn,
		@modBy,
		@history
	FROM
		[dbo].[employee] ee
	WHERE 
		ee.hireDate < @planYearEndDate
			AND
		ee.employer_id = @employerID
			AND
		ee.plan_year_id = @currPlanYearID
			AND
		ee. employee_id NOT IN (
			SELECT
				eio.employee_id
			FROM
				[dbo].[employee_insurance_offer] eio
			WHERE
				eio.plan_year_id=@missingPlanYearID
					AND
				eio.employer_id=@employerID
			);

END TRY
BEGIN CATCH
      EXEC INSERT_ErrorLogging
END CATCH

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/24/2016>
-- Description:	<This stored procedure is meant to return all employees who haven't had their 1095c approved for a specific tax year.>
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_employer_employees_Tax_Year_Approved](
	@employerID int,
	@taxYear int
	)
AS
BEGIN
	SELECT
		[employee_id],
		[employee_type_id],
		[HR_status_id],
		[employer_id],
		[fName],
		[mName],
		[lName],
		[address],
		[city],
		[state_id],
		[zip],
		[hireDate],
		[currDate],
		[ssn],
		[ext_emp_id],
		[terminationDate],
		[dob],
		[initialMeasurmentEnd],
		[plan_year_id],
		[limbo_plan_year_id],
		[meas_plan_year_id],
		[modOn],
		[modBy],
		[plan_year_avg_hours],
		[limbo_plan_year_avg_hours],
		[meas_plan_year_avg_hours],
		[imp_plan_year_avg_hours],
		[classification_id],
		[aca_status_id],
		[ResourceId]
	FROM
		[dbo].[employee] ee
	WHERE
		ee.[employer_id] = @employerID
			AND
		ee.[employee_id] NOT IN (
			SELECT
				ty1a.[employee_id]
			FROM [tax_year_1095c_approval] ty1a
			WHERE
				ty1a.[tax_year] = @taxYear
			);

END

GO

