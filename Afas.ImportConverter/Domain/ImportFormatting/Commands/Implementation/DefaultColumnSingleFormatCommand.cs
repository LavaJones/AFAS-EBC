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
    /// This class implemements the Default Value if single Command, 
    /// that takes one column and replaces nulls or blanks with a value if there is only one possible value.
    /// This is a mid level change that generally happens after column name changes.
    /// </summary>
    public class DefaultColumnSingleFormatCommand : AImportFormatCommand
    {

        private ILog Log = LogManager.GetLogger(typeof(DefaultColumnSingleFormatCommand));

        /// <summary>
        /// This gives us access to the availible options from the database
        /// </summary>
        private IManagedDataSource DataSource;

        /// <summary>
        /// The Type of command that this is
        /// </summary>
        public override ImportFormatCommandTypes CommandType
        {
            // This is hard coded because we are using the strategy pattern (ish) 
            get { return ImportFormatCommandTypes.DefaultColumnIfSingleChoice; }
        }

        /// <summary>
        /// A list of the Required Parameters that the Command cannot function without.
        /// </summary>
        public override IList<string> RequiredParameters { get { return new List<string> { "ColumnName", "ValueSource" }; } }

        /// <summary>
        /// A list of optional Parameters that the Command can use but are not Required.
        /// </summary>
        public override IList<string> OptionalParameters { get { return new List<string>(); } }


        public DefaultColumnSingleFormatCommand() : base()
        {
          
        }



            /// <summary>
            /// Default Constructor, sets initial values
            /// </summary>
            /// <param name="parameters">The Parameters for this Command.</param>
            /// <param name="scope">The scope to which this Command is applied</param>
            /// <param name="importFormatCommandId">Data base Id or 0</param>
        public DefaultColumnSingleFormatCommand(IManagedDataSource dataSource) : base() 
        {
            // Use default behavior Plus
            if (null == dataSource) 
            {
                throw new ArgumentNullException("DataSource");
            }

            this.DataSource = dataSource;
        }

        /// <summary>
        /// Apply this Command to format this data
        /// </summary>
        /// <param name="metaData">The metaData, including the data to be formatted.</param>
        /// <returns>True if the format was applied correctly, false if there was a failure.</returns>
        public override bool ApplyTo(ImportData metaData)
        {
            try
            {
                System.Data.DataTable data = metaData.Data;

                // This param should be the header name of the column to fill.
                string ColumnName = Parameters["ColumnName"];

                // This param should Describe the source to check for a single choice.
                string ValueSource = Parameters["ValueSource"];

                if (ValueSource.IsNullOrEmpty() || ValueSource.Trim().IsNullOrEmpty())
                {
                    // if the Generator Type is blank or null
                    Log.Info("ValueSource is not valid [" + ValueSource + "] for DefaultColumnSingleFormatCommand.");

                    return false;
                }
            
                Log.Info(String.Format("Applying DefaultColumnIfSingleChoice, using params: ColumnName: [{0}], valueFrom: [{1}], on data.Rows.Count: [{2}]",
                    ColumnName, ValueSource, data.Rows.Count));        
            
                // If it doesn't contain this column then we ignore it
                if (data.Columns.Contains(ColumnName))
                {

                    IList<String> allValues = DataSource.GetPossibleValues(ValueSource, metaData);
                    string DefaultValue = null;
                    if (allValues.Count == 1)
                    {
                        DefaultValue = allValues.Single();
                    } 
                    else if (allValues.Count > 1)
                    {
                        Log.Debug("Too many Choices Found for DefaultColumnIfSingleChoice. Found " + allValues.Count);

                        return false;
                    }                     
                    else if (allValues.Count < 1)
                    {
                        Log.Debug("No Choice Found for DefaultColumnIfSingleChoice.");

                        return false;
                    }    

                    Log.Info("Applying Default Value of: " + DefaultValue);

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
                }
                else
                {
                    Log.Info("Table Did not contain column [" + ColumnName + "] for DefaultColumnSingleFormatCommand.");

                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Exception while trying to Apply DefaultColumnSingleFormatCommand to Data.", ex);

                return false;
            }
            
            return true;
        }
    }
}
