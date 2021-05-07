USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId int, @taxYearId SMALLINT)

-- Returns a joined table between covered_individual and covered_individual_monthly_detail.
-- It is _never_ to be filtered in this function against the approval table. That belongs upstream.

RETURNS TABLE 

AS

RETURN

	SELECT
		ci.[covered_individual_id], 
		ci.[employer_id],
		ci.[employee_id],
		ci.[first_name],
		ci.[middle_name],
		ci.[last_name],
		ci.[name_suffix],
		ci.[ssn],
		ci.[birth_date],
		ci.[annual_coverage_indicator],	-- broken in the legacy code, passing on the busted value for the time being. gc5
		cimd.[cim_id],
		tf.[year_id],
		tf.[month_id],
		cimd.[time_frame_id],
		cimd.[covered_indicator]
	FROM
		[air].[emp].[covered_individual] ci
		INNER JOIN [air].[emp].[covered_individual_monthly_detail] cimd ON (ci.[covered_individual_id] = cimd.[covered_individual_id])
		INNER JOIN [air].[gen].[time_frame] tf ON (cimd.[time_frame_id] = tf.[time_frame_id])
	WHERE
		ci.[employer_id] = @employerId
			AND
		tf.[year_id] = @taxYearId

GO

GRANT SELECT ON [dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear] TO [aca-user] AS [DBO]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SELECT_employee_with_employer_info]
	@employerID int,
	@taxYearId int
AS
BEGIN TRY

	-- print routines. gc5

	SET NOCOUNT ON;

	DECLARE @SanitizedTaxYear1095CApprovalsWithPrinted TABLE
	(
		tax_year INT INDEX sty1a_tax_year NONCLUSTERED,
		employer_id INT INDEX sty1a_employer_id NONCLUSTERED,
		employee_id INT INDEX sty1a_employee_id NONCLUSTERED,
		get1095C BIT INDEX sty1a_get_1095c NONCLUSTERED,
		printed BIT INDEX sty1a_printed NONCLUSTERED,
		INDEX sty1a_tax_year_employer_id_employee_id NONCLUSTERED(tax_year, employer_id, employee_id),
		INDEX sty1a_tax_year_employee_id NONCLUSTERED(tax_year, employee_id)
	)

	-- Patch around the fact that certain groups have multiple approvals. gc5
	-- does not filter on tax_year since it is used in many places to fix issues. gc5
	INSERT INTO @SanitizedTaxYear1095CApprovalsWithPrinted
	SELECT DISTINCT
		ty1a.tax_year,
		ty1a.employer_id,
		ty1a.employee_id,
		ty1a.get1095C,
		ty1a.printed
	FROM
		[aca].[dbo].[tax_year_1095c_approval] ty1a
	WHERE
		(ty1a.employer_id = @employerId)
			AND
		ty1a.get1095C = 1;

	WITH approvedEmployeesGetting1095 AS
      (
		  SELECT 
			air_ee.employee_id, 
			air_ee.employer_id,
			ee.ResourceId,
			air_ee.first_name AS Fname, 
			air_ee.middle_name AS Mi, 
			air_ee.last_name AS Lname, 
			air_ee.ssn,
			air_ee.name_suffix AS Suffix, 
			air_ee.[address] AS [Address],
			air_ee.city AS City, 
			air_ee.state_code AS [State], 
			air_ee.zipcode AS ZIP,
			ee.dob
		FROM
			[air].[emp].[employee] air_ee
			INNER JOIN [aca].[dbo].[employee] ee ON (air_ee.employee_id = ee.employee_id) 
			INNER JOIN @SanitizedTaxYear1095CApprovalsWithPrinted ty1a ON (ty1a.employee_id = air_ee.employee_id AND ty1a.tax_year = @taxYearId)
		WHERE
			air_ee.employer_id = @employerID 
				AND
			ty1a.get1095C = 1
				AND
			ty1a.printed = 0 
      )
      SELECT DISTINCT 
		aeg1.employee_id,
	    er.ResourceId as EmployerResourceId,
		air_er.name as BusinessNameLine1,
		air_er.[address] as BusinessAddressLine1Txt, 
		air_er.city as BusinessCityNm, 
		air_er.state_code AS BusinessUSStateCd,
		air_er.zipcode AS BusinessUSZIPCd, 
		air_er.ein as EIN,
        air_er.contact_telephone AS ContactPhoneNum,
		aeg1.ResourceId AS EmployeeResourceId,
		aeg1.Fname,
		aeg1.Mi,
		aeg1.Lname,
		aeg1.Suffix,
		aeg1.City,
		aeg1.[State],
		aeg1.[Address],
		aeg1.ZIP,
		aeg1.ssn,
		aeg1.dob as PersonBirthDt
      FROM
		-- declared in the with statement upstream.
		approvedEmployeesGetting1095 aeg1
		INNER JOIN [air].[ale].[employer] air_er ON (air_er.employer_id = aeg1.employer_id)
		INNER JOIN [aca].[dbo].[employer] er ON (er.employer_id = aeg1.employer_id)		
      ORDER BY
		aeg1.Fname

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
-- Create date: <4/7/2016>
-- Description:	<This stored procedure is meant to return all LINE 3 info.>
-- Changes:
--			
-- =============================================
ALTER PROCEDURE [dbo].[sp_AIR_SELECT_employee_LINE3_coverage]
	@employeeID int
