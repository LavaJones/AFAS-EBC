using System;

using NUnit.Framework;
using Rhino.Mocks;

using Afas.AfComply.Domain.Mapping;

namespace Afas.AfComply.Domain.MappingTests
{

    [TestFixture]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class AfComplyFormatExtendedCarrierDictionaryTests
    {

        [Test]
        public void MiddleNameIsMarkedForDeletion()
        {

            String renamedColumn = AfComplyFormatExtendedCarrierDictionary.Map["Middle Name"];

            Assert.IsTrue(renamedColumn.StartsWith("DEL"), "Should be flagged for deletion.");

        }

        [Test]
        public void SuffixIsMarkedForDeletion()
        {

            String renamedColumn = AfComplyFormatExtendedCarrierDictionary.Map["Suffix"];

            Assert.IsTrue(renamedColumn.StartsWith("DEL"), "Should be flagged for deletion.");

        }

        [Test]
        public void MemberIsMarkedForInsuredMemberRename()
        {

            String renamedColumn = AfComplyFormatExtendedCarrierDictionary.Map["Member"];

            Assert.AreEqual("InsuredMember", renamedColumn, "Should have the correct renamed field name.");

        }

        [Test]
        public void MemberSSNIsMarkedForSSNRename()
        {

            String renamedColumn = AfComplyFormatExtendedCarrierDictionary.Map["Member SSN"];

            Assert.AreEqual("SSN", renamedColumn, "Should have the correct renamed field name.");

        }

        [Test]
        public void SubscriberSSNIsMarkedForSUBIDRename()
        {

            String renamedColumn = AfComplyFormatExtendedCarrierDictionary.Map["Subscriber SSN"];

            Assert.AreEqual("SUBID", renamedColumn, "Should have the correct renamed field name.");

        }

    }

}
