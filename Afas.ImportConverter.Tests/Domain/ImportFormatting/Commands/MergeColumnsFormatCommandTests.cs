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
    public class MergeColumnsFormatCommandTests
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
            testData.Data.Columns.Add("EmptyColumn", typeof(string));
            testRow = testData.Data.NewRow();
            testRow["TestColumn"] = "MyValue";
            testRow["OtherColumn"] = "OtherValue";
            testRow["EmptyColumn"] = DBNull.Value;
            testData.Data.Rows.Add(testRow);

            TestObject = new MergeColumnsFormatCommand();
            TestObject.Parameters.Add("ColumnName", "EmptyColumn");
            TestObject.Parameters.Add("MergePattern", "Test{TestColumn}");            
            TestObject.CreatedBy = "tester";
            TestObject.ModifiedBy = "tester";
            TestObject.EntityStatus = Afc.Core.Domain.EntityStatusEnum.Active;
            TestObject.MetaData = MockRepository.GenerateMock<ImportMetaData>();

        }

        [Test]
        public void MergeColumnsFormatCommand_ConstructorTest()
        {
            var obj = new MergeColumnsFormatCommand();
            Assert.NotNull(obj);

            Assert.AreNotEqual(0, obj.EnsureIsWellFormed.Count);

            Assert.AreEqual(0, TestObject.EnsureIsWellFormed.Count);

        }

        [Test]
        public void MergeColumnsFormatCommand_GetterTest()
        {
            Assert.AreEqual(ImportFormatCommandTypes.MergeColumns, TestObject.CommandType);
            Assert.IsNotNull(TestObject.RequiredParameters);
            Assert.IsNotNull(TestObject.OptionalParameters);
            Assert.AreEqual(2, TestObject.RequiredParameters.Count);
            Assert.IsTrue(TestObject.RequiredParameters.Contains("ColumnName"));
            Assert.IsTrue(TestObject.RequiredParameters.Contains("MergePattern"));
            Assert.AreEqual(0, TestObject.OptionalParameters.Count);

        }

        [TestCase("TestMyValue", "Test{TestColumn}")]
        [TestCase("TestOtherValue", "Test{OtherColumn}")]
        [TestCase("Test", "Test{EmptyColumn}")]
        [TestCase("MyValue", "{Test{EmptyColumn}Column}")]
        [TestCase("MyValue-OtherValue", "{TestColumn}-{OtherColumn}")]
        [TestCase("test", "test")]
        [TestCase("MyValue", "{TestColumn}")]
        public void MergeColumnsFormatCommand_ApplyTo_HappyPath(string ExpectedResult, string pattern)
        {

            TestObject.Parameters["MergePattern"] = pattern;

            Assert.IsTrue(testData.Data.Columns.Contains("EmptyColumn"));

            Assert.IsTrue(TestObject.ApplyTo(testData));

            Assert.AreEqual(ExpectedResult, testRow["EmptyColumn"]);

        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("      ")]
        [TestCase(" ")]
        [TestCase("RandomColumn")]
        public void MergeColumnsFormatCommand_ApplyTo_BadParamColumn(string value)
        {

            TestObject.Parameters["ColumnName"] = value;

            Assert.IsFalse(TestObject.ApplyTo(testData));

            Assert.AreEqual(DBNull.Value, testRow["EmptyColumn"]);

        }
        
        [Test]
        public void MergeColumnsFormatCommand_ApplyTo_MissingParam_Column()
        {

            TestObject.Parameters.Remove("ColumnName");

            Assert.IsFalse(TestObject.ApplyTo(testData));

            Assert.AreEqual(DBNull.Value, testRow["EmptyColumn"]);

        }

        [Test]
        public void MergeColumnsFormatCommand_ApplyTo_MissingParam_merge()
        {

            TestObject.Parameters.Remove("MergePattern");

            Assert.IsFalse(TestObject.ApplyTo(testData));

            Assert.AreEqual(DBNull.Value, testRow["EmptyColumn"]);

        }

        [TestCase("Test{RandomColumn}", "Test{RandomColumn}")]
        [TestCase("Test{}", "Test{}")]
        [TestCase("Test{EmptyColumn", "Test{EmptyColumn")]
        [TestCase("Test}{EmptyColumn", "Test}{EmptyColumn")]
        [TestCase("Test{", "Test{{EmptyColumn}")]
        [TestCase("Test{MyValue", "Test{{TestColumn}")]
        [TestCase("Test{Test{}Column}", "Test{Test{}Column}")]
        public void MergeColumnsFormatCommand_ApplyTo_BadMergePattern(string ExpectedResult, string pattern)
        {

            TestObject.Parameters["MergePattern"] = pattern;

            Assert.IsTrue(testData.Data.Columns.Contains("EmptyColumn"));

            Assert.IsTrue(TestObject.ApplyTo(testData));

            Assert.AreEqual(ExpectedResult, testRow["EmptyColumn"]);

        }
    }
}
