USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SELECT_form_1095C_upstream_detail] 
	@employerId int,
	@taxYearId int,
	@correctedInd bit
AS
BEGIN TRY

	SET NOCOUNT ON;

	-- IRS transmission version.

	CREATE TABLE #SanitizedTaxYear1095CApprovals
	(
		tax_year INT INDEX sty1a_tax_year NONCLUSTERED,
		employer_id INT INDEX sty1a_employer_id NONCLUSTERED,
		employee_id INT INDEX sty1a_employee_id NONCLUSTERED,
		get1095C BIT INDEX sty1a_get_1095c NONCLUSTERED,
		INDEX sty1a_tax_year_employer_id_employee_id NONCLUSTERED(tax_year, employer_id, employee_id),
		INDEX sty1a_tax_year_employee_id NONCLUSTERED(tax_year, employee_id)
	)

	-- Patch around the fact that certain groups have multiple approvals. gc5
	INSERT INTO #SanitizedTaxYear1095CApprovals
	SELECT DISTINCT
		ty1a.tax_year,
		ty1a.employer_id,
		ty1a.employee_id,
		ty1a.get1095C
	FROM
		[aca].[dbo].[tax_year_1095c_approval] ty1a
	WHERE
		ty1a.employer_id = @employerId
			AND
		ty1a.tax_year = @taxYearId
			AND
		ty1a.get1095C = 1;

	CREATE TABLE #SafeMonthlyDetails
	(
		employee_id INT INDEX smd_employee_id NONCLUSTERED,
		offer_of_coverage_code varchar(max),
		share_lowest_cost_monthly_premium varchar(max),
		month_id INT INDEX smd_month_id NONCLUSTERED,
		safe_harbor_code varchar(max),
		insurance_type_id INT INDEX smd_insurance_type_id NONCLUSTERED,
		INDEX smd_employee_id_month_id NONCLUSTERED(employee_id, month_id)
	)

	DECLARE @CorrectedRecordInfoGrp TABLE
	(
		employer_id INT,
		employee_id INT,
		CorrectedUniqueRecordId varchar(100),
		PersonFirstNm nvarchar(50),
		PersonMiddleNm nvarchar(50),
		PersonLastNm nvarchar(50),
		SuffixNm nvarchar(50)
	)

	INSERT INTO @CorrectedRecordInfoGrp
	SELECT
	    tyc.employer_id,
	    tyc.employee_id,
		tyc.CorrectedUniqueRecordId,
		e.first_name,
		e.middle_name,
		e.last_name,
		e.name_suffix
	FROM [air].[emp].employee e INNER JOIN [aca].[dbo].[tax_year_1095c_correction] tyc ON e.employee_id = tyc.employee_id
	WHERE tyc.tax_year = @taxYearId
		AND tyc.employer_id = @employerId
		AND tyc.Corrected = 1
		AND tyc.Transmitted = 0
		AND tyc.EntityStatusId = 1

	INSERT INTO #SafeMonthlyDetails
	SELECT 
		emd.employee_id,
		ISNULL(emd.offer_of_coverage_code, '') AS offer_of_coverage_code,
		CONVERT(varchar(max), emd.share_lowest_cost_monthly_premium) AS share_lowest_cost_monthly_premium,
		tf.month_id,
		ISNULL(emd.safe_harbor_code, '') AS safe_harbor_code,
		emd.insurance_type_id
	FROM
		[air].[appr].[employee_monthly_detail] emd
		INNER JOIN air.gen.time_frame tf ON (emd.time_frame_id = tf.time_frame_id)
	WHERE
		emd.employer_id = @employerId
			AND
		tf.year_id = @taxYearId;

	DECLARE @EmployeeCoverageDetails TABLE (
		employee_id int NOT NULL,
		AnnlShrLowestCostMthlyPremAmt varchar(max),
		JanuaryAmt varchar(max),
		FebruaryAmt varchar(max),
		MarchAmt varchar(max),
		AprilAmt varchar(max),
		MayAmt varchar(max),
		JuneAmt varchar(max),
		JulyAmt varchar(max),
		AugustAmt varchar(max),
		SeptemberAmt varchar(max),
		OctoberAmt varchar(max),
		NovemberAmt varchar(max),
		DecemberAmt varchar(max),
		AnnualOfferOfCoverageCd varchar(max),
		JanOfferCd varchar(max),
		FebOfferCd varchar(max),
		MarOfferCd varchar(max),
		AprOfferCd varchar(max),
		MayOfferCd varchar(max),
		JunOfferCd varchar(max),
		JulOfferCd varchar(max),
		AugOfferCd varchar(max),
		SepOfferCd varchar(max),
		OctOfferCd varchar(max),
		NovOfferCd varchar(max),
		DecOfferCd varchar(max),
		AnnualSafeHarborCd varchar(max),
		JanSafeHarborCd varchar(max),
		FebSafeHarborCd varchar(max),
		MarSafeHarborCd varchar(max),
		AprSafeHarborCd varchar(max),
		MaySafeHarborCd varchar(max),
		JunSafeHarborCd varchar(max),
		JulSafeHarborCd varchar(max),
		AugSafeHarborCd varchar(max),
		SepSafeHarborCd varchar(max),
		OctSafeHarborCd varchar(max),
		NovSafeHarborCd varchar(max),
		DecSafeHarborCd varchar(max)
	);

	---- pull through the approval table filter here so everything else can drive from the in memory table. gc5
	INSERT INTO @EmployeeCoverageDetails (
		employee_id
	)
	SELECT * FROM
	(
	SELECT 
		air_ee.employee_id
	FROM
		[air].[emp].[employee] air_ee
		INNER JOIN #SanitizedTaxYear1095CApprovals ty1a ON (air_ee.employee_id = ty1a.employee_id AND ty1a.tax_year = @taxYearId)
	WHERE
		air_ee.employer_id = @employerId
			AND
		ty1a.get1095C = 1

	UNION

	SELECT 
		air_ee.employee_id
	FROM
		[air].[emp].[employee] air_ee
		INNER JOIN @CorrectedRecordInfoGrp cr ON (air_ee.employee_id = cr.employee_id)

	) AS employeeIds

	
	-- To simplify the queries, the c# client code handles the xml tree creation for monthly details if these are null.
	UPDATE @EmployeeCoverageDetails
	SET
		AnnualOfferOfCoverageCd = i.offer_of_coverage_code,
		AnnualSafeHarborCd = i.safe_harbor_code,
		AnnlShrLowestCostMthlyPremAmt = CONVERT(VARCHAR(max), i.share_lowest_cost_monthly_premium)
	FROM
		@EmployeeCoverageDetails ec
		INNER JOIN (
			SELECT
				emd.employee_id,
				CASE
					WHEN COUNT(DISTINCT emd.offer_of_coverage_code) + COUNT(DISTINCT CASE WHEN emd.offer_of_coverage_code is null THEN 1 END) = 1
					THEN MAX(emd.offer_of_coverage_code)
					ELSE '' 
				END AS offer_of_coverage_code,
				CASE
					WHEN COUNT(DISTINCT emd.share_lowest_cost_monthly_premium) + COUNT(DISTINCT CASE WHEN emd.share_lowest_cost_monthly_premium is null THEN 1 END) = 1
					THEN CONVERT(varchar(max),MAX(emd.share_lowest_cost_monthly_premium))
					ELSE ''
				END AS share_lowest_cost_monthly_premium,
				CASE
					WHEN COUNT(DISTINCT emd.safe_harbor_code) + COUNT(DISTINCT CASE WHEN emd.safe_harbor_code is null THEN 1 END) = 1
					THEN MAX(emd.safe_harbor_code)
					ELSE ''
				END AS safe_harbor_code
			FROM
				[air].[appr].[employee_monthly_detail] emd
				INNER JOIN [air].[gen].[time_frame] tf ON (emd.time_frame_id = tf.time_frame_id)
			WHERE
				emd.employer_id = @employerId
					AND
				tf.year_id = @taxYearId
			GROUP BY
				emd.employee_id
		) i ON (i.employee_id = ec.employee_id);

	--January values--
	UPDATE @EmployeeCoverageDetails
	SET
		JanOfferCd = i.offer_of_coverage_code,
		JanSafeHarborCd = i.safe_harbor_code,
		JanuaryAmt = i.share_lowest_cost_monthly_premium
	FROM
		@EmployeeCoverageDetails ec
		INNER JOIN #SafeMonthlyDetails i ON (i.employee_id = ec.employee_id)
	WHERE
		ec.employee_id = i.employee_id
			AND
		i.month_id = 1;

	--February values--
	UPDATE @EmployeeCoverageDetails
	SET
		FebOfferCd = i.offer_of_coverage_code,
		FebSafeHarborCd = i.safe_harbor_code,
		FebruaryAmt = i.share_lowest_cost_monthly_premium
	FROM
		@EmployeeCoverageDetails ec
		INNER JOIN #SafeMonthlyDetails i ON (i.employee_id = ec.employee_id)
	WHERE
		ec.employee_id = i.employee_id
			AND
		i.month_id = 2;

	--March values--
	UPDATE @EmployeeCoverageDetails
	SET
		MarOfferCd = i.offer_of_coverage_code,
		MarSafeHarborCd = i.safe_harbor_code,
		MarchAmt = i.share_lowest_cost_monthly_premium
	FROM
		@EmployeeCoverageDetails ec
		INNER JOIN #SafeMonthlyDetails i ON (i.employee_id = ec.employee_id)
	WHERE
		ec.employee_id = i.employee_id
			AND
		i.month_id = 3;

	--April values--
	UPDATE @EmployeeCoverageDetails
	SET
		AprOfferCd = i.offer_of_coverage_code,
		AprSafeHarborCd = i.safe_harbor_code,
		AprilAmt = i.share_lowest_cost_monthly_premium
	FROM
		@EmployeeCoverageDetails ec
		INNER JOIN #SafeMonthlyDetails i ON (i.employee_id = ec.employee_id)
	WHERE
		ec.employee_id = i.employee_id
			AND
		i.month_id = 4;

	--May values--
	UPDATE @EmployeeCoverageDetails
	SET
		MayOfferCd = i.offer_of_coverage_code,
		MaySafeHarborCd = i.safe_harbor_code,
		MayAmt = i.share_lowest_cost_monthly_premium
	FROM
		@EmployeeCoverageDetails ec
		INNER JOIN #SafeMonthlyDetails i ON (i.employee_id = ec.employee_id)
	WHERE
		ec.employee_id = i.employee_id
			AND
		i.month_id = 5;

	--June values--
	UPDATE @EmployeeCoverageDetails
	SET
		JunOfferCd = i.offer_of_coverage_code,
		JunSafeHarborCd = i.safe_harbor_code,
		JuneAmt = i.share_lowest_cost_monthly_premium
	FROM
		@EmployeeCoverageDetails ec
		INNER JOIN #SafeMonthlyDetails i ON (i.employee_id = ec.employee_id)
	WHERE
		ec.employee_id = i.employee_id
			AND
		i.month_id = 6;

	--July values--
	UPDATE @EmployeeCoverageDetails
	SET
		JulOfferCd = i.offer_of_coverage_code,
		JulSafeHarborCd = i.safe_harbor_code,
		JulyAmt = i.share_lowest_cost_monthly_premium
	FROM
		@EmployeeCoverageDetails ec
		INNER JOIN #SafeMonthlyDetails i ON (i.employee_id = ec.employee_id)
	WHERE
		ec.employee_id = i.employee_id
			AND
		i.month_id = 7;

	--August values--
	UPDATE @EmployeeCoverageDetails
	SET
		AugOfferCd = i.offer_of_coverage_code,
		AugSafeHarborCd = i.safe_harbor_code,
		AugustAmt = i.share_lowest_cost_monthly_premium
	FROM
		@EmployeeCoverageDetails ec
		INNER JOIN #SafeMonthlyDetails i ON (i.employee_id = ec.employee_id)
	WHERE
		ec.employee_id = i.employee_id
			AND
		i.month_id = 8;

	--September values--
	UPDATE @EmployeeCoverageDetails
	SET
		SepOfferCd = i.offer_of_coverage_code,
		SepSafeHarborCd = i.safe_harbor_code,
		SeptemberAmt = i.share_lowest_cost_monthly_premium
	FROM
		@EmployeeCoverageDetails ec
		INNER JOIN #SafeMonthlyDetails i ON (i.employee_id = ec.employee_id)
	WHERE
		ec.employee_id = i.employee_id
			AND
		i.month_id = 9;

	--October values--
	UPDATE @EmployeeCoverageDetails
	SET
		OctOfferCd = i.offer_of_coverage_code,
		OctSafeHarborCd = i.safe_harbor_code,
		OctoberAmt = i.share_lowest_cost_monthly_premium
	FROM
		@EmployeeCoverageDetails ec
		INNER JOIN #SafeMonthlyDetails i ON (i.employee_id = ec.employee_id)
	WHERE
		ec.employee_id = i.employee_id
			AND
		i.month_id = 10;

	--November values--
	UPDATE @EmployeeCoverageDetails
	SET
		NovOfferCd = i.offer_of_coverage_code,
		NovSafeHarborCd = i.safe_harbor_code,
		NovemberAmt = i.share_lowest_cost_monthly_premium
	FROM
		@EmployeeCoverageDetails ec
		INNER JOIN #SafeMonthlyDetails i ON (i.employee_id = ec.employee_id)
	WHERE
		ec.employee_id = i.employee_id
			AND
		i.month_id = 11;

	--December values--
	UPDATE @EmployeeCoverageDetails
	SET
		DecOfferCd = i.offer_of_coverage_code,
		DecSafeHarborCd = i.safe_harbor_code,
		DecemberAmt = i.share_lowest_cost_monthly_premium
	FROM
		@EmployeeCoverageDetails ec
		INNER JOIN #SafeMonthlyDetails i ON (i.employee_id = ec.employee_id)
	WHERE
		ec.employee_id = i.employee_id
			AND
		i.month_id = 12;

	CREATE TABLE #SelfInsuredTable
	(
		employee_id INT INDEX sit_employee_id NONCLUSTERED,
		indicator INT NULL
	)

	INSERT INTO #SelfInsuredTable
	SELECT DISTINCT
		smd.employee_id,
		NULL as indicator
	FROM
		#SafeMonthlyDetails smd

	UPDATE #SelfInsuredTable
	SET
		indicator = 1
	FROM
		#SelfInsuredTable sit
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci ON (sit.employee_id = ci.employee_id)
	WHERE
		ci.employer_id = @employerId;

