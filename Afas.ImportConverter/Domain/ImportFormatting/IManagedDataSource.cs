using Afas.ImportConverter.Domain.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.ImportConverter.Domain.ImportFormatting
{

    /// <summary>
    /// Defines a Managed Data Source Access to simplify access accross multiple tables and types.
    /// </summary>
    public interface IManagedDataSource
    {

        /// <summary>
        /// Gets the list of valid values for the provided values source
        /// </summary>
        /// <param name="ValueSource">The data source to check</param>
        /// <param name="metaData">MetaData that is related to the request.</param>
        /// <returns>The list of valid values.</returns>
        IList<string> GetPossibleValues(string ValueSource, ImportData metaData);

    }
}
