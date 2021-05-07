namespace Afas.AfComply.Reporting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _00015AfasAFcomply : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Approved1094FinalPart2", "Approved1094FinalPart1_ID", "dbo.Approved1094FinalPart1");
            DropIndex("dbo.Approved1094FinalPart2", new[] { "Approved1094FinalPart1_ID" });
            DropTable("dbo.Approved1094FinalPart2");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Approved1094FinalPart2",
                c => new
                    {
                        Approved1094FinalPart2Id = c.Long(nullable: false, identity: true),
                        Total1095Forms = c.Int(nullable: false),
                        IsAggregatedAleGroup = c.Boolean(nullable: false),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                        Approved1094FinalPart1_ID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Approved1094FinalPart2Id);
            
            CreateIndex("dbo.Approved1094FinalPart2", "Approved1094FinalPart1_ID");
            AddForeignKey("dbo.Approved1094FinalPart2", "Approved1094FinalPart1_ID", "dbo.Approved1094FinalPart1", "Approved1094FinalPart1Id", cascadeDelete: true);
        }
    }
}
