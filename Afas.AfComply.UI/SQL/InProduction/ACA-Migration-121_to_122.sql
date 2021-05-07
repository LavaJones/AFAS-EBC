USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Obiye, Kolokolo
-- Create date: 04/18/2017
-- Description:	check if a tax year 1095c correction already exists
-- =============================================
CREATE PROCEDURE CHECK_if_tax_year_1095c_correction_exists
	-- Add the parameters for the stored procedure here
	@employee_id int,
	@tax_year int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT TOP 1 *
	FROM tax_year_1095c_correction
	WHERE employee_id = @employee_id
	AND tax_year = @tax_year
	AND EntityStatusId = 1


END
GO

GRANT EXECUTE ON CHECK_if_tax_year_1095c_correction_exists TO [aca-user] as [dbo]
GO
