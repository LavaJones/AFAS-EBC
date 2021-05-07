
USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		
-- Create date: 
-- Description:	This stored procedure is meant to return all active Plan Year Group  for the specific Employee Id 
-- Modifications : Modified Date 6/28/2017, Modified by GN.The select query in dev database is not filtering records on EntityStatusId(1=Active, 2=Deactive(DELETE)).
--                 Though they are deactive all the records selected and populated on grid in admin/AdminPortal/PlanYearGroup.aspx page. So, Added filter to select only Active records.
--                 Replaced "*" with all coloumn names. 
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_PlanYearGroup_ForEmployer]
      @EmployerId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

     SELECT [PlanYearGroupId]
		  ,[ResourceId]
		  ,[EntityStatusId]
		  ,[CreatedBy]
		  ,[CreatedDate]
		  ,[ModifiedBy]
		  ,[ModifiedDate]
		  ,[GroupName]
		  ,[Employer_id]
	 FROM [aca].[dbo].[PlanYearGroup]
      WHERE [Employer_Id] = @EmployerId
		AND [EntityStatusId] = 1;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
-----------------------------------------------------------------------------------------

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		
-- Create date: 
-- Description:	This stored procedure is meant to return all active Plan Year Group  for the specific Plan Year Group Id 
-- Modifications : Modified Date 6/28/2017, Modified by GN.The select query in dev database is not filtering records on EntityStatusId(1=Active, 2=Deactive(DELETE)).
--				   Replaced "*" with all coloumn names. 
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_PlanYearGroup]
      @PlanYearGroupId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT [PlanYearGroupId]
		  ,[ResourceId]
		  ,[EntityStatusId]
		  ,[CreatedBy]
		  ,[CreatedDate]
		  ,[ModifiedBy]
		  ,[ModifiedDate]
		  ,[GroupName]
		  ,[Employer_id]
	 FROM [aca].[dbo].[PlanYearGroup]
      WHERE [PlanYearGroupId] = @PlanYearGroupId
	  AND [EntityStatusId] = 1;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH

GO


-----------------------------------------------------------------------------------------

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Travis Wells>
-- Create date: <10/15/2014>
-- Description:	<This stored procedure is meant to return all measurement periods for the specific values.>
-- Modifications : Modified Date 6/28/2017, Modified by GN. Replaced "*" with all coloumn names.
-- =============================================
Alter PROCEDURE [dbo].[SELECT_planyear_measurement](
	@employerID int, 
	@planyearID int,
	@employeeTypeID int
	)
AS
BEGIN
	SELECT [measurement_id]
      ,[employer_id]
      ,[plan_year_id]
      ,[employee_type_id]
      ,[measurement_type_id]
      ,[meas_start]
      ,[meas_end]
      ,[admin_start]
      ,[admin_end]
      ,[open_start]
      ,[open_end]
      ,[stability_start]
      ,[stability_end]
      ,[notes]
      ,[history]
      ,[modOn]
      ,[modBy]
      ,[sw_start]
      ,[sw_end]
      ,[sw2_start]
      ,[sw2_end]
      ,[meas_complete]
      ,[admin_complete]
      ,[stability_complete]
      ,[meas_completed_by]
      ,[admin_completed_by]
      ,[stability_completed_by]
      ,[meas_completed_on]
      ,[admin_completed_on]
      ,[stability_completed_on]
      ,[ResourceId]
	FROM [aca].[dbo].[measurement]
	WHERE employer_id=@employerID
	AND plan_year_id = @planyearID
	AND employee_type_id = @employeeTypeID;
END


GO
-------------------------------------------------------------------------------------

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Travis Wells>
-- Create date: <2/1/2016>
-- Description:	<This stored procedure is meant to return all payroll data for a specific batch ID.>
-- Modifications: Modified Date 6/28/2017, Modified by GN. Replaced "*" with all coloumn names.
-- =============================================
Alter PROCEDURE [dbo].[SELECT_payroll_batch](
	@batchID int,
	@employerID int
	)
