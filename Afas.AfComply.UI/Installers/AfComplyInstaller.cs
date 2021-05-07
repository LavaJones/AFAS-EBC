using Afas.AfComply.UI.Code.AFcomply.DataAccess;
using Afas.AfComply.UI.Code.AFcomply.DataUpload;
using Afas.ImportConverter.Application;
using Afas.Application.Archiver;
using Afc.Core.Presentation.Web;
using Afc.Framework.Presentation.Web;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System.Web.Mvc;
using Afas.Domain.POCO;
using Afc.Core.Domain;
using Afas.Application.Services;

namespace Afas.AfComply.UI.Installers
{
    public class AfComplyInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {

            container.Register(Component
                .For<AfComplyFileDataImporter>()
                .LifestyleTransient());

            container.Register(Component
                .For<ReportingFileDataImporter>()
                .LifestyleTransient());
            
            container.Register(Component
                .For<IHeaderValidator>()
                .ImplementedBy<HeaderValidator>()
                .LifestyleTransient());

            /*
             * 



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
                */
        }
    }
}