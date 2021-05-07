using Afas.Domain;
using Afas.Domain.POCO;
using Afas.ImportConverter.Domain.ImportFormatting;
using Afas.ImportConverter.Domain.ImportFormatting.Staging;
using Afas.ImportConverter.Domain.ImportFormatting.UploadedData;
using Afas.ImportConverter.Domain.POCO;
using Afc.Core.Domain;
using Afc.Core.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.ImportConverter.Domain
{

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ImportConverterDataContext : AfasDataContext, IImportConverterDataContext
    {

        protected ILogger Log { private set; get; }

        public ImportConverterDataContext() : base()
        {
            if (Afc.Core.Logging.LogManager.LoggerFactory != null)
            {
                this.Log = Afc.Core.Logging.LogManager.GetLogger("Internal.Afas.ImportConverter.Domain.EfLoggingAdapter");
                if (this.Log.IsDebugEnabled)
                {
                    this.Database.Log = (logStatement => this.Log.Info(logStatement));
                }
            }
            this.Context.Database.CommandTimeout = 240;
            this.Context.Configuration.UseDatabaseNullSemantics = true;                 
        }

        public IDbSet<AImportFormatCommand> ImportFormatCommands { get; set; }
        public IDbSet<ImportData> ImportDatas { get; set; }
        public IDbSet<ImportFormatCommandScope> ImportFormatCommandScopes { get; set; }
        public IDbSet<ImportFormatFileCommandScope> ImportFormatFileCommandScopes { get; set; }
        public IDbSet<ImportMetaData> ImportMetaDatas { get; set; }
        public IDbSet<ImportFileMetaData> ImportFileMetaDatas { get; set; }
        public IDbSet<StagingImport> StagingImports { get; set; }
        public IDbSet<BaseUploadedDataInfo> UploadedDataInfos { get; set; }
        public IDbSet<UploadedFileInfo> UploadedFileInfos { get; set; }
        
        /// <summary>
        /// Force EF from defaults into the AFA environment.
        /// </summary>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            ////This sets the table names to use singular names instead of plural names correctly for SQL creation.
            ////This sets the DateTime properties to use DateTime2 correctly for SQL creation.
            base.OnModelCreating(modelBuilder);

            setImportConverterValues(modelBuilder);
        }

        private void setImportConverterValues(DbModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<BaseUploadedDataInfo>()
                .Property(item => item.CreatedDate)
                .HasColumnType("datetime2");

            modelBuilder.Entity<BaseUploadedDataInfo>()
                .Property(item => item.ModifiedDate)
                .HasColumnType("datetime2");

            modelBuilder.Entity<BaseUploadedDataInfo>()
                 .Property(item => item.ProcessingFailed)
                 .HasColumnName("FailedProcessing");

            modelBuilder.Entity<UploadedDataInfo>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("UploadedDataInfo");
            });

            modelBuilder.Entity<UploadedFileInfo>().Map(m =>
            {
                m.MapInheritedProperties();
                m.ToTable("UploadedFileInfo");
            });

            modelBuilder.Entity<UploadedDataInfo>()
                .ToTable("UploadedDataInfo")
                .Property(item => item.ID)
                .HasColumnName("UploadedDataInfoId")
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
            
            modelBuilder.Entity<UploadedFileInfo>()
                .ToTable("UploadedFileInfo")
                .Property(item => item.ID)
                .HasColumnName("UploadedFileInfoId")
                .HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<UploadedFileInfo>()
                .Property(item => item.UploadOwnerId)
                .HasColumnName("EmployerId");

            modelBuilder.Entity<UploadedFileInfo>()
                .HasOptional(item => item.ArchiveFileInfo)
                .WithMany()
                .Map(m => m.MapKey("ArchiveFileInfoId"));


            modelBuilder.Entity<StagingImport>().ToTable("StagingImport");

            modelBuilder.Entity<StagingImport>()
                .Property(item => item.ID)
                .HasColumnName("StagingImportId");

            modelBuilder.Entity<StagingImport>()
                .HasRequired(item => item.UploadInfo)
                .WithMany()
                .Map(m => m.MapKey("UploadedFileInfoId"));

            modelBuilder.Entity<StagingImport>()
                .Ignore(item => item.Original)
                .Property(item => item.OriginalXml)
                .HasColumnType("xml")
                .IsRequired()
                .HasColumnName("Original");

            modelBuilder.Entity<StagingImport>()
                .Ignore(item => item.Modified)
                .Property(item => item.ModifiedXml)
                .HasColumnType("xml")
                .IsOptional()
                .HasColumnName("Modify");

            modelBuilder.Entity<StagingImport>()
                .Property(item => item.CreatedDate)
                .HasColumnType("datetime2");

            modelBuilder.Entity<StagingImport>()
                .Property(item => item.ModifiedDate)
                .HasColumnType("datetime2");
            
            modelBuilder.Entity<ImportData>()
                .ToTable("ImportData")
                .Ignore(item => item.Data)
                .Property(item => item.ID)
                .HasColumnName("ImportDataId");

            modelBuilder.Entity<ImportData>()
                .Property(item => item.DataXml)
                .HasColumnType("xml")
                .HasColumnName("Data");

            modelBuilder.Entity<ImportFormatCommandScope>()
                .ToTable("ImportFormatCommandScope")
                .Property(item => item.ID)
                .HasColumnName("ImportFormatCommandScopeId");

            modelBuilder.Entity<ImportFormatFileCommandScope>()
                .ToTable("ImportFormatFileCommandScope")
                .Property(item => item.ID)
                .HasColumnName("ImportFormatFileCommandScopeId");

            modelBuilder.Entity<ImportMetaData>()
                .ToTable("ImportMetaData")
                .Property(item => item.ID)
                .HasColumnName("ImportMetaDataId");

            modelBuilder.Entity<ImportFileMetaData>()
                .ToTable("ImportFileMetaData")
                .Property(item => item.ID)
                .HasColumnName("ImportFileMetaDataId");

            modelBuilder.Entity<AImportFormatCommand>()
                .ToTable("ImportFormatCommands")
                .Property(item => item.ID)
                .HasColumnName("ImportFormatCommandId");

            modelBuilder.Entity<AddNewColumnFormatCommand>();
            modelBuilder.Entity<AutoGenerateDefaultValuesFormatCommand>();
            modelBuilder.Entity<DefaultColumnSingleFormatCommand>();
            modelBuilder.Entity<DefaultColumnValueFormatCommand>();
            modelBuilder.Entity<DeleteByValueFormatCommand>();
            modelBuilder.Entity<DeleteColumnFormatCommand>();
            modelBuilder.Entity<DeleteColumnIfBlankFormatCommand>();
            modelBuilder.Entity<DeleteRowsFormatCommand>();
            modelBuilder.Entity<FormatColumnValuesFormatCommand>();
            modelBuilder.Entity<MergeColumnsFormatCommand>();
            modelBuilder.Entity<RenameColumnFormatCommand>();
            modelBuilder.Entity<ReplaceByValueFormatCommand>();
            modelBuilder.Entity<UnifyColumnsFormatCommand>();


        }
    }
}
