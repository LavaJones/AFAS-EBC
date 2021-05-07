using System;

using System.IO;

using NUnit.Framework;
using Rhino.Mocks;

using Afas.Domain;

namespace Afas.AfComply.DomainTests
{
    
    [TestFixture]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class TextWriterExtensionMethodsTests
    {

        [Test]
        public void WriteCsvEscapedStripsDoubleQuotes()
        {
            
            StringWriter stringWriter = new StringWriter();
            ( (TextWriter) stringWriter).WriteCsvEscaped("\"stuff\"");

            Assert.AreEqual("stuff", stringWriter.ToString(), "Should match post write.");

        }

        [Test]
        public void WriteCsvEscapesWithDoubleQuotesForCommaValues()
        {

            StringWriter stringWriter = new StringWriter();
            ((TextWriter)stringWriter).WriteCsvEscaped("stuff, jr");

            Assert.AreEqual("\"stuff, jr\"", stringWriter.ToString(), "Should match post write.");

        }

        [Test]
        public void WriteCsvLeavesDataAloneWithoutCommasNumbers()
        {

            StringWriter stringWriter = new StringWriter();
            ((TextWriter)stringWriter).WriteCsvEscaped("12345");

            Assert.AreEqual("12345", stringWriter.ToString(), "Should match post write.");

        }

        [Test]
        public void WriteCsvLeavesDataAloneWithoutCommasDates()
        {

            StringWriter stringWriter = new StringWriter();
            ((TextWriter)stringWriter).WriteCsvEscaped("2016-01-01");

            Assert.AreEqual("2016-01-01", stringWriter.ToString(), "Should match post write.");

        }

        [Test]
        public void GetCsvReturnEmptyStringForNullValue()
        {
            String value = null;
            Assert.AreEqual(String.Empty, value.GetCsvEscaped(), "Should return empty string");
        }

    }

}
