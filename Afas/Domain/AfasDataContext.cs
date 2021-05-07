using Afas.Domain.POCO;
using Afc.Core.Domain;
using Afc.Core.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.Domain
{

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class AfasDataContext : BaseDbContext, IAfasDataContext
    {
        protected ILogger Log { private set; get; }

        public AfasDataContext() : base()
        {
            if (Afc.Core.Logging.LogManager.LoggerFactory != null)
            {
                this.Log = Afc.Core.Logging.LogManager.GetLogger("Internal.Afas.Domain.EfLoggingAdapter");
                if (this.Log.IsDebugEnabled)
                {
                    this.Database.Log = (logStatement => this.Log.Info(logStatement));
                }
            }

            this.Context.Database.CommandTimeout = 240;
            //this.Configuration.ProxyCreationEnabled = false;
            //this.Configuration.LazyLoadingEnabled = false;
            this.Context.Configuration.UseDatabaseNullSemantics = true; // If we set this to false then it does weird or 'AND (@p__linq__0 IS NOT NULL)'

        }

        public IDbSet<ArchiveFileInfo> ArchiveFileInfos { get; set; }

        //public IDbSet<> s { get; set; }

        /// <summary>
        /// Force EF from defaults into the AFA environment.
        /// </summary>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            ////This sets the table names to use singular names instead of plural names correctly for SQL creation.
            //modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            ////This sets the DateTime properties to use DateTime2 correctly for SQL creation.
            //modelBuilder.Properties<DateTime>().Configure(property => property.HasColumnType("datetime2"));

            //modelBuilder.Properties<Guid>()
            //        .Where(property => property.Name == "ResourceId")
            //        .Configure(column => column.HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("Index") { IsUnique = true })));

            // Chaining them together, not sure I like it but what are you supposed to do?
            base.OnModelCreating(modelBuilder);

            setAfasValues(modelBuilder);
        }
        private void setAfasValues(DbModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<ArchiveFileInfo>()
                .ToTable("ArchiveFileInfo")
                .Property(item => item.ArchiveFileInfoId)
                .HasColumnName("ArchiveFileInfoId");

            modelBuilder.Entity<ArchiveFileInfo>()
                .Property(item => item.CreatedDate)
                .HasColumnType("datetime2");

            modelBuilder.Entity<ArchiveFileInfo>()
                .Property(item => item.ModifiedDate)
                .HasColumnType("datetime2");

        }
    }
}