AS

BEGIN

-- we need the employer id for the table function below, grab it now. gc5
DECLARE @employerId INT
SELECT
	@employerId = ee.[employer_id]
FROM
	[aca].[dbo].[employee] ee
WHERE
	ee.[employee_id] = @employeeID;

WITH
cim AS
(
	SELECT
		ci.covered_individual_id,
		time_frame_id,
		CAST(covered_indicator AS INT) AS COV_IND
	FROM
		-- hard coded to 2016 to line up with other hard codes. gc5
		[aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, 2016) ci
	WHERE
		ci.employee_id = @employeeID       
),
cim_pivoted AS
(
	SELECT
		-- 2016 values replacing the 2015 values. gc5  [13],[14],[15],[16],[17],[18],[19],[20],[21],[22],[23],[24]
		covered_individual_id, [25], [26], [27], [28], [29], [30], [31], [32], [33], [34], [35], [36]
	FROM
		cim
	PIVOT (
		-- 2016 values replacing the 2015 values. gc5 ([13],[14],[15],[16],[17],[18],[19],[20],[21],[22],[23],[24])
		MAX(COV_IND) FOR time_frame_id IN ([25], [26], [27], [28], [29], [30], [31], [32], [33], [34], [35], [36]) 
	) as cip
)
SELECT
	ci.employee_id,
	ci.first_name,
	ci.last_name,
	ci.ssn,
	ci.birth_date,
	cip.* 
FROM
	cim_pivoted cip
    INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, 2016) ci ON (cip.covered_individual_id = ci.covered_individual_id)
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SELECT_form_1094C_upstream_detail]
	@taxYearId int,
	@employerId int = null -- null is set here but we should _never_ call it in practice for 2016. gc5
