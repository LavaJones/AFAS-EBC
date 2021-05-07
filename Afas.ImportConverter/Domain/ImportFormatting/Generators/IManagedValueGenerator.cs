using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.ImportConverter.Domain.ImportFormatting.Generators
{

    /// <summary>
    /// This code manages the task of Default Value Generator
    /// </summary>
    public interface IManagedValueGenerator
    {
        
        /// <summary>
        /// Generates a Default Value for the Row based on the Generator Type, Pattern, and the Row data itself
        /// </summary>
        /// <param name="GeneratorType">The Type of generator toy use.</param>
        /// <param name="GeneratorPattern">The individual pattern to generate</param>
        /// <param name="row">The Source Data for the Row.</param>
        /// <returns>The generated Value</returns>
        string GenerateDefaultValue(string GeneratorType, string GeneratorPattern, System.Data.DataRow row);
    
    }
}
