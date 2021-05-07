using System;

using System.Data;

namespace Afas.AfComply.Application
{

    /// <summary>
    /// Converts different String sources into the proper DataTable structures.
    /// Cleans up fields with extra double quotes and removes any row that has no values across any fields.
    /// </summary>
    public interface ICsvConverterService
    {

        /// <summary>
        /// Generic CSV -> DataTable conversion.
        /// </summary>
        DataTable Convert(String[] source);

    }

}
