USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO

ALTER TABLE [dbo].[fee] NOCHECK CONSTRAINT ALL
GO
ALTER TABLE [dbo].[fee] ADD [ResourceId] uniqueidentifier NOT NULL DEFAULT NEWID()
GO
ALTER TABLE [dbo].[fee] CHECK CONSTRAINT ALL
GO
CREATE UNIQUE INDEX [IDX_Fee_ResourceId] ON [dbo].[fee]([ResourceId])
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <1/6/2016>
-- Description:	<This stored procedure is meant to return all fees.>
-- Changes:
--			
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_all_fees]
AS
BEGIN

	SELECT
		[id],
		[su_fee],
		[base_fee],
		[employee_fee],
		[cap],
		[history],
		[modOn],
		[modBy],
		[billing_cycle_length_months],
		[current_discount_percentage],
		[ResourceId]
	FROM [fee];
END
GO

GRANT EXECUTE ON [dbo].[SELECT_all_fees] TO [aca-user] AS [dbo]
GO

GRANT EXECUTE ON [dbo].[PrepareAcaForIRSStaging] TO [aca-user] AS [dbo]
GO
GRANT EXECUTE ON [dbo].[Migrate_Coverage] TO [aca-user] AS [dbo]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <7/16/2014>
-- Description:	<This stored procedure is meant to return all employers.>
-- Changes:		<removed select *>
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_all_employers]
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
      ,[IrsEnabled]
	  ,[fee_id]
	FROM [employer]
	ORDER BY name;
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
-- Changes:		<removed select *>
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
      ,[IrsEnabled]
	  ,[fee_id]
	FROM [employer]
	WHERE employer_id = @employerID;
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
      ,[IrsEnabled]
      ,[fee_id]
	FROM [employer]
	WHERE [autoBill]=1;
END


GO
