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
    /// This class implemements the Delete Column If Blank Command,
    /// It will check if a column is completly blank and if so, will remove the column
    /// </summary>
    public class DeleteByValueFormatCommand : AImportFormatCommand
    {
        private ILog Log = LogManager.GetLogger(typeof(DeleteByValueFormatCommand));

        /// <summary>
        /// The Type of command that this is
        /// </summary>
        public override ImportFormatCommandTypes CommandType
        {
            // This is hard coded because we are using the strategy pattern (ish) 
            get { return ImportFormatCommandTypes.DeleteByValue; }
        }

        /// <summary>
        /// A list of the Required Parameters that the Command cannot function without.
        /// </summary>
        public override IList<string> RequiredParameters { get { return new List<string> { "ColumnName", "RemoveValue" }; } }

        /// <summary>
        /// A list of optional Parameters that the Command can use but are not Required.
        /// </summary>
        public override IList<string> OptionalParameters { get { return new List<string>(); } }

        /// <summary>
        /// Default Constructor with No arguments
        /// </summary>
        public DeleteByValueFormatCommand()
            : base() 
        {
            // Use default behavior
        }

        /// <summary>
        /// Apply the Delete Column If Blank Command to clean out empty columns
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
                
                string RemoveValue = Parameters["RemoveValue"];

                if (ColumnName.IsNullOrEmpty() || false == data.Columns.Contains(ColumnName))
                {
                    // if the column name is blank or the data doesn't have the column
                    Log.Info("Table Did not contain column [" + ColumnName + "] for DeleteByValueFormatCommand.");

                    return false;
                }
                if(RemoveValue.IsNullOrEmpty() || RemoveValue.Trim().IsNullOrEmpty())
                {
                    // if the Remove Value is null, since we remove by setting it to null.
                    Log.Info("RemoveValue is null or empty, cannot remove null by replacing with null.");

                    return false;
                }

                foreach (DataRow row in data.Rows)
                {
                    if (true == row[ColumnName].ToString().Equals(RemoveValue))
                    {
                        //if there is data in the column, then don't delete it
                        row[ColumnName] = DBNull.Value;
                    }
                }
                
            }
            catch(Exception ex)
            {
                Log.Warn("Exception while trying to Apply DeleteByValueFormatCommand to Data.", ex);
                return false;
            }

            return true;
        }
    }
}
