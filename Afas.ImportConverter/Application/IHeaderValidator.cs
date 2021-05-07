using System;

namespace Afas.ImportConverter.Application
{

    /// <summary>
    /// Validates Data Table Headers
    /// </summary>
    public interface IHeaderValidator
    {

        /// <summary>
        /// Check if the header row looks like a real header or contains actual data
        /// </summary>
        /// <param name="headers">The Row To Check</param>
        /// <returns>True if the row looks liek a header, false if it looks like data</returns>
        bool ValidateHeaders(System.Collections.Generic.IEnumerable<string> headers);

    }

}
