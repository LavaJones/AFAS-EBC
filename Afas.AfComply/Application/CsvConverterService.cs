using System;
using System.Collections.Generic;
using System.Linq;

using System.Data;
using System.Globalization;

using log4net;

using Afas.AfComply.Domain;
using Afas.AfComply.Domain.Mapping;
using Afas.Application.CSV;
using Afas.Domain;

namespace Afas.AfComply.Application
{

    public class CsvConverterService : ICsvConverterService
    {

        public CsvConverterService() : this(LogManager.GetLogger(typeof(CsvConverterService))) { }

        public CsvConverterService(ILog log)
        {

            this.Log = log;
        
        }

        DataTable ICsvConverterService.Convert(String[] source)
        {

            DataTable dataTable = new DataTable("CSVTable");

            if (source[0].IsNotHeaderRow())
            {
                throw new InvalidOperationException("First row does not appear to contain valid headers.");
            }

            String[] headers = CsvParse.SplitRow(source[0]);

            headers = CsvHelper.SanitizeHeaderValues(headers);

            foreach (String header in headers)
            {
                dataTable.Columns.Add(header);
            }

            this.Log.Info(String.Format("Found {0} column(s)", dataTable.Columns.Count));

            int errorRowIndex = 2;
            foreach (String row in source.Skip(1))
            {

                if (row.Trim() == null || row.Trim().Equals(String.Empty))
                {
                    continue;
                }

                String[] rowValues = CsvParse.SplitRow(row);

                if (DataValidation.StringArrayContainsOnlyBlanks(rowValues))
                {

                    this.Log.Info("Skipping row since all columns where blank.");
                    
                    continue;
                
                }

                DataRow dataRow = dataTable.NewRow();

                if (rowValues.Count() != headers.Count())
                {

                    throw new InvalidOperationException(
                            String.Format(
                                    "Unbalanced rows detected at line {0}, expected {1} columns but found {2}!",
                                    errorRowIndex,
                                    headers.Count(),
                                    rowValues.Count()
                                )
                        );

                }

                for (int i = 0; i < rowValues.Length; i++)
                {

                    DateTime dateValue;

                    String value = rowValues[i].TrimDoubleQuotes();
                    if (ConverterHelper.ParseDate(value, out dateValue))
                    {
                        dataRow[headers[i]] = dateValue;
                    }
                    else
                    {
                        dataRow[headers[i]] = value;
                    }

                }

                dataTable.Rows.Add(dataRow);

                errorRowIndex++;

            }

            this.Log.Info(String.Format("Received {0} line(s) including one header, passing back {1} rows.", source.Count(), dataTable.Rows.Count));

            return dataTable;

        }

        protected ILog Log { get; private set; }

    }

}
