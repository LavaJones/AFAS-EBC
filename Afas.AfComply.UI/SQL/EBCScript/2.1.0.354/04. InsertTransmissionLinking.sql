USE aca
GO
INSERT INTO dbo.TransmissionLinking ([Approved1095FinalId]
      ,[EmployerId]
      ,[EmployeeId]
         ,[RecordId]
      ,[TaxYear]
      ,[TaxYearEmployerTransmissionId]
      ,[TaxYearEmployeeTransmissionId]
      ,[EntityStatusId]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[ModifiedBy]
      ,[ModifiedDate])
SELECT af.Approved1095FinalId, 
             et.employer_id, 
             et.employee_id,
             et.RecordID, 
             et.tax_year, 
             et.tax_year_employer_transmissionId, 
             et.tax_year_employee_transmissionId, 
             1, 
             'SYSTEM', 
             GETDATE(), 
             'SYSTEM', 
             GETDATE()
FROM dbo.tax_year_employee_transmission et 
       INNER JOIN dbo.Approved1095Final af 
             ON et.employee_id = af.employeeID
WHERE et.transmission_status_code_id in (2,1) and et.EntityStatusId = 1 and et.tax_year = 2017 and af.EntityStatusId = 1
