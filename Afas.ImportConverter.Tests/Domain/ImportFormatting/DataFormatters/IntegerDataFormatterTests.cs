using Afas.ImportConverter.Domain.ImportFormatting.DataFormatters.Implementation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.ImportConverter.Tests.Domain.ImportFormatting.DataFormatters
{
    [TestFixture]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class IntegerDataFormatterTests
    {

        private IntegerDataFormatter TestObject;

        /// <summary>
        /// Test setup that should run before every test to make sure that the Mocks are in a consistent state
        /// </summary>
        [SetUp]
        public void Init()
        {

            TestObject = new IntegerDataFormatter();
        }

        [Test]
        public void IntegerDataFormatter_ConstructorTest()
        {
            Assert.NotNull(new IntegerDataFormatter());
        }

        [TestCase("#")]
        [TestCase("asdf")]
        [TestCase("123..546")]
        [TestCase("1234i")]
        [TestCase("#2134")]
        public void IntegerDataFormatter_FormatData_BadValues(string data)
        {
            Assert.AreEqual(data, TestObject.FormatData(data));
        }

        [TestCase("2", "2")]
        [TestCase("2", "2.00")]
        [TestCase("2", "2.465338050")]
        [TestCase("200", "200")]
        [TestCase("2", "2.9999999")]
        public void IntegerDataFormatter_GenerateValue_Formated(string ExpectedResult, string data)
        {

            Assert.AreEqual(ExpectedResult, TestObject.FormatData(data));

        }

        [TestCase("2.0", "2.00", "0.0")]
        [TestCase("2.00", "2.465338050", "0.00")]
        [TestCase("0.00%", ".02465338050", "0.00%")]
        [TestCase("0.0%", ".024", "0.0%")]
        [TestCase("2.0a", "2.4", "0.0a")]
        [TestCase("2.0000", "2.465338050", "0.0000")]
        public void IntegerDataFormatter_GenerateValue_Formated(string ExpectedResult, string data, string format)
        {

            Assert.AreEqual(ExpectedResult, TestObject.FormatData(data, format));

        }

    }
}
