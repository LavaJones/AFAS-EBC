using Afas.AfComply.Application.Validators;
using Afas.ImportConverter.Application;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Application
{
    public class AfComplyApplicationInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {

            container.Register(Component
                .For<IDataValidator>()
                .ImplementedBy<FeinDataValidator>()
                .LifestyleTransient());

            

            
         }
    }
}
