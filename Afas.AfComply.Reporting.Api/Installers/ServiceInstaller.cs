using Afas.AfComply.Reporting.Application.Services.LegacyServices;
using Afc.Core.Application;
using Afc.Framework.Application;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Afas.AfComply.Reporting.Api.Installers
{
    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyInThisApplication()
                .BasedOn<BaseService>()
                .WithServiceAllInterfaces()
                .LifestyleTransient());

            container.Register(Classes.FromAssemblyInThisApplication()
                .BasedOn<LegacyService>()
                .WithServiceAllInterfaces()
                .LifestyleTransient());
            
        }
    }
}