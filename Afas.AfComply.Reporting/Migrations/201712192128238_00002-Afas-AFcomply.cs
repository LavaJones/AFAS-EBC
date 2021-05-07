namespace Afas.AfComply.Reporting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _00002AfasAFcomply : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Correction1095", "Voided_ID", "dbo.Void1095");
            DropIndex("dbo.Correction1095", new[] { "Voided_ID" });
            CreateTable(
                "dbo.Approved1095Final",
                c => new
                    {
                        Approved1095FinalId = c.Long(nullable: false, identity: true),
                        employerID = c.Int(nullable: false),
                        employeeID = c.Int(nullable: false),
                        terminationDate = c.DateTime(nullable: false),
                        acaStatusID = c.Int(nullable: false),
                        classificationID = c.Int(nullable: false),
                        monthID = c.Int(nullable: false),
                        monthlyAverageHours = c.Int(nullable: false),
                        defaultOfferOfCoverage = c.String(),
                        mec = c.String(),
                        defaultSafeHarborCode = c.String(),
                        employeesMonthlyCost = c.Double(nullable: false),
                        employeeOfferedCoverage = c.Boolean(nullable: false),
                        employeeEnrolledInCoverage = c.Boolean(nullable: false),
                        insuranceType = c.Int(nullable: false),
                        offeredToSpouse = c.Boolean(nullable: false),
                        OfferedToSpouseConditional = c.Boolean(nullable: false),
                        mainLand = c.Boolean(nullable: false),
                        coverageEffectiveDate = c.DateTime(nullable: false),
                        fullyPlusSelfInsured = c.Boolean(nullable: false),
                        minValue = c.Int(nullable: false),
                        waitingPeriodID = c.Int(nullable: false),
                        taxYear = c.Int(nullable: false),
                        planYearID = c.Int(nullable: false),
                        hireDate = c.DateTime(nullable: false),
                        line14 = c.String(),
                        line15 = c.String(),
                        line16 = c.String(),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Approved1095FinalId);
            
            CreateTable(
                "dbo.Approved1095Initial",
                c => new
                    {
                        Approved1095InitialId = c.Long(nullable: false, identity: true),
                        employerID = c.Int(nullable: false),
                        employeeID = c.Int(nullable: false),
                        terminationDate = c.DateTime(nullable: false),
                        acaStatusID = c.Int(nullable: false),
                        classificationID = c.Int(nullable: false),
                        monthID = c.Int(nullable: false),
                        monthlyAverageHours = c.Int(nullable: false),
                        defaultOfferOfCoverage = c.String(),
                        mec = c.String(),
                        defaultSafeHarborCode = c.String(),
                        employeesMonthlyCost = c.Double(nullable: false),
                        employeeOfferedCoverage = c.Boolean(nullable: false),
                        employeeEnrolledInCoverage = c.Boolean(nullable: false),
                        insuranceType = c.Int(nullable: false),
                        offeredToSpouse = c.Boolean(nullable: false),
                        OfferedToSpouseConditional = c.Boolean(nullable: false),
                        mainLand = c.Boolean(nullable: false),
                        coverageEffectiveDate = c.DateTime(nullable: false),
                        fullyPlusSelfInsured = c.Boolean(nullable: false),
                        minValue = c.Int(nullable: false),
                        waitingPeriodID = c.Int(nullable: false),
                        taxYear = c.Int(nullable: false),
                        planYearID = c.Int(nullable: false),
                        hireDate = c.DateTime(nullable: false),
                        line14 = c.String(),
                        line15 = c.String(),
                        line16 = c.String(),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Approved1095InitialId);
            
            CreateTable(
                "dbo.UserEditPart2",
                c => new
                    {
                        UserEditPart2Id = c.Long(nullable: false, identity: true),
                        OldValue = c.String(nullable: false),
                        NewValue = c.String(nullable: false),
                        MonthId = c.Int(nullable: false),
                        LineId = c.Int(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                        EmployerId = c.Int(nullable: false),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserEditPart2Id);
            
            AddColumn("dbo.Correction1095", "Voided1095_ID", c => c.Long(nullable: false));
            AddColumn("dbo.Transmission1094", "TransmissionType", c => c.Int(nullable: false));
            AddColumn("dbo.Transmission1095", "TransmissionType", c => c.Int(nullable: false));
            CreateIndex("dbo.Correction1095", "Voided1095_ID");
            AddForeignKey("dbo.Correction1095", "Voided1095_ID", "dbo.Void1095", "Void1095Id", cascadeDelete: true);
            DropColumn("dbo.Correction1095", "Voided_ID");
            DropColumn("dbo.Transmission1094", "TranmissionType");
            DropColumn("dbo.Transmission1095", "TranmissionType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Transmission1095", "TranmissionType", c => c.Int(nullable: false));
            AddColumn("dbo.Transmission1094", "TranmissionType", c => c.Int(nullable: false));
            AddColumn("dbo.Correction1095", "Voided_ID", c => c.Long(nullable: false));
            DropForeignKey("dbo.Correction1095", "Voided1095_ID", "dbo.Void1095");
            DropIndex("dbo.Correction1095", new[] { "Voided1095_ID" });
            DropColumn("dbo.Transmission1095", "TransmissionType");
            DropColumn("dbo.Transmission1094", "TransmissionType");
            DropColumn("dbo.Correction1095", "Voided1095_ID");
            DropTable("dbo.UserEditPart2");
            DropTable("dbo.Approved1095Initial");
            DropTable("dbo.Approved1095Final");
            CreateIndex("dbo.Correction1095", "Voided_ID");
            AddForeignKey("dbo.Correction1095", "Voided_ID", "dbo.Void1095", "Void1095Id", cascadeDelete: true);
        }
    }
}
