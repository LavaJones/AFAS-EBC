using System;
namespace Afas.ImportConverter.Domain.ImportFormatting.DataFormatters
{

    /// <summary>
    /// Interchageable Data Formatter 
    /// </summary>
    public interface IDataFormatter
    {

        /// <summary>
        /// Format the porvided data
        /// </summary>
        /// <param name="data">Data to be formatted.</param>
        /// <returns>Formatted data or the origonal if it could not be formatted.</returns>
        string FormatData(string data);

        /// <summary>
        /// Format the porvided data
        /// </summary>
        /// <param name="data">Data to be formatted.</param>
        /// <param name="format">Specifies the format to be returned as.</param>
        /// <returns>Formatted data or the origonal if it could not be formatted.</returns>
        string FormatData(string data, string format);

    }
}
