using System;

using NUnit.Framework;
using Rhino.Mocks;

using Afas.Domain;

namespace Afas.ApplicationTests
{

    [TestFixture]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class CsvStringExtensionMethodsTests
    {

        [Test]
        public void EnsureRemoveCommasRemovesCommas()
        {
            Assert.AreEqual(@"same", "sa,me".RemoveCommas(), "String should have requested characters removed.");
        }

        [Test]
        public void EnsureRemoveCommasRemovesCommasManyCommas()
        {
            Assert.AreEqual(@"same", ",sa,me,".RemoveCommas(), "String should have requested characters removed.");
        }

        [Test]
        public void EnsureRemoveCommasLeavesQuotes()
        {
            Assert.AreEqual(@"same'", "sa,me'".RemoveCommas(), "String should have requested characters removed.");
        }

        [Test]
        public void EnsureRemoveCommasLeavesDoubleQuotes()
        {
            Assert.AreEqual(@"same""", @"sa,me""".RemoveCommas(), "String should have requested characters removed.");
        }

        [Test]
        public void EnsureRemoveDoubleQuotesRemovesDoubleQuotes()
        {
            Assert.AreEqual(@"same", @"sa""me".RemoveDoubleQuotes(), "String should have requested characters removed.");
        }

        [Test]
        public void EnsureRemoveDoubleQuotesRemovesManyDoubleQuotes()
        {
            Assert.AreEqual(@"same", @"""sa""me""".RemoveDoubleQuotes(), "String should have requested characters removed.");
        }

        [Test]
        public void EnsureRemoveDoubleQuotesLeavesCommas()
        {
            Assert.AreEqual(@"sa,me", @"sa"",me".RemoveDoubleQuotes(), "String should have requested characters removed.");
        }

        [Test]
        public void EnsureRemoveDoubleQuotesLeavesQuotes()
        {
            Assert.AreEqual(@"sa'me", @"sa'me""".RemoveDoubleQuotes(), "String should have requested characters removed.");
        }

        [Test]
        public void EnsureRemoveSingleQuotesRemovesRemoveSingleQuotes()
        {
            Assert.AreEqual(@"same", @"sa'me".RemoveSingleQuotes(), "String should have requested characters removed.");
        }

        [Test]
        public void EnsureRemoveSingleQuotesRemovesManyQuotes()
        {
            Assert.AreEqual(@"same", @"'sa'me'".RemoveSingleQuotes(), "String should have requested characters removed.");
        }

        [Test]
        public void EnsureRemoveSingleQuotesLeavesCommas()
        {
            Assert.AreEqual(@"sa,me", @"sa',me".RemoveSingleQuotes(), "String should have requested characters removed.");
        }

        [Test]
        public void EnsureRemoveSingleQuotesLeavesDoubleQuotes()
        {
            Assert.AreEqual(@"sa""me", @"sa""me'".RemoveSingleQuotes(), "String should have requested characters removed.");
        }

    }

}