AS
BEGIN
	SELECT [row_id]
		  ,[employer_id]
		  ,[batch_id]
		  ,[employee_id]
		  ,[gp_id]
		  ,[act_hours]
		  ,[sdate]
		  ,[edate]
		  ,[cdate]
		  ,[modBy]
		  ,[modOn]
		  ,[description]
		  ,[external_id]
		  ,[ext_emp_id]
		  ,[fName]
		  ,[lName]
		  ,[history]
  FROM [aca].[dbo].[View_payroll]
	WHERE employer_id=@employerID AND
	batch_id=@batchID
	ORDER BY batch_id ASC;
END


GO
------------------------------------------------------------------------------------------------------------


/****** Object:  StoredProcedure [dbo].[SELECT_past_due_measurement_periods]    Script Date: 6/28/2017 9:50:17 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Travis Wells>
-- Create date: <10/15/2014>
-- Description:	<This stored procedure is meant to return all measurement periods for the specific values.>
-- =============================================
Alter PROCEDURE [dbo].[SELECT_past_due_measurement_periods]
AS
BEGIN
	SELECT [measurement_id]
      ,[employer_id]
      ,[plan_year_id]
      ,[employee_type_id]
      ,[measurement_type_id]
      ,[meas_start]
      ,[meas_end]
      ,[admin_start]
      ,[admin_end]
      ,[open_start]
      ,[open_end]
      ,[stability_start]
      ,[stability_end]
      ,[notes]
      ,[history]
      ,[modOn]
      ,[modBy]
      ,[sw_start]
      ,[sw_end]
      ,[sw2_start]
      ,[sw2_end]
      ,[meas_complete]
      ,[admin_complete]
      ,[stability_complete]
      ,[meas_completed_by]
      ,[admin_completed_by]
      ,[stability_completed_by]
      ,[meas_completed_on]
      ,[admin_completed_on]
      ,[stability_completed_on]
      ,[ResourceId]
  FROM [aca].[dbo].[measurement]
	WHERE meas_end < SYSDATETIME() and (meas_complete is null or meas_complete = 0);
END


GO

---------------------------------------------------------------------------------------------------------------------------

GO

/****** Object:  StoredProcedure [dbo].[SELECT_open_invoices]    Script Date: 6/28/2017 9:53:46 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Travis Wells>
-- Create date: <11/10/2015>
-- Description:	<This stored procedure is meant to return all invoices that are currently open.>
-- Modifications: Modified Date 6/28/2017, Modified by GN. Replaced "*" with all coloumn names.
-- =============================================
Alter PROCEDURE [dbo].[SELECT_open_invoices]
AS
BEGIN
	SELECT [invoice_id]
		  ,[employer_id]
		  ,[invoice_month]
		  ,[invoice_year]
		  ,[count]
		  ,[base_fee]
		  ,[employee_fee]
		  ,[su_fee]
		  ,[total]
		  ,[createdOn]
		  ,[createdBy]
		  ,[message]
		  ,[paymentConfirmed]
		  ,[history]
		  ,[ResourceId]
  FROM [aca].[dbo].[invoice]
	WHERE paymentConfirmed IS NULL or paymentConfirmed = 0;
END

GO


---------------------------------------------------------------------------------------------------------------------
GO
/****** Object:  StoredProcedure [dbo].[SELECT_employee_dependents]    Script Date: 6/30/2017 9:12:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <2-24-2016>
-- Description:	<This stored procedure is meant to return all dependents and existing employee has.>
-- Modifications:Modified date 6/30/2017, modified by GN. Replaced "*" with columns. 
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_employee_dependents](
	@employeeID int
	)
AS
BEGIN
	SELECT [dependent_id]
		  ,[employee_id]
		  ,[fName]
		  ,[mName]
		  ,[lName]
		  ,[ssn]
		  ,[dob]
		  ,[ResourceId]
    FROM [aca].[dbo].[employee_dependents]
	WHERE employee_id=@employeeID;
END





