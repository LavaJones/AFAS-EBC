USE aca
GO
UPDATE dbo.Approved1095FinalPart3 SET Approved1095Final_ID = b.Approved1095FinalId
FROM dbo.Approved1095FinalPart3 a INNER JOIN dbo.Approved1095Final b ON a.EmployeeID = b.employeeID AND a.TaxYear = b.TaxYear