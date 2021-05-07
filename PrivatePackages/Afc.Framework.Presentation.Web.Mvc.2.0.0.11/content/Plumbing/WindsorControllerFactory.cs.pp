using Castle.Windsor;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace $rootnamespace$.Plumbing
{
    public class WindsorControllerFactory : DefaultControllerFactory
    {
        private readonly IWindsorContainer _container;

        public WindsorControllerFactory(IWindsorContainer container)
        {
            _container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                return base.GetControllerInstance(requestContext, controllerType);
            }
            else
            {
                return (IController)_container.Resolve(controllerType);
            }
        }

        public override void ReleaseController(IController controller)
        {
            _container.Release(controller);
        }
    }
}