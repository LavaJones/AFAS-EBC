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
    /// This class implemements the Add New Column Command, 
    /// that Adds a new Column with the desired name, if it does not already exist.
    /// This is a general first pass or high level change that can bring many documents into line quickly.
    /// </summary>
    public class AddNewColumnFormatCommand : AImportFormatCommand
    {
        private ILog Log = LogManager.GetLogger(typeof(AddNewColumnFormatCommand));

        /// <summary>
        /// The Type of command that this is
        /// </summary>
        public override ImportFormatCommandTypes CommandType
        {
            // This is hard coded because we are using the strategy pattern (ish) 
            get { return ImportFormatCommandTypes.AddNewColumn; }
        }

        /// <summary>
        /// A list of the Required Parameters that the Command cannot function without.
        /// </summary>
        public override IList<string> RequiredParameters { get { return new List<string> {"ColumnName"}; } }

        /// <summary>
        /// A list of optional Parameters that the Command can use but are not Required.
        /// </summary>
        public override IList<string> OptionalParameters { get { return new List<string>(); } }
        
        /// <summary>
        /// Default Constructor with No arguments
        /// </summary>
        public AddNewColumnFormatCommand()
            : base() 
        {
            // Use default behavior
        }

        /// <summary>
        /// Apply the Add Column Command, which will add a new Column with the Name Parameter provided
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

                if (ColumnName.IsNullOrEmpty() || ColumnName.Trim().IsNullOrEmpty())
                {
                    // if the column name is blank or null
                    Log.Info("ColumnName is not valid [" + ColumnName + "] for AddNewColumnFormatCommand.");

                    return false;
                } 
                else if(false == data.Columns.Contains(ColumnName)) 
                {
                    // if the data doesn't have the column
                    Log.Info("Table Did not contain column [" + ColumnName + "], Adding it now, by AddNewColumnFormatCommand.");
                    
                    data.Columns.Add(ColumnName, typeof(String));
                    
                    return true;
                }
            }
            catch(Exception ex)
            {
                Log.Warn("Exception while trying to Apply AddNewColumnFormatCommand to Data.", ex);
                return false;
            }

            return true;
        }
    }
}
