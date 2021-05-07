using Afc.Framework.Presentation.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using $rootnamespace$.Plumbing;
using System.Web.Mvc;

namespace $rootnamespace$.Installers
{
    public class ControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
                .BasedOn<IController>()
                .LifestyleTransient());

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));
			DependencyResolver.SetResolver(new WindsorDependencyResolver(container));
        }
    }
}