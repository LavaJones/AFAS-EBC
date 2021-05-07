using System;
using System.Collections.Generic;

using System.Data;

using NUnit.Framework;
using Rhino.Mocks;

using Afas.AfComply.Domain;

namespace Afas.AfComply.DomainTests
{

    [TestFixture]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class DataRowCollectionExtensionMethodsTests
    {

        [Test]
        public void RemoveRowsLeavesEmptyDataTableAlone()
        {

            DataTable dataTable = new DataTable("CSVTable");

            IList<DataRow> dataRowsToRemove = new List<DataRow>();

            dataTable.Rows.RemoveRows(dataRowsToRemove);

        }

        [Test]
        public void RemoveRowsLeavesDataTableAloneEmptyRemoveCollection()
        {

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("DOES_NOT_MATTER", typeof(String));

            IList<DataRow> dataRowsToRemove = new List<DataRow>();

            DataRow dataRow = dataTable.NewRow();
            dataRow["DOES_NOT_MATTER"] = String.Empty;
            dataTable.Rows.Add(dataRow);

            dataTable.Rows.RemoveRows(dataRowsToRemove);

            Assert.AreEqual(1, dataTable.Rows.Count, "Should be left alone.");

        }

        [Test]
        public void RemoveRowsRemovesRequestedRowsIncludingLastRowInTable()
        {

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("DOES_NOT_MATTER", typeof(String));

            IList<DataRow> dataRowsToRemove = new List<DataRow>();

            DataRow dataRow = dataTable.NewRow();
            dataRow["DOES_NOT_MATTER"] = String.Empty;
            dataTable.Rows.Add(dataRow);
            dataRowsToRemove.Add(dataRow);

            dataTable.Rows.RemoveRows(dataRowsToRemove);

            Assert.AreEqual(0, dataTable.Rows.Count, "Should be the right count.");

        }

        [Test]
        public void RemoveRowsRemovesRequestedRowsLeavingOtherRows()
        {

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("DOES_NOT_MATTER", typeof(String));

            IList<DataRow> dataRowsToRemove = new List<DataRow>();

            DataRow dataRow1 = dataTable.NewRow();
            dataRow1["DOES_NOT_MATTER"] = String.Empty;
            dataTable.Rows.Add(dataRow1);
            dataRowsToRemove.Add(dataRow1);

            DataRow dataRow2 = dataTable.NewRow();
            dataRow2["DOES_NOT_MATTER"] = String.Empty;
            dataTable.Rows.Add(dataRow2);

            dataTable.Rows.RemoveRows(dataRowsToRemove);

            Assert.AreEqual(1, dataTable.Rows.Count, "Should be the right count.");
            Assert.AreSame(dataRow2, dataTable.Rows[0], "Should be the original item.");

        }

    }

}
