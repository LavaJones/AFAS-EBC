USE [aca]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE SELECT_employee_tax_year_dependents
	@employeeId int,
	@taxYearId int
AS
BEGIN TRY
	
	DECLARE @EmployeeDependents TABLE (
		employee_id int NOT NULL,
		dependent_id int,
		Fname varchar(max),
		Mi varchar(max),
		Lname varchar(max),
		ssn varchar(max),
		dob date,
		Jan bit,
		Feb bit,
		Mar bit,
		Apr bit,
		May bit,
		Jun bit,
		Jul bit,
		Aug bit,
		Sep bit,
		Oct bit,
		Nov bit,
		[Dec] bit
	)

	INSERT INTO @EmployeeDependents(employee_id,dependent_id,Fname,Mi,Lname,ssn,dob)
	SELECT
		ci.employee_id,
		ci.covered_individual_id AS dependent_id,
		ci.first_name AS Fname,
		ci.middle_name AS Mi,
		ci.last_name AS Lname,
		ci.ssn,
		ci.birth_date AS dob
	FROM air.emp.employee a INNER JOIN air.appr.employee_yearly_detail e ON a.employee_id = e.employee_id
							INNER JOIN air.emp.covered_individual ci ON ci.employee_id = e.employee_id
	WHERE e.employee_id = @employeeId
	AND e.year_id = @taxYearId
	AND e._1095C = 1

	--January values--
	UPDATE @EmployeeDependents
	SET Jan = i.Jan
	FROM @EmployeeDependents ed INNER JOIN(
	SELECT
		ci.employee_id,
		ci.covered_individual_id, 
		cim.covered_indicator AS Jan
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 1
	) i ON i.covered_individual_id = ed.dependent_id
	WHERE ed.employee_id = i.employee_id

	--Feburary values--
	UPDATE @EmployeeDependents
	SET Feb = i.Feb
	FROM @EmployeeDependents ed INNER JOIN(
	SELECT
		ci.employee_id, 
		ci.covered_individual_id,
		cim.covered_indicator AS Feb
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 2
	) i ON i.covered_individual_id = ed.dependent_id
	WHERE ed.employee_id = i.employee_id
	
	--March values--
	UPDATE @EmployeeDependents
	SET Mar = i.Mar
	FROM @EmployeeDependents ed INNER JOIN(
	SELECT
		ci.employee_id, 
		ci.covered_individual_id,
		cim.covered_indicator AS Mar
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 3
	) i ON i.covered_individual_id = ed.dependent_id
	WHERE ed.employee_id = i.employee_id

	--April values--
	UPDATE @EmployeeDependents
	SET Apr = i.Apr
	FROM @EmployeeDependents ed INNER JOIN(
	SELECT
		ci.employee_id, 
		ci.covered_individual_id,
		cim.covered_indicator AS Apr
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 4
	) i ON i.covered_individual_id = ed.dependent_id
	WHERE ed.employee_id = i.employee_id

	--May values--
	UPDATE @EmployeeDependents
	SET May = i.May
	FROM @EmployeeDependents ed INNER JOIN(
	SELECT
		ci.employee_id, 
		ci.covered_individual_id,
		cim.covered_indicator AS May
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 5
	) i ON i.covered_individual_id = ed.dependent_id
	WHERE ed.employee_id = i.employee_id

	--Jun values--
	UPDATE @EmployeeDependents
	SET Jun = i.Jun
	FROM @EmployeeDependents ed INNER JOIN(
	SELECT
		ci.employee_id, 
		ci.covered_individual_id,
		cim.covered_indicator AS Jun
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 6
	) i ON i.covered_individual_id = ed.dependent_id
	WHERE ed.employee_id = i.employee_id

	--Jul values--
	UPDATE @EmployeeDependents
	SET Jul = i.Jul
	FROM @EmployeeDependents ed INNER JOIN(
	SELECT
		ci.employee_id, 
		ci.covered_individual_id,
		cim.covered_indicator AS Jul
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 7
	) i ON i.covered_individual_id = ed.dependent_id
	WHERE ed.employee_id = i.employee_id

	--Aug values--
	UPDATE @EmployeeDependents
	SET Aug = i.Aug
	FROM @EmployeeDependents ed INNER JOIN(
	SELECT
		ci.employee_id, 
		ci.covered_individual_id,
		cim.covered_indicator AS Aug
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 8
	) i ON i.covered_individual_id = ed.dependent_id
	WHERE ed.employee_id = i.employee_id

	--Sep values--
	UPDATE @EmployeeDependents
	SET Sep = i.Sep
	FROM @EmployeeDependents ed INNER JOIN(
	SELECT
		ci.employee_id, 
		ci.covered_individual_id,
		cim.covered_indicator AS Sep
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 9
	) i ON i.covered_individual_id = ed.dependent_id
	WHERE ed.employee_id = i.employee_id

	--Oct values--
	UPDATE @EmployeeDependents
	SET Oct = i.Oct
	FROM @EmployeeDependents ed INNER JOIN(
	SELECT
		ci.employee_id, 
		ci.covered_individual_id,
		cim.covered_indicator AS Oct
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 10
	) i ON i.covered_individual_id = ed.dependent_id
	WHERE ed.employee_id = i.employee_id

	--Nov values--
	UPDATE @EmployeeDependents
	SET Nov = i.Nov
	FROM @EmployeeDependents ed INNER JOIN(
	SELECT
		ci.employee_id, 
		ci.covered_individual_id,
		cim.covered_indicator AS Nov
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 11
	) i ON i.covered_individual_id = ed.dependent_id
	WHERE ed.employee_id = i.employee_id

	--Dec values--
	UPDATE @EmployeeDependents
	SET [Dec] = i.[Dec]
	FROM @EmployeeDependents ed INNER JOIN(
	SELECT
		ci.employee_id, 
		ci.covered_individual_id,
		cim.covered_indicator AS [Dec]
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 12
	) i ON i.covered_individual_id = ed.dependent_id
	WHERE ed.employee_id = i.employee_id

	SELECT * FROM @EmployeeDependents

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH
GO