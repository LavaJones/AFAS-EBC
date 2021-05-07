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
    public class DateDataFormatterTests
    {

        private DateDataFormatter TestObject;

        /// <summary>
        /// Test setup that should run before every test to make sure that the Mocks are in a consistent state
        /// </summary>
        [SetUp]
        public void Init()
        {

            TestObject = new DateDataFormatter();

        }

        [Test]
        public void DateDataFormatter_ConstructorTest()
        {

            Assert.NotNull(new DateDataFormatter());

        }

        [TestCase("#")]
        [TestCase("asdf")]
        [TestCase("123..546")]
        [TestCase("1234i")]
        [TestCase("#2134")]
        public void DateDataFormatter_FormatData_BadValues(string data)
        {

            Assert.AreEqual(data, TestObject.FormatData(data));

        }

        [TestCase("2/5/2017", "2017/02/05")]
        [TestCase("2/5/2017", "2017-02-05")]
        [TestCase("2/5/2017", "20170205")]
        [TestCase("2/5/2017", "02/05/2017")]
        [TestCase("2/5/2017", "2017-02-05")]
        [TestCase("2/5/2017", "02/05/2017 12:34")]
        [TestCase("2/5/2017", "02/05/2017 12:34:56")]
        [TestCase("2/5/2017", "02/05/2017 12:34:56 am")]
        [TestCase("2/5/2017", "02/05/2017 2:34:56 am")]
        [TestCase("2/5/2017", "02/5/2017 12:34:56 am")]
        [TestCase("2/5/2017", "2/05/2017 12:34:56 am")]
        [TestCase("2/5/2017", "2/5/2017 12:34:56 am")]
        public void DateDataFormatter_GenerateValue_Formated(string ExpectedResult, string data)
        {

            Assert.AreEqual(ExpectedResult, TestObject.FormatData(data));

        }

        [TestCase("2017/02/05 12:00", "2017-02-05", "yyyy/MM/dd hh:mm")]
        [TestCase("2017-02-05 12", "2017-02-05", "yyyy-MM-dd hh")]
        [TestCase("20170205123456", "02/05/2017 12:34:56 am", "yyyyMMddhhmmss")]
        [TestCase("2/5/2017", "2017-02-05", "M/d/yyyy")]
        [TestCase("02/05/2017", "2017-02-05", "MM/dd/yyyy")]
        public void DateDataFormatter_GenerateValue_Formated(string ExpectedResult, string data, string format)
        {

            Assert.AreEqual(ExpectedResult, TestObject.FormatData(data, format));

        }


        [TestCase("#")]
        [TestCase("asdf")]
        [TestCase("123..546")]
        [TestCase("1234i")]
        [TestCase("#2134")]
        public void DateDataFormatter_FormatData_BadValues_ProvidedFormat(string data)
        {

            Assert.AreEqual(data, TestObject.FormatData(data,"##.0"));

        }
    }
}
