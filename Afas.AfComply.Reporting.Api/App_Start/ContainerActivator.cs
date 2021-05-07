using Afc.Core.Container;
using Afc.Framework.Container;
using Castle.Windsor;
using Castle.Windsor.Installer;
using WebActivatorEx;

[assembly: PreApplicationStartMethod(typeof(Afas.AfComply.Reporting.Api.App_Start.ContainerActivator), "PreStart")]
[assembly: ApplicationShutdownMethodAttribute(typeof(Afas.AfComply.Reporting.Api.App_Start.ContainerActivator), "Shutdown")]

namespace Afas.AfComply.Reporting.Api.App_Start
{
    public static class ContainerActivator
    {
        public static IContainer _container;

        public static void PreStart()
        {
            _container = new WindsorContainerAdapter(
                FrameworkFeatures.DataAccess | 
                FrameworkFeatures.DeclarativeTransactions |
                FrameworkFeatures.Logging, 
                "dependencies.config");

            var windsor = _container.Resolve<IWindsorContainer>();
            windsor.Install(FromAssembly.InThisApplication());


            Mapping.ConfigureMapper();
        }

        public static void Shutdown()
        {
            if (_container != null)
            {
                _container.Dispose();
            }
        }
    }
}