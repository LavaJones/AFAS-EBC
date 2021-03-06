using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Castle.Windsor;

namespace Afas.AfComply.Reporting.Api.Plumbing
{

    /// <summary>
    /// Lifted from Cidm.Api.Plumbing.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class WindsorDependencyScope : IDependencyScope
    {

        protected readonly IWindsorContainer _container;
        private ConcurrentBag<Object> _toBeReleased = new ConcurrentBag<Object>();

        public WindsorDependencyScope(IWindsorContainer container)
        {
            _container = container;
        }

        public void Dispose()
        {

            if (_toBeReleased != null)
            {

                foreach (var o in _toBeReleased)
                {
                    _container.Release(o);
                }

            }

            _toBeReleased = null;

        }

        public object GetService(Type serviceType)
        {

            if (!_container.Kernel.HasComponent(serviceType))
                return null;

            var resolved = _container.Resolve(serviceType);

            if (resolved != null)
                _toBeReleased.Add(resolved);

            return resolved;

        }

        public IEnumerable<object> GetServices(Type serviceType)
        {

            if (!_container.Kernel.HasComponent(serviceType))
                return new object[0];


            var allResolved = _container.ResolveAll(serviceType).Cast<object>();

            if (allResolved != null)
            {
                allResolved.ToList()
                    .ForEach(x => _toBeReleased.Add(x));
            }

            return allResolved;

        }

    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class WindsorResolver : IDependencyResolver
    {

        private readonly IWindsorContainer _container;

        public WindsorResolver(IWindsorContainer container)
        {
            _container = container;
        }

        public IDependencyScope BeginScope()
        {
            return new WindsorDependencyScope(_container);
        }

        public void Dispose()
        {
            _container.Dispose();
        }

        public object GetService(Type serviceType)
        {

            if (!_container.Kernel.HasComponent(serviceType))
                return null;

            return _container.Resolve(serviceType);

        }

        public IEnumerable<object> GetServices(Type serviceType)
        {

            if (!_container.Kernel.HasComponent(serviceType))
                return new object[0];

            return _container.ResolveAll(serviceType).Cast<object>();

        }

    }

}
