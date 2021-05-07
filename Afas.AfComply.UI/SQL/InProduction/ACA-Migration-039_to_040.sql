USE [aca]
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
ALTER PROCEDURE [dbo].[Migrate_Coverage] 
	@employer_id INT, 
	@year_id SMALLINT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE [dbo].[insurance_coverage_editable]
	WHERE
		tax_year = @year_id
			AND
		employer_id = @employer_id;

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
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <4/7/2016>
-- Description:	<This stored procedure is meant to return all LINE 3 info.>
-- Changes:
--			
-- =============================================
ALTER PROCEDURE [dbo].[sp_AIR_SELECT_employee_LINE3_coverage]
	@employeeID int
AS

BEGIN
WITH cim
AS
	(
	SELECT
		cim.covered_individual_id,
		time_frame_id,
		CAST(covered_indicator AS INT) AS COV_IND
	FROM
		air.emp.covered_individual_monthly_detail cim
        INNER JOIN air.emp.covered_individual ci ON (cim.covered_individual_id = ci.covered_individual_id)
	WHERE
	ci.employee_id = @employeeID       
),
cim_pivoted AS
(
	SELECT
		covered_individual_id, [25], [26], [27], [28], [29], [30], [31], [32], [33], [34], [35], [36] --[13],[14],[15],[16],[17],[18],[19],[20],[21],[22],[23],[24]
	FROM
		cim
	PIVOT (
		MAX(COV_IND) FOR time_frame_id IN ([25], [26], [27], [28], [29], [30], [31], [32], [33], [34], [35], [36]) -- ([13],[14],[15],[16],[17],[18],[19],[20],[21],[22],[23],[24])
	) as cip
)
SELECT
	ci.employee_id,
	ci.first_name,
	ci.last_name,
	ci.ssn,
	ci.birth_date,
	cip.* 
FROM
	cim_pivoted cip
    INNER JOIN air.emp.covered_individual ci ON (cip.covered_individual_id = ci.covered_individual_id)
END
GO

