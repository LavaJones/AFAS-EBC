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
    public class RandomNumberValueGeneratorTests
    {

        private RandomNumberValueGenerator TestObject;

        /// <summary>
        /// Test setup that should run before every test to make sure that the Mocks are in a consistent state
        /// </summary>
        [SetUp]
        public void Init()
        {
            Random rand = new Random(1);

            TestObject = new RandomNumberValueGenerator(rand);
        }

        [Test]
        public void RandomNumberValueGenerator_ConstructorTest()
        {
            Assert.NotNull(new RandomNumberValueGenerator(new Random()));
        }

        [Test]
        public void RandomNumberValueGenerator_ConstructorTest_NullIsOkay()
        {
            var testThis = new RandomNumberValueGenerator(null);
                       
            Assert.NotNull(testThis); 

            Assert.IsNotNull(testThis.GenerateValue());
            Assert.IsFalse(string.IsNullOrEmpty(testThis.GenerateValue()));

        }

        [Test]
        public void RandomNumberValueGenerator_GenerateValue()
        {
            Assert.AreEqual("534011718", TestObject.GenerateValue());
        }

        [TestCase("2", "#")] 
        [TestCase("20-465338050", "##-#########")]
        [TestCase("!@$%2^&*()", "!@$%#^&*()")]
        [TestCase("Nothing", "Nothing")]
        [TestCase("2 0 4", "# # #")]
        [TestCase("", "")]
        public void RandomNumberValueGenerator_GenerateValue_Formated(string ExpectedResult, string Format)
        {

            Assert.AreEqual(ExpectedResult, TestObject.GenerateValue(Format));

        }
    }
}
