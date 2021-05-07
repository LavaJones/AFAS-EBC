USE [aca]
--Start Migration script 22-23
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
ALTER TABLE dbo.employer ALTER COLUMN name VARCHAR(75) NULL
GO
ALTER TABLE dbo.employer ALTER COLUMN DBAName VARCHAR(75) NULL
GO
ALTER TABLE dbo.PlanYearGroup ALTER COLUMN GroupName NVARCHAR(75) NOT NULL
GO
ALTER TABLE dbo.equivalency ALTER COLUMN name VARCHAR(75) NULL
GO
ALTER TABLE dbo.equiv_detail ALTER COLUMN name VARCHAR(75) NOT NULL
GO
/****** Object:  StoredProcedure [dbo].[INSERT_new_employer]    Script Date: 11/29/2016 9:41:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[INSERT_new_employer]
      @name varchar(50),
      @add varchar(75),
      @city varchar(50),
      @stateID int,
      @zip varchar(15),
      @logo varchar(50),
      @b_add varchar(50),
      @b_city varchar(50),
      @b_stateID int,
      @b_zip varchar(15),
      @empTypeID int,
      @ein varchar(50),
      @dbaName varchar(100),
      @empid int OUTPUT
AS
 
BEGIN TRY
      INSERT INTO [employer](
            name,
            [address],
            city,
            state_id,
            zip,
            img_logo,
            bill_address,
            bill_city,
            bill_state,
            bill_zip,
            employer_type_id, 
            ein,
            DBAName)
      VALUES(
            @name,
            @add,
            @city,
            @stateID,
            @zip,
            @logo,
            @b_add,
            @b_city,
            @b_stateID,
            @b_zip,
            @empTypeID,
            @ein,
            @dbaName)
 
SELECT @empid = SCOPE_IDENTITY();
END TRY
BEGIN CATCH
      exec dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER PROCEDURE [dbo].[INSERT_new_equivalency]
      @employerID int,
      @name varchar(75),
      @gpID int,
      @every decimal(18,4),
      @unitID int,
      @credit decimal(18,4),
      @sdate datetime,
      @edate datetime,
      @notes varchar(1000),
      @modBy varchar(50),
      @modOn datetime, 
      @history varchar(max),
      @active bit,
      @equivTypeID int,
      @posID int,
      @actID int,
      @detID int,
      @equivalencyID int OUTPUT
AS
 
BEGIN TRY
      INSERT INTO [equivalency](
            employer_id,
            name,
            gpid,
            every,
            unit_id,
            credit,
            [start_date],
            end_date,
            notes,
            modBy,
            modOn,
            history,
            active,
            equivalency_type_id,
            position_id,
            activity_id,
            detail_id)
      VALUES(
            @employerID,
            @name,
            @gpID,
            @every,
            @unitID,
            @credit,
            @sdate,
            @edate,
            @notes,
            @modBy,
            @modOn,
            @history,
            @active,
            @equivTypeID,
            @posID,
            @actID,
            @detID)
 
SELECT @equivalencyID = SCOPE_IDENTITY();
END TRY
BEGIN CATCH
      exec dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER PROCEDURE [dbo].[INSERT_UPDATE_employer_irs_submission_approval]
      @approvalID int,
      @employerID int,
      @taxYear int,
      @dge bit,
      @dgeName varchar(75),
      @dgeEIN varchar(50),
      @dgeAddress varchar(50),
      @dgeCity varchar(50),
      @dgeStateID int,
      @dgeZip varchar(50),
      @dgeFname varchar(50),
      @dgeLname varchar(50),
      @dgePhone varchar(50),
      @ale bit,
      @tr1 bit,
      @tr2 bit, 
      @tr3 bit,
      @tr4 bit,
      @tr5 bit,
      @tr bit,
      @tobacco bit,
      @unpaidLeave bit,
      @safeHarbor bit,
      @completedBy varchar(50),
      @completedOn datetime,
      @ebcApproved bit,
      @ebcApprovedBy varchar(50),
      @ebcApprovedOn datetime,
      @allowEditing bit,
      @approvalID_Final int OUTPUT
AS
 
BEGIN TRY
      SET @approvalID_Final = @approvalID;
 
      /************************************************************************************************************************
      Compare EmployerID and TAX YEAR to see if a record exists. 
      ************************************************************************************************************************/
IF @approvalID_Final<= 0
      BEGIN
            SELECT @approvalID_Final=approval_id FROM tax_year_approval
            WHERE employer_id=@employerID AND tax_year=@taxYear;
      END
 
