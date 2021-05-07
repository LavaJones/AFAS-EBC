USE aca
GO
ALTER PROCEDURE [dbo].[spGetInsertDeletedLog]
	@tableLog int,
	@employerId int = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF(@tableLog = 1)
		SELECT [coveredIndividualLogId]
			  ,[covered_individual_id]
			  ,[employee_id]
			  ,[employer_id]
			  ,[first_name]
			  ,[middle_name]
			  ,[last_name]
			  ,[name_suffix]
			  ,[ssn]
			  ,[birth_date]
			  ,[annual_coverage_indicator]
			  ,[deleted]
			  ,[modifiedDate]
			  ,[modifiedBy]
			  ,[storedProcedureName]
		 FROM [air].[log].[covered_individual]
		 WHERE (employer_id = @employerId OR @employerId IS NULL)
	IF(@tableLog = 2)
		DECLARE @coveredMonthlyTemp TABLE(
			[coveredIndividualMonthlyLogId] int,
			[cim_id] int,
			[covered_individual_id] int,
			[time_frame_id] int,
			[covered_indicator] bit,
			[deleted] bit,
			[modifiedDate] datetime,
			[modifiedBy] varchar(50),
			[storedProcedureName] varchar(50)
		)

		INSERT INTO @coveredMonthlyTemp ( [coveredIndividualMonthlyLogId]
			  ,[cim_id]
			  ,[covered_individual_id]
			  ,[time_frame_id]
			  ,[covered_indicator]
			  ,[deleted]
			  ,[modifiedDate]
			  ,[modifiedBy]
			  ,[storedProcedureName])
		(SELECT cimd.[coveredIndividualMonthlyLogId]
			  ,cimd.[cim_id]
			  ,cimd.[covered_individual_id]
			  ,cimd.[time_frame_id]
			  ,cimd.[covered_indicator]
			  ,cimd.[deleted]
			  ,cimd.[modifiedDate]
			  ,cimd.[modifiedBy]
			  ,cimd.[storedProcedureName]
		  FROM [air].[log].[covered_individual_monthly_detail] cimd 
			INNER JOIN 
			   [air].[log].[covered_individual] ci 
					ON cimd.covered_individual_id = ci.covered_individual_id
				WHERE (employer_id = @employerId OR @employerId IS NULL)
		  UNION
		  SELECT cimd.[coveredIndividualMonthlyLogId]
			  ,cimd.[cim_id]
			  ,cimd.[covered_individual_id]
			  ,cimd.[time_frame_id]
			  ,cimd.[covered_indicator]
			  ,cimd.[deleted]
			  ,cimd.[modifiedDate]
			  ,cimd.[modifiedBy]
			  ,cimd.[storedProcedureName]
		  FROM [air].[log].[covered_individual_monthly_detail] cimd 
			INNER JOIN 
			   [air].[emp].[covered_individual] ci 
					ON cimd.covered_individual_id = ci.covered_individual_id
				WHERE (employer_id = @employerId OR @employerId IS NULL))
	IF(@tableLog = 3)
		SELECT [employeeMonthlyDetailLogId]
			  ,[employee_id]
			  ,[time_frame_id]
			  ,[employer_id]
			  ,[monthly_hours]
			  ,[offer_of_coverage_code]
			  ,[mec_offered]
			  ,[share_lowest_cost_monthly_premium]
			  ,[safe_harbor_code]
			  ,[enrolled]
			  ,[monthly_status_id]
			  ,[insurance_type_id]
			  ,[hra_flex_contribution]
			  ,[create_date]
			  ,[modified_date]
			  ,[modified_by]
			  ,[deleted]
			  ,[modifiedDate]
			  ,[modifiedBy]
			  ,[storedProcedureName]
		  FROM [air].[log].[employee_monthly_detail]
		  WHERE (employer_id = @employerId OR @employerId IS NULL)
	IF(@tableLog = 4)
		SELECT [employeeYearlyDetailLogId]
			  ,[employee_id]
			  ,[year_id]
			  ,[employer_id]
			  ,[annual_offer_of_coverage_code]
			  ,[annual_share_lowest_cost_monthly_premium]
			  ,[annual_safe_harbor_code]
			  ,[enrolled]
			  ,[submittal_ready]
			  ,[submitted]
			  ,[correction_ready]
			  ,[ack_status_code_id]
			  ,[_1095C]
			  ,[plan_start_month]
			  ,[create_date]
			  ,[modified_date]
			  ,[modified_by]
			  ,[insurance_type_id]
			  ,[must_supply_ci_info]
			  ,[is_1G]
			  ,[deleted]
			  ,[modifiedDate]
			  ,[modifiedBy]
			  ,[storedProcedureName]
		  FROM [air].[log].[employee_yearly_detail]
		  WHERE (employer_id = @employerId OR @employerId IS NULL)
	IF(@tableLog = 5)
		SELECT [taxYear1095cApprovalLogId],
			[tax_year],
			[employee_id],
			[employer_id],
			[approvedBy],
			[approvedOn],
			[get1095C],
			[ResourceId],
			[printed] [bit],
			[deleted] [bit],
			[modifiedDate],
			[modifiedBy],
			[storedProcedureName]
		FROM [log].[tax_year_1095c_approval]
		WHERE (employer_id = @employerId OR @employerId IS NULL) 
	IF(@tableLog = 6)
		SELECT [aleEmployerLogId]
			  ,[employer_id]
			  ,[ein]
			  ,[name]
			  ,[employer_control_name]
			  ,[address]
			  ,[city]
			  ,[state_code]
			  ,[zipcode]
			  ,[zipcode_ext]
			  ,[contact_first_name]
			  ,[contact_middle_name]
			  ,[contact_last_name]
			  ,[contact_name_suffix]
			  ,[contact_telephone]
			  ,[dge_ein]
			  ,[test_id]
			  ,[deleted]
			  ,[modifiedDate]
			  ,[modifiedBy]
			  ,[storedProcedureName]
		  FROM [air].[log].[aleEmployer]
		  WHERE (employer_id = @employerId OR @employerId IS NULL)
	IF(@tableLog = 7)	
		SELECT [empEmployeeLogId]
			  ,[employee_id]
			  ,[employer_id]
			  ,[first_name]
			  ,[middle_name]
			  ,[last_name]
			  ,[name_suffix]
			  ,[address]
			  ,[city]
			  ,[state_code]
			  ,[zipcode]
			  ,[zipcode_ext]
			  ,[telephone]
			  ,[ssn]
			  ,[deleted]
			  ,[modifiedDate]
			  ,[modifiedBy]
			  ,[storedProcedureName]
		  FROM [air].[log].[empEmployee]
		  WHERE (employer_id = @employerId OR @employerId IS NULL)
END