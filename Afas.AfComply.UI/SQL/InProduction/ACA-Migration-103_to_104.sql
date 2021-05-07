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
-- Adjusted to pull from the override table to handle reporting only customers without full counts.
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_ale_member_information_monthly]
	 @taxYearId int,
	 @employerId int
AS
BEGIN TRY

	SET NOCOUNT ON;

	DECLARE @SanitizedTaxYear1095CApprovals TABLE
	(
		tax_year INT ,--INDEX sty1a_tax_year NONCLUSTERED,
		employer_id INT ,--INDEX sty1a_employer_id NONCLUSTERED,
		employee_id INT ,--INDEX sty1a_employee_id NONCLUSTERED,
		get1095C BIT --INDEX sty1a_get_1095c NONCLUSTERED,
		--INDEX sty1a_tax_year_employer_id_employee_id NONCLUSTERED(tax_year, employer_id, employee_id),
		--INDEX sty1a_tax_year_employee_id NONCLUSTERED(tax_year, employee_id)
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

	-- plug in the overrides.
	UPDATE @ALEMemberInformationGrp
	SET
		TotalEmployeeCnt = e1co.TotalEmployeeCnt
	FROM 
		@ALEMemberInformationGrp ale 
		INNER JOIN [aca].[dbo].[Employee1094CountOverride] e1co ON (ale.employer_id = e1co.EmployerId AND e1co.TaxYearId = @taxYearId)
	WHERE
		ale.time_frame_id = e1co.TimeFrameId

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


