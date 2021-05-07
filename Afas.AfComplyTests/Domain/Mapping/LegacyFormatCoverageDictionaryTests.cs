using System;

using NUnit.Framework;
using Rhino.Mocks;

using Afas.AfComply.Domain.Mapping;

namespace Afas.AfComply.Domain.MappingTests
{

    [TestFixture]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class LegacyFormatCoverageDictionaryTests
    {

        [Test]
        public void CoFEINIsMarkedForDeletion()
        {

            String renamedColumn = LegacyFormatCoverageDictionary.Map["CoFEIN"];

            Assert.IsTrue(renamedColumn.StartsWith("DEL"), "Should be flagged for deletion.");

        }

        [Test]
        public void HrStatusCodeIsMarkedForDeletion()
        {

            String renamedColumn = LegacyFormatCoverageDictionary.Map["HR Status Code"];

            Assert.IsTrue(renamedColumn.StartsWith("DEL"), "Should be flagged for deletion.");

        }

        [Test]
        public void HrStatusDescriptionIsMarkedForDeletion()
        {

            String renamedColumn = LegacyFormatCoverageDictionary.Map["HR Status Description"];

            Assert.IsTrue(renamedColumn.StartsWith("DEL"), "Should be flagged for deletion.");

        }

        [Test]
        public void AcaStatusIsMarkedForDeletion()
        {

            String renamedColumn = LegacyFormatCoverageDictionary.Map["ACA Status"];

            Assert.IsTrue(renamedColumn.StartsWith("DEL"), "Should be flagged for deletion.");

        }

        [Test]
        public void EmployeeClassIsMarkedForDeletion()
        {

            String renamedColumn = LegacyFormatCoverageDictionary.Map["Employee Class"];

            Assert.IsTrue(renamedColumn.StartsWith("DEL"), "Should be flagged for deletion.");

        }

        [Test]
        public void EmployeeTypeIsMarkedForDeletion()
        {

            String renamedColumn = LegacyFormatCoverageDictionary.Map["Employee Type"];

            Assert.IsTrue(renamedColumn.StartsWith("DEL"), "Should be flagged for deletion.");

        }

        [Test]
        public void RelationshipIsMarkedForInsuredMemberRename()
        {

            String renamedColumn = LegacyFormatCoverageDictionary.Map["Relationship"];

            Assert.AreEqual("InsuredMember", renamedColumn, "Should have the correct renamed field name.");

        }

        [Test]
        public void NameIsMarkedForCoveredNameRename()
        {

            String renamedColumn = LegacyFormatCoverageDictionary.Map["Name"];

            Assert.AreEqual("CoveredName", renamedColumn, "Should have the correct renamed field name.");

        }

        [Test]
        public void EmpEINIsMarkedForSubscriberSSNRename()
        {

            String renamedColumn = LegacyFormatCoverageDictionary.Map["EmpEIN"];

            Assert.AreEqual("Subscriber SSN", renamedColumn, "Should have the correct renamed field name.");

        }

        [Test]
        public void DepEINIsMarkedForDependentSSNRename()
        {

            String renamedColumn = LegacyFormatCoverageDictionary.Map["DepEIN"];

            Assert.AreEqual("Dependent SSN", renamedColumn, "Should have the correct renamed field name.");

        }

        [Test]
        public void OfferDateIsMarkedForOfferedOnRename()
        {

            String renamedColumn = LegacyFormatCoverageDictionary.Map["OfferDate"];

            Assert.AreEqual("Offered On", renamedColumn, "Should have the correct renamed field name.");

        }

        [Test]
        public void StartDateIsMarkedForCoverageDateStartRename()
        {

            String renamedColumn = LegacyFormatCoverageDictionary.Map["StartDate"];

            Assert.AreEqual("Coverage Date Start", renamedColumn, "Should have the correct renamed field name.");

        }

        [Test]
        public void EndDateIsMarkedForCoverageDateEndRename()
        {

            String renamedColumn = LegacyFormatCoverageDictionary.Map["EndDate"];

            Assert.AreEqual("Coverage Date End", renamedColumn, "Should have the correct renamed field name.");

        }

        [Test]
        public void MemberNameIsMarkedForCoveredNameRename()
        {

            String renamedColumn = LegacyFormatCoverageDictionary.Map["Member Name"];

            Assert.AreEqual("CoveredName", renamedColumn, "Should have the correct renamed field name.");

        }

    }

}
