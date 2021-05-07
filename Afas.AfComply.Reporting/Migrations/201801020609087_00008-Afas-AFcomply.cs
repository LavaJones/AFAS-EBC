namespace Afas.AfComply.Reporting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _00008AfasAFcomply : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Approval1094", "DataCopy_ID", "dbo.Approved1094Final");
            DropForeignKey("dbo.Approval1095", "DataCopy_ID", "dbo.Approved1095Final");
            DropForeignKey("dbo.Correction1094", "Approved1094_ID", "dbo.Approval1094");
            DropForeignKey("dbo.Void1094", "Approval_ID", "dbo.Approval1094");
            DropForeignKey("dbo.Print1094", "Approved1094_ID", "dbo.Approval1094");
            DropForeignKey("dbo.Print1095", "Approved1095_ID", "dbo.Approval1095");
            DropForeignKey("dbo.Correction1095", "Approved1095_ID", "dbo.Approval1095");
            DropForeignKey("dbo.Void1095", "Approval_ID", "dbo.Approval1095");
            DropForeignKey("dbo.Transmission1095", "Approval_ID", "dbo.Approval1095");
            DropForeignKey("dbo.Transmission1094", "Approved1094_ID", "dbo.Approval1094");
            DropIndex("dbo.Approval1094", new[] { "DataCopy_ID" });
            DropIndex("dbo.Approval1095", new[] { "DataCopy_ID" });
            DropIndex("dbo.Correction1094", new[] { "Approved1094_ID" });
            DropIndex("dbo.Void1094", new[] { "Approval_ID" });
            DropIndex("dbo.Print1094", new[] { "Approved1094_ID" });
            DropIndex("dbo.Print1095", new[] { "Approved1095_ID" });
            DropIndex("dbo.Correction1095", new[] { "Approved1095_ID" });
            DropIndex("dbo.Void1095", new[] { "Approval_ID" });
            DropIndex("dbo.Transmission1095", new[] { "Approval_ID" });
            DropIndex("dbo.Transmission1094", new[] { "Approved1094_ID" });
            AddColumn("dbo.Approved1095Final", "TaxYear", c => c.Int(nullable: false));
            CreateIndex("dbo.Correction1094", "Approved1094_ID");
            CreateIndex("dbo.Void1094", "Approval_ID");
            CreateIndex("dbo.Print1094", "Approved1094_ID");
            CreateIndex("dbo.Print1095", "Approved1095_ID");
            CreateIndex("dbo.Correction1095", "Approved1095_ID");
            CreateIndex("dbo.Void1095", "Approval_ID");
            CreateIndex("dbo.Transmission1095", "Approval_ID");
            CreateIndex("dbo.Transmission1094", "Approved1094_ID");
            AddForeignKey("dbo.Correction1094", "Approved1094_ID", "dbo.Approved1094Final", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Void1094", "Approval_ID", "dbo.Approved1094Final", "ID");
            AddForeignKey("dbo.Print1094", "Approved1094_ID", "dbo.Approved1094Final", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Print1095", "Approved1095_ID", "dbo.Approved1095Final", "Approved1095FinalId", cascadeDelete: true);
            AddForeignKey("dbo.Correction1095", "Approved1095_ID", "dbo.Approved1095Final", "Approved1095FinalId", cascadeDelete: true);
            AddForeignKey("dbo.Void1095", "Approval_ID", "dbo.Approved1095Final", "Approved1095FinalId");
            AddForeignKey("dbo.Transmission1095", "Approval_ID", "dbo.Approved1095Final", "Approved1095FinalId", cascadeDelete: true);
            AddForeignKey("dbo.Transmission1094", "Approved1094_ID", "dbo.Approved1094Final", "ID", cascadeDelete: true);
            DropTable("dbo.Approval1094");
            DropTable("dbo.Approval1095");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Approval1095",
                c => new
                    {
                        Approval1095Id = c.Long(nullable: false, identity: true),
                        ApprovedBy = c.String(nullable: false),
                        ApprovedOn = c.DateTime(nullable: false),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                        DataCopy_ID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Approval1095Id);
            
            CreateTable(
                "dbo.Approval1094",
                c => new
                    {
                        Approval1094Id = c.Long(nullable: false, identity: true),
                        ApprovedBy = c.String(nullable: false),
                        ApprovedOn = c.DateTime(nullable: false),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                        DataCopy_ID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Approval1094Id);
            
            DropForeignKey("dbo.Transmission1094", "Approved1094_ID", "dbo.Approved1094Final");
            DropForeignKey("dbo.Transmission1095", "Approval_ID", "dbo.Approved1095Final");
            DropForeignKey("dbo.Void1095", "Approval_ID", "dbo.Approved1095Final");
            DropForeignKey("dbo.Correction1095", "Approved1095_ID", "dbo.Approved1095Final");
            DropForeignKey("dbo.Print1095", "Approved1095_ID", "dbo.Approved1095Final");
            DropForeignKey("dbo.Print1094", "Approved1094_ID", "dbo.Approved1094Final");
            DropForeignKey("dbo.Void1094", "Approval_ID", "dbo.Approved1094Final");
            DropForeignKey("dbo.Correction1094", "Approved1094_ID", "dbo.Approved1094Final");
            DropIndex("dbo.Transmission1094", new[] { "Approved1094_ID" });
            DropIndex("dbo.Transmission1095", new[] { "Approval_ID" });
            DropIndex("dbo.Void1095", new[] { "Approval_ID" });
            DropIndex("dbo.Correction1095", new[] { "Approved1095_ID" });
            DropIndex("dbo.Print1095", new[] { "Approved1095_ID" });
            DropIndex("dbo.Print1094", new[] { "Approved1094_ID" });
            DropIndex("dbo.Void1094", new[] { "Approval_ID" });
            DropIndex("dbo.Correction1094", new[] { "Approved1094_ID" });
            DropColumn("dbo.Approved1095Final", "TaxYear");
            CreateIndex("dbo.Transmission1094", "Approved1094_ID");
            CreateIndex("dbo.Transmission1095", "Approval_ID");
            CreateIndex("dbo.Void1095", "Approval_ID");
            CreateIndex("dbo.Correction1095", "Approved1095_ID");
            CreateIndex("dbo.Print1095", "Approved1095_ID");
            CreateIndex("dbo.Print1094", "Approved1094_ID");
            CreateIndex("dbo.Void1094", "Approval_ID");
            CreateIndex("dbo.Correction1094", "Approved1094_ID");
            CreateIndex("dbo.Approval1095", "DataCopy_ID");
            CreateIndex("dbo.Approval1094", "DataCopy_ID");
            AddForeignKey("dbo.Transmission1094", "Approved1094_ID", "dbo.Approval1094", "Approval1094Id", cascadeDelete: true);
            AddForeignKey("dbo.Transmission1095", "Approval_ID", "dbo.Approval1095", "Approval1095Id", cascadeDelete: true);
            AddForeignKey("dbo.Void1095", "Approval_ID", "dbo.Approval1095", "Approval1095Id");
            AddForeignKey("dbo.Correction1095", "Approved1095_ID", "dbo.Approval1095", "Approval1095Id", cascadeDelete: true);
            AddForeignKey("dbo.Print1095", "Approved1095_ID", "dbo.Approval1095", "Approval1095Id", cascadeDelete: true);
            AddForeignKey("dbo.Print1094", "Approved1094_ID", "dbo.Approval1094", "Approval1094Id", cascadeDelete: true);
            AddForeignKey("dbo.Void1094", "Approval_ID", "dbo.Approval1094", "Approval1094Id");
            AddForeignKey("dbo.Correction1094", "Approved1094_ID", "dbo.Approval1094", "Approval1094Id", cascadeDelete: true);
            AddForeignKey("dbo.Approval1095", "DataCopy_ID", "dbo.Approved1095Final", "Approved1095FinalId", cascadeDelete: true);
            AddForeignKey("dbo.Approval1094", "DataCopy_ID", "dbo.Approved1094Final", "ID", cascadeDelete: true);
        }
    }
}
