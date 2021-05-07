namespace Afas.AfComply.Reporting.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _00001AfasAFcomply : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Approval1094",
                c => new
                    {
                        Approval1094Id = c.Long(nullable: false, identity: true),
                        ApprovedBy = c.String(nullable: false),
                        ApprovedOn = c.DateTime(nullable: false),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Approval1094Id);
            
            CreateTable(
                "dbo.Approval1095",
                c => new
                    {
                        Approval1095Id = c.Long(nullable: false, identity: true),
                        ApprovedBy = c.String(nullable: false),
                        ApprovedOn = c.DateTime(nullable: false),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Approval1095Id);
            
            CreateTable(
                "dbo.Correction1094",
                c => new
                    {
                        Correction1094Id = c.Long(nullable: false, identity: true),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                        Approved1094_ID = c.Long(nullable: false),
                        Voided1094_ID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Correction1094Id)
                .ForeignKey("dbo.Approval1094", t => t.Approved1094_ID, cascadeDelete: true)
                .ForeignKey("dbo.Void1094", t => t.Voided1094_ID, cascadeDelete: true)
                .Index(t => t.Approved1094_ID)
                .Index(t => t.Voided1094_ID);
            
            CreateTable(
                "dbo.Void1094",
                c => new
                    {
                        Void1094Id = c.Long(nullable: false, identity: true),
                        VoidedOn = c.DateTime(nullable: false),
                        VoidedBy = c.String(nullable: false),
                        Reason = c.String(),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                        Approval_ID = c.Long(),
                        Print_ID = c.Long(),
                    })
                .PrimaryKey(t => t.Void1094Id)
                .ForeignKey("dbo.Approval1094", t => t.Approval_ID)
                .ForeignKey("dbo.Print1094", t => t.Print_ID)
                .Index(t => t.Approval_ID)
                .Index(t => t.Print_ID);
            
            CreateTable(
                "dbo.Print1094",
                c => new
                    {
                        Print1094Id = c.Long(nullable: false, identity: true),
                        OutputFilePath = c.String(nullable: false),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                        Approved1094_ID = c.Long(nullable: false),
                        PrintBatch_ID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Print1094Id)
                .ForeignKey("dbo.Approval1094", t => t.Approved1094_ID, cascadeDelete: true)
                .ForeignKey("dbo.PrintBatches", t => t.PrintBatch_ID, cascadeDelete: true)
                .Index(t => t.Approved1094_ID)
                .Index(t => t.PrintBatch_ID);
            
            CreateTable(
                "dbo.PrintBatches",
                c => new
                    {
                        PrintBatchId = c.Long(nullable: false, identity: true),
                        Reprint = c.Boolean(nullable: false),
                        PrintFileArchivePath = c.String(nullable: false),
                        RequestedBy = c.String(nullable: false),
                        RequestedOn = c.DateTime(nullable: false),
                        SentOn = c.DateTime(nullable: false),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PrintBatchId);
            
            CreateTable(
                "dbo.Print1095",
                c => new
                    {
                        Print1095Id = c.Long(nullable: false, identity: true),
                        OutputFilePath = c.String(nullable: false),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                        Approved1095_ID = c.Long(nullable: false),
                        PrintBatch_ID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Print1095Id)
                .ForeignKey("dbo.Approval1095", t => t.Approved1095_ID, cascadeDelete: true)
                .ForeignKey("dbo.PrintBatches", t => t.PrintBatch_ID, cascadeDelete: true)
                .Index(t => t.Approved1095_ID)
                .Index(t => t.PrintBatch_ID);
            
            CreateTable(
                "dbo.Correction1095",
                c => new
                    {
                        Correction1095Id = c.Long(nullable: false, identity: true),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                        Approved1095_ID = c.Long(nullable: false),
                        Voided_ID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Correction1095Id)
                .ForeignKey("dbo.Approval1095", t => t.Approved1095_ID, cascadeDelete: true)
                .ForeignKey("dbo.Void1095", t => t.Voided_ID, cascadeDelete: true)
                .Index(t => t.Approved1095_ID)
                .Index(t => t.Voided_ID);
            
            CreateTable(
                "dbo.Void1095",
                c => new
                    {
                        Void1095Id = c.Long(nullable: false, identity: true),
                        VoidedOn = c.DateTime(nullable: false),
                        VoidedBy = c.String(nullable: false),
                        Reason = c.String(),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                        Approval_ID = c.Long(),
                        Print_ID = c.Long(),
                    })
                .PrimaryKey(t => t.Void1095Id)
                .ForeignKey("dbo.Approval1095", t => t.Approval_ID)
                .ForeignKey("dbo.Print1095", t => t.Print_ID)
                .Index(t => t.Approval_ID)
                .Index(t => t.Print_ID);
            
            CreateTable(
                "dbo.TimeFrames",
                c => new
                    {
                        TimeFrameId = c.Long(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.TimeFrameId);
            
            CreateTable(
                "dbo.Transmission1094",
                c => new
                    {
                        Transmission1094Id = c.Long(nullable: false, identity: true),
                        TransmissionTime = c.DateTime(nullable: false),
                        TranmissionType = c.Int(nullable: false),
                        UniqueRecordId = c.String(nullable: false),
                        TransmissionStatus = c.Int(nullable: false),
                        IrsReciptId = c.String(),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                        Approved1094_ID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Transmission1094Id)
                .ForeignKey("dbo.Approval1094", t => t.Approved1094_ID, cascadeDelete: true)
                .Index(t => t.Approved1094_ID);
            
            CreateTable(
                "dbo.Transmission1095",
                c => new
                    {
                        Transmission1095Id = c.Long(nullable: false, identity: true),
                        TransmissionTime = c.DateTime(nullable: false),
                        TranmissionType = c.Int(nullable: false),
                        UniqueRecordId = c.String(nullable: false),
                        TransmissionStatus = c.Int(nullable: false),
                        ResourceId = c.Guid(nullable: false),
                        EntityStatusId = c.Int(nullable: false),
                        CreatedBy = c.String(nullable: false, maxLength: 50),
                        CreatedDate = c.DateTime(nullable: false),
                        ModifiedBy = c.String(nullable: false, maxLength: 50),
                        ModifiedDate = c.DateTime(nullable: false),
                        Approval_ID = c.Long(nullable: false),
                        Transmission1094_ID = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Transmission1095Id)
                .ForeignKey("dbo.Approval1095", t => t.Approval_ID, cascadeDelete: true)
                .ForeignKey("dbo.Transmission1094", t => t.Transmission1094_ID, cascadeDelete: true)
                .Index(t => t.Approval_ID)
                .Index(t => t.Transmission1094_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transmission1094", "Approved1094_ID", "dbo.Approval1094");
            DropForeignKey("dbo.Transmission1095", "Transmission1094_ID", "dbo.Transmission1094");
            DropForeignKey("dbo.Transmission1095", "Approval_ID", "dbo.Approval1095");
            DropForeignKey("dbo.Correction1095", "Voided_ID", "dbo.Void1095");
            DropForeignKey("dbo.Void1095", "Print_ID", "dbo.Print1095");
            DropForeignKey("dbo.Void1095", "Approval_ID", "dbo.Approval1095");
            DropForeignKey("dbo.Correction1095", "Approved1095_ID", "dbo.Approval1095");
            DropForeignKey("dbo.Correction1094", "Voided1094_ID", "dbo.Void1094");
            DropForeignKey("dbo.Void1094", "Print_ID", "dbo.Print1094");
            DropForeignKey("dbo.Print1094", "PrintBatch_ID", "dbo.PrintBatches");
            DropForeignKey("dbo.Print1095", "PrintBatch_ID", "dbo.PrintBatches");
            DropForeignKey("dbo.Print1095", "Approved1095_ID", "dbo.Approval1095");
            DropForeignKey("dbo.Print1094", "Approved1094_ID", "dbo.Approval1094");
            DropForeignKey("dbo.Void1094", "Approval_ID", "dbo.Approval1094");
            DropForeignKey("dbo.Correction1094", "Approved1094_ID", "dbo.Approval1094");
            DropIndex("dbo.Transmission1094", new[] { "Approved1094_ID" });
            DropIndex("dbo.Transmission1095", new[] { "Transmission1094_ID" });
            DropIndex("dbo.Transmission1095", new[] { "Approval_ID" });
            DropIndex("dbo.Correction1095", new[] { "Voided_ID" });
            DropIndex("dbo.Void1095", new[] { "Print_ID" });
            DropIndex("dbo.Void1095", new[] { "Approval_ID" });
            DropIndex("dbo.Correction1095", new[] { "Approved1095_ID" });
            DropIndex("dbo.Correction1094", new[] { "Voided1094_ID" });
            DropIndex("dbo.Void1094", new[] { "Print_ID" });
            DropIndex("dbo.Print1094", new[] { "PrintBatch_ID" });
            DropIndex("dbo.Print1095", new[] { "PrintBatch_ID" });
            DropIndex("dbo.Print1095", new[] { "Approved1095_ID" });
            DropIndex("dbo.Print1094", new[] { "Approved1094_ID" });
            DropIndex("dbo.Void1094", new[] { "Approval_ID" });
            DropIndex("dbo.Correction1094", new[] { "Approved1094_ID" });
            DropTable("dbo.Transmission1095");
            DropTable("dbo.Transmission1094");
            DropTable("dbo.TimeFrames");
            DropTable("dbo.Void1095");
            DropTable("dbo.Correction1095");
            DropTable("dbo.Print1095");
            DropTable("dbo.PrintBatches");
            DropTable("dbo.Print1094");
            DropTable("dbo.Void1094");
            DropTable("dbo.Correction1094");
            DropTable("dbo.Approval1095");
            DropTable("dbo.Approval1094");
        }
    }
}
