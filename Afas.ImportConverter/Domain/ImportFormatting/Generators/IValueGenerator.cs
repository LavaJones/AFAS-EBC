using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.ImportConverter.Domain.ImportFormatting.Generators
{
    /// <summary>
    /// Interface defining methods for Generating default Values
    /// </summary>
    public interface IValueGenerator
    {
        /// <summary>
        /// Gets the Placeholder value for this Generator
        /// </summary>
        String GetPlaceholder { get; }

        /// <summary>
        /// Generates a Value
        /// </summary>
        /// <returns>A Value</returns>
        String GenerateValue();

        /// <summary>
        /// Gnereates 
        /// </summary>
        /// <param name="pattern">A Format Pattern to use</param>
        /// <returns>A Formated Value</returns>
        String GenerateValue(string pattern);
    }
}
