USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <12/30/2014>
-- Description:	<This stored procedure is meant to return a specific Vendor.>
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_vendor](
	@vendorID int
	)
AS
BEGIN
	SELECT
		[vendor_id],
		[name],
		[autoUpload]
	FROM [payroll_vendor]
	WHERE [vendor_id]=@vendorID;
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan, McCully
-- Create date: 9/19/2016
-- Description: Select all Rows of UploadedFileInfo for an Employer that are unprocessed
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_UploadedFileInfo_UnprocessedForEmployer]
		@employerId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT
		[UploadedFileInfoId]
      ,[EmployerId]
      ,[UploadedByUser]
      ,[UploadTime]
      ,[UploadSourceDescription]
      ,[UploadTypeDescription]
      ,[FileTypeDescription]
      ,[FileName]
      ,[Processed]
      ,[FailedProcessing]
      ,[ArchiveFileInfoId]
      ,[ResourceId]
      ,[EntityStatusId]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[ModifiedBy]
      ,[ModifiedDate]
	 FROM [dbo].[UploadedFileInfo] 
      WHERE [Processed] = 0 AND [EmployerId] = @employerId AND EntityStatusId = 1;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan McCully
-- Create date: 9/19/2016
-- Description: Select all Rows of UploadedFileInfo that are unprocessed
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_UploadedFileInfo_Unprocessed]
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT
	[UploadedFileInfoId]
      ,[EmployerId]
      ,[UploadedByUser]
      ,[UploadTime]
      ,[UploadSourceDescription]
      ,[UploadTypeDescription]
      ,[FileTypeDescription]
      ,[FileName]
      ,[Processed]
      ,[FailedProcessing]
      ,[ArchiveFileInfoId]
      ,[ResourceId]
      ,[EntityStatusId]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[ModifiedBy]
      ,[ModifiedDate]
	FROM [dbo].[UploadedFileInfo] 
      WHERE [Processed] = 0  AND EntityStatusId = 1;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan McCully
-- Create date: 9/19/2016
-- Description: Select all Rows of UploadedFileInfo for an Employer
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_UploadedFileInfo_ForEmployer]
      @employerId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT
	   [UploadedFileInfoId]
      ,[EmployerId]
      ,[UploadedByUser]
      ,[UploadTime]
      ,[UploadSourceDescription]
      ,[UploadTypeDescription]
      ,[FileTypeDescription]
      ,[FileName]
      ,[Processed]
      ,[FailedProcessing]
      ,[ArchiveFileInfoId]
      ,[ResourceId]
      ,[EntityStatusId]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[ModifiedBy]
      ,[ModifiedDate]
	FROM [dbo].[UploadedFileInfo] 
      WHERE [EmployerId] = @employerId AND EntityStatusId = 1;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan McCully
-- Create date: 10/10/2016
-- Description: Select all Rows of UploadedFileInfo that Failed Processing
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_UploadedFileInfo_Failed]
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT [UploadedFileInfoId]
      ,[EmployerId]
      ,[UploadedByUser]
      ,[UploadTime]
      ,[UploadSourceDescription]
      ,[UploadTypeDescription]
      ,[FileTypeDescription]
      ,[FileName]
      ,[Processed]
      ,[FailedProcessing]
      ,[ArchiveFileInfoId]
      ,[ResourceId]
      ,[EntityStatusId]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[ModifiedBy]
      ,[ModifiedDate]
	FROM [dbo].[UploadedFileInfo] 
      WHERE [FailedProcessing] = 1 AND [Processed] = 0;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan McCully
-- Create date: 9/19/2016
-- Description: Select a Single Row of 
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_UploadedFileInfo]
      @UploadedFileInfoId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT [UploadedFileInfoId]
      ,[EmployerId]
      ,[UploadedByUser]
      ,[UploadTime]
      ,[UploadSourceDescription]
      ,[UploadTypeDescription]
      ,[FileTypeDescription]
      ,[FileName]
      ,[Processed]
      ,[FailedProcessing]
      ,[ArchiveFileInfoId]
      ,[ResourceId]
      ,[EntityStatusId]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[ModifiedBy]
      ,[ModifiedDate]
	FROM [dbo].[UploadedFileInfo] 
      WHERE [UploadedFileInfoId] = @UploadedFileInfoId;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <4/21/2014>
-- Description:	<This stored procedure is meant to return all ACA users for a specific District.>
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_employer_users](
	@employerID int
	)
