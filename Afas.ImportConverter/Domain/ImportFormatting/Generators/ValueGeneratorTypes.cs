using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.ImportConverter.Domain.ImportFormatting.Generators
{
    /// <summary>
    /// Defines the Type of Value that is generated
    /// </summary>
    public enum ValueGeneratorTypes
    {
        /// <summary>
        /// Generates a random Guid
        /// </summary>
        RandomGuid,

        /// <summary>
        /// Generates a number using a standard random process
        /// </summary>
        RandomNumber,

        /// <summary>
        /// Generates a number based on the time in ticks that should be unique
        /// </summary>
        UniqueNumber,

        /// <summary>
        /// Use the Current date as part of the Default Value
        /// </summary>
        UniqueDate,

        /// <summary>
        /// Simple Format where values for multiple columns are combined to create the default value
        /// </summary>
        ValueCombination,

    }
}
