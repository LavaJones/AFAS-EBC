USE aca
GO
CREATE NONCLUSTERED INDEX IDX_EntityStatus
 ON dbo.insurance_coverage ([EntityStatusId])
 INCLUDE([tax_year], [employee_id], [dependent_id], [jan], [feb], [mar], [apr], [may], [jun], [jul], [aug], [sep], [oct], [nov], [dec]
)
GO
CREATE NONCLUSTERED INDEX IDX_EntityStatusEmployer
 ON dbo.UserEditPart2 ([EntityStatusId], [EmployerId])
 INCLUDE([MonthId], [LineId], [EmployeeId]
)
GO
CREATE NONCLUSTERED INDEX IDX_Employer
 ON dbo.payroll ([employer_id])
 INCLUDE([row_id], [batch_id], [employee_id], [gp_id], [act_hours], [sdate], [edate], [cdate], [modBy], [modOn], [history]

)
GO
CREATE NONCLUSTERED INDEX IDX_Employer
 ON dbo.ArchiveFileInfo ([EmployerId])
 INCLUDE([ArchiveFileInfoId], [EmployerGuid], [ArchivedTime], [FileName], [SourceFilePath], [ArchiveFilePath], [ArchiveReason], [ResourceId], [EntityStatusId], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]


)
GO
CREATE NONCLUSTERED INDEX IDX_Employee
 ON dbo.UserEditPart2 ([EmployeeId])
 INCLUDE([UserEditPart2Id], [MonthId], [LineId]
)
GO
CREATE NONCLUSTERED INDEX IDX_Employee
 ON [aca].[dbo].[Approved1095FinalPart2] ([EmployeeId])
GO
