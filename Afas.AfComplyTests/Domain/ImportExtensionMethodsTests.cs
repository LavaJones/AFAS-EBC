using System;

using NUnit.Framework;
using Rhino.Mocks;

using Afas.AfComply.Domain;

namespace Afas.AfComply.DomainTests
{
    
    [TestFixture]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ImportExtensionMethodsTests
    {

        [Test]
        public void IsHeaderRowIsTrueForLegacyFederalIdentificationNumberVariant1()
        {
            Assert.IsTrue("CoFEIN".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyFederalIdentificationNumberVariant2()
        {
            Assert.IsTrue("Co_FEIN".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyEmployeeIdentificationNumberVariant1()
        {
            Assert.IsTrue("EmpEIN".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyDependentIdentificationNumberVariant1()
        {
            Assert.IsTrue("DepEIN".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyAddressLineVariant1()
        {
            Assert.IsTrue("Address1".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyAddressLineVariant2()
        {
            Assert.IsTrue("Address2".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyNameSuffixVariant1()
        {
            Assert.IsTrue("Suffix".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyPayrollHoursWorkedVariant1()
        {
            Assert.IsTrue("HoursWorked".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyPayrollPayPeriodEndDateVariant1()
        {
            Assert.IsTrue("PayPeriodEndDate".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyFirstNameVariant1()
        {
            Assert.IsTrue("FirstName".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyLastNameVariant1()
        {
            Assert.IsTrue("LastName".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyFirstNameVariant2()
        {
            Assert.IsTrue("First_Name".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyLastNameVariant2()
        {
            Assert.IsTrue("Last_Name".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyMiddleNameVariant1()
        {
            Assert.IsTrue("MiddleName".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyMiddleNameVariant2()
        {
            Assert.IsTrue("Middle_Name".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyNameVariant1()
        {
            Assert.IsTrue("Name".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyOfferImportRowId()
        {
            Assert.IsTrue("ROW ID".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyOfferImportEmployeeId()
        {
            Assert.IsTrue("EMPLOYEE ID".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyOfferImportEmployerId()
        {
            Assert.IsTrue("EMPLOYER ID".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyOfferImportPlanYearId()
        {
            Assert.IsTrue("PLAN YEAR ID".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyOfferImportPayrollId()
        {
            Assert.IsTrue("PAYROLL ID".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyOfferImportName()
        {
            Assert.IsTrue("NAME".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyOfferImportClassId()
        {
            Assert.IsTrue("CLASS ID".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyOfferImportAvgHours()
        {
            Assert.IsTrue("AVG HOURS".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyOfferImportOfferedOn()
        {
            Assert.IsTrue("OFFERED ON".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyOfferImportOffered()
        {
            Assert.IsTrue("OFFERED".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyOfferImportAccepteDeclinedOn()
        {
            Assert.IsTrue("ACCEPTED/DECLINED ON".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyOfferImportAccepted()
        {
            Assert.IsTrue("ACCEPTED".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyOfferImportInsuranceId()
        {
            Assert.IsTrue("INSURANCE ID".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyOfferImportContributionId()
        {
            Assert.IsTrue("CONTRIBUTION ID".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyOfferImportEffectiveDate()
        {
            Assert.IsTrue("EFFECTIVE DATE".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyOfferImportHRAFlex()
        {
            Assert.IsTrue("HRA-Flex".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyDemographicsSocial()
        {
            Assert.IsTrue("Social".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyDemographicsHire_Date()
        {
            Assert.IsTrue("Hire_Date".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyDemographicsTerm_Date()
        {
            Assert.IsTrue("Term_Date".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyDemographicsEmployee_Number()
        {
            Assert.IsTrue("Employee_Number".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyDemographicsAddress_1()
        {
            Assert.IsTrue("Address_1".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyDemographicsAddress_2()
        {
            Assert.IsTrue("Address_2".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyDemographicsPostal_Code()
        {
            Assert.IsTrue("Postal_Code".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyDemographicsEmployee_Type()
        {
            Assert.IsTrue("Employee_Type".IsHeaderRow(), "Known column name should be true.");
        }

        [Test]
        public void IsHeaderRowIsTrueForLegacyDemographicsMeasurement_Group()
        {
            Assert.IsTrue("Measurement_Group".IsHeaderRow(), "Known column name should be true.");
        }

    }

}

