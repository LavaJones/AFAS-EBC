USE [aca]
GO

/****** Object:  StoredProcedure [dbo].[SELECT_all_1095FinalizedEmployers]    Script Date: 3/12/2018 1:01:50 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:          <Ganesh Nasani>
-- Create date: <03/09/2018>
-- Description:     <This stored procedure is meant to return all 1095 finalized employers .>
-- Changes:         <removed select *>
-- =============================================
Create PROCEDURE [dbo].[SELECT_all_1095FinalizedEmployers]
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
       where [employer_id] in ( select distinct ([employerID])
                                               from [aca].[dbo].[Approved1095Final] )
       ORDER BY name;
END


GO
GRANT EXECUTE ON [dbo].[SELECT_all_1095FinalizedEmployers] TO [aca-user]
GO