AS
BEGIN
	SELECT [user_id]
      ,[fname]
      ,[lname]
      ,[email]
      ,[phone]
      ,[username]
      ,[password]
      ,[employer_id]
      ,[active]
      ,[poweruser]
      ,[last_mod_by]
      ,[last_mod]
      ,[reset_pwd]
      ,[billing]
      ,[irsContact]
      ,[floater]
      ,[ResourceId]
	FROM [user]
	WHERE [employer_id]=@employerID
	AND [active] = 1;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <5/13/2014>
-- Description:	<This stored procedure is meant to return all ACA users for a specific District.>
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_employer_plan_years](
	@employerID int
	)
AS
BEGIN
	SELECT [plan_year_id]
      ,[employer_id]
      ,[description]
      ,[startDate]
      ,[endDate]
      ,[notes]
      ,[history]
      ,[modOn]
      ,[modBy]
      ,[ResourceId]
      ,[default_meas_start]
      ,[default_meas_end]
      ,[default_admin_start]
      ,[default_admin_end]
      ,[default_open_start]
      ,[default_open_end]
      ,[default_stability_start]
      ,[default_stability_end]
      ,[PlanYearGroupId]
	FROM [plan_year]
	WHERE [employer_id]=@employerID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ryan McCully 
-- Create date: 10/10/2016
-- Description:	Select all payroll for an employer based on <Travis Wells> [SELECT_employee_payroll]
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_employer_payroll]
	@employerId int
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
	FROM [View_payroll]
	WHERE [employer_id]=@employerId 
	ORDER BY [employee_id], [edate];
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <8/18/2014>
-- Description:	<This stored procedure is meant to return all measurement periods for a specific employer.>
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_employer_measurements](
	@employerID int
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
	FROM [measurement]
	WHERE [employer_id]=@employerID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/6/2016>
-- Description:	<This stored procedure is meant to return a single Tax Year Approval Form.>
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_employer_irs_submission](
	@employerID int,
	@taxYear int
	)
AS
BEGIN
	SELECT [approval_id]
      ,[employer_id]
      ,[tax_year]
      ,[dge]
      ,[dge_name]
      ,[dge_ein]
      ,[dge_address]
      ,[dge_city]
      ,[state_id]
      ,[dge_zip]
      ,[dge_contact_fname]
      ,[dge_contact_lname]
      ,[dge_phone]
      ,[ale]
      ,[tr_q1]
      ,[tr_q2]
      ,[tr_q3]
      ,[tr_q4]
      ,[tr_q5]
      ,[tr_qualified]
      ,[tobacco]
      ,[unpaidLeave]
      ,[safeHarbor]
      ,[completed_by]
      ,[completed_on]
      ,[ebc_approval]
      ,[ebc_approved_by]
      ,[ebc_approved_on]
      ,[allow_editing]
      ,[ResourceId]
	FROM [tax_year_approval]
	WHERE 
		[tax_year]=@taxYear AND
		[employer_id] = @employerID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <2/9/2015>
-- Description:	<This stored procedure is meant to return all invoices for a specific employer.>
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_employer_invoices](
	@employerID int
	)
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
	FROM [invoice]
	WHERE [employer_id]=@employerID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <2/23/2016>
-- Description:	<This stored procedure is meant to return all insurance coverage rows that have not been processed.>
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_employer_insurance_coverage_import_alerts](
	@employerID int
	)
AS
BEGIN
	SELECT [row_id]
      ,[batch_id]
      ,[employer_id]
      ,[tax_year]
      ,[employee_id]
      ,[dependent_link]
      ,[dependent_id]
      ,[fName]
      ,[lName]
      ,[mName]
      ,[ssn]
      ,[dob]
      ,[jan]
      ,[feb]
      ,[march]
      ,[april]
      ,[may]
      ,[june]
      ,[july]
      ,[august]
      ,[september]
      ,[october]
      ,[november]
      ,[december]
      ,[subscriber]
      ,[all12]
      ,[state_id]
      ,[zip]
      ,[jan_i]
      ,[feb_i]
      ,[march_i]
      ,[april_i]
      ,[may_i]
      ,[june_i]
      ,[july_i]
      ,[aug_i]
      ,[sep_i]
      ,[oct_i]
      ,[nov_i]
      ,[dec_i]
      ,[all12_i]
      ,[subscriber_i]
      ,[address_i]
      ,[city_i]
      ,[state_i]
      ,[zip_i]
      ,[carrier_id]
      ,[ResourceId] 
	FROM [import_insurance_coverage]
	WHERE [employer_id]=@employerID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <12/31/2014>
