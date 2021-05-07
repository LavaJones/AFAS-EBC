using Afas.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Domain.POCO
{
    /// <summary>
    /// Lists all the commands that can act on the data in an import
    /// </summary>
    public enum ImputFormatCommandTypes 
    {
        /// <summary>
        /// Map multiple column names to a single Column Name
        /// </summary>
        StandardizeColumnName, 

        /// <summary>
        /// Completly remove this column
        /// </summary> 
        DeleteColumn,

        /// <summary>
        /// Change the header of this column
        /// </summary>
        RenameColumn,
        
        /// <summary>
        /// Set a default value for a column if it is null 
        /// </summary>
        DefaultColumn,

        /// <summary>
        /// Turn multiple rows of format A into multiple columns in format B
        /// </summary>
        PivotColumnToRows,

        /// <summary>
        /// Turn multiple columns in format A into multiple rows in format B
        /// </summary>
        PivotRowsToColumns,

        /// <summary>
        /// Mutate the data in some consistent way
        /// </summary>
        ConvertColumn, 

        /// <summary>
        /// Run column through a sanitization procedure
        /// </summary>
        SanitizeColumn,

    }

    /// <summary>
    /// A generic Formatting Command that can be strung together to format the input data
    /// </summary>
    public class ImputFormatCommand : BaseAfasModel
    {
        /// <summary>
        /// Database PK for this object
        /// </summary>
        public int ImportFormatCommandId { get; set; }

        /// <summary>
        /// The order in which this command should be run, ex. ordinal 5, would be the 5th command run 
        /// </summary>
        public int Ordinal { get; set; }
        
        /// <summary>
        /// The Type of command that this is
        /// </summary>
        public ImputFormatCommandTypes command { get; set; }

        /// <summary>
        /// The Name of the column that is targeted by this command or null if it doesn't target a specific column
        /// </summary>
        public string TargetColumnName{ get; set; }

        /// <summary>
        /// Key value Pair for parameters of the command, key is the parameter name, value is the string value.
        /// </summary>
        public Dictionary<string, string> Parameters { get; set; }
    }
}
