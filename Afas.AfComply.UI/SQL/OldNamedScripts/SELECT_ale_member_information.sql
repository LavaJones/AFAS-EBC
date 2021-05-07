Use aca
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
ALTER PROCEDURE SELECT_ale_member_information
	-- Add the parameters for the stored procedure here
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
		annual_mec_offer_indicator bit,
		annual_aag_indicator bit,

		YearlyMinEssentialCvrOffrCd varchar(1),
		YearlyALEMemberFTECnt varchar(max),
		YearlyTotalEmployeeCnt varchar(max),
		YearlyAggregatedGroupInd varchar(1),
		YearlyALESect4980HTrnstReliefCd varchar(1),
		
		JanMinEssentialCvrOffrCd varchar(1),
		JanALEMemberFTECnt varchar(max),
		JanTotalEmployeeCnt varchar(max),
		JanAggregatedGroupInd varchar(1),
		JanALESect4980HTrnstReliefCd varchar(1),

		FebMinEssentialCvrOffrCd varchar(1),
		FebALEMemberFTECnt varchar(max),
		FebTotalEmployeeCnt varchar(max),
		FebAggregatedGroupInd varchar(1),
		FebALESect4980HTrnstReliefCd varchar(1),

		MarMinEssentialCvrOffrCd varchar(1),
		MarALEMemberFTECnt varchar(max),
		MarTotalEmployeeCnt varchar(max),
		MarAggregatedGroupInd varchar(1),
		MarALESect4980HTrnstReliefCd varchar(1),

		AprMinEssentialCvrOffrCd varchar(1),
		AprALEMemberFTECnt varchar(max),
		AprTotalEmployeeCnt varchar(max),
		AprAggregatedGroupInd varchar(1),
		AprALESect4980HTrnstReliefCd varchar(1),

		MayMinEssentialCvrOffrCd varchar(1),
		MayALEMemberFTECnt varchar(max),
		MayTotalEmployeeCnt varchar(max),
		MayAggregatedGroupInd varchar(1),
		MayALESect4980HTrnstReliefCd varchar(1),

		JunMinEssentialCvrOffrCd varchar(1),
		JunALEMemberFTECnt varchar(max),
		JunTotalEmployeeCnt varchar(max),
		JunAggregatedGroupInd varchar(1),
		JunALESect4980HTrnstReliefCd varchar(1),

		JulMinEssentialCvrOffrCd varchar(1),
		JulALEMemberFTECnt varchar(max),
		JulTotalEmployeeCnt varchar(max),
		JulAggregatedGroupInd varchar(1),
		JulALESect4980HTrnstReliefCd varchar(1),

		AugMinEssentialCvrOffrCd varchar(1),
		AugALEMemberFTECnt varchar(max),
		AugTotalEmployeeCnt varchar(max),
		AugAggregatedGroupInd varchar(1),
		AugALESect4980HTrnstReliefCd varchar(1),

		SeptMinEssentialCvrOffrCd varchar(1),
		SeptALEMemberFTECnt varchar(max),
		SeptTotalEmployeeCnt varchar(max),
		SeptAggregatedGroupInd varchar(1),
		SeptALESect4980HTrnstReliefCd varchar(1),

		OctMinEssentialCvrOffrCd varchar(1),
		OctALEMemberFTECnt varchar(max),
		OctTotalEmployeeCnt varchar(max),
		OctAggregatedGroupInd varchar(1),
		OctALESect4980HTrnstReliefCd varchar(1),

		NovMinEssentialCvrOffrCd varchar(1),
		NovALEMemberFTECnt varchar(max),
		NovTotalEmployeeCnt varchar(max),
		NovAggregatedGroupInd varchar(1),
		NovALESect4980HTrnstReliefCd varchar(1),

		DecMinEssentialCvrOffrCd varchar(1),
		DecALEMemberFTECnt varchar(max),
		DecTotalEmployeeCnt varchar(max),
		DecAggregatedGroupInd varchar(1),
		DecALESect4980HTrnstReliefCd varchar(1)

	);

	--Yearly Values
	INSERT INTO @ALEMemberInformationGrp(
		employer_id,
		annual_mec_offer_indicator,
		annual_aag_indicator,
		YearlyMinEssentialCvrOffrCd,
		YearlyALEMemberFTECnt,
		YearlyTotalEmployeeCnt,
		YearlyAggregatedGroupInd,
		YearlyALESect4980HTrnstReliefCd)
	SELECT
		a.employer_id,
		a.annual_mec_offer_indicator,
		a.annual_aag_indicator,
		CAST(a.annual_mec_offer_indicator AS varchar(1)) AS MinEssentialCvrOffrCd,
		a.annual_fulltime_employee_count AS ALEMemberFTECnt,
		a.total_employee_count_through_year AS TotalEmployeeCnt,
		CAST(a.annual_aag_indicator AS varchar(1)) AS AggregatedGroupInd,
		a._4980H_transition_relief_code AS ALESect4980HTrnstReliefCd
	FROM air.ale.yearly_detail a 
	WHERE (a.employer_id = @employerId OR @employerId IS NULL)
	AND a.year_id = @taxYearId

	--Jan values--
	UPDATE @ALEMemberInformationGrp
	SET --JanMinEssentialCvrOffrCd = (CASE WHEN ale.annual_mec_offer_indicator = 1 THEN NULL ELSE i.MinEssentialCvrOffrCd END), 
		JanMinEssentialCvrOffrCd = i.MinEssentialCvrOffrCd,
		JanALEMemberFTECnt = i.ALEMemberFTECnt, 
		JanTotalEmployeeCnt = i.TotalEmployeeCnt,
		--JanAggregatedGroupInd = (CASE WHEN ale.annual_aag_indicator = 1 THEN NULL ELSE i.AggregatedGroupInd END),
		JanAggregatedGroupInd = i.AggregatedGroupInd,
		JanALESect4980HTrnstReliefCd = i.ALESect4980HTrnstReliefCd
	FROM @ALEMemberInformationGrp ale INNER JOIN(
	SELECT
		a.employer_id,
		CAST(a.mec_offered_without_equivalents AS varchar(1)) AS MinEssentialCvrOffrCd,
		CONVERT(varchar(max),a.full_time_employee_count) AS ALEMemberFTECnt,
		CONVERT(varchar(max),a.total_employee_count) AS TotalEmployeeCnt,
		CAST(a.aag_indicator AS varchar(1)) AS AggregatedGroupInd,
		CONVERT(varchar(max),a._4980H_transition_relief_code) AS ALESect4980HTrnstReliefCd
	FROM air.ale.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE b.year_id = @taxYearId AND m.month_id = 1
	) i ON i.employer_id = ale.employer_id

	--Feb values--
	UPDATE @ALEMemberInformationGrp
	SET --FebMinEssentialCvrOffrCd = (CASE WHEN ale.annual_mec_offer_indicator = 1 THEN NULL ELSE i.MinEssentialCvrOffrCd END), 
		FebMinEssentialCvrOffrCd = i.MinEssentialCvrOffrCd,
		FebALEMemberFTECnt = i.ALEMemberFTECnt, 
		FebTotalEmployeeCnt = i.TotalEmployeeCnt,
		--FebAggregatedGroupInd = (CASE WHEN ale.annual_aag_indicator = 1 THEN NULL ELSE i.AggregatedGroupInd END),
		FebAggregatedGroupInd = i.AggregatedGroupInd,
		FebALESect4980HTrnstReliefCd = i.ALESect4980HTrnstReliefCd
	FROM @ALEMemberInformationGrp ale INNER JOIN(
	SELECT
		a.employer_id,
		CAST(a.mec_offered_without_equivalents AS varchar(1)) AS MinEssentialCvrOffrCd,
		CONVERT(varchar(max),a.full_time_employee_count) AS ALEMemberFTECnt,
		CONVERT(varchar(max),a.total_employee_count) AS TotalEmployeeCnt,
		CAST(a.aag_indicator AS varchar(1)) AS AggregatedGroupInd,
		CONVERT(varchar(max),a._4980H_transition_relief_code) AS ALESect4980HTrnstReliefCd
	FROM air.ale.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE b.year_id = @taxYearId AND m.month_id = 2
	) i ON i.employer_id = ale.employer_id

	--Mar values--
	UPDATE @ALEMemberInformationGrp
	SET --MarMinEssentialCvrOffrCd = (CASE WHEN ale.annual_mec_offer_indicator = 1 THEN NULL ELSE i.MinEssentialCvrOffrCd END), 
		MarMinEssentialCvrOffrCd = i.MinEssentialCvrOffrCd,
		MarALEMemberFTECnt = i.ALEMemberFTECnt, 
		MarTotalEmployeeCnt = i.TotalEmployeeCnt,
		--MarAggregatedGroupInd = (CASE WHEN ale.annual_aag_indicator = 1 THEN NULL ELSE i.AggregatedGroupInd END),
		MarAggregatedGroupInd = i.AggregatedGroupInd,
		MarALESect4980HTrnstReliefCd = i.ALESect4980HTrnstReliefCd
	FROM @ALEMemberInformationGrp ale INNER JOIN(
	SELECT
		a.employer_id,
		CAST(a.mec_offered_without_equivalents AS varchar(1)) AS MinEssentialCvrOffrCd,
		CONVERT(varchar(max),a.full_time_employee_count) AS ALEMemberFTECnt,
		CONVERT(varchar(max),a.total_employee_count) AS TotalEmployeeCnt,
		CAST(a.aag_indicator AS varchar(1)) AS AggregatedGroupInd,
		CONVERT(varchar(max),a._4980H_transition_relief_code) AS ALESect4980HTrnstReliefCd
	FROM air.ale.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
								  
	WHERE b.year_id = @taxYearId AND m.month_id = 3
	) i ON i.employer_id = ale.employer_id

	--Apr values--
	UPDATE @ALEMemberInformationGrp
	SET --AprMinEssentialCvrOffrCd = (CASE WHEN ale.annual_mec_offer_indicator = 1 THEN NULL ELSE i.MinEssentialCvrOffrCd END), 
		AprMinEssentialCvrOffrCd = i.MinEssentialCvrOffrCd,
		AprALEMemberFTECnt = i.ALEMemberFTECnt, 
		AprTotalEmployeeCnt = i.TotalEmployeeCnt,
		--AprAggregatedGroupInd = (CASE WHEN ale.annual_aag_indicator = 1 THEN NULL ELSE i.AggregatedGroupInd END),
		AprAggregatedGroupInd = i.AggregatedGroupInd,
		AprALESect4980HTrnstReliefCd = i.ALESect4980HTrnstReliefCd
	FROM @ALEMemberInformationGrp ale INNER JOIN(
	SELECT
		a.employer_id,
		CAST(a.mec_offered_without_equivalents AS varchar(1)) AS MinEssentialCvrOffrCd,
		CONVERT(varchar(max),a.full_time_employee_count) AS ALEMemberFTECnt,
		CONVERT(varchar(max),a.total_employee_count) AS TotalEmployeeCnt,
		CAST(a.aag_indicator AS varchar(1)) AS AggregatedGroupInd,
		CONVERT(varchar(max),a._4980H_transition_relief_code) AS ALESect4980HTrnstReliefCd
	FROM air.ale.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE b.year_id = @taxYearId AND m.month_id = 4
	) i ON i.employer_id = ale.employer_id

	--May values--
	UPDATE @ALEMemberInformationGrp
	SET --MayMinEssentialCvrOffrCd = (CASE WHEN ale.annual_mec_offer_indicator = 1 THEN NULL ELSE i.MinEssentialCvrOffrCd END), 
		MayMinEssentialCvrOffrCd = i.MinEssentialCvrOffrCd,
		MayALEMemberFTECnt = i.ALEMemberFTECnt, 
		MayTotalEmployeeCnt = i.TotalEmployeeCnt,
		--MayAggregatedGroupInd = (CASE WHEN ale.annual_aag_indicator = 1 THEN NULL ELSE i.AggregatedGroupInd END),
		MayAggregatedGroupInd = i.AggregatedGroupInd,
		MayALESect4980HTrnstReliefCd = i.ALESect4980HTrnstReliefCd
	FROM @ALEMemberInformationGrp ale INNER JOIN(
	SELECT
		a.employer_id,
		CAST(a.mec_offered_without_equivalents AS varchar(1)) AS MinEssentialCvrOffrCd,
		CONVERT(varchar(max),a.full_time_employee_count) AS ALEMemberFTECnt,
		CONVERT(varchar(max),a.total_employee_count) AS TotalEmployeeCnt,
		CAST(a.aag_indicator AS varchar(1)) AS AggregatedGroupInd,
		CONVERT(varchar(max),a._4980H_transition_relief_code) AS ALESect4980HTrnstReliefCd
	FROM air.ale.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
								  
	WHERE b.year_id = @taxYearId AND m.month_id = 5
	) i ON i.employer_id = ale.employer_id

	--Jun values--
	UPDATE @ALEMemberInformationGrp
	SET --JunMinEssentialCvrOffrCd = (CASE WHEN ale.annual_mec_offer_indicator = 1 THEN NULL ELSE i.MinEssentialCvrOffrCd END), 
		JunMinEssentialCvrOffrCd = i.MinEssentialCvrOffrCd,
		JunALEMemberFTECnt = i.ALEMemberFTECnt, 
		JunTotalEmployeeCnt = i.TotalEmployeeCnt,
		--JunAggregatedGroupInd = (CASE WHEN ale.annual_aag_indicator = 1 THEN NULL ELSE i.AggregatedGroupInd END),
		JunAggregatedGroupInd = i.AggregatedGroupInd,
		JunALESect4980HTrnstReliefCd = i.ALESect4980HTrnstReliefCd
	FROM @ALEMemberInformationGrp ale INNER JOIN(
	SELECT
		a.employer_id,
		CAST(a.mec_offered_without_equivalents AS varchar(1)) AS MinEssentialCvrOffrCd,
		CONVERT(varchar(max),a.full_time_employee_count) AS ALEMemberFTECnt,
		CONVERT(varchar(max),a.total_employee_count) AS TotalEmployeeCnt,
		CAST(a.aag_indicator AS varchar(1)) AS AggregatedGroupInd,
		CONVERT(varchar(max),a._4980H_transition_relief_code) AS ALESect4980HTrnstReliefCd
	FROM air.ale.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
								  
	WHERE b.year_id = @taxYearId AND m.month_id = 6
	) i ON i.employer_id = ale.employer_id

	--Jul values--
	UPDATE @ALEMemberInformationGrp
	SET --JulMinEssentialCvrOffrCd = (CASE WHEN ale.annual_mec_offer_indicator = 1 THEN NULL ELSE i.MinEssentialCvrOffrCd END), 
		JulMinEssentialCvrOffrCd = i.MinEssentialCvrOffrCd,
		JulALEMemberFTECnt = i.ALEMemberFTECnt, 
		JulTotalEmployeeCnt = i.TotalEmployeeCnt,
		--JulAggregatedGroupInd = (CASE WHEN ale.annual_aag_indicator = 1 THEN NULL ELSE i.AggregatedGroupInd END),
		JulAggregatedGroupInd = i.AggregatedGroupInd,
		JulALESect4980HTrnstReliefCd = i.ALESect4980HTrnstReliefCd
	FROM @ALEMemberInformationGrp ale INNER JOIN(
	SELECT
		a.employer_id,
		CAST(a.mec_offered_without_equivalents AS varchar(1)) AS MinEssentialCvrOffrCd,
		CONVERT(varchar(max),a.full_time_employee_count) AS ALEMemberFTECnt,
		CONVERT(varchar(max),a.total_employee_count) AS TotalEmployeeCnt,
		CAST(a.aag_indicator AS varchar(1)) AS AggregatedGroupInd,
		CONVERT(varchar(max),a._4980H_transition_relief_code) AS ALESect4980HTrnstReliefCd
	FROM air.ale.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
								  
	WHERE b.year_id = @taxYearId AND m.month_id = 7
	) i ON i.employer_id = ale.employer_id

	--Aug values--
	UPDATE @ALEMemberInformationGrp
	SET --AugMinEssentialCvrOffrCd = (CASE WHEN ale.annual_mec_offer_indicator = 1 THEN NULL ELSE i.MinEssentialCvrOffrCd END), 
		AugMinEssentialCvrOffrCd = i.MinEssentialCvrOffrCd,
		AugALEMemberFTECnt = i.ALEMemberFTECnt, 
		AugTotalEmployeeCnt = i.TotalEmployeeCnt,
		--AugAggregatedGroupInd = (CASE WHEN ale.annual_aag_indicator = 1 THEN NULL ELSE i.AggregatedGroupInd END),
		AugAggregatedGroupInd = i.AggregatedGroupInd,
		AugALESect4980HTrnstReliefCd = i.ALESect4980HTrnstReliefCd
	FROM @ALEMemberInformationGrp ale INNER JOIN(
	SELECT
		a.employer_id,
		CAST(a.mec_offered_without_equivalents AS varchar(1)) AS MinEssentialCvrOffrCd,
		CONVERT(varchar(max),a.full_time_employee_count) AS ALEMemberFTECnt,
		CONVERT(varchar(max),a.total_employee_count) AS TotalEmployeeCnt,
		CAST(a.aag_indicator AS varchar(1)) AS AggregatedGroupInd,
		CONVERT(varchar(max),a._4980H_transition_relief_code) AS ALESect4980HTrnstReliefCd
	FROM air.ale.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
								  
	WHERE b.year_id = @taxYearId AND m.month_id = 8
	) i ON i.employer_id = ale.employer_id

	--Sept values--
	UPDATE @ALEMemberInformationGrp
	SET --SeptMinEssentialCvrOffrCd = (CASE WHEN ale.annual_mec_offer_indicator = 1 THEN NULL ELSE i.MinEssentialCvrOffrCd END), 
		SeptMinEssentialCvrOffrCd = i.MinEssentialCvrOffrCd,
		SeptALEMemberFTECnt = i.ALEMemberFTECnt, 
		SeptTotalEmployeeCnt = i.TotalEmployeeCnt,
		--SeptAggregatedGroupInd = (CASE WHEN ale.annual_aag_indicator = 1 THEN NULL ELSE i.AggregatedGroupInd END),
		SeptAggregatedGroupInd = i.AggregatedGroupInd,
		SeptALESect4980HTrnstReliefCd = i.ALESect4980HTrnstReliefCd
	FROM @ALEMemberInformationGrp ale INNER JOIN(
	SELECT
		a.employer_id,
		CAST(a.mec_offered_without_equivalents AS varchar(1)) AS MinEssentialCvrOffrCd,
		CONVERT(varchar(max),a.full_time_employee_count) AS ALEMemberFTECnt,
		CONVERT(varchar(max),a.total_employee_count) AS TotalEmployeeCnt,
		CAST(a.aag_indicator AS varchar(1)) AS AggregatedGroupInd,
		CONVERT(varchar(max),a._4980H_transition_relief_code) AS ALESect4980HTrnstReliefCd
	FROM air.ale.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
								  
	WHERE b.year_id = @taxYearId AND m.month_id = 9
	) i ON i.employer_id = ale.employer_id

	--Oct values--
	UPDATE @ALEMemberInformationGrp
	SET --OctMinEssentialCvrOffrCd = (CASE WHEN ale.annual_mec_offer_indicator = 1 THEN NULL ELSE i.MinEssentialCvrOffrCd END), 
		OctMinEssentialCvrOffrCd = i.MinEssentialCvrOffrCd,
		OctALEMemberFTECnt = i.ALEMemberFTECnt, 
		OctTotalEmployeeCnt = i.TotalEmployeeCnt,
		--OctAggregatedGroupInd = (CASE WHEN ale.annual_aag_indicator = 1 THEN NULL ELSE i.AggregatedGroupInd END),
		OctAggregatedGroupInd = i.AggregatedGroupInd,
		OctALESect4980HTrnstReliefCd = i.ALESect4980HTrnstReliefCd
	FROM @ALEMemberInformationGrp ale INNER JOIN(
	SELECT
		a.employer_id,
		CAST(a.mec_offered_without_equivalents AS varchar(1)) AS MinEssentialCvrOffrCd,
		CONVERT(varchar(max),a.full_time_employee_count) AS ALEMemberFTECnt,
		CONVERT(varchar(max),a.total_employee_count) AS TotalEmployeeCnt,
		CAST(a.aag_indicator AS varchar(1)) AS AggregatedGroupInd,
		CONVERT(varchar(max),a._4980H_transition_relief_code) AS ALESect4980HTrnstReliefCd
	FROM air.ale.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
								  
	WHERE b.year_id = @taxYearId AND m.month_id = 10
	) i ON i.employer_id = ale.employer_id

	--Nov values--
	UPDATE @ALEMemberInformationGrp
	SET --NovMinEssentialCvrOffrCd = (CASE WHEN ale.annual_mec_offer_indicator = 1 THEN NULL ELSE i.MinEssentialCvrOffrCd END), 
		NovMinEssentialCvrOffrCd = i.MinEssentialCvrOffrCd,
		NovALEMemberFTECnt = i.ALEMemberFTECnt, 
		NovTotalEmployeeCnt = i.TotalEmployeeCnt,
		--NovAggregatedGroupInd = (CASE WHEN ale.annual_aag_indicator = 1 THEN NULL ELSE i.AggregatedGroupInd END),
		NovAggregatedGroupInd = i.AggregatedGroupInd,
		NovALESect4980HTrnstReliefCd = i.ALESect4980HTrnstReliefCd
	FROM @ALEMemberInformationGrp ale INNER JOIN(
	SELECT
		a.employer_id,
		CAST(a.mec_offered_without_equivalents AS varchar(1)) AS MinEssentialCvrOffrCd,
		CONVERT(varchar(max),a.full_time_employee_count) AS ALEMemberFTECnt,
		CONVERT(varchar(max),a.total_employee_count) AS TotalEmployeeCnt,
		CAST(a.aag_indicator AS varchar(1)) AS AggregatedGroupInd,
		CONVERT(varchar(max),a._4980H_transition_relief_code) AS ALESect4980HTrnstReliefCd
	FROM air.ale.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
								  
	WHERE b.year_id = @taxYearId AND m.month_id = 11
	) i ON i.employer_id = ale.employer_id

	--Dec values--
	UPDATE @ALEMemberInformationGrp
	SET --DecMinEssentialCvrOffrCd = (CASE WHEN ale.annual_mec_offer_indicator = 1 THEN NULL ELSE i.MinEssentialCvrOffrCd END), 
		DecMinEssentialCvrOffrCd = i.MinEssentialCvrOffrCd,
		DecALEMemberFTECnt = i.ALEMemberFTECnt, 
		DecTotalEmployeeCnt = i.TotalEmployeeCnt,
		--DecAggregatedGroupInd = (CASE WHEN ale.annual_aag_indicator = 1 THEN NULL ELSE i.AggregatedGroupInd END),
		DecAggregatedGroupInd = i.AggregatedGroupInd,
		DecALESect4980HTrnstReliefCd = i.ALESect4980HTrnstReliefCd
	FROM @ALEMemberInformationGrp ale INNER JOIN(
	SELECT
		a.employer_id,
		CAST(a.mec_offered_without_equivalents AS varchar(1)) AS MinEssentialCvrOffrCd,
		CONVERT(varchar(max),a.full_time_employee_count) AS ALEMemberFTECnt,
		CONVERT(varchar(max),a.total_employee_count) AS TotalEmployeeCnt,
		CAST(a.aag_indicator AS varchar(1)) AS AggregatedGroupInd,
		CONVERT(varchar(max),a._4980H_transition_relief_code) AS ALESect4980HTrnstReliefCd
	FROM air.ale.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
								  
	WHERE b.year_id = @taxYearId AND m.month_id = 12
	) i ON i.employer_id = ale.employer_id


	SELECT * FROM @ALEMemberInformationGrp


END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