-- Description:	<This stored procedure is meant to return all insurance alerts.>
-- Modifications:
--		3-8-2016 TLW
--			Added HRA/Flex Contributions to Alerts. 
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_employer_insurance_alerts](
	@employerID int
	)
AS
BEGIN
	SELECT [rowid]
      ,[employee_id]
      ,[plan_year_id]
      ,[employer_id]
      ,[avg_hours_month]
      ,[offered]
      ,[accepted]
      ,[acceptedOn]
      ,[modOn]
      ,[modBy]
      ,[notes]
      ,[history]
      ,[ext_emp_id]
      ,[fName]
      ,[lName]
      ,[effectiveDate]
      ,[offeredOn]
      ,[ins_cont_id]
      ,[insurance_id]
      ,[HR_status_id]
      ,[limbo_plan_year_id]
      ,[limbo_plan_year_avg_hours]
      ,[imp_plan_year_avg_hours]
      ,[classification_id]
      ,[hra_flex_contribution]
	FROM [View_insurance_alert_details]
	WHERE [employer_id]=@employerID and 
	(([accepted] is null and
	[offered] is null and
	[acceptedOn] is null) OR 
	([offered] = 1 AND [hra_flex_contribution] IS NULL));
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <7/22/2014>
-- Description:	<This stored procedure is meant to return all HR status for a specific employer.>
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_employer_hr_status](
	@employerID int
	)
AS
BEGIN
	SELECT [HR_status_id]
      ,[employer_id]
      ,[ext_id]
      ,[name]
      ,[active]
      ,[ResourceId]
	FROM [hr_status]
	WHERE [employer_id]=@employerID
	AND [active] = 1;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <4/21/2014>
-- Description:	<This stored procedure is meant to return all ACA users for a specific District.>
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_employer_gross_pay_types](
	@employerID int
	)
AS
BEGIN
	SELECT [gross_pay_id]
      ,[employer_id]
      ,[external_id]
      ,[description]
      ,[active]
      ,[ResourceId]
	FROM [gross_pay_type]
	WHERE [employer_id]=@employerID
	AND [active] = 1
	ORDER BY [description];
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <1/22/2015>
-- Description:	<This stored procedure is meant to return all Gross Pay Filters for a specific district.>
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_employer_gross_pay_filters](
	@employerID int
	)
AS
BEGIN
	SELECT [gp_filter_id]
      ,[employer_id]
      ,[gross_pay_id]
      ,[ResourceId]
	FROM [gross_pay_filter]
	WHERE [employer_id]=@employerID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/12/2014>
-- Description:	<This stored procedure is meant to return all equivalencies for a specific District.>
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_employer_equivalencies](
	@employerID int
	)
AS
BEGIN
	SELECT [equivalency_id]
      ,[employer_id]
      ,[name]
      ,[gpID]
      ,[every]
      ,[unit_id]
      ,[credit]
      ,[start_date]
      ,[end_date]
      ,[notes]
      ,[modBy]
      ,[modOn]
      ,[history]
      ,[active]
      ,[equivalency_type_id]
      ,[equivalency_type_name]
      ,[unit_name]
      ,[position_id]
      ,[activity_id]
      ,[detail_id]
	FROM [View_employer_equivalency]
	WHERE [employer_id]=@employerID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/24/2016>
-- Description:	<This stored procedure is meant to return all employees who haven't had their 1095c approved for a specific tax year.>
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_employer_employees_Tax_Year_Approved](
	@employerID int,
	@taxYear int
	)
AS
BEGIN
	SELECT [employee_id]
      ,[employee_type_id]
      ,[HR_status_id]
      ,[employer_id]
      ,[fName]
      ,[mName]
      ,[lName]
      ,[address]
      ,[city]
      ,[state_id]
      ,[zip]
      ,[hireDate]
      ,[currDate]
      ,[ssn]
      ,[ext_emp_id]
      ,[terminationDate]
      ,[dob]
      ,[initialMeasurmentEnd]
      ,[plan_year_id]
      ,[limbo_plan_year_id]
      ,[meas_plan_year_id]
      ,[modOn]
      ,[modBy]
      ,[plan_year_avg_hours]
      ,[limbo_plan_year_avg_hours]
      ,[meas_plan_year_avg_hours]
      ,[imp_plan_year_avg_hours]
      ,[classification_id]
      ,[aca_status_id]
      ,[ResourceId]
	FROM [employee]
	WHERE [employer_id]=@employerID AND [employee_id] NOT IN (Select [employee_id] FROM [tax_year_1095c_approval] WHERE [tax_year]=@taxYear);
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <7/30/2014>
-- Description:	<This stored procedure is meant to return all HR status for a specific employer.>
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_employer_employees](
	@employerID int
	)