AS
BEGIN TRY

	SET NOCOUNT ON;

	DECLARE @SanitizedTaxYear1095CApprovals TABLE
	(
		tax_year INT INDEX sty1a_tax_year NONCLUSTERED,
		employer_id INT INDEX sty1a_employer_id NONCLUSTERED,
		employee_id INT INDEX sty1a_employee_id NONCLUSTERED,
		get1095C BIT INDEX sty1a_get_1095c NONCLUSTERED,
		INDEX sty1a_tax_year_employer_id_employee_id NONCLUSTERED(tax_year, employer_id, employee_id),
		INDEX sty1a_tax_year_employee_id NONCLUSTERED(tax_year, employee_id)
	)

	-- Patch around the fact that certain groups have multiple approvals. gc5
	-- does not filter on tax_year since it is used in many places to fix issues. gc5
	INSERT INTO @SanitizedTaxYear1095CApprovals
	SELECT DISTINCT
		ty1a.tax_year,
		ty1a.employer_id,
		ty1a.employee_id,
		ty1a.get1095C
	FROM
		[aca].[dbo].[tax_year_1095c_approval] ty1a
	WHERE
		(ty1a.employer_id = @employerId OR @employerId IS NULL)
			AND
		ty1a.get1095C = 1;

	WITH employers AS (
		SELECT DISTINCT
			air_er.employer_id,
			air_er.dge_ein,
			air_er.ein AS EmployerEIN,
			er.ResourceId,
			air_er.name AS BusinessNameLine1Txt,
			air_er.[address] AS AddressLine1Txt,
			--a.employer_control_name AS BusinessNameControlTxt,
			air_er.city AS CityNm, 
			air_er.state_code AS USStateCd, 
			air_er.zipcode AS USZIPCd,
			air_er.zipcode_ext AS USZIPExtensionCd,
			air_er.contact_first_name AS PersonFirstNm,
			air_er.contact_middle_name AS PersonMiddleNm,
			air_er.contact_last_name AS PersonLastNm,
			air_er.contact_name_suffix AS SuffixNm,
			air_er.contact_telephone AS ContactPhoneNum
		FROM
			[air].[ale].[employer] air_er 
			INNER JOIN [aca].[dbo].[employer] er ON (air_er.employer_id = er.employer_id) 
			INNER JOIN @SanitizedTaxYear1095CApprovals ty1a ON (air_er.employer_id = ty1a.employer_id AND ty1a.tax_year = @taxYearId)
		WHERE
			(air_er.employer_id = @employerId OR @employerId IS NULL)  
				AND
			ty1a.get1095C = 1
	)

	SELECT
		ROW_NUMBER() OVER (ORDER BY er.employer_id) AS SubmissionId,
		'0' AS CorrectedInd, --appr.employee_yearly_detail
		er.*, -- todo fix! gc5
		tya.dge_name AS GovtBusinessNameLine1Txt,
		tya.dge_ein AS GovtEmployerEIN,
		tya.dge_address AS GovtAddressLine1Txt,
		tya.dge_city AS GovtCityNm,
		s.abbreviation AS GovtUSStateCd, -- dge info is in the air database? gc5
		tya.dge_zip AS GovtUSZIPCd,
		tya.dge_contact_fname AS GovtPersonFirstNm,
		tya.dge_contact_lname AS GovtPersonLastNm,
		tya.dge_phone AS GovtContactPhoneNum,
		CASE WHEN er.dge_ein IS NULL THEN '1' ELSE '0' END AS AuthoritativeTransmittalInd,
		CAST(ty1a._1095C_count AS varchar(max)) AS TotalForm1095CALEMemberCnt, --tax year aca.dbo.tax_year_1095C_approval
		CAST(tya.ale AS varchar(1)) AS AggregatedGroupMemberCd, -- ale tax_year_approval
		'1' AS QualifyingOfferMethodInd, -- probably always true
		'0' AS QlfyOfferMethodTrnstReliefInd, -- set always to false
		CAST(tya.tr_qualified AS varchar(1)) AS Section4980HReliefInd, -- tr_qualified on aca.dbo.tax_year_approval?
		'0' AS NinetyEightPctOfferMethodInd -- set always to false
		--yd.jurat_signature_pin AS JuratSignaturePIN,
		--yd.person_title_text AS PersonTitleTxt,
		--yd.signature_date AS dtSignature
	FROM
		-- query defined with the With statement
		employers er 
		INNER JOIN [aca].[dbo].[tax_year_approval] tya ON (er.employer_id = tya.employer_id)
		INNER JOIN (
			SELECT 
				employer_id,
				COUNT(employee_id) AS _1095C_count
			FROM
				@SanitizedTaxYear1095CApprovals ty1a
			WHERE
				ty1a.get1095C = 1
			GROUP BY
				ty1a.employer_id
		) AS ty1a ON (ty1a.employer_id = er.employer_id)
		--LEFT JOIN air.ale.dge d ON e.dge_ein = d.ein --might look at aca.dbo.tax_year_approval	
		LEFT JOIN aca.dbo.[state] s ON s.state_id = tya.state_id		 
	WHERE
		tya.tax_year = @taxYearId

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
-- Author:		<Ryan, McCully -- based on Obiye's work>
-- Create date: <3/9/2017>
-- Description:	<Pull the monthly details for 1094>
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_ale_member_information_monthly]
	 @taxYearId int,
	 @employerId int
