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
    public class DecimalDataFormatterTests
    {

        private DecimalDataFormatter TestObject;

        /// <summary>
        /// Test setup that should run before every test to make sure that the Mocks are in a consistent state
        /// </summary>
        [SetUp]
        public void Init()
        {

            TestObject = new DecimalDataFormatter();
        }

        [Test]
        public void DecimalDataFormatter_ConstructorTest()
        {
            Assert.NotNull(new DecimalDataFormatter());
        }

        [TestCase("#")]
        [TestCase("asdf")]
        [TestCase("123..546")]
        [TestCase("1234i")]
        [TestCase("#2134")]
        public void DecimalDataFormatter_FormatData_BadValues(string data)
        {
            Assert.AreEqual(data, TestObject.FormatData(data));
        }
        
        [TestCase("2.00", "2.00")]
        [TestCase("2.47", "2.465338050")]
        public void DecimalDataFormatter_GenerateValue_Formated(string ExpectedResult, string data)
        {

            Assert.AreEqual(ExpectedResult, TestObject.FormatData(data));

        }

        [TestCase("2.0", "2.00", "0.0")]
        [TestCase("2.47", "2.465338050", "0.00")]
        [TestCase("2.47%", ".02465338050", "0.00%")]
        [TestCase("2.4%", ".024", "0.0%")]
        [TestCase("2.4a", "2.4", "0.0a")]
        [TestCase("2.4653", "2.465338050", "0.0000")]
        public void DecimalDataFormatter_GenerateValue_Formated(string ExpectedResult, string data, string format)
        {

            Assert.AreEqual(ExpectedResult, TestObject.FormatData(data, format));

        }

        [TestCase("#")]
        [TestCase("asdf")]
        [TestCase("123..546")]
        [TestCase("1234i")]
        [TestCase("#2134")]
        public void DecimalDataFormatter_FormatData_BadValues_ProvidedFormat(string data)
        {
            Assert.AreEqual(data, TestObject.FormatData(data, "##.0"));
        }
    }
}
