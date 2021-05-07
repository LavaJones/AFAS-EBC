namespace Afas.AfComply.Reporting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _00010AfasAFcomply : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Approved1095FinalPart3", "EnrolledJan", c => c.Boolean(nullable: false));
            AddColumn("dbo.Approved1095FinalPart3", "EnrolledFeb", c => c.Boolean(nullable: false));
            AddColumn("dbo.Approved1095FinalPart3", "EnrolledMar", c => c.Boolean(nullable: false));
            AddColumn("dbo.Approved1095FinalPart3", "EnrolledApr", c => c.Boolean(nullable: false));
            AddColumn("dbo.Approved1095FinalPart3", "EnrolledMay", c => c.Boolean(nullable: false));
            AddColumn("dbo.Approved1095FinalPart3", "EnrolledJun", c => c.Boolean(nullable: false));
            AddColumn("dbo.Approved1095FinalPart3", "EnrolledJul", c => c.Boolean(nullable: false));
            AddColumn("dbo.Approved1095FinalPart3", "EnrolledAug", c => c.Boolean(nullable: false));
            AddColumn("dbo.Approved1095FinalPart3", "EnrolledSep", c => c.Boolean(nullable: false));
            AddColumn("dbo.Approved1095FinalPart3", "EnrolledOct", c => c.Boolean(nullable: false));
            AddColumn("dbo.Approved1095FinalPart3", "EnrolledNov", c => c.Boolean(nullable: false));
            AddColumn("dbo.Approved1095FinalPart3", "EnrolledDec", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Approved1095FinalPart3", "EnrolledDec");
            DropColumn("dbo.Approved1095FinalPart3", "EnrolledNov");
            DropColumn("dbo.Approved1095FinalPart3", "EnrolledOct");
            DropColumn("dbo.Approved1095FinalPart3", "EnrolledSep");
            DropColumn("dbo.Approved1095FinalPart3", "EnrolledAug");
            DropColumn("dbo.Approved1095FinalPart3", "EnrolledJul");
            DropColumn("dbo.Approved1095FinalPart3", "EnrolledJun");
            DropColumn("dbo.Approved1095FinalPart3", "EnrolledMay");
            DropColumn("dbo.Approved1095FinalPart3", "EnrolledApr");
            DropColumn("dbo.Approved1095FinalPart3", "EnrolledMar");
            DropColumn("dbo.Approved1095FinalPart3", "EnrolledFeb");
            DropColumn("dbo.Approved1095FinalPart3", "EnrolledJan");
        }
    }
}
