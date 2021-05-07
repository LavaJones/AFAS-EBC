using System;

using NUnit.Framework;
using Rhino.Mocks;

using Afas.AfComply.Domain;
using Afas.Application.CSV;

namespace Afas.AfComply.DomainTests
{

    [TestFixture]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class CsvHelperTests
    {

        [Test]
        public void SanitizeHeaderValuesStripsSingleTicks()
        {
            
            String[] values = @"'one',two".Split(',');

            String[] cleaned = CsvHelper.SanitizeHeaderValues(values);

            Assert.AreEqual("one", cleaned[0], "Values should be in sync if sanitized.");
            Assert.AreEqual("two", cleaned[1], "Values should be left if no sanitazation is needed.");

        }

        [Test]
        public void SanitizeHeaderValuesStripsDoubleQuotes()
        {

            String[] values = @"""one"",two".Split(',');

            String[] cleaned = CsvHelper.SanitizeHeaderValues(values);

            Assert.AreEqual("one", cleaned[0], "Values should be in sync if sanitized.");
            Assert.AreEqual("two", cleaned[1], "Values should be left if no sanitazation is needed.");

        }

        [Test]
        public void SanitizeHeaderValuesLeavesBackTicks()
        {

            String[] values = @"`one`,two".Split(',');

            String[] cleaned = CsvHelper.SanitizeHeaderValues(values);

            Assert.AreEqual("`one`", cleaned[0], "Values should be left if no sanitazation is needed.");
            Assert.AreEqual("two", cleaned[1], "Values should be left if no sanitazation is needed.");

        }

    }

}
