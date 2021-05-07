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
CREATE PROCEDURE SELECT_ale_member_information
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
		YearlyMinEssentialCvrOffrCd,
		YearlyALEMemberFTECnt,
		YearlyTotalEmployeeCnt,
		YearlyAggregatedGroupInd,
		YearlyALESect4980HTrnstReliefCd)
	SELECT
		a.employer_id,
		ISNULL(CAST(a.annual_mec_offer_indicator AS varchar(1)),'0') AS MinEssentialCvrOffrCd,
		ISNULL(a.annual_fulltime_employee_count,'0') AS ALEMemberFTECnt,
		ISNULL(a.total_employee_count_through_year,'0') AS TotalEmployeeCnt,
		ISNULL(CAST(a.annual_aag_indicator AS varchar(1)),'0') AS AggregatedGroupInd,
		ISNULL(a._4980H_transition_relief_code,'B') AS ALESect4980HTrnstReliefCd
	FROM air.ale.yearly_detail a 
	WHERE (a.employer_id = @employerId OR @employerId IS NULL)
	AND a.year_id = @taxYearId

	--Jan values--
	UPDATE @ALEMemberInformationGrp
	SET JanMinEssentialCvrOffrCd = ISNULL(i.MinEssentialCvrOffrCd,'0'), 
		JanALEMemberFTECnt = ISNULL(i.ALEMemberFTECnt,'0'), 
		JanTotalEmployeeCnt = ISNULL(i.TotalEmployeeCnt,'0'),
		JanAggregatedGroupInd = ISNULL(i.AggregatedGroupInd,'0'),
		JanALESect4980HTrnstReliefCd = ISNULL(i.ALESect4980HTrnstReliefCd,'B')
	FROM @ALEMemberInformationGrp ale INNER JOIN(
	SELECT
		a.employer_id,
		CAST(a.mec_offered_without_equivalents AS varchar(1)) AS MinEssentialCvrOffrCd,
		CONVERT(varchar(max),a.full_time_employee_count) AS ALEMemberFTECnt,
		CONVERT(varchar(max),a.total_employee_count) AS TotalEmployeeCnt,
		CAST(a.aag_indicator AS varchar(1)) AS AggregatedGroupInd,
		CONVERT(varchar(max),y._4980H_transition_relief_code) AS ALESect4980HTrnstReliefCd
	FROM air.ale.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
								  INNER JOIN air.ale.yearly_detail y ON y.employer_id = a.employer_id AND y.year_id = b.year_id
	WHERE b.year_id = @taxYearId AND m.month_id = 1
	) i ON i.employer_id = ale.employer_id

	--Feb values--
	UPDATE @ALEMemberInformationGrp
	SET FebMinEssentialCvrOffrCd = ISNULL(i.MinEssentialCvrOffrCd,'0'), 
		FebALEMemberFTECnt = ISNULL(i.ALEMemberFTECnt,'0'), 
		FebTotalEmployeeCnt = ISNULL(i.TotalEmployeeCnt,'0'),
		FebAggregatedGroupInd = ISNULL(i.AggregatedGroupInd,'0'),
		FebALESect4980HTrnstReliefCd = ISNULL(i.ALESect4980HTrnstReliefCd,'B')
	FROM @ALEMemberInformationGrp ale INNER JOIN(
	SELECT
		a.employer_id,
		CAST(a.mec_offered_without_equivalents AS varchar(1)) AS MinEssentialCvrOffrCd,
		CONVERT(varchar(max),a.full_time_employee_count) AS ALEMemberFTECnt,
		CONVERT(varchar(max),a.total_employee_count) AS TotalEmployeeCnt,
		CAST(a.aag_indicator AS varchar(1)) AS AggregatedGroupInd,
		CONVERT(varchar(max),y._4980H_transition_relief_code) AS ALESect4980HTrnstReliefCd
	FROM air.ale.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
								  INNER JOIN air.ale.yearly_detail y ON y.employer_id = a.employer_id AND y.year_id = b.year_id
	WHERE b.year_id = @taxYearId AND m.month_id = 2
	) i ON i.employer_id = ale.employer_id

	--Mar values--
	UPDATE @ALEMemberInformationGrp
	SET MarMinEssentialCvrOffrCd = ISNULL(i.MinEssentialCvrOffrCd,'0'), 
		MarALEMemberFTECnt = ISNULL(i.ALEMemberFTECnt,'0'), 
		MarTotalEmployeeCnt = ISNULL(i.TotalEmployeeCnt,'0'),
		MarAggregatedGroupInd = ISNULL(i.AggregatedGroupInd,'0'),
		MarALESect4980HTrnstReliefCd = ISNULL(i.ALESect4980HTrnstReliefCd,'B')
	FROM @ALEMemberInformationGrp ale INNER JOIN(
	SELECT
		a.employer_id,
		CAST(a.mec_offered_without_equivalents AS varchar(1)) AS MinEssentialCvrOffrCd,
		CONVERT(varchar(max),a.full_time_employee_count) AS ALEMemberFTECnt,
		CONVERT(varchar(max),a.total_employee_count) AS TotalEmployeeCnt,
		CAST(a.aag_indicator AS varchar(1)) AS AggregatedGroupInd,
		CONVERT(varchar(max),y._4980H_transition_relief_code) AS ALESect4980HTrnstReliefCd
	FROM air.ale.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
								  INNER JOIN air.ale.yearly_detail y ON y.employer_id = a.employer_id AND y.year_id = b.year_id
	WHERE b.year_id = @taxYearId AND m.month_id = 3
	) i ON i.employer_id = ale.employer_id

	--Apr values--
	UPDATE @ALEMemberInformationGrp
	SET AprMinEssentialCvrOffrCd = ISNULL(i.MinEssentialCvrOffrCd,'0'), 
		AprALEMemberFTECnt = ISNULL(i.ALEMemberFTECnt,'0'), 
		AprTotalEmployeeCnt = ISNULL(i.TotalEmployeeCnt,'0'),
		AprAggregatedGroupInd = ISNULL(i.AggregatedGroupInd,'0'),
		AprALESect4980HTrnstReliefCd = ISNULL(i.ALESect4980HTrnstReliefCd,'B')
	FROM @ALEMemberInformationGrp ale INNER JOIN(
	SELECT
		a.employer_id,
		CAST(a.mec_offered_without_equivalents AS varchar(1)) AS MinEssentialCvrOffrCd,
		CONVERT(varchar(max),a.full_time_employee_count) AS ALEMemberFTECnt,
		CONVERT(varchar(max),a.total_employee_count) AS TotalEmployeeCnt,
		CAST(a.aag_indicator AS varchar(1)) AS AggregatedGroupInd,
		CONVERT(varchar(max),y._4980H_transition_relief_code) AS ALESect4980HTrnstReliefCd
	FROM air.ale.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
								  INNER JOIN air.ale.yearly_detail y ON y.employer_id = a.employer_id AND y.year_id = b.year_id
	WHERE b.year_id = @taxYearId AND m.month_id = 4
	) i ON i.employer_id = ale.employer_id

	--May values--
	UPDATE @ALEMemberInformationGrp
	SET MayMinEssentialCvrOffrCd = ISNULL(i.MinEssentialCvrOffrCd,'0'), 
		MayALEMemberFTECnt = ISNULL(i.ALEMemberFTECnt,'0'), 
		MayTotalEmployeeCnt = ISNULL(i.TotalEmployeeCnt,'0'),
		MayAggregatedGroupInd = ISNULL(i.AggregatedGroupInd,'0'),
		MayALESect4980HTrnstReliefCd = ISNULL(i.ALESect4980HTrnstReliefCd,'B')
	FROM @ALEMemberInformationGrp ale INNER JOIN(
	SELECT
		a.employer_id,
		CAST(a.mec_offered_without_equivalents AS varchar(1)) AS MinEssentialCvrOffrCd,
		CONVERT(varchar(max),a.full_time_employee_count) AS ALEMemberFTECnt,
		CONVERT(varchar(max),a.total_employee_count) AS TotalEmployeeCnt,
		CAST(a.aag_indicator AS varchar(1)) AS AggregatedGroupInd,
		CONVERT(varchar(max),y._4980H_transition_relief_code) AS ALESect4980HTrnstReliefCd
	FROM air.ale.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
								  INNER JOIN air.ale.yearly_detail y ON y.employer_id = a.employer_id AND y.year_id = b.year_id
	WHERE b.year_id = @taxYearId AND m.month_id = 5
	) i ON i.employer_id = ale.employer_id

	--Jun values--
	UPDATE @ALEMemberInformationGrp
	SET JunMinEssentialCvrOffrCd = ISNULL(i.MinEssentialCvrOffrCd,'0'), 
		JunALEMemberFTECnt = ISNULL(i.ALEMemberFTECnt,'0'), 
		JunTotalEmployeeCnt = ISNULL(i.TotalEmployeeCnt,'0'),
		JunAggregatedGroupInd = ISNULL(i.AggregatedGroupInd,'0'),
		JunALESect4980HTrnstReliefCd = ISNULL(i.ALESect4980HTrnstReliefCd,'B')
	FROM @ALEMemberInformationGrp ale INNER JOIN(
	SELECT
		a.employer_id,
		CAST(a.mec_offered_without_equivalents AS varchar(1)) AS MinEssentialCvrOffrCd,
		CONVERT(varchar(max),a.full_time_employee_count) AS ALEMemberFTECnt,
		CONVERT(varchar(max),a.total_employee_count) AS TotalEmployeeCnt,
		CAST(a.aag_indicator AS varchar(1)) AS AggregatedGroupInd,
		CONVERT(varchar(max),y._4980H_transition_relief_code) AS ALESect4980HTrnstReliefCd
	FROM air.ale.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
								  INNER JOIN air.ale.yearly_detail y ON y.employer_id = a.employer_id AND y.year_id = b.year_id
	WHERE b.year_id = @taxYearId AND m.month_id = 6
	) i ON i.employer_id = ale.employer_id

	--Jul values--
	UPDATE @ALEMemberInformationGrp
	SET JulMinEssentialCvrOffrCd = ISNULL(i.MinEssentialCvrOffrCd,'0'), 
		JulALEMemberFTECnt = ISNULL(i.ALEMemberFTECnt,'0'), 
		JulTotalEmployeeCnt = ISNULL(i.TotalEmployeeCnt,'0'),
		JulAggregatedGroupInd = ISNULL(i.AggregatedGroupInd,'0'),
		JulALESect4980HTrnstReliefCd = ISNULL(i.ALESect4980HTrnstReliefCd,'B')
	FROM @ALEMemberInformationGrp ale INNER JOIN(
	SELECT
		a.employer_id,
		CAST(a.mec_offered_without_equivalents AS varchar(1)) AS MinEssentialCvrOffrCd,
		CONVERT(varchar(max),a.full_time_employee_count) AS ALEMemberFTECnt,
		CONVERT(varchar(max),a.total_employee_count) AS TotalEmployeeCnt,
		CAST(a.aag_indicator AS varchar(1)) AS AggregatedGroupInd,
		CONVERT(varchar(max),y._4980H_transition_relief_code) AS ALESect4980HTrnstReliefCd
	FROM air.ale.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
								  INNER JOIN air.ale.yearly_detail y ON y.employer_id = a.employer_id AND y.year_id = b.year_id
	WHERE b.year_id = @taxYearId AND m.month_id = 7
	) i ON i.employer_id = ale.employer_id

	--Aug values--
	UPDATE @ALEMemberInformationGrp
	SET AugMinEssentialCvrOffrCd = ISNULL(i.MinEssentialCvrOffrCd,'0'), 
		AugALEMemberFTECnt = ISNULL(i.ALEMemberFTECnt,'0'), 
		AugTotalEmployeeCnt = ISNULL(i.TotalEmployeeCnt,'0'),
		AugAggregatedGroupInd = ISNULL(i.AggregatedGroupInd,'0'),
		AugALESect4980HTrnstReliefCd = ISNULL(i.ALESect4980HTrnstReliefCd,'B')
	FROM @ALEMemberInformationGrp ale INNER JOIN(
	SELECT
		a.employer_id,
		CAST(a.mec_offered_without_equivalents AS varchar(1)) AS MinEssentialCvrOffrCd,
		CONVERT(varchar(max),a.full_time_employee_count) AS ALEMemberFTECnt,
		CONVERT(varchar(max),a.total_employee_count) AS TotalEmployeeCnt,
		CAST(a.aag_indicator AS varchar(1)) AS AggregatedGroupInd,
		CONVERT(varchar(max),y._4980H_transition_relief_code) AS ALESect4980HTrnstReliefCd
	FROM air.ale.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
								  INNER JOIN air.ale.yearly_detail y ON y.employer_id = a.employer_id AND y.year_id = b.year_id
	WHERE b.year_id = @taxYearId AND m.month_id = 8
	) i ON i.employer_id = ale.employer_id

	--Sept values--
	UPDATE @ALEMemberInformationGrp
	SET SeptMinEssentialCvrOffrCd = ISNULL(i.MinEssentialCvrOffrCd,'0'), 
		SeptALEMemberFTECnt = ISNULL(i.ALEMemberFTECnt,'0'), 
		SeptTotalEmployeeCnt = ISNULL(i.TotalEmployeeCnt,'0'),
		SeptAggregatedGroupInd = ISNULL(i.AggregatedGroupInd,'0'),
		SeptALESect4980HTrnstReliefCd = ISNULL(i.ALESect4980HTrnstReliefCd,'B')
	FROM @ALEMemberInformationGrp ale INNER JOIN(
	SELECT
		a.employer_id,
		CAST(a.mec_offered_without_equivalents AS varchar(1)) AS MinEssentialCvrOffrCd,
		CONVERT(varchar(max),a.full_time_employee_count) AS ALEMemberFTECnt,
		CONVERT(varchar(max),a.total_employee_count) AS TotalEmployeeCnt,
		CAST(a.aag_indicator AS varchar(1)) AS AggregatedGroupInd,
		CONVERT(varchar(max),y._4980H_transition_relief_code) AS ALESect4980HTrnstReliefCd
	FROM air.ale.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
								  INNER JOIN air.ale.yearly_detail y ON y.employer_id = a.employer_id AND y.year_id = b.year_id
	WHERE b.year_id = @taxYearId AND m.month_id = 9
	) i ON i.employer_id = ale.employer_id

	--Oct values--
	UPDATE @ALEMemberInformationGrp
	SET OctMinEssentialCvrOffrCd = ISNULL(i.MinEssentialCvrOffrCd,'0'), 
		OctALEMemberFTECnt = ISNULL(i.ALEMemberFTECnt,'0'), 
		OctTotalEmployeeCnt = ISNULL(i.TotalEmployeeCnt,'0'),
		OctAggregatedGroupInd = ISNULL(i.AggregatedGroupInd,'0'),
		OctALESect4980HTrnstReliefCd = ISNULL(i.ALESect4980HTrnstReliefCd,'B')
	FROM @ALEMemberInformationGrp ale INNER JOIN(
	SELECT
		a.employer_id,
		CAST(a.mec_offered_without_equivalents AS varchar(1)) AS MinEssentialCvrOffrCd,
		CONVERT(varchar(max),a.full_time_employee_count) AS ALEMemberFTECnt,
		CONVERT(varchar(max),a.total_employee_count) AS TotalEmployeeCnt,
		CAST(a.aag_indicator AS varchar(1)) AS AggregatedGroupInd,
		CONVERT(varchar(max),y._4980H_transition_relief_code) AS ALESect4980HTrnstReliefCd
	FROM air.ale.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
								  INNER JOIN air.ale.yearly_detail y ON y.employer_id = a.employer_id AND y.year_id = b.year_id
	WHERE b.year_id = @taxYearId AND m.month_id = 10
	) i ON i.employer_id = ale.employer_id

	--Nov values--
	UPDATE @ALEMemberInformationGrp
	SET NovMinEssentialCvrOffrCd = ISNULL(i.MinEssentialCvrOffrCd,'0'), 
		NovALEMemberFTECnt = ISNULL(i.ALEMemberFTECnt,'0'), 
		NovTotalEmployeeCnt = ISNULL(i.TotalEmployeeCnt,'0'),
		NovAggregatedGroupInd = ISNULL(i.AggregatedGroupInd,'0'),
		NovALESect4980HTrnstReliefCd = ISNULL(i.ALESect4980HTrnstReliefCd,'B')
	FROM @ALEMemberInformationGrp ale INNER JOIN(
	SELECT
		a.employer_id,
		CAST(a.mec_offered_without_equivalents AS varchar(1)) AS MinEssentialCvrOffrCd,
		CONVERT(varchar(max),a.full_time_employee_count) AS ALEMemberFTECnt,
		CONVERT(varchar(max),a.total_employee_count) AS TotalEmployeeCnt,
		CAST(a.aag_indicator AS varchar(1)) AS AggregatedGroupInd,
		CONVERT(varchar(max),y._4980H_transition_relief_code) AS ALESect4980HTrnstReliefCd
	FROM air.ale.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
								  INNER JOIN air.ale.yearly_detail y ON y.employer_id = a.employer_id AND y.year_id = b.year_id
	WHERE b.year_id = @taxYearId AND m.month_id = 11
	) i ON i.employer_id = ale.employer_id

	--Dec values--
	UPDATE @ALEMemberInformationGrp
	SET DecMinEssentialCvrOffrCd = ISNULL(i.MinEssentialCvrOffrCd,'0'), 
		DecALEMemberFTECnt = ISNULL(i.ALEMemberFTECnt,'0'), 
		DecTotalEmployeeCnt = ISNULL(i.TotalEmployeeCnt,'0'),
		DecAggregatedGroupInd = ISNULL(i.AggregatedGroupInd,'0'),
		DecALESect4980HTrnstReliefCd = ISNULL(i.ALESect4980HTrnstReliefCd,'B')
	FROM @ALEMemberInformationGrp ale INNER JOIN(
	SELECT
		a.employer_id,
		CAST(a.mec_offered_without_equivalents AS varchar(1)) AS MinEssentialCvrOffrCd,
		CONVERT(varchar(max),a.full_time_employee_count) AS ALEMemberFTECnt,
		CONVERT(varchar(max),a.total_employee_count) AS TotalEmployeeCnt,
		CAST(a.aag_indicator AS varchar(1)) AS AggregatedGroupInd,
		CONVERT(varchar(max),y._4980H_transition_relief_code) AS ALESect4980HTrnstReliefCd
	FROM air.ale.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								  INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
								  INNER JOIN air.ale.yearly_detail y ON y.employer_id = a.employer_id AND y.year_id = b.year_id
	WHERE b.year_id = @taxYearId AND m.month_id = 12
	) i ON i.employer_id = ale.employer_id


	SELECT * FROM @ALEMemberInformationGrp


