namespace Afas.ImportConverter.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _00001AfasImportConverter : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArchiveFileInfo",
                c => new
                    {
                        ArchiveFileInfoId = c.Long(nullable: false, identity: true),
                        EmployerId = c.Int(nullable: false),
                        EmployerGuid = c.Guid(nullable: false),
                        ArchivedTime = c.DateTime(nullable: false),
                        FileName = c.String(maxLength: 256),
                        SourceFilePath = c.String(maxLength: 256),
                        ArchiveFilePath = c.String(maxLength: 256),
                        ArchiveReason = c.String(maxLength: 256),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.ArchiveFileInfoId);
            
            CreateTable(
                "dbo.ImportData",
                c => new
                    {
                        ImportDataId = c.Long(nullable: false, identity: true),
                        Data = c.String(storeType: "xml"),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                        MetaData_ID = c.Long(),
                    })
                .PrimaryKey(t => t.ImportDataId)
                .ForeignKey("dbo.ImportMetaData", t => t.MetaData_ID)
                .Index(t => t.MetaData_ID);
            
            CreateTable(
                "dbo.ImportMetaData",
                c => new
                    {
                        ImportMetaDataId = c.Long(nullable: false, identity: true),
                        UploadSourceDescription = c.String(),
                        UploadTypeDescription = c.String(),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ImportMetaDataId);
            
            CreateTable(
                "dbo.ImportFormatCommandScope",
                c => new
                    {
                        ImportFormatCommandScopeId = c.Long(nullable: false, identity: true),
                        Headers = c.Boolean(nullable: false),
                        InputSource = c.Boolean(nullable: false),
                        UploadType = c.Boolean(nullable: false),
                        Global = c.Boolean(nullable: false),
                        None = c.Boolean(nullable: false),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ImportFormatCommandScopeId);
            
            CreateTable(
                "dbo.ImportFormatCommands",
                c => new
                    {
                        ImportFormatCommandId = c.Long(nullable: false, identity: true),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        MetaData_ID = c.Long(),
                        Scope_ID = c.Long(),
                    })
                .PrimaryKey(t => t.ImportFormatCommandId)
                .ForeignKey("dbo.ImportMetaData", t => t.MetaData_ID)
                .ForeignKey("dbo.ImportFormatCommandScope", t => t.Scope_ID)
                .Index(t => t.MetaData_ID)
                .Index(t => t.Scope_ID);
            
            CreateTable(
                "dbo.StagingImport",
                c => new
                    {
                        StagingImportId = c.Long(nullable: false, identity: true),
                        Original = c.String(nullable: false, storeType: "xml"),
                        Modify = c.String(storeType: "xml"),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UploadedFileInfoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.StagingImportId);
            
            CreateTable(
                "dbo.ImportFileMetaData",
                c => new
                    {
                        ImportFileMetaDataId = c.Long(nullable: false),
                        SourceFilePath = c.String(),
                        FileTypeDescription = c.String(),
                    })
                .PrimaryKey(t => t.ImportFileMetaDataId)
                .ForeignKey("dbo.ImportMetaData", t => t.ImportFileMetaDataId)
                .Index(t => t.ImportFileMetaDataId);
            
            CreateTable(
                "dbo.ImportFormatFileCommandScope",
                c => new
                    {
                        ImportFormatFileCommandScopeId = c.Long(nullable: false),
                        FileType = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ImportFormatFileCommandScopeId)
                .ForeignKey("dbo.ImportFormatCommandScope", t => t.ImportFormatFileCommandScopeId)
                .Index(t => t.ImportFormatFileCommandScopeId);
            
            CreateTable(
                "dbo.UploadedFileInfo",
                c => new
                    {
                        UploadedFileInfoId = c.Long(nullable: false, identity: true),
                        ArchiveFileInfoId = c.Long(),
                        UploadedByUser = c.String(nullable: false, maxLength: 50),
                        UploadTime = c.DateTime(nullable: false),
                        UploadSourceDescription = c.String(nullable: false, maxLength: 50),
                        UploadTypeDescription = c.String(nullable: false, maxLength: 50),
                        Processed = c.Boolean(nullable: false),
                        FailedProcessing = c.Boolean(nullable: false),
                        EmployerId = c.Int(nullable: false),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        FileTypeDescription = c.String(nullable: false, maxLength: 50),
                        FileName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.UploadedFileInfoId)
                .ForeignKey("dbo.ArchiveFileInfo", t => t.ArchiveFileInfoId)
                .Index(t => t.ArchiveFileInfoId);
            
            CreateTable(
                "dbo.UploadedDataInfo",
                c => new
                    {
                        UploadedDataInfoId = c.Long(nullable: false, identity: true),
                        UploadedByUser = c.String(nullable: false, maxLength: 50),
                        UploadTime = c.DateTime(nullable: false),
                        UploadSourceDescription = c.String(nullable: false, maxLength: 50),
                        UploadTypeDescription = c.String(nullable: false, maxLength: 50),
                        Processed = c.Boolean(nullable: false),
                        FailedProcessing = c.Boolean(nullable: false),
                        UploadOwnerId = c.Int(nullable: false),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.UploadedDataInfoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UploadedFileInfo", "ArchiveFileInfoId", "dbo.ArchiveFileInfo");
            DropForeignKey("dbo.ImportFormatFileCommandScope", "ImportFormatFileCommandScopeId", "dbo.ImportFormatCommandScope");
            DropForeignKey("dbo.ImportFileMetaData", "ImportFileMetaDataId", "dbo.ImportMetaData");
            DropForeignKey("dbo.ImportFormatCommands", "Scope_ID", "dbo.ImportFormatCommandScope");
            DropForeignKey("dbo.ImportFormatCommands", "MetaData_ID", "dbo.ImportMetaData");
            DropForeignKey("dbo.ImportData", "MetaData_ID", "dbo.ImportMetaData");
            DropIndex("dbo.UploadedFileInfo", new[] { "ArchiveFileInfoId" });
            DropIndex("dbo.ImportFormatFileCommandScope", new[] { "ImportFormatFileCommandScopeId" });
            DropIndex("dbo.ImportFileMetaData", new[] { "ImportFileMetaDataId" });
            DropIndex("dbo.ImportFormatCommands", new[] { "Scope_ID" });
            DropIndex("dbo.ImportFormatCommands", new[] { "MetaData_ID" });
            DropIndex("dbo.ImportData", new[] { "MetaData_ID" });
            DropTable("dbo.UploadedDataInfo");
            DropTable("dbo.UploadedFileInfo");
            DropTable("dbo.ImportFormatFileCommandScope");
            DropTable("dbo.ImportFileMetaData");
            DropTable("dbo.StagingImport");
            DropTable("dbo.ImportFormatCommands");
            DropTable("dbo.ImportFormatCommandScope");
            DropTable("dbo.ImportMetaData");
            DropTable("dbo.ImportData");
            DropTable("dbo.ArchiveFileInfo");
        }
    }
}
