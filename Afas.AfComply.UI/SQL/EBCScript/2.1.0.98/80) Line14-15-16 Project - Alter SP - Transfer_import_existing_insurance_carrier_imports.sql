USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[TRANSFER_import_existing_insurance_carrier_imports]    Script Date: 12/11/2017 9:11:44 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <8/18/2014>
-- Description:	<This stored procedure is meant update an existing employee from the import table. It will then Delete the record from the import_employee table.
-- Changes:		
--		12/11/2017 tlw
--			-- added the batch id.
-- =============================================
ALTER PROCEDURE [dbo].[TRANSFER_import_existing_insurance_carrier_imports]
	@rowID int,
	@taxYear int,
	@carrierID int,
	@employeeID int, 
	@dependentID int, 
	@all12 bit, 
	@jan bit, 
	@feb bit, 
	@mar bit, 
	@apr bit, 
	@may bit, 
	@jun bit, 
	@jul bit, 
	@aug bit, 
	@sep bit, 
	@oct bit, 
	@nov bit, 
	@dec bit, 
	@history varchar(max),
	@batchID int,
	@modBy varchar(50)
AS

BEGIN

	/*************************************************************
	******* Create a transaction that must fully complete ********
	**************************************************************/
	BEGIN TRANSACTION
		BEGIN TRY
			DECLARE @default varchar(50);
			SET @default = 'All Employees';

			-- Step 1: Create a new EMPLOYEE based on the registration information.
			--			- Return the new Employee_ID
			INSERT INTO [insurance_coverage](
				tax_year,
				carrier_id,
				employee_id,
				dependent_id,
				all12,
				jan,
				feb,
				mar,
				apr,
				may,
				jun,
				jul,
				aug,
				sep,
				oct,
				nov,
				[dec],
				history,
				batch_id, 
				CreatedBy,
				CreatedDate,
				ModifiedBy,
				ModifiedDate,
				EntityStatusID
				)
			VALUES(
				@taxYear,
				@carrierID,
				@employeeID,
				@dependentID,
				@all12,
				@jan,
				@feb,
				@mar,
				@apr,
				@may,
				@jun,
				@jul,
				@aug,
				@sep,
				@oct,
				@nov,
				@dec,
				@history,
				@batchID,
				@modBy,
				GETDATE(),
				@modBy,
				GETDATE(),
				1
				)
	
			-- Step 2: DELETE the record from the import_employee table. 
				DELETE FROM import_insurance_coverage
				WHERE
					row_id = @rowID;
			COMMIT
		END TRY
		BEGIN CATCH

			ROLLBACK TRANSACTION
		END CATCH

END

