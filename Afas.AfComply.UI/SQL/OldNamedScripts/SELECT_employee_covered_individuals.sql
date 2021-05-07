USE aca
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
ALTER PROCEDURE SELECT_employee_covered_individuals
	-- Add the parameters for the stored procedure here
	@employeeId int,
	@taxYearId int
AS
BEGIN TRY
	
	DECLARE @EmployeeCoveredIndividuals TABLE (
		employee_id int NOT NULL,
		covered_individual_id int NOT NULL,
		PersonFirstNm varchar(max),
		PersonMiddleNm varchar(max),
		PersonLastNm varchar(max),
		SuffixNm varchar(max),
		PersonNameControlTxt varchar(max),
		SSN varchar(max),
		DOB date,
		BirthDt varchar(max),
		CoveredIndividualAnnualInd varchar(1),
		JanuaryInd varchar(1),
		FebruaryInd varchar(1),
		MarchInd varchar(1),
		AprilInd varchar(1),
		MayInd varchar(1),
		JuneInd varchar(1),
		JulyInd varchar(1),
		AugustInd varchar(1),
		SeptemberInd varchar(1),
		OctoberInd varchar(1),
		NovemberInd varchar(1),
		DecemberInd varchar(1)
	)

	INSERT INTO @EmployeeCoveredIndividuals(employee_id,covered_individual_id,PersonFirstNm,PersonMiddleNm,PersonLastNm,SuffixNm,PersonNameControlTxt,SSN,DOB,BirthDt,CoveredIndividualAnnualInd)
	SELECT
		ci.employee_id,
		ci.covered_individual_id,
		ci.first_name,
		ci.middle_name,
		ci.last_name,
		ci.name_suffix,
		ci.control_name,
		ci.ssn,
		ci.birth_date,
		ci.birth_date,
		null	-- hard coded to null since the eyd table does not seem to update correctly.
	FROM air.emp.employee a INNER JOIN air.appr.employee_yearly_detail e ON a.employee_id = e.employee_id
							INNER JOIN air.emp.covered_individual ci ON ci.employee_id = e.employee_id
							INNER JOIN (SELECT 
											covered_individual_id,
											SUM(CASE WHEN covered_indicator = 1 THEN 1 ELSE 0 END) AS total_covered_indicator
										FROM air.emp.covered_individual_monthly_detail
										GROUP BY covered_individual_id
										) cim ON cim.covered_individual_id = ci.covered_individual_id
										
	WHERE e.employee_id = @employeeId
	AND e.year_id = @taxYearId
	AND e._1095C = 1
	AND cim.total_covered_indicator > 0

	-- work around the faulty annual coverage indicator.
	UPDATE @EmployeeCoveredIndividuals
	SET
		CoveredIndividualAnnualInd = '1'
	FROM
		@EmployeeCoveredIndividuals ed
		INNER JOIN (SELECT
						covered_individual_id,
						SUM(CASE WHEN covered_indicator = 1 THEN 1 ELSE 0 END) AS total_covered_indicator
					FROM air.emp.covered_individual_monthly_detail cimd
					GROUP BY cimd.covered_individual_id
				  ) cim ON (cim.covered_individual_id = ed.covered_individual_id)
	WHERE cim.total_covered_indicator = 12;

	--January values--
	UPDATE @EmployeeCoveredIndividuals
	SET JanuaryInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '0' ELSE i.Jan END
	FROM @EmployeeCoveredIndividuals ed INNER JOIN(
	SELECT
		ci.covered_individual_id,
		ci.employee_id, 
		CAST(cim.covered_indicator AS varchar(1)) AS Jan
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 1
	) i ON i.covered_individual_id = ed.covered_individual_id
	WHERE ed.employee_id = i.employee_id

	--Feburary values--
	UPDATE @EmployeeCoveredIndividuals
	SET FebruaryInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '0' ELSE i.Feb END
	FROM @EmployeeCoveredIndividuals ed INNER JOIN(
	SELECT
		ci.covered_individual_id,
		ci.employee_id, 
		CAST(cim.covered_indicator AS varchar(1)) AS Feb
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 2
	) i ON i.covered_individual_id = ed.covered_individual_id
	WHERE ed.employee_id = i.employee_id
	
	--March values--
	UPDATE @EmployeeCoveredIndividuals
	SET MarchInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '0' ELSE i.Mar END
	FROM @EmployeeCoveredIndividuals ed INNER JOIN(
	SELECT
		ci.covered_individual_id,
		ci.employee_id, 
		CAST(cim.covered_indicator AS varchar(1)) AS Mar
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 3
	) i ON i.covered_individual_id = ed.covered_individual_id
	WHERE ed.employee_id = i.employee_id

	--April values--
	UPDATE @EmployeeCoveredIndividuals
	SET AprilInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '0' ELSE i.Apr END
	FROM @EmployeeCoveredIndividuals ed INNER JOIN(
	SELECT
		ci.covered_individual_id,
		ci.employee_id, 
		CAST(cim.covered_indicator AS varchar(1)) AS Apr
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 4
	) i ON i.covered_individual_id = ed.covered_individual_id
	WHERE ed.employee_id = i.employee_id

	--May values--
	UPDATE @EmployeeCoveredIndividuals
	SET MayInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '0' ELSE i.May END
	FROM @EmployeeCoveredIndividuals ed INNER JOIN(
	SELECT
		ci.covered_individual_id,
		ci.employee_id, 
		CAST(cim.covered_indicator AS varchar(1)) AS May
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 5
	) i ON i.covered_individual_id = ed.covered_individual_id
	WHERE ed.employee_id = i.employee_id

	--Jun values--
	UPDATE @EmployeeCoveredIndividuals
	SET JuneInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '0' ELSE i.Jun END
	FROM @EmployeeCoveredIndividuals ed INNER JOIN(
	SELECT
		ci.covered_individual_id,
		ci.employee_id, 
		CAST(cim.covered_indicator AS varchar(1)) AS Jun
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 6
	) i ON i.covered_individual_id = ed.covered_individual_id
	WHERE ed.employee_id = i.employee_id

	--Jul values--
	UPDATE @EmployeeCoveredIndividuals
	SET JulyInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '0' ELSE i.Jul END
	FROM @EmployeeCoveredIndividuals ed INNER JOIN(
	SELECT
		ci.covered_individual_id,
		ci.employee_id, 
		CAST(cim.covered_indicator AS varchar(1)) AS Jul
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 7
	) i ON i.covered_individual_id = ed.covered_individual_id
	WHERE ed.employee_id = i.employee_id

	--Aug values--
	UPDATE @EmployeeCoveredIndividuals
	SET AugustInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '0' ELSE i.Aug END
	FROM @EmployeeCoveredIndividuals ed INNER JOIN(
	SELECT
		ci.covered_individual_id,
		ci.employee_id, 
		CAST(cim.covered_indicator AS varchar(1)) AS Aug
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 8
	) i ON i.covered_individual_id = ed.covered_individual_id
	WHERE ed.employee_id = i.employee_id

	--Sep values--
	UPDATE @EmployeeCoveredIndividuals
	SET SeptemberInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '0' ELSE i.Sep END
	FROM @EmployeeCoveredIndividuals ed INNER JOIN(
	SELECT
		ci.covered_individual_id,
		ci.employee_id, 
		CAST(cim.covered_indicator AS varchar(1)) AS Sep
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 9
	) i ON i.covered_individual_id = ed.covered_individual_id
	WHERE ed.employee_id = i.employee_id

	--Oct values--
	UPDATE @EmployeeCoveredIndividuals
	SET OctoberInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '0' ELSE i.Oct END
	FROM @EmployeeCoveredIndividuals ed INNER JOIN(
	SELECT
		ci.covered_individual_id,
		ci.employee_id, 
		CAST(cim.covered_indicator AS varchar(1)) AS Oct
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 10
	) i ON i.covered_individual_id = ed.covered_individual_id
	WHERE ed.employee_id = i.employee_id

	--Nov values--
	UPDATE @EmployeeCoveredIndividuals
	SET NovemberInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '0' ELSE i.Nov END
	FROM @EmployeeCoveredIndividuals ed INNER JOIN(
	SELECT
		ci.covered_individual_id,
		ci.employee_id, 
		CAST(cim.covered_indicator AS varchar(1)) AS Nov
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 11
	) i ON i.covered_individual_id = ed.covered_individual_id
	WHERE ed.employee_id = i.employee_id

	--Dec values--
	UPDATE @EmployeeCoveredIndividuals
	SET DecemberInd = CASE WHEN ed.CoveredIndividualAnnualInd = '1' THEN '0' ELSE i.[Dec] END
	FROM @EmployeeCoveredIndividuals ed INNER JOIN(
	SELECT
		ci.covered_individual_id,
		ci.employee_id, 
		CAST(cim.covered_indicator AS varchar(1)) AS [Dec]
	FROM air.emp.covered_individual ci INNER JOIN air.emp.covered_individual_monthly_detail cim ON ci.covered_individual_id = cim.covered_individual_id
									   INNER JOIN air.gen.time_frame b ON b.time_frame_id = cim.time_frame_id
									   INNER JOIN air.gen.[month] m ON m.month_id = b.month_id
	WHERE ci.employee_id = @employeeId AND b.year_id = @taxYearId AND m.month_id = 12
	) i ON i.covered_individual_id = ed.covered_individual_id
	WHERE ed.employee_id = i.employee_id

	SELECT * FROM @EmployeeCoveredIndividuals

END TRY
BEGIN CATCH
	EXEC INSERT_ErrorLogging
END CATCH

