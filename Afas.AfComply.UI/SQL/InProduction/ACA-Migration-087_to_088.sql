USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ryan, McCully -- based on Obiye's work>
-- Create date: <3/9/2017>
-- Description:	<Pull the monthly details for 1094>
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_ale_member_information_monthly]
	 @taxYearId int,
	 @employerId int = null
AS
BEGIN TRY

	SET NOCOUNT ON;

	DECLARE @ALEMemberInformationGrp TABLE (		
		employer_id int NOT NULL,
		time_frame_id int,
		ALEMemberFTECnt varchar(max),
		TotalEmployeeCnt varchar(max)
	)

	INSERT INTO 
		@ALEMemberInformationGrp(
		employer_id, 
		time_frame_id) 
	Select distinct
		emd.employer_id, 
		emd.time_frame_id 
	FROM 
		[air].[appr].[employee_monthly_detail] emd 
	WHERE 
		(emd.employer_id = @employerId OR @employerId IS NULL);


	UPDATE 
		@ALEMemberInformationGrp
	SET
		ALEMemberFTECnt = ISNULL(i.EmployeeCountOfOfferStatusPerMonth,'0')
	FROM 
		@ALEMemberInformationGrp ale 
	INNER JOIN(
		SELECT
			emd.employer_id, 
			emd.time_frame_id,
			COUNT(*) AS EmployeeCountOfOfferStatusPerMonth
		FROM
			[air].[appr].[employee_monthly_detail] emd 
			inner join [aca].[dbo].[tax_year_1095c_approval] ty on ty.employee_id = emd.employee_id AND ty.tax_year = @taxYearId
		WHERE
			(emd.employer_id = @employerId OR @employerId IS NULL)
				AND
			emd.monthly_status_id IN (1)
				AND
			(
				emd.safe_harbor_code <> '2D'
					OR
				emd.safe_harbor_code IS NULL
			)
				AND
			emd.mec_offered = 1
		GROUP BY
			emd.employer_id, emd.time_frame_id
	) i 
	ON 
		i.employer_id = ale.employer_id 
			AND 
		i.time_frame_id = ale.time_frame_id;

	UPDATE 
		@ALEMemberInformationGrp
	SET
		TotalEmployeeCnt = ISNULL(i.EmployeeCountOfTypePerMonth,'0')
	FROM 
		@ALEMemberInformationGrp ale 
	INNER JOIN(
		SELECT
			emd.employer_id, 
			emd.time_frame_id, 
			COUNT(*) AS EmployeeCountOfTypePerMonth 
		FROM
			[air].[appr].[employee_monthly_detail] emd
			inner join [aca].[dbo].[tax_year_1095c_approval] ty on ty.employee_id = emd.employee_id AND ty.tax_year = @taxYearId
		WHERE
			(emd.employer_id = @employerId OR @employerId IS NULL)
				AND
			emd.monthly_status_id NOT IN (4, 5, 8)
				AND
			(
				emd.safe_harbor_code <> '2D'
					OR
				emd.safe_harbor_code IS NULL
			)
		GROUP BY
			emd.employer_id, 
			emd.time_frame_id
	) i 
	ON 
		i.employer_id = ale.employer_id 
			AND 
		i.time_frame_id = ale.time_frame_id;
		

	SELECT * FROM @ALEMemberInformationGrp ORDER BY
			employer_id, 
			time_frame_id

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH

GO 
GRANT EXECUTE ON [dbo].[SELECT_ale_member_information_monthly] TO [aca-user] AS [dbo] 
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

AlTER PROCEDURE dbo.SELECT_form_1095C_upstream_detail 
	@employerId int,
	@taxYearId int
