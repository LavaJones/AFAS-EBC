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
    public class FormatColumnValuesFormatCommandTests
    {
        private ImportData testData;
        private DataRow testRow;

        private IManagedDataFormatter formatter;

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
            testRow["TestColumn"] = "TestData";
            testData.Data.Rows.Add(testRow);

            formatter = MockRepository.GenerateMock<IManagedDataFormatter>();
            formatter.Stub(form => form.FormatData(
                    Arg<string>.Is.Equal("TestData"),
                    Arg<string>.Is.Equal("Type"), 
                    Arg<string>.Is.Equal("Format")))
                .Return("FormattedTestData");

            TestObject = new FormatColumnValuesFormatCommand(formatter);
            TestObject.Parameters.Add("ColumnName","TestColumn");
            TestObject.Parameters.Add("FormatterType", "Type");
            TestObject.Parameters.Add("Format", "Format");
            TestObject.CreatedBy = "tester";
            TestObject.ModifiedBy = "tester";
            TestObject.EntityStatus = Afc.Core.Domain.EntityStatusEnum.Active;
            TestObject.MetaData = MockRepository.GenerateMock<ImportMetaData>();

        }

        [Test]
        public void FormatColumnValuesFormatCommand_ConstructorTest_HappyPath()
        {
            var obj = new FormatColumnValuesFormatCommand(formatter);
            Assert.NotNull(obj);

            Assert.AreNotEqual(0, obj.EnsureIsWellFormed.Count);

            Assert.AreEqual(0, TestObject.EnsureIsWellFormed.Count);

        }

        [Test]
        public void FormatColumnValuesFormatCommand_ConstructorTest_NullArg()
        {

            Assert.Throws<ArgumentNullException>(
                () => { new FormatColumnValuesFormatCommand(null); },
                "Did not recieve expected Null Argument exception on a null dependency."
                );
            
        }

        [Test]
        public void FormatColumnValuesFormatCommand_GetterTest()
        {

            Assert.AreEqual(ImportFormatCommandTypes.FormatColumn, TestObject.CommandType);
            Assert.IsNotNull(TestObject.RequiredParameters);
            Assert.AreEqual(3, TestObject.RequiredParameters.Count);
            Assert.IsTrue(TestObject.RequiredParameters.Contains("ColumnName"));
            Assert.IsTrue(TestObject.RequiredParameters.Contains("FormatterType"));
            Assert.IsTrue(TestObject.RequiredParameters.Contains("Format"));
            Assert.IsNotNull(TestObject.OptionalParameters);
            Assert.AreEqual(0, TestObject.OptionalParameters.Count);

        }        

        [Test]
        public void FormatColumnValuesFormatCommand_ApplyTo_HappyPath()
        {

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsTrue(TestObject.ApplyTo(testData));

            Assert.IsFalse(testRow.IsNull("TestColumn"));

            Assert.AreEqual("FormattedTestData", testRow["TestColumn"]);
            
        }
        
        [Test]
        public void FormatColumnValuesFormatCommand_ApplyTo_NonExistingColumn()
        {

            TestObject.Parameters["ColumnName"] = "OtherColumn";

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsFalse(TestObject.ApplyTo(testData));

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

            Assert.AreEqual("TestData", testRow["TestColumn"]);

        }        
    }
}
