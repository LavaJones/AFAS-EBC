USE aca
GO

  UPDATE aca.dbo.Approved1095FinalPart3 SET 
	   [EnrolledJan] = a.jan 
      ,[EnrolledFeb] = a.feb
      ,[EnrolledMar] = a.mar
      ,[EnrolledApr] = a.apr
      ,[EnrolledMay] = a.may
      ,[EnrolledJun] = a.jun
      ,[EnrolledJul] = a.jul
      ,[EnrolledAug] = a.aug
      ,[EnrolledSep] = a.sep
      ,[EnrolledOct] = a.oct
      ,[EnrolledNov] = a.nov
      ,[EnrolledDec] = a.dec
  FROM aca.dbo.View_full_insurance_coverage a 
	INNER JOIN aca.dbo.Approved1095FinalPart3 b 
  ON a.employee_id = b.EmployeeID AND a.tax_year = b.TaxYear