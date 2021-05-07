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
    public class RenameColumnFormatCommandTests
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
            testData.Data.Columns.Add("OtherColumn", typeof(string));
            testRow = testData.Data.NewRow();
            testRow["TestColumn"] = "TestData";
            testRow["OtherColumn"] = "Rando";
            testData.Data.Rows.Add(testRow);

            TestObject = new RenameColumnFormatCommand();
            TestObject.Parameters.Add("RenameFrom", "TestColumn");
            TestObject.Parameters.Add("RenameTo", "RenamedColumn");
            TestObject.CreatedBy = "tester";
            TestObject.ModifiedBy = "tester";
            TestObject.EntityStatus = Afc.Core.Domain.EntityStatusEnum.Active;
            TestObject.MetaData = MockRepository.GenerateMock<ImportMetaData>();

        }

        [Test]
        public void RenameColumnFormatCommand_ConstructorTest()
        {

            var obj = new RenameColumnFormatCommand();
            Assert.NotNull(obj);

            Assert.AreNotEqual(0, obj.EnsureIsWellFormed.Count);

            Assert.AreEqual(0, TestObject.EnsureIsWellFormed.Count);

        }

        [Test]
        public void RenameColumnFormatCommand_GetterTest()
        {

            Assert.AreEqual(ImportFormatCommandTypes.RenameColumn, TestObject.CommandType);
            Assert.IsNotNull(TestObject.RequiredParameters);
            Assert.AreEqual(2, TestObject.RequiredParameters.Count);
            Assert.IsTrue(TestObject.RequiredParameters.Contains("RenameFrom"));
            Assert.IsTrue(TestObject.RequiredParameters.Contains("RenameTo"));
            Assert.IsNotNull(TestObject.OptionalParameters);
            Assert.AreEqual(0, TestObject.OptionalParameters.Count);

        }

        [Test]
        public void RenameColumnFormatCommand_ApplyTo_HappyPath()
        {

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));
            Assert.IsFalse(testData.Data.Columns.Contains("RenamedColumn"));

            Assert.AreEqual("TestData", testRow["TestColumn"]);

            Assert.IsTrue(TestObject.ApplyTo(testData));

            Assert.IsFalse(testData.Data.Columns.Contains("TestColumn"));
            Assert.IsTrue(testData.Data.Columns.Contains("RenamedColumn"));

            Assert.AreEqual("TestData", testRow["RenamedColumn"]);

        }

        [Test]
        public void RenameColumnFormatCommand_ApplyTo_NonExistingColumn()
        {

            TestObject.Parameters["RenameFrom"] = "RandoColumn";

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));
            Assert.IsFalse(testData.Data.Columns.Contains("RenamedColumn"));

            Assert.AreEqual("TestData", testRow["TestColumn"]);

            Assert.IsFalse(TestObject.ApplyTo(testData));

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));
            Assert.IsFalse(testData.Data.Columns.Contains("RenamedColumn"));

            Assert.AreEqual("TestData", testRow["TestColumn"]);

        }
        
        private List<DataRow> BuildRows(int number)
        {

            var rows = new List<DataRow>();

            for (int i = 1; i < number; i++)
            {
                DataRow row = testData.Data.NewRow();
                row["TestColumn"] = "TestData";
                row["OtherColumn"] = "Rando";
                testData.Data.Rows.Add(row);
                rows.Add(row);
            }

            return rows;

        }

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(40)]
        public void RenameColumnFormatCommand_ApplyTo_HappyPath_MultiRow(int rowCount)
        {

            List<DataRow> builtRows = BuildRows(rowCount);

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));
            Assert.IsFalse(testData.Data.Columns.Contains("RenamedColumn"));

            Assert.AreEqual("TestData", testRow["TestColumn"]);

            Assert.IsTrue(TestObject.ApplyTo(testData));

            Assert.IsFalse(testData.Data.Columns.Contains("TestColumn"));
            Assert.IsTrue(testData.Data.Columns.Contains("RenamedColumn"));

            Assert.AreEqual("TestData", testRow["RenamedColumn"]);

            foreach (DataRow row in builtRows)
            {
                Assert.AreEqual("TestData", row["RenamedColumn"]);
            }

        }
    }
}