AS
BEGIN TRY

	SET NOCOUNT ON;

	DECLARE @SanitizedTaxYear1095CApprovals TABLE
	(
		tax_year INT INDEX sty1a_tax_year NONCLUSTERED,
		employer_id INT INDEX sty1a_employer_id NONCLUSTERED,
		employee_id INT INDEX sty1a_employee_id NONCLUSTERED,
		get1095C BIT INDEX sty1a_get_1095c NONCLUSTERED,
		INDEX sty1a_tax_year_employer_id_employee_id NONCLUSTERED(tax_year, employer_id, employee_id),
		INDEX sty1a_tax_year_employee_id NONCLUSTERED(tax_year, employee_id)
	)

	-- Patch around the fact that certain groups have multiple approvals. gc5
	-- does not filter on tax_year since it is used in many places to fix issues. gc5
	INSERT INTO @SanitizedTaxYear1095CApprovals
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
		ty1a.get1095C = 1;

	DECLARE @ALEMemberInformationGrp TABLE (		
		employer_id int NOT NULL,
		time_frame_id int,
		ALEMemberFTECnt varchar(max),
		TotalEmployeeCnt varchar(max),
		MecEmployeeCnt varchar(max)
	)

	-- find all employer/timeframe combinations for the particular employer.
	INSERT INTO @ALEMemberInformationGrp (
		employer_id, 
		time_frame_id
	) 
	SELECT DISTINCT
		emd.employer_id, 
		emd.time_frame_id 
	FROM 
		[air].[appr].[employee_monthly_detail] emd 
	WHERE 
		(emd.employer_id = @employerId OR @employerId IS NULL);

	UPDATE @ALEMemberInformationGrp
	SET
		ALEMemberFTECnt = ISNULL(i.EmployeeCountOfOfferStatusPerMonth, '0')
	FROM 
		@ALEMemberInformationGrp ale 
		INNER JOIN (
			--FTE is is people who 
			SELECT
				emd.employer_id, 
				emd.time_frame_id,
				COUNT(*) AS EmployeeCountOfOfferStatusPerMonth
			FROM
				[air].[appr].[employee_monthly_detail] emd 
				 --on an approved form
				INNER JOIN @SanitizedTaxYear1095CApprovals ty1a ON (ty1a.employee_id = emd.employee_id AND ty1a.tax_year = @taxYearId)
				WHERE
					emd.employer_id = @employerId
						AND
					emd.monthly_status_id IN (1) --show Full time for their monthly status
						AND
					(
						emd.safe_harbor_code IS NULL	 
							OR
						NOT (
							--Do NOT count people who say full time but show code 2D for that month
							--Also exclude any full time employee where the codes for the month are 1H 2E (multi-employer)
							emd.safe_harbor_code = '2D' 
								OR 					
							emd.offer_of_coverage_code = '1H' AND emd.safe_harbor_code = '2E'
							)		
					)
				GROUP BY
					emd.employer_id, 
					emd.time_frame_id
			) i ON (i.employer_id = ale.employer_id AND i.time_frame_id = ale.time_frame_id);
	
	--Total employees takes the FTE count and adds people who are part time, variable hour, and in a limited non-assessment period in the status filed
	UPDATE @ALEMemberInformationGrp
	SET
		TotalEmployeeCnt = ISNULL(i.EmployeeCountOfTypePerMonth, '0')
	FROM 
		@ALEMemberInformationGrp ale 
	INNER JOIN (
		SELECT
			emd.employer_id, 
			emd.time_frame_id, 
			COUNT(*) AS EmployeeCountOfTypePerMonth 
		FROM
			[air].[appr].[employee_monthly_detail] emd
			--Include for finalized AND not finalized 
		WHERE
			emd.employer_id = @employerId
				AND
			emd.monthly_status_id NOT IN (4, 5, 7, 8, 9)
		GROUP BY
			emd.employer_id, 
			emd.time_frame_id
	) i ON (i.employer_id = ale.employer_id AND i.time_frame_id = ale.time_frame_id);

	UPDATE @ALEMemberInformationGrp
	SET
		MecEmployeeCnt = ISNULL(i.MecEmployeeCountPerMonth, '0')
	FROM 
		@ALEMemberInformationGrp ale 
		INNER JOIN (
			--For MEC, the denominator is the FTE count. Then the numerator is just those people from the denominator 
			--FTE is is people who 
			SELECT
				emd.employer_id, 
				emd.time_frame_id,
				COUNT(*) AS MecEmployeeCountPerMonth
			FROM
				[air].[appr].[employee_monthly_detail] emd 
				-- on an approved form
				INNER JOIN @SanitizedTaxYear1095CApprovals ty1a ON (ty1a.employee_id = emd.employee_id AND ty1a.tax_year = @taxYearId)
			WHERE
				emd.employer_id = @employerId
					AND
				emd.monthly_status_id IN (1) --show Full time for their monthly status
					AND
				(
					emd.safe_harbor_code IS NULL	 
						OR
					NOT (
					--Do NOT count people who say full time but show code 2D for that month
					--Also exclude any full time employee where the codes for the month are 1H 2E (multi-employer)
					emd.safe_harbor_code = '2D' 
						OR 					
					emd.offer_of_coverage_code = '1H' AND emd.safe_harbor_code = '2E'
					)		
				)
					AND
				emd.mec_offered = 1 --who also have the MEC box checked
			GROUP BY
				emd.employer_id, 
				emd.time_frame_id
		) i ON (i.employer_id = ale.employer_id AND i.time_frame_id = ale.time_frame_id);

	SELECT
		ale.employer_id,
		ale.time_frame_id,
		ale.ALEMemberFTECnt,
		ale.TotalEmployeeCnt,
		ale.MecEmployeeCnt
	FROM
		@ALEMemberInformationGrp ale
	ORDER BY
		ale.employer_id, 
		ale.time_frame_id

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SELECT_employee_covered_individuals]
	@employeeId int,
	@taxYearId int
