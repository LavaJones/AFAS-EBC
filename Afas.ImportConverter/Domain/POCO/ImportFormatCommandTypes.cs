using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.ImportConverter.Domain.POCO
{

    /// <summary>
    /// Lists all the commands that can act on the data in an import
    /// </summary>
    public enum ImportFormatCommandTypes
    {
        /// <summary>
        /// Add a blank new column to be populated later
        /// </summary>
        AddNewColumn,

        /// <summary>
        /// Map multiple column names to a single Column Name
        /// </summary>
        UnifyColumnNames,

        /// <summary>
        /// Change the header of this column
        /// </summary>
        RenameColumn,

        /// <summary>
        /// Set a default value for a column to replace null 
        /// </summary>
        DefaultColumnIfBlank,

        /// <summary>
        /// Set a default value for a column if there is only one valid choice, only replaces null
        /// </summary>
        DefaultColumnIfSingleChoice,

        /// <summary>
        /// Fills null value's with an autogenerated value
        /// </summary>
        AutoGenerateDefaultValues,

        /// <summary>
        /// Combine the values from 2 columns into a single column
        /// </summary>
        MergeColumns,

        /// <summary>
        /// Replaces a Value in a column with another value.
        /// </summary>
        ReplaceByValue,

        /// <summary>
        /// Leave the data the same but store it a specific format
        /// </summary>
        FormatColumn,

        /// <summary>
        /// Mutate the data in some consistent way
        /// </summary>
        ConvertColumn, 

        /// <summary>
        /// Run column through a sanitization procedure
        /// </summary>
        SanitizeColumn, 

        /// <summary>
        /// Turn multiple rows of format A into multiple columns in format B
        /// </summary>
        PivotColumnToRows, 

        /// <summary>
        /// Turn multiple columns in format A into multiple rows in format B
        /// </summary>
        PivotRowsToColumns, 

        /// <summary>
        /// Completly remove this column
        /// </summary> 
        DeleteColumn,

        /// <summary>
        /// Remove matched values from a column replaceing it with null
        /// </summary> 
        DeleteByValue,

        /// <summary>
        /// Completly remove rows matching the parameters 
        /// </summary> 
        DeleteRows,

        /// <summary>
        /// Completly remove this column only if everything is null 
        /// </summary>
        DeleteColumnIfBlank,
    }

}
