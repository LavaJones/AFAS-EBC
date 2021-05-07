USE air
GO
IF object_id('[log].[covered_individual]') is not null
	Begin
		DROP TABLE [log].[employee_yearly_detail]
		DROP TABLE [log].[employee_monthly_detail]
		DROP Table [log].[covered_individual]
		DROP Table [log].[covered_individual_monthly_detail]
		DROP SCHEMA [log]
		Drop TRIGGER [emp].[insertLog]
		Drop TRIGGER [emp].[deleteLog]
		Drop TRIGGER [emp].[insertMonthlyLog]
		Drop TRIGGER [emp].[deleteMonthlyLog]
		DROP TRIGGER [appr].[insertEmployeeMonthlyLog]
		DROP TRIGGER [appr].[deleteEmployeeMonthlyLog]
		DROP TRIGGER [appr].[insertEmployeeYearlyLog]
		DROP TRIGGER [appr].[deleteEmployeeYearlyLog]
	End
GO
GO
CREATE SCHEMA [log]
GO
CREATE TABLE [log].[covered_individual](
	[coveredIndividualLogId] [int] IDENTITY(1,1) NOT NULL,
	[covered_individual_id] [int] NULL,
	[employee_id] [int] NOT NULL,
	[employer_id] [int] NULL,
	[first_name] [nvarchar](50) NOT NULL,
	[middle_name] [nvarchar](50) NULL,
	[last_name] [nvarchar](50) NOT NULL,
	[name_suffix] [nvarchar](50) NULL,
	[ssn] [nvarchar](50) NULL,
	[birth_date] [date] NULL,
	[annual_coverage_indicator] [bit] NULL,
	[deleted] [bit] NOT NULL,
	[modifiedDate] [datetime] NOT NULL,
	[modifiedBy] [varchar](50) NOT NULL,
	[storedProcedureName] [varchar](50) NULL
 CONSTRAINT [PK_covered_individual] PRIMARY KEY CLUSTERED 
(
	[coveredIndividualLogId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [log].[covered_individual] ADD  CONSTRAINT [DF_covered_individual_annual_coverage_indicator]  DEFAULT ((0)) FOR [annual_coverage_indicator]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The identifier for the covered individual under the employees plan. Should match the dependent id in aca.' , @level0type=N'SCHEMA',@level0name=N'log', @level1type=N'TABLE',@level1name=N'covered_individual', @level2type=N'COLUMN',@level2name=N'covered_individual_id'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The employee''s id that the covered individual is related to.' , @level0type=N'SCHEMA',@level0name=N'log', @level1type=N'TABLE',@level1name=N'covered_individual', @level2type=N'COLUMN',@level2name=N'employee_id'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The covered individuals first name.' , @level0type=N'SCHEMA',@level0name=N'log', @level1type=N'TABLE',@level1name=N'covered_individual', @level2type=N'COLUMN',@level2name=N'first_name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The covered individuals middle name.' , @level0type=N'SCHEMA',@level0name=N'log', @level1type=N'TABLE',@level1name=N'covered_individual', @level2type=N'COLUMN',@level2name=N'middle_name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The covered individuals last name.' , @level0type=N'SCHEMA',@level0name=N'log', @level1type=N'TABLE',@level1name=N'covered_individual', @level2type=N'COLUMN',@level2name=N'last_name'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The covered individuals name suffix.' , @level0type=N'SCHEMA',@level0name=N'log', @level1type=N'TABLE',@level1name=N'covered_individual', @level2type=N'COLUMN',@level2name=N'name_suffix'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The covered individuals social security number.' , @level0type=N'SCHEMA',@level0name=N'log', @level1type=N'TABLE',@level1name=N'covered_individual', @level2type=N'COLUMN',@level2name=N'ssn'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The covered individuals birth date.' , @level0type=N'SCHEMA',@level0name=N'log', @level1type=N'TABLE',@level1name=N'covered_individual', @level2type=N'COLUMN',@level2name=N'birth_date'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The covered individuals annual coverage indicator.' , @level0type=N'SCHEMA',@level0name=N'log', @level1type=N'TABLE',@level1name=N'covered_individual', @level2type=N'COLUMN',@level2name=N'annual_coverage_indicator'
GO
CREATE TABLE [log].[covered_individual_monthly_detail](
	[coveredIndividualMonthlyLogId] [int] IDENTITY(1,1) NOT NULL,
	[cim_id] [int] NOT NULL,
	[covered_individual_id] [int] NULL,
	[time_frame_id] [int] NOT NULL,
	[covered_indicator] [bit] NOT NULL,
	[deleted] [bit] NOT NULL,
	[modifiedDate] [datetime] NOT NULL,
	[modifiedBy] [varchar](50) NOT NULL,
	[storedProcedureName] [varchar](50) NULL
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [log].[covered_individual_monthly_detail] ADD  CONSTRAINT [DF_covered_individual_monthly_detail_covered_indicator]  DEFAULT ((1)) FOR [covered_indicator]
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The table key.' , @level0type=N'SCHEMA',@level0name=N'log', @level1type=N'TABLE',@level1name=N'covered_individual_monthly_detail', @level2type=N'COLUMN',@level2name=N'cim_id'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The identifier fo the covered individual under th employees plan; foreign key to the covered individual table.' , @level0type=N'SCHEMA',@level0name=N'log', @level1type=N'TABLE',@level1name=N'covered_individual_monthly_detail', @level2type=N'COLUMN',@level2name=N'covered_individual_id'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'A timeframe is a year and month combination.' , @level0type=N'SCHEMA',@level0name=N'log', @level1type=N'TABLE',@level1name=N'covered_individual_monthly_detail', @level2type=N'COLUMN',@level2name=N'time_frame_id'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Flag Denoting whether covered individual was covered for the month.' , @level0type=N'SCHEMA',@level0name=N'log', @level1type=N'TABLE',@level1name=N'covered_individual_monthly_detail', @level2type=N'COLUMN',@level2name=N'covered_indicator'
--GO
--ALTER SCHEMA [log] TRANSFER dbo.covered_individual
--GO
--ALTER SCHEMA [log] TRANSFER dbo.covered_individual_monthly_detail
GO
CREATE TABLE [log].[employee_monthly_detail](
	[employeeMonthlyDetailLogId] [int] IDENTITY(1,1) NOT NULL,
	[employee_id] [int] NOT NULL,
	[time_frame_id] [int] NOT NULL,
	[employer_id] [int] NULL,
	[monthly_hours] [decimal](18, 2) NULL,
	[offer_of_coverage_code] [nchar](2) NULL,
	[mec_offered] [bit] NULL,
	[share_lowest_cost_monthly_premium] [decimal](18, 2) NULL,
	[safe_harbor_code] [nchar](2) NULL,
	[enrolled] [bit] NOT NULL,
	[monthly_status_id] [tinyint] NULL,
	[insurance_type_id] [tinyint] NULL,
	[hra_flex_contribution] [decimal](10, 2) NULL,
	[create_date] [datetime] NULL CONSTRAINT [DF_employee_monthly_detail_create_date]  DEFAULT (getdate()),
	[modified_date] [datetime] NULL CONSTRAINT [DF_employee_monthly_detail_modified_date]  DEFAULT (getdate()),
	[modified_by] [nvarchar](100) NULL CONSTRAINT [DF_employee_monthly_detail_modified_by]  DEFAULT (suser_sname()),
	[deleted] [bit] NOT NULL,
	[modifiedDate] [datetime] NOT NULL,
	[modifiedBy] [varchar](50) NOT NULL,
	[storedProcedureName] [varchar](50) NULL
	) ON [PRIMARY]
GO
CREATE TABLE [log].[employee_yearly_detail](
	[employeeYearlyDetailLogId] [int] IDENTITY(1,1) NOT NULL,
	[employee_id] [int] NOT NULL,
	[year_id] [smallint] NOT NULL,
	[employer_id] [int] NULL,
	[annual_offer_of_coverage_code] [nchar](2) NULL,
	[annual_share_lowest_cost_monthly_premium] [decimal](18, 2) NULL,
	[annual_safe_harbor_code] [nchar](2) NULL,
	[enrolled] [bit] NOT NULL,
	[submittal_ready] [bit] NOT NULL CONSTRAINT [DF_employee_yearly_detail_submittal_ready]  DEFAULT ((0)),
	[submitted] [bit] NOT NULL CONSTRAINT [DF_employee_yearly_detail_submitted]  DEFAULT ((0)),
	[correction_ready] [bit] NULL,
	[ack_status_code_id] [tinyint] NULL,
	[_1095C] [bit] NULL CONSTRAINT [DF_employee_yearly_detail__1095C]  DEFAULT ((0)),
	[plan_start_month] [nchar](2) NULL,
	[create_date] [datetime] NULL CONSTRAINT [DF_employee_yearly_detail_create_date]  DEFAULT (getdate()),
	[modified_date] [datetime] NULL CONSTRAINT [DF_employee_yearly_detail_modified_date]  DEFAULT (getdate()),
	[modified_by] [nvarchar](100) NULL CONSTRAINT [DF_employee_yearly_detail_modified_by]  DEFAULT (suser_sname()),
	[insurance_type_id] [tinyint] NULL,
	[must_supply_ci_info] [bit] NOT NULL CONSTRAINT [DF_employee_yearly_detail_must_supply_ci_info]  DEFAULT ((0)),
	[is_1G] [bit] NOT NULL CONSTRAINT [DF_employee_yearly_detail_is_1G]  DEFAULT ((0)),
	[deleted] [bit] NOT NULL,
	[modifiedDate] [datetime] NOT NULL,
	[modifiedBy] [varchar](50) NOT NULL,
	[storedProcedureName] [varchar](50) NULL
	) ON [PRIMARY]
GO
USE air
GO
CREATE TRIGGER [emp].[insertLog]
ON [emp].covered_individual
FOR INSERT
AS
BEGIN
	INSERT INTO [log].[covered_individual]	([employee_id]
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
      ,[storedProcedureName])
	  SELECT [employee_id]
      ,[employer_id]
      ,[first_name]
      ,[middle_name]
      ,[last_name]
      ,[name_suffix]
      ,[ssn]
      ,[birth_date]
      ,[annual_coverage_indicator]
      ,0
      ,GETDATE()
      ,'SYSTEM TRIGGER'
      ,OBJECT_NAME(@@PROCID)
	 FROM inserted
END
GO
CREATE TRIGGER [emp].[deleteLog]
ON [emp].covered_individual
FOR DELETE
AS
BEGIN
	INSERT INTO [log].[covered_individual]	([employee_id]
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
      ,[storedProcedureName])
	  SELECT [employee_id]
      ,[employer_id]
      ,[first_name]
      ,[middle_name]
      ,[last_name]
      ,[name_suffix]
      ,[ssn]
      ,[birth_date]
      ,[annual_coverage_indicator]
      ,1
      ,GETDATE()
      ,'SYSTEM TRIGGER'
      ,OBJECT_NAME(@@PROCID)
	FROM deleted
END
GO
CREATE TRIGGER [emp].[insertMonthlyLog]
ON [emp].[covered_individual_monthly_detail]
FOR INSERT 
AS
BEGIN
	INSERT INTO [log].[covered_individual_monthly_detail] ([cim_id]
      ,[covered_individual_id]
      ,[time_frame_id]
      ,[covered_indicator]
      ,[deleted]
      ,[modifiedDate]
      ,[modifiedBy]
      ,[storedProcedureName])
	 SELECT [cim_id]
      ,[covered_individual_id]
      ,[time_frame_id]
      ,[covered_indicator]
      ,'0'
      ,GETDATE()
      ,'SYSTEM TRIGGER'
      ,OBJECT_NAME(@@PROCID)
	FROM inserted
END
GO
CREATE TRIGGER [emp].[deleteMonthlyLog]
ON [emp].[covered_individual_monthly_detail]
FOR DELETE
AS
BEGIN
	INSERT INTO [log].[covered_individual_monthly_detail] ([cim_id]
      ,[covered_individual_id]
      ,[time_frame_id]
      ,[covered_indicator]
      ,[deleted]
      ,[modifiedDate]
      ,[modifiedBy]
      ,[storedProcedureName])
	 SELECT [cim_id]
      ,[covered_individual_id]
      ,[time_frame_id]
      ,[covered_indicator]
      ,'1'
      ,GETDATE()
      ,'SYSTEM TRIGGER'
      ,OBJECT_NAME(@@PROCID)
	FROM deleted
END
GO
CREATE TRIGGER [appr].[insertEmployeeMonthlyLog]
ON [appr].[employee_monthly_detail]
FOR INSERT
AS
BEGIN
	INSERT INTO [log].[employee_monthly_detail] ([employee_id]
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
      ,[storedProcedureName])
	SELECT [employee_id]
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
      ,'0'
      ,GETDATE()
      ,'SYSTEM TRIGGER'
      ,OBJECT_NAME(@@PROCID)
	  FROM inserted
END
GO
CREATE TRIGGER [appr].[deleteEmployeeMonthlyLog]
ON [appr].[employee_monthly_detail]
FOR DELETE
AS
BEGIN
	INSERT INTO [log].[employee_monthly_detail] ([employee_id]
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
      ,[storedProcedureName])
	SELECT [employee_id]
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
      ,'1'
      ,GETDATE()
      ,'SYSTEM TRIGGER'
      ,OBJECT_NAME(@@PROCID)
	  FROM deleted
END
GO
CREATE TRIGGER [appr].[insertEmployeeYearlyLog]
ON [appr].[employee_yearly_detail]
FOR INSERT
AS
BEGIN
	INSERT INTO [log].[employee_yearly_detail] ([employee_id]
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
      ,[storedProcedureName])
	SELECT [employee_id]
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
      ,'0'
      ,GETDATE()
      ,'SYSTEM TRIGGER'
      ,OBJECT_NAME(@@PROCID)
	FROM inserted
END
GO
CREATE TRIGGER [appr].[deleteEmployeeYearlyLog]
ON [appr].[employee_yearly_detail]
FOR DELETE
AS
BEGIN
	INSERT INTO [log].[employee_yearly_detail] ([employee_id]
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
      ,[storedProcedureName])
	SELECT [employee_id]
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
      ,'1'
      ,GETDATE()
      ,'SYSTEM TRIGGER'
      ,OBJECT_NAME(@@PROCID)
	FROM deleted
END