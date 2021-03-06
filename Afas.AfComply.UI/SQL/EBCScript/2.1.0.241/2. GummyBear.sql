USE [aca]
GO
/****** Object:  Trigger [dbo].[Printed1095Trigger]    Script Date: 2/14/2018 4:32:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[Printed1095Trigger] ON [dbo].[Print1095]
   INSTEAD OF INSERT, UPDATE
AS 
BEGIN

    IF  EXISTS  (   SELECT U.[Print1095Id] FROM [dbo].[Print1095]  U
                    INNER JOIN Inserted I 
                        ON u.[Print1095Id] = I.[Print1095Id]
                    )
		BEGIN             
       
			UPDATE  
				CurrentValues
			SET                  
				CurrentValues.[OutputFilePath] = NewValues.[OutputFilePath],
				CurrentValues.[ModifiedBy] = NewValues.[ModifiedBy],
				CurrentValues.[ModifiedDate] = NewValues.[ModifiedDate]                
			FROM    
				[dbo].[Print1095] CurrentValues, 
				Inserted NewValues 
			WHERE  
				CurrentValues.[Print1095Id] = NewValues.[Print1095Id]

		END
    Else 
       BEGIN
              
			INSERT INTO 
				[dbo].[Print1095]
			SELECT 
				 [OutputFilePath]
				,[ResourceId]
				,[EntityStatusId]
				,[CreatedBy]
				,[CreatedDate]
				,[ModifiedBy]
				,[ModifiedDate]
				,[Approved1095_ID]
				,[PrintBatch_ID]
			FROM 
				Inserted

       END
END

