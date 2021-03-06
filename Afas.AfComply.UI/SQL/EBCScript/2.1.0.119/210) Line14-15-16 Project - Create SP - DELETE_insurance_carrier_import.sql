USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[DELETE_payroll_import]    Script Date: 12/18/2017 11:44:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[DELETE_insurance_carrier_import]
	@batchID int, 
	@modBy varchar(50),
	@employerID int
AS
BEGIN
	BEGIN TRANSACTION
		BEGIN TRY
			--Archive any insurance carrier records that were in the ACT system related to the batch being removed.  
			INSERT INTO dbo.import_insurance_coverage_archive
			(
				[row_id]
			  ,[batch_id]
			  ,[employer_id]
			  ,[tax_year]
			  ,[employee_id]
			  ,[dependent_link]
			  ,[dependent_id]
			  ,[fName]
			  ,[lName]
			  ,[mName]
			  ,[ssn]
			  ,[dob]
			  ,[jan]
			  ,[feb]
			  ,[march]
			  ,[april]
			  ,[may]
			  ,[june]
			  ,[july]
			  ,[august]
			  ,[september]
			  ,[october]
			  ,[november]
			  ,[december]
			  ,[subscriber]
			  ,[all12]
			  ,[state_id]
			  ,[zip]
			  ,[jan_i]
			  ,[feb_i]
			  ,[march_i]
			  ,[april_i]
			  ,[may_i]
			  ,[june_i]
			  ,[july_i]
			  ,[aug_i]
			  ,[sep_i]
			  ,[oct_i]
			  ,[nov_i]
			  ,[dec_i]
			  ,[all12_i]
			  ,[subscriber_i]
			  ,[address_i]
			  ,[city_i]
			  ,[state_i]
			  ,[zip_i]
			  ,[carrier_id]
			  ,[modBy]
			  ,[modOn]
			  ,[ResourceId]
		)
		SELECT 
				[row_id]
			  ,[batch_id]
			  ,[employer_id]
			  ,[tax_year]
			  ,[employee_id]
			  ,[dependent_link]
			  ,[dependent_id]
			  ,[fName]
			  ,[lName]
			  ,[mName]
			  ,[ssn]
			  ,[dob]
			  ,[jan]
			  ,[feb]
			  ,[march]
			  ,[april]
			  ,[may]
			  ,[june]
			  ,[july]
			  ,[august]
			  ,[september]
			  ,[october]
			  ,[november]
			  ,[december]
			  ,[subscriber]
			  ,[all12]
			  ,[state_id]
			  ,[zip]
			  ,[jan_i]
			  ,[feb_i]
			  ,[march_i]
			  ,[april_i]
			  ,[may_i]
			  ,[june_i]
			  ,[july_i]
			  ,[aug_i]
			  ,[sep_i]
			  ,[oct_i]
			  ,[nov_i]
			  ,[dec_i]
			  ,[all12_i]
			  ,[subscriber_i]
			  ,[address_i]
			  ,[city_i]
			  ,[state_i]
			  ,[zip_i]
			  ,[carrier_id]
			  ,@modBy
			  ,GETDATE()
			  ,[ResourceId]
			FROM [dbo].[import_insurance_coverage]
			WHERE batch_id=@batchID;

			--Hard delete all of the import records related to the batch id, these records are archived into another table before this delete. 
			DELETE FROM [dbo].[import_insurance_coverage]
			WHERE batch_id=@batchID;

			-- Soft delete all of the insurance coverage records related to this batch id. 
			UPDATE [dbo].[insurance_coverage]
			SET
				EntityStatusID=3,
				ModifiedBy=@modBy,
				ModifiedDate=GETDATE()
			WHERE
				batch_id=@batchID;

			-- Set the batch row to show who soft deleted and when. 
			UPDATE batch 
			SET
				delBy=@modBy, 
				delOn=GETDATE()
			WHERE
				batch_id=@batchID;


			-- Soft DELETE any Dependents who came in on the file and do not have records that were not soft deleted. 
			UPDATE dbo.employee_dependents
			SET
				dbo.employee_dependents.EntityStatusID=3
			WHERE
				(	
					--Dependent NOT IN Insurance Coverage Editable where Entity Status is Active
					dbo.employee_dependents.dependent_id NOT IN (SELECT DISTINCT c.dependent_id FROM dbo.insurance_coverage_editable c WHERE c.EntityStatusID=1 and c.dependent_id IS NOT NULL AND c.EntityStatusID=1)
					AND
					dbo.employee_dependents.dependent_id NOT IN (SELECT DISTINCT c.dependent_id FROM dbo.insurance_coverage c WHERE c.EntityStatusID=1 AND c.dependent_id IS NOT NULL AND c.EntityStatusID=1)
				)
				AND dbo.employee_dependents.employee_id IN (SELECT employee_id FROM dbo.employee e where e.employer_id=@employerID)

		
			COMMIT
		END TRY
		BEGIN CATCH
			ROLLBACK TRANSACTION
			exec dbo.INSERT_ErrorLogging
		END CATCH
END
