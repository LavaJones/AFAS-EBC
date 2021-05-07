using Afas.Application.Archiver;
using Afas.Application.CSV;
using Afas.Application.FileAccess;
using Afas.Application.Services;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.Application
{
     
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class AfasApplicationInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {

            container.Register(
                Component.For<IFileArchiver>()
                .ImplementedBy<FileArchiver>()
                .LifestyleTransient());

            container.Register(
                Component.For<IFileAccess>()
                .ImplementedBy<FileAccess.FileAccess>()
                .LifestyleTransient());

            container.Register(
                Component.For<ICsvParser>()
                .ImplementedBy<CsvParse>()
                .LifestyleTransient());
            
            //container.Register(
            //    Component.For<IArchiveFileInfoAccess, IArchiveFileInfoService>()
            //    .ImplementedBy<ArchiveFileInfoService>()
            //    .LifestyleTransient());

        }
    }
}
