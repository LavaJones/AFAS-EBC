namespace Afas.AfComply.Reporting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _00012AfasAFcomply : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PrintBatches", "PdfReceivedOn", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PrintBatches", "PdfReceivedOn", c => c.DateTime(nullable: false));
        }
    }
}
