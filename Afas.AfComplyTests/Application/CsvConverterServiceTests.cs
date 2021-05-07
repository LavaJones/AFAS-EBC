using System;
using System.Data;

using log4net;

using NUnit.Framework;
using Rhino.Mocks;

using Afas.AfComply.Application;

namespace Afas.AfComply.ApplicationTests
{

    [TestFixture]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class CsvConverterServiceTests
    {

        [Test]
        public void ImplementsICsvConverterService()
        {
            Assert.IsNotNull((new CsvConverterService() as ICsvConverterService), "Should implement the public interface.");
        }

        public void ConvertShouldFindAndParseTwoColumns()
        {

            ICsvConverterService csvConverterService = new CsvConverterService(MockRepository.GenerateStub<ILog>());

            String header = "First Name,Last Name";
            String data = "\"data1\",\"data2\"";
            String[] source = new String[] { header, data };

            DataTable dataTable = csvConverterService.Convert(source);

            Assert.AreEqual(2, dataTable.Columns.Count, "Column count should match.");

        }

        [Test]
        public void ConvertShouldRemoveExtraDoubleQuotesFromHeaders()
        {

            ICsvConverterService csvConverterService = new CsvConverterService(MockRepository.GenerateStub<ILog>());

            String header = "\"First Name\",\"Last Name\"";
            String data = "\"data1\",\"data2\"";
            String[] source = new String[] { header, data };

            DataTable dataTable = csvConverterService.Convert(source);

            Assert.IsTrue(dataTable.Columns.Contains("First Name"), "Should have header without quotes.");
            Assert.IsTrue(dataTable.Columns.Contains("Last Name"), "Should have header without quotes.");
        
        }

        public void ConvertShouldFindAndParseRow()
        {

            ICsvConverterService csvConverterService = new CsvConverterService(MockRepository.GenerateStub<ILog>());

            String header = "First Nane,First_Name";
            String data = "data1,data2";
            String[] source = new String[] { header, data };

            DataTable dataTable = csvConverterService.Convert(source);

            Assert.AreEqual(1, dataTable.Rows.Count, "Should have the right number of rows.");

        }

        public void ConvertShouldRemoveExtraDoubleQuotes()
        {

            ICsvConverterService csvConverterService = new CsvConverterService(MockRepository.GenerateStub<ILog>());

            String header = "FirstName,LastName";
            String data = "\"data1\",\"data2\"";
            String[] source = new String[] { header, data };

            DataTable dataTable = csvConverterService.Convert(source);

            Assert.AreEqual("data1", dataTable.Rows[0]["FirstName"], "Excessive quotes should be removed.");

        }

        public void ConvertShouldThrowExceptionIfColumnsUmbalancedHeaders()
        {

            ICsvConverterService csvConverterService = new CsvConverterService(MockRepository.GenerateStub<ILog>());

            String header = "First Name,First_Name,FirstName";
            String data = "data1,data2";
            String[] source = new String[] { header, data };

            DataTable dataTable = csvConverterService.Convert(source);

            Throws.TypeOf<System.InvalidOperationException>();
        }

        public void ConvertShouldThrowExceptionIfColumnsUmbalancedRows()
        {

            ICsvConverterService csvConverterService = new CsvConverterService(MockRepository.GenerateStub<ILog>());

            String header = "First Name,Last Name";
            String data1 = "data1,data2";
            String data2 = "data1,data2,data3";
            String[] source = new String[] { header, data1, data2 };

            DataTable dataTable = csvConverterService.Convert(source);

            Throws.TypeOf<System.InvalidOperationException>();
        }

        [Test]
        public void ConvertShouldFindAndParseDatesLegacyFormat()
        {

            ICsvConverterService csvConverterService = new CsvConverterService(MockRepository.GenerateStub<ILog>());

            String header = "Effective Date,Name";
            String data = "20160101,data2";
            String[] source = new String[] { header, data };

            DataTable dataTable = csvConverterService.Convert(source);

            Assert.AreEqual("1/1/2016 12:00:00 AM", dataTable.Rows[0]["Effective Date"], "Should have parsed and saved the date.");

        }

        [Test]
        public void ConvertShouldFindAndParseDatesIsoDayFormat()
        {

            ICsvConverterService csvConverterService = new CsvConverterService(MockRepository.GenerateStub<ILog>());

            String header = "Effective Date,Name";
            String data = "2016-01-01,data2";
            String[] source = new String[] { header, data };

            DataTable dataTable = csvConverterService.Convert(source);

            Assert.AreEqual("1/1/2016 12:00:00 AM", dataTable.Rows[0]["Effective Date"], "Should have parsed and saved the date.");

        }

        public void ConvertShouldThroughExceptionIfFirstRowIsNotRecognizedAsHeaders()
        {

            ICsvConverterService csvConverterService = new CsvConverterService(MockRepository.GenerateStub<ILog>());

            String header = "Header1,Header2";
            String data = "2016-01-01,data2";
            String[] source = new String[] { header, data };

            DataTable dataTable = csvConverterService.Convert(source);

            Assert.AreEqual("1/1/2016 12:00:00 AM", dataTable.Rows[0]["Effective Date"], "Should have parsed and saved the date.");

            Throws.TypeOf<System.InvalidOperationException>();

        }

    }

}
