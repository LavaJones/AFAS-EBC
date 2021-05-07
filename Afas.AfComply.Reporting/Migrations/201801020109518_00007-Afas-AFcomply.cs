namespace Afas.AfComply.Reporting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class _00007AfasAFcomply : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserEditPart2", "NewValue", c => c.String());
            CreateTable(
                "dbo.UserRevieweds",
                c => new
                {
                    UserReviewedId = c.Long(nullable: false, identity: true),
                    TaxYear = c.Int(nullable: false),
                    EmployeeId = c.Int(nullable: false),
                    EmployerId = c.Int(nullable: false),
                    ReviewedBy = c.String(nullable: false),
                    ReviewedOn = c.DateTime(nullable: false),
                    ResourceId = c.Guid(nullable: false),
                    EntityStatusId = c.Int(nullable: false),
                    CreatedBy = c.String(nullable: false, maxLength: 50),
                    CreatedDate = c.DateTime(nullable: false),
                    ModifiedBy = c.String(nullable: false, maxLength: 50),
                    ModifiedDate = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.UserReviewedId);

        }

        public override void Down()
        {
            AlterColumn("dbo.UserEditPart2", "NewValue", c => c.String(nullable: false));
            DropTable("dbo.UserRevieweds");
        }
    }
}
