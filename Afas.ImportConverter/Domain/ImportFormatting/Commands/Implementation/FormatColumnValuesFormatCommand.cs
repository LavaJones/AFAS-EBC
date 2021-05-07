using Afas.ImportConverter.Domain.ImportFormatting.DataFormatters;
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
    /// This class implemements the Format Column Values  Command, 
    /// that takes one column and formatts the values based on the provided parmeters.
    /// This is a cleanup change that can happen at any time.
    /// </summary>
    public class FormatColumnValuesFormatCommand : AImportFormatCommand
    {
        private ILog Log = LogManager.GetLogger(typeof(FormatColumnValuesFormatCommand));

        /// <summary>
        /// The Managed Generator responsible for Generating Default Values
        /// </summary>
        private IManagedDataFormatter Formatter;

        /// <summary>
        /// The Type of command that this is
        /// </summary>
        public override ImportFormatCommandTypes CommandType
        {
            // This is hard coded because we are using the strategy pattern (ish) 
            get { return ImportFormatCommandTypes.FormatColumn; }
        }

        /// <summary>
        /// A list of the Required Parameters that the Command cannot function without.
        /// </summary>
        public override IList<string> RequiredParameters { get { return new List<string> { "ColumnName", "FormatterType", "Format" }; } }

        /// <summary>
        /// A list of optional Parameters that the Command can use but are not Required.
        /// </summary>
        public override IList<string> OptionalParameters { get { return new List<string>(); } }

        public FormatColumnValuesFormatCommand() : base()
        {
           
        }

        /// <summary>
        /// Default Constructor, sets initial values
        /// </summary>
        /// <param name="parameters">The Parameters for this Command.</param>
        /// <param name="scope">The scope to which this Command is applied</param>
        /// <param name="importFormatCommandId">Data base Id or 0</param>
        public FormatColumnValuesFormatCommand( IManagedDataFormatter formatter) : base() 
        {
            // Use default behavior Plus
            if (null == formatter) 
            {
                throw new ArgumentNullException("Generator");
            }

            this.Formatter = formatter;
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

                // This Param should be the type of Formatter to use
                string FormatterType = Parameters["FormatterType"];

                // This Param should be the 
                string Format = Parameters["Format"];

                string FormattedValue = null;

                Log.Info(String.Format("Applying FormatColumnValuesFormatCommand, using params: ColumnName: [{0}], FormatterType: [{1}], Format: [{2}] on data.Rows.Count: [{3}]",
                    ColumnName, FormatterType, Format, data.Rows.Count));


                // If it doesn't contain this column then we ignore it
                if (data.Columns.Contains(ColumnName))
                {
                    // check each row
                    foreach (System.Data.DataRow row in data.Rows)
                    {

                        FormattedValue = Formatter.FormatData(row[ColumnName].ToString(), FormatterType, Format);

                        if (FormattedValue != null 
                            && false == FormattedValue.IsNullOrEmpty() 
                            && row[ColumnName].ToString() != FormattedValue)
                        {
                            // replace the value with the formatted value
                            row[ColumnName] = FormattedValue;
                        }
                    }
                }
                else 
                {
                    Log.Info("Table Did not contain column [" + ColumnName + "] for FormatColumnValuesFormatCommand.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Exception while trying to Apply FormatColumnValuesFormatCommand to Data.", ex);
                return false;
            }
            
            return true;
        }
    }
}