AS
BEGIN
	SELECT [employee_id]
      ,[employee_type_id]
      ,[HR_status_id]
      ,[employer_id]
      ,[fName]
      ,[mName]
      ,[lName]
      ,[address]
      ,[city]
      ,[state_id]
      ,[zip]
      ,[hireDate]
      ,[currDate]
      ,[ssn]
      ,[ext_emp_id]
      ,[terminationDate]
      ,[dob]
      ,[initialMeasurmentEnd]
      ,[plan_year_id]
      ,[limbo_plan_year_id]
      ,[meas_plan_year_id]
      ,[modOn]
      ,[modBy]
      ,[plan_year_avg_hours]
      ,[limbo_plan_year_avg_hours]
      ,[meas_plan_year_avg_hours]
      ,[imp_plan_year_avg_hours]
      ,[classification_id]
      ,[aca_status_id]
      ,[ResourceId]
	FROM [employee]
	WHERE [employer_id]=@employerID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <7/30/2014>
-- Description:	<This stored procedure is meant to return all HR status for a specific employer.>
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_employer_employee_export](
	@employerID int
	)
AS
BEGIN
	SELECT [employee_id]
      ,[employee_type_id]
      ,[HR_status_id]
      ,[employer_id]
      ,[fName]
      ,[mName]
      ,[lName]
      ,[address]
      ,[city]
      ,[state_id]
      ,[zip]
      ,[hireDate]
      ,[currDate]
      ,[ssn]
      ,[ext_emp_id]
      ,[terminationDate]
      ,[dob]
      ,[initialMeasurmentEnd]
      ,[plan_year_id]
      ,[limbo_plan_year_id]
      ,[meas_plan_year_id]
      ,[modOn]
      ,[modBy]
      ,[plan_year_avg_hours]
      ,[limbo_plan_year_avg_hours]
      ,[meas_plan_year_avg_hours]
      ,[imp_plan_year_avg_hours]
      ,[classification_id]
      ,[aca_status_id]
      ,[hrStatus]
      ,[acaStatus]
      ,[className]
	FROM [View_Employee_Export]
	WHERE [employer_id]=@employerID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/1/2015>
-- Description:	<This stored procedure is meant to return all EMPLOYEE CLASSIFICATIONS for a specific district.>
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_employer_classifications](
	@employerID int
	)
AS
BEGIN
	SELECT [classification_id]
      ,[employer_id]
      ,[description]
      ,[modOn]
      ,[modBy]
      ,[history]
      ,[ash_code]
      ,[ResourceId]
	FROM [employee_classification]
	WHERE [employer_id]=@employerID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <1/5/2014>
-- Description:	<This stored procedure is meant to return all employers who are ready for billing.>
-- Changes:
--			
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_employer_billing]
AS

BEGIN
	SELECT [employer_id]
      ,[name]
      ,[address]
      ,[city]
      ,[state_id]
      ,[zip]
      ,[img_logo]
      ,[bill_address]
      ,[bill_city]
      ,[bill_state]
      ,[bill_zip]
      ,[employer_type_id]
      ,[ein]
      ,[initial_measurement_id]
      ,[import_demo]
      ,[import_payroll]
      ,[iei]
      ,[iec]
      ,[ftpei]
      ,[ftpec]
      ,[ipi]
      ,[ipc]
      ,[ftppi]
      ,[ftppc]
      ,[importProcess]
      ,[vendor_id]
      ,[autoUpload]
      ,[autoBill]
      ,[suBilled]
      ,[import_gp]
      ,[import_hr]
      ,[import_ec]
      ,[import_io]
      ,[import_ic]
      ,[import_pay_mod]
      ,[ResourceId]
      ,[DBAName]
	FROM [employer]
	WHERE [autoBill]=1;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <2/2/2015>
-- Description:	<This stored procedure is meant to return all batch ID's for a specific employer.>
-- Modifications:
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_employer_batch_top25](
	@employerID int
	)
AS
BEGIN
	SELECT TOP 100 [batch_id]
      ,[employer_id]
      ,[modOn]
      ,[modBy]
      ,[delOn]
      ,[delBy]
      ,[ResourceId] 
	FROM [batch] b
	WHERE b.[employer_id]=@employerID AND b.[batch_id] in(Select [batch_id] from [payroll] where [payroll].[employer_id] = @employerID)
	ORDER BY [modOn] DESC;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <2/11/2015>