AS
BEGIN TRY
	
	-- IRS routines. gc5

	DECLARE @SanitizedTaxYear1095CApprovals TABLE
	(
		tax_year INT INDEX sty1a_tax_year NONCLUSTERED,
		employer_id INT INDEX sty1a_employer_id NONCLUSTERED,
		employee_id INT INDEX sty1a_employee_id NONCLUSTERED,
		get1095C BIT INDEX sty1a_get_1095c NONCLUSTERED,
		INDEX sty1a_tax_year_employer_id_employee_id NONCLUSTERED(tax_year, employer_id, employee_id),
		INDEX sty1a_tax_year_employee_id NONCLUSTERED(tax_year, employee_id)
	)

	-- Patch around the fact that certain groups have multiple approvals. gc5
	INSERT INTO @SanitizedTaxYear1095CApprovals
	SELECT DISTINCT
		ty1a.tax_year,
		ty1a.employer_id,
		ty1a.employee_id,
		ty1a.get1095C
	FROM
		[aca].[dbo].[tax_year_1095c_approval] ty1a
	WHERE
		ty1a.employee_id = @employeeId
			AND
		ty1a.tax_year = @taxYearId
			AND
		ty1a.get1095C = 1;

	-- pivot back to the employerId from the above table, should always be one entry for the employer. gc5
	DECLARE @employerId INT;
	SELECT
		@employerId = employer_id 
	FROM
		@SanitizedTaxYear1095CApprovals sty1a
	WHERE
		sty1a.employee_id = @employeeId;

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

	-- grab all of the employees and dependents into the table, all bit flags are empty.
	-- we filter through the approval table for this insert then use the in memory table to drive all other updates.
	INSERT INTO @EmployeeCoveredIndividuals(
		employee_id,
		covered_individual_id,
		PersonFirstNm,
		PersonMiddleNm,
		PersonLastNm,
		SuffixNm,
		SSN,
		DOB,
		CoveredIndividualAnnualInd
	)
	SELECT
		ci.employee_id,
		ci.covered_individual_id,
		ci.first_name,
		ci.middle_name,
		ci.last_name,
		ci.name_suffix,
		ci.ssn,
		ci.birth_date,
		'0' as CoveredIndividualAnnualInd	-- hard coded to null since the eyd table does not seem to update correctly.
	FROM
		[air].[emp].[employee] air_ee
		INNER JOIN @SanitizedTaxYear1095CApprovals ty1a ON (ty1a.employee_id = air_ee.employee_id AND ty1a.tax_year = @taxYearId)
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(air_ee.employee_id = ci.employee_id)
						AND
					(ci.year_id = @taxYearId)
				)
	WHERE
		air_ee.employee_id = @employeeId
			AND
		ty1a.get1095C = 1;

	-- work around the faulty annual coverage indicator.
	UPDATE @EmployeeCoveredIndividuals
	SET
		CoveredIndividualAnnualInd = '1'
	FROM
		@EmployeeCoveredIndividuals ed
		INNER JOIN (SELECT
						ci.covered_individual_id,
						SUM(CASE WHEN ci.covered_indicator = 1 THEN 1 ELSE 0 END) AS total_covered_indicator
					FROM
						[aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
					GROUP BY
						ci.covered_individual_id
			) ci ON (ci.covered_individual_id = ed.covered_individual_id)
	WHERE ci.total_covered_indicator = 12;

	--January values--
	UPDATE @EmployeeCoveredIndividuals
	SET
		JanuaryInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '' ELSE CAST(ci.[covered_indicator] AS varchar(1)) END
	FROM
		@EmployeeCoveredIndividuals ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.[year_id] = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 1;

	--Feburary values--
	UPDATE @EmployeeCoveredIndividuals
	SET
		FebruaryInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '' ELSE CAST(ci.[covered_indicator] AS varchar(1)) END
	FROM
		@EmployeeCoveredIndividuals ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.[year_id] = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 2;
	
	--March values--
	UPDATE @EmployeeCoveredIndividuals
	SET
		MarchInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '' ELSE CAST(ci.[covered_indicator] AS varchar(1)) END
	FROM
		@EmployeeCoveredIndividuals ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.[year_id] = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 3;

	--April values--
	UPDATE @EmployeeCoveredIndividuals
	SET
		AprilInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '' ELSE CAST(ci.[covered_indicator] AS varchar(1)) END
	FROM
		@EmployeeCoveredIndividuals ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.[year_id] = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 4;

	--May values--
	UPDATE @EmployeeCoveredIndividuals
	SET
		MayInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '' ELSE CAST(ci.[covered_indicator] AS varchar(1)) END
	FROM
		@EmployeeCoveredIndividuals ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.[year_id] = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 5;

	--Jun values--
	UPDATE @EmployeeCoveredIndividuals
	SET
		JuneInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '' ELSE CAST(ci.[covered_indicator] AS varchar(1)) END
	FROM
		@EmployeeCoveredIndividuals ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.[year_id] = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 6;

	--Jul values--
	UPDATE @EmployeeCoveredIndividuals
	SET
		JulyInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '' ELSE CAST(ci.[covered_indicator] AS varchar(1)) END
	FROM
		@EmployeeCoveredIndividuals ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.[year_id] = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 7;

	--Aug values--
	UPDATE @EmployeeCoveredIndividuals
	SET
		AugustInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '' ELSE CAST(ci.[covered_indicator] AS varchar(1)) END
	FROM
		@EmployeeCoveredIndividuals ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.[year_id] = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 8;

	--Sep values--
	UPDATE @EmployeeCoveredIndividuals
	SET
		SeptemberInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '' ELSE CAST(ci.[covered_indicator] AS varchar(1)) END
	FROM
		@EmployeeCoveredIndividuals ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.[year_id] = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 9;

	--Oct values--
	UPDATE @EmployeeCoveredIndividuals
	SET
		OctoberInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '' ELSE CAST(ci.[covered_indicator] AS varchar(1)) END
	FROM
		@EmployeeCoveredIndividuals ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.[year_id] = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 10;

	--Nov values--
	UPDATE @EmployeeCoveredIndividuals
	SET
		NovemberInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '' ELSE CAST(ci.[covered_indicator] AS varchar(1)) END
	FROM
		@EmployeeCoveredIndividuals ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.[year_id] = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 11;

	--Dec values--
	UPDATE @EmployeeCoveredIndividuals
	SET
		DecemberInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '' ELSE CAST(ci.[covered_indicator] AS varchar(1)) END
	FROM
		@EmployeeCoveredIndividuals ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.[year_id] = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 12;

	SELECT
		ed.*
	FROM
		@EmployeeCoveredIndividuals ed;

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SELECT_employee_offer_and_coverage]
	@employeeID int,
	@taxYear int
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @SanitizedTaxYear1095CApprovals TABLE
	(
		tax_year INT INDEX sty1a_tax_year NONCLUSTERED,
		employer_id INT INDEX sty1a_employer_id NONCLUSTERED,
		employee_id INT INDEX sty1a_employee_id NONCLUSTERED,
		get1095C BIT INDEX sty1a_get_1095c NONCLUSTERED,
		INDEX sty1a_tax_year_employer_id_employee_id NONCLUSTERED(tax_year, employer_id, employee_id),
		INDEX sty1a_tax_year_employee_id NONCLUSTERED(tax_year, employee_id)
	)

	-- Patch around the fact that certain groups have multiple approvals. gc5
	INSERT INTO @SanitizedTaxYear1095CApprovals
	SELECT DISTINCT
		ty1a.tax_year,
		ty1a.employer_id,
		ty1a.employee_id,
		ty1a.get1095C
	FROM
		[aca].[dbo].[tax_year_1095c_approval] ty1a
	WHERE
		ty1a.employee_id = @employeeId
			AND
		ty1a.tax_year = @taxYear
			AND
		ty1a.get1095C = 1;

	SELECT 
		emd.employee_id,
		emd.time_frame_id,
		emd.employer_id, 
		emd.offer_of_coverage_code, 
		emd.monthly_status_id, 
		emd.share_lowest_cost_monthly_premium,
		m.name,
		tf.year_id,
		emd.safe_harbor_code, 
		emd.mec_offered, 
		emd.insurance_type_id
	FROM
		[air].[appr].[employee_monthly_detail] emd
		INNER JOIN @SanitizedTaxYear1095CApprovals ty1a ON (emd.employee_id = ty1a.employee_id AND ty1a.tax_year = @taxYear)
		INNER JOIN [air].[gen].[time_frame] tf ON (emd.time_frame_id = tf.time_frame_id)
		INNER JOIN [air].[gen].[month] m ON (tf.month_id = m.month_id)
	WHERE
		emd.employee_id = @employeeID
			AND
		ty1a.get1095C = 1
	ORDER BY
		m.month_id

