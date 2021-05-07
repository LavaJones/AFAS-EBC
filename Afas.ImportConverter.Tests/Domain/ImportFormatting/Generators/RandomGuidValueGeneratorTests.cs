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
    public class RandomGuidValueGeneratorTests
    {

        private RandomGuidValueGenerator TestObject;

        /// <summary>
        /// Test setup that should run before every test to make sure that the Mocks are in a consistent state
        /// </summary>
        [SetUp]
        public void Init()
        {
            TestObject = new RandomGuidValueGenerator();
        }

        [Test]
        public void RandomGuidValueGenerator_ConstructorTest()
        {
            Assert.NotNull(new RandomGuidValueGenerator());
        }

        [Test]
        public void RandomGuidValueGenerator_GenerateValue()
        {

            Guid guid = new Guid();
            string value = TestObject.GenerateValue();
            Assert.IsTrue(Guid.TryParse(value, out guid));
            Assert.AreNotEqual(new Guid(), guid);

        }
        
        [TestCase("[GID]", "[GID]")]
        [TestCase("[GUID", "[GUID")]
        [TestCase("[GUID ]", "[GUID ]")]
        [TestCase("GUID", "GUID")]
        public void RandomGuidValueGenerator_GenerateValue_WrongFormated(string ExpectedResult, string Format)
        {

            string value = TestObject.GenerateValue(Format);
            Assert.AreEqual(ExpectedResult, value);

        }

        [Test]
        public void RandomGuidValueGenerator_GenerateValue_Formated()
        {

            Guid guid = new Guid();
            string value = TestObject.GenerateValue("[GUID]");
            Assert.IsTrue(Guid.TryParse(value, out guid));
            Assert.AreNotEqual(new Guid(), guid);

        }
    }
}