AS
BEGIN TRY

	SET NOCOUNT ON;

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

	INSERT INTO @EmployeeCoverageDetails(
		employee_id)
	SELECT 
		a.employee_id
	FROM air.emp.employee a INNER JOIN air.appr.employee_yearly_detail e ON a.employee_id = e.employee_id
							INNER JOIN aca.dbo.tax_year_1095c_approval tya ON tya.employee_id = e.employee_id AND tya.tax_year = e.year_id
							INNER JOIN aca.dbo.employee c ON c.employee_id = e.employee_id
	WHERE a.employer_id = @employerId AND e.year_id = @taxYearId
	AND e._1095C = 1

	--All values--
	UPDATE @EmployeeCoverageDetails
	SET AnnualOfferOfCoverageCd = i.offer_of_coverage_code, AnnualSafeHarborCd = i.safe_harbor_code, AnnlShrLowestCostMthlyPremAmt = convert(varchar(max),i.monthly_hours)
	FROM @EmployeeCoverageDetails ec INNER JOIN(
	SELECT
		a.employee_id,
		CASE WHEN COUNT(DISTINCT a.offer_of_coverage_code) + COUNT(DISTINCT CASE WHEN a.offer_of_coverage_code is null THEN 1 END) = 1 THEN MAX(a.offer_of_coverage_code) ELSE '' END AS offer_of_coverage_code,
		CASE WHEN COUNT(DISTINCT a.monthly_hours) + COUNT(DISTINCT CASE WHEN a.monthly_hours is null THEN 1 END) = 1 THEN convert(varchar(max),MAX(a.monthly_hours)) ELSE '' END AS monthly_hours,
		CASE WHEN COUNT(DISTINCT a.safe_harbor_code) + COUNT(DISTINCT CASE WHEN a.safe_harbor_code is null THEN 1 END) = 1 THEN MAX(a.safe_harbor_code) ELSE '' END AS safe_harbor_code
	FROM air.appr.employee_monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE a.employer_id = @employerId AND b.year_id = @taxYearId
	GROUP BY a.employee_id
	) i ON i.employee_id = ec.employee_id

	--January values--
	UPDATE @EmployeeCoverageDetails
	SET JanOfferCd = ISNULL(i.offer_of_coverage_code,''), JanSafeHarborCd = ISNULL(i.safe_harbor_code,''), JanuaryAmt = convert(varchar(max),i.monthly_hours)
	FROM @EmployeeCoverageDetails ec INNER JOIN(
	SELECT 
		a.employee_id,
		a.offer_of_coverage_code,
		a.monthly_hours,
		m.month_id,
		a.safe_harbor_code
	FROM air.appr.employee_monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE a.employer_id = @employerId AND b.year_id = @taxYearId AND m.month_id = 1
	) i ON i.employee_id = ec.employee_id
	WHERE ec.employee_id = i.employee_id

	--February values--
	UPDATE @EmployeeCoverageDetails
	SET FebOfferCd = ISNULL(i.offer_of_coverage_code,''), FebSafeHarborCd = ISNULL(i.safe_harbor_code,''), FebruaryAmt = convert(varchar(max),i.monthly_hours)
	FROM @EmployeeCoverageDetails ec INNER JOIN(
	SELECT 
		a.employee_id,
		a.offer_of_coverage_code,
		a.monthly_hours,
		m.month_id,
		a.safe_harbor_code
	FROM air.appr.employee_monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE a.employer_id = @employerId AND b.year_id = @taxYearId AND m.month_id = 2
	) i ON i.employee_id = ec.employee_id
	WHERE ec.employee_id = i.employee_id

	--March values--
	UPDATE @EmployeeCoverageDetails
	SET MarOfferCd = ISNULL(i.offer_of_coverage_code,''), MarSafeHarborCd = ISNULL(i.safe_harbor_code,''), MarchAmt = convert(varchar(max),i.monthly_hours)
	FROM @EmployeeCoverageDetails ec INNER JOIN(
	SELECT 
		a.employee_id,
		a.offer_of_coverage_code,
		a.monthly_hours,
		m.month_id,
		a.safe_harbor_code
	FROM air.appr.employee_monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE a.employer_id = @employerId AND b.year_id = @taxYearId AND m.month_id = 3
	) i ON i.employee_id = ec.employee_id
	WHERE ec.employee_id = i.employee_id

	--April values--
	UPDATE @EmployeeCoverageDetails
	SET AprOfferCd = ISNULL(i.offer_of_coverage_code,''), AprSafeHarborCd = ISNULL(i.safe_harbor_code,''), AprilAmt = convert(varchar(max),i.monthly_hours)
	FROM @EmployeeCoverageDetails ec INNER JOIN(
	SELECT 
		a.employee_id,
		a.offer_of_coverage_code,
		a.monthly_hours,
		m.month_id,
		a.safe_harbor_code
	FROM air.appr.employee_monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE a.employer_id = @employerId AND b.year_id = @taxYearId AND m.month_id = 4
	) i ON i.employee_id = ec.employee_id
	WHERE ec.employee_id = i.employee_id

	--May values--
	UPDATE @EmployeeCoverageDetails
	SET MayOfferCd = ISNULL(i.offer_of_coverage_code,''), MaySafeHarborCd = ISNULL(i.safe_harbor_code,''), MayAmt = convert(varchar(max),i.monthly_hours)
	FROM @EmployeeCoverageDetails ec INNER JOIN(
	SELECT 
		a.employee_id,
		a.offer_of_coverage_code,
		a.monthly_hours,
		m.month_id,
		a.safe_harbor_code
	FROM air.appr.employee_monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE a.employer_id = @employerId AND b.year_id = @taxYearId AND m.month_id = 5
	) i ON i.employee_id = ec.employee_id
	WHERE ec.employee_id = i.employee_id

	--June values--
	UPDATE @EmployeeCoverageDetails
	SET JunOfferCd = ISNULL(i.offer_of_coverage_code,''), JunSafeHarborCd = ISNULL(i.safe_harbor_code,''), JuneAmt = convert(varchar(max),i.monthly_hours)
	FROM @EmployeeCoverageDetails ec INNER JOIN(
	SELECT 
		a.employee_id,
		a.offer_of_coverage_code,
		a.monthly_hours,
		m.month_id,
		a.safe_harbor_code
	FROM air.appr.employee_monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE a.employer_id = @employerId AND b.year_id = @taxYearId AND m.month_id = 6
	) i ON i.employee_id = ec.employee_id
	WHERE ec.employee_id = i.employee_id

	--July values--
	UPDATE @EmployeeCoverageDetails
	SET JulOfferCd = ISNULL(i.offer_of_coverage_code,''), JulSafeHarborCd = ISNULL(i.safe_harbor_code,''), JulyAmt = convert(varchar(max),i.monthly_hours)
	FROM @EmployeeCoverageDetails ec INNER JOIN(
	SELECT 
		a.employee_id,
		a.offer_of_coverage_code,
		a.monthly_hours,
		m.month_id,
		a.safe_harbor_code
	FROM air.appr.employee_monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE a.employer_id = @employerId AND b.year_id = @taxYearId AND m.month_id = 7
	) i ON i.employee_id = ec.employee_id
	WHERE ec.employee_id = i.employee_id

	--August values--
	UPDATE @EmployeeCoverageDetails
	SET AugOfferCd = ISNULL(i.offer_of_coverage_code,''), AugSafeHarborCd = ISNULL(i.safe_harbor_code,''), AugustAmt = convert(varchar(max),i.monthly_hours)
	FROM @EmployeeCoverageDetails ec INNER JOIN(
	SELECT 
		a.employee_id,
		a.offer_of_coverage_code,
		a.monthly_hours,
		m.month_id,
		a.safe_harbor_code
	FROM air.appr.employee_monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE a.employer_id = @employerId AND b.year_id = @taxYearId AND m.month_id = 8
	) i ON i.employee_id = ec.employee_id
	WHERE ec.employee_id = i.employee_id

	--September values--
	UPDATE @EmployeeCoverageDetails
	SET SepOfferCd = ISNULL(i.offer_of_coverage_code,''), SepSafeHarborCd = ISNULL(i.safe_harbor_code,''), SeptemberAmt = convert(varchar(max),i.monthly_hours)
	FROM @EmployeeCoverageDetails ec INNER JOIN(
	SELECT 
		a.employee_id,
		a.offer_of_coverage_code,
		a.monthly_hours,
		m.month_id,
		a.safe_harbor_code
	FROM air.appr.employee_monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE a.employer_id = @employerId AND b.year_id = @taxYearId AND m.month_id = 9
	) i ON i.employee_id = ec.employee_id
	WHERE ec.employee_id = i.employee_id

	--October values--
	UPDATE @EmployeeCoverageDetails
	SET OctOfferCd = ISNULL(i.offer_of_coverage_code,''), OctSafeHarborCd = ISNULL(i.safe_harbor_code,''), OctoberAmt = convert(varchar(max),i.monthly_hours)
	FROM @EmployeeCoverageDetails ec INNER JOIN(
	SELECT 
		a.employee_id,
		a.offer_of_coverage_code,
		a.monthly_hours,
		m.month_id,
		a.safe_harbor_code
	FROM air.appr.employee_monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE a.employer_id = @employerId AND b.year_id = @taxYearId AND m.month_id = 10
	) i ON i.employee_id = ec.employee_id
	WHERE ec.employee_id = i.employee_id

	--November values--
	UPDATE @EmployeeCoverageDetails
	SET NovOfferCd = ISNULL(i.offer_of_coverage_code,''), NovSafeHarborCd = ISNULL(i.safe_harbor_code,''), NovemberAmt = convert(varchar(max),i.monthly_hours)
	FROM @EmployeeCoverageDetails ec INNER JOIN(
	SELECT 
		a.employee_id,
		a.offer_of_coverage_code,
		a.monthly_hours,
		m.month_id,
		a.safe_harbor_code
	FROM air.appr.employee_monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE a.employer_id = @employerId AND b.year_id = @taxYearId AND m.month_id = 11
	) i ON i.employee_id = ec.employee_id
	WHERE ec.employee_id = i.employee_id

	--December values--
	UPDATE @EmployeeCoverageDetails
	SET DecOfferCd = ISNULL(i.offer_of_coverage_code,''), DecSafeHarborCd = ISNULL(i.safe_harbor_code,''), DecemberAmt = convert(varchar(max),i.monthly_hours)
	FROM @EmployeeCoverageDetails ec INNER JOIN(
	SELECT 
		a.employee_id,
		a.offer_of_coverage_code,
		a.monthly_hours,
		m.month_id,
		a.safe_harbor_code
	FROM air.appr.employee_monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE a.employer_id = @employerId AND b.year_id = @taxYearId AND m.month_id = 12
	) i ON i.employee_id = ec.employee_id
	WHERE ec.employee_id = i.employee_id

    -- Insert statements for procedure here
	SELECT
		a.employer_id,
		a.employee_id,
		--a.test_id AS TestScenarioId,
		--a.employee_control_name AS PersonNameControlTxt,
		c.ResourceId,
		ROW_NUMBER() OVER (ORDER BY a.employee_id) AS RecordId,
		'0' AS CorrectedInd,
		a.first_name AS OtherCompletePersonFirstNm,
		a.last_name AS OtherCompletePersonLastNm,
		yd.plan_start_month AS StartMonthNumberCd,
		CAST(yd.enrolled AS varchar(1)) AS CoveredIndividualInd,
		a.ssn AS SSN,
		c.dob AS DOB,
		a.[address] AS AddressLine1Txt,
		e.contact_telephone AS ALEContactPhoneNum,
		a.city AS CityNm,
		a.state_code AS USStateCd,
		a.zipcode AS USZIPCd,
		a.zipcode_ext AS USZIPExtensionCd,
		ecd.*
	FROM air.emp.employee a INNER JOIN aca.dbo.employee	c ON a.employee_id = c.employee_id		
							INNER JOIN @EmployeeCoverageDetails ecd ON a.employee_id = ecd.employee_id
							INNER JOIN air.appr.employee_yearly_detail yd ON yd.employee_id = a.employee_id
							INNER JOIN air.ale.employer e ON e.employer_id = a.employer_id
	WHERE a.employer_id = @employerId 

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE SELECT_form_1094C_upstream_detail
	@taxYearId int,
	@employerId int = null
