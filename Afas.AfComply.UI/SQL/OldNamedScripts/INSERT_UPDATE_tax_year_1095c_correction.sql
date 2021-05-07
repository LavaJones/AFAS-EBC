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
-- Author:		Obiye, Kolokolo
-- Create date: 03/31/2017
-- Description:	stored procedure to insert and update the [tax_year_1095c_correction] table
-- =============================================
CREATE PROCEDURE [dbo].[INSERT_UPDATE_tax_year_1095c_correction]
	@tax_yearCorrectionId int,
    @tax_year int,
    @employee_id int,
    @employer_id int,
    @approvedBy varchar(50),
    @approvedOn datetime2(7),
    @get1095C bit,
    @Corrected bit,
    @OriginalUniqueSubmissionId varchar(100),
    @CorrectedUniqueSubmissionId varchar(100),
    @CorrectedUniqueRecordId varchar(100),
    @Transmitted bit,
    @ModifiedBy nvarchar(50)
AS

BEGIN TRY

      SET NOCOUNT ON;

      IF @tax_yearCorrectionId <= 0
      BEGIN
		INSERT INTO [tax_year_1095c_correction](
			tax_year
			,employee_id
			,employer_id
			,approvedBy
			,approvedOn
			,get1095C
			,ResourceId
			,Corrected
			,OriginalUniqueSubmissionId
			,CorrectedUniqueSubmissionId
			,CorrectedUniqueRecordId
			,Transmitted
			,EntityStatusId
			,ModifiedBy
			,ModifiedDate)
		VALUES(
			@tax_year
			,@employee_id
			,@employer_id
			,@approvedBy
			,@approvedOn
			,@get1095C
			,NEWID()
			,@Corrected
			,@OriginalUniqueSubmissionId
			,@CorrectedUniqueSubmissionId
			,@CorrectedUniqueRecordId
			,@Transmitted
			,1
			,@ModifiedBy
			,GETDATE()
		)
      END
      ELSE
      BEGIN
			UPDATE dbo.[tax_year_1095c_correction]
            SET tax_year = @tax_year,
                  approvedBy = @approvedBy,
                  approvedOn = @approvedOn,
                  get1095C = @get1095C,
                  Corrected = @Corrected,
				  OriginalUniqueSubmissionId = @OriginalUniqueSubmissionId,
				  CorrectedUniqueSubmissionId = @CorrectedUniqueSubmissionId,
				  CorrectedUniqueRecordId = @CorrectedUniqueRecordId,
				  Transmitted = @Transmitted,
				  ModifiedBy = @ModifiedBy,
				  ModifiedDate = GETDATE()
            WHERE tax_yearCorrectionId = @tax_yearCorrectionId
      END

      IF @tax_yearCorrectionId <= 0
      BEGIN
            SET @tax_yearCorrectionId = SCOPE_IDENTITY();
      END

      SELECT @tax_yearCorrectionId;

END TRY
BEGIN CATCH
      EXEC INSERT_ErrorLogging
END CATCH
GO

GRANT EXECUTE ON [dbo].[INSERT_UPDATE_tax_year_1095c_correction] TO [aca-user] AS [DBO]
GO


