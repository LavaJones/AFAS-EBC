namespace Afas.AfComply.Reporting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _00009AfasAFcomply : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Approved1095FinalPart2", "Offered", c => c.Boolean(nullable: false));
            AddColumn("dbo.Approved1095FinalPart2", "Receiving1095C", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Approved1095FinalPart2", "Receiving1095C");
            DropColumn("dbo.Approved1095FinalPart2", "Offered");
        }
    }
}
