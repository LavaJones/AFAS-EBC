USE [aca]
GO
/****** Object:  StoredProcedure [dbo].[Pull_1094C_XML_File_Path]    Script Date: 10/23/2017 12:54:27 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:          Long Vu
-- Create date:		10/23/2017
-- Description:    Simple file exporter.
-- =============================================
ALTER PROCEDURE [dbo].[Pull_1094C_XML_File_Path]
       @EmployerId INT
AS
BEGIN
       -- SET NOCOUNT ON added to prevent extra result sets from
       -- interfering with SELECT statements.
       SET NOCOUNT ON;

    SELECT TOP 1 
		man.document_system_file_name, 
		SUBSTRING(manifest_file_name,0,LEN(LEFT(manifest_file_name, CHARINDEX('/', manifest_file_name)))) + '/' + document_system_file_name AS manifest_file_name 
	FROM air.br.manifest man INNER JOIN aca.dbo.employer emp 
		ON SUBSTRING(emp.ein,1,2) + SUBSTRING(emp.ein,4,10) = man.ein
    WHERE emp.employer_id = @EmployerId
    ORDER BY man.header_id DESC
END
GO
GRANT EXECUTE ON [dbo].[Pull_1094C_XML_File_Path] TO [aca-user] AS [dbo]
GO