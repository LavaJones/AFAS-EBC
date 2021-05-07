using System;

using System.Data;

using NUnit.Framework;
using Rhino.Mocks;

using Afas.AfComply.Domain;
using Afas.Domain;

namespace Afas.AfComply.DomainTests
{

    [TestFixture]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class DataColumnCollectionExtensionMethodsTests
    {

        [Test]
        public void DetermineYearFromColumnNamesDefaultsTo2015()
        {

            Assert.AreEqual("2015", new DataTable().Columns.DetermineYearFromColumnNames(), "No matching columns should be the default.");

        }

        [Test]
        public void DetermineYearFromColumnNamesIs2016IfItExists()
        {

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("January_2016_Hours_Worked"));

            Assert.AreEqual("2016", dataTable.Columns.DetermineYearFromColumnNames(), "Column containing year should be detected.");

        }

        [Test]
        public void DetermineYearFromColumnNamesIs2015IfItExists()
        {

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("January_2015_Hours_Worked"));

            Assert.AreEqual("2015", dataTable.Columns.DetermineYearFromColumnNames(), "Column containing year should be detected.");

        }

        [Test]
        public void DetermineYearFromColumnNamesIs2014IfItExists()
        {

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("January_2014_Hours_Worked"));

            Assert.AreEqual("2014", dataTable.Columns.DetermineYearFromColumnNames(), "Column containing year should be detected.");

        }

        [Test]
        public void DetermineYearFromColumnNamesIs2013IfItExists()
        {

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("January_2013_Hours_Worked"));

            Assert.AreEqual("2013", dataTable.Columns.DetermineYearFromColumnNames(), "Column containing year should be detected.");

        }

        [Test]
        public void DetermineYearFromColumnNamesIs2014IfItIsTheLatest()
        {

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("January_2013_Hours_Worked"));
            dataTable.Columns.Add(new DataColumn("January_2014_Hours_Worked"));

            Assert.AreEqual("2014", dataTable.Columns.DetermineYearFromColumnNames(), "Column containing year should be detected.");

        }

        [Test]
        public void DetermineYearFromColumnNamesIs2015IfItIsTheLatest()
        {

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("January_2013_Hours_Worked"));
            dataTable.Columns.Add(new DataColumn("January_2014_Hours_Worked"));
            dataTable.Columns.Add(new DataColumn("January_2015_Hours_Worked"));

            Assert.AreEqual("2015", dataTable.Columns.DetermineYearFromColumnNames(), "Column containing year should be detected.");

        }

        [Test]
        public void DetermineYearFromColumnNamesIs2016IfItIsTheLatest()
        {

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("January_2013_Hours_Worked"));
            dataTable.Columns.Add(new DataColumn("January_2014_Hours_Worked"));
            dataTable.Columns.Add(new DataColumn("January_2015_Hours_Worked"));
            dataTable.Columns.Add(new DataColumn("January_2016_Hours_Worked"));

            Assert.AreEqual("2016", dataTable.Columns.DetermineYearFromColumnNames(), "Column containing year should be detected.");

        }

    }

}
