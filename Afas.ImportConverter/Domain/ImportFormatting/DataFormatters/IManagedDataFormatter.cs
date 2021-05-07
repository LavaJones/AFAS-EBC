using System;

namespace Afas.ImportConverter.Domain.ImportFormatting
{
    /// <summary>
    /// Manages the different types of Data Formatters.
    /// </summary>
    public interface IManagedDataFormatter
    {
        /// <summary>
        /// Formats the provided data using the specified formatter type and optional format
        /// </summary>
        /// <param name="data">Data to format</param>
        /// <param name="type">Type of fomatter to use</param>
        /// <param name="format">Optional specified format</param>
        /// <returns>THe formatted string or the origonal data if unable to format</returns>
        string FormatData(string data, string formatterType, string format = null);
    }
}
