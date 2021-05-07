using Afas.Application.FileAccess;
using Afas.ImportConverter.Domain;
using Afas.Application.CSV;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace Afas.ImportConverter.Application.Implementation
{

    /// <summary>
    /// A file uploader specific to CSV files
    /// </summary>
    public class CsvDataTableBuilder : ICsvDataTableBuilder, ICsvFileDataTableBuilder
    {

        private ILog Log = LogManager.GetLogger(typeof(CsvDataTableBuilder));

        /// <summary>
        /// This Object requires acces to the file system 
        /// </summary>
        private IFileAccess FileAccess;

        /// <summary>
        /// The builder for constructing the datatable
        /// </summary>
        private IDataTableBuilder Builder;

        /// <summary>
        /// The parser to and from CSV format
        /// </summary>
        private ICsvParser CsvParse;

        /// <summary>
        /// Standard Constructor
        /// </summary>
        /// <param name="fileAccess">Access to the File System</param>
        /// <param name="builder">builder to simplify DataTable Construction</param>
        /// <param name="csvParse">CSV Parsing utility</param>
        public CsvDataTableBuilder(IFileAccess fileAccess, IDataTableBuilder builder, ICsvParser csvParse)
        {
            //check that all dependencies are provided
            if(null == fileAccess)
            {
                throw new ArgumentNullException("fileAccess");
            }
            this.FileAccess = fileAccess;

            if(null == builder)
            {
                throw new ArgumentNullException("builder");
            }  
            this.Builder = builder;

            if(null == csvParse)
            {
                throw new ArgumentNullException("csvParse");
            }
            this.CsvParse = csvParse;
        }

        /// <summary>
        /// Fills a Data Table Builder from the File provided
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <returns>A Data Table storing all the data</returns>
        public IDataTableBuilder CreateDataTableBuilderFromCsvFile(string sourceFilePath)
        {
            if (FileAccess.FileExists(sourceFilePath) == false)
            {
                throw new ArgumentException("CSV File could not be read.");
            }

            using (StreamReader fileReader = new StreamReader(sourceFilePath))
            {
                return CreateDataTableBuilderFromCsv(fileReader);
            }
        }
        
        /// <summary>
        /// Fills a Data Table from the File provided
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <returns>A Data Table storing all the data</returns>
        public DataTable CreateDataTableFromCsvFile(string sourceFilePath)
        {
            return CreateDataTableBuilderFromCsvFile(sourceFilePath).Build();
        }

        /// <summary>
        /// Fills a Data Table Builder from the stream provided
        /// </summary>
        /// <param name="dataReader">The reader that pulls the data in CSV format</param>
        /// <returns>A datatable Builder that is filled with all the values from the data stream</returns>
        public IDataTableBuilder CreateDataTableBuilderFromCsv(StreamReader dataReader)
        {
            if(null == dataReader)
            {
                throw new ArgumentNullException("dataReader");
            }

            // Split the header line
            String[] headers = CsvParse.SplitRow(dataReader.ReadLine());
            Builder.ParseHeaders(headers);

            string row = string.Empty;

            while ((row = dataReader.ReadLine()) != null)
            {
                // Parse each row
                Builder.AddRowToTable(CsvParse.SplitRow(row));
            }

            return Builder;
        }

        /// <summary>
        /// Fills a Data Table from the stream provided
        /// </summary>
        /// <param name="dataReader">The reader that pulls the data in CSV format</param>
        /// <returns>A datatable Builder that is filled with all the values from the data stream</returns>
        public DataTable CreateDataTableFromCsv(StreamReader dataReader)
        {
            return CreateDataTableBuilderFromCsv(dataReader).Build();
        }
    }
}