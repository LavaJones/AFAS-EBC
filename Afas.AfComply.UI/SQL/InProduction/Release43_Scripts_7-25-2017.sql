USE [aca]
GO

/****** Object:  StoredProcedure [dbo].[SELECT_EmployeeInsuranceOfferAndEmployeeInsuranceOfferArchive_ForEmployee]    Script Date: 7/18/2017 4:46:15 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Ganesh Nasani>
-- Create date: <07/17/2017>
-- Description:	<This stored procedure returns all insurance offered rows from [employee_insurance_offer] and [employee_insurance_offer_archive]  
--				tabels offered to specific employee by specific employer
--			
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_EmployeeInsuranceOfferAndEmployeeInsuranceOfferArchive_ForEmployee]
	(@employeeID int
	)
AS

BEGIN
	SELECT [employee_id]
		  ,[employer_id]
		  ,[InsuranceDescription]
		  ,[PlanYearDescryption]
		  ,[InsuranceContributionAmount]
		  ,[Monthlycost]
		  ,[avg_hours_month]
		  ,[hra_flex_contribution]
		  ,[offered]
		  ,[offeredOn]
		  ,[accepted]
		  ,[acceptedOn]
		  ,[notes]
		  ,[history]
		  ,[effectiveDate]
  FROM [aca].[dbo].[View_Employee_Insurance_Offer_All]
  WHERE employee_id = @employeeID 
END

GO

GRANT EXECUTE ON [dbo].[SELECT_EmployeeInsuranceOfferAndEmployeeInsuranceOfferArchive_ForEmployee]  TO [aca-user] AS [dbo]
GO

/****** Object:  View [dbo].[View_Employee_Insurance_Offer_All]    Script Date: 7/18/2017 4:48:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[View_Employee_Insurance_Offer_All]
AS

---Employee Insurance offer
 SELECT EEIO.[employee_id],EEIO.employer_id,INS.[description] AS [InsuranceDescription], PY.[description] as [PlanYearDescryption]
      ,INSC.amount as [InsuranceContributionAmount],[Monthlycost],[avg_hours_month],[hra_flex_contribution],[offered],[offeredOn],[accepted]
      ,[acceptedOn],EEIO.[notes],EEIO.[history],[effectiveDate] 
 FROM [aca].[dbo].[employee_insurance_offer]EEIO 
 left join [aca].[dbo].[plan_year] PY  ON EEIO.plan_year_id=PY.plan_year_id
 left join [aca].[dbo].[insurance]INS on EEIO.insurance_id=INS.insurance_id
 left join [aca].[dbo].[insurance_contribution] INSC on EEIO.ins_cont_id=INSC.ins_cont_id
UNION 
  ---Employee Insurance offer archive
  SELECT EEIOA.[employee_id],EEIOA.employer_id,INS.[description] AS [InsuranceDescription], PY.[description] as [PlanYearDescryption]
      ,INSC.amount as [InsuranceContributionAmount],[Monthlycost],[avg_hours_month],[hra_flex_contribution],[offered],[offeredOn],[accepted]
      ,[acceptedOn],EEIOA.[notes],EEIOA.[history],[effectiveDate]
 FROM [aca].[dbo].[employee_insurance_offer_archive]EEIOA 
 left join [aca].[dbo].[plan_year] PY  ON EEIOA.plan_year_id=PY.plan_year_id AND EEIOA.employer_id=PY.employer_id
 left join [aca].[dbo].[insurance]INS on EEIOA.insurance_id=INS.insurance_id
 left join [aca].[dbo].[insurance_contribution] INSC on EEIOA.ins_cont_id=INSC.ins_cont_id
 
GO

GRANT select ON [aca].[dbo].[View_Employee_Insurance_Offer_All]  TO [aca-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Ganesh Nasani
-- Create date: 07/19/2017
-- Description: This stored procedure  set multiple rows of table [EmployeeMeasurementAverageHours] as Entity status = 2 (inactive) for all employees belonging to an employer.
--				This stored Proc takes employer_ID as a param and only change it for those employees of that employer.
-- =============================================
create PROCEDURE [dbo].[Update_EmployeeMeasurementAverageHours_EntityStatusToInactive_ForEmployeesOfEmployer]
	@employerId int,
	@modifiedBy nvarchar(50)
AS
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;
	UPDATE EMAH
	SET EntityStatusId = 2,
		ModifiedBy=@modifiedBy,
		ModifiedDate=getdate()
	from [aca].[dbo].[EmployeeMeasurementAverageHours] EMAH
	join [aca].[dbo].[employee] Empee
	on empee.employee_id=EMAH.EmployeeId
	WHERE empee.[employer_id]=@employerId;
END

GO

GRANT EXECUTE ON [dbo].[Update_EmployeeMeasurementAverageHours_EntityStatusToInactive_ForEmployeesOfEmployer]  TO [aca-user] AS [dbo]
GO

/****** Object:  StoredProcedure [dbo].[SELECT_employee_individual_coverage_all_tax_years]    Script Date: 7/17/2017 10:55:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Travis Wells>
-- Create date: <6/19/2017>
-- Description:	<This stored procedure is meant to return all rows of imported coverage data for a specific employee regardless of tax year.>
--         - This is being used on the employee merge screen. 
--Modified:By GN, On 07/17/2017. Replaced * with column names and removed GRANT EXECUTE statement from stored proc. 
--			
-- =============================================
Create PROCEDURE [dbo].[SELECT_employee_individual_coverage_all_tax_years]
	@employeeID int
AS

BEGIN
	SELECT [dependent_id]
		  ,[employee_id]
		  ,[fName]
		  ,[mName]
		  ,[lName]
		  ,[ssn]
		  ,[dob]
		  ,[row_id]
		  ,[tax_year]
		  ,[carrier_id]
		  ,[all12]
		  ,[jan]
		  ,[feb]
		  ,[mar]
		  ,[apr]
		  ,[may]
		  ,[jun]
		  ,[jul]
		  ,[aug]
		  ,[sep]
		  ,[oct]
		  ,[nov]
		  ,[dec]
		  ,[history]
  FROM [aca].[dbo].[View_all_insurance_coverage]
  WHERE employee_id = @employeeID AND dependent_id IS NULL;
END

go
GRANT EXECUTE ON [dbo].[SELECT_employee_individual_coverage_all_tax_years] TO [aca-user] AS [dbo]
GO
