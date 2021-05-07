/****** Script for SelectTopNRows command from SSMS  ******/
USE aca
GO
UPDATE [aca].[dbo].[tax_year_employer_transmission] set ReceiptId = NULL WHERE UniqueTransmissionId in ('4e743ffc-f214-44f6-9ca2-68cb9215da47:SYS12:BBYN5::T', 'eb832215-93bf-4f57-aa6d-043febd96289:SYS12:BBYN5::T'
, '61c6d75c-6468-4242-9ef1-2b913d3e1568:SYS12:BBYN5::T', '324ba0ec-6749-4575-87cb-d5f014a51ff7:SYS12:BBYN5::T', '0fbe7da9-cac9-46a1-8002-4cd782ac3e4c:SYS12:BBYN5::T'
, '9b1ee974-ee73-488e-934b-06fa3e5a5c29:SYS12:BBYN5::T', '612e5546-1829-49b3-8ee7-a18d2f481d00:SYS12:BBYN5::T', '952b231e-b106-402f-9eb6-fad3cbfc4995:SYS12:BBYN5::T', 'e559808d-2d6d-4de4-a4a9-7f7b73a4a77d:SYS12:BBYN5::T'
, 'eb832215-93bf-4f57-aa6d-043febd96289:SYS12:BBYN5::T')

