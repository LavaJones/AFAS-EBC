USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[SELECT_employee_with_employer_info]    Script Date: 12/20/2016 1:25:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[SELECT_employee_with_employer_info]
	@employerID int,
	@taxYearId int
AS
BEGIN TRY
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	WITH  empB AS
      (
		  SELECT 
			a.employee_id, 
			a.employer_id,
			c.ResourceId,
			a.first_name AS Fname, 
			a.middle_name AS Mi, 
			a.last_name AS Lname, 
			a.ssn,
			a.name_suffix AS Suffix, 
			a.[address] AS [Address],
			a.city AS City, 
			a.state_code AS [State], 
			a.zipcode AS ZIP,
			c.dob
		FROM air.emp.employee a INNER JOIN aca.dbo.employee c ON a.employee_id = c.employee_id 
							    INNER JOIN aca.dbo.tax_year_1095c_approval e ON e.employee_id = a.employee_id
		WHERE a.employer_id = @employerID 
			AND e.tax_year = @taxYearId
			AND e.get1095C = 1 
      )
      SELECT DISTINCT 
		B.employee_id,
	    e.ResourceId as EmployerResourceId,
		d.name as BusinessNameLine1,
		d.[address] as BusinessAddressLine1Txt, 
		d.city as BusinessCityNm, 
		d.state_code AS BusinessUSStateCd,
		d.zipcode AS BusinessUSZIPCd, 
		d.ein as EIN,
        d.contact_telephone AS ContactPhoneNum,
		B.ResourceId AS EmployeeResourceId,
		B.Fname,
		B.Mi,
		B.Lname,
		B.Suffix,
		B.City,
		B.[State],
		B.[Address],
		B.ZIP,
		B.ssn,
		B.dob as PersonBirthDt
      FROM empB B INNER JOIN air.ale.employer d ON d.employer_id = B.employer_id
				  INNER JOIN employer e ON e.employer_id = B.employer_id		
      ORDER BY b.Fname
END TRY
BEGIN CATCH
      EXEC INSERT_ErrorLogging
END CATCH