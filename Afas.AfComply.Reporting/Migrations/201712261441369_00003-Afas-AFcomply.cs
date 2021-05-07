namespace Afas.AfComply.Reporting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _00003AfasAFcomply : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserEditPart2", "TaxYear", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserEditPart2", "TaxYear");
        }
    }
}
