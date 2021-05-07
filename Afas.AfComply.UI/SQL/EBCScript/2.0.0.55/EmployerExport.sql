USE aca
GO
CREATE PROCEDURE dbo.Employer_Export_Info
AS
BEGIN
	SELECT
		er.[employer_id],
		er.[name],
		er.[DBAName],
		er.[address],
		er.[city],
		s.abbreviation,
		er.[zip],
		er.[ein],
		er.[ResourceId],
		er.[IrsEnabled]
	FROM
		[aca].[dbo].[employer] er
		INNER JOIN [aca].[dbo].[state] s ON (s.state_id = er.state_id)
	ORDER BY
		er.employer_id
END
GO
GRANT EXECUTE ON [dbo].[Employer_Export_Info] TO [aca-user] AS [dbo]
GO