using Afas.ImportConverter.Domain.ImportFormatting;
using Afas.ImportConverter.Domain.ImportFormatting.DataFormatters;
using Afas.ImportConverter.Domain.ImportFormatting.DataFormatters.Implementation;
using Afas.ImportConverter.Domain.ImportFormatting.Generators;
using Afas.ImportConverter.Domain.ImportFormatting.Generators.Implementation;
using Afas.ImportConverter.Domain.POCO;
using Afc.Core.Application;
using Afc.Framework.Application;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace Afas.ImportConverter
{
    public class ImportConverterInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {

        }
    }
}