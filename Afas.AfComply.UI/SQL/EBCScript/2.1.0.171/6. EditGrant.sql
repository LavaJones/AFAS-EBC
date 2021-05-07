USE aca
GO
GRANT SELECT ON dbo.View_full_insurance_coverage TO [aca-user]
GO
GRANT SELECT ON [dbo].[Approved1095Final] TO [aca-user]
GO
GRANT SELECT ON [dbo].[Approved1095Initial] TO [aca-user]
GO
GRANT SELECT ON [dbo].[UserEditPart2] TO [aca-user]
GO
GRANT INSERT ON [dbo].[Approved1095Final] TO [aca-user]
GO
GRANT INSERT ON [dbo].[Approved1095Initial] TO [aca-user]
GO
GRANT INSERT ON [dbo].[UserEditPart2] TO [aca-user]
GO
GRANT UPDATE ON [dbo].[Approved1095Final] TO [aca-user]
GO
GRANT UPDATE ON [dbo].[Approved1095Initial] TO [aca-user]
GO
GRANT UPDATE ON [dbo].[UserEditPart2] TO [aca-user]