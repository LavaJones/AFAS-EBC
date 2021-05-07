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
    /// This class implemements the Delete Rows Command,
    /// It will check if a Row matches the parameters and will remove it
    /// </summary>
    public class DeleteRowsFormatCommand : AImportFormatCommand
    {
        private ILog Log = LogManager.GetLogger(typeof(DeleteRowsFormatCommand));

        /// <summary>
        /// The Type of command that this is
        /// </summary>
        public override ImportFormatCommandTypes CommandType
        {
            // This is hard coded because we are using the strategy pattern (ish) 
            get { return ImportFormatCommandTypes.DeleteRows; }
        }

        /// <summary>
        /// A list of the Required Parameters that the Command cannot function without.
        /// </summary>
        public override IList<string> RequiredParameters { get { return new List<string> { "ColumnName", "MatchValue" }; } }

        /// <summary>
        /// A list of optional Parameters that the Command can use but are not Required.
        /// </summary>
        public override IList<string> OptionalParameters { get { return new List<string>(); } }

        /// <summary>
        /// Default Constructor with No arguments
        /// </summary>
        public DeleteRowsFormatCommand()
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

                // Get the column name
                string ColumnName = Parameters["ColumnName"];

                string MatchValue = Parameters["MatchValue"];

                if (ColumnName.IsNullOrEmpty() || false == data.Columns.Contains(ColumnName))
                {
                    // if the column name is blank or the data doesn't have the column
                    Log.Info("Table Did not contain column [" + ColumnName + "] for .");

                    return false;
                }

                if (MatchValue.IsNullOrEmpty() || MatchValue.Trim().IsNullOrEmpty())
                {
                    Log.Info("Mtch Value is not acceptable [" + MatchValue + "].");

                    return false;
                }

                List<DataRow> toRemove = new List<DataRow>();

                // check for rows to remove
                foreach (DataRow row in data.Rows)
                {
                    if (true == row[ColumnName].ToString().Equals(MatchValue))
                    {
                        // if the value matches, delete the whole row
                        toRemove.Add(row);
                    }
                }

                // remove the rows
                foreach (DataRow row in toRemove) 
                {
                    data.Rows.Remove(row);
                }
            }
            catch(Exception ex)
            {
                Log.Warn("Exception while trying to Apply DeleteRowsFormatCommand to Data.", ex);
                return false;
            }

            return true;
        }
    }
}
