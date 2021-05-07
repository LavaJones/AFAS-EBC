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
    /// This class implemements the Replace By Value Format Command,
    /// It will check if a column value matches a specified value, will replace the value
    /// </summary>
    public class ReplaceByValueFormatCommand : AImportFormatCommand
    {
        private ILog Log = LogManager.GetLogger(typeof(ReplaceByValueFormatCommand));

        /// <summary>
        /// The Type of command that this is
        /// </summary>
        public override ImportFormatCommandTypes CommandType
        {
            // This is hard coded because we are using the strategy pattern (ish) 
            get { return ImportFormatCommandTypes.ReplaceByValue; }
        }

        /// <summary>
        /// A list of the Required Parameters that the Command cannot function without.
        /// </summary>
        public override IList<string> RequiredParameters { get { return new List<string> { "ColumnName", "RemoveValue", "ReplaceValue" }; } }

        /// <summary>
        /// A list of optional Parameters that the Command can use but are not Required.
        /// </summary>
        public override IList<string> OptionalParameters { get { return new List<string>(); } }
        

        /// <summary>
        /// Default Constructor with No arguments
        /// </summary>
        public ReplaceByValueFormatCommand()
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

                string ReplaceValue = Parameters["ReplaceValue"];

                if (ColumnName.IsNullOrEmpty() || false == data.Columns.Contains(ColumnName))
                {
                    // if the column name is blank or the data doesn't have the column
                    Log.Info("Table Did not contain column [" + ColumnName + "] for ReplaceByValueFormatCommand.");

                    return false;
                }

                if (RemoveValue == null)
                {
                    // if the Remove Value is null, since we remove by setting it to null.
                    Log.Info("RemoveValue is null, cannot remove null by replacing with null.");

                    return false;
                }

                foreach (DataRow row in data.Rows)
                {
                    if (true == row[ColumnName].ToString().Equals(RemoveValue))
                    {
                        // if the values match then replace it
                        row[ColumnName] = ReplaceValue;
                    }
                }
                
            }
            catch(Exception ex)
            {
                Log.Warn("Exception while trying to Apply ReplaceByValueFormatCommand to Data.", ex);
                return false;
            }

            return true;
        }
    }
}