-- Description:	<This stored procedure is meant to return all employers who are ready for billing.>
-- Changes:
--			
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_employer_autoupload]
AS

BEGIN
	SELECT [employer_id]
      ,[name]
      ,[address]
      ,[city]
      ,[state_id]
      ,[zip]
      ,[img_logo]
      ,[bill_address]
      ,[bill_city]
      ,[bill_state]
      ,[bill_zip]
      ,[employer_type_id]
      ,[ein]
      ,[initial_measurement_id]
      ,[import_demo]
      ,[import_payroll]
      ,[iei]
      ,[iec]
      ,[ftpei]
      ,[ftpec]
      ,[ipi]
      ,[ipc]
      ,[ftppi]
      ,[ftppc]
      ,[importProcess]
      ,[vendor_id]
      ,[autoUpload]
      ,[autoBill]
      ,[suBilled]
      ,[import_gp]
      ,[import_hr]
      ,[import_ec]
      ,[import_io]
      ,[import_ic]
      ,[import_pay_mod]
      ,[ResourceId]
      ,[DBAName]
	FROM [employer]
	WHERE [autoUpload] = 1;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <8/19/2014>
-- Description:	<This stored procedure is meant to return all ALERTS for a specific district.>
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_employer_alerts](
	@employerID int
	)
AS
BEGIN
	SELECT [alert_id]
      ,[name]
      ,[alert_type_id]
      ,[employer_id]
      ,[table_name]
      ,[alertCount]
      ,[alerttypename]
      ,[image_url]
	FROM [View_alerts]
	WHERE [employer_id]=@employerID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <4/22/2014>
-- Description:	<This stored procedure is meant to return a single district.>
-- Changes:
--			
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_employer]
	@employerID varchar(100)
AS

BEGIN
	SELECT [employer_id]
      ,[name]
      ,[address]
      ,[city]
      ,[state_id]
      ,[zip]
      ,[img_logo]
      ,[bill_address]
      ,[bill_city]
      ,[bill_state]
      ,[bill_zip]
      ,[employer_type_id]
      ,[ein]
      ,[initial_measurement_id]
      ,[import_demo]
      ,[import_payroll]
      ,[iei]
      ,[iec]
      ,[ftpei]
      ,[ftpec]
      ,[ipi]
      ,[ipc]
      ,[ftppi]
      ,[ftppc]
      ,[importProcess]
      ,[vendor_id]
      ,[autoUpload]
      ,[autoBill]
      ,[suBilled]
      ,[import_gp]
      ,[import_hr]
      ,[import_ec]
      ,[import_io]
      ,[import_ic]
      ,[import_pay_mod]
      ,[ResourceId]
      ,[DBAName]
	FROM [employer]
	WHERE [employer_id] = @employerID;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan McCully
-- Create date: 10/6/2016
-- Description: Select all Rows of AverageHours for a Measurement Period
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_EmployeeMeasurementAverageHours_ForMeasurement]
      @measurementId int
AS
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT [EmployeeMeasurementAverageHoursId]
      ,[EmployeeId]
      ,[MeasurementId]
      ,[WeeklyAverageHours]
      ,[MonthlyAverageHours]
      ,[ResourceId]
      ,[EntityStatusId]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[ModifiedBy]
      ,[ModifiedDate]
      ,[IsNewHire]
      ,[TrendingWeeklyAverageHours]
      ,[TrendingMonthlyAverageHours]
      ,[TotalHours]
	FROM [dbo].[EmployeeMeasurementAverageHours] 
      WHERE [MeasurementId] = @measurementId AND [EntityStatusId] = 1;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan McCully
-- Create date: 10/6/2016
-- Description: Select all Rows of AverageHours for an Employee
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_EmployeeMeasurementAverageHours_ForEmployee]
      @employeeId int
AS
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT [EmployeeMeasurementAverageHoursId]
      ,[EmployeeId]
      ,[MeasurementId]
      ,[WeeklyAverageHours]
      ,[MonthlyAverageHours]
      ,[ResourceId]
      ,[EntityStatusId]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[ModifiedBy]
      ,[ModifiedDate]
      ,[IsNewHire]
      ,[TrendingWeeklyAverageHours]
      ,[TrendingMonthlyAverageHours]
      ,[TotalHours]
	FROM [dbo].[EmployeeMeasurementAverageHours] 
      WHERE [EmployeeId] = @employeeId AND [EntityStatusId] = 1;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan McCully
