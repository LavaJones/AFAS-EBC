using Afas.ImportConverter.Application;
using Afas.ImportConverter.Application.Implementation;
using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.ImportConverter.Tests.Application.Implementation
{
    [TestFixture]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class DataTableBuilderTests
    {
        private string[] RowValues;

        private string[] HeaderValues;

        private IHeaderValidator stubedValidator;

        private DataTableBuilder TestObject;

        /// <summary>
        /// Test setup that should run before every test to make sure that the Mocks are in a consistent state
        /// </summary>
        [SetUp]
        public void Init()
        {
            HeaderValues = new string[] { "test1", "test2", "other" };
            RowValues = new string[] { "value1", "value2", "otherValue" };

            stubedValidator = MockRepository.GenerateMock<IHeaderValidator>();
            stubedValidator.Stub(gen => gen.ValidateHeaders(Arg<string[]>.Is.Anything)).Return(true);
            
            TestObject = new DataTableBuilder(stubedValidator);

        }

        [Test]
        public void DataTableBuilder_ConstructorTest_NullDependency()
        {

            Assert.Throws<ArgumentNullException>(
                () => { new DataTableBuilder(null); },
                "Did not recieve expected Null Argument exception on a null dependency."
                );

        }

        [Test]
        public void DataTableBuilder_ConstructorTest_HappyPath()
        {

            Assert.IsNotNull(new DataTableBuilder(stubedValidator));

        }

        [Test]
        public void DataTableBuilder_ParseHeaders_HappyPath()
        {

            TestObject.ParseHeaders(HeaderValues);
            DataTable table = TestObject.Build();

            foreach (string header in HeaderValues)
            {
                Assert.IsTrue(table.Columns.Contains(header));
            }

        }

        [Test]
        public void DataTableBuilder_ParseHeaders_ValidatorFalse()
        {
            stubedValidator.BackToRecord(BackToRecordOptions.All);
            stubedValidator.Replay();
            stubedValidator.Stub(gen => gen.ValidateHeaders(Arg<string[]>.Is.Anything)).Return(false);

            TestObject.ParseHeaders(HeaderValues);
            DataTable table = TestObject.Build();

            for (int i = 0; i < HeaderValues.Count(); i++)
            {
                Assert.IsFalse(table.Columns.Contains(HeaderValues[i]));
                Assert.IsTrue(table.Columns.Contains(i.ToString()));
            }

        }

        [Test]
        public void DataTableBuilder_AddRowToTable_HappyPath()
        {

            TestObject.ParseHeaders(HeaderValues);
            TestObject.AddRowToTable(RowValues);
            DataTable table = TestObject.Build();

            Assert.AreEqual(1, table.Rows.Count);
            DataRow first = table.Rows[0];

            for(int i =0; i < HeaderValues.Count(); i++)
            {
                Assert.IsTrue(table.Columns.Contains(HeaderValues[i]));
                Assert.AreEqual(RowValues[i], first[HeaderValues[i]]);
            }

        }

        [Test]
        public void DataTableBuilder_ClearBuilder_HappyPath()
        {

            TestObject.ParseHeaders(HeaderValues);
            TestObject.AddRowToTable(RowValues);

            TestObject.ClearBuilder();

            DataTable table = TestObject.Build();

            Assert.AreEqual(0, table.Columns.Count);
            Assert.AreEqual(0, table.Rows.Count);

        }

        [Test]
        public void DataTableBuilder_ResetBuilder_HappyPath()
        {

            TestObject.ParseHeaders(HeaderValues);
            TestObject.AddRowToTable(RowValues);

            TestObject.ResetBuilder("SomeName");

            DataTable table = TestObject.Build();

            Assert.AreEqual("SomeName", table.TableName);
            Assert.AreEqual(0, table.Columns.Count);
            Assert.AreEqual(0, table.Rows.Count);

        }
    }
}
