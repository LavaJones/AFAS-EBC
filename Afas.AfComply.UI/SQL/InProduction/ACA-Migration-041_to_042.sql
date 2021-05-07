USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Travis Wells>
-- Create date: <3/21/2016>
-- Description:	<This stored procedure is meant to return all monthly detail records for an employee.>
-- Changes:
--			
-- =============================================
CREATE PROCEDURE [dbo].[SELECT_employee_with_employer_info]
	@employerID int
AS

BEGIN
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
		WHERE a.employer_id = @employerID
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
END

GO
/****** Object:  StoredProcedure [dbo].[SELECT_employee_offer_and_coverage]    Script Date: 1/31/2017 10:57:08 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SELECT_employee_offer_and_coverage]
	-- Add the parameters for the stored procedure here
	@employeeID int,
	@taxYear int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT 
		a.employee_id,
		a.time_frame_id,
		a.employer_id, 
		a.offer_of_coverage_code, 
		a.monthly_status_id, 
		a.monthly_hours,
		m.name,
		f.year_id,
		a.safe_harbor_code, 
		a.mec_offered, 
		a.insurance_type_id
	FROM air.emp.monthly_detail a INNER JOIN air.gen.time_frame b ON b.time_frame_id = a.time_frame_id
								INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
								INNER JOIN air.gen.time_frame f ON f.time_frame_id = b.time_frame_id
	WHERE a.employee_id = @employeeID AND b.year_id = @taxYear
	ORDER By m.month_id

END
