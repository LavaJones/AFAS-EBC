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
    public class ReplaceByValueFormatCommandTests
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
            testRow["TestColumn"] = "RemoveValue";
            testData.Data.Rows.Add(testRow);

            TestObject = new ReplaceByValueFormatCommand();
            TestObject.Parameters.Add("ColumnName","TestColumn");
            TestObject.Parameters.Add("RemoveValue", "RemoveValue");
            TestObject.Parameters.Add("ReplaceValue", "ReplaceValue");
            TestObject.CreatedBy = "tester";
            TestObject.ModifiedBy = "tester";
            TestObject.EntityStatus = Afc.Core.Domain.EntityStatusEnum.Active;
            TestObject.MetaData = MockRepository.GenerateMock<ImportMetaData>();

        }

        [Test]
        public void ReplaceByValueFormatCommand_ConstructorTest()
        {
            var obj = new ReplaceByValueFormatCommand();
            Assert.NotNull(obj);

            Assert.AreNotEqual(0, obj.EnsureIsWellFormed.Count);

            Assert.AreEqual(0, TestObject.EnsureIsWellFormed.Count);

        }

        [Test]
        public void ReplaceByValueFormatCommand_GetterTest()
        {

            Assert.AreEqual(ImportFormatCommandTypes.ReplaceByValue, TestObject.CommandType);
            Assert.IsNotNull(TestObject.RequiredParameters);
            Assert.AreEqual(3, TestObject.RequiredParameters.Count);
            Assert.IsTrue(TestObject.RequiredParameters.Contains("ColumnName"));
            Assert.IsTrue(TestObject.RequiredParameters.Contains("RemoveValue"));
            Assert.IsTrue(TestObject.RequiredParameters.Contains("ReplaceValue"));
            Assert.IsNotNull(TestObject.OptionalParameters);
            Assert.AreEqual(0, TestObject.OptionalParameters.Count);

        }

        [Test]
        public void ReplaceByValueFormatCommand_ApplyTo_HappyPath()
        {

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsFalse(testRow.IsNull("TestColumn"));

            Assert.IsTrue(TestObject.ApplyTo(testData));

            Assert.IsFalse(testRow.IsNull("TestColumn"));

            Assert.AreEqual("ReplaceValue", testRow["TestColumn"]);

        }

        [Test]
        public void ReplaceByValueFormatCommand_ApplyTo_NonExistingColumn()
        {

            TestObject.Parameters["ColumnName"] = "OtherColumn";

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsFalse(testRow.IsNull("TestColumn"));

            Assert.IsFalse(TestObject.ApplyTo(testData));

            Assert.IsFalse(testRow.IsNull("TestColumn"));

            Assert.AreEqual("RemoveValue", testRow["TestColumn"]);

        }

        [Test]
        public void ReplaceByValueFormatCommand_ApplyTo_RemoveValueIsNull()
        {

            TestObject.Parameters["RemoveValue"] = null;

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsFalse(testRow.IsNull("TestColumn"));

            Assert.IsFalse(TestObject.ApplyTo(testData));

            Assert.IsFalse(testRow.IsNull("TestColumn"));

            Assert.AreEqual("RemoveValue", testRow["TestColumn"]);

        }        

        private List<DataRow> BuildRows(int number)
        {

            var rows = new List<DataRow>();

            for (int i = 1; i < number; i++)
            {
                DataRow row = testData.Data.NewRow();
                row["TestColumn"] = "RemoveValue";
                testData.Data.Rows.Add(row);
                rows.Add(row);
            }

            return rows;

        }

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(40)]
        public void ReplaceByValueFormatCommand_ApplyTo_HappyPath_MultiRow(int rowCount)
        {

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsFalse(testRow.IsNull("TestColumn"));

            List<DataRow> builtRows = BuildRows(rowCount);

            Assert.IsTrue(TestObject.ApplyTo(testData));

            Assert.AreEqual("ReplaceValue", testRow["TestColumn"]);

            foreach (DataRow row in builtRows)
            {
                Assert.AreEqual("ReplaceValue", row["TestColumn"]);
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
        public void ReplaceByValueFormatCommand_ApplyTo_ExistingOtherValue(int rowCount)
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
        public void ReplaceByValueFormatCommand_ApplyTo_SomeRemoveValues(int rowCount)
        {

            testRow["TestColumn"] = "RemoveValue";
            List<DataRow> builtRows = FillRows(rowCount);

            foreach (DataRow row in builtRows)
            {
                Assert.AreEqual("TestValue", row["TestColumn"]);
            }
            Assert.IsFalse(testRow.IsNull("TestColumn"));

            Assert.IsTrue(TestObject.ApplyTo(testData));

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.AreEqual("ReplaceValue", testRow["TestColumn"]);

            foreach (DataRow row in builtRows)
            {
                Assert.AreEqual("TestValue", row["TestColumn"]);
            }
        
        }
    }
}
