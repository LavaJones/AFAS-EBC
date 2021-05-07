USE aca
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Obiye Kolokolo
-- Create date: 04/14/2017
-- Description:	updates the transmitted bit for given employee and tax year
-- =============================================
CREATE PROCEDURE UPDATE_employer_tax_year_1095c_correction_transmitted_bit
	-- Add the parameters for the stored procedure here
	@tax_year int,
	@employee_id int,
	@Transmitted bit,
	@ModifiedBy nvarchar(50)
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE tax_year_1095c_correction
	SET Transmitted = @Transmitted,
		ModifiedBy = @ModifiedBy,
		ModifiedDate = GETDATE()
	WHERE tax_year = @tax_year
	AND employee_id = @employee_id
	AND EntityStatusId = 1

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO

GRANT EXECUTE ON UPDATE_employer_tax_year_1095c_correction_transmitted_bit TO [aca-user] as [dbo]
GO