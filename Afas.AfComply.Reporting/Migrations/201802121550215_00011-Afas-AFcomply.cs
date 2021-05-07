namespace Afas.AfComply.Reporting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _00011AfasAFcomply : DbMigration
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
            
            AddColumn("dbo.PrintBatches", "PrintFileName", c => c.String(nullable: false));
            AddColumn("dbo.PrintBatches", "EmployerId", c => c.Int(nullable: false));
            AddColumn("dbo.PrintBatches", "TaxYear", c => c.Int(nullable: false));
            AddColumn("dbo.PrintBatches", "AfasRequested", c => c.Boolean(nullable: false));
            AddColumn("dbo.PrintBatches", "ArchivedFile_ArchiveFileInfoId", c => c.Long());
            CreateIndex("dbo.PrintBatches", "ArchivedFile_ArchiveFileInfoId");
            AddForeignKey("dbo.PrintBatches", "ArchivedFile_ArchiveFileInfoId", "dbo.ArchiveFileInfo", "ArchiveFileInfoId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PrintBatches", "ArchivedFile_ArchiveFileInfoId", "dbo.ArchiveFileInfo");
            DropIndex("dbo.PrintBatches", new[] { "ArchivedFile_ArchiveFileInfoId" });
            DropColumn("dbo.PrintBatches", "ArchivedFile_ArchiveFileInfoId");
            DropColumn("dbo.PrintBatches", "AfasRequested");
            DropColumn("dbo.PrintBatches", "TaxYear");
            DropColumn("dbo.PrintBatches", "EmployerId");
            DropColumn("dbo.PrintBatches", "PrintFileName");
            DropTable("dbo.ArchiveFileInfo");
        }
    }
}
