USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[TRANSFER_insurance_change_event]
      @rowID int,
      @insuranceID int,
      @contributionID int,
      @avgHours decimal,
      @offered bit, 
      @offeredOn datetime,
      @accepted bit,
      @acceptedOn datetime,
      @modOn datetime,
      @modBy varchar(50),
      @notes varchar(max),
      @history varchar(max),
      @effDate datetime,
      @hraFlex decimal
AS

BEGIN

      /*************************************************************
      ******* Create a transaction that must fully complete ********
      **************************************************************/
      BEGIN TRANSACTION
      
	  BEGIN TRY
		  -- Step 1: Archive the current insurance offer.
		  --                - Return the new Employee_ID
		  INSERT INTO dbo.employee_insurance_offer_archive
		  (
			[rowid],
			[employee_id],
			[plan_year_id],
			[employer_id],
			[insurance_id],
			[ins_cont_id],
			[avg_hours_month],
			[offered],
			[offeredOn],
			[accepted],
			[acceptedOn],
			[modOn],
			[modBy],
			[notes],
			[history],
			[effectiveDate],
			[hra_flex_contribution],
			[OfferResourceId]
		  )
		  SELECT
			[rowid],
			[employee_id],
			[plan_year_id],
			[employer_id],
			[insurance_id],
			[ins_cont_id],
			[avg_hours_month],
			[offered],
			[offeredOn],
			[accepted],
			[acceptedOn],
			[modOn],
			[modBy],
			[notes],
			[history],
			[effectiveDate],
			[hra_flex_contribution],
			[ResourceId]
		 FROM dbo.employee_insurance_offer
		 WHERE rowid = @rowID;

		 -- Step 2: Update the new insurance offer.
		 EXEC UPDATE_insurance_offer
			@rowID,
			@insuranceID,
			@contributionID,
			@avgHours,
			@offered, 
			@offeredOn,
			@accepted,
			@acceptedOn,
			@modOn,
			@modBy,
			@notes,
			@history,
			@effDate,
			@hraFlex;
      
		COMMIT

    END TRY
    
	BEGIN CATCH
 
		ROLLBACK TRANSACTION
 
		EXEC dbo.INSERT_ErrorLogging
 
	END CATCH

	END

GO

ALTER TABLE [dbo].[import_insurance_coverage_archive] ADD [CoverageResourceId] uniqueidentifier not null default '00000000-0000-0000-0000-000000000000';
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <04/07/2016>
-- Description:	<This stored procedure is meant to delete a single insurance_carrier_import record matching the row id.>
-- =============================================
ALTER PROCEDURE [dbo].[DELETE_insurance_carrier_import_row]
	@rowID int,
	@modBy varchar(50),
	@modOn datetime
AS
BEGIN

BEGIN TRANSACTION
	BEGIN TRY

		INSERT INTO import_insurance_coverage_archive
		(
			[row_id],
			[batch_id],
			[employer_id],
			[tax_year],
			[employee_id],
			[dependent_link],
			[dependent_id],
			[fName],
			[lName],
			[mName],
			[ssn],
			[dob],
			[jan],
			[feb],
			[march],
			[april],
			[may],
			[june],
			[july],
			[august],
			[september],
			[october],
			[november],
			[december],
			[subscriber],
			[all12],
			[state_id],
			[zip],
			[jan_i],
			[feb_i],
			[march_i],
			[april_i],
			[may_i],
			[june_i],
			[july_i],
			[aug_i],
			[sep_i],
			[oct_i],
			[nov_i],
			[dec_i],
			[all12_i],
			[subscriber_i],
			[address_i],
			[city_i],
			[state_i],
			[zip_i],
			[carrier_id],
			[CoverageResourceId]
		)

		SELECT
			[row_id],
			[batch_id],
			[employer_id],
			[tax_year],
			[employee_id],
			[dependent_link],
			[dependent_id],
			[fName],
			[lName],
			[mName],
			[ssn],
			[dob],
			[jan],
			[feb],
			[march],
			[april],
			[may],
			[june],
			[july],
			[august],
			[september],
			[october],
			[november],
			[december],
			[subscriber],
			[all12],
			[state_id],
			[zip],
			[jan_i],
			[feb_i],
			[march_i],
			[april_i],
			[may_i],
			[june_i],
			[july_i],
			[aug_i],
			[sep_i],
			[oct_i],
			[nov_i],
			[dec_i],
			[all12_i],
			[subscriber_i],
			[address_i],
			[city_i],
			[state_i],
			[zip_i],
			[carrier_id],
			[ResourceId]
		FROM
			[dbo].[import_insurance_coverage]
		WHERE
			row_id = @rowID;

		DELETE FROM import_insurance_coverage
		WHERE
			row_id = @rowID;

		COMMIT

	END TRY
	BEGIN CATCH

		ROLLBACK TRANSACTION
 
		EXEC dbo.INSERT_ErrorLogging
 
	END CATCH

END
GO


