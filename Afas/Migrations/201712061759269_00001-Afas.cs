namespace Afas.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _00001Afas : DbMigration
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ArchiveFileInfo");
        }
    }
}