END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SELECT_employee_tax_year_dependents]
	@employeeId int,
	@taxYearId int
AS
BEGIN TRY
	
	-- print routines. gc5

	DECLARE @SanitizedTaxYear1095CApprovals TABLE
	(
		tax_year INT INDEX sty1a_tax_year NONCLUSTERED,
		employer_id INT INDEX sty1a_employer_id NONCLUSTERED,
		employee_id INT INDEX sty1a_employee_id NONCLUSTERED,
		get1095C BIT INDEX sty1a_get_1095c NONCLUSTERED,
		INDEX sty1a_tax_year_employer_id_employee_id NONCLUSTERED(tax_year, employer_id, employee_id),
		INDEX sty1a_tax_year_employee_id NONCLUSTERED(tax_year, employee_id)
	)

	-- Patch around the fact that certain groups have multiple approvals. gc5
	-- does not filter on tax_year since it is used in many places to fix issues. gc5
	INSERT INTO @SanitizedTaxYear1095CApprovals
	SELECT DISTINCT
		ty1a.tax_year,
		ty1a.employer_id,
		ty1a.employee_id,
		ty1a.get1095C
	FROM
		[aca].[dbo].[tax_year_1095c_approval] ty1a
	WHERE
		ty1a.employee_id = @employeeId
			AND
		ty1a.tax_year = @taxYearId
			AND
		ty1a.get1095C = 1;

	-- pivot back to the employerId from the above table, should always be one entry for the employer. gc5
	DECLARE @employerId INT;
	SELECT
		@employerId = employer_id 
	FROM
		@SanitizedTaxYear1095CApprovals sty1a
	WHERE
		sty1a.employee_id = @employeeId

	-- in memory table to build up all of the answers.
	DECLARE @EmployeeDependents TABLE (
		employee_id int NOT NULL,
		covered_individual_id int,
		Fname varchar(max),
		Mi varchar(max),
		Lname varchar(max),
		ssn varchar(max),
		dob date,
		Jan bit,
		Feb bit,
		Mar bit,
		Apr bit,
		May bit,
		Jun bit,
		Jul bit,
		Aug bit,
		Sep bit,
		Oct bit,
		Nov bit,
		[Dec] bit
	)

	-- grab all of the employees and dependents into the table, all bit flags are empty.
	-- we filter through the approval table for this insert then use the in memory table to drive all other updates.
	INSERT INTO @EmployeeDependents (
		employee_id,
		covered_individual_id,
		Fname,
		Mi,
		Lname,
		ssn,
		dob,
		Jan,
		Feb,
		Mar,
		Apr,
		May,
		Jun,
		Jul,
		Aug,
		Sep,
		Oct,
		Nov,
		[Dec]
	)
	SELECT
		ci.employee_id,
		ci.covered_individual_id,
		ci.first_name AS Fname,
		ci.middle_name AS Mi,
		ci.last_name AS Lname,
		ci.ssn,
		ci.birth_date AS dob,
		0 AS Jan,
		0 AS Feb,
		0 AS Mar,
		0 AS Apr,
		0 AS May,
		0 AS Jun,
		0 AS Jul,
		0 AS Aug,
		0 AS Sep,
		0 AS Oct,
		0 AS Nov,
		0 AS [Dec]
	FROM
		[air].[emp].[employee] air_ee
		INNER JOIN @SanitizedTaxYear1095CApprovals ty1a ON (ty1a.employee_id = air_ee.employee_id AND ty1a.tax_year = @taxYearId)
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(air_ee.employee_id = ci.employee_id)
						AND
					(ci.year_id = @taxYearId)
				)
	WHERE
		air_ee.employee_id = @employeeId
			AND
		ty1a.get1095C = 1

	-- January values for everyone, hence the covered_individual_id vs. employee_id
	UPDATE @EmployeeDependents
	SET
		Jan = ci.[covered_indicator]
	FROM
		@EmployeeDependents ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.year_id = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 1

	-- February values for everyone, hence the covered_individual_id vs. employee_id
	UPDATE @EmployeeDependents
	SET
		Feb = ci.[covered_indicator]
	FROM
		@EmployeeDependents ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.year_id = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 2
	
	-- March values for everyone, hence the covered_individual_id vs. employee_id
	UPDATE @EmployeeDependents
	SET
		Mar = ci.[covered_indicator]
	FROM
		@EmployeeDependents ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.year_id = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 3

	-- April values for everyone, hence the covered_individual_id vs. employee_id
	UPDATE @EmployeeDependents
	SET
		Apr = ci.[covered_indicator]
	FROM
		@EmployeeDependents ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.year_id = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 4

	-- May values for everyone, hence the covered_individual_id vs. employee_id
	UPDATE @EmployeeDependents
	SET
		May = ci.[covered_indicator]
	FROM
		@EmployeeDependents ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.year_id = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 5

	-- June values for everyone, hence the covered_individual_id vs. employee_id
	UPDATE @EmployeeDependents
	SET
		Jun = ci.[covered_indicator]
	FROM
		@EmployeeDependents ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.year_id = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 6

	-- July values for everyone, hence the covered_individual_id vs. employee_id
	UPDATE @EmployeeDependents
	SET
		Jul = ci.[covered_indicator]
	FROM
		@EmployeeDependents ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.year_id = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 7

	-- August values for everyone, hence the covered_individual_id vs. employee_id
	UPDATE @EmployeeDependents
	SET
		Aug = ci.[covered_indicator]
	FROM
		@EmployeeDependents ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.year_id = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 8

	-- September values for everyone, hence the covered_individual_id vs. employee_id
	UPDATE @EmployeeDependents
	SET
		Sep = ci.[covered_indicator]
	FROM
		@EmployeeDependents ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.year_id = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 9

	-- October values for everyone, hence the covered_individual_id vs. employee_id
	UPDATE @EmployeeDependents
	SET
		Oct = ci.[covered_indicator]
	FROM
		@EmployeeDependents ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.year_id = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 10

	-- November values for everyone, hence the covered_individual_id vs. employee_id
	UPDATE @EmployeeDependents
	SET
		Nov = ci.[covered_indicator]
	FROM
		@EmployeeDependents ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.year_id = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 11

	-- December values for everyone, hence the covered_individual_id vs. employee_id
	UPDATE @EmployeeDependents
	SET
		[Dec] = ci.[covered_indicator]
	FROM
		@EmployeeDependents ed
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci
			ON (
					(ed.[covered_individual_id] = ci.[covered_individual_id])
						AND
					(ci.year_id = @taxYearId)
				)
	WHERE
		ed.employee_id = ci.employee_id
			AND
		ci.month_id = 12

	SELECT
		ed.*
	FROM
		@EmployeeDependents ed

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SELECT_form_1095C_upstream_detail] 
	@employerId int,
	@taxYearId int
