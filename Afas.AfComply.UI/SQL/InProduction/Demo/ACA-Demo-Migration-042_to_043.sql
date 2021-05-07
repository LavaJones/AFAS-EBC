USE [aca-demo]

INSERT INTO TransmissionStatus ([TransmissionStatusName],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate]) 
VALUES('ETL','SYSTEM',GETDATE(),'SYSTEM',GETDATE())
GO

INSERT INTO TransmissionStatus ([TransmissionStatusName],[CreatedBy],[CreatedDate],[ModifiedBy],[ModifiedDate]) 
VALUES('Halt','SYSTEM',GETDATE(),'SYSTEM',GETDATE())
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sp_AIR_ETL_ShortBuild]
	@employerID int,
	@taxYear int,
	@employeeID int
AS

BEGIN TRY

	EXEC [air].etl.spETL_ShortBuild
		@employerid,
		@taxYear,
		@employeeID
END TRY
BEGIN CATCH
	exec dbo.INSERT_ErrorLogging
END CATCH
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/21/2016>
-- Description:	<This stored procedure is meant to return all 4980H codes.>
-- Changes:
--			
-- =============================================
ALTER PROCEDURE [dbo].[sp_AIR_SELECT_4980H_codes]
AS

BEGIN
	SELECT
		[4980H_code],
      [description]
	FROM
		[air].[il]._4980H_code
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
		[air-demo].emp.covered_individual_monthly_detail cim
        INNER JOIN [air-demo].emp.covered_individual ci ON (cim.covered_individual_id = ci.covered_individual_id)
	WHERE
	ci.employee_id = @employeeID       
),
cim_pivoted AS
(
	SELECT
		-- 2016 values replacing the 2015 values. gc5  [13],[14],[15],[16],[17],[18],[19],[20],[21],[22],[23],[24]
		covered_individual_id, [25], [26], [27], [28], [29], [30], [31], [32], [33], [34], [35], [36]
	FROM
		cim
	PIVOT (
		-- 2016 values replacing the 2015 values. gc5 ([13],[14],[15],[16],[17],[18],[19],[20],[21],[22],[23],[24])
		MAX(COV_IND) FOR time_frame_id IN ([25], [26], [27], [28], [29], [30], [31], [32], [33], [34], [35], [36]) 
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
    INNER JOIN [air-demo].emp.covered_individual ci ON (cip.covered_individual_id = ci.covered_individual_id)
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/24/2016>
-- Description:	<This stored procedure is meant to return a single district.>
-- Changes:
--	4-18-2016	TLW
--		- Started using the the appr.employee_yearly_detail for the source of the Employee ID's. 
--			
-- =============================================
ALTER PROCEDURE [dbo].[sp_AIR_SELECT_employer_employees_in_yearly_detail]
	@employerID int,
	@taxYear int
AS

BEGIN
	SELECT DISTINCT
		employee_id
	FROM
		[air].[appr].[employee_yearly_detail]
	WHERE
		_1095C=1
			AND
		employer_id=@employerID 
			AND
		year_id=@taxYear;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/21/2016>
-- Description:	<This stored procedure is meant to return all activities.>
-- Changes:
--			
-- =============================================
ALTER PROCEDURE [dbo].[sp_AIR_SELECT_mec_codes]
AS

BEGIN
	SELECT
		[mec_code],
		[description]
	FROM
		[air].[il].mec_code
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <4/16/2016>
-- Description:	<This stored procedure is meant to return all Monthly Status rows from the AIR system.>
-- Changes:
--			
-- =============================================
ALTER PROCEDURE [dbo].[sp_AIR_SELECT_status_codes]
AS

BEGIN
	SELECT
		[monthly_status_id],
		[status_description]
	FROM
		[air].[emp].[monthly_status]
END

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/22/2016>
-- Description:	<This stored procedure is meant to move a finalized 1095C form to the edited table.>
-- Changes:
--			
-- =============================================
ALTER PROCEDURE [dbo].[sp_AIR_UPDATE_approved_monthly_detail]
	@employeeID int,
	@timeFrameID int,
	@employerID int,
	@hours decimal(18,2),
	@ooc nchar(2),
	@mec bit, 
	@lcmp decimal,
	@ash nchar(2),
	@enrolled bit,
	@monthlyStatusID int,
	@insuranceTypeID int,
	@modBy varchar(50),
	@modOn datetime
AS

BEGIN

DECLARE @employeeID2 int;
SET @employeeID2=0;

UPDATE [air].[appr].employee_monthly_detail
	SET
		offer_of_coverage_code=@ooc,
		mec_offered=@mec,
		share_lowest_cost_monthly_premium=@lcmp,
		safe_harbor_code=@ash,
		enrolled=@enrolled,
		monthly_status_id=@monthlyStatusID,
		insurance_type_id=@insuranceTypeID,
		modified_by=@modBy,
		modified_date=@modOn
	WHERE
		employee_id=@employeeID AND
		time_frame_id=@timeFrameID

IF @@ROWCOUNT = 0
	BEGIN
		/*************************************************************
		****** Changed this to look at monthly detail as some employees 
		existedin the system, but had no Monthly Details to write too. 
		*************************************************************/
		SELECT @employeeID2=employee_id FROM [air].appr.employee_monthly_detail
		WHERE employee_id=@employeeID;

		PRINT 'EMPLOYEE ID: ' + CONVERT(varchar, @employeeID2);

		IF @employeeID2 = 0
			BEGIN
				-- Extract Employee Info through AIR Process.
				EXEC [air].etl.spETL_Build
					@employerid, 
					-- changed hard coded 2015 to 2016. gc5
					2016, 
					@employeeID
			END

		UPDATE [air].[appr].employee_monthly_detail
			SET
				offer_of_coverage_code=@ooc,
				mec_offered=@mec,
				share_lowest_cost_monthly_premium=@lcmp,
				safe_harbor_code=@ash,
				enrolled=@enrolled,
				monthly_status_id=@monthlyStatusID,
				insurance_type_id=@insuranceTypeID,
				modified_by=@modBy,
				modified_date=@modOn
			WHERE
				employee_id=@employeeID AND
				time_frame_id=@timeFrameID
	END

END

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
ALTER PROCEDURE SELECT_employee_classification_by_plan_year_and_employer
	-- Add the parameters for the stored procedure here
	@employerId INT,
	@planYearId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		ins.insurance_description,
		ins.insurance_id,
		ins.employee_contribution,
		ins.insurance_type,
		ec.[description] AS classification_description,
		ec.classification_id
	FROM [employee_classification] ec 
		LEFT OUTER JOIN (
			SELECT
				i.insurance_id, 
				it.[name] as insurance_type,
				ic.classification_id,
				convert(numeric(18,2),(i.monthlycost - ic.amount)) as employee_contribution,
				i.[description] AS insurance_description
			FROM [insurance] i 
				INNER JOIN [insurance_contribution] ic ON i.insurance_id = ic.insurance_id
				INNER JOIN [insurance_type] it ON it.insurance_type_id = i.insurance_type_id
			WHERE i.plan_year_id = @planYearId) ins 
		ON ec.classification_id = ins.classification_id
	WHERE ec.employer_id = @employerId

END
GO