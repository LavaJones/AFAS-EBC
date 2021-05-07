using Afas.AfComply.Reporting.Domain.Approvals;
using Afas.AfComply.Reporting.Domain.Corrections;
using Afas.AfComply.Reporting.Domain.Printing;
using Afas.AfComply.Reporting.Domain.Reporting;
using Afas.AfComply.Reporting.Domain.TimeFrames;
using Afas.AfComply.Reporting.Domain.Transmission;
using Afas.AfComply.Reporting.Domain.Voids;
using System.Data.Entity;
using Afas.AfComply.Reporting.Domain.Approvals.SsisFileTransfer;
using Afc.Core.Logging;
using Afas.Domain;
using Afas.AfComply.Reporting.Domain.Approvals.FileCabinet;
using Afas.AfComply.Reporting.Domain.FileCabinet;



namespace Afas.AfComply.Reporting.Domain
{

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ReportingDataContext : AfasDataContext, IReportingDataContext
    {

        public ReportingDataContext() : base()
        {
            if (Afc.Core.Logging.LogManager.LoggerFactory != null)
            {
                this.Log = Afc.Core.Logging.LogManager.GetLogger("Internal.Afas.AfComply.Reporting.Domain.EfLoggingAdapter");
                if (this.Log.IsDebugEnabled)
                {
                    this.Database.Log = (logStatement => this.Log.Info(logStatement));
                }
            }

            this.Context.Database.CommandTimeout = 240;
            //this.Configuration.ProxyCreationEnabled = false;
            //this.Configuration.LazyLoadingEnabled = false;
            //this.Context.Configuration.LazyLoadingEnabled = false;
            this.Context.Configuration.UseDatabaseNullSemantics = true; // If we set this to false then it does weird or 'AND (@p__linq__0 IS NOT NULL)'

        }

        public IDbSet<TimeFrame> TimeFrames { get; set; }

        public IDbSet<Transmission1095> Transmission1095s { get; set; }

        public IDbSet<Transmission1094> Transmission1094s { get; set; }

        public IDbSet<Void1095> Void1095s { get; set; }

        public IDbSet<Void1094> Void1094s { get; set; }

        public IDbSet<PrintBatch> PrintBatches { get; set; }

        public IDbSet<Print1095> Print1095s { get; set; }

        public IDbSet<Print1094> Print1094s { get; set; }

        public IDbSet<Correction1095> Correction1095s { get; set; }

        public IDbSet<Correction1094> Correction1094s { get; set; }

        public IDbSet<UserEditPart2> UserEditPart2s { get; set; }

        public IDbSet<Approved1094FinalPart1> Approved1094FinalPart1s { get; set; }



        public IDbSet<Approved1094FinalPart4> Approved1094FinalPart4s { get; set; }

        public IDbSet<Approved1095Initial> Approved1095Initials { get; set; }

        public IDbSet<Approved1095Final> Approved1095Finals { get; set; }

        public IDbSet<Approved1095FinalPart2> Approved1095FinalPart2s { get; set; }

        public IDbSet<Approved1095FinalPart3> Approved1095FinalPart3s { get; set; }

        public IDbSet<UserReviewed> UserReviews { get; set; }

        public IDbSet<FileCabinetInfo> FileCabinetInfo { get; set; }

        public IDbSet<SsisFileTransfer> SsisFileTransfer { get; set; }

        public IDbSet<FileCabinetFolderInfo> FileCabinetFolderInfo { get; set; }

        public IDbSet<FileCabinetAccess> FileCabinetAccess { get; set; }


        /// <summary>
        /// Force EF from defaults into the AFA environment.
        /// </summary>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            ////This sets the table names to use singular names instead of plural names correctly for SQL creation.
            base.OnModelCreating(modelBuilder);

            setReportingValues(modelBuilder);

        }
        private void setReportingValues(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<TimeFrame>()
                .Property(timeframe => timeframe.ID)
                .HasColumnName("TimeFrameId");

            modelBuilder.Entity<Transmission1095>()
                .Property(Transmission => Transmission.ID)
                .HasColumnName("Transmission1095Id");

            modelBuilder.Entity<Transmission1094>()
                .Property(Transmission => Transmission.ID)
                .HasColumnName("Transmission1094Id");

            modelBuilder.Entity<Void1095>()
                .Property(voided => voided.ID)
                .HasColumnName("Void1095Id");

            modelBuilder.Entity<Void1094>()
                .Property(voided => voided.ID)
                .HasColumnName("Void1094Id");

            modelBuilder.Entity<PrintBatch>()
                .Property(batch => batch.ID)
                .HasColumnName("PrintBatchId");

            modelBuilder.Entity<SsisFileTransfer>()
               .Property(SSISFileTransfer => SSISFileTransfer.ID)
               .HasColumnName(" SSISFileTransferId");

            modelBuilder.Entity<Print1095>()
                .Property(print => print.ID)
                .HasColumnName("Print1095Id");

            modelBuilder.Entity<Print1094>()
                .Property(print => print.ID)
                .HasColumnName("Print1094Id");

            modelBuilder.Entity<Correction1095>()
                .Property(correction => correction.ID)
                .HasColumnName("Correction1095Id");

            modelBuilder.Entity<Correction1094>()
                .Property(correction => correction.ID)
                .HasColumnName("Correction1094Id");

            modelBuilder.Entity<UserEditPart2>()
                .Property(edit => edit.ID)
                .HasColumnName("UserEditPart2Id");

            modelBuilder.Entity<Approved1095Initial>()
                .Property(approve => approve.ID)
                .HasColumnName("Approved1095InitialId");

            modelBuilder.Entity<Approved1095Final>()
                .Property(approve => approve.ID)
                .HasColumnName("Approved1095FinalId");

            modelBuilder.Entity<Approved1095FinalPart2>()
                .Property(approve => approve.ID)
                .HasColumnName("Approved1095FinalPart2Id");

            modelBuilder.Entity<Approved1095FinalPart3>()
                .Property(approve => approve.ID)
                .HasColumnName("Approved1095FinalPart3Id");

            modelBuilder.Entity<UserReviewed>()
                .Property(approve => approve.ID)
                .HasColumnName("UserReviewedId");

            modelBuilder.Entity<Approved1094FinalPart1>()
            .Property(approved1094FinalPart1 => approved1094FinalPart1.ID)
             .HasColumnName("Approved1094FinalPart1Id");

            modelBuilder.Entity<Approved1094FinalPart3>()
               .Property(approved1094FinalPart3 => approved1094FinalPart3.ID)
               .HasColumnName("Approved1094FinalPart3Id");

            modelBuilder.Entity<Approved1094FinalPart4>()
               .Property(approved1094FinalPart4 => approved1094FinalPart4.ID)
               .HasColumnName("Approved1094FinalPart4Id");

            modelBuilder.Entity<FileCabinetInfo>()
              .ToTable("FileCabinetInfo")
              .Property(FileCabinetInfo => FileCabinetInfo.ID)
              .HasColumnName("FileCabinetInfoID");

            modelBuilder.Entity<FileCabinetFolderInfo>()
                .ToTable("FileCabinetFolderInfo")
                .Property(FileCabinetFolderInfo => FileCabinetFolderInfo.ID)
                .HasColumnName("FileCabinetFolderInfoID");

            modelBuilder.Entity<FileCabinetFolderInfo>()
             .HasMany(children => children.children)
             .WithOptional()
             .Map(m => m.MapKey("ParentFolderId"));

            modelBuilder.Entity<FileCabinetAccess>()
                .ToTable("FileCabinetAccess")
                .Property(FileCabinetAccess => FileCabinetAccess.ID)
                .HasColumnName("FileCabinetAccessID");

            modelBuilder.Entity<Approved1094FinalPart3>()
                    .HasRequired(approved1094FinalPart3 => approved1094FinalPart3.Approved1094FinalPart1)
                    .WithMany(approved1094FinalPart1 => (approved1094FinalPart1.Approved1094FinalPart3s))
                    .Map(map => map.MapKey("Approved1094FinalPart1Id"));

            modelBuilder.Entity<Approved1094FinalPart4>()
                        .HasRequired(approved1094FinalPart4 => approved1094FinalPart4.Approved1094FinalPart1)
                        .WithMany(approved1094FinalPart1 => (approved1094FinalPart1.Approved1094FinalPart4s))
                        .Map(map => map.MapKey("Approved1094FinalPart1Id"));
        }

        protected ILogger Log { private set; get; }
    }
}
