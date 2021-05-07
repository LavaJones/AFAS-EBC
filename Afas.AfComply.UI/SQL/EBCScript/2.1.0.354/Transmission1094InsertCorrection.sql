USE aca
GO
-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE dbo.INSERT_tax_year_employer_1094_transmission
	@taxYear int,
	@employerId int,
	@transmissionType varchar(50),
	@UTId varchar(100),
	@USId varchar(100),
	@transmissionStatusCodeId int,
	@ORId varchar(50),
	@OUSId varchar(50),
	@userName varchar(50),
	@bulkFile varchar(max),
	@manifestFile varchar(max)
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO aca.dbo.tax_year_employer_transmission(
              tax_year,
              employer_id,
              TransmissionType,
              UniqueTransmissionId,                      
              UniqueSubmissionId,
              transmission_status_code_id,
              OriginalReceiptId,
              OriginalUniqueSubmissionId,
              EntityStatusId,
              CreatedBy,
              CreatedDate,
              ModifiedBy,
              ModifiedDate,
              BulkFile,
              ManifestFile
              )
       SELECT DISTINCT 
              @taxYear,
			  @employerId,
			  @transmissionType,
			  @UTId,
			  @USId,
			  @transmissionStatusCodeId,
			  @ORId,
			  @OUSId,
              1,
              @userName,
              GETDATE(),
              @userName,
              GETDATE(),
              @bulkFile,
			  @manifestFile 

   
END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO
GRANT EXEC ON dbo.INSERT_tax_year_employer_1094_transmission TO [aca-user]
GO
