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
CREATE PROCEDURE etl.spUpdate_employee_from_file 
	-- Add the parameters for the stored procedure here
	@first_name nvarchar(50),
	@middle_name nvarchar(50),
	@last_name nvarchar(50),
	@address nvarchar(50),
	@city nvarchar(50),
	@state_code nchar(2),
	@zipcode nchar(5),
	@telephone nvarchar(12),
	@employerResourceId uniqueidentifier,
	@employeeResourceId uniqueidentifier
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE air.emp.employee  
		SET first_name = @first_name, 
			last_name = @last_name, 
			[address] = @address, 
			city = @city, 
			state_code = @state_code, 
			zipcode = @zipcode,
			telephone = @telephone
	FROM	aca.dbo.employee ee
				INNER JOIN aca.dbo.employer aca_er ON (ee.employer_id = aca_er.employer_id)
				INNER JOIN air.ale.employer er ON (ee.employer_id = er.employer_id)
				INNER JOIN air.emp.employee em ON (ee.employer_id = em.employer_id) AND (ee.employee_id = em.employee_id)
				INNER JOIN aca.dbo.state s ON (ee.state_id = s.state_id)
	WHERE	(ee.ResourceId = @employeeResourceId) AND (aca_er.ResourceId = @employerResourceId)

END
GO
