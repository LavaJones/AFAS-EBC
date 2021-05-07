
USE [aca]
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Ryan McCully
-- Create date: 10/29/2019
-- Description:	Used to enable SQLBulkInsertOrUpdate
-- =============================================
CREATE TRIGGER [dbo].[UserReviewedTrigger]
   ON [dbo].[UserRevieweds]
   INSTEAD OF INSERT, UPDATE
AS 
BEGIN


	-- SET NOCOUNT ON added to prevent extra result sets from 
	-- interfering with SELECT statements.
	SET NOCOUNT ON;--This is added by default, so I left it


	IF  EXISTS(
			SELECT Existing.[UserReviewedId] FROM [dbo].[UserRevieweds] Existing
			INNER JOIN Inserted I 
			ON Existing.[UserReviewedId] = I.[UserReviewedId]
			)
		BEGIN    
			UPDATE  
						  CurrentValues
			SET
						  CurrentValues.[EntityStatusId] = NewValues.[EntityStatusId],
						  CurrentValues.[ModifiedBy] = NewValues.[ModifiedBy],
						  CurrentValues.[ModifiedDate] = NewValues.[ModifiedDate]                
			FROM    
						  [dbo].[UserRevieweds] CurrentValues, 
						  Inserted NewValues 
			WHERE  
						  CurrentValues.[UserReviewedId] = NewValues.[UserReviewedId]
		END
    ELSE 
    BEGIN
	   	 	
		INSERT INTO 
			[dbo].[UserRevieweds]
		SELECT 
			[TaxYear]
			,[EmployeeId]
			,[EmployerId]
			,[ReviewedBy]
			,[ReviewedOn]
			,[ResourceId]
			,[EntityStatusId]
			,[CreatedBy]
			,[CreatedDate]
			,[ModifiedBy]
			,[ModifiedDate]
		FROM 
		Inserted

	END

END
GO

ALTER TABLE [dbo].[UserRevieweds] ENABLE TRIGGER [UserReviewedTrigger]
GO

