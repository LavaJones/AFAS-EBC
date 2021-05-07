namespace Afas.AfComply.Reporting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _00017AfasAFcomply : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FileCabinetInfo", "OtherResourceId", c => c.Guid());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FileCabinetInfo", "OtherResourceId");
        }
    }
}
