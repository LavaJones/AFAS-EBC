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
								COUNT(employee_id) AS _1095C_count
							FROM aca.dbo.tax_year_1095c_approval
							WHERE get1095C = 1
							GROUP BY employer_id
						) AS tya ON tya.employer_id = e.employer_id
					  --LEFT JOIN air.ale.dge d ON e.dge_ein = d.ein --might look at aca.dbo.tax_year_approval	
					  LEFT JOIN aca.dbo.[state] s ON s.state_id = yd.state_id				 
	WHERE yd.tax_year = @taxYearId

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
