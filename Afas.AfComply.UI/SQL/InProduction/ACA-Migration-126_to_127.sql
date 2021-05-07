USE [aca]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Obiye, Kolokolo
-- Create date: 04/26/2017
-- Description:	inserts or updates an employee covered monthly detail
-- =============================================
CREATE PROCEDURE INSERT_UPDATE_covered_individual_monthly_detail 
(
	-- Add the parameters for the function here
	@coveredIndividualID INT,
	@employeeID INT,
	@employerID INT,
	@taxYear INT,
	@month INT,
	@coveredIndicator INT
)
AS
BEGIN TRY

	DECLARE @cim_id INT = 0;
	
	SELECT @cim_id = cim.cim_id
	FROM [air].[emp].[covered_individual_monthly_detail] cim 
		INNER JOIN [air].[emp].[covered_individual] ci ON cim.covered_individual_id = ci.covered_individual_id
		INNER JOIN [air].[gen].[time_frame] tf ON tf.time_frame_id = cim.time_frame_id
	WHERE ci.employee_id = @employeeID
		AND ci.employer_id = @employerID
		AND ci.covered_individual_id = @coveredIndividualID
		AND tf.year_id = @taxYear
		AND tf.month_id = @month
	
	IF @cim_id = 0 
	BEGIN

		INSERT [air].[emp].[covered_individual_monthly_detail](covered_individual_id,covered_indicator,time_frame_id)
		SELECT 
			@coveredIndividualID,
			@coveredIndicator,
			time_frame_id
		FROM [air].[gen].[time_frame] tf
		WHERE tf.month_id = @month
		AND tf.year_id = @taxYear

	END
	ELSE
	BEGIN

		UPDATE [air].[emp].[covered_individual_monthly_detail]
		SET covered_indicator = @coveredIndicator
		WHERE cim_id = @cim_id

	END

END TRY
BEGIN CATCH

	EXEC [dbo].[INSERT_ErrorLogging]

END CATCH
GO

GRANT EXECUTE ON [dbo].[INSERT_UPDATE_covered_individual_monthly_detail] TO [aca-user] AS [dbo]
GO