-- Create date: 10/6/2016
-- Description: Select a Single Row of 
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_EmployeeMeasurementAverageHours_ByEmployeeMeasurement]
      @employeeId int,
	  @measurementId int
AS
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT [EmployeeMeasurementAverageHoursId]
      ,[EmployeeId]
      ,[MeasurementId]
      ,[WeeklyAverageHours]
      ,[MonthlyAverageHours]
      ,[ResourceId]
      ,[EntityStatusId]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[ModifiedBy]
      ,[ModifiedDate]
      ,[IsNewHire]
      ,[TrendingWeeklyAverageHours]
      ,[TrendingMonthlyAverageHours]
      ,[TotalHours]
	FROM [dbo].[EmployeeMeasurementAverageHours] 
      WHERE [EmployeeId] = @employeeId 
		AND [MeasurementId] = @measurementId
	    AND [EntityStatusId] = 1;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan McCully
-- Create date: 10/6/2016
-- Description: Select a Single Row of 
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_EmployeeMeasurementAverageHours]
      @Id int
AS
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT [EmployeeMeasurementAverageHoursId]
      ,[EmployeeId]
      ,[MeasurementId]
      ,[WeeklyAverageHours]
      ,[MonthlyAverageHours]
      ,[ResourceId]
      ,[EntityStatusId]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[ModifiedBy]
      ,[ModifiedDate]
      ,[IsNewHire]
      ,[TrendingWeeklyAverageHours]
      ,[TrendingMonthlyAverageHours]
      ,[TotalHours]
	FROM [dbo].[EmployeeMeasurementAverageHours] 
      WHERE [EmployeeMeasurementAverageHoursId] = @Id;
END

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan McCully
-- Create date: 9/23/2016
-- Description: Select a Single Row of 
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_ArchiveFileInfo_ForEmployer]
      @EmployerId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT [ArchiveFileInfoId]
      ,[EmployerId]
      ,[EmployerGuid]
      ,[ArchivedTime]
      ,[FileName]
      ,[SourceFilePath]
      ,[ArchiveFilePath]
      ,[ArchiveReason]
      ,[ResourceId]
      ,[EntityStatusId]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[ModifiedBy]
      ,[ModifiedDate] 
	FROM [dbo].[ArchiveFileInfo] 
      WHERE [EmployerId] = @EmployerId;
END TRY
BEGIN CATCH
	EXEC dbo.INSERT_ErrorLogging
--Insert
END CATCH

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan McCully
-- Create date: 9/23/2016
-- Description: Select a Single Row of 
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_ArchiveFileInfo]
      @ArchiveFileInfoId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT [ArchiveFileInfoId]
      ,[EmployerId]
      ,[EmployerGuid]
      ,[ArchivedTime]
      ,[FileName]
      ,[SourceFilePath]
      ,[ArchiveFilePath]
      ,[ArchiveReason]
      ,[ResourceId]
      ,[EntityStatusId]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[ModifiedBy]
      ,[ModifiedDate]
	FROM [dbo].[ArchiveFileInfo] 
      WHERE [ArchiveFileInfoId] = @ArchiveFileInfoId;
END TRY
BEGIN CATCH
	exec dbo.INSERT_ErrorLogging
END CATCH
--Select for employer

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan McCully
-- Create date: 10/11/2016
-- Description: Select All Active Imports that are tied to this file
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_ActiveStagingImport_ForUpload]
	@uploadedFileInfoId int
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT [StagingImportId]
      ,[Original]
      ,[UploadedFileInfoId]
      ,[Modify]
      ,[ResourceId]
      ,[EntityStatusId]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[ModifiedBy]
      ,[ModifiedDate]
	FROM [dbo].[StagingImport] 
      WHERE [EntityStatusId] = 1 AND [UploadedFileInfoId] = @uploadedFileInfoId;
END TRy
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ryan McCully
-- Create date: 9/19/2016
-- Description: Select All Active Imports 
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_ActiveStagingImport]

AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    SELECT [StagingImportId]
      ,[Original]
      ,[UploadedFileInfoId]
      ,[Modify]
      ,[ResourceId]
      ,[EntityStatusId]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[ModifiedBy]
      ,[ModifiedDate]
	FROM [dbo].[StagingImport] 
      WHERE [EntityStatusId] = 1;
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH

GO
