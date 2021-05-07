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
    public class DefaultColumnSingleFormatCommandTests
    {
        private ImportData testData;
        private DataRow testRow;
        private List<string> possibleValues;
        
        private IManagedDataSource stubedDataSource;

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
            possibleValues = new List<string>();
            possibleValues.Add("TestPossibleValue");

            stubedDataSource = MockRepository.GenerateMock<IManagedDataSource>();
            stubedDataSource.Stub(gen => gen.GetPossibleValues(
                    Arg<string>.Is.Equal("TestValueSource"),
                    Arg<ImportData>.Is.Anything))
                .Return(possibleValues);            

            TestObject = new DefaultColumnSingleFormatCommand(stubedDataSource);
            TestObject.Parameters.Add("ColumnName","TestColumn");
            TestObject.Parameters.Add("ValueSource", "TestValueSource");
            TestObject.CreatedBy = "tester";
            TestObject.ModifiedBy = "tester";
            TestObject.EntityStatus = Afc.Core.Domain.EntityStatusEnum.Active;
            TestObject.MetaData = MockRepository.GenerateMock<ImportMetaData>();

        }

        [Test]
        public void DefaultColumnSingleFormatCommand_ConstructorTest()
        {
            var obj = new DefaultColumnSingleFormatCommand(stubedDataSource);
            Assert.NotNull(obj);

            Assert.AreNotEqual(0, obj.EnsureIsWellFormed.Count);

            Assert.AreEqual(0, TestObject.EnsureIsWellFormed.Count);

        }

        [Test]
        public void DefaultColumnSingleFormatCommand_Constructor_NullDependency()
        {
            
            Assert.Throws<ArgumentNullException>(
                ()=>{new DefaultColumnSingleFormatCommand(null);},
                "Did not recieve expected Null Argument exception on a null dependency."
                );

        }

        [Test]
        public void DefaultColumnSingleFormatCommand_Check_CommandType()
        {
            Assert.AreEqual(ImportFormatCommandTypes.DefaultColumnIfSingleChoice, TestObject.CommandType);
        }

        [Test]
        public void DefaultColumnSingleFormatCommand_Check_RequiredParameters()
        {
            Assert.IsNotNull(TestObject.RequiredParameters);
            Assert.AreEqual(2, TestObject.RequiredParameters.Count);
            Assert.IsTrue(TestObject.RequiredParameters.Contains("ColumnName"));
            Assert.IsTrue(TestObject.RequiredParameters.Contains("ValueSource"));

        }

        [Test]
        public void DefaultColumnSingleFormatCommand_Check_OptionalParameters()
        {
            Assert.IsNotNull(TestObject.OptionalParameters);
            Assert.AreEqual(0, TestObject.OptionalParameters.Count);
        }

        [Test]
        public void DefaultColumnSingleFormatCommand_ApplyTo_HappyPath()
        {

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsTrue(testRow.IsNull("TestColumn"));

            Assert.IsTrue(TestObject.ApplyTo(testData));

            Assert.IsFalse(testRow.IsNull("TestColumn"));

            Assert.AreEqual("TestPossibleValue", testRow["TestColumn"]);
            
        }
        
        [Test]
        public void DefaultColumnSingleFormatCommand_ApplyTo_NonExistingColumn()
        {

            testData.Data.Columns.Remove("TestColumn");

            Assert.IsFalse(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsFalse(TestObject.ApplyTo(testData));

            Assert.IsFalse(testData.Data.Columns.Contains("TestColumn"));

        }

        [Test]
        public void DefaultColumnSingleFormatCommand_ApplyTo_NoChoices()
        {

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsTrue(testRow.IsNull("TestColumn"));

            possibleValues.RemoveRange(0, possibleValues.Count);

            Assert.IsFalse(TestObject.ApplyTo(testData));

            Assert.IsTrue(testRow.IsNull("TestColumn"));
            
        }

        [Test]
        public void DefaultColumnSingleFormatCommand_ApplyTo_MultipleChoices()
        {

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsTrue(testRow.IsNull("TestColumn"));

            possibleValues.Add("AnotherChoice");

            Assert.IsFalse(TestObject.ApplyTo(testData));

            Assert.IsTrue(testRow.IsNull("TestColumn"));

            possibleValues.Add("ThirdChoice");

            Assert.IsFalse(TestObject.ApplyTo(testData));

            Assert.IsTrue(testRow.IsNull("TestColumn"));

        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("      ")]
        [TestCase(" ")]
        public void DefaultColumnSingleFormatCommand_ApplyTo_BadParamValueSource(string value)
        {

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsTrue(testRow.IsNull("TestColumn"));

            TestObject.Parameters["ValueSource"] = value;

            Assert.IsFalse(TestObject.ApplyTo(testData));

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsTrue(testRow.IsNull("TestColumn"));
        
        }
                 
        [Test]
        public void DefaultColumnSingleFormatCommand_ApplyTo_MissingParamValueSource()
        {

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsTrue(testRow.IsNull("TestColumn"));

            TestObject.Parameters.Remove("ValueSource");

            Assert.IsFalse(TestObject.ApplyTo(testData));

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsTrue(testRow.IsNull("TestColumn"));

        }
        
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("      ")]
        [TestCase(" ")]
        public void DefaultColumnSingleFormatCommand_ApplyTo_ValueAlreadySet_ButBlank(string value)
        {

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsTrue(testRow.IsNull("TestColumn"));

            testRow["TestColumn"] = value;

            Assert.IsFalse(testRow.IsNull("TestColumn"));

            Assert.IsTrue(TestObject.ApplyTo(testData));

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsFalse(testRow.IsNull("TestColumn"));

            Assert.AreEqual("TestPossibleValue", testRow["TestColumn"]);

        }

        [TestCase("this that the other")]
        [TestCase("asdf")]
        [TestCase(" asdf ")]
        [TestCase("null")]
        [TestCase("NULL")]
        [TestCase("blank")]
        [TestCase("empty")]
        [TestCase("string.empty")]
        public void DefaultColumnSingleFormatCommand_ApplyTo_ValueAlreadySet(string value)
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

        [Test]
        public void DefaultColumnSingleFormatCommand_ApplyTo_UnExpectedException()
        {

            stubedDataSource.BackToRecord(BackToRecordOptions.All);
            stubedDataSource.Replay();
            stubedDataSource.Stub(gen => gen.GetPossibleValues(
                    Arg<string>.Is.Anything,
                    Arg<ImportData>.Is.Anything))
                .Throw(new Exception("unexpected Exception"));

            Assert.IsFalse(TestObject.ApplyTo(testData));

            Assert.IsTrue(testRow.IsNull("TestColumn"));

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
        public void DefaultColumnSingleFormatCommand_ApplyTo_HappyPath_MultiRow(int rowCount)
        {

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsTrue(testRow.IsNull("TestColumn"));

            List<DataRow> builtRows = BuildRows(rowCount);

            Assert.IsTrue(TestObject.ApplyTo(testData));

            foreach (DataRow row in builtRows)
            {
                Assert.AreEqual("TestPossibleValue", row["TestColumn"]);
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
        public void DefaultColumnSingleFormatCommand_ApplyTo_ExistingValue(int rowCount)
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
        public void DefaultColumnSingleFormatCommand_ApplyTo_SomeExistingValues(int rowCount)
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

            Assert.AreEqual("TestPossibleValue", testRow["TestColumn"]);

            foreach (DataRow row in builtRows)
            {
                Assert.AreEqual("TestValue", row["TestColumn"]);
            }

        }
    }
}
