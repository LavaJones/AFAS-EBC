using Afas.Application;
using Afas.ImportConverter.Domain.ImportFormatting.DataFormatters;
using Afas.ImportConverter.Domain.ImportFormatting.DataFormatters.Implementation;
using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.ImportConverter.Tests.Domain.ImportFormatting.DataFormatters
{
    [TestFixture]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ManagedDataFormatterTests
    {

        private ManagedDataFormatter TestObject;

        private IDefaultFactory<IDataFormatter> factory;

        private IDataFormatter dateFormatter;
        private IDataFormatter intFormatter;
        private IDataFormatter decFormatter;
        private IDataFormatter boolFormatter;

        /// <summary>
        /// Test setup that should run before every test to make sure that the Mocks are in a consistent state
        /// </summary>
        [SetUp]
        public void Init()
        {

            dateFormatter = MockRepository.GenerateMock<IDataFormatter>();
            dateFormatter.Stub(genFac => genFac.FormatData(Arg<string>.Is.Anything)).Return("Formatted Value");
            dateFormatter.Stub(genFac => genFac.FormatData(Arg<string>.Is.Anything, Arg<string>.Is.Anything)).Return("Formatted Value");

            decFormatter = MockRepository.GenerateMock<IDataFormatter>();
            decFormatter.Stub(genFac => genFac.FormatData(Arg<string>.Is.Anything)).Return("Formatted Value");
            decFormatter.Stub(genFac => genFac.FormatData(Arg<string>.Is.Anything, Arg<string>.Is.Anything)).Return("Formatted Value");

            intFormatter = MockRepository.GenerateMock<IDataFormatter>();
            intFormatter.Stub(genFac => genFac.FormatData(Arg<string>.Is.Anything)).Return("Formatted Value");
            intFormatter.Stub(genFac => genFac.FormatData(Arg<string>.Is.Anything, Arg<string>.Is.Anything)).Return("Formatted Value");

            boolFormatter = MockRepository.GenerateMock<IDataFormatter>();
            boolFormatter.Stub(genFac => genFac.FormatData(Arg<string>.Is.Anything)).Return("Formatted Value");
            boolFormatter.Stub(genFac => genFac.FormatData(Arg<string>.Is.Anything, Arg<string>.Is.Anything)).Return("Formatted Value");

            factory = MockRepository.GenerateDynamicMockWithRemoting<IDefaultFactory<IDataFormatter>>();
            factory.Stub(genFac => genFac.GetById(Arg<string>.Is.Equal("DateFormatter"))).Return(dateFormatter);
            factory.Stub(genFac => genFac.GetById(Arg<string>.Is.Equal("IntegerFormatter"))).Return(intFormatter);
            factory.Stub(genFac => genFac.GetById(Arg<string>.Is.Equal("DecimalFormatter"))).Return(decFormatter);
            factory.Stub(genFac => genFac.GetById(Arg<string>.Is.Equal("BooleanFormatter"))).Return(boolFormatter);


            TestObject = new ManagedDataFormatter(factory);

        }

        [Test]
        public void ManagedDataFormatter_ConstructorTest()
        {

            Assert.NotNull(new ManagedDataFormatter(factory));

        }

        [Test]
        public void ManagedDataFormatter_ConstructorTest_NullArg()
        {

            Assert.Throws<ArgumentNullException>(
                () => { new ManagedDataFormatter(null); },
                "Did not recieve expected Null Argument exception on a null dependency."
                );

        }

        [TestCase("DateFormatter123")]
        [TestCase("asDateFormatterdf")]
        [TestCase("asdf")]
        [TestCase("")]
        public void ManagedDataFormatter_FormatData_BadTypes(string type)
        {

            string data = "this is some data 12345.678";

            Assert.Throws<ArgumentException>(
                () => { TestObject.FormatData(data, type); },
                "Did not recieve expected Argument exception on a bad formatter."
                );

        }

        [Test]
        public void ManagedDataFormatter_FormatData_NullData()
        {

            Assert.AreEqual(null, TestObject.FormatData(null, "DateFormatter"));

        }

        [TestCase("DateFormatter")]
        [TestCase("DecimalFormatter")]
        [TestCase("IntegerFormatter")]
        [TestCase("BooleanFormatter")]
        public void ManagedDataFormatter_FormatData_HappyPath(string type)
        {

            string data = "this is some data 12345.678";
            Assert.AreEqual("Formatted Value", TestObject.FormatData(data, type));

        }

        [TestCase("DateFormatter")]
        [TestCase("DecimalFormatter")]
        [TestCase("IntegerFormatter")]
        [TestCase("BooleanFormatter")]
        public void ManagedDataFormatter_FormatData_FormatterNotFound(string type)
        {
            factory.BackToRecord(BackToRecordOptions.All);
            factory.Replay();
            factory.Stub(genFac => genFac.GetById(Arg<string>.Is.Equal("DateFormatter"))).Return(null);
            factory.Stub(genFac => genFac.GetById(Arg<string>.Is.Equal("IntegerFormatter"))).Return(null);
            factory.Stub(genFac => genFac.GetById(Arg<string>.Is.Equal("DecimalFormatter"))).Return(null);
            factory.Stub(genFac => genFac.GetById(Arg<string>.Is.Equal("BooleanFormatter"))).Return(null);

            string data = "this is some data 12345.678";

            Assert.AreEqual(data, TestObject.FormatData(data, type));

        }

        [TestCase("DateFormatter123")]
        [TestCase("asDateFormatterdf")]
        [TestCase("asdf")]
        [TestCase("")]
        public void ManagedDataFormatter_FormatData_BadTypes_Format(string type)
        {

            string data = "this is some data 12345.678";

            Assert.Throws<ArgumentException>(
                () => { TestObject.FormatData(data, type, "someFormat"); },
                "Did not recieve expected Argument exception on a bad formatter."
                );

        }

        [TestCase("DateFormatter")]
        [TestCase("DecimalFormatter")]
        [TestCase("IntegerFormatter")]
        [TestCase("BooleanFormatter")]
        public void ManagedDataFormatter_FormatData_HappyPath_Format(string type)
        {

            string data = "this is some data 12345.678";
            Assert.AreEqual("Formatted Value", TestObject.FormatData(data, type, "someFormat"));

        }
    }
}
