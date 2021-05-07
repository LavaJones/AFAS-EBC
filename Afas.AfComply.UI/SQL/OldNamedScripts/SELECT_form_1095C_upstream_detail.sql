USE aca
-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
AlTER PROCEDURE dbo.SELECT_form_1095C_upstream_detail 
	-- Add the parameters for the stored procedure here
	@employerId int,
	@taxYearId int
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
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
	FROM air.emp.employee a INNER JOIN air.emp.yearly_detail e ON a.employee_id = e.employee_id
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
	FROM air.emp.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
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
	FROM air.emp.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
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
	FROM air.emp.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
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
	FROM air.emp.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
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
	FROM air.emp.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
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
	FROM air.emp.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
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
	FROM air.emp.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
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
	FROM air.emp.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
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
	FROM air.emp.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
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
	FROM air.emp.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
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
	FROM air.emp.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
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
	FROM air.emp.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
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
	FROM air.emp.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE a.employer_id = @employerId AND b.year_id = @taxYearId AND m.month_id = 12
	) i ON i.employee_id = ec.employee_id
	WHERE ec.employee_id = i.employee_id

    -- Insert statements for procedure here
	SELECT
		a.employer_id,
		a.employee_id,
		--a.test_id AS TestScenarioId,
		a.employee_control_name AS PersonNameControlTxt,
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
							INNER JOIN air.emp.yearly_detail yd ON yd.employee_id = a.employee_id
							INNER JOIN air.ale.employer e ON e.employer_id = a.employer_id
	WHERE a.employer_id = @employerId 

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
