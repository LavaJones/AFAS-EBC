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
    /// This class implemements the Default Value Command, 
    /// that takes one column and replaces nulls and blanks with the specified value.
    /// This is a mid level change that generally happens after column name changes.
    /// </summary>
    public class DefaultColumnValueFormatCommand : AImportFormatCommand
    {
        private ILog Log = LogManager.GetLogger(typeof(DefaultColumnValueFormatCommand));

        /// <summary>
        /// The Type of command that this is
        /// </summary>
        public override ImportFormatCommandTypes CommandType
        {
            // This is hard coded because we are using the strategy pattern (ish) 
            get { return ImportFormatCommandTypes.DefaultColumnIfBlank; }
        }

        /// <summary>
        /// A list of the Required Parameters that the Command cannot function without.
        /// </summary>
        public override IList<string> RequiredParameters { get { return new List<string> { "ColumnName", "DefaultValue" }; } }

        /// <summary>
        /// A list of optional Parameters that the Command can use but are not Required.
        /// </summary>
        public override IList<string> OptionalParameters { get { return new List<string>(); } }

        /// <summary>
        /// Default Constructor with No arguments
        /// </summary>
        public DefaultColumnValueFormatCommand() 
            : base() 
        {
            // Use default behavior
        }

        /// <summary>
        /// Apply the Default Value Command to fill default values into blank rows in the column, if it exists
        /// </summary>
        /// <param name="metaData">The metaData, including the data to be formatted.</param>
        /// <returns>True if the format was applied correctly, false if there was a failure.</returns>
        public override bool ApplyTo(ImportData metaData)
        {
            try
            {
                System.Data.DataTable data = metaData.Data;

                // The first param should be the header name of the column to fill.
                string ColumnName = Parameters["ColumnName"];

                // The Last param should be the desired Default Value.
                string DefaultValue = Parameters["DefaultValue"];

                if (DefaultValue.IsNullOrEmpty() || DefaultValue.Trim().IsNullOrEmpty())
                {
                    // if the Generator Type is blank or null
                    Log.Info("DefaultValue is not valid [" + DefaultValue + "] for DefaultColumnValueFormatCommand.");

                    return false;
                }
            
                // If it doesn't contain this column then we ignore it
                if (data.Columns.Contains(ColumnName))
                {
                    // check each row
                    foreach (System.Data.DataRow row in data.Rows)
                    {
                        // check to see if the value is null
                        if (row[ColumnName] == null || row[ColumnName] == DBNull.Value || row[ColumnName].ToString().Trim().IsNullOrEmpty())
                        {
                            // replace the null with the default value
                            row[ColumnName] = DefaultValue;
                        }
                    }

                    Log.Debug("Finished setting Default Value [" + DefaultValue + "] on Column: " + ColumnName);
                }
                else 
                {
                    Log.Info("Table Did not contain column to set default on by DefaultColumnValueFormatCommand.");

                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Exception while trying to Apply DefaultColumnValueFormatCommand to Data.", ex);
                
                return false;
            }

            return true;
        }
    }
}
