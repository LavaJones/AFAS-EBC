using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Afas.AfComply.Domain;
using Afas.Domain;

namespace Afas.AfComplyTests.Domain
{
    [TestFixture]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class IListExtensionMethodsTests
    {
        [Test]
        public void ConversionOfListToDataTable()
        {

            var expected = new TaxYear1095CCorrection()
            {
                tax_year = 2016,
                employee_id = 3,
                employer_id = 1,
                ResourceId = Guid.NewGuid(),
                Corrected = false,
                OriginalUniqueSubmissionId = "Test Original Unique Submission Id",
                CorrectedUniqueRecordId = "Test Corrected Unique Record Id",
                CorrectedUniqueSubmissionId = "Test Corrected Unique Submission Id",
                Transmitted = true,
                ModifiedBy = "SYSTEM",
                ModifiedDate = DateTime.Now
            };

            List<TaxYear1095CCorrection> taxCorrections = new List<TaxYear1095CCorrection>() { expected };

            DataTable dataTable = taxCorrections.AsEnumerable().ToDataTable();

            var actual = dataTable.DataTableToObject<TaxYear1095CCorrection>();

            Assert.AreEqual(expected.tax_year, actual.tax_year, "tax_year should be the same");
            Assert.AreEqual(expected.employee_id, actual.employee_id, "employee_id should be the same");
            Assert.AreEqual(expected.employer_id, actual.employer_id, "employer_id should be the same");
            Assert.AreEqual(expected.ResourceId, actual.ResourceId, "ResourceId should be the same");
            Assert.AreEqual(expected.Corrected, actual.Corrected, "Corrected should be the same");
            Assert.AreEqual(expected.OriginalUniqueSubmissionId, actual.OriginalUniqueSubmissionId, "OriginalUniqueSubmissionId should be the same");
            Assert.AreEqual(expected.CorrectedUniqueRecordId, actual.CorrectedUniqueRecordId, "CorrectedUniqueRecordId should be the same");
            Assert.AreEqual(expected.CorrectedUniqueSubmissionId, actual.CorrectedUniqueSubmissionId, "CorrectedUniqueSubmissionId should be the same");
            Assert.AreEqual(expected.Transmitted, actual.Transmitted, "Transmitted should be the same");
            Assert.AreEqual(expected.ModifiedBy, actual.ModifiedBy, "ModifiedBy should be the same");
            Assert.AreEqual(expected.ModifiedDate, actual.ModifiedDate, "ModifiedDate should be the same");

        }
   
        [Test]
        public void ConversionOfListToDataTableWithNulls()
        {

            var expected = new TaxYear1095CCorrection()
            {
                tax_year = 2016,
                employee_id = 3,
                employer_id = 1,
                ResourceId = Guid.NewGuid(),
                Corrected = false,
                Transmitted = true,
                ModifiedBy = "SYSTEM",
                ModifiedDate = DateTime.Now
            };

            List<TaxYear1095CCorrection> taxCorrections = new List<TaxYear1095CCorrection>() { expected };

            DataTable dataTable = taxCorrections.AsEnumerable().ToDataTable();

            var actual = dataTable.DataTableToObject<TaxYear1095CCorrection>();

            Assert.AreEqual(expected.tax_year, actual.tax_year, "tax_year should be the same");
            Assert.AreEqual(expected.employee_id, actual.employee_id, "employee_id should be the same");
            Assert.AreEqual(expected.employer_id, actual.employer_id, "employer_id should be the same");
            Assert.AreEqual(expected.ResourceId, actual.ResourceId, "ResourceId should be the same");
            Assert.AreEqual(expected.Corrected, actual.Corrected, "Corrected should be the same");
            Assert.AreEqual(expected.OriginalUniqueSubmissionId, actual.OriginalUniqueSubmissionId, "OriginalUniqueSubmissionId should be the same");
            Assert.AreEqual(expected.CorrectedUniqueRecordId, actual.CorrectedUniqueRecordId, "CorrectedUniqueRecordId should be the same");
            Assert.AreEqual(expected.CorrectedUniqueSubmissionId, actual.CorrectedUniqueSubmissionId, "CorrectedUniqueSubmissionId should be the same");
            Assert.AreEqual(expected.Transmitted, actual.Transmitted, "Transmitted should be the same");
            Assert.AreEqual(expected.ModifiedBy, actual.ModifiedBy, "ModifiedBy should be the same");
            Assert.AreEqual(expected.ModifiedDate, actual.ModifiedDate, "ModifiedDate should be the same");

        }
    
    }
}