AS
BEGIN TRY

	SET NOCOUNT ON;

	-- IRS transmission version.

	DECLARE @SanitizedTaxYear1095CApprovals TABLE
	(
		tax_year INT INDEX sty1a_tax_year NONCLUSTERED,
		employer_id INT INDEX sty1a_employer_id NONCLUSTERED,
		employee_id INT INDEX sty1a_employee_id NONCLUSTERED,
		get1095C BIT INDEX sty1a_get_1095c NONCLUSTERED,
		INDEX sty1a_tax_year_employer_id_employee_id NONCLUSTERED(tax_year, employer_id, employee_id),
		INDEX sty1a_tax_year_employee_id NONCLUSTERED(tax_year, employee_id)
	)

	-- Patch around the fact that certain groups have multiple approvals. gc5
	INSERT INTO @SanitizedTaxYear1095CApprovals
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

	DECLARE @SafeMonthlyDetails TABLE
	(
		employee_id INT INDEX smd_employee_id NONCLUSTERED,
		offer_of_coverage_code varchar(max),
		share_lowest_cost_monthly_premium varchar(max),
		month_id INT INDEX smd_month_id NONCLUSTERED,
		safe_harbor_code varchar(max),
		insurance_type_id INT INDEX smd_insurance_type_id NONCLUSTERED,
		INDEX smd_employee_id_month_id NONCLUSTERED(employee_id, month_id)
	)

	INSERT INTO @SafeMonthlyDetails
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

	-- pull through the approval table filter here so everything else can drive from the in memory table. gc5
	INSERT INTO @EmployeeCoverageDetails (
		employee_id
	)
	SELECT 
		air_ee.employee_id
	FROM
		[air].[emp].[employee] air_ee
		INNER JOIN @SanitizedTaxYear1095CApprovals ty1a ON (air_ee.employee_id = ty1a.employee_id AND ty1a.tax_year = @taxYearId)
	WHERE
		air_ee.employer_id = @employerId
			AND
		ty1a.get1095C = 1;

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
		INNER JOIN @SafeMonthlyDetails i ON (i.employee_id = ec.employee_id)
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
		INNER JOIN @SafeMonthlyDetails i ON (i.employee_id = ec.employee_id)
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
		INNER JOIN @SafeMonthlyDetails i ON (i.employee_id = ec.employee_id)
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
		INNER JOIN @SafeMonthlyDetails i ON (i.employee_id = ec.employee_id)
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
		INNER JOIN @SafeMonthlyDetails i ON (i.employee_id = ec.employee_id)
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
		INNER JOIN @SafeMonthlyDetails i ON (i.employee_id = ec.employee_id)
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
		INNER JOIN @SafeMonthlyDetails i ON (i.employee_id = ec.employee_id)
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
		INNER JOIN @SafeMonthlyDetails i ON (i.employee_id = ec.employee_id)
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
		INNER JOIN @SafeMonthlyDetails i ON (i.employee_id = ec.employee_id)
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
		INNER JOIN @SafeMonthlyDetails i ON (i.employee_id = ec.employee_id)
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
		INNER JOIN @SafeMonthlyDetails i ON (i.employee_id = ec.employee_id)
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
		INNER JOIN @SafeMonthlyDetails i ON (i.employee_id = ec.employee_id)
	WHERE
		ec.employee_id = i.employee_id
			AND
		i.month_id = 12;

	DECLARE @SelfInsuredTable TABLE
	(
		employee_id INT INDEX sit_employee_id NONCLUSTERED,
		indicator INT NULL
	)

	INSERT INTO @SelfInsuredTable
	SELECT DISTINCT
		smd.employee_id,
		NULL as indicator
	FROM
		@SafeMonthlyDetails smd

	UPDATE @SelfInsuredTable
	SET
		indicator = 1
	FROM
		@SelfInsuredTable sit
		INNER JOIN [aca].[dbo].[ufnCoveredIndividualDetailsForEmployerAndTaxYear](@employerId, @taxYearId) ci ON (sit.employee_id = ci.employee_id)
	WHERE
		ci.employer_id = @employerId;

	SELECT
		air_ee.employer_id,
		air_ee.employee_id,
		--a.test_id AS TestScenarioId,
		--a.employee_control_name AS PersonNameControlTxt,
		ee.ResourceId,
		ROW_NUMBER() OVER (ORDER BY air_ee.employee_id) AS RecordId,
		'0' AS CorrectedInd,
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
		LEFT JOIN @SelfInsuredTable sit ON (air_ee.employee_id = sit.employee_id)
	WHERE
		air_ee.employer_id = @employerId 

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH

GO