AS
BEGIN TRY

	SET NOCOUNT ON;

	WITH employers AS (
	SELECT DISTINCT
		a.employer_id,
		a.dge_ein,
		a.ein AS EmployerEIN,
		c.ResourceId,
		a.name AS BusinessNameLine1Txt,
		a.[address] AS AddressLine1Txt,
		--a.employer_control_name AS BusinessNameControlTxt,
		a.city AS CityNm, 
		a.state_code AS USStateCd, 
		a.zipcode AS USZIPCd,
		a.zipcode_ext AS USZIPExtensionCd,
		a.contact_first_name AS PersonFirstNm,
		a.contact_middle_name AS PersonMiddleNm,
		a.contact_last_name AS PersonLastNm,
		a.contact_name_suffix AS SuffixNm,
		a.contact_telephone AS ContactPhoneNum
	FROM air.ale.employer a 
		INNER JOIN aca.dbo.employer c ON a.employer_id = c.employer_id 
		INNER JOIN air.appr.employee_yearly_detail e ON a.employer_id = e.employer_id
	WHERE (e.employer_id = @employerId OR @employerId IS NULL)  
		AND e._1095C = 1
		AND e.year_id = @taxYearId
	)

	SELECT
		ROW_NUMBER() OVER (ORDER BY e.employer_id) AS SubmissionId,
		'0' AS CorrectedInd, --appr.employee_yearly_detail
		e.*,
		yd.dge_name AS GovtBusinessNameLine1Txt,
		yd.dge_ein AS GovtEmployerEIN,
		yd.dge_address AS GovtAddressLine1Txt,
		yd.dge_city AS GovtCityNm,
		s.abbreviation AS GovtUSStateCd,
		yd.dge_zip AS GovtUSZIPCd,
		yd.dge_contact_fname AS GovtPersonFirstNm,
		yd.dge_contact_lname AS GovtPersonLastNm,
		yd.dge_phone AS GovtContactPhoneNum,
		CASE WHEN e.dge_ein IS NULL THEN '1' ELSE '0' END AS AuthoritativeTransmittalInd,
		CAST(tya._1095C_count AS varchar(max)) AS TotalForm1095CALEMemberCnt, --tax year aca.dbo.tax_year_1095C_approval
		CAST(yd.ale AS varchar(1)) AS AggregatedGroupMemberCd, -- ale tax_year_approval
		'1' AS QualifyingOfferMethodInd, -- probably always true
		null AS QlfyOfferMethodTrnstReliefInd, -- set always to false
		CAST(yd.tr_qualified AS varchar(1)) AS Section4980HReliefInd, -- tr_qualified on aca.dbo.tax_year_approval?
		null AS NinetyEightPctOfferMethodInd -- set always to false
		--yd.jurat_signature_pin AS JuratSignaturePIN,
		--yd.person_title_text AS PersonTitleTxt,
		--yd.signature_date AS dtSignature
	FROM employers e  INNER JOIN aca.dbo.tax_year_approval yd ON e.employer_id = yd.employer_id
					  INNER JOIN (
							SELECT 
								employer_id,
								tax_year,
								COUNT(employee_id) AS _1095C_count
							FROM aca.dbo.tax_year_1095c_approval
							WHERE get1095C = 1
							GROUP BY employer_id, tax_year
						) AS tya ON tya.employer_id = e.employer_id AND tya.tax_year = yd.tax_year
					  --LEFT JOIN air.ale.dge d ON e.dge_ein = d.ein --might look at aca.dbo.tax_year_approval	
					  LEFT JOIN aca.dbo.[state] s ON s.state_id = yd.state_id				 
	WHERE yd.tax_year = @taxYearId

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO



