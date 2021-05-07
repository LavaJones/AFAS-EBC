using Afc.Core.Application;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace $rootnamespace$.Installers
{
    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyInThisApplication()
                .BasedOn<BaseService>()
                .WithServiceAllInterfaces()
                .LifestyleTransient());
        }
    }
}