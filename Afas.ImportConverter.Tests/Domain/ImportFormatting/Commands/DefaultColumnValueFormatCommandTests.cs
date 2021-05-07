using Afas.ImportConverter.Domain.ImportFormatting;
using Afas.ImportConverter.Domain.POCO;
using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.ImportConverter.Tests.Domain.ImportFormatting.Commands
{
    [TestFixture]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class DefaultColumnValueFormatCommandTests
    {
        private ImportData testData;
        private DataRow testRow;
        
        private AImportFormatCommand TestObject;

        /// <summary>
        /// Test setup that should run before every test to make sure that the Mocks are in a consistent state
        /// </summary>
        [SetUp]
        public void Init()
        {
            testData = new ImportData();
            testData.Data = new DataTable();
            testData.Data.Columns.Add("TestColumn", typeof(string));
            testRow = testData.Data.NewRow();
            testRow["TestColumn"] = null;
            testData.Data.Rows.Add(testRow);

            TestObject = new DefaultColumnValueFormatCommand();
            TestObject.Parameters.Add("ColumnName","TestColumn");
            TestObject.Parameters.Add("DefaultValue", "DefaultValue");
            TestObject.CreatedBy = "tester";
            TestObject.ModifiedBy = "tester";
            TestObject.EntityStatus = Afc.Core.Domain.EntityStatusEnum.Active;
            TestObject.MetaData = MockRepository.GenerateMock<ImportMetaData>();

        }

        [Test]
        public void DefaultColumnValueFormatCommand_ConstructorTest()
        {
            var obj = new DefaultColumnValueFormatCommand();
            Assert.NotNull(obj);

            Assert.AreNotEqual(0, obj.EnsureIsWellFormed.Count);

            Assert.AreEqual(0, TestObject.EnsureIsWellFormed.Count);

        }

        [Test]
        public void DefaultColumnValueFormatCommand_GetterTest()
        {

            Assert.AreEqual(ImportFormatCommandTypes.DefaultColumnIfBlank, TestObject.CommandType);
            Assert.IsNotNull(TestObject.RequiredParameters);
            Assert.AreEqual(2, TestObject.RequiredParameters.Count);
            Assert.IsTrue(TestObject.RequiredParameters.Contains("ColumnName"));
            Assert.IsTrue(TestObject.RequiredParameters.Contains("DefaultValue"));
            Assert.IsNotNull(TestObject.OptionalParameters);
            Assert.AreEqual(0, TestObject.OptionalParameters.Count);

        }

        [Test]
        public void DefaultColumnValueFormatCommand_ApplyTo_HappyPath()
        {

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsTrue(testRow.IsNull("TestColumn"));

            Assert.IsTrue(TestObject.ApplyTo(testData));

            Assert.IsFalse(testRow.IsNull("TestColumn"));

            Assert.AreEqual("DefaultValue", testRow["TestColumn"]);
            
        }
        
        [Test]
        public void DefaultColumnValueFormatCommand_ApplyTo_NonExistingColumn()
        {

            testData.Data.Columns.Remove("TestColumn");

            Assert.IsFalse(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsFalse(TestObject.ApplyTo(testData));

            Assert.IsFalse(testData.Data.Columns.Contains("TestColumn"));

        }
        
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("      ")]
        [TestCase(" ")]
        public void DefaultColumnValueFormatCommand_ApplyTo_BadParamDefaultValue(string value)
        {

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsTrue(testRow.IsNull("TestColumn"));

            TestObject.Parameters["DefaultValue"] = value;

            Assert.IsFalse(TestObject.ApplyTo(testData));

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsTrue(testRow.IsNull("TestColumn"));
        
        }
                 
        [Test]
        public void DefaultColumnValueFormatCommand_ApplyTo_MissingParamDefaultValue()
        {

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsTrue(testRow.IsNull("TestColumn"));

            TestObject.Parameters.Remove("DefaultValue");

            Assert.IsFalse(TestObject.ApplyTo(testData));

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsTrue(testRow.IsNull("TestColumn"));

        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("      ")]
        [TestCase(" ")]
        public void DefaultColumnValueFormatCommand_ApplyTo_ValueAlreadySet_ButBlank(string value)
        {

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsTrue(testRow.IsNull("TestColumn"));

            testRow["TestColumn"] = value;

            Assert.IsFalse(testRow.IsNull("TestColumn"));

            Assert.IsTrue(TestObject.ApplyTo(testData));

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsFalse(testRow.IsNull("TestColumn"));

            Assert.AreEqual("DefaultValue", testRow["TestColumn"]);

        }

        [TestCase("this that the other")]
        [TestCase("asdf")]
        [TestCase(" asdf ")]
        [TestCase("null")]
        [TestCase("NULL")]
        [TestCase("blank")]
        [TestCase("empty")]
        [TestCase("string.empty")]
        public void DefaultColumnValueFormatCommand_ApplyTo_ValueAlreadySet(string value)
        {

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsTrue(testRow.IsNull("TestColumn"));

            testRow["TestColumn"] = value;

            Assert.IsFalse(testRow.IsNull("TestColumn"));

            Assert.IsTrue(TestObject.ApplyTo(testData));

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsFalse(testRow.IsNull("TestColumn"));

            Assert.AreEqual(value, testRow["TestColumn"]);

        }


        private List<DataRow> BuildRows(int number)
        {

            var rows = new List<DataRow>();

            for (int i = 1; i < number; i++)
            {
                DataRow row = testData.Data.NewRow();
                testData.Data.Rows.Add(row);
                rows.Add(row);
            }

            return rows;

        }

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(40)]
        public void DefaultColumnValueFormatCommand_ApplyTo_HappyPath_MultiRow(int rowCount)
        {

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsTrue(testRow.IsNull("TestColumn"));

            List<DataRow> builtRows = BuildRows(rowCount);

            Assert.IsTrue(TestObject.ApplyTo(testData));

            foreach (DataRow row in builtRows)
            {
                Assert.AreEqual("DefaultValue", row["TestColumn"]);
            }

        }

        private List<DataRow> FillRows(int number, string column = "TestColumn", string value = "TestValue")
        {

            List<DataRow> rows = BuildRows(number);

            foreach (DataRow row in rows)
            {
                row[column] = value;
            }

            return rows;

        }

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(40)]
        public void DefaultColumnValueFormatCommand_ApplyTo_ExistingValue(int rowCount)
        {

            testRow["TestColumn"] = "TestValue";
            List<DataRow> builtRows = FillRows(rowCount);

            foreach (DataRow row in builtRows)
            {
                Assert.AreEqual("TestValue", testRow["TestColumn"]);
            }

            Assert.IsTrue(TestObject.ApplyTo(testData));

            foreach (DataRow row in builtRows)
            {
                Assert.AreEqual("TestValue", testRow["TestColumn"]);
            }

        }

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(40)]
        public void DefaultColumnValueFormatCommand_ApplyTo_SomeExistingValues(int rowCount)
        {

            testRow["TestColumn"] = null;
            List<DataRow> builtRows = FillRows(rowCount);

            foreach (DataRow row in builtRows)
            {
                Assert.AreEqual("TestValue", row["TestColumn"]);
            }
            Assert.IsTrue(testRow.IsNull("TestColumn"));

            Assert.IsTrue(TestObject.ApplyTo(testData));

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.AreEqual("DefaultValue", testRow["TestColumn"]);

            foreach (DataRow row in builtRows)
            {
                Assert.AreEqual("TestValue", row["TestColumn"]);
            }

        }
    }
}
