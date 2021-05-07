USE aca
GO

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
CREATE PROCEDURE SELECT_form_1094C_upstream_detail
	-- Add the parameters for the stored procedure here
	@taxYearId int,
	@employerId int = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	WITH employers AS (
    -- Insert statements for procedure here
	SELECT DISTINCT
		a.employer_id,
		a.ein AS EmployerEIN,
		c.ResourceId,
		a.name AS BusinessNameLine1Txt,
		a.[address] AS AddressLine1Txt,
		a.employer_control_name AS BusinessNameControlTxt,
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
		INNER JOIN air.emp.yearly_detail e ON a.employer_id = e.employer_id
	WHERE (e.employer_id = @employerId OR @employerId IS NULL)  
		AND e._1095C = 1
		AND e.year_id = @taxYearId
	)

	SELECT
		ROW_NUMBER() OVER (ORDER BY employer_id) AS SubmissionId,
		'0' AS CorrectedInd,
		*
	FROM employers

END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE dbo.SELECT_form_1095C_upstream_detail 
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
		JanSafeHaborCd varchar(max),
		FebSafeHaborCd varchar(max),
		MarSafeHaborCd varchar(max),
		AprSafeHaborCd varchar(max),
		MaySafeHaborCd varchar(max),
		JunSafeHaborCd varchar(max),
		JulSafeHaborCd varchar(max),
		AugSafeHaborCd varchar(max),
		SepSafeHaborCd varchar(max),
		OctSafeHaborCd varchar(max),
		NovSafeHaborCd varchar(max),
		DecSafeHaborCd varchar(max)
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
	SET JanOfferCd = ISNULL(i.offer_of_coverage_code,''), JanSafeHaborCd = ISNULL(i.safe_harbor_code,''), JanuaryAmt = convert(varchar(max),i.monthly_hours)
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
	SET FebOfferCd = ISNULL(i.offer_of_coverage_code,''), FebSafeHaborCd = ISNULL(i.safe_harbor_code,''), FebruaryAmt = convert(varchar(max),i.monthly_hours)
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
	SET MarOfferCd = ISNULL(i.offer_of_coverage_code,''), MarSafeHaborCd = ISNULL(i.safe_harbor_code,''), MarchAmt = convert(varchar(max),i.monthly_hours)
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
	SET AprOfferCd = ISNULL(i.offer_of_coverage_code,''), AprSafeHaborCd = ISNULL(i.safe_harbor_code,''), AprilAmt = convert(varchar(max),i.monthly_hours)
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
	SET MayOfferCd = ISNULL(i.offer_of_coverage_code,''), MaySafeHaborCd = ISNULL(i.safe_harbor_code,''), MayAmt = convert(varchar(max),i.monthly_hours)
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
	SET JunOfferCd = ISNULL(i.offer_of_coverage_code,''), JunSafeHaborCd = ISNULL(i.safe_harbor_code,''), JuneAmt = convert(varchar(max),i.monthly_hours)
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
	SET JulOfferCd = ISNULL(i.offer_of_coverage_code,''), JulSafeHaborCd = ISNULL(i.safe_harbor_code,''), JulyAmt = convert(varchar(max),i.monthly_hours)
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
	SET AugOfferCd = ISNULL(i.offer_of_coverage_code,''), AugSafeHaborCd = ISNULL(i.safe_harbor_code,''), AugustAmt = convert(varchar(max),i.monthly_hours)
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
	SET SepOfferCd = ISNULL(i.offer_of_coverage_code,''), SepSafeHaborCd = ISNULL(i.safe_harbor_code,''), SeptemberAmt = convert(varchar(max),i.monthly_hours)
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
	SET OctOfferCd = ISNULL(i.offer_of_coverage_code,''), OctSafeHaborCd = ISNULL(i.safe_harbor_code,''), OctoberAmt = convert(varchar(max),i.monthly_hours)
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
	SET NovOfferCd = ISNULL(i.offer_of_coverage_code,''), NovSafeHaborCd = ISNULL(i.safe_harbor_code,''), NovemberAmt = convert(varchar(max),i.monthly_hours)
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
	SET DecOfferCd = ISNULL(i.offer_of_coverage_code,''), DecSafeHaborCd = ISNULL(i.safe_harbor_code,''), DecemberAmt = convert(varchar(max),i.monthly_hours)
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
		c.ResourceId,
		ROW_NUMBER() OVER (ORDER BY a.employee_id) AS RecordId,
		'0' AS CorrectedInd,
		a.first_name AS OtherCompletePersonFirstNm,
		a.last_name AS OtherCompletePersonLastNm,
		a.ssn AS SSN,
		c.dob AS DOB,
		a.[address] AS AddressLine1Txt,
		a.city AS CityNm,
		a.state_code AS USStateCd,
		a.zipcode AS USZIPCd,
		ecd.*
	FROM air.emp.employee a INNER JOIN aca.dbo.employee	c ON a.employee_id = c.employee_id		
							INNER JOIN @EmployeeCoverageDetails ecd ON a.employee_id = ecd.employee_id
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
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE SELECT_employee_covered_individuals
	-- Add the parameters for the stored procedure here
	@employeeId int,
	@taxYearId int
