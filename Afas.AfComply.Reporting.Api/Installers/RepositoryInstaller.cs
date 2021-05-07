using Afc.Core.Domain;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Afas.AfComply.Reporting.Api.Installers
{
    public class RepositoryInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromAssemblyInThisApplication()
                .BasedOn(typeof(IRepository<>))
                .OrBasedOn(typeof(IEntityRepository<>))
                .WithServiceDefaultInterfaces()
                .LifestyleTransient());

            container.Register(Classes.FromAssemblyInThisApplication()
                .BasedOn<IDbContext>()
                .WithServiceSelf()
				.WithServiceDefaultInterfaces()
                .LifestyleTransient());

        }
    }
}