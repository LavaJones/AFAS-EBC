USE aca
GO
UPDATE b SET b.TaxYearId = 2017
  FROM [aca].[dbo].[EmployerTaxYearTransmissionStatus] a 
  inner join dbo.EmployerTaxYearTransmission b 
  ON a.EmployerTaxYearTransmissionId = b.EmployerTaxYearTransmissionId
  where TransmissionStatusId = 14 and a.ModifiedDate > '2017-07-21 09:05:03.5830000'