AS
BEGIN TRY
	
	DECLARE @EmployeeCoveredIndividuals TABLE (
		employee_id int NOT NULL,
		covered_individual_id int NOT NULL,
		PersonFirstNm varchar(max),
		PersonMiddleNm varchar(max),
		PersonLastNm varchar(max),
		SuffixNm varchar(max),
		SSN varchar(max),
		DOB date,
		CoveredIndividualAnnualInd varchar(1),
		JanuaryInd varchar(1),
		FebruaryInd varchar(1),
		MarchInd varchar(1),
		AprilInd varchar(1),
		MayInd varchar(1),
		JuneInd varchar(1),
		JulyInd varchar(1),
		AugustInd varchar(1),
		SeptemberInd varchar(1),
		OctoberInd varchar(1),
		NovemberInd varchar(1),
		DecemberInd varchar(1)
	)

	INSERT INTO @EmployeeCoveredIndividuals(employee_id,covered_individual_id,PersonFirstNm,PersonMiddleNm,PersonLastNm,SuffixNm,SSN,DOB,CoveredIndividualAnnualInd)
	SELECT
		ci.employee_id,
		ci.covered_individual_id,
		ci.first_name,
		ci.middle_name,
		ci.last_name,
		ci.name_suffix,
		ci.ssn,
		ci.birth_date,
		null	-- hard coded to null since the eyd table does not seem to update correctly.
	FROM air.emp.employee a INNER JOIN air.appr.employee_yearly_detail e ON a.employee_id = e.employee_id
							INNER JOIN air.emp.covered_individual ci ON ci.employee_id = e.employee_id
							INNER JOIN (SELECT 
											covered_individual_id,
											SUM(CASE WHEN covered_indicator = 1 THEN 1 ELSE 0 END) AS total_covered_indicator
										FROM air.emp.covered_individual_monthly_detail
										GROUP BY covered_individual_id
										) cim ON cim.covered_individual_id = ci.covered_individual_id
										
	WHERE e.employee_id = @employeeId
	AND e.year_id = @taxYearId
	AND e._1095C = 1
	AND cim.total_covered_indicator > 0

	-- work around the faulty annual coverage indicator.
	UPDATE @EmployeeCoveredIndividuals
	SET
		CoveredIndividualAnnualInd = '1'
	FROM
		@EmployeeCoveredIndividuals ed
		INNER JOIN (SELECT
						covered_individual_id,
						SUM(CASE WHEN covered_indicator = 1 THEN 1 ELSE 0 END) AS total_covered_indicator
					FROM air.emp.covered_individual_monthly_detail cimd
					GROUP BY cimd.covered_individual_id
				  ) cim ON (cim.covered_individual_id = ed.covered_individual_id)
	WHERE cim.total_covered_indicator = 12;

	--January values--
	UPDATE @EmployeeCoveredIndividuals
	SET JanuaryInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '0' ELSE i.Jan END
	FROM @EmployeeCoveredIndividuals ed INNER JOIN(
	SELECT
		ci.covered_individual_id,
		ci.employee_id, 
		CAST(cim.covered_indicator AS varchar(1)) AS Jan
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 1
	) i ON i.covered_individual_id = ed.covered_individual_id
	WHERE ed.employee_id = i.employee_id

	--Feburary values--
	UPDATE @EmployeeCoveredIndividuals
	SET FebruaryInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '0' ELSE i.Feb END
	FROM @EmployeeCoveredIndividuals ed INNER JOIN(
	SELECT
		ci.covered_individual_id,
		ci.employee_id, 
		CAST(cim.covered_indicator AS varchar(1)) AS Feb
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 2
	) i ON i.covered_individual_id = ed.covered_individual_id
	WHERE ed.employee_id = i.employee_id
	
	--March values--
	UPDATE @EmployeeCoveredIndividuals
	SET MarchInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '0' ELSE i.Mar END
	FROM @EmployeeCoveredIndividuals ed INNER JOIN(
	SELECT
		ci.covered_individual_id,
		ci.employee_id, 
		CAST(cim.covered_indicator AS varchar(1)) AS Mar
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 3
	) i ON i.covered_individual_id = ed.covered_individual_id
	WHERE ed.employee_id = i.employee_id

	--April values--
	UPDATE @EmployeeCoveredIndividuals
	SET AprilInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '0' ELSE i.Apr END
	FROM @EmployeeCoveredIndividuals ed INNER JOIN(
	SELECT
		ci.covered_individual_id,
		ci.employee_id, 
		CAST(cim.covered_indicator AS varchar(1)) AS Apr
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 4
	) i ON i.covered_individual_id = ed.covered_individual_id
	WHERE ed.employee_id = i.employee_id

	--May values--
	UPDATE @EmployeeCoveredIndividuals
	SET MayInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '0' ELSE i.May END
	FROM @EmployeeCoveredIndividuals ed INNER JOIN(
	SELECT
		ci.covered_individual_id,
		ci.employee_id, 
		CAST(cim.covered_indicator AS varchar(1)) AS May
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 5
	) i ON i.covered_individual_id = ed.covered_individual_id
	WHERE ed.employee_id = i.employee_id

	--Jun values--
	UPDATE @EmployeeCoveredIndividuals
	SET JuneInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '0' ELSE i.Jun END
	FROM @EmployeeCoveredIndividuals ed INNER JOIN(
	SELECT
		ci.covered_individual_id,
		ci.employee_id, 
		CAST(cim.covered_indicator AS varchar(1)) AS Jun
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 6
	) i ON i.covered_individual_id = ed.covered_individual_id
	WHERE ed.employee_id = i.employee_id

	--Jul values--
	UPDATE @EmployeeCoveredIndividuals
	SET JulyInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '0' ELSE i.Jul END
	FROM @EmployeeCoveredIndividuals ed INNER JOIN(
	SELECT
		ci.covered_individual_id,
		ci.employee_id, 
		CAST(cim.covered_indicator AS varchar(1)) AS Jul
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 7
	) i ON i.covered_individual_id = ed.covered_individual_id
	WHERE ed.employee_id = i.employee_id

	--Aug values--
	UPDATE @EmployeeCoveredIndividuals
	SET AugustInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '0' ELSE i.Aug END
	FROM @EmployeeCoveredIndividuals ed INNER JOIN(
	SELECT
		ci.covered_individual_id,
		ci.employee_id, 
		CAST(cim.covered_indicator AS varchar(1)) AS Aug
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 8
	) i ON i.covered_individual_id = ed.covered_individual_id
	WHERE ed.employee_id = i.employee_id

	--Sep values--
	UPDATE @EmployeeCoveredIndividuals
	SET SeptemberInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '0' ELSE i.Sep END
	FROM @EmployeeCoveredIndividuals ed INNER JOIN(
	SELECT
		ci.covered_individual_id,
		ci.employee_id, 
		CAST(cim.covered_indicator AS varchar(1)) AS Sep
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 9
	) i ON i.covered_individual_id = ed.covered_individual_id
	WHERE ed.employee_id = i.employee_id

	--Oct values--
	UPDATE @EmployeeCoveredIndividuals
	SET OctoberInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '0' ELSE i.Oct END
	FROM @EmployeeCoveredIndividuals ed INNER JOIN(
	SELECT
		ci.covered_individual_id,
		ci.employee_id, 
		CAST(cim.covered_indicator AS varchar(1)) AS Oct
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 10
	) i ON i.covered_individual_id = ed.covered_individual_id
	WHERE ed.employee_id = i.employee_id

	--Nov values--
	UPDATE @EmployeeCoveredIndividuals
	SET NovemberInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '0' ELSE i.Nov END
	FROM @EmployeeCoveredIndividuals ed INNER JOIN(
	SELECT
		ci.covered_individual_id,
		ci.employee_id, 
		CAST(cim.covered_indicator AS varchar(1)) AS Nov
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 11
	) i ON i.covered_individual_id = ed.covered_individual_id
	WHERE ed.employee_id = i.employee_id

	--Dec values--
	UPDATE @EmployeeCoveredIndividuals
	SET DecemberInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '0' ELSE i.[Dec] END
	FROM @EmployeeCoveredIndividuals ed INNER JOIN(
	SELECT
		ci.covered_individual_id,
		ci.employee_id, 
		CAST(cim.covered_indicator AS varchar(1)) AS [Dec]
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 12
	) i ON i.covered_individual_id = ed.covered_individual_id
	WHERE ed.employee_id = i.employee_id

	SELECT * FROM @EmployeeCoveredIndividuals

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO

GRANT EXECUTE ON SELECT_form_1094C_upstream_detail TO [aca-user] AS [dbo] 
GRANT EXECUTE ON SELECT_form_1095C_upstream_detail TO [aca-user] AS [dbo] 
GRANT EXECUTE ON SELECT_employee_covered_individuals TO [aca-user] AS [dbo]