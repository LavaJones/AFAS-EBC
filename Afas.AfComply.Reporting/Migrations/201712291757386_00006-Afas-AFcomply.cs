namespace Afas.AfComply.Reporting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _00006AfasAFcomply : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Approved1094Final",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        EmployerID = c.Int(nullable: false),
                        EmployerResourceId = c.Guid(nullable: false),
                        EIN = c.Int(nullable: false),
                        BusinessNameLine1 = c.String(nullable: false),
                        BusinessAddressLine1Txt = c.String(nullable: false),
                        BusinessCityNm = c.String(nullable: false),
                        BusinessUSStateCd = c.String(nullable: false),
                        BusinessUSZipCd = c.String(nullable: false),
                        ContactPhoneNum = c.String(nullable: false),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Approval1094", "DataCopy_ID", c => c.Long(nullable: false));
            AddColumn("dbo.Approved1095Final", "EmployeeResourceId", c => c.Guid(nullable: false));
            AddColumn("dbo.Approved1095Final", "SelfInsured", c => c.Boolean(nullable: false));
            AlterColumn("dbo.Approved1095Final", "EmployerID", c => c.Int(nullable: false));
            AlterColumn("dbo.Approved1095Final", "EmployeeID", c => c.Int(nullable: false));
            CreateIndex("dbo.Approval1094", "DataCopy_ID");
            AddForeignKey("dbo.Approval1094", "DataCopy_ID", "dbo.Approved1094Final", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Approval1094", "DataCopy_ID", "dbo.Approved1094Final");
            DropIndex("dbo.Approval1094", new[] { "DataCopy_ID" });
            AlterColumn("dbo.Approved1095Final", "EmployeeID", c => c.Int(nullable: false));
            AlterColumn("dbo.Approved1095Final", "EmployerID", c => c.Int(nullable: false));
            DropColumn("dbo.Approved1095Final", "SelfInsured");
            DropColumn("dbo.Approved1095Final", "EmployeeResourceId");
            DropColumn("dbo.Approval1094", "DataCopy_ID");
            DropTable("dbo.Approved1094Final");
        }
    }
}
