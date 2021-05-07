using Afc.Framework.Presentation.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Afas.AfComply.Reporting.Api.Plumbing;
using System.Web.Mvc;
using System.Web.Http.Controllers;
using Afc.Marketing;
using System.Web.Http.Dispatcher;
using Afc.Marketing.Framework.WebApi.Plumbing;
using Afc.Marketing.Integration;
using Afas.Application;
using Castle.Facilities.TypedFactory;
using Afas.Application.Archiver;

namespace Afas.AfComply.Reporting.Api.Installers
{
    public class ControllersInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<TypedFactoryFacility>();
            

            container.Register(Component.For<IGenericFactory>().AsFactory());
            container.Register(Component.For(typeof(IDefaultFactory<>)).AsFactory());


            container.Register(
                Component.For<IArchiveLocationProvider>()
                .ImplementedBy<MachineConfigProvider>());


            container.Register(Classes.FromThisAssembly()
                .BasedOn<IHttpController>()
                .LifestyleTransient());

            container.Register(Component.For<IRuntimeConfiguration>().ImplementedBy(typeof(RuntimeConfiguration)));

            container.Register(
                    Component.For<IHttpControllerActivator>()
                    .Instance(new WindsorControllerActivator(Afas.AfComply.Reporting.Api.App_Start.ContainerActivator._container.Resolve<IWindsorContainer>()))
                );

        }
    }
}