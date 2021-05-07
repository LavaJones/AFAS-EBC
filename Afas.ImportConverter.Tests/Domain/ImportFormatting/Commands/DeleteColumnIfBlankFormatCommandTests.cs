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
    public class DeleteColumnIfBlankFormatCommandTests
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
            testRow["TestColumn"] = null;
            testRow["OtherColumn"] = "Rando";
            testData.Data.Rows.Add(testRow);


            TestObject = new DeleteColumnIfBlankFormatCommand();
            TestObject.Parameters.Add("ColumnName","TestColumn");
            TestObject.CreatedBy = "tester";
            TestObject.ModifiedBy = "tester";
            TestObject.EntityStatus = Afc.Core.Domain.EntityStatusEnum.Active;
            TestObject.MetaData = MockRepository.GenerateMock<ImportMetaData>();

        }

        [Test]
        public void DeleteColumnIfBlankFormatCommand_ConstructorTest()
        {
            var obj = new DeleteColumnIfBlankFormatCommand();
            Assert.NotNull(obj);

            Assert.AreNotEqual(0, obj.EnsureIsWellFormed.Count);

            Assert.AreEqual(0, TestObject.EnsureIsWellFormed.Count);

        }

        [Test]
        public void DeleteColumnIfBlankFormatCommand_GetterTest()
        {

            Assert.AreEqual(ImportFormatCommandTypes.DeleteColumnIfBlank, TestObject.CommandType);
            Assert.IsNotNull(TestObject.RequiredParameters);
            Assert.AreEqual(1, TestObject.RequiredParameters.Count);
            Assert.IsTrue(TestObject.RequiredParameters.Contains("ColumnName"));
            Assert.IsNotNull(TestObject.OptionalParameters);
            Assert.AreEqual(0, TestObject.OptionalParameters.Count);

        }

        [Test]
        public void DeleteColumnIfBlankFormatCommand_ApplyTo_HappyPath()
        {

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsTrue(testRow.IsNull("TestColumn"));

            Assert.IsTrue(TestObject.ApplyTo(testData));

            Assert.IsFalse(testData.Data.Columns.Contains("TestColumn"));

        }

        [Test]
        public void DeleteColumnIfBlankFormatCommand_ApplyTo_NonExistingColumn()
        {

            testData.Data.Columns.Remove("TestColumn");

            Assert.IsFalse(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsFalse(TestObject.ApplyTo(testData));

            Assert.IsFalse(testData.Data.Columns.Contains("TestColumn"));

        }
       
        [TestCase("this that the other")]
        [TestCase("asdf")]
        [TestCase(" asdf ")]
        [TestCase("null")]
        [TestCase("NULL")]
        [TestCase("blank")]
        [TestCase("empty")]
        [TestCase("string.empty")]
        public void DeleteColumnIfBlankFormatCommand_ApplyTo_HasValue(string value)
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

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("     ")]
        [TestCase("      ")]
        [TestCase("\n\r")]
        public void DeleteColumnIfBlankFormatCommand_ApplyTo_HasBlankValue(string value)
        {

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            testRow["TestColumn"] = value;

            Assert.IsTrue(TestObject.ApplyTo(testData));

            Assert.IsFalse(testData.Data.Columns.Contains("TestColumn"));

        }

        private List<DataRow> BuildRows(int number)
        {

            var rows = new List<DataRow>();

            for (int i = 1; i < number; i++)
            {
                DataRow row = testData.Data.NewRow();
                row["TestColumn"] = null;
                row["OtherColumn"] = "Rando";
                testData.Data.Rows.Add(row);
                rows.Add(row);
            }

            return rows;

        }

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(40)]
        public void DeleteColumnIfBlankFormatCommand_ApplyTo_HappyPath_MultiRow(int rowCount)
        {

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsTrue(testRow.IsNull("TestColumn"));

            List<DataRow> builtRows = BuildRows(rowCount);

            Assert.IsTrue(TestObject.ApplyTo(testData));

            Assert.IsFalse(testData.Data.Columns.Contains("TestColumn"));
            
        }

        private List<DataRow> FillRows(int number, string column = "TestColumn", string value = null)
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
        public void DeleteColumnIfBlankFormatCommand_ApplyTo_AllBlank(int rowCount)
        {

            List<DataRow> builtRows = FillRows(rowCount);

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsTrue(TestObject.ApplyTo(testData));

            Assert.IsFalse(testData.Data.Columns.Contains("TestColumn"));

        }

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(40)]
        public void DeleteColumnIfBlankFormatCommand_ApplyTo_SomeHaveValues(int rowCount)
        {

            testRow["TestColumn"] = "Some Value";
            List<DataRow> builtRows = FillRows(rowCount);

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsFalse(testRow.IsNull("TestColumn"));

            Assert.IsTrue(TestObject.ApplyTo(testData));

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));
            
        }
    }
}
