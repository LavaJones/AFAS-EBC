using Afas.ImportConverter.Domain.POCO;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Afas.Domain;

namespace Afas.ImportConverter.Domain.ImportFormatting
{
    /// <summary>
    /// This class implemements the Delete Column Command,
    /// It will check if a column exists and if so, will remove the column
    /// </summary>
    public class DeleteColumnFormatCommand : AImportFormatCommand
    {
        private ILog Log = LogManager.GetLogger(typeof(DeleteColumnFormatCommand));

        /// <summary>
        /// The Type of command that this is
        /// </summary>
        public override ImportFormatCommandTypes CommandType
        {
            // This is hard coded because we are using the strategy pattern (ish) 
            get { return ImportFormatCommandTypes.DeleteColumn; }
        }

        /// <summary>
        /// A list of the Required Parameters that the Command cannot function without.
        /// </summary>
        public override IList<string> RequiredParameters { get { return new List<string> { "ColumnName" }; } }

        /// <summary>
        /// A list of optional Parameters that the Command can use but are not Required.
        /// </summary>
        public override IList<string> OptionalParameters { get { return new List<string>(); } }
        
        /// <summary>
        /// Default Constructor with No arguments
        /// </summary>
        public DeleteColumnFormatCommand()
            : base() 
        {
            // Use default behavior
        }

        /// <summary>
        /// Apply the Delete Column Command to remove columns
        /// </summary>
        /// <param name="metaData">The metaData, including the data to be formatted.</param>
        /// <returns>True if the format was applied correctly, false if there was a failure.</returns>
        public override bool ApplyTo(ImportData metaData)
        {
            try
            {
                System.Data.DataTable data = metaData.Data;

                // Get the column name to clear if it is empty
                string ColumnName = Parameters["ColumnName"];

                if (ColumnName.IsNullOrEmpty() || false == data.Columns.Contains(ColumnName)) 
                {
                    // if the column name is blank or the data doesn't have the column
                    Log.Info("Table Did not contain column [" + ColumnName + "] for DeleteColumnFormatCommand.");

                    return false;
                }
                                
                var column = data.Columns[ColumnName];
                data.Columns.Remove(column);

            }
            catch(Exception ex)
            {
                Log.Warn("Exception while trying to Apply DeleteColumnFormatCommand to Data.", ex);
                return false;
            }

            return true;
        }
    }
}
