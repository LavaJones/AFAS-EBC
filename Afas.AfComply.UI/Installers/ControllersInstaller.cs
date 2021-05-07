using Afc.Framework.Presentation.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using System;
using System.Web.Mvc;

using Castle.Windsor;

using Afas.Application;
using Afc.Marketing;
using Afc.Marketing.Integration;

using Afc.Core.Encryption;
using Afas.AfComply.UI.Plumbing;
using Castle.Facilities.TypedFactory;
using Afas.Application.Archiver;

namespace Afas.AfComply.UI.Installers
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
                .BasedOn<IController>()
                .LifestyleTransient());

            container.Register(Component.For<IHasher>().ImplementedBy(typeof(Afc.Framework.Encryption.Sha2)));
            container.Register(Component.For<IApiHelper>().ImplementedBy(typeof(ApiHelper)));
            container.Register(Component.For<IRuntimeConfiguration>().ImplementedBy(typeof(RuntimeConfiguration)));

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(container));
			DependencyResolver.SetResolver(new WindsorDependencyResolver(container));
        }
    }
}