using Afas.ImportConverter.Application.Implementation;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Afas.ImportConverter.Application
{
    public class ImportConverterApplicationInstaller : IWindsorInstaller
    {
        [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {

          
            container.Register(
                Component.For<ICsvDataTableBuilder, ICsvFileDataTableBuilder>()
                .ImplementedBy<CsvDataTableBuilder>()
                .LifestyleTransient());

            container.Register(
                Component.For<IXlsxFileDataTableBuilder>()
                .ImplementedBy<XlsxDataTableBuilder>()
                .LifestyleTransient());

            container.Register(
                 Component.For<IDataProcessor>()
                 .ImplementedBy<DataProcessor>()
                 .LifestyleTransient());

            container.Register(
                 Component.For<IDataTableBuilder>()
                 .ImplementedBy<DataTableBuilder>()
                 .LifestyleTransient());

            container.Register(
                 Component.For<IDataValidationManager>()
                 .ImplementedBy<DataValidationManager>()
                 .LifestyleTransient());

        }
    }
}