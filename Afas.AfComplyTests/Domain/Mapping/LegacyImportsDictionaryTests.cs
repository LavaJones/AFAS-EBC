using System;

using NUnit.Framework;
using Rhino.Mocks;

using Afas.AfComply.Domain.Mapping;

namespace Afas.AfComply.Domain.MappingTests
{

    [TestFixture]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class LegacyImportsDictionaryTests
    {

        [Test]
        public void SocialIsMarkedForSSNRename()
        {

            String renamedColumn = LegacyImportsDictionary.Map["Social"];

            Assert.AreEqual("SSN", renamedColumn, "Should have the correct renamed field name.");

        }

        [Test]
        public void Employee_EINIsMarkedForSSNRename()
        {

            String renamedColumn = LegacyImportsDictionary.Map["Employee_EIN"];

            Assert.AreEqual("SSN", renamedColumn, "Should have the correct renamed field name.");

        }

        [Test]
        public void Dependent_EINMarkedForDependent_EINRename()
        {

            String renamedColumn = LegacyImportsDictionary.Map["Dependent_EIN"];

            Assert.AreEqual("DepEIN", renamedColumn, "Should have the correct renamed field name.");

        }

        [Test]
        public void EmployeeGroupMarkedForEETypeRename()
        {

            String renamedColumn = LegacyImportsDictionary.Map["Employee_Group"];

            Assert.AreEqual("EEType", renamedColumn, "Should have the correct renamed field name.");

        }

    }

}