IF @approvalID_Final <= 0
      BEGIN
            INSERT INTO [tax_year_approval](
                  employer_id,
                  tax_year,
                  dge,
                  dge_name,
                  dge_ein,
                  dge_address,
                  dge_city,
                  state_id,
                  dge_zip,
                  dge_contact_fname,
                  dge_contact_lname,
                  dge_phone,
                  ale,
                  tr_q1,
                  tr_q2,
                  tr_q3,
                  tr_q4,
                  tr_q5,
                  tr_qualified,
                  tobacco,
                  unpaidLeave,
                  safeHarbor,
                  completed_by,
                  completed_on,
                  ebc_approval,
                  ebc_approved_by,
                  ebc_approved_on,
                  allow_editing)
            VALUES(
                  @employerID,
                  @taxYear,
                  @dge,
                  @dgeName,
                  @dgeEIN,
                  @dgeAddress,
                  @dgeCity,
                  @dgeStateID,
                  @dgeZip,
                  @dgeFname,
                  @dgeLname,
                  @dgePhone,
                  @ale,
                  @tr1,
                  @tr2, 
                  @tr3,
                  @tr4,
                  @tr5,
                  @tr,
                  @tobacco,
                  @unpaidLeave,
                  @safeHarbor,
                  @completedBy,
                  @completedOn,
                  @ebcApproved,
                  @ebcApprovedBy,
                  @ebcApprovedOn,
                  @allowEditing)
      END
ELSE
      BEGIN
            UPDATE [tax_year_approval]
            SET
                  employer_id=@employerID,
                  tax_year=@taxYear,
                  dge=@dge,
                  dge_name=@dgeName,
                  dge_ein=@dgeEIN,
                  dge_address=@dgeAddress,
                  dge_city=@dgeCity,
                  state_id=@dgeStateID,
                  dge_zip=@dgeZip,
                  dge_contact_fname=@dgeFname,
                  dge_contact_lname=@dgeLname,
                  dge_phone=@dgePhone,
                  ale=@ale,
                  tr_q1=@tr1,
                  tr_q2=@tr2,
                  tr_q3=@tr3,
                  tr_q4=@tr4,
                  tr_q5=@tr5,
                  tr_qualified=@tr,
                  tobacco=@tobacco,
                  unpaidLeave=@unpaidLeave,
                  safeHarbor=@safeHarbor,
                  completed_by=@completedBy,
                  completed_on=@completedOn,
                  ebc_approval=@ebcApproved,
                  ebc_approved_by=@ebcApprovedBy,
                  ebc_approved_on=@ebcApprovedOn,
                  allow_editing=@allowEditing
            WHERE
                  approval_id=@approvalID_Final;
      END
 
IF @approvalID_Final <= 0
      BEGIN
            SET @approvalID_Final = SCOPE_IDENTITY();
      END
 
SELECT @approvalID_Final;
END TRY
BEGIN CATCH
      exec dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER PROCEDURE [dbo].[UPDATE_employer]
       @employerID int,
       @name varchar(75),
       @address varchar(50),
       @city varchar(50),
       @stateID int,
       @zip varchar(15),
       @logo varchar(50),
       @ein varchar(50),
       @employerTypeId int,
         @dbaName varchar(100)
AS
BEGIN TRY
       UPDATE employer
       SET
             name = @name, 
             [address] = @address,
             city = @city,
             state_id = @stateID, 
             zip = @zip,  
             img_logo = @logo,
             ein = @ein,
             employer_type_id = @employerTypeId,
                  DBAName = @dbaName
       WHERE
             employer_id = @employerID;
 
END TRY
BEGIN CATCH
       exec dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER PROCEDURE [dbo].[UPDATE_equivalency]
      @equivalencyID int,
      @employerID int,
      @name varchar(75),
      @gpID int,
      @every decimal(18,4),
      @unitID int,
      @credit decimal(18,4),
      @sdate datetime,
      @edate datetime,
      @notes varchar(1000),
      @modBy varchar(50),
      @modOn datetime, 
      @history varchar(max),
      @active bit,
      @equivTypeID int,
      @posID int,
      @actID int,
      @detID int
AS
 
BEGIN TRY
      UPDATE [equivalency]
      SET
            employer_id = @employerID,
            name = @name,
            gpID = @gpID,
            every = @every,
            unit_id = @unitID,
            credit = @credit,
            [start_date] = @sdate,
            end_date = @edate,
            notes = @notes,
            modBy = @modBy,
            modOn = @modOn,
            history = @history,
            active = @active,
            equivalency_type_id = @equivTypeID,
            position_id = @posID,
            activity_id = @actID,
            detail_id = @detID
      WHERE
            equivalency_id = @equivalencyID
 
END TRY
BEGIN CATCH
      exec dbo.INSERT_ErrorLogging
END CATCH
GO
ALTER PROCEDURE [dbo].[UPDATE_PlanYearGroup]
      @modifiedBy nvarchar(50),
      @PlanYearGroupId int,
      @GroupName nvarchar(75)
AS
BEGIN TRY
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;
    UPDATE [dbo].[PlanYearGroup] SET GroupName = @GroupName,
            ModifiedBy = @modifiedBy, ModifiedDate = GETDATE()
      WHERE PlanYearGroupId = @PlanYearGroupId
END TRY
BEGIN CATCH
      EXEC INSERT_ErrorLogging
END CATCH
--End Migration script 22-23