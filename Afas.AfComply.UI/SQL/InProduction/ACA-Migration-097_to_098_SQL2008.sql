USE [aca]
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
		tax_year INT ,--INDEX sty1a_tax_year NONCLUSTERED,
		employer_id INT ,--INDEX sty1a_employer_id NONCLUSTERED,
		employee_id INT ,--INDEX sty1a_employee_id NONCLUSTERED,
		get1095C BIT --INDEX sty1a_get_1095c NONCLUSTERED,
		--INDEX sty1a_tax_year_employer_id_employee_id NONCLUSTERED(tax_year, employer_id, employee_id),
		--INDEX sty1a_tax_year_employee_id NONCLUSTERED(tax_year, employee_id)
	)

	-- Patch around the fact that certain groups have multiple approvals. gc5
	-- pull in the tax year here to make the table smaller. gc5
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
		ty1a.tax_year = @taxYearId
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
		'0' AS CorrectedInd, -- appr.employee_yearly_detail eventually, right now set off. gc5
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
		'1' AS AuthoritativeTransmittalInd, -- any client using the system and under the transmission size limit is authoritive.
		CAST(ty1a._1095C_count AS varchar(max)) AS TotalForm1095CALEMemberCnt, --tax year aca.dbo.tax_year_1095C_approval
		CAST(tya.ale AS varchar(1)) AS AggregatedGroupMemberCd, -- aca tax_year_approval flag saying it is an aggregate member.
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
					AND
				ty1a.tax_year = @taxYearId	-- redundent but makes it clear we are filtering the approval counts down. gc5
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


