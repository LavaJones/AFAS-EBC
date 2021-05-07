USE [aca-demo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:<Travis Wells>
-- Create date: <4/9/2016>
-- Description: <This function will transpose all insurance carriers rows for a single individual. If they have multiple rows, 
--the TRUE value will always show. >
-- EXAMPLE:
--		Jan	Feb	Mar	Apr	May	Jun	Jul	Aug	Sep	Oct	Nov	Dec
--Bob	1	1	1	0	0	0	1	1	1	1	1	1
--Bob	0	0	0	0	0	1	1	1	1	1	0	0
--____________________________________________________
--Bob	1	1	1	0	0	1	1	1	1	1	1	1	Final Results for AIR system.
-- =============================================
CREATE FUNCTION [dbo].[ufnGetConsolidatedEmployeeInsurance](@employer_id int, @tax_year SMALLINT)
RETURNS TABLE 
AS
RETURN 
SELECT DISTINCT
              MIN(ic.row_id) AS first_row_id,
              ic.employee_id,
              ee.employer_id, 
              ic.dependent_id, 
              ic.tax_year,
              MAX(CASE WHEN ic.dependent_id IS NULL THEN ee.fName ELSE ed.fName END) AS fName,
              MAX(CASE WHEN ic.dependent_id IS NULL THEN ee.mName ELSE ed.mName END) AS mName,
              MAX(CASE WHEN ic.dependent_id IS NULL THEN ee.lName ELSE ed.lName END) AS lName,
              MAX(CASE WHEN ic.dependent_id IS NULL THEN ee.ssn ELSE ed.ssn END) AS ssn,
              MAX(CASE WHEN ic.dependent_id IS NULL THEN ee.dob ELSE ed.dob END) AS dob,
              MAX(CONVERT(INTEGER,ic.jan)) as Jan,
              MAX(CONVERT(INTEGER,ic.feb))as Feb,
              MAX(CONVERT(INTEGER,ic.mar)) as Mar,
              MAX(CONVERT(INTEGER,ic.apr)) as Apr,
              MAX(CONVERT(INTEGER,ic.may)) as May,
              MAX(CONVERT(INTEGER,ic.jun)) as Jun,
              MAX(CONVERT(INTEGER,ic.jul)) as Jul,
              MAX(CONVERT(INTEGER,ic.aug)) as Aug,
              MAX(CONVERT(INTEGER,ic.sep)) as Sept,
              MAX(CONVERT(INTEGER,ic.oct)) as Oct,
              MAX(CONVERT(INTEGER,ic.nov)) as Nov,
              MAX(CONVERT(INTEGER,ic.[dec])) as [Dec]
FROM
	[aca-demo].dbo.insurance_coverage ic
    INNER JOIN [aca-demo].dbo.employee ee ON (ic.employee_id = ee.employee_id)
    LEFT OUTER JOIN [aca-demo].dbo.employee_dependents ed ON (ic.dependent_id = ed.dependent_id)
WHERE
	ic.tax_year = @tax_year
		AND
	ee.employer_id = @employer_id
GROUP BY
	ic.employee_id,
	ee.employer_id,
	ic.dependent_id,
	tax_year
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Migrate_Coverage] 
	@employer_id INT, 
	@year_id SMALLINT
AS
BEGIN
	SET NOCOUNT ON;

    INSERT INTO [dbo].[insurance_coverage_editable]
	(
		employee_id,
		employer_id,
		dependent_id,
		tax_year,
		jan,
		feb,
		Mar,
		Apr,
		May,
		Jun,
		Jul,
		Aug,
		Sept,
		Oct,
		Nov,
		[Dec]
	)
	SELECT
		employee_id,
		employer_id,
		dependent_id,
		tax_year,
		jan,
		feb,
		Mar,
		Apr,
		May,
		Jun,
		Jul,
		Aug,
		Sept,
		Oct,
		Nov, 
		[Dec]
	FROM
		dbo.[ufnGetConsolidatedEmployeeInsurance](@employer_id, @year_id) 
	WHERE
		employer_id = @employer_id
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PrepareAcaForIRSStaging] 
	@employer_id INT,
	@year_id SMALLINT
AS
BEGIN TRY
	BEGIN TRAN PrepareAcaForIRSStaging
		BEGIN
		EXEC dbo.Migrate_Coverage @employer_id, @year_id
		END
	COMMIT TRAN
END TRY
BEGIN CATCH
	IF @@TRANCOUNT > 0 ROLLBACK TRAN PrepareAcaForIRSStaging
	EXEC dbo.INSERT_ErrorLogging
END CATCH
GO