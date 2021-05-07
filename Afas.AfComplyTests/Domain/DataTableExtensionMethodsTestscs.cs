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
    public class DataTableExtensionMethodsTests
    {

        [Test]
        public void LeftPadSocialSecurityNumberLeaves9DigitSSNsAlone()
        {

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["SSN"] = "123456789";
            dataTable.Rows.Add(dataRow);

            dataTable.LeftPadSocialSecurityNumber("SSN");

            Assert.AreEqual("123456789", dataTable.Rows[0]["SSN"], "Should be left alone, 9 digits.");

        }

        [Test]
        public void LeftPadSocialSecurityNumberLeftZeroPadsForShortNumbers()
        {

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["SSN"] = "56789";
            dataTable.Rows.Add(dataRow);

            dataTable.LeftPadSocialSecurityNumber("SSN");

            Assert.AreEqual("000056789", dataTable.Rows[0]["SSN"], "Should be  left padded to 9 digits.");

        }

        [Test]
        public void LeftPadSocialSecurityNumberLeavesNullsAlone()
        {

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["SSN"] = null;
            dataTable.Rows.Add(dataRow);

            dataTable.LeftPadSocialSecurityNumber("SSN");

            Assert.IsTrue(dataTable.Rows[0].IsNull("SSN"), "Should be left alone, null.");

        }

        [Test]
        public void LeftPadSocialSecurityNumberLeavesZeroLenghtStringsAlone()
        {

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["SSN"] = String.Empty;
            dataTable.Rows.Add(dataRow);

            dataTable.LeftPadSocialSecurityNumber("SSN");

            Assert.AreEqual(String.Empty, dataTable.Rows[0]["SSN"], "Should be left alone, zero length.");

        }

        [Test]
        public void IsRowBlankReturnsTrueWithZeroColumnRow()
        {
            DataTable dataTable = new DataTable("CSVTable");
            DataRow dataRow = dataTable.NewRow();
            Assert.IsTrue(StandardDataTableExtensionMethods.IsRowBlank(dataRow));
        }

        [Test]
        public void IsRowBlankReturnsTrueWithTwoBlankColumns()
        {
            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("FName", typeof(String));
            dataTable.Columns.Add("LName", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["FName"] = String.Empty;
            dataRow["LName"] = String.Empty;

            Assert.IsTrue(StandardDataTableExtensionMethods.IsRowBlank(dataRow));
        }

        [Test]
        public void IsRowBlankReturnsTrueWithTwoNullColumns()
        {
            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("FName", typeof(String));
            dataTable.Columns.Add("LName", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["FName"] = null;
            dataRow["LName"] = null;

            Assert.IsTrue(StandardDataTableExtensionMethods.IsRowBlank(dataRow));
        }

        [Test]
        public void IsRowBlankReturnsFalseWithOneBlankAndOneNonBlankColumn()
        {
            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("FName", typeof(String));
            dataTable.Columns.Add("LName", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["FName"] = "Test";
            dataRow["LName"] = String.Empty;

            Assert.IsFalse(StandardDataTableExtensionMethods.IsRowBlank(dataRow));
        }

        [Test]
        public void IsRowBlankReturnsFalseWithOnlyNonBlankColumns()
        {
            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("FName", typeof(String));
            dataTable.Columns.Add("LName", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["FName"] = "Test";
            dataRow["LName"] = String.Empty;

            Assert.IsFalse(StandardDataTableExtensionMethods.IsRowBlank(dataRow));
        }

        [Test]
        public void RowIsNotBlankReturnsTrueWithNonBlankColumns()
        {
            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("FName", typeof(String));
            dataTable.Columns.Add("LName", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["FName"] = "Test";
            dataRow["LName"] = String.Empty;

            Assert.IsTrue(Afas.Domain.DataTableExtensionMethods.RowIsNotBlank(dataRow));
        }

    }

}
