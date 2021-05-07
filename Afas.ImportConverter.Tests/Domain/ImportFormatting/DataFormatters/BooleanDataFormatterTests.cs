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
    public class BooleanDataFormatterTests
    {

        private BooleanDataFormatter TestObject;

        /// <summary>
        /// Test setup that should run before every test to make sure that the Mocks are in a consistent state
        /// </summary>
        [SetUp]
        public void Init()
        {

            TestObject = new BooleanDataFormatter();

        }

        [Test]
        public void BooleanDataFormatter_ConstructorTest()
        {

            Assert.NotNull(new BooleanDataFormatter());

        }

        [TestCase("")]
        [TestCase("#")]
        [TestCase("asdf")]
        [TestCase("123..546")]
        [TestCase("1234i")]
        [TestCase("#2134")]
        public void BooleanDataFormatter_FormatData_BadValues(string data)
        {

            Assert.AreEqual(data, TestObject.FormatData(data));

        }
        
        [TestCase("True", "1")]
        [TestCase("True", "x")]
        [TestCase("True", "X")]
        [TestCase("True", "t")]
        [TestCase("True", "T")]
        [TestCase("True", "y")]
        [TestCase("True", "Y")]
        [TestCase("True", "yes")]
        [TestCase("True", "Yes")]
        [TestCase("True", "YES")]
        [TestCase("True", "true")]
        [TestCase("True", "True")]
        [TestCase("True", "TRUE")]
        [TestCase("False", "0")]
        [TestCase("False", "n")]
        [TestCase("False", "N")]
        [TestCase("False", "o")]
        [TestCase("False", "O")]
        [TestCase("False", "f")]
        [TestCase("False", "F")]
        [TestCase("False", "no")]
        [TestCase("False", "No")]
        [TestCase("False", "NO")]
        [TestCase("False", "false")]
        [TestCase("False", "False")]
        [TestCase("False", "FALSE")]
        public void BooleanDataFormatter_GenerateValue_Formated(string ExpectedResult, string data)
        {

            Assert.AreEqual(ExpectedResult, TestObject.FormatData(data));

        }

        [TestCase("Jafa", "1", "Jafa/Cree")]
        [TestCase("Jafa", "x", "Jafa/Cree")]
        [TestCase("Jafa", "X", "Jafa/Cree")]
        [TestCase("Jafa", "t", "Jafa/Cree")]
        [TestCase("Jafa", "T", "Jafa/Cree")]
        [TestCase("Jafa", "y", "Jafa/Cree")]
        [TestCase("Jafa", "Y", "Jafa/Cree")]
        [TestCase("Jafa", "yes", "Jafa/Cree")]
        [TestCase("Jafa", "Yes", "Jafa/Cree")]
        [TestCase("Jafa", "YES", "Jafa/Cree")]
        [TestCase("Jafa", "true", "Jafa/Cree")]
        [TestCase("Jafa", "True", "Jafa/Cree")]
        [TestCase("Jafa", "TRUE", "Jafa/Cree")]
        [TestCase("Cree", "0", "Jafa/Cree")]
        [TestCase("Cree", "n", "Jafa/Cree")]
        [TestCase("Cree", "N", "Jafa/Cree")]
        [TestCase("Cree", "o", "Jafa/Cree")]
        [TestCase("Cree", "O", "Jafa/Cree")]
        [TestCase("Cree", "f", "Jafa/Cree")]
        [TestCase("Cree", "F", "Jafa/Cree")]
        [TestCase("Cree", "no", "Jafa/Cree")]
        [TestCase("Cree", "No", "Jafa/Cree")]
        [TestCase("Cree", "NO", "Jafa/Cree")]
        [TestCase("Cree", "false", "Jafa/Cree")]
        [TestCase("Cree", "False", "Jafa/Cree")]
        [TestCase("Cree", "FALSE", "Jafa/Cree")]
        [TestCase("SI", "TRUE", "SI/NO")]
        [TestCase("Yes", "TRUE", "Yes/No")]
        [TestCase("a", "TRUE", "a/b")]
        [TestCase("NO", "FALSE", "SI/NO")]
        [TestCase("No", "FALSE", "Yes/No")]
        [TestCase("b", "FALSE", "a/b")]
        public void BooleanDataFormatter_GenerateValue_Formated(string ExpectedResult, string data, string format)
        {

            Assert.AreEqual(ExpectedResult, TestObject.FormatData(data, format));

        }

        [TestCase("")]
        [TestCase("#")]
        [TestCase("asdf")]
        [TestCase("123..546")]
        [TestCase("1234i")]
        [TestCase("#2134")]
        public void BooleanDataFormatter_FormatData_BadValues_ProvidedFormat(string data)
        {

            Assert.AreEqual(data, TestObject.FormatData(data, "Yes/No"));

        }

        [TestCase("")]
        [TestCase("#")]
        [TestCase("asdf")]
        [TestCase("1234")]
        [TestCase("#2134")]
        public void BooleanDataFormatter_FormatData_BadValues_BadFormat(string data)
        {

            Assert.AreEqual(data, TestObject.FormatData(data, "BOB"));

        }

        [TestCase("TrueFalse", "1")]
        [TestCase("T.F", "x")]
        [TestCase("T\\F", "X")]
        [TestCase("T'F", "t")]
        [TestCase("T", "T")]
        [TestCase("False=True", "y")]
        public void BooleanDataFormatter_FormatData_BadFormat(string format,string data)
        {

            Assert.AreEqual("True", TestObject.FormatData(data, format));

        }

    }
}
