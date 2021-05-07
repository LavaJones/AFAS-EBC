namespace Afas.AfComply.Reporting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _00016AfasAFcomply : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FileCabinetAccess",
                c => new
                    {
                        FileCabinetAccessID = c.Long(nullable: false, identity: true),
                        OwnerResourceId = c.Guid(nullable: false),
                        HasFiles = c.Boolean(nullable: false),
                        HasSubFolders = c.Boolean(nullable: false),
                        ApplicationId = c.Int(nullable: false),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                        FileCabinetFolderInfo_ID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.FileCabinetAccessID)
                .ForeignKey("dbo.FileCabinetFolderInfo", t => t.FileCabinetFolderInfo_ID, cascadeDelete: true)
                .Index(t => t.FileCabinetFolderInfo_ID);
            
            CreateTable(
                "dbo.FileCabinetFolderInfo",
                c => new
                    {
                        FileCabinetFolderInfoID = c.Long(nullable: false, identity: true),
                        FolderName = c.String(nullable: false),
                        FolderDepth = c.Int(nullable: false),
                        ApplicationId = c.Int(nullable: false),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                        ParentFolderId = c.Long(),
                    })
                .PrimaryKey(t => t.FileCabinetFolderInfoID)
                .ForeignKey("dbo.FileCabinetFolderInfo", t => t.ParentFolderId)
                .Index(t => t.ParentFolderId);
            
            CreateTable(
                "dbo.FileCabinetInfo",
                c => new
                    {
                        FileCabinetInfoID = c.Long(nullable: false, identity: true),
                        Filename = c.String(nullable: false, maxLength: 256),
                        FileDescription = c.String(maxLength: 1000),
                        FileType = c.String(),
                        OwnerResourceId = c.Guid(nullable: false),
                        ApplicationId = c.Int(nullable: false),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                        ArchiveFileInfo_ArchiveFileInfoId = c.Long(nullable: false),
                        FileCabinetFolderInfo_ID = c.Long(),
                    })
                .PrimaryKey(t => t.FileCabinetInfoID)
                .ForeignKey("dbo.ArchiveFileInfo", t => t.ArchiveFileInfo_ArchiveFileInfoId, cascadeDelete: true)
                .ForeignKey("dbo.FileCabinetFolderInfo", t => t.FileCabinetFolderInfo_ID)
                .Index(t => t.ArchiveFileInfo_ArchiveFileInfoId)
                .Index(t => t.FileCabinetFolderInfo_ID);
            
            CreateTable(
                "dbo.SsisFileTransfers",
                c => new
                    {
                        SSISFileTransferId = c.Long(name: " SSISFileTransferId", nullable: false, identity: true),
                        FEIN = c.String(maxLength: 50),
                        FileName = c.String(),
                        RunTime = c.DateTime(nullable: false),
                        EmployerId = c.Int(nullable: false),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SSISFileTransferId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FileCabinetInfo", "FileCabinetFolderInfo_ID", "dbo.FileCabinetFolderInfo");
            DropForeignKey("dbo.FileCabinetInfo", "ArchiveFileInfo_ArchiveFileInfoId", "dbo.ArchiveFileInfo");
            DropForeignKey("dbo.FileCabinetAccess", "FileCabinetFolderInfo_ID", "dbo.FileCabinetFolderInfo");
            DropForeignKey("dbo.FileCabinetFolderInfo", "ParentFolderId", "dbo.FileCabinetFolderInfo");
            DropIndex("dbo.FileCabinetInfo", new[] { "FileCabinetFolderInfo_ID" });
            DropIndex("dbo.FileCabinetInfo", new[] { "ArchiveFileInfo_ArchiveFileInfoId" });
            DropIndex("dbo.FileCabinetAccess", new[] { "FileCabinetFolderInfo_ID" });
            DropIndex("dbo.FileCabinetFolderInfo", new[] { "ParentFolderId" });
            DropTable("dbo.SsisFileTransfers");
            DropTable("dbo.FileCabinetInfo");
            DropTable("dbo.FileCabinetFolderInfo");
            DropTable("dbo.FileCabinetAccess");
        }
    }
}
