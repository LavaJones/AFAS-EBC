Use aca
GO
UPDATE dbo.UserEditPart2 SET EntityStatusId = 2
GO
UPDATE dbo.UserEditPart2 SET EntityStatusId = 1 WHERE UserEditPart2Id IN(SELECT MAX(UserEditPart2Id) UEID FROM dbo.UserEditPart2 GROUP BY EmployeeId, MonthId, LineId) 
