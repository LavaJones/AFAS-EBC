using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.Domain
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class AfasDomainInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            /*
            container.Register(
                Component.For<IManagedValueGenerator>()
                .ImplementedBy<ManagedValueGenerator>());
            container.Register(
                Component.For<IManagedDataFormatter>()
                .ImplementedBy<ManagedDataFormatter>());
            */
        }
    }
}