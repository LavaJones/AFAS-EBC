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
using ClosedXML.Excel;

namespace Afas.ImportConverter.Application.Implementation
{

    /// <summary>
    /// A file uploader specific to CSV files
    /// </summary>
    public class XlsxDataTableBuilder : IXlsxFileDataTableBuilder
    {

        private ILog Log = LogManager.GetLogger(typeof(XlsxDataTableBuilder));

        /// <summary>
        /// This Object requires acces to the file system 
        /// </summary>
        private IFileAccess FileAccess;

        /// <summary>
        /// The builder for constructing the datatable
        /// </summary>
        private IDataTableBuilder Builder;

        /// <summary>
        /// Standard Constructor
        /// </summary>
        /// <param name="fileAccess">Access to the File System</param>
        /// <param name="builder">builder to simplify DataTable Construction</param>
        /// <param name="csvParse">CSV Parsing utility</param>
        public XlsxDataTableBuilder(IFileAccess fileAccess, IDataTableBuilder builder)
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
        }

        /// <summary>
        /// Fills a Data Table Builder from the File provided
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <returns>A Data Table storing all the data</returns>
        public IDataTableBuilder CreateDataTableBuilderFromXlsxFile(string sourceFilePath)
        {
            if (FileAccess.FileExists(sourceFilePath) == false)
            {
                throw new ArgumentException("Excel File could not be found.");
            }

            using (var workbook1 = new XLWorkbook(sourceFilePath))
            {

                var worksheet = workbook1.Worksheets.First();

                int firstColumnId = worksheet.FirstColumnUsed().ColumnNumber();//worksheet.ColumnCount();//.FirstCellUsed().Address.ColumnNumber;
                int lastColumnId = worksheet.LastColumnUsed().ColumnNumber();//;worksheet.Columns().Count(); //.ColumnCount(); //worksheet.LastCellUsed().Address.ColumnNumber;

                // just like CSV, read the rows one at a time, and check if the first row is a header
                bool checkHeaders = true;
                foreach (IXLRow row in worksheet.Rows())
                {
                    // see if the first row looks like data or headers

                    if (checkHeaders)
                    {

                        // Split the header line
                        List<string> headers = new List<string>();
                        foreach (IXLCell cell in row.Cells(firstColumnId, lastColumnId))
                        {
                            headers.Add(cell.Value?.ToString());
                        }
                        //var temp = headers.ToArray();
                        Builder.ParseHeaders(headers.ToArray());

                        checkHeaders = false;
                    }
                    else
                    {

                        List<string> rowValues = new List<string>();

                        foreach (IXLCell cell in row.Cells(firstColumnId, lastColumnId))
                        {
                            rowValues.Add(cell.Value?.ToString());
                        }
                        //var temp = rowValues.ToArray();
                        // Parse each row
                        Builder.AddRowToTable(rowValues.ToArray());
                    }
                }
            }

            return Builder;

        }
        


        /// <summary>
        /// Fills a Data Table from the File provided
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <returns>A Data Table storing all the data</returns>
        public DataTable CreateDataTableFromXlsxFile(string sourceFilePath)
        {
            return CreateDataTableBuilderFromXlsxFile(sourceFilePath).Build();
        }
    }
}