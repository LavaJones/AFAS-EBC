USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[InsuranceOfferUpload]    Script Date: 12/10/2017 7:05:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[InsuranceOfferUpload]
	@insuranceOffer BULK_import_insurance_offer readonly
AS
BEGIN
	BEGIN TRAN

		DELETE 
			eioa
		FROM 
			[dbo].[employee_insurance_offer_archive]  eioa
        INNER JOIN 
			@insuranceOffer ino
        ON 
			ino.employee_id = eioa.employee_id 
				AND 
			ino.plan_year_id = eioa.plan_year_id
        

		DELETE 
			eio
		FROM 
			[dbo].[employee_insurance_offer]eio
        INNER JOIN 
			@insuranceOffer ino
        ON 
			ino.employee_id = eio.employee_id 
				AND 
			ino.plan_year_id = eio.plan_year_id


	COMMIT TRAN

	INSERT INTO [dbo].[employee_insurance_offer] 
		(	employee_id, 
			plan_year_id, 
			employer_id, 
			insurance_id, 
			ins_cont_id,
			offered, 
			offeredOn, 
			accepted, 
			acceptedOn, 
			modOn, 
			modBy, 
			notes, 
			history, 
			effectiveDate, 
			hra_flex_contribution  )
	SELECT  
			eio.employee_id, 
			eio.plan_year_id, 
			eio.employer_id, 
			eio.insurance_id, 
			eio.ins_cont_id,
			eio.offered, 
			eio.offeredOn, 
			eio.accepted, 
			eio.acceptedOn, 
			eio.modOn, 
			eio.modBy, 
			eio.notes, 
			eio.history, 
			eio.effectiveDate, 
			eio.hra_flex_contribution
	FROM 
		@insuranceOffer eio
	INNER JOIN
		(SELECT employee_id, plan_year_id, MAX(effectiveDate) AS MaxDateTime
		FROM @insuranceOffer
		GROUP BY employee_id, plan_year_id) grouped 
	ON 
		eio.employee_id = grouped.employee_id 
			And
		eio.plan_year_id = grouped.plan_year_id
			AND 
		eio.effectiveDate = grouped.MaxDateTime;

	INSERT INTO [dbo].[employee_insurance_offer_archive]
		(	
			rowid,
			employee_id, 
			plan_year_id, 
			employer_id, 
			insurance_id, 
			ins_cont_id,
			offered, 
			offeredOn, 
			accepted, 
			acceptedOn, 
			modOn, 
			modBy, 
			notes, 
			history, 
			effectiveDate, 
			hra_flex_contribution )
	SELECT  
			eio.rowid,
			eioa.employee_id, 
			eioa.plan_year_id, 
			eioa.employer_id, 
			eioa.insurance_id, 
			eioa.ins_cont_id,
			eioa.offered, 
			eioa.offeredOn, 
			eioa.accepted, 
			eioa.acceptedOn, 
			eioa.modOn, 
			eioa.modBy, 
			eioa.notes, 
			eioa.history, 
			eioa.effectiveDate, 
			eioa.hra_flex_contribution
	FROM 
		@insuranceOffer eioa
	INNER JOIN
		[dbo].[employee_insurance_offer] eio
	ON 
		eioa.employee_id = eio.employee_id 
			AND
		eioa.plan_year_id = eio.plan_year_id
			AND 
		eioa.effectiveDate <> eio.effectiveDate;

END
GO
GRANT EXECUTE ON [dbo].[InsuranceOfferUpload] TO [aca-user]
GO
GRANT EXEC ON TYPE::dbo.BULK_import_insurance_offer TO [aca-user]

