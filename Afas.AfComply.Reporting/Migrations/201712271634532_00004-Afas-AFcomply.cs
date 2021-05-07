namespace Afas.AfComply.Reporting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _00004AfasAFcomply : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Approved1095FinalPart2",
                c => new
                    {
                        Approved1095FinalPart2Id = c.Long(nullable: false, identity: true),
                        employeeID = c.Int(nullable: false),
                        TaxYear = c.Int(nullable: false),
                        MonthId = c.Int(nullable: false),
                        Line14 = c.String(),
                        Line15 = c.String(),
                        Line16 = c.String(),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                        Approved1095Final_ID = c.Long(),
                    })
                .PrimaryKey(t => t.Approved1095FinalPart2Id)
                .ForeignKey("dbo.Approved1095Final", t => t.Approved1095Final_ID)
                .Index(t => t.Approved1095Final_ID);
            
            CreateTable(
                "dbo.Approved1095FinalPart3",
                c => new
                    {
                        Approved1095FinalPart3Id = c.Long(nullable: false, identity: true),
                        InsuranceCoverageRowID = c.Int(nullable: false),
                        EmployeeID = c.Int(nullable: false),
                        DependantID = c.Int(nullable: false),
                        TaxYear = c.Int(nullable: false),
                        FirstName = c.String(nullable: false),
                        MiddleName = c.String(),
                        LastName = c.String(nullable: false),
                        SSN = c.String(),
                        Dob = c.DateTime(),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                        Approved1095Final_ID = c.Long(),
                    })
                .PrimaryKey(t => t.Approved1095FinalPart3Id)
                .ForeignKey("dbo.Approved1095Final", t => t.Approved1095Final_ID)
                .Index(t => t.Approved1095Final_ID);
            
            AddColumn("dbo.Approved1095Final", "FirstName", c => c.String(nullable: false));
            AddColumn("dbo.Approved1095Final", "MiddleName", c => c.String());
            AddColumn("dbo.Approved1095Final", "LastName", c => c.String(nullable: false));
            AddColumn("dbo.Approved1095Final", "SSN", c => c.String(nullable: false));
            AddColumn("dbo.Approved1095Final", "StreetAddress", c => c.String(nullable: false));
            AddColumn("dbo.Approved1095Final", "City", c => c.String(nullable: false));
            AddColumn("dbo.Approved1095Final", "State", c => c.String(nullable: false));
            AddColumn("dbo.Approved1095Final", "Zip", c => c.String(nullable: false));
            AlterColumn("dbo.UserEditPart2", "OldValue", c => c.String());
            DropColumn("dbo.Approved1095Final", "terminationDate");
            DropColumn("dbo.Approved1095Final", "acaStatusID");
            DropColumn("dbo.Approved1095Final", "classificationID");
            DropColumn("dbo.Approved1095Final", "monthID");
            DropColumn("dbo.Approved1095Final", "monthlyAverageHours");
            DropColumn("dbo.Approved1095Final", "defaultOfferOfCoverage");
            DropColumn("dbo.Approved1095Final", "mec");
            DropColumn("dbo.Approved1095Final", "defaultSafeHarborCode");
            DropColumn("dbo.Approved1095Final", "employeesMonthlyCost");
            DropColumn("dbo.Approved1095Final", "employeeOfferedCoverage");
            DropColumn("dbo.Approved1095Final", "employeeEnrolledInCoverage");
            DropColumn("dbo.Approved1095Final", "insuranceType");
            DropColumn("dbo.Approved1095Final", "offeredToSpouse");
            DropColumn("dbo.Approved1095Final", "OfferedToSpouseConditional");
            DropColumn("dbo.Approved1095Final", "mainLand");
            DropColumn("dbo.Approved1095Final", "coverageEffectiveDate");
            DropColumn("dbo.Approved1095Final", "fullyPlusSelfInsured");
            DropColumn("dbo.Approved1095Final", "minValue");
            DropColumn("dbo.Approved1095Final", "waitingPeriodID");
            DropColumn("dbo.Approved1095Final", "taxYear");
            DropColumn("dbo.Approved1095Final", "planYearID");
            DropColumn("dbo.Approved1095Final", "hireDate");
            DropColumn("dbo.Approved1095Final", "line14");
            DropColumn("dbo.Approved1095Final", "line15");
            DropColumn("dbo.Approved1095Final", "line16");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Approved1095Final", "line16", c => c.String());
            AddColumn("dbo.Approved1095Final", "line15", c => c.String());
            AddColumn("dbo.Approved1095Final", "line14", c => c.String());
            AddColumn("dbo.Approved1095Final", "hireDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Approved1095Final", "planYearID", c => c.Int(nullable: false));
            AddColumn("dbo.Approved1095Final", "taxYear", c => c.Int(nullable: false));
            AddColumn("dbo.Approved1095Final", "waitingPeriodID", c => c.Int(nullable: false));
            AddColumn("dbo.Approved1095Final", "minValue", c => c.Int(nullable: false));
            AddColumn("dbo.Approved1095Final", "fullyPlusSelfInsured", c => c.Boolean(nullable: false));
            AddColumn("dbo.Approved1095Final", "coverageEffectiveDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Approved1095Final", "mainLand", c => c.Boolean(nullable: false));
            AddColumn("dbo.Approved1095Final", "OfferedToSpouseConditional", c => c.Boolean(nullable: false));
            AddColumn("dbo.Approved1095Final", "offeredToSpouse", c => c.Boolean(nullable: false));
            AddColumn("dbo.Approved1095Final", "insuranceType", c => c.Int(nullable: false));
            AddColumn("dbo.Approved1095Final", "employeeEnrolledInCoverage", c => c.Boolean(nullable: false));
            AddColumn("dbo.Approved1095Final", "employeeOfferedCoverage", c => c.Boolean(nullable: false));
            AddColumn("dbo.Approved1095Final", "employeesMonthlyCost", c => c.Double(nullable: false));
            AddColumn("dbo.Approved1095Final", "defaultSafeHarborCode", c => c.String());
            AddColumn("dbo.Approved1095Final", "mec", c => c.String());
            AddColumn("dbo.Approved1095Final", "defaultOfferOfCoverage", c => c.String());
            AddColumn("dbo.Approved1095Final", "monthlyAverageHours", c => c.Int(nullable: false));
            AddColumn("dbo.Approved1095Final", "monthID", c => c.Int(nullable: false));
            AddColumn("dbo.Approved1095Final", "classificationID", c => c.Int(nullable: false));
            AddColumn("dbo.Approved1095Final", "acaStatusID", c => c.Int(nullable: false));
            AddColumn("dbo.Approved1095Final", "terminationDate", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.Approved1095FinalPart3", "Approved1095Final_ID", "dbo.Approved1095Final");
            DropForeignKey("dbo.Approved1095FinalPart2", "Approved1095Final_ID", "dbo.Approved1095Final");
            DropIndex("dbo.Approved1095FinalPart3", new[] { "Approved1095Final_ID" });
            DropIndex("dbo.Approved1095FinalPart2", new[] { "Approved1095Final_ID" });
            AlterColumn("dbo.UserEditPart2", "OldValue", c => c.String(nullable: false));
            DropColumn("dbo.Approved1095Final", "Zip");
            DropColumn("dbo.Approved1095Final", "State");
            DropColumn("dbo.Approved1095Final", "City");
            DropColumn("dbo.Approved1095Final", "StreetAddress");
            DropColumn("dbo.Approved1095Final", "SSN");
            DropColumn("dbo.Approved1095Final", "LastName");
            DropColumn("dbo.Approved1095Final", "MiddleName");
            DropColumn("dbo.Approved1095Final", "FirstName");
            DropTable("dbo.Approved1095FinalPart3");
            DropTable("dbo.Approved1095FinalPart2");
        }
    }
}
