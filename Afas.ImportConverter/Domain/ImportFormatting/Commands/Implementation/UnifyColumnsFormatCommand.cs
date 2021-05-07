using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Afas.ImportConverter.Domain.POCO;
using Afas.Domain;

namespace Afas.ImportConverter.Domain.ImportFormatting
{
    /// <summary>
    /// This class implemements the Unify Command, that takes many possible column names and if it finds any it renames it to a single unified name.
    /// This is a general first pass or high level change that can bring many documents into line quickly.
    /// </summary>
    public class UnifyColumnsFormatCommand : AImportFormatCommand
    {
        private ILog Log = LogManager.GetLogger(typeof(UnifyColumnsFormatCommand));

        /// <summary>
        /// The Type of command that this is
        /// </summary>
        public override ImportFormatCommandTypes CommandType
        {
            // This is hard coded because we are using the strategy pattern (ish) 
            get { return ImportFormatCommandTypes.UnifyColumnNames; }
        }

        /// <summary>
        /// A list of the Required Parameters that the Command cannot function without.
        /// </summary>
        public override IList<string> RequiredParameters { get { return new List<string> { "UnifyTo" }; } }

        /// <summary>
        /// A list of optional Parameters that the Command can use but are not Required.
        /// </summary>
        public override IList<string> OptionalParameters { get { return new List<string>(); } }

        /// <summary>
        /// Default Constructor with No arguments
        /// </summary>
        public UnifyColumnsFormatCommand() : base() 
        {
            // Use default behavior
        }

        /// <summary>
        /// Apply the Unify Command to format this data and unify their columns
        /// </summary>
        /// <param name="metaData">The metaData, including the data to be formatted.</param>
        /// <returns>True if the format was applied correctly, false if there was a failure.</returns>
        public override bool ApplyTo(ImportData metaData)
        {
            try
            {
                System.Data.DataTable data = metaData.Data;

                // Get the header to unify it to.
                string UnifyTo = Parameters["UnifyTo"];

                if (UnifyTo.IsNullOrEmpty() || data.Columns.Contains(UnifyTo)) 
                {
                    // can't unify to a blank column and can't Unify to a name that already exists
                    Log.Info("Table did contain column [" + UnifyTo + "] for UnifyColumnsFormatCommand.");

                    return false;
                }

                // Loop through all possible headers
                foreach (string header in Parameters.Values) 
                {
                    // If one of the headers is found then rename it
                    if (data.Columns.Contains(header)) 
                    {
                        Log.Debug("Unified Column From [" + header + "] To: " + UnifyTo);

                        data.Columns[header].ColumnName = UnifyTo;
                        
                        // once it has been found (and renamed) we are done, can't have multiple columns renamed to the same name. 
                        return true;
                    }
                }
            }
            catch(Exception ex)
            {
                Log.Warn("Exception while trying to Apply UnifyColumnsFormatCommand to Data.", ex);
                return false;
            }

            return true;
        }
    }
}
