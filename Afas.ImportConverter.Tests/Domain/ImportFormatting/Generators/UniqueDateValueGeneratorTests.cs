using Afas.ImportConverter.Domain.ImportFormatting.Generators.Implementation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.ImportConverter.Tests.Domain.ImportFormatting.Generators
{
    [TestFixture]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class UniqueDateValueGeneratorTests
    {

        private UniqueDateValueGenerator TestObject;

        /// <summary>
        /// Test setup that should run before every test to make sure that the Mocks are in a consistent state
        /// </summary>
        [SetUp]
        public void Init()
        {
            TestObject = new UniqueDateValueGenerator();
        }

        [Test]
        public void UniqueDateValueGenerator_ConstructorTest()
        {
            Assert.NotNull(new UniqueDateValueGenerator());
        }

        [Test]
        public void UniqueDateValueGenerator_GenerateValue()
        {

            DateTime time = new DateTime();
            string value = TestObject.GenerateValue().Replace('_', ' '); 
            Assert.IsTrue(DateTime.TryParse(value, out time));
            Assert.AreNotEqual(new DateTime(), time);

        }

        [TestCase("[DAT]", "[DAT]")]
        [TestCase("[DATE", "[DATE")]
        [TestCase("[DATE ]", "[DATE ]")]
        [TestCase("DATE", "DATE")]
        public void UniqueDateValueGenerator_GenerateValue_WrongFormated(string ExpectedResult, string Format)
        {

            string value = TestObject.GenerateValue(Format);
            Assert.AreEqual(ExpectedResult, value);

        }

        [Test]
        public void UniqueDateValueGenerator_GenerateValue_Formated()
        {

            DateTime time = new DateTime();
            string value = TestObject.GenerateValue("[DATE]").Replace('_', ' '); 
            Assert.IsTrue(DateTime.TryParse(value, out time));
            Assert.AreNotEqual(new DateTime(), time);

        }
    }
}
