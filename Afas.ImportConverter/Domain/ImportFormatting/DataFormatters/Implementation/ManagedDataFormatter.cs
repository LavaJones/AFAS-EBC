using Afas.Application;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.ImportConverter.Domain.ImportFormatting.DataFormatters.Implementation
{

    /// <summary>
    /// Defines the different types of formatters
    /// </summary>
    public enum DataFormatterTypes
    {
        /// <summary>
        /// Format the data into a Date value
        /// </summary>
        DateFormatter,

        /// <summary>
        /// Format the data into a number with decimal places
        /// </summary>
        DecimalFormatter,

        /// <summary>
        /// Format the data into a number witohut decimals
        /// </summary>
        IntegerFormatter,

        /// <summary>
        /// Format the data into a True/False Boolean
        /// </summary>
        BooleanFormatter,

        //SsnFormatter,
        //ZipCodeFormatter,
        //ZipPlus4Formater,
    }

    /// <summary>
    /// Manages the different types of Data Formatters.
    /// </summary>
    public class ManagedDataFormatter : IManagedDataFormatter
    {
        private ILog Log = LogManager.GetLogger(typeof(ManagedDataFormatter));

        private IDefaultFactory<IDataFormatter> Factory;

        public ManagedDataFormatter(IDefaultFactory<IDataFormatter> factory)
        {
            if (null == factory)
            {
                throw new ArgumentNullException("Factory");
            }

            Factory = factory;
        }

        /// <summary>
        /// Formats the provided data using the specified formatter type and optional format
        /// </summary>
        /// <param name="data">Data to format</param>
        /// <param name="type">Type of fomatter to use</param>
        /// <param name="format">Optional specified format</param>
        /// <returns>THe formatted string or the origonal data if unable to format</returns>
        public String FormatData(string data, string formatterType, string format = null) 
        {
            // can't format null
            if (null == data) 
            {
                return data;
            }

            DataFormatterTypes type;
            if (false == Enum.TryParse(formatterType, out type))
            {
                throw new ArgumentException("A Valid Formatter Type should be provided. Recieved:" + formatterType);
            }

            IDataFormatter formatter = Factory.GetById(type.ToString());
            
            if (null == format && formatter != null)
            {
                return formatter.FormatData(data);
            }
            else if (formatter != null)
            {
                return formatter.FormatData(data, format);
            }
            
            return data;
        }

    }
}
