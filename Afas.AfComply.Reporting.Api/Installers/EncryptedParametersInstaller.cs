using Afc.Core.Presentation.Web;
using Afc.Framework.Presentation.Web;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System.Web.Mvc;

namespace Afas.AfComply.Reporting.Api.Installers
{
    public class EncryptedParametersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component
                .For<IEncryptedParameters>()
                .ImplementedBy<EncryptedParameters>()
                .LifestyleTransient());

            container.Register(Component
                .For<ValueProviderFactory>()
                .ImplementedBy<EncryptedParametersValueProvider>()
                .LifestyleTransient());

            container.Register(Component
                .For<EncryptedParametersConstraint>()
                .LifestyleTransient());
        }
    }
}