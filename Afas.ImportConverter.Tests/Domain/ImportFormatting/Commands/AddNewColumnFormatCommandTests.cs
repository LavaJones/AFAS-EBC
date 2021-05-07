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
    public class AddNewColumnFormatCommandTests
    {

        private ImportData testData;

        private AImportFormatCommand TestObject;

        /// <summary>
        /// Test setup that should run before every test to make sure that the Mocks are in a consistent state
        /// </summary>
        [SetUp]
        public void Init()
        {
            testData = new ImportData();
            testData.Data = new DataTable();

            TestObject = new AddNewColumnFormatCommand();
            TestObject.Parameters.Add("ColumnName","TestColumn");
            TestObject.CreatedBy = "tester";
            TestObject.ModifiedBy = "tester";
            TestObject.EntityStatus = Afc.Core.Domain.EntityStatusEnum.Active;
            TestObject.MetaData = MockRepository.GenerateMock<ImportMetaData>();

        }

        [Test]
        public void AddNewColumnFormatCommand_ConstructorTest()
        {
            var obj = new AddNewColumnFormatCommand();
            Assert.NotNull(obj);

            Assert.AreNotEqual(0, obj.EnsureIsWellFormed.Count);

            Assert.AreEqual(0, TestObject.EnsureIsWellFormed.Count);

        }

        [Test]
        public void AddNewColumnFormatCommand_GetterTest()
        {
            Assert.AreEqual(ImportFormatCommandTypes.AddNewColumn, TestObject.CommandType);
            Assert.IsNotNull(TestObject.RequiredParameters);
            Assert.IsNotNull(TestObject.OptionalParameters);
            Assert.AreEqual(1, TestObject.RequiredParameters.Count);
            Assert.IsTrue(TestObject.RequiredParameters.Contains("ColumnName"));
            Assert.AreEqual(0, TestObject.OptionalParameters.Count);

        }

        [Test]
        public void AddNewColumnFormatCommand_ApplyTo_HappyPath()
        {
            
            Assert.IsFalse(testData.Data.Columns.Contains("TestColumn"));

            Assert.IsTrue(TestObject.ApplyTo(testData));

            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

        }

        [Test]
        public void AddNewColumnFormatCommand_ApplyTo_ExistingColumn()
        {

            testData.Data.Columns.Add("TestColumn", typeof(string));


            Assert.IsTrue(TestObject.ApplyTo(testData));
            
            Assert.IsTrue(testData.Data.Columns.Contains("TestColumn"));

        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("      ")]
        [TestCase(" ")]
        public void AddNewColumnFormatCommand_ApplyTo_BadParamColumn(string value)
        {

            TestObject.Parameters["ColumnName"] = value;
            Assert.IsFalse(TestObject.ApplyTo(testData));
            Assert.AreEqual(0, testData.Data.Columns.Count);
        
        }
                 
        [Test]
        public void AddNewColumnFormatCommand_ApplyTo_MissingParamColumn()
        {

            TestObject.Parameters.Remove("ColumnName");
            Assert.IsFalse(TestObject.ApplyTo(testData));
            Assert.AreEqual(0, testData.Data.Columns.Count);

        }
    }
}
