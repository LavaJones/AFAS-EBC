using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Afas.ImportConverter.Domain.ImportFormatting.Generators.Implementation
{

    /// <summary>
    /// Generates Vaules using a Random Number Generatator
    /// </summary>
    public class UniqueDateValueGenerator : IValueGenerator
    {

        /// <summary>
        /// Genereate a Random Number Value without a specific format
        /// </summary>
        /// <returns>Random Number Value </returns>
        public String GenerateValue()
        {
            return DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss.fff");
        }

        /// <summary>
        /// Genereate a Random Number Value based on a specific format
        /// </summary>
        /// <param name="pattern">The format to add Random numbers to.</param>
        /// <returns>Formatted Random Number Value</returns>
        public String GenerateValue(string pattern)
        {

            return Regex.Replace(pattern, Regex.Escape(GetPlaceholder), (match) => DateTime.UtcNow.ToString("yyyy-MM-dd_HH:mm:ss.fff"));

        }

        /// <summary>
        /// This is the placeholder for replacement based on this Generator
        /// </summary>
        public String GetPlaceholder { get { return "[DATE]"; } }

    }
}
