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
    public class UniqueNumberValueGeneratorTests
    {

        private UniqueNumberValueGenerator TestObject;

        /// <summary>
        /// Test setup that should run before every test to make sure that the Mocks are in a consistent state
        /// </summary>
        [SetUp]
        public void Init()
        {
            TestObject = new UniqueNumberValueGenerator();
        }

        [Test]
        public void UniqueNumberValueGenerator_ConstructorTest()
        {
            Assert.NotNull(new UniqueDateValueGenerator());
        }

        [Test]
        public void UniqueNumberValueGenerator_GenerateValue()
        {

            long number = 0;
            string value = TestObject.GenerateValue();
            Assert.IsTrue(long.TryParse(value, out number));
            Assert.AreNotEqual(0, number);

        }

        [TestCase("[UNIQU]", "[UNIQU]")]
        [TestCase("[UNIQUE", "[UNIQUE")]
        [TestCase("[UNIQUE ]", "[UNIQUE ]")]
        [TestCase("UNIQUE", "UNIQUE")]
        public void UniqueNumberValueGenerator_GenerateValue_WrongFormated(string ExpectedResult, string Format)
        {

            string value = TestObject.GenerateValue(Format);
            Assert.AreEqual(ExpectedResult, value);

        }

        [Test]
        public void UniqueNumberValueGenerator_GenerateValue_Formated()
        {

            long number = 0;
            string value = TestObject.GenerateValue("[UNIQUE]");
            Assert.IsTrue(long.TryParse(value, out number));
            Assert.AreNotEqual(0, number);

        }
    }
}
