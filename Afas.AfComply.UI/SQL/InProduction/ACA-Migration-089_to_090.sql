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
ALTER PROCEDURE [dbo].[SELECT_ale_member_information_monthly]
	 @taxYearId int,
	 @employerId int = null
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @ALEMemberInformationGrp TABLE (		
		employer_id int NOT NULL,
		time_frame_id int,
		ALEMemberFTECnt varchar(max),
		TotalEmployeeCnt varchar(max),
		MecEmployeeCnt varchar(max)
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
	--FTE is is people who 
		SELECT
			emd.employer_id, 
			emd.time_frame_id,
			COUNT(*) AS EmployeeCountOfOfferStatusPerMonth
		FROM
			[air].[appr].[employee_monthly_detail] emd 
			inner join 
			[aca].[dbo].[tax_year_1095c_approval] ty --on an approved form
				on 
			ty.employee_id = emd.employee_id AND ty.tax_year = @taxYearId 
		WHERE
			(emd.employer_id = @employerId OR @employerId IS NULL)
				AND
			emd.monthly_status_id IN (1) --show Full time for their monthly status
				AND
			(emd.safe_harbor_code IS NULL	 
				OR
				NOT(
				--Do NOT count people who say full time but show code 2D for that month
				--Also exclude any full time employee where the codes for the month are 1H 2E (multi-employer)
				emd.safe_harbor_code = '2D' 
					OR 					
					(
					emd.offer_of_coverage_code = '1H' AND emd.safe_harbor_code = '2E'
					)
				)		
			)
			AND 
			ty.get1095C = 1 --on an approved form
			--	AND
			--emd.mec_offered = 1
		GROUP BY
			emd.employer_id, 
			emd.time_frame_id
	) i 
	ON 
		i.employer_id = ale.employer_id 
			AND 
		i.time_frame_id = ale.time_frame_id;
	
	--Total employees takes the FTE count and adds people who are part time, variable hour, and in a limited non-assessment period in the status filed
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
			--Include for finalized AND not finalized 
		WHERE
			(emd.employer_id = @employerId OR @employerId IS NULL)
				AND
			emd.monthly_status_id NOT IN (4, 5, 7, 8, 9)
				AND
			(emd.safe_harbor_code IS NULL	 
				OR
				NOT(
				--Do NOT count people who say full time but show code 2D for that month
				--Also exclude any full time employee where the codes for the month are 1H 2E (multi-employer)
				emd.safe_harbor_code = '2D' 
					OR 					
					(
					emd.offer_of_coverage_code = '1H' AND emd.safe_harbor_code = '2E'
					)
				)		
			)
		GROUP BY
			emd.employer_id, 
			emd.time_frame_id
	) i 
	ON 
		i.employer_id = ale.employer_id 
			AND 
		i.time_frame_id = ale.time_frame_id;


	UPDATE 
		@ALEMemberInformationGrp
	SET
		MecEmployeeCnt = ISNULL(i.MecEmployeeCountPerMonth,'0')
	FROM 
		@ALEMemberInformationGrp ale 
	INNER JOIN(
	--For MEC, the denominator is the FTE count. Then the numerator is just those people from the denominator 
	--FTE is is people who 
		SELECT
			emd.employer_id, 
			emd.time_frame_id,
			COUNT(*) AS MecEmployeeCountPerMonth
		FROM
			[air].[appr].[employee_monthly_detail] emd 
			inner join 
			[aca].[dbo].[tax_year_1095c_approval] ty --on an approved form
				on 
			ty.employee_id = emd.employee_id AND ty.tax_year = @taxYearId
		WHERE
			(emd.employer_id = @employerId OR @employerId IS NULL)
				AND
			emd.monthly_status_id IN (1) --show Full time for their monthly status
				AND
			(emd.safe_harbor_code IS NULL	 
				OR
				NOT(
				--Do NOT count people who say full time but show code 2D for that month
				--Also exclude any full time employee where the codes for the month are 1H 2E (multi-employer)
				emd.safe_harbor_code = '2D' 
					OR 					
				emd.offer_of_coverage_code = '1H' AND emd.safe_harbor_code = '2E'
				)		
			)
				AND 
			ty.get1095C = 1 --on an approved form
				AND
			emd.mec_offered = 1 --who also have the MEC box checked
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

