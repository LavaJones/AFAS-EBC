USE [aca]
GO

/****** Object:  Trigger [dbo].[Approved1095FinalPart3Trigger]    Script Date: 2/12/2018 2:35:50 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER TRIGGER [dbo].[Approved1095FinalPart3Trigger] ON [dbo].[Approved1095FinalPart3]
   INSTEAD OF INSERT, UPDATE
AS 
BEGIN

	IF  EXISTS  (   SELECT U.[Approved1095FinalPart3Id] FROM [dbo].[Approved1095FinalPart3]  U
					INNER JOIN Inserted I 
						ON u.Approved1095FinalPart3Id = I.Approved1095FinalPart3Id
					)
		BEGIN             
       
			UPDATE  
				CurrentValues
			SET                  
				CurrentValues.[EntityStatusId] = NewValues.[EntityStatusId],
				CurrentValues.[ModifiedBy] = NewValues.[ModifiedBy],
				CurrentValues.[ModifiedDate] = NewValues.[ModifiedDate]                
			FROM    
				[dbo].[Approved1095FinalPart3] CurrentValues, 
				Inserted NewValues 
			WHERE  
				CurrentValues.[Approved1095FinalPart3Id] = NewValues.[Approved1095FinalPart3Id]

		END
    Else 
        BEGIN

			INSERT INTO 
				[dbo].[Approved1095FinalPart3]
			SELECT 
				[InsuranceCoverageRowID]
				,[EmployeeID]
				,[DependantID]
				,[TaxYear]
				,[FirstName]
				,[MiddleName]
				,[LastName]
				,[SSN]
				,[Dob]
				,[ResourceId]
				,[EntityStatusId]
				,[CreatedBy]
				,[CreatedDate]
				,[ModifiedBy]
				,[ModifiedDate]
				,[Approved1095Final_ID]
				,[EnrolledJan]
				,[EnrolledFeb]
				,[EnrolledMar]
				,[EnrolledApr]
				,[EnrolledMay]
				,[EnrolledJun]
				,[EnrolledJul]
				,[EnrolledAug]
				,[EnrolledSep]
				,[EnrolledOct]
				,[EnrolledNov]
				,[EnrolledDec] 
			FROM
				Inserted
              
       END
END

GO

GO

ALTER TRIGGER [dbo].[Approved1095FinalPart2Trigger] ON [dbo].[Approved1095FinalPart2]
   INSTEAD OF INSERT, UPDATE
AS 
BEGIN

    IF  EXISTS  (   SELECT U.[Approved1095FinalPart2Id] FROM [dbo].[Approved1095FinalPart2]  U
                    INNER JOIN Inserted I 
                        ON u.Approved1095FinalPart2Id = I.Approved1095FinalPart2Id
                    )
		BEGIN             
       
			UPDATE  
				CurrentValues
			SET                  
				CurrentValues.[EntityStatusId] = NewValues.[EntityStatusId],
				CurrentValues.[ModifiedBy] = NewValues.[ModifiedBy],
				CurrentValues.[ModifiedDate] = NewValues.[ModifiedDate]                
			FROM    
				[dbo].[Approved1095FinalPart2] CurrentValues, 
				Inserted NewValues 
			WHERE  
				CurrentValues.[Approved1095FinalPart2Id] = NewValues.[Approved1095FinalPart2Id]

		END
    Else 
       BEGIN             

			INSERT INTO 
				[dbo].[Approved1095FinalPart2]
			SELECT 
				[employeeID]
				,[TaxYear]
				,[MonthId]
				,[Line14]
				,[Line15]
				,[Line16]
				,[ResourceId]
				,[EntityStatusId]
				,[CreatedBy]
				,[CreatedDate]
				,[ModifiedBy]
				,[ModifiedDate]
				,[Approved1095Final_ID]
				,[Offered]
				,[Receiving1095C] 
			FROM 
				Inserted

       END
END

GO

GO

ALTER TRIGGER [dbo].[Approved1095FinalTrigger] ON [dbo].[Approved1095Final]
   INSTEAD OF INSERT, UPDATE
AS 
BEGIN

    IF  EXISTS  (   SELECT U.Approved1095FinalId FROM [dbo].[Approved1095Final]  U
                    INNER JOIN Inserted I 
                        ON u.Approved1095FinalId = I.Approved1095FinalId
                    )
		BEGIN             
       
			UPDATE  
				CurrentValues
			SET                  
				CurrentValues.[EntityStatusId] = NewValues.[EntityStatusId],
				CurrentValues.[ModifiedBy] = NewValues.[ModifiedBy],
				CurrentValues.[ModifiedDate] = NewValues.[ModifiedDate]                
			FROM    
				[dbo].[Approved1095Final] CurrentValues, 
				Inserted NewValues 
			WHERE  
				CurrentValues.Approved1095FinalId = NewValues.Approved1095FinalId

		END
    Else 
       BEGIN
              
			INSERT INTO 
				[dbo].[Approved1095Final]
			SELECT 
				[employerID]
				,[employeeID]
				,[ResourceId]
				,[EntityStatusId]
				,[CreatedBy]
				,[CreatedDate]
				,[ModifiedBy]
				,[ModifiedDate]
				,[FirstName]
				,[MiddleName]
				,[LastName]
				,[SSN]
				,[StreetAddress]
				,[City]
				,[State]
				,[Zip]
				,[EmployeeResourceId]
				,[SelfInsured]
				,[TaxYear] 
			FROM 
				Inserted

       END
END

GO
GRANT ALTER ON [dbo].[Approved1095FinalPart3] TO [aca-user]
GO
GRANT ALTER ON [dbo].[Approved1095FinalPart2] TO [aca-user]
GO
GRANT ALTER ON [dbo].[Approved1095Final] TO [aca-user]
GO
GO
GRANT UPDATE ON [dbo].[Approved1095FinalPart3] TO [aca-user]
GO
GRANT UPDATE ON [dbo].[Approved1095FinalPart2] TO [aca-user]
GO
GRANT UPDATE ON [dbo].[Approved1095Final] TO [aca-user]
GO
GO
GRANT INSERT ON [dbo].[Approved1095FinalPart3] TO [aca-user]
GO
GRANT INSERT ON [dbo].[Approved1095FinalPart2] TO [aca-user]
GO
GRANT INSERT ON [dbo].[Approved1095Final] TO [aca-user]
GO
GRANT SELECT ON [dbo].[Approved1095FinalPart3] TO [aca-user]
GO
GRANT SELECT ON [dbo].[Approved1095FinalPart2] TO [aca-user]
GO
GRANT SELECT ON [dbo].[Approved1095Final] TO [aca-user]
GO


