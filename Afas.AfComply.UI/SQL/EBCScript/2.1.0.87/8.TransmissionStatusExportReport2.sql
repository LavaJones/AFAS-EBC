USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[Employer_Export_Info]    Script Date: 11/30/2017 9:30:13 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[TransmissionStatusExportReport]
AS
BEGIN

	SELECT er.name, es.StartDate, es.EndDate, et.TaxYearId, er.ein, ts.TransmissionStatusName, tya.ale, tya.dge
	FROM dbo.EmployerTaxYearTransmissionStatus es 
		INNER JOIN dbo.EmployerTaxYearTransmission et 
			ON es.EmployerTaxYearTransmissionId = et.EmployerTaxYearTransmissionId 
		INNER JOIN dbo.employer er 
			ON er.employer_id = et.EmployerId
		INNER JOIN dbo.TransmissionStatus ts
			ON es.TransmissionStatusId = ts.TransmissionStatusId
		INNER JOIN dbo.tax_year_approval tya
			ON er.employer_id = tya.employer_id
END
GO
GRANT EXECUTE ON [dbo].[TransmissionStatusExportReport] TO [aca-user]
GO
