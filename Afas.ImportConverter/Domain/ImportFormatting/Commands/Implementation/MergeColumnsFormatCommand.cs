using Afas.ImportConverter.Domain.POCO;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Afas.Domain;
using System.Data;

namespace Afas.ImportConverter.Domain.ImportFormatting
{
    /// <summary>
    /// This class implemements the Merge Columns Format Command, 
    /// that takes one column and replace the values with a Merger of one or more columns as defied by the Format Parameter 
    /// This is a Low level change that generally happens after most changes.
    /// </summary>
    public class MergeColumnsFormatCommand : AImportFormatCommand
    {
        private ILog Log = LogManager.GetLogger(typeof(MergeColumnsFormatCommand));

        /// <summary>
        /// The Type of command that this is
        /// </summary>
        public override ImportFormatCommandTypes CommandType
        {
            // This is hard coded because we are using the strategy pattern (ish) 
            get { return ImportFormatCommandTypes.MergeColumns; }
        }

        /// <summary>
        /// A list of the Required Parameters that the Command cannot function without.
        /// </summary>
        public override IList<string> RequiredParameters { get { return new List<string> { "ColumnName", "MergePattern" }; } }

        /// <summary>
        /// A list of optional Parameters that the Command can use but are not Required.
        /// </summary>
        public override IList<string> OptionalParameters { get { return new List<string>(); } }

        /// <summary>
        /// Default Constructor with No arguments
        /// </summary>
        public MergeColumnsFormatCommand()
            : base()
        {
            // Use default behavior
        }

        /// <summary>
        /// Apply this Command to fill the column with formatted data from other columns
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

                // This Param should be the type of Generator to use
                string MergePattern = Parameters["MergePattern"];

                string MergedValue = null;

                Log.Info(String.Format("Applying MergeColumnsFormatCommand, using params: ColumnName: [{0}], MergePattern: [{1}] on data.Rows.Count: [{2}]",
                    ColumnName, MergePattern, data.Rows.Count));

                // If it doesn't contain this column then we ignore it
                if (data.Columns.Contains(ColumnName))
                {

                    // check each row
                    foreach (System.Data.DataRow row in data.Rows)
                    {

                        row[ColumnName] = RecursiveRegex(MergePattern, row);


                        //// check to see if the value is null
                        //if (row[ColumnName] == null || row[ColumnName] == DBNull.Value || row[ColumnName].ToString().IsNullOrEmpty())
                        //{
                        //    // replace each column in the pattern with the columns value 
                        //    MergedValue = Regex.Replace(MergePattern, "{.+?}", delegate (Match match)
                        //    {
                        //        string value = match.Value.Replace("}", string.Empty).Replace("{", string.Empty).Trim();
                        //        if (row.Table.Columns.Contains(value) && null != row[value])
                        //        {
                        //            return row[value].ToString();
                        //        }
                        //        return "";
                        //    });

                        //    // Clean up the result 
                        //    MergedValue = MergedValue.Replace("NULL", String.Empty).Trim(); // remove any null values

                        //    if (MergedValue != null && false == MergedValue.IsNullOrEmpty())
                        //    {
                        //        // replace the null with the default value
                        //        row[ColumnName] = MergedValue;
                        //    }
                        //}
                    }
                }
                else
                {
                    Log.Info("Table Did not contain column [" + ColumnName + "] for MergeColumnsFormatCommand.");

                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Exception while trying to Apply MergeColumnsFormatCommand to Data.", ex);
                return false;
            }

            return true;
        }

        // replace each column in the pattern with the columns value 
        const string RegExString = @"
                \{                    # Match {
                (
                [^{}]+            # all chars except {}
                | (?<Level>\{)    # or if { then Level += 1
                | (?<-Level>\})   # or if } then Level -= 1
                )+                    # Repeat (to go from inside to outside)
                (?(Level)(?!))        # zero-width negative lookahead assertion
                \}                    # Match }";

        private string RecursiveRegex(String pattern, DataRow row)
        {



            MatchEvaluator OnMatch = null;// weird compiler *feature* that you can only call a delegate from inside it if it has already been instantiated.

            OnMatch = delegate (Match match)
            {
                string Matched = match.Value;
                // Trim the first and last characters, because we know that they are {}
                string value = Matched.Substring(1, (Matched.Length - 2)).Trim();
                if (false == row.Table.Columns.Contains(value))
                {
                    // Use recursion to make sure any nested values are caught
                    value = Regex.Replace(
                        value,
                        RegExString,
                        OnMatch,
                        RegexOptions.IgnorePatternWhitespace);

                    value = value.Replace("NULL", String.Empty).Trim(); // remove any null values

                    // Update the matched value if needed
                    Matched = '{' + value + '}';
                }

                if (row.Table.Columns.Contains(value) && null != row[value])
                {
                    return row[value].ToString();
                }

                return Matched;
            };

            pattern = Regex.Replace(
                pattern,
                RegExString,
                OnMatch,
                RegexOptions.IgnorePatternWhitespace);

            return pattern;
        }
    }
}
