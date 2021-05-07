USE [aca]
GO

/****** Object:  Trigger [UserEditTrigger]    Script Date: 2/7/2018 11:28:43 AM ******/
DROP TRIGGER [dbo].[UserEditTrigger]
GO

/****** Object:  Trigger [dbo].[UserEditTrigger]    Script Date: 2/7/2018 11:28:43 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[UserEditTrigger] ON [dbo].[UserEditPart2]
   INSTEAD OF INSERT, UPDATE
AS 
BEGIN

        IF  EXISTS  (   SELECT U.[UserEditPart2Id] FROM [dbo].[UserEditPart2]  U
                        INNER JOIN Inserted I 
                         ON u.UserEditPart2ID = I.UserEditPart2ID 
                     )

    BEGIN             
       
        UPDATE  
                      CurrentValues
        SET                  
                      CurrentValues.[EntityStatusId] = NewValues.[EntityStatusId],
                      CurrentValues.[ModifiedBy] = NewValues.[ModifiedBy],
                      CurrentValues.[ModifiedDate] = NewValues.[ModifiedDate]                
        FROM    
                      [dbo].[UserEditPart2] CurrentValues, 
                      Inserted NewValues 
        WHERE  
                      CurrentValues.[UserEditPart2Id] = NewValues.[UserEditPart2Id]

    END
    Else 
       BEGIN
              IF (EXISTS(SELECT CurrentValues.[UserEditPart2Id] FROM [dbo].[UserEditPart2] CurrentValues INNER JOIN Inserted NewValues 
              ON
                      CurrentValues.[MonthId] = NewValues.[MonthId] 
                      AND CurrentValues.[LineId] = NewValues.[LineId]
                      AND CurrentValues.[EmployeeId] = NewValues.[EmployeeId]
                      AND CurrentValues.[TaxYear] = NewValues.[TaxYear]
                      AND CurrentValues.[EntityStatusId] = 1 ))
              BEGIN
       
                      UPDATE  
                             CurrentValues
                      SET                   
                             CurrentValues.[EntityStatusId] = NewValues.[EntityStatusId],
                             CurrentValues.[ModifiedBy] = NewValues.[ModifiedBy],
                             CurrentValues.[ModifiedDate] = NewValues.[ModifiedDate]                
                      FROM    
                             [dbo].[UserEditPart2] CurrentValues, 
                             Inserted NewValues 
                      WHERE   
                                 CurrentValues.[MonthId] = NewValues.[MonthId] 
                             AND CurrentValues.[LineId] = NewValues.[LineId]
                             AND CurrentValues.[EmployeeId] = NewValues.[EmployeeId]
                             AND CurrentValues.[TaxYear] = NewValues.[TaxYear]
                             AND CurrentValues.[EntityStatusId] = 1

              END
              ELSE BEGIN

                      INSERT INTO 
                             [dbo].[UserEditPart2]
                      SELECT 
                         [OldValue]
                        ,[NewValue]
                        ,[MonthId]
                        ,[LineId]
                        ,[EmployeeId]
                        ,[EmployerId]
                        ,[ResourceId]
                        ,[EntityStatusId]
                        ,[CreatedBy]
                        ,[CreatedDate]
                        ,[ModifiedBy]
                        ,[ModifiedDate]
                        ,[TaxYear] FROM Inserted

              END
       END
END

GO


