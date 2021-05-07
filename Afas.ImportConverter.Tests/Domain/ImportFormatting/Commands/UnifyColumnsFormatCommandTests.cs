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
    public class UnifyColumnsFormatCommandTests
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
            testData.Data.Columns.Add("TestColumn1", typeof(string));
            testData.Data.Columns.Add("OtherColumn", typeof(string));
            testRow = testData.Data.NewRow();
            testRow["TestColumn1"] = "TestData";
            testRow["OtherColumn"] = "Rando";
            testData.Data.Rows.Add(testRow);

            TestObject = new UnifyColumnsFormatCommand();
            TestObject.Parameters.Add("RenameFrom", "TestColumn1");
            TestObject.Parameters.Add("RenameOther", "TestColumn2");
            TestObject.Parameters.Add("UnifyTo", "RenamedColumn");
            TestObject.CreatedBy = "tester";
            TestObject.ModifiedBy = "tester";
            TestObject.EntityStatus = Afc.Core.Domain.EntityStatusEnum.Active;
            TestObject.MetaData = MockRepository.GenerateMock<ImportMetaData>();

        }

        [Test]
        public void UnifyColumnsFormatCommand_ConstructorTest()
        {

            var obj = new UnifyColumnsFormatCommand();
            Assert.NotNull(obj);

            Assert.AreNotEqual(0, obj.EnsureIsWellFormed.Count);

            Assert.AreEqual(0, TestObject.EnsureIsWellFormed.Count);

        }

        [Test]
        public void UnifyColumnsFormatCommand_GetterTest()
        {

            Assert.AreEqual(ImportFormatCommandTypes.UnifyColumnNames, TestObject.CommandType);
            Assert.IsNotNull(TestObject.RequiredParameters);
            Assert.AreEqual(1, TestObject.RequiredParameters.Count);
            Assert.IsTrue(TestObject.RequiredParameters.Contains("UnifyTo"));
            Assert.IsNotNull(TestObject.OptionalParameters);
            Assert.AreEqual(0, TestObject.OptionalParameters.Count);

        }

        [Test]
        public void UnifyColumnsFormatCommand_ApplyTo_HappyPath()
        {

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn1"));
            Assert.IsFalse(testData.Data.Columns.Contains("RenamedColumn"));

            Assert.AreEqual("TestData", testRow["TestColumn1"]);

            Assert.IsTrue(TestObject.ApplyTo(testData));

            Assert.IsFalse(testData.Data.Columns.Contains("TestColumn1"));
            Assert.IsTrue(testData.Data.Columns.Contains("RenamedColumn"));

            Assert.AreEqual("TestData", testRow["RenamedColumn"]);

        }

        [Test]
        public void UnifyColumnsFormatCommand_ApplyTo_HappyPath_OtherColumn()
        {
            testData.Data.Columns["TestColumn1"].ColumnName = "TestColumn2";

            Assert.IsFalse(testData.Data.Columns.Contains("TestColumn1"));
            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn2"));
            Assert.IsFalse(testData.Data.Columns.Contains("RenamedColumn"));

            Assert.AreEqual("TestData", testRow["TestColumn2"]);

            Assert.IsTrue(TestObject.ApplyTo(testData));

            Assert.IsFalse(testData.Data.Columns.Contains("TestColumn1"));
            Assert.IsFalse(testData.Data.Columns.Contains("TestColumn2"));
            Assert.IsTrue(testData.Data.Columns.Contains("RenamedColumn"));

            Assert.AreEqual("TestData", testRow["RenamedColumn"]);

        }

        [Test]
        public void UnifyColumnsFormatCommand_ApplyTo_UnifyToExistingColumn()
        {

            TestObject.Parameters["UnifyTo"] = "OtherColumn";

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn1"));
            Assert.IsTrue(testData.Data.Columns.Contains("OtherColumn"));

            Assert.AreEqual("TestData", testRow["TestColumn1"]);

            Assert.IsFalse(TestObject.ApplyTo(testData));

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn1"));
            Assert.IsTrue(testData.Data.Columns.Contains("OtherColumn"));

            Assert.AreEqual("TestData", testRow["TestColumn1"]);

        }

        [Test]
        public void UnifyColumnsFormatCommand_ApplyTo_MultipleColumns()
        {

            testData.Data.Columns.Add("TestColumn2", typeof(string));

            testRow = testData.Data.NewRow();
            testRow["TestColumn1"] = "TestData";
            testRow["TestColumn2"] = "TestData";
            testRow["OtherColumn"] = "Rando";
            testData.Data.Rows.Add(testRow);

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn1"));
            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn2"));
            Assert.IsFalse(testData.Data.Columns.Contains("RenamedColumn"));

            Assert.AreEqual("TestData", testRow["TestColumn1"]);
            Assert.AreEqual("TestData", testRow["TestColumn2"]);

            Assert.IsTrue(TestObject.ApplyTo(testData));

            Assert.IsFalse(testData.Data.Columns.Contains("TestColumn1"));
            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn2"));
            Assert.IsTrue(testData.Data.Columns.Contains("RenamedColumn"));

            Assert.AreEqual("TestData", testRow["RenamedColumn"]);
            Assert.AreEqual("TestData", testRow["TestColumn2"]);

        }

        private List<DataRow> BuildRows(int number)
        {

            var rows = new List<DataRow>();

            for (int i = 1; i < number; i++)
            {
                DataRow row = testData.Data.NewRow();
                row["TestColumn1"] = "TestData";
                row["OtherColumn"] = "Rando";
                testData.Data.Rows.Add(row);
                rows.Add(row);
            }

            return rows;

        }

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(40)]
        public void UnifyColumnsFormatCommand_ApplyTo_HappyPath_MultiRow(int rowCount)
        {

            List<DataRow> builtRows = BuildRows(rowCount);

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn1"));
            Assert.IsFalse(testData.Data.Columns.Contains("RenamedColumn"));

            Assert.AreEqual("TestData", testRow["TestColumn1"]);

            Assert.IsTrue(TestObject.ApplyTo(testData));

            Assert.IsFalse(testData.Data.Columns.Contains("TestColumn1"));
            Assert.IsTrue(testData.Data.Columns.Contains("RenamedColumn"));

            Assert.AreEqual("TestData", testRow["RenamedColumn"]);

            foreach (DataRow row in builtRows)
            {
                Assert.AreEqual("TestData", row["RenamedColumn"]);
            }

        }
    }
}
