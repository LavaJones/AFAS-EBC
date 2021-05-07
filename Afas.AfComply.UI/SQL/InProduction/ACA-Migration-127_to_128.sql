USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Obiye, Kolokolo
-- Create date: 04/28/2016
-- Description:	inserts or updates insurance coverage editable table
-- =============================================
CREATE PROCEDURE INSERT_UPDATE_insurance_coverage_editable 
	-- Add the parameters for the stored procedure here
	@row_id INT,
	@employee_id INT,
	@employer_id INT,
	@dependent_id INT = NULL,
	@tax_year INT,
	@Jan bit,
	@Feb bit,
	@Mar bit,
	@Apr bit,
	@May bit,
	@Jun bit,
	@Jul bit,
	@Aug bit,
	@Sept bit,
	@Oct bit,
	@Nov bit,
	@Dec bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @existing_row_id INT = 0;
	SELECT @existing_row_id = row_id FROM [aca].[dbo].[insurance_coverage_editable] ice WHERE row_id = @row_id

    -- Insert statements for procedure here
	IF @existing_row_id = 0
	BEGIN

		SET IDENTITY_INSERT [aca].[dbo].[insurance_coverage_editable] ON

		INSERT INTO [aca].[dbo].[insurance_coverage_editable](
			row_id
		   ,employee_id
		   ,employer_id
		   ,dependent_id
		   ,tax_year
		   ,Jan
		   ,Feb
		   ,Mar
		   ,Apr
		   ,May
		   ,Jun
		   ,Jul
		   ,Aug
		   ,Sept
		   ,Oct
		   ,Nov
		   ,Dec
		   ,ResourceId
		)VALUES(
			@row_id
		   ,@employee_id
		   ,@employer_id
		   ,@dependent_id
		   ,@tax_year
		   ,@Jan
		   ,@Feb
		   ,@Mar
		   ,@Apr
		   ,@May	
		   ,@Jun
		   ,@Jul
		   ,@Aug
		   ,@Sept
		   ,@Oct
		   ,@Nov
		   ,@Dec
		   ,NEWID())

		SET IDENTITY_INSERT [aca].[dbo].[insurance_coverage_editable] OFF
	END
	ELSE
	BEGIN
		UPDATE insurance_coverage_editable
		SET
			Jan=@Jan,
			Feb=@Feb,
			Mar=@Mar,
			Apr=@Apr,
			May=@May,
			Jun=@Jun,
			Jul=@Jul,
			Aug=@Aug,
			Sept=@Sept,
			Oct=@Oct,
			Nov=@Nov,
			[Dec]=@Dec
		WHERE
			row_id=@row_id;
	END

END
GO

GRANT EXECUTE ON [dbo].[INSERT_UPDATE_insurance_coverage_editable] TO [aca-user] AS [dbo]
GO
