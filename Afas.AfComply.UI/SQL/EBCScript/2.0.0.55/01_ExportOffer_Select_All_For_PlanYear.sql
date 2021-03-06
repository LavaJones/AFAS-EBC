USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[SELECT_employer_planyear_insurance_offer_all]    Script Date: 10/26/2017 1:32:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Ryan McCully>
-- Create date: <10/17/2017>
-- Description:	<Need to get the full Offer and Change Event Details for a planyear.>
-- Reason Added:
--     We need to be able to export this data for any plan year.
-- =============================================
Create PROCEDURE [dbo].[SELECT_employer_planyear_insurance_offer_all](
	@employerID int,
	@planYearID int
	)
AS
BEGIN
	select  ROW_NUMBER() OVER(ORDER BY [employee_id] ASC) AS Row#
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
	 from (
		SELECT [employee_id]
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
		  FROM 
			  [aca].[dbo].[View_insurance_alert_details]
		  where 
			  [employer_id] = @employerID
					and
			  [plan_year_id] = @planYearID
		  union all 
		  select
			  [employee_id]
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
			from 
				[aca].[dbo].[View_insurance_alert_details_change_events]
			where 
				[employer_id] = @employerID
			  				and
				[plan_year_id] = @planYearID
		  ) As both
		  Order by [employee_id], [effectiveDate];		
    
END
GO

GO
GRANT EXECUTE ON [dbo].[SELECT_employer_planyear_insurance_offer_all] TO [aca-user] AS [dbo]
GO
