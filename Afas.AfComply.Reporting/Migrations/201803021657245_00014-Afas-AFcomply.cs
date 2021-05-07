namespace Afas.AfComply.Reporting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _00014AfasAFcomply : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Correction1094", "Approved1094_ID", "dbo.Approved1094Final");
            DropForeignKey("dbo.Void1094", "Approval_ID", "dbo.Approved1094Final");
            DropForeignKey("dbo.Print1094", "Approved1094_ID", "dbo.Approved1094Final");
            DropForeignKey("dbo.Transmission1094", "Approved1094_ID", "dbo.Approved1094Final");
            DropIndex("dbo.Correction1094", new[] { "Approved1094_ID" });
            DropIndex("dbo.Void1094", new[] { "Approval_ID" });
            DropIndex("dbo.Print1094", new[] { "Approved1094_ID" });
            DropIndex("dbo.Transmission1094", new[] { "Approved1094_ID" });
            CreateTable(
                "dbo.Approved1094FinalPart1",
                c => new
                    {
                        Approved1094FinalPart1Id = c.Long(nullable: false, identity: true),
                        Address = c.String(nullable: false),
                        City = c.String(nullable: false),
                        DgeAddress = c.String(),
                        DgeCity = c.String(),
                        DgeContactName = c.String(),
                        DgeContactPhoneNumber = c.String(),
                        DgeEIN = c.String(),
                        DgeName = c.String(),
                        DgeState = c.Int(nullable: false),
                        DgeZipCode = c.String(),
                        EIN = c.String(nullable: false),
                        EmployerId = c.Int(nullable: false),
                        EmployerDBAName = c.String(),
                        EmployerName = c.String(nullable: false),
                        EmployerResourceId = c.Guid(nullable: false),
                        IrsContactName = c.String(nullable: false),
                        IrsContactPhone = c.String(nullable: false),
                        IsAuthoritiveTransmission = c.Boolean(nullable: false),
                        IsDge = c.Boolean(nullable: false),
                        TaxYearId = c.Int(nullable: false),
                        TransmissionTotal1095Forms = c.Int(nullable: false),
                        StateId = c.Int(nullable: false),
                        ZipCode = c.String(nullable: false),
                        Total1095Forms = c.Int(nullable: false),
                        IsAggregatedAleGroup = c.Boolean(nullable: false),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Approved1094FinalPart1Id);
            
            CreateTable(
                "dbo.Approved1094FinalPart3",
                c => new
                    {
                        Approved1094FinalPart3Id = c.Long(nullable: false, identity: true),
                        MonthId = c.Int(nullable: false),
                        MinimumEssentialCoverageOfferIndicator = c.Boolean(nullable: false),
                        FullTimeEmployeeCount = c.Int(nullable: false),
                        TotalEmployeeCount = c.Int(nullable: false),
                        AggregatedGroupIndicator = c.Boolean(nullable: false),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                        Approved1094FinalPart1Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Approved1094FinalPart3Id)
                .ForeignKey("dbo.Approved1094FinalPart1", t => t.Approved1094FinalPart1Id, cascadeDelete: true)
                .Index(t => t.Approved1094FinalPart1Id);
            
            CreateTable(
                "dbo.Approved1094FinalPart4",
                c => new
                    {
                        Approved1094FinalPart4Id = c.Long(nullable: false, identity: true),
                        EIN = c.String(nullable: false),
                        EmployerName = c.String(nullable: false),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                        Approved1094FinalPart1Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Approved1094FinalPart4Id)
                .ForeignKey("dbo.Approved1094FinalPart1", t => t.Approved1094FinalPart1Id, cascadeDelete: true)
                .Index(t => t.Approved1094FinalPart1Id);
            
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
                .PrimaryKey(t => t.Approved1094FinalPart2Id)
                .ForeignKey("dbo.Approved1094FinalPart1", t => t.Approved1094FinalPart1_ID, cascadeDelete: true)
                .Index(t => t.Approved1094FinalPart1_ID);
            
            CreateIndex("dbo.Correction1094", "Approved1094_ID");
            CreateIndex("dbo.Void1094", "Approval_ID");
            CreateIndex("dbo.Print1094", "Approved1094_ID");
            CreateIndex("dbo.Transmission1094", "Approved1094_ID");
            AddForeignKey("dbo.Correction1094", "Approved1094_ID", "dbo.Approved1094FinalPart1", "Approved1094FinalPart1Id", cascadeDelete: true);
            AddForeignKey("dbo.Void1094", "Approval_ID", "dbo.Approved1094FinalPart1", "Approved1094FinalPart1Id");
            AddForeignKey("dbo.Print1094", "Approved1094_ID", "dbo.Approved1094FinalPart1", "Approved1094FinalPart1Id", cascadeDelete: true);
            AddForeignKey("dbo.Transmission1094", "Approved1094_ID", "dbo.Approved1094FinalPart1", "Approved1094FinalPart1Id", cascadeDelete: true);
            DropTable("dbo.Approved1094Final");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Approved1094Final",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        EmployerID = c.Int(nullable: false),
                        EmployerResourceId = c.Guid(nullable: false),
                        EIN = c.Int(nullable: false),
                        BusinessNameLine1 = c.String(nullable: false),
                        BusinessAddressLine1Txt = c.String(nullable: false),
                        BusinessCityNm = c.String(nullable: false),
                        BusinessUSStateCd = c.String(nullable: false),
                        BusinessUSZipCd = c.String(nullable: false),
                        ContactPhoneNum = c.String(nullable: false),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            DropForeignKey("dbo.Transmission1094", "Approved1094_ID", "dbo.Approved1094FinalPart1");
            DropForeignKey("dbo.Print1094", "Approved1094_ID", "dbo.Approved1094FinalPart1");
            DropForeignKey("dbo.Void1094", "Approval_ID", "dbo.Approved1094FinalPart1");
            DropForeignKey("dbo.Correction1094", "Approved1094_ID", "dbo.Approved1094FinalPart1");
            DropForeignKey("dbo.Approved1094FinalPart2", "Approved1094FinalPart1_ID", "dbo.Approved1094FinalPart1");
            DropForeignKey("dbo.Approved1094FinalPart4", "Approved1094FinalPart1Id", "dbo.Approved1094FinalPart1");
            DropForeignKey("dbo.Approved1094FinalPart3", "Approved1094FinalPart1Id", "dbo.Approved1094FinalPart1");
            DropIndex("dbo.Transmission1094", new[] { "Approved1094_ID" });
            DropIndex("dbo.Print1094", new[] { "Approved1094_ID" });
            DropIndex("dbo.Void1094", new[] { "Approval_ID" });
            DropIndex("dbo.Correction1094", new[] { "Approved1094_ID" });
            DropIndex("dbo.Approved1094FinalPart2", new[] { "Approved1094FinalPart1_ID" });
            DropIndex("dbo.Approved1094FinalPart4", new[] { "Approved1094FinalPart1Id" });
            DropIndex("dbo.Approved1094FinalPart3", new[] { "Approved1094FinalPart1Id" });
            DropTable("dbo.Approved1094FinalPart2");
            DropTable("dbo.Approved1094FinalPart4");
            DropTable("dbo.Approved1094FinalPart3");
            DropTable("dbo.Approved1094FinalPart1");
            CreateIndex("dbo.Transmission1094", "Approved1094_ID");
            CreateIndex("dbo.Print1094", "Approved1094_ID");
            CreateIndex("dbo.Void1094", "Approval_ID");
            CreateIndex("dbo.Correction1094", "Approved1094_ID");
            AddForeignKey("dbo.Transmission1094", "Approved1094_ID", "dbo.Approved1094Final", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Print1094", "Approved1094_ID", "dbo.Approved1094Final", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Void1094", "Approval_ID", "dbo.Approved1094Final", "ID");
            AddForeignKey("dbo.Correction1094", "Approved1094_ID", "dbo.Approved1094Final", "ID", cascadeDelete: true);
        }
    }
}
