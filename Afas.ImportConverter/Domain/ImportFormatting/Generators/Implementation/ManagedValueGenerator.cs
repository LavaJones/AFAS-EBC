using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Afas.Domain;
using Afas.Application;
using System.Data;

namespace Afas.ImportConverter.Domain.ImportFormatting.Generators.Implementation
{

    /// <summary>
    /// This code manages the task of Default Value Generator
    /// </summary>
    public class ManagedValueGenerator : IManagedValueGenerator
    {
        private ILog Log = LogManager.GetLogger(typeof(ManagedValueGenerator));

        private IDefaultFactory<IValueGenerator> GeneratorFactory;

        /// <summary>
        /// Standard Constructor taking dependencies as arguments
        /// </summary>
        /// <param name="generatorFactory">A IoC Factory that Creates IValueGenerator for the provided ValueGeneratorType</param>
        public ManagedValueGenerator(IDefaultFactory<IValueGenerator> generatorFactory)
        {
            if (null == generatorFactory)
            {
                throw new ArgumentNullException("GeneratorFactory");
            }

            this.GeneratorFactory = generatorFactory;
        }

        /// <summary>
        /// Generates a Default Value for the Row based on the Generator Type, Pattern, and the Row data itself
        /// </summary>
        /// <param name="generatorTypeValue">The Type of generator to use.</param>
        /// <param name="generatorPattern">The individual pattern to generate</param>
        /// <param name="row">The Source Data for the Row.</param>
        /// <exception cref="ArgumentException">If a valid Generator doesn't exist for generatorTypeValue.</exception>
        /// <returns>The generated Value</returns>
        public string GenerateDefaultValue(string generatorTypeValue, string generatorPattern, System.Data.DataRow row)
        {
            // Clean up the parameters
            if (generatorPattern.IsNullOrEmpty())
            {
                generatorPattern = String.Empty;
            }

            string result = generatorPattern;

            // This loop allows composite generator types
            if (generatorTypeValue.IsNullOrEmpty())
            {
                throw new ArgumentException("A Valid Generator Type should be provided. Recieved:" + generatorTypeValue);
            }

            foreach (string generatorType in generatorTypeValue.Split(','))
            {
                // Determine the type of generator to use
                ValueGeneratorTypes type;
                if (false == Enum.TryParse(generatorType, out type))
                {
                    throw new ArgumentException("A Valid Generator Type should be provided. Recieved:" + generatorType);
                }

                // Build the Generator code
                IValueGenerator generator = GeneratorFactory.GetById(type.ToString());

                // Use the Generator to genrate the value
                if (null != generator)
                {
                    if (generatorPattern.Contains(generator.GetPlaceholder))
                    {
                        result = generator.GenerateValue(result);
                    }
                }
                else
                {
                    Log.Warn("Attempted to use unsupported GeneratorType: " + generatorType);
                }
            }

            return RecursiveRegex(result, row);
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

            MatchEvaluator OnMatch = null;

            OnMatch = delegate (Match match)
                {
                    string Matched = match.Value;

                    string value = Matched.Substring(1, (Matched.Length - 2)).Trim();
                    if (false == row.Table.Columns.Contains(value))
                    {
                        // Use recursion to make sure any nested values are caught
                        value = Regex.Replace(
                            value,
                            RegExString,
                            OnMatch,
                            RegexOptions.IgnorePatternWhitespace);


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
