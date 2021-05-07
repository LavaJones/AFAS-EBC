using System;

using NUnit.Framework;
using Rhino.Mocks;

using Afas.AfComply.Domain.Mapping;

namespace Afas.AfComply.Domain.MappingTests
{

    [TestFixture]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class LegacyFormatDemographicsDictionaryTests
    {

        [Test]
        public void CoFEINIsMarkedForDeletion()
        {

            String renamedColumn = LegacyFormatDemographicsDictionary.Map["CoFEIN"];

            Assert.IsTrue(renamedColumn.StartsWith("DEL"), "Should be flagged for deletion.");

        }

        [Test]
        public void Client_EEIdIsMarkedForEmployeeNumberRename()
        {

            String renamedColumn = LegacyFormatDemographicsDictionary.Map["Client_EEId"];

            Assert.AreEqual("Employee #", renamedColumn, "Should have the correct renamed field name.");

        }

        [Test]
        public void EINIsMarkedForSSNRename()
        {

            String renamedColumn = LegacyFormatDemographicsDictionary.Map["EIN"];

            Assert.AreEqual("SSN", renamedColumn, "Should have the correct renamed field name.");

        }

        [Test]
        public void LastNameIsMarkedForLast_NameRename()
        {

            String renamedColumn = LegacyFormatDemographicsDictionary.Map["LastName"];

            Assert.AreEqual("Last_Name", renamedColumn, "Should have the correct renamed field name.");

        }

        [Test]
        public void FirstNameIsMarkedForFirst_NameRename()
        {

            String renamedColumn = LegacyFormatDemographicsDictionary.Map["FirstName"];

            Assert.AreEqual("First_Name", renamedColumn, "Should have the correct renamed field name.");

        }

        [Test]
        public void MiddleNameIsMarkedForMiddle_NameRename()
        {

            String renamedColumn = LegacyFormatDemographicsDictionary.Map["MiddleName"];

            Assert.AreEqual("Middle_Name", renamedColumn, "Should have the correct renamed field name.");

        }

        [Test]
        public void SuffixIsMarkedForDeletion()
        {

            String renamedColumn = LegacyFormatDemographicsDictionary.Map["Suffix"];

            Assert.IsTrue(renamedColumn.StartsWith("DEL"), "Should be flagged for deletion.");

        }

        [Test]
        public void Address1IsMarkedForStreet_AddressRename()
        {

            String renamedColumn = LegacyFormatDemographicsDictionary.Map["Address1"];

            Assert.AreEqual("Street Address", renamedColumn, "Should have the correct renamed field name.");

        }

        [Test]
        public void Address2IsMarkedForDeletion()
        {

            String renamedColumn = LegacyFormatDemographicsDictionary.Map["Address2"];

            Assert.IsTrue(renamedColumn.StartsWith("DEL"), "Should be flagged for deletion.");

        }

        [Test]
        public void CityIsNotInTheCollection()
        {
            Assert.IsFalse(LegacyFormatDemographicsDictionary.Map.Keys.Contains("City"), "Columns without renames are not present.");
        }

        [Test]
        public void StateIsMarkedForStateCodeRename()
        {

            String renamedColumn = LegacyFormatDemographicsDictionary.Map["State"];

            Assert.AreEqual("State Code", renamedColumn, "Should have the correct renamed field name.");

        }

        [Test]
        public void PostalCodeIsMarkedForZipCodeRename()
        {

            String renamedColumn = LegacyFormatDemographicsDictionary.Map["PostalCode"];

            Assert.AreEqual("Zip Code", renamedColumn, "Should have the correct renamed field name.");

        }

        [Test]
        public void HireDtIsMarkedForHireDateRename()
        {

            String renamedColumn = LegacyFormatDemographicsDictionary.Map["HireDt"];

            Assert.AreEqual("Hire Date", renamedColumn, "Should have the correct renamed field name.");

        }

        [Test]
        public void TermDtIsMarkedForTerminationDateRename()
        {

            String renamedColumn = LegacyFormatDemographicsDictionary.Map["TermDt"];

            Assert.AreEqual("Termination Date", renamedColumn, "Should have the correct renamed field name.");

        }

        [Test]
        public void EmpTypeIsMarkedForHrStatusCodeRename()
        {

            String renamedColumn = LegacyFormatDemographicsDictionary.Map["EmpType"];

            Assert.AreEqual("HR Status Code", renamedColumn, "Should have the correct renamed field name.");

        }

        [Test]
        public void MeasurementGroupIsMarkedForHrStatusDescriptionRename()
        {

            String renamedColumn = LegacyFormatDemographicsDictionary.Map["MeasurementGroup"];

            Assert.AreEqual("HR Status Description", renamedColumn, "Should have the correct renamed field name.");

        }

        [Test]
        public void SalaryIsMarkedForDeletion()
        {

            String renamedColumn = LegacyFormatDemographicsDictionary.Map["Salary"];

            Assert.IsTrue(renamedColumn.StartsWith("DEL"), "Should be flagged for deletion.");

        }

        [Test]
        public void PayRateIsMarkedForDeletion()
        {

            String renamedColumn = LegacyFormatDemographicsDictionary.Map["PayRate"];

            Assert.IsTrue(renamedColumn.StartsWith("DEL"), "Should be flagged for deletion.");

        }

        [Test]
        public void PayPeriodNameIsMarkedForDeletion()
        {

            String renamedColumn = LegacyFormatDemographicsDictionary.Map["PayPeriodName"];

            Assert.IsTrue(renamedColumn.StartsWith("DEL"), "Should be flagged for deletion.");

        }

        [Test]
        public void EmployeeGroupNameIsMarkedForDeletion()
        {

            String renamedColumn = LegacyFormatDemographicsDictionary.Map["EmployeeGroupName"];

            Assert.IsTrue(renamedColumn.StartsWith("DEL"), "Should be flagged for deletion.");

        }

        [Test]
        public void CountryIsMarkedForDeletion()
        {

            String renamedColumn = LegacyFormatDemographicsDictionary.Map["Country"];

            Assert.IsTrue(renamedColumn.StartsWith("DEL"), "Should be flagged for deletion.");

        }

        [Test]
        public void SocialIsMarkedForSSNRename()
        {

            String renamedColumn = LegacyFormatDemographicsDictionary.Map["Social"];

            Assert.AreEqual("SSN", renamedColumn, "Should have the correct renamed field name.");

        }

        [Test]
        public void Hire_DateIsMarkedForHireDateRename()
        {

            String renamedColumn = LegacyFormatDemographicsDictionary.Map["Hire_Date"];

            Assert.AreEqual("Hire Date", renamedColumn, "Should have the correct renamed field name.");

        }

        [Test]
        public void Term_DateIsMarkedForTerminationDateRename()
        {

            String renamedColumn = LegacyFormatDemographicsDictionary.Map["Term_Date"];

            Assert.AreEqual("Termination Date", renamedColumn, "Should have the correct renamed field name.");

        }

        [Test]
        public void Employee_NumberIsMarkedForEmployeeHashtagRename()
        {

            String renamedColumn = LegacyFormatDemographicsDictionary.Map["Employee_Number"];

            Assert.AreEqual("Employee #", renamedColumn, "Should have the correct renamed field name.");

        }

        [Test]
        public void Address_1IsMarkedForStreetAddressRename()
        {

            String renamedColumn = LegacyFormatDemographicsDictionary.Map["Address_1"];

            Assert.AreEqual("Street Address", renamedColumn, "Should have the correct renamed field name.");

        }

        [Test]
        public void Address_2IsMarkedForDeletion()
        {

            String renamedColumn = LegacyFormatDemographicsDictionary.Map["Address_2"];

            Assert.IsTrue(renamedColumn.StartsWith("DEL"), "Should be flagged for removal.");

        }

        [Test]
        public void Postal_CodeIsMarkedForZipCodeRename()
        {

            String renamedColumn = LegacyFormatDemographicsDictionary.Map["Postal_Code"];

            Assert.AreEqual("Zip Code", renamedColumn, "Should have the correct renamed field name.");

        }

        [Test]
        public void Employee_TypeIsMarkedForHrStatusCodeRename()
        {

            String renamedColumn = LegacyFormatDemographicsDictionary.Map["Employee_Type"];

            Assert.AreEqual("HR Status Code", renamedColumn, "Should have the correct renamed field name.");

        }

        [Test]
        public void Measurement_GroupIsMarkedForHrStatusDescriptionRename()
        {

            String renamedColumn = LegacyFormatDemographicsDictionary.Map["Measurement_Group"];

            Assert.AreEqual("HR Status Description", renamedColumn, "Should have the correct renamed field name.");

        }

    }

}