END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
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
ALTER PROCEDURE SELECT_form_1094C_upstream_detail
	-- Add the parameters for the stored procedure here
	@taxYearId int,
	@employerId int = null
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	WITH employers AS (
	SELECT DISTINCT
		a.employer_id,
		a.dge_ein,
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
		ROW_NUMBER() OVER (ORDER BY e.employer_id) AS SubmissionId,
		'0' AS CorrectedInd,
		e.*,
		d.name AS GovtBusinessNameLine1Txt,
		d.ein AS GovtEmployerEIN,
		d.[address] AS GovtAddressLine1Txt,
		d.city AS GovtCityNm,
		d.state_code AS GovtUSStateCd,
		d.zipcode AS GovtUSZIPCd,
		d.zipcode_ext AS GovtUSZIPExtensionCd,
		d.contact_first_name AS GovtPersonFirstNm,
		d.contact_middle_name AS GovtPersonMiddleNm,
		d.contact_last_name AS GovtPersonLastNm,
		d.contact_name_suffix AS GovtSuffixNm,
		d.contact_telephone AS GovtContactPhoneNum,
		CASE WHEN e.dge_ein IS NULL THEN '1' ELSE '0' END AS AuthoritativeTransmittalInd,
		CAST(yd._1095C_count AS varchar(max)) AS TotalForm1095CALEMemberCnt,
		yd.aag_code AS AggregatedGroupMemberCd,
		CAST(yd.qom_indicator AS varchar(1)) AS QualifyingOfferMethodInd,
		CAST(yd.qom_transition_relief_indicator AS varchar(1)) AS QlfyOfferMethodTrnstReliefInd,
		CAST(yd._4980H_transition_relief_indicator AS varchar(1)) AS Section4980HReliefInd,
		CAST(yd._98_percent_offer_method_indicator AS varchar(1)) AS NinetyEightPctOfferMethodInd,
		yd.jurat_signature_pin AS JuratSignaturePIN,
		yd.person_title_text AS PersonTitleTxt,
		yd.signature_date AS dtSignature
	FROM employers e LEFT JOIN air.ale.dge d ON e.dge_ein = d.ein
					 LEFT JOIN air.ale.yearly_detail yd ON e.employer_id = yd.employer_id
	WHERE yd.year_id = @taxYearId

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
GRANT EXECUTE ON SELECT_ale_member_information TO [aca-user] AS [dbo] 
GO
