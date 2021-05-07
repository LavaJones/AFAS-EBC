USE [aca]
GO

CREATE VIEW View_Insurance_Combined AS

	WITH insuranceCombined
	AS (
		SELECT
			rowid,
			a.employer_id,
			employee_id,
			a.plan_year_id,
			offered,
			offeredOn,
			accepted,
			acceptedOn,
			effectiveDate,
			insurance_id,
			ins_cont_id,
			avg_hours_month,
			hireDate,
			terminationDate,
			hra_flex_contribution,
			InOffer,
			InOfferArchive
		FROM (
			SELECT
				eioa.rowid,
				eioa.employer_id,
				eioa.employee_id,
				eioa.plan_year_id,
				eioa.offered,
				eioa.offeredOn,
				ISNULL(eioa.accepted, 0) AS accepted,
				ISNULL(eioa.acceptedOn, eioa.offeredOn) AS acceptedOn,
				ISNULL(eioa.effectiveDate, ISNULL(eioa.acceptedOn, eioa.offeredOn)) AS effectiveDate,
				eioa.insurance_id,
				eioa.ins_cont_id,
				eioa.avg_hours_month,
				ee.hireDate,
				ee.terminationDate,
				eioa.hra_flex_contribution,
				0 AS [InOffer],
				1 AS [InOfferArchive]
			FROM
				[aca].[dbo].[employee_insurance_offer_archive] eioa
				INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = eioa.employee_id)

			UNION

			SELECT
				eio.rowid,
				eio.employer_id,
				eio.employee_id,
				eio.plan_year_id,
				eio.offered,
				eio.offeredOn,
				ISNULL(eio.accepted, 0) AS accepted,
				ISNULL(eio.acceptedOn, eio.offeredOn) AS acceptedOn,
				ISNULL(eio.effectiveDate, ISNULL(eio.acceptedOn, eio.offeredOn)) AS effectiveDate,
				eio.insurance_id,
				eio.ins_cont_id,
				eio.avg_hours_month,
				ee.hireDate,
				ee.terminationDate,
				eio.hra_flex_contribution,
				1 AS [InOffer],
				0 AS [InOfferArchive]
			FROM
				[aca].[dbo].[employee_insurance_offer] eio
				INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = eio.employee_id)
			) AS A
			INNER JOIN [aca].[dbo].[plan_year] py ON (py.plan_year_id = a.plan_year_id)
			WHERE (
				-- these may not be needed anymore, gc5
				py.endDate <= '2016-12-31'
					OR
				py.startDate >= '2015-01-01'
			)
				AND
			(
				-- getting weird dates here from near future events, removing to be safe. gc5
				offeredOn <= '2016-12-31'
					AND
				acceptedOn <= '2016-12-31'
			)

	)


	SELECT *
	FROM insuranceCombined
GO

CREATE VIEW View_Combined_Date_Data AS

WITH getCombinedDateData
	AS (

		SELECT 
			ROW_NUMBER() OVER(ORDER BY ic.offeredOn) as dateRowId,
			ic.employer_id,
			ic.employee_id,
			ic.plan_year_id,
			ic.offered,
			ic.offeredOn,
			ic.accepted,
			ic.acceptedOn,
			ic.insurance_id,
			ic.ins_cont_id,
			0 AS inEffect,		 -- TODO: determine later when peeking at the term date and effective date. gc5
			ic.effectiveDate,
			ee.hireDate,
			ee.terminationDate,
			ic.hra_flex_contribution
		FROM
			[aca].[dbo].View_Insurance_Combined ic
			INNER JOIN [aca].[dbo].[employee] ee ON (ee.employee_id = ic.employee_id)
	)

	SELECT *
	FROM getCombinedDateData
GO

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
CREATE PROCEDURE SELECT_insurance_combined_by_employer
	-- Add the parameters for the stored procedure here
	@employerId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM dbo.View_Insurance_Combined
	WHERE employer_id = @employerId

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
CREATE PROCEDURE SELECT_combined_date_data_by_employer
	-- Add the parameters for the stored procedure here
	@employerId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT *
	FROM dbo.View_Combined_Date_Data
	WHERE employer_id = @employerId
END
GO
