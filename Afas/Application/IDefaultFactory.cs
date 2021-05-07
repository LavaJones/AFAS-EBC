using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.Application
{

    /// <summary>
    /// A generic Factory for DI
    /// </summary>
    /// <typeparam name="TDependency">The Type to be resolved.</typeparam>
    public interface IDefaultFactory<TDependency>
    {

        TDependency Create();

        TType Create<TType>() where TType: TDependency;

        TDependency GetById(string value);

        void Release(TDependency value);

    } 

}
