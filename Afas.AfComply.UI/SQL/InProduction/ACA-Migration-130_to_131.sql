USE aca
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Obiye, Kolokolo
-- Create date: 05/02/2016
-- Description:	Inserts or updates record in tax_year_1095c_correction_exception table
-- =============================================
CREATE PROCEDURE INSERT_UPDATE_tax_year_1095c_correction_exception 
	-- Add the parameters for the stored procedure here
	 @TaxYear1095cCorrectionExceptionId INT OUTPUT,
     @tax_year INT,
     @employer_id INT,
     @employee_id INT,
     @Justification varchar(2048),
     @CreatedBy nvarchar(50),
     @ModifiedBy nvarchar(50)
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF @TaxYear1095cCorrectionExceptionId = 0
	BEGIN
		INSERT INTO dbo.tax_year_1095c_correction_exception(
			[tax_year],
			[employer_id],
			[employee_id],
			[Justification],
			[CreatedBy],
			[CreatedDate],
			[ModifiedBy],
			[ModifiedDate],
			[EntityStatusId],
			[ResourceId]
		) VALUES(
			@tax_year,
			@employer_id,
			@employee_id,
			@Justification,
			@CreatedBy,
			GETDATE(),
			@ModifiedBy,
			GETDATE(),
			1,
			NEWID()
		)
	END
	ELSE
	BEGIN

		UPDATE dbo.tax_year_1095c_correction_exception
		SET Justification = @Justification,
			ModifiedBy = @ModifiedBy,
			ModifiedDate = GETDATE()
		WHERE TaxYear1095cCorrectionExceptionId = @TaxYear1095cCorrectionExceptionId

	END

	 IF @TaxYear1095cCorrectionExceptionId = 0
      BEGIN
            SET @TaxYear1095cCorrectionExceptionId = SCOPE_IDENTITY();
      END

      SELECT @TaxYear1095cCorrectionExceptionId

END TRY
BEGIN CATCH
      EXEC INSERT_ErrorLogging
END CATCH
GO

GRANT EXECUTE ON [dbo].[INSERT_UPDATE_tax_year_1095c_correction_exception] TO [aca-user] AS [DBO]
GO




