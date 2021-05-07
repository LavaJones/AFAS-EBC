USE [aca]
GO

CREATE TABLE [dbo].[EmployeeInsuranceOfferEditable](
	[EmployeeInsuranceOfferEditableId] [int] IDENTITY(1,1) NOT NULL,
	[TaxYearId] [int] NOT NULL,
	[MonthId] [int] NOT NULL,
	[TimeFrameId] [int] NOT NULL,
	[EmployerId] [int] NOT NULL,
	[EmployeeId] [int] NOT NULL,
	[OfferInForce] [bit] NOT NULL,
	[AcceptedInForce] [bit] NOT NULL,
	[CoverageInForce] [bit] NOT NULL,
	[ResourceId] [uniqueidentifier] ROWGUIDCOL NOT NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[EmployeeInsuranceOfferEditable] ADD  CONSTRAINT [DF_EmployeeInsuranceOfferEditable_ResourceId]  DEFAULT (newid()) FOR [ResourceId]
GO

GRANT SELECT ON [dbo].[EmployeeInsuranceOfferEditable] TO  [aca-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spInsertEmployeeInsuranceOfferEditable]
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

	DECLARE @insuranceStateCombined TABLE
	(
		dateRowId int NOT NULL,
		employer_id int NOT NULL,
		employee_id int NOT NULL,
		offered bit NOT NULL,
		offeredOn datetime NOT NULL,
		accepted bit NOT NULL,
		acceptedOn datetime NOT NULL,
		inEffect bit NOT NULL,
		effectiveDate datetime NOT NULL,
		hireDate datetime NOT NULL,
		terminationDate datetime NULL
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
				py.endDate <= '2016-12-31'
					OR
				py.startDate >= '2015-01-01'
			)
				AND
			(
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
			ic.offered,
			ic.offeredOn,
			ic.accepted,
			ic.acceptedOn,
			0 AS inEffect,		              
			ic.effectiveDate,
			ee.hireDate,
			ee.terminationDate
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
		offered,
		offeredOn,
		accepted,
		acceptedOn,
		inEffect,
		effectiveDate,
		hireDate,
		terminationDate
	)
	SELECT
		gcdd.dateRowId,
		gcdd.employer_id,
		gcdd.employee_id,
		gcdd.offered,
		gcdd.offeredOn,
		gcdd.accepted,
		gcdd.acceptedOn,
		gcdd.inEffect,
		gcdd.effectiveDate,
		gcdd.hireDate,
		gcdd.terminationDate
	FROM
		getCombinedDateData gcdd
	WHERE 
		gcdd.offeredOn IS NOT NULL

	DECLARE @offerByMonth TABLE(
		[TaxYearId] [int] NOT NULL,
		[MonthId] [int] NOT NULL,
		[TimeFrameId] [int] NOT NULL,
		[EmployerId] [int] NOT NULL,
		[EmployeeId] [int] NOT NULL,
		[InsuranceId] [int] NULL,
		[InsuranceContribution] [int] NULL,
		[OfferInForce] [bit] NOT NULL,
		[AcceptedInForce] [bit] NOT NULL,
		[CoverageInForce] [bit] NOT NULL
	);

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

	INSERT INTO @offerByMonth (
		[TaxYearId],
		[MonthId],
		[TimeFrameId],
		[EmployerId],
		[EmployeeId],
		[OfferInForce],
		[AcceptedInForce],
		[CoverageInForce]
	)
	SELECT
		tf.year_id,
		tf.month_id,
		tf.time_frame_id,
		ee.employer_id,
		ee.employee_id,
		0,
		0,
		0
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

	DECLARE @rowCount INT

	DECLARE @currentEmployeeId INT

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

		IF (@rowCount > 1)

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

			SELECT @minRow = MIN(dateRowId) - 1 FROM @insuranceStateCombined WHERE employee_id = @currentEmployeeId;

			SELECT @offeredDateEnd = '2016-12-31'
			SELECT @acceptedDateEnd = '2016-12-31'
			SELECT @coverageDateEnd = '2016-12-31'

			SELECT @rowCount = @rowCount - 1
			WHILE (@rowCount >= 0)
			BEGIN

				SELECT
					@currentRow = MIN(isc.dateRowId)
				FROM
					@insuranceStateCombined isc
				WHERE
					isc.employee_id = @currentEmployeeId
						AND
					isc.dateRowId > @minRow;

				SELECT @endingTimeFrame = [air].[etl].[ufnGetTimeFrameID](YEAR(@offeredDateEnd), MONTH(@offeredDateEnd))
		
				SELECT
					@offered = isc.offered,
					@offeredDateStart = isc.acceptedOn
				FROM
					@insuranceStateCombined isc
				WHERE
					isc.employee_id = @currentEmployeeId
						AND
					dateRowId = @currentRow;

				SELECT @startingTimeFrame = [air].[etl].ufnGetTimeFrameID(YEAR(@offeredDateStart), MONTH(@offeredDateStart));

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
					(obm.TimeFrameId >= @startingTimeFrame AND obm.TimeFrameId <= @endingTimeFrame);

				SELECT @endingTimeFrame = [air].[etl].[ufnGetTimeFrameID](YEAR(@acceptedDateEnd), MONTH(@acceptedDateEnd))
		
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
					(obm.TimeFrameId >= @startingTimeFrame AND obm.TimeFrameId <= @endingTimeFrame);

				SELECT @endingTimeFrame = [air].[etl].[ufnGetTimeFrameID](YEAR(@coverageDateEnd), MONTH(@coverageDateEnd))
		
				SELECT
					@coverageDateStart = isc.effectiveDate
				FROM
					@insuranceStateCombined isc
				WHERE
					isc.employee_id = @currentEmployeeId
						AND
					dateRowId = @currentRow;

				SELECT @startingTimeFrame = [air].[etl].ufnGetTimeFrameID(YEAR(@coverageDateStart), MONTH(@coverageDateStart));

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
					CoverageInForce = @accepted
				FROM
					@offerByMonth obm
					INNER JOIN tax_year_data tyd ON (obm.MonthId = tyd.month_id AND obm.TaxYearId = tyd.tax_year_id)
				WHERE
					EmployeeId = @currentEmployeeId
						AND
					(obm.TimeFrameId >= @startingTimeFrame AND obm.TimeFrameId <= @endingTimeFrame);

				SELECT @minRow = @currentRow;

				SELECT @rowCount = @rowCount - 1

		END
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
		[OfferInForce],
		[AcceptedInForce],
		[CoverageInForce]
	)
	SELECT 
		obm.[TaxYearId],
		obm.[MonthId],
		obm.[TimeFrameId],
		obm.[EmployerId],
		obm.[EmployeeId],
		obm.[OfferInForce],
		obm.[AcceptedInForce],
		obm.[CoverageInForce]
	FROM	
		@offerByMonth obm
	WHERE
		obm.EmployerId = @employerId;

	SET NOCOUNT OFF

END
GO

GRANT EXECUTE ON [dbo].[spInsertEmployeeInsuranceOfferEditable] TO  [aca-user] AS [dbo]
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
