using Afas.ImportConverter.Domain.POCO;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Afas.Domain;

namespace Afas.ImportConverter.Domain.ImportFormatting
{
    /// <summary>
    /// This class implemements the Rename Command, 
    /// that takes one column name and if it finds it then it renames it to specified name.
    /// This is a very specific item used only when a single name is needed.
    /// </summary>
    public class RenameColumnFormatCommand : AImportFormatCommand
    {
        private ILog Log = LogManager.GetLogger(typeof(RenameColumnFormatCommand));

        /// <summary>
        /// The Type of command that this is
        /// </summary>
        public override ImportFormatCommandTypes CommandType
        {
            // This is hard coded because we are using the strategy pattern (ish) 
            get { return ImportFormatCommandTypes.RenameColumn; }
        }

        /// <summary>
        /// A list of the Required Parameters that the Command cannot function without.
        /// </summary>
        public override IList<string> RequiredParameters { get { return new List<string> { "RenameFrom", "RenameTo" }; } }

        /// <summary>
        /// A list of optional Parameters that the Command can use but are not Required.
        /// </summary>
        public override IList<string> OptionalParameters { get { return new List<string>(); } }
        
        /// <summary>
        /// Default Constructor with No arguments
        /// </summary>
        public RenameColumnFormatCommand()
            : base() 
        {
            // Use default behavior
        }

        /// <summary>
        /// Apply the Rename Command to format this data and rename a column if it exists
        /// </summary>
        /// <param name="metaData">The metaData, including the data to be formatted.</param>
        /// <returns>True if the format was applied correctly, false if there was a failure.</returns>
        public override bool ApplyTo(ImportData metaData)
        {
            try
            {
                System.Data.DataTable data = metaData.Data;

                // The first param should be the header to unify it From.
                string RenameFrom = Parameters["RenameFrom"];
                // The Last param should be the header to unify it To.
                string RenameTo = Parameters["RenameTo"];

                if (RenameTo.IsNullOrEmpty()) 
                {
                    // Can't Rename to a blank or null value
                    return false;
                }

                // If one of the headers is found then rename it (also checks to avoid 2 columns with same name)
                if (data.Columns.Contains(RenameFrom) && false == data.Columns.Contains(RenameTo))
                {
                    Log.Debug("Renamed Column From [" + RenameFrom + "] To: " + RenameTo);

                    data.Columns[RenameFrom].ColumnName = RenameTo;

                    // once it has been found (and renamed) we are done, can't have multiple columns with same name. 
                    return true;
                }
                else 
                {
                    Log.Info("Table Did not contain column [" + RenameFrom + "] or did contain [" + RenameTo + "] for RenameColumnFormatCommand.");
                    return false;
                }
            }
            catch(Exception ex)
            {
                Log.Warn("Exception while trying to Apply RenameColumnFormatCommand to Data.", ex);
                return false;
            }

            return true;
        }
    }
}
