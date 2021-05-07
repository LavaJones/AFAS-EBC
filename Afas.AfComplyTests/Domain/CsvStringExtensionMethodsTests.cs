using System;

using NUnit.Framework;
using Rhino.Mocks;

using Afas.AfComply.Domain;
using Afas.Domain;
using Afas.Application.CSV;

namespace Afas.AfComply.DomainTests
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

        [Test]
        public void EnsureRemoveIllegalAddressIRSCharacters()
        {
            Assert.AreEqual(@"1420 NW 11ST ST D-104", @"1420 NW 11ST ST ?{})(*^%$""#D-104".RemoveIllegalAddressIRSCharacters().TruncateLongString(35), "String should have requested characters removed.");
        }

        [Test]
        public void EnsureRemoveIllegalNameIRSCharacters()
        {
            Assert.AreEqual(@"abc", @"!a;:)@b($c#_\.,+=?'*{}|".RemoveIllegalNameIRSCharacters(), "String should have requested characters removed.");
        }

        [Test]
        public void EnsureRemoveIllegalBusinessIRSCharacters()
        {
            Assert.AreEqual(@"abc", @"!a;:)@b($c#_.,+=?*{}|".RemoveIllegalBusinessIRSCharacters(), "String should have requested characters removed.");
        }

        [Test]
        public void EnsureRemoveIllegalCityIRSCharacters()
        {
            Assert.AreEqual(@"abc", @"!a;:)@b($c#_.,+=?*{}|".RemoveIllegalCityIRSCharacters(), "String should have requested characters removed.");
        }

        [Test]
        public void EnsureRemoveDashes()
        {
            Assert.AreEqual(@"1544454892", @"154-445-4892".RemoveDashes(), "String should have requested characters removed.");
        }

    }

}