WITH employees AS (
	SELECT
		air_ee.employer_id,
		ee.ResourceId,
		ROW_NUMBER() OVER (ORDER BY air_ee.employee_id) AS RecordId,
		CAST(@correctedInd AS varchar(1)) AS CorrectedInd,
		null AS CorrectedUniqueRecordId,
		null AS PersonFirstNm,
		null AS PersonMiddleNm,
		null AS PersonLastNm,
		null AS SuffixNm,
		air_ee.first_name AS OtherCompletePersonFirstNm,
		air_ee.last_name AS OtherCompletePersonLastNm,
		NULL AS StartMonthNumberCd,	-- optional field, not used.
		CAST(sit.indicator AS varchar(1)) AS CoveredIndividualInd,
		air_ee.ssn AS SSN,
		ee.dob AS DOB,
		air_ee.[address] AS AddressLine1Txt,
		er.contact_telephone AS ALEContactPhoneNum,
		air_ee.city AS CityNm,
		air_ee.state_code AS USStateCd,
		air_ee.zipcode AS USZIPCd,
		air_ee.zipcode_ext AS USZIPExtensionCd,
		ecd.*
	FROM
		[air].[emp].[employee] air_ee
		INNER JOIN [aca].[dbo].[employee] ee ON (air_ee.employee_id = ee.employee_id)		
		INNER JOIN @EmployeeCoverageDetails ecd ON (air_ee.employee_id = ecd.employee_id)
		INNER JOIN [air].[ale].[employer] er ON (air_ee.employer_id = er.employer_id)
		LEFT JOIN #SelfInsuredTable sit ON (air_ee.employee_id = sit.employee_id)
	WHERE @correctedInd = 0
			AND
		air_ee.employer_id = @employerId 
			AND
		air_ee.employee_id NOT IN (
			SELECT
				ty1a.[employee_id]
			FROM @CorrectedRecordInfoGrp ty1a
		)
	), corrected_employees AS (
		SELECT
			air_ee.employer_id,
			ee.ResourceId,
			ROW_NUMBER() OVER (ORDER BY air_ee.employee_id) AS RecordId,
			CAST(@correctedInd AS varchar(1)) AS CorrectedInd,
			cr.CorrectedUniqueRecordId,
			cr.PersonFirstNm,
			cr.PersonMiddleNm,
			cr.PersonLastNm,
			cr.SuffixNm,
			air_ee.first_name AS OtherCompletePersonFirstNm,
			air_ee.last_name AS OtherCompletePersonLastNm,
			NULL AS StartMonthNumberCd,	-- optional field, not used.
			CAST(sit.indicator AS varchar(1)) AS CoveredIndividualInd,
			air_ee.ssn AS SSN,
			ee.dob AS DOB,
			air_ee.[address] AS AddressLine1Txt,
			er.contact_telephone AS ALEContactPhoneNum,
			air_ee.city AS CityNm,
			air_ee.state_code AS USStateCd,
			air_ee.zipcode AS USZIPCd,
			air_ee.zipcode_ext AS USZIPExtensionCd,
			ecd.*
		FROM  [air].[emp].[employee] air_ee
			INNER JOIN [aca].[dbo].[employee] ee ON (air_ee.employee_id = ee.employee_id)		
			INNER JOIN @EmployeeCoverageDetails ecd ON (air_ee.employee_id = ecd.employee_id)
			INNER JOIN [air].[ale].[employer] er ON (air_ee.employer_id = er.employer_id)
			INNER JOIN @CorrectedRecordInfoGrp cr ON (air_ee.employee_id = cr.employee_id)
			LEFT JOIN #SelfInsuredTable sit ON (air_ee.employee_id = sit.employee_id)
		WHERE @correctedInd = 1
				AND
			air_ee.employer_id = @employerId
	)

	SELECT * 
	FROM employees 
	------WHERE @correctedInd = 0

	UNION

	SELECT * 
	FROM corrected_employees 
	--WHERE @correctedInd = 1

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
		
	DROP TABLE #SelfInsuredTable;
	DROP TABLE #SafeMonthlyDetails;
	DROP TABLE #SanitizedTaxYear1095CApprovals;

GO


