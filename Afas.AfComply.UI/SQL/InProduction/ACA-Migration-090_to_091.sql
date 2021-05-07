USE aca
GO
IF object_id('[log].[tax_year_1095c_approval]') is not null
	Begin
		DROP TABLE [log].[tax_year_1095c_approval]
		Drop TRIGGER [dbo].[insertTaxYear1095cApprovalLog]
		Drop TRIGGER [dbo].[deleteTaxYear1095cApprovalLog]		
		DROP SCHEMA [log]
	End
GO
CREATE SCHEMA [log]
GO
CREATE TABLE [log].[tax_year_1095c_approval](
	[taxYear1095cApprovalLogId] [int] NOT NULL IDENTITY(1,1),
	[tax_year] [int] NOT NULL,
	[employee_id] [int] NOT NULL,
	[employer_id] [int] NOT NULL,
	[approvedBy] [varchar](50) NOT NULL,
	[approvedOn] [datetime] NOT NULL,
	[get1095C] [bit] NOT NULL,
	[ResourceId] [uniqueidentifier] NOT NULL,
	[printed] [bit] NOT NULL,
	[deleted] [bit] NOT NULL,
	[modifiedDate] [datetime] NOT NULL,
	[modifiedBy] [varchar](50) NOT NULL,
	[storedProcedureName] [varchar](50) NULL
) ON [PRIMARY]

GO
GRANT SELECT ON [log].[tax_year_1095c_approval] TO [aca-user] AS [DBO]
GO
GRANT INSERT ON [log].[tax_year_1095c_approval] TO [aca-user] AS [DBO]
GO
--GRANT EXECUTE ON [log].[tax_year_1095c_approval] TO [aca-user] AS [DBO]
--GO
CREATE TRIGGER [dbo].[insertTaxYear1095cApprovalLog]
ON [dbo].[tax_year_1095c_approval]
FOR INSERT
AS
BEGIN
	INSERT INTO [log].[tax_year_1095c_approval]	([tax_year]
      ,[employee_id]
      ,[employer_id]
      ,[approvedBy]
      ,[approvedOn]
      ,[get1095C]
      ,[ResourceId]
      ,[printed]
	  ,[deleted]
      ,[modifiedDate]
      ,[modifiedBy]
      ,[storedProcedureName])
	  SELECT [tax_year]
      ,[employee_id]
      ,[employer_id]
      ,[approvedBy]
      ,[approvedOn]
      ,[get1095C]
      ,[ResourceId]
      ,[printed]
      ,0
      ,GETDATE()
      ,'SYSTEM TRIGGER'
      ,OBJECT_NAME(@@PROCID)
	 FROM inserted
END
GO
CREATE TRIGGER [dbo].[deleteTaxYear1095cApprovalLog]
ON [dbo].[tax_year_1095c_approval]
FOR DELETE
AS
BEGIN
	INSERT INTO [log].[tax_year_1095c_approval]	([tax_year]
      ,[employee_id]
      ,[employer_id]
      ,[approvedBy]
      ,[approvedOn]
      ,[get1095C]
      ,[ResourceId]
      ,[printed]
	  ,[deleted]
      ,[modifiedDate]
      ,[modifiedBy]
      ,[storedProcedureName])
	  SELECT [tax_year]
      ,[employee_id]
      ,[employer_id]
      ,[approvedBy]
      ,[approvedOn]
      ,[get1095C]
      ,[ResourceId]
      ,[printed]
      ,1
      ,GETDATE()
      ,'SYSTEM TRIGGER'
      ,OBJECT_NAME(@@PROCID)
	FROM deleted
END
GO
CREATE PROCEDURE [dbo].[spGetInsertDeletedLog]
	@tableLog int
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
	IF(@tableLog = 2)
		SELECT TOP 1000 [coveredIndividualMonthlyLogId]
			  ,[cim_id]
			  ,[covered_individual_id]
			  ,[time_frame_id]
			  ,[covered_indicator]
			  ,[deleted]
			  ,[modifiedDate]
			  ,[modifiedBy]
			  ,[storedProcedureName]
		  FROM [air].[log].[covered_individual_monthly_detail]
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
END
GO
GRANT EXECUTE ON [dbo].[spGetInsertDeletedLog] TO [aca-user] AS [DBO]
GO
