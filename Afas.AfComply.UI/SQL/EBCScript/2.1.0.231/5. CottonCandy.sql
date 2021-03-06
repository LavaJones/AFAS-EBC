USE aca
GO
UPDATE [aca].[dbo].[Approved1095FinalPart3] SET SSN = b.SSN
FROM dbo.Approved1095FinalPart3 a INNER JOIN dbo.employee b ON a.EmployeeID = b.employee_id
where a.DependantID = 0
GO
UPDATE [aca].[dbo].[Approved1095FinalPart3] SET SSN = b.SSN
FROM dbo.Approved1095FinalPart3 a INNER JOIN dbo.employee_dependents b ON a.EmployeeID = b.employee_id AND b.dependent_id = a.DependantID
where a.DependantID > 0
GO
UPDATE [aca].[dbo].[Approved1095FinalPart3] SET Dob = b.dob
FROM dbo.Approved1095FinalPart3 a INNER JOIN dbo.employee_dependents b ON a.EmployeeID = b.employee_id AND b.dependent_id = a.DependantID
where a.DependantID > 0 and a.SSN is null