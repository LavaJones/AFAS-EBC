namespace Afas.AfComply.Reporting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _00012AfasAFcomply : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PrintBatches", "PdfReceivedOn", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.PrintBatches", "PdfReceivedOn");
        }
    }
}
