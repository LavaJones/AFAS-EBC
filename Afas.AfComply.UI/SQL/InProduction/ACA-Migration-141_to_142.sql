USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/19/2017>
-- Description:	<This stored procedure is meant to return all rows of imported coverage data for all dependents of a specific employee regardless of tax year.>
--         - This is being used on the employee merge screen. 
--			
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employee_dependent_coverage_all_tax_years]
	@employeeID int
AS

BEGIN
	SELECT *
	FROM [View_all_insurance_coverage]
	WHERE employee_id = @employeeID AND dependent_id IS NOT NULL;
END

GRANT EXECUTE ON [dbo].[SELECT_employee_dependent_coverage_all_tax_years] TO [aca-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/19/2017>
-- Description:	<This stored procedure is meant to delete a single insurance carrier data row, matching the row id.>
-- =============================================
CREATE PROCEDURE [dbo].[DELETE_insurance_coverage_row]
	@rowID int,
	@employeeID int
AS
BEGIN

DELETE aca.dbo.insurance_coverage
WHERE 
	row_id=@rowID AND
	employee_id=@employeeID;

END

GRANT EXECUTE ON [dbo].[DELETE_insurance_coverage_row] TO [aca-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/16/2017>
-- Description:	<This will delete Insurance Offers & Insurance Offer change events for a specified employee.>
--     This is used when we are merging employees togethor because they were duplicated. 
-- =============================================
CREATE PROCEDURE [dbo].[DELETE_insurance_offer_single]
	@rowID int,
	@employeeID int,
	@employerID int
AS
BEGIN

	--DELETE all Insurance Change Events if they exist.
	DELETE aca.dbo.employee_insurance_offer_archive
	WHERE
		rowid=@rowID AND
		employer_id=@employerID AND
		employee_id=@employeeID;

	--DELETE all Insurance Offers if they exist.
	DELETE aca.dbo.employee_insurance_offer
	WHERE
		rowid=@rowID AND
		employer_id=@employerID AND
		employee_id=@employeeID;

END

GRANT EXECUTE ON [dbo].[DELETE_insurance_offer_single] TO [aca-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/20/2017>
-- Description:	<This will update the employeeID on a single dependent record.>
--     This is used when we are merging employees togethor because they were duplicated. 
--
-- =============================================
CREATE PROCEDURE [dbo].[MIGRATE_dependent_single]
	@dependentID int,
	@employeeIDold int,
	@employeeIDnew int
AS
BEGIN
	UPDATE aca.dbo.employee_dependents
	SET
		employee_id=@employeeIDnew
	WHERE
		dependent_id=@dependentID AND
		employee_id=@employeeIDold;
END

GO

GRANT EXECUTE ON [dbo].[MIGRATE_dependent_single] TO [aca-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/19/2017>
-- Description:	<This stored procedure is meant to delete a single insurance carrier data row, matching the row id.>
-- =============================================
CREATE PROCEDURE [dbo].[MIGRATE_insurance_coverage_row]
	@rowID int,
	@employeeIDold int,
	@employeeIDnew int,
	@history varchar(max)
AS
BEGIN

UPDATE aca.dbo.insurance_coverage
SET
	employee_id=@employeeIDnew,
	history=@history
WHERE 
	row_id=@rowID AND
	employee_id=@employeeIDold;

END

GRANT EXECUTE ON [dbo].[MIGRATE_insurance_coverage_row] TO [aca-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/20/2017>
-- Description:	<This will update a single insurance coverage row for a specific employee dependent.>
--     This is used when we are merging employees togethor because they were duplicated. 
-- =============================================
CREATE PROCEDURE [dbo].[MIGRATE_insurance_coverage_row_dependent]
	@rowID int,
	@employeeIDold int,
	@employeeIDnew int,
	@dependentIDold int,
	@dependentIDnew int,
	@history varchar(max)
AS
BEGIN
	UPDATE aca.dbo.insurance_coverage
	SET
		employee_id=@employeeIDnew,
		dependent_id=@dependentIDnew,
		history=@history
	WHERE
		row_id=@rowID AND
		employee_id=@employeeIDold AND
		dependent_id=@dependentIDold;
END

GO

GRANT EXECUTE ON [dbo].[MIGRATE_insurance_coverage_row_dependent] TO [aca-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/16/2017>
-- Description:	<This will update the employeeID on Insurance Offers & Insurance Offer change events.>
--     This is used when we are merging employees togethor because they were duplicated. 
-- =============================================
CREATE PROCEDURE [dbo].[MIGRATE_insurance_offers]
	@rowID int,
	@employeeIDnew int,
	@employeeIDold int,
	@employerID int,
	@modBy varchar(50),
	@modOn datetime,
	@history varchar(max)
AS
BEGIN
	UPDATE aca.dbo.employee_insurance_offer
	SET
		employee_id=@employeeIDnew,
		modOn=@modOn,
		modBy=@modBy,
		history=@history
	WHERE
		rowid=@rowID AND
		employer_id=@employerID AND
		employee_id=@employeeIDold;
	/*
		Only update the insurance offer change events if the parent record was able to be moved.
	*/
	IF @@ROWCOUNT> 0
		BEGIN
			UPDATE aca.dbo.employee_insurance_offer_archive
			SET
				employee_id=@employeeIDnew,
				modOn=@modOn,
				modBy=@modBy,
				history=@history
			WHERE
				rowid=@rowID AND
				employer_id=@employerID AND
				employee_id=@employeeIDold;
		END
END

GO

GRANT EXECUTE ON [dbo].[MIGRATE_insurance_offers] TO [aca-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/16/2017>
-- Description:	<This will update the employeeID on a single payroll record.>
--     This is used when we are merging employees togethor because they were duplicated. 
-- =============================================
CREATE PROCEDURE [dbo].[MIGRATE_payroll_single]
	@rowID int,
	@employerID int,
	@employeeID int,
	@modBy varchar(50),
	@modOn datetime,
	@history varchar(max)
AS
BEGIN
	UPDATE aca.dbo.payroll
	SET
		employee_id=@employeeID,
		modOn=@modOn,
		modBy=@modBy,
		history=@history
	WHERE
		row_id=@rowID AND
		employer_id=@employerID;
END

GO

GRANT EXECUTE ON [dbo].[MIGRATE_payroll_single] TO [aca-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/19/2017>
-- Description:	<This stored procedure is meant to return all rows of imported coverage data for a specific employee regardless of tax year.>
--         - This is being used on the employee merge screen. 
--			
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employee_individual_coverage_all_tax_years]
	@employeeID int
AS

BEGIN
	SELECT *
	FROM [View_all_insurance_coverage]
	WHERE employee_id = @employeeID AND dependent_id IS NULL;
END

GRANT EXECUTE ON [dbo].[SELECT_employee_individual_coverage_all_tax_years] TO [aca-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/15/2017>
-- Description:	<This stored procedure is meant to return all insurance offers for a single employee.>
-- Reason Added:
--     A new screen was developed to merge employees and we needed a way to pull all of the insurance offers on a per employee basis. 
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employee_insurance_offer_all](
	@employeeID int
	)
AS
BEGIN
	SELECT *
	FROM [aca].[dbo].[View_insurance_alert_details]
	WHERE 
		employee_id=@employeeID;
    
END

GO

GRANT EXECUTE ON [dbo].[SELECT_employee_insurance_offer_all] TO [aca-user] AS [dbo]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[View_insurance_alert_details_change_events]
AS
SELECT dbo.employee_insurance_offer_archive.rowid, dbo.employee_insurance_offer_archive.employee_id, dbo.employee_insurance_offer_archive.plan_year_id, 
                  dbo.employee_insurance_offer_archive.employer_id, dbo.employee_insurance_offer_archive.avg_hours_month, dbo.employee_insurance_offer_archive.offered, 
                  dbo.employee_insurance_offer_archive.accepted, dbo.employee_insurance_offer_archive.acceptedOn, dbo.employee_insurance_offer_archive.modOn, 
                  dbo.employee_insurance_offer_archive.modBy, dbo.employee_insurance_offer_archive.notes, dbo.employee_insurance_offer_archive.history, dbo.employee.ext_emp_id, 
                  dbo.employee.fName, dbo.employee.lName, dbo.employee_insurance_offer_archive.effectiveDate, dbo.employee_insurance_offer_archive.offeredOn, 
                  dbo.employee_insurance_offer_archive.ins_cont_id, dbo.employee_insurance_offer_archive.insurance_id, dbo.employee.HR_status_id, dbo.employee.limbo_plan_year_id, 
                  dbo.employee.limbo_plan_year_avg_hours, dbo.employee.imp_plan_year_avg_hours, dbo.employee.classification_id, 
                  dbo.employee_insurance_offer_archive.hra_flex_contribution
FROM     dbo.employee RIGHT OUTER JOIN
                  dbo.employee_insurance_offer_archive ON dbo.employee.employee_id = dbo.employee_insurance_offer_archive.employee_id

GO

GRANT SELECT ON [dbo].[View_insurance_alert_details_change_events] TO [aca-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/15/2017>
-- Description:	<This stored procedure is meant to return all insurance offer change events for a single employee.>
-- Reason Added:
--		1) A new screen was developed to merge employees and we needed a way to pull all of the insurance offer change events on a per employee basis. 
--		2) Clients will be able to utilize this SP to pull a running history of the change events. 
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employee_insurance_offer_change_events_all](
	@employeeID int
	)
AS
BEGIN
	SELECT *
	FROM [aca].[dbo].view_insurance_alert_details_change_events
	WHERE 
		employee_id=@employeeID;
    
END

GO

GRANT EXECUTE ON [dbo].[SELECT_employee_insurance_offer_change_events_all] TO [aca-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[View_dependent_insurance_coverage]
AS
SELECT dbo.employee_dependents.dependent_id, dbo.employee_dependents.employee_id, dbo.employee_dependents.fName, dbo.employee_dependents.mName, 
                  dbo.employee_dependents.lName, dbo.employee_dependents.ssn, dbo.employee_dependents.dob, dbo.insurance_coverage.row_id, dbo.insurance_coverage.tax_year, 
                  dbo.insurance_coverage.carrier_id, dbo.insurance_coverage.all12, dbo.insurance_coverage.jan, dbo.insurance_coverage.feb, dbo.insurance_coverage.mar, 
                  dbo.insurance_coverage.apr, dbo.insurance_coverage.may, dbo.insurance_coverage.jun, dbo.insurance_coverage.jul, dbo.insurance_coverage.aug, 
                  dbo.insurance_coverage.sep, dbo.insurance_coverage.oct, dbo.insurance_coverage.nov, dbo.insurance_coverage.dec, dbo.insurance_coverage.history
FROM     dbo.employee_dependents RIGHT OUTER JOIN
                  dbo.insurance_coverage ON dbo.employee_dependents.dependent_id = dbo.insurance_coverage.dependent_id

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[View_employee_insurance_coverage]
AS
SELECT dbo.insurance_coverage.dependent_id, dbo.insurance_coverage.employee_id, dbo.employee.fName, dbo.employee.mName, dbo.employee.lName, dbo.employee.ssn, 
                  dbo.employee.dob, dbo.insurance_coverage.row_id, dbo.insurance_coverage.tax_year, dbo.insurance_coverage.carrier_id, dbo.insurance_coverage.all12, 
                  dbo.insurance_coverage.jan, dbo.insurance_coverage.feb, dbo.insurance_coverage.mar, dbo.insurance_coverage.apr, dbo.insurance_coverage.may, 
                  dbo.insurance_coverage.jun, dbo.insurance_coverage.jul, dbo.insurance_coverage.aug, dbo.insurance_coverage.sep, dbo.insurance_coverage.oct, 
                  dbo.insurance_coverage.nov, dbo.insurance_coverage.dec, dbo.insurance_coverage.history
FROM     dbo.insurance_coverage LEFT OUTER JOIN
                  dbo.employee ON dbo.insurance_coverage.employee_id = dbo.employee.employee_id
WHERE  (dbo.insurance_coverage.dependent_id IS NULL)

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER VIEW [dbo].[View_all_insurance_coverage]
AS
SELECT *
  FROM [aca].[dbo].[View_employee_insurance_coverage]
UNION 
Select * 
	FROM [aca].[dbo].[View_dependent_insurance_coverage]         

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[View_dependent_insurance_coverage_editable]
AS
SELECT dbo.insurance_coverage_editable.row_id, dbo.insurance_coverage_editable.employee_id, dbo.insurance_coverage_editable.employer_id, 
                  dbo.insurance_coverage_editable.dependent_id, dbo.insurance_coverage_editable.tax_year, dbo.insurance_coverage_editable.Jan, dbo.insurance_coverage_editable.Feb, 
                  dbo.insurance_coverage_editable.Mar, dbo.insurance_coverage_editable.Apr, dbo.insurance_coverage_editable.May, dbo.insurance_coverage_editable.Jun, 
                  dbo.insurance_coverage_editable.Jul, dbo.insurance_coverage_editable.Aug, dbo.insurance_coverage_editable.Sept, dbo.insurance_coverage_editable.Oct, 
                  dbo.insurance_coverage_editable.Nov, dbo.insurance_coverage_editable.Dec, dbo.employee_dependents.fName, dbo.employee_dependents.mName, 
                  dbo.employee_dependents.lName, dbo.employee_dependents.ssn, dbo.employee_dependents.dob
FROM     dbo.insurance_coverage_editable LEFT OUTER JOIN
                  dbo.employee_dependents ON dbo.insurance_coverage_editable.dependent_id = dbo.employee_dependents.dependent_id
WHERE  (dbo.insurance_coverage_editable.dependent_id IS NOT NULL)

GO

GRANT SELECT ON [dbo].[View_dependent_insurance_coverage_editable] TO [aca-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[View_employee_insurance_coverage_editable]
AS
SELECT dbo.insurance_coverage_editable.row_id, dbo.insurance_coverage_editable.employee_id, dbo.insurance_coverage_editable.employer_id, 
                  dbo.insurance_coverage_editable.dependent_id, dbo.insurance_coverage_editable.tax_year, dbo.insurance_coverage_editable.Jan, dbo.insurance_coverage_editable.Feb, 
                  dbo.insurance_coverage_editable.Mar, dbo.insurance_coverage_editable.Apr, dbo.insurance_coverage_editable.May, dbo.insurance_coverage_editable.Jun, 
                  dbo.insurance_coverage_editable.Jul, dbo.insurance_coverage_editable.Aug, dbo.insurance_coverage_editable.Sept, dbo.insurance_coverage_editable.Oct, 
                  dbo.insurance_coverage_editable.Nov, dbo.insurance_coverage_editable.Dec, dbo.employee.fName, dbo.employee.mName, dbo.employee.lName, dbo.employee.ssn, 
                  dbo.employee.dob
FROM     dbo.insurance_coverage_editable LEFT OUTER JOIN
                  dbo.employee ON dbo.insurance_coverage_editable.employee_id = dbo.employee.employee_id
WHERE  (dbo.insurance_coverage_editable.dependent_id IS NULL)

GO

GRANT SELECT ON [dbo].[View_employee_insurance_coverage_editable] TO [aca-user] AS [dbo]
GO

CREATE VIEW [dbo].[View_all_insurance_coverage_editable]
AS
SELECT *
FROM     [aca].[dbo].[View_employee_insurance_coverage_editable]
UNION
SELECT *
FROM     [aca].[dbo].[View_dependent_insurance_coverage_editable]

GO

GRANT SELECT ON [dbo].[View_all_insurance_coverage_editable] TO [aca-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/20/2017>
-- Description:	<This stored procedure is meant to return all editable rows for a specific individuals.>
-- Changes:
--			This is used for the Employee Merge Screen.
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employee_editable_individual_coverage_with_names]
	@employeeID int
AS
BEGIN
	SELECT	*
	FROM
		aca.dbo.View_all_insurance_coverage_editable
	WHERE
		employee_id = @employeeID;

END

GO

GRANT EXECUTE ON [dbo].[SELECT_employee_editable_individual_coverage_with_names] TO [aca-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/20/2017>
-- Description:	<This stored procedure will DELETE an Employee from the ACA database. All information is deleted.>
-- =============================================
CREATE PROCEDURE [dbo].[REMOVE_employee_from_aca]
	@employeeID int
AS
BEGIN

/**********************************************
DELETE all import tables.
**********************************************/
DELETE aca.dbo.import_payroll
WHERE employee_id=@employeeID;

DELETE aca.dbo.import_insurance_coverage
WHERE employee_id=@employeeID;


/**********************************************
DELETE all archive tables.
**********************************************/
DELETE aca.dbo.payroll_archive
WHERE employee_id=@employeeID;

DELETE aca.dbo.import_insurance_coverage_archive
WHERE employee_id=@employeeID;

DELETE aca.dbo.employee_insurance_offer_archive
WHERE employee_id=@employeeID;

/***********************************************
DELETE all payroll records.
***********************************************/
DELETE aca.dbo.payroll
WHERE employee_id=@employeeID;

/*********************************************
DELETE all insurance records
*********************************************/
DELETE aca.dbo.insurance_coverage_editable
WHERE employee_id=@employeeID;

DELETE aca.dbo.insurance_coverage
WHERE employee_id=@employeeID;

DELETE aca.dbo.employee_insurance_offer
WHERE employee_id=@employeeID;

/**********************************************
DELETE all dependents
**********************************************/
DELETE aca.dbo.employee_dependents
WHERE employee_id=@employeeID;

/*********************************************
DELETE Average Hours
*********************************************/
DELETE aca.dbo.EmployeeMeasurementAverageHours
WHERE employeeid=@employeeID;

/*********************************************
DELETE 1095c Information
*********************************************/
DELETE aca.dbo.tax_year_1095c_approval
WHERE employee_id=@employeeID;

/*********************************************
DELETE the employee
**********************************************/
DELETE aca.dbo.employee
WHERE employee_id=@employeeID;


/******************************************************************************* 
Tabels with employee id but no foreign key constraint.
	- The employee will be removed, but these tables will have broken relationships.
*********************************************************************************

	1) BULK_Tax_year_1095c_correction
	2) EmployeeInsuranceOfferEditable
	3) IRSHoldingImport
	4) tax_year_1095c_correction
	5) tax_year_1095c_correction_exception
	6) log.insurance_coverage
	7) log.insurance_coverage_editable
	8) log.tax_year_1095c_approval
*/

END

GRANT EXECUTE ON [dbo].[REMOVE_employee_from_aca] TO [aca-user] AS [dbo]
GO





































GO


