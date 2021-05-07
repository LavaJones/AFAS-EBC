using Afas.ImportConverter.Domain.POCO;
using System;

namespace Afas.ImportConverter.Domain.ImportFormatting
{
    /// <summary>
    /// A generic Formatting Command that can be combined together to format the input data
    /// </summary>
    public interface IImportFormatCommand
    {
        /// <summary>
        /// Checks to see if this Command applys to an bject with this metadata
        /// </summary>
        /// <param name="other">The meta data to check.</param>
        /// <returns>True it applies, false it does not apply.</returns>
        bool AppliesTo(ImportMetaData other);

        /// <summary>
        /// Apply this Command to format this data
        /// </summary>
        /// <param name="metaData">The metaData, including the data to be formatted.</param>
        /// <returns>True if the format was applied correctly, false if there was a failure.</returns>
        bool ApplyTo(ImportData metaData);

        /// <summary>
        /// Implements the compare to Interface to compare and sort ImportFormatCommands
        /// </summary>
        /// <param name="other">The ImportFormatCommand to compare to.</param>
        /// <returns>
        /// Less than zero: This instance precedes obj in the sort order.
        /// Zero: This instance occurs in the same position in the sort order as obj.
        /// Greater than zero: This instance follows obj in the sort order.
        /// </returns>
        int CompareTo(IImportFormatCommand other);

        /// <summary>
        /// The Type of command that this is
        /// </summary>
        ImportFormatCommandTypes CommandType { get; }

        /// <summary>
        /// The Scope of that the Format Command applies to. 
        /// </summary>
        ImportFormatCommandScope Scope { get; set; }

        /// <summary>
        /// The Metadata defining what this command applies to.
        /// </summary>
        ImportMetaData MetaData { get; set; }

        /// <summary>
        /// Key value Pair for parameters of the command, key is the parameter name, value is the string value.
        /// </summary>
        System.Collections.Generic.IDictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// A list of the Required Parameters by Name that the Command cannot function without.
        /// </summary>
        System.Collections.Generic.IList<string> RequiredParameters { get; }

        /// <summary>
        /// A list of optional Parameters by Name that the Command can use but are not Required.
        /// </summary>
        System.Collections.Generic.IList<string> OptionalParameters { get; }
        
    }
}
