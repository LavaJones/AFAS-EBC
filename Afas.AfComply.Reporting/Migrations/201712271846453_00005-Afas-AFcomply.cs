namespace Afas.AfComply.Reporting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _00005AfasAFcomply : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Approval1095", "DataCopy_ID", c => c.Long(nullable: false));
            CreateIndex("dbo.Approval1095", "DataCopy_ID");
            AddForeignKey("dbo.Approval1095", "DataCopy_ID", "dbo.Approved1095Final", "Approved1095FinalId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Approval1095", "DataCopy_ID", "dbo.Approved1095Final");
            DropIndex("dbo.Approval1095", new[] { "DataCopy_ID" });
            DropColumn("dbo.Approval1095", "DataCopy_ID");
        }
    }
}
