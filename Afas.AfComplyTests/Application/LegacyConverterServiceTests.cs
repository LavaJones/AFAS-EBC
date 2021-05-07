using System;
using System.Collections.Generic;
using System.Data;

using log4net;

using NUnit.Framework;
using Rhino.Mocks;

using Afas.AfComply.Application;
using Afas.AfComply.Domain;

namespace Afas.AfComply.ApplicationTests
{

    [TestFixture]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class LegacyConverterServiceTests
    {

        [Test]
        public void RemoveZeroHourEntriesFromPayrollConversionRunsEmptyTable()
        {

            DataTable dataTable = new DataTable("CSVTable");

            new LegacyConverterService().RemoveZeroHourEntriesFromPayrollConversion(dataTable);

        }

        [Test]
        public void RemoveZeroHourEntriesFromPayrollConversionLeavesEqualOne()
        {

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("ACA Hours");
            DataRow dataRow = dataTable.NewRow();
            dataRow["ACA Hours"] = "1.00";
            dataTable.Rows.Add(dataRow);

            new LegacyConverterService().RemoveZeroHourEntriesFromPayrollConversion(dataTable);

            Assert.AreEqual(1, dataTable.Rows.Count, "Should still have the valid entries.");

        }

        [Test]
        public void RemoveZeroHourEntriesFromPayrollConversionLeavesGreaterThanOne()
        {

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("ACA Hours");
            DataRow dataRow = dataTable.NewRow();
            dataRow["ACA Hours"] = "1.10";
            dataTable.Rows.Add(dataRow);

            new LegacyConverterService().RemoveZeroHourEntriesFromPayrollConversion(dataTable);

            Assert.AreEqual(1, dataTable.Rows.Count, "Should still have the valid entries.");

        }

        [Test]
        public void RemoveZeroHourEntriesFromPayrollConversionLeavesNegative()
        {

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("ACA Hours");
            DataRow dataRow = dataTable.NewRow();
            dataRow["ACA Hours"] = "-1.00";
            dataTable.Rows.Add(dataRow);

            new LegacyConverterService().RemoveZeroHourEntriesFromPayrollConversion(dataTable);

            Assert.AreEqual(1, dataTable.Rows.Count, "Should still have the valid entries.");

        }

        [Test]
        public void RemoveZeroHourEntriesFromPayrollConversionRemovesZero()
        {

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("ACA Hours");

            DataRow dataRow = dataTable.NewRow();
            dataRow["ACA Hours"] = "0";
            dataTable.Rows.Add(dataRow);

            dataRow = dataTable.NewRow();
            dataRow["ACA Hours"] = "1.00";
            dataTable.Rows.Add(dataRow);

            new LegacyConverterService().RemoveZeroHourEntriesFromPayrollConversion(dataTable);

            Assert.AreEqual(1, dataTable.Rows.Count, "Should still have the valid entries.");

        }

        [Test]
        public void RemoveZeroHourEntriesFromPayrollConversionRemovesZeroDecimal()
        {

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("ACA Hours");

            DataRow dataRow = dataTable.NewRow();
            dataRow["ACA Hours"] = "0.00";
            dataTable.Rows.Add(dataRow);

            dataRow = dataTable.NewRow();
            dataRow["ACA Hours"] = "2.00";
            dataTable.Rows.Add(dataRow);

            new LegacyConverterService().RemoveZeroHourEntriesFromPayrollConversion(dataTable);

            Assert.AreEqual(1, dataTable.Rows.Count, "Should still have the valid entries.");
            

        }

        [Test]
        public void AddAFcomplyCarrierColumnsAddsColumnsMissing()
        {

            DataTable dataTable = new DataTable("CSVTable");


            new LegacyConverterService().AddAFcomplyCarrierColumns(dataTable);

            foreach (String columnName in "Member,Member SSN,First Name,Middle Name,Last Name,Suffix,DOB,JAN,FEB,MAR,APR,MAY,JUN,JUL,AUG,SEP,OCT,NOV,DEC".Split(','))
            {
                Assert.IsTrue(dataTable.Columns.Contains(columnName), String.Format("Requested Column {0} should be present.", columnName));
            }

        }

        [Test]
        public void AddAFcomplyDemographicColumnsAddsColumnsMissing()
        {

            DataTable dataTable = new DataTable("CSVTable");


            new LegacyConverterService().AddAFcomplyDemographicColumns(dataTable);

            foreach (String columnName in "Zip+4,Change Date,DOB".Split(','))
            {
                Assert.IsTrue(dataTable.Columns.Contains(columnName), String.Format("Requested Column {0} should be present.", columnName));
            }

        }

        [Test]
        public void AddAFcomplyExtendedOfferColumnsAddsColumnsMissing()
        {

            DataTable dataTable = new DataTable("CSVTable");


            new LegacyConverterService().AddAFcomplyExtendedOfferColumns(dataTable);

            foreach (String columnName in "First Name,Middle Name,Last Name,Suffix,DOB,Offered,Offered On,Accepted,Accepted/Declined On,Medical Plan Name,Coverage Date Start,Coverage Date End,Hire Date,Change Date,Rehire Date,Termination Date,Employee #".Split(','))
            {
                Assert.IsTrue(dataTable.Columns.Contains(columnName), String.Format("Requested Column {0} should be present.", columnName));
            }

        }

        [Test]
        public void AddAFcomplyPayrollColumnsAddsColumnsMissing()
        {

            DataTable dataTable = new DataTable("CSVTable");


            new LegacyConverterService().AddAFcomplyPayrollColumns(dataTable);

            foreach (String columnName in "Middle_Name,Start Date,Pay Description,Pay Description ID,Check Date,Employee #".Split(','))
            {
                Assert.IsTrue(dataTable.Columns.Contains(columnName), String.Format("Requested Column {0} should be present.", columnName));
            }

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForJanuaryStartEnd()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("January", DateTime.Parse("2016-01-01"), DateTime.Parse("2016-01-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForJanuaryStartEndDecember()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("January", DateTime.Parse("2016-01-01"), DateTime.Parse("2016-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForFebruaryStartEnd()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("February", DateTime.Parse("2016-02-01"), DateTime.Parse("2016-02-28")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForFebruaryStartEndDecember()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("February", DateTime.Parse("2016-02-01"), DateTime.Parse("2016-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForFebruaryJanuaryStartEndDecember()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("February", DateTime.Parse("2016-01-01"), DateTime.Parse("2016-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForMarchStartEnd()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("March", DateTime.Parse("2016-03-01"), DateTime.Parse("2016-03-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForMarchStartEndDecember()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("March", DateTime.Parse("2016-03-01"), DateTime.Parse("2016-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForMarchJanuaryStartEndDecember()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("March", DateTime.Parse("2016-01-01"), DateTime.Parse("2016-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForAprilStartEnd()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("April", DateTime.Parse("2016-04-01"), DateTime.Parse("2016-04-30")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForAprilStartEndDecember()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("April", DateTime.Parse("2016-04-01"), DateTime.Parse("2016-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForAprilJanuaryStartEndDecember()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("April", DateTime.Parse("2016-01-01"), DateTime.Parse("2016-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForMayStartEnd()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("May", DateTime.Parse("2016-05-01"), DateTime.Parse("2016-05-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForMayStartEndDecember()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("May", DateTime.Parse("2016-05-01"), DateTime.Parse("2016-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForMayJanuaryStartEndDecember()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("May", DateTime.Parse("2016-01-01"), DateTime.Parse("2016-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForJuneStartEnd()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("June", DateTime.Parse("2016-06-01"), DateTime.Parse("2016-06-30")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForJuneStartEndDecember()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("June", DateTime.Parse("2016-06-01"), DateTime.Parse("2016-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForJuneJanuaryStartEndDecember()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("June", DateTime.Parse("2016-01-01"), DateTime.Parse("2016-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForJulyStartEnd()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("July", DateTime.Parse("2016-07-01"), DateTime.Parse("2016-07-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForJulyStartEndDecember()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("July", DateTime.Parse("2016-07-01"), DateTime.Parse("2016-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForJulyJanuaryStartEndDecember()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("July", DateTime.Parse("2016-01-01"), DateTime.Parse("2016-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForAugustStartEnd()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("August", DateTime.Parse("2016-08-01"), DateTime.Parse("2016-08-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForAugustStartEndDecember()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("August", DateTime.Parse("2016-08-01"), DateTime.Parse("2016-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForAugustJanuaryStartEndDecember()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("August", DateTime.Parse("2016-01-01"), DateTime.Parse("2016-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForSeptemberStartEnd()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("September", DateTime.Parse("2016-09-01"), DateTime.Parse("2016-09-30")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForSeptemberStartEndDecember()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("September", DateTime.Parse("2016-09-01"), DateTime.Parse("2016-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForSeptemberJanuaryStartEndDecember()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("September", DateTime.Parse("2016-01-01"), DateTime.Parse("2016-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForOctoberStartEnd()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("October", DateTime.Parse("2016-10-01"), DateTime.Parse("2016-10-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForOctoberStartEndDecember()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("October", DateTime.Parse("2016-10-01"), DateTime.Parse("2016-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForOctoberJanuaryStartEndDecember()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("October", DateTime.Parse("2016-01-01"), DateTime.Parse("2016-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForNovemberStartEnd()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("November", DateTime.Parse("2016-11-01"), DateTime.Parse("2016-11-30")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForNovemberStartEndDecember()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("November", DateTime.Parse("2016-11-01"), DateTime.Parse("2016-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForNovemberJanuaryStartEndDecember()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("November", DateTime.Parse("2016-01-01"), DateTime.Parse("2016-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForDecemberStartEndDecember()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("December", DateTime.Parse("2016-12-01"), DateTime.Parse("2016-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIn2016IsTrueForDecemberJanuaryStartEndDecember()
        {

            Assert.IsTrue(
                    new LegacyConverterService(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonthIn2016("December", DateTime.Parse("2016-01-01"), DateTime.Parse("2016-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void FillCoverageColumnsSetsJanuaryOnlyForOneMonthsCoverage()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("JAN", typeof(String));
            dataTable.Columns.Add("FEB", typeof(String));
            dataTable.Columns.Add("MAR", typeof(String));
            dataTable.Columns.Add("APR", typeof(String));
            dataTable.Columns.Add("MAY", typeof(String));
            dataTable.Columns.Add("JUN", typeof(String));
            dataTable.Columns.Add("JUL", typeof(String));
            dataTable.Columns.Add("AUG", typeof(String));
            dataTable.Columns.Add("SEP", typeof(String));
            dataTable.Columns.Add("OCT", typeof(String));
            dataTable.Columns.Add("NOV", typeof(String));
            dataTable.Columns.Add("DEC", typeof(String));
            dataTable.Columns.Add("TOTAL", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            legacyConverterService.FillCoverageColumns(dataRow, DateTime.Parse("2016-01-01"), DateTime.Parse("2016-01-31"), 2016);

            Assert.AreEqual("1", dataTable.Rows[0]["JAN"], "Enrolled coverages are recorded.");

        }

        [Test]
        public void FillCoverageColumnsSetsFebruaryOnlyForOneMonthsCoverage()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("JAN", typeof(String));
            dataTable.Columns.Add("FEB", typeof(String));
            dataTable.Columns.Add("MAR", typeof(String));
            dataTable.Columns.Add("APR", typeof(String));
            dataTable.Columns.Add("MAY", typeof(String));
            dataTable.Columns.Add("JUN", typeof(String));
            dataTable.Columns.Add("JUL", typeof(String));
            dataTable.Columns.Add("AUG", typeof(String));
            dataTable.Columns.Add("SEP", typeof(String));
            dataTable.Columns.Add("OCT", typeof(String));
            dataTable.Columns.Add("NOV", typeof(String));
            dataTable.Columns.Add("DEC", typeof(String));
            dataTable.Columns.Add("TOTAL", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            legacyConverterService.FillCoverageColumns(dataRow, DateTime.Parse("2016-02-01"), DateTime.Parse("2016-02-29"), 2016);

            Assert.AreEqual("1", dataTable.Rows[0]["FEB"], "Enrolled coverages are recorded.");

        }

        [Test]
        public void FillCoverageColumnsSetsMarchOnlyForOneMonthsCoverage()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("JAN", typeof(String));
            dataTable.Columns.Add("FEB", typeof(String));
            dataTable.Columns.Add("MAR", typeof(String));
            dataTable.Columns.Add("APR", typeof(String));
            dataTable.Columns.Add("MAY", typeof(String));
            dataTable.Columns.Add("JUN", typeof(String));
            dataTable.Columns.Add("JUL", typeof(String));
            dataTable.Columns.Add("AUG", typeof(String));
            dataTable.Columns.Add("SEP", typeof(String));
            dataTable.Columns.Add("OCT", typeof(String));
            dataTable.Columns.Add("NOV", typeof(String));
            dataTable.Columns.Add("DEC", typeof(String));
            dataTable.Columns.Add("TOTAL", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            legacyConverterService.FillCoverageColumns(dataRow, DateTime.Parse("2016-03-01"), DateTime.Parse("2016-03-31"), 2016);

            Assert.AreEqual("1", dataTable.Rows[0]["MAR"], "Enrolled coverages are recorded.");

        }

        [Test]
        public void FillCoverageColumnsSetsAprilOnlyForOneMonthsCoverage()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("JAN", typeof(String));
            dataTable.Columns.Add("FEB", typeof(String));
            dataTable.Columns.Add("MAR", typeof(String));
            dataTable.Columns.Add("APR", typeof(String));
            dataTable.Columns.Add("MAY", typeof(String));
            dataTable.Columns.Add("JUN", typeof(String));
            dataTable.Columns.Add("JUL", typeof(String));
            dataTable.Columns.Add("AUG", typeof(String));
            dataTable.Columns.Add("SEP", typeof(String));
            dataTable.Columns.Add("OCT", typeof(String));
            dataTable.Columns.Add("NOV", typeof(String));
            dataTable.Columns.Add("DEC", typeof(String));
            dataTable.Columns.Add("TOTAL", typeof(String));
            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            legacyConverterService.FillCoverageColumns(dataRow, DateTime.Parse("2016-04-01"), DateTime.Parse("2016-04-30"), 2016);

            Assert.AreEqual("1", dataTable.Rows[0]["APR"], "Enrolled coverages are recorded.");

        }

        [Test]
        public void FillCoverageColumnsSetsMayOnlyForOneMonthsCoverage()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("JAN", typeof(String));
            dataTable.Columns.Add("FEB", typeof(String));
            dataTable.Columns.Add("MAR", typeof(String));
            dataTable.Columns.Add("APR", typeof(String));
            dataTable.Columns.Add("MAY", typeof(String));
            dataTable.Columns.Add("JUN", typeof(String));
            dataTable.Columns.Add("JUL", typeof(String));
            dataTable.Columns.Add("AUG", typeof(String));
            dataTable.Columns.Add("SEP", typeof(String));
            dataTable.Columns.Add("OCT", typeof(String));
            dataTable.Columns.Add("NOV", typeof(String));
            dataTable.Columns.Add("DEC", typeof(String));
            dataTable.Columns.Add("TOTAL", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            legacyConverterService.FillCoverageColumns(dataRow, DateTime.Parse("2016-05-01"), DateTime.Parse("2016-05-31"), 2016);

            Assert.AreEqual("1", dataTable.Rows[0]["MAY"], "Enrolled coverages are recorded.");

        }

        [Test]
        public void FillCoverageColumnsSetsJuneOnlyForOneMonthsCoverage()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("JAN", typeof(String));
            dataTable.Columns.Add("FEB", typeof(String));
            dataTable.Columns.Add("MAR", typeof(String));
            dataTable.Columns.Add("APR", typeof(String));
            dataTable.Columns.Add("MAY", typeof(String));
            dataTable.Columns.Add("JUN", typeof(String));
            dataTable.Columns.Add("JUL", typeof(String));
            dataTable.Columns.Add("AUG", typeof(String));
            dataTable.Columns.Add("SEP", typeof(String));
            dataTable.Columns.Add("OCT", typeof(String));
            dataTable.Columns.Add("NOV", typeof(String));
            dataTable.Columns.Add("DEC", typeof(String));
            dataTable.Columns.Add("TOTAL", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            legacyConverterService.FillCoverageColumns(dataRow, DateTime.Parse("2016-06-01"), DateTime.Parse("2016-06-30"), 2016);

            Assert.AreEqual("1", dataTable.Rows[0]["JUN"], "Enrolled coverages are recorded.");

        }

        [Test]
        public void FillCoverageColumnsSetsJulyOnlyForOneMonthsCoverage()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("JAN", typeof(String));
            dataTable.Columns.Add("FEB", typeof(String));
            dataTable.Columns.Add("MAR", typeof(String));
            dataTable.Columns.Add("APR", typeof(String));
            dataTable.Columns.Add("MAY", typeof(String));
            dataTable.Columns.Add("JUN", typeof(String));
            dataTable.Columns.Add("JUL", typeof(String));
            dataTable.Columns.Add("AUG", typeof(String));
            dataTable.Columns.Add("SEP", typeof(String));
            dataTable.Columns.Add("OCT", typeof(String));
            dataTable.Columns.Add("NOV", typeof(String));
            dataTable.Columns.Add("DEC", typeof(String));
            dataTable.Columns.Add("TOTAL", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            legacyConverterService.FillCoverageColumns(dataRow, DateTime.Parse("2016-07-01"), DateTime.Parse("2016-07-31"), 2016);

            Assert.AreEqual("1", dataTable.Rows[0]["JUL"], "Enrolled coverages are recorded.");

        }

        [Test]
        public void FillCoverageColumnsSetsAugustOnlyForOneMonthsCoverage()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("JAN", typeof(String));
            dataTable.Columns.Add("FEB", typeof(String));
            dataTable.Columns.Add("MAR", typeof(String));
            dataTable.Columns.Add("APR", typeof(String));
            dataTable.Columns.Add("MAY", typeof(String));
            dataTable.Columns.Add("JUN", typeof(String));
            dataTable.Columns.Add("JUL", typeof(String));
            dataTable.Columns.Add("AUG", typeof(String));
            dataTable.Columns.Add("SEP", typeof(String));
            dataTable.Columns.Add("OCT", typeof(String));
            dataTable.Columns.Add("NOV", typeof(String));
            dataTable.Columns.Add("DEC", typeof(String));
            dataTable.Columns.Add("TOTAL", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            legacyConverterService.FillCoverageColumns(dataRow, DateTime.Parse("2016-08-01"), DateTime.Parse("2016-08-31"), 2016);

            Assert.AreEqual("1", dataTable.Rows[0]["AUG"], "Enrolled coverages are recorded.");

        }

        [Test]
        public void FillCoverageColumnsSetsSeptemberOnlyForOneMonthsCoverage()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("JAN", typeof(String));
            dataTable.Columns.Add("FEB", typeof(String));
            dataTable.Columns.Add("MAR", typeof(String));
            dataTable.Columns.Add("APR", typeof(String));
            dataTable.Columns.Add("MAY", typeof(String));
            dataTable.Columns.Add("JUN", typeof(String));
            dataTable.Columns.Add("JUL", typeof(String));
            dataTable.Columns.Add("AUG", typeof(String));
            dataTable.Columns.Add("SEP", typeof(String));
            dataTable.Columns.Add("OCT", typeof(String));
            dataTable.Columns.Add("NOV", typeof(String));
            dataTable.Columns.Add("DEC", typeof(String));
            dataTable.Columns.Add("TOTAL", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            legacyConverterService.FillCoverageColumns(dataRow, DateTime.Parse("2016-09-01"), DateTime.Parse("2016-09-30"), 2016);

            Assert.AreEqual("1", dataTable.Rows[0]["SEP"], "Enrolled coverages are recorded.");

        }

        [Test]
        public void FillCoverageColumnsSetsOctoberOnlyForOneMonthsCoverage()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("JAN", typeof(String));
            dataTable.Columns.Add("FEB", typeof(String));
            dataTable.Columns.Add("MAR", typeof(String));
            dataTable.Columns.Add("APR", typeof(String));
            dataTable.Columns.Add("MAY", typeof(String));
            dataTable.Columns.Add("JUN", typeof(String));
            dataTable.Columns.Add("JUL", typeof(String));
            dataTable.Columns.Add("AUG", typeof(String));
            dataTable.Columns.Add("SEP", typeof(String));
            dataTable.Columns.Add("OCT", typeof(String));
            dataTable.Columns.Add("NOV", typeof(String));
            dataTable.Columns.Add("DEC", typeof(String));
            dataTable.Columns.Add("TOTAL", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            legacyConverterService.FillCoverageColumns(dataRow, DateTime.Parse("2016-10-01"), DateTime.Parse("2016-10-31"), 2016);

            Assert.AreEqual("1", dataTable.Rows[0]["OCT"], "Enrolled coverages are recorded.");

        }

        [Test]
        public void FillCoverageColumnsSetsNovemberOnlyForOneMonthsCoverage()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("JAN", typeof(String));
            dataTable.Columns.Add("FEB", typeof(String));
            dataTable.Columns.Add("MAR", typeof(String));
            dataTable.Columns.Add("APR", typeof(String));
            dataTable.Columns.Add("MAY", typeof(String));
            dataTable.Columns.Add("JUN", typeof(String));
            dataTable.Columns.Add("JUL", typeof(String));
            dataTable.Columns.Add("AUG", typeof(String));
            dataTable.Columns.Add("SEP", typeof(String));
            dataTable.Columns.Add("OCT", typeof(String));
            dataTable.Columns.Add("NOV", typeof(String));
            dataTable.Columns.Add("DEC", typeof(String));
            dataTable.Columns.Add("TOTAL", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            legacyConverterService.FillCoverageColumns(dataRow, DateTime.Parse("2016-11-01"), DateTime.Parse("2016-11-30"), 2016);

            Assert.AreEqual("1", dataTable.Rows[0]["NOV"], "Enrolled coverages are recorded.");

        }

        [Test]
        public void FillCoverageColumnsSetsDecemberOnlyForOneMonthsCoverage()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("JAN", typeof(String));
            dataTable.Columns.Add("FEB", typeof(String));
            dataTable.Columns.Add("MAR", typeof(String));
            dataTable.Columns.Add("APR", typeof(String));
            dataTable.Columns.Add("MAY", typeof(String));
            dataTable.Columns.Add("JUN", typeof(String));
            dataTable.Columns.Add("JUL", typeof(String));
            dataTable.Columns.Add("AUG", typeof(String));
            dataTable.Columns.Add("SEP", typeof(String));
            dataTable.Columns.Add("OCT", typeof(String));
            dataTable.Columns.Add("NOV", typeof(String));
            dataTable.Columns.Add("DEC", typeof(String));
            dataTable.Columns.Add("TOTAL", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            legacyConverterService.FillCoverageColumns(dataRow, DateTime.Parse("2016-12-01"), DateTime.Parse("2016-12-31"), 2016);

            Assert.AreEqual("1", dataTable.Rows[0]["DEC"], "Enrolled coverages are recorded.");

        }

        [Test]
        public void MorphFileIntoAFcomplyCoverageStructureWillFillInCoverageMonthsWholeYear()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("OFFER_ROW_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EMPLOYER_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EMPLOYEE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_PLANYEAR_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EETYPE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_NAME", typeof(String));
            dataTable.Columns.Add("OFFER_CLASS_ID", typeof(String));
            dataTable.Columns.Add("OFFER_AVERAGE_HOURS", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_INSURANCE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_CONTRIBUTION_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EFFECTIVE_DATE", typeof(String));
            dataTable.Columns.Add("OFFER_HRA_FLEX", typeof(String));
            dataTable.Columns.Add("Coverage Date Start", typeof(String));
            dataTable.Columns.Add("Coverage Date End", typeof(String));
            dataTable.Columns.Add("Member SSN", typeof(String));
            dataTable.Columns.Add("Subscriber SSN", typeof(String));
            dataTable.Columns.Add("Dependent SSN", typeof(String));
            dataTable.Columns.Add("EnrollStatus", typeof(String));
            dataTable.Columns.Add("Accepted", typeof(String));
            dataTable.Columns.Add("Offered", typeof(String));
            dataTable.Columns.Add("Offered On", typeof(String));
            dataTable.Columns.Add("Accepted/Declined On", typeof(String));
            dataTable.Columns.Add("InsuredMember", typeof(String));
            dataTable.Columns.Add("CoveredName", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("LAST NAME", typeof(String));
            dataTable.Columns.Add("FIRST NAME", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("JAN", typeof(String));
            dataTable.Columns.Add("FEB", typeof(String));
            dataTable.Columns.Add("MAR", typeof(String));
            dataTable.Columns.Add("APR", typeof(String));
            dataTable.Columns.Add("MAY", typeof(String));
            dataTable.Columns.Add("JUN", typeof(String));
            dataTable.Columns.Add("JUL", typeof(String));
            dataTable.Columns.Add("AUG", typeof(String));
            dataTable.Columns.Add("SEP", typeof(String));
            dataTable.Columns.Add("OCT", typeof(String));
            dataTable.Columns.Add("NOV", typeof(String));
            dataTable.Columns.Add("DEC", typeof(String));
            dataTable.Columns.Add("TOTAL", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["SSN"] = "000056789";
            dataRow["InsuredMember"] = "E";
            dataRow["EnrollStatus"] = "E";
            dataRow["Accepted/Declined On"] = "2016-01-01";
            dataRow["Coverage Date Start"] = "2016-01-01";
            dataRow["Coverage Date End"] = "2016-12-31";
            dataTable.Rows.Add(dataRow);

            legacyConverterService.MorphFileIntoAFcomplyCoverageStructure(dataTable, 2016);

            Assert.AreEqual("1", dataTable.Rows[0]["JAN"], "Enrolled coverages are recorded.");
            Assert.AreEqual("1", dataTable.Rows[0]["FEB"], "Enrolled coverages are recorded.");
            Assert.AreEqual("1", dataTable.Rows[0]["MAR"], "Enrolled coverages are recorded.");
            Assert.AreEqual("1", dataTable.Rows[0]["APR"], "Enrolled coverages are recorded.");
            Assert.AreEqual("1", dataTable.Rows[0]["MAY"], "Enrolled coverages are recorded.");
            Assert.AreEqual("1", dataTable.Rows[0]["JUN"], "Enrolled coverages are recorded.");
            Assert.AreEqual("1", dataTable.Rows[0]["JUL"], "Enrolled coverages are recorded.");
            Assert.AreEqual("1", dataTable.Rows[0]["AUG"], "Enrolled coverages are recorded.");
            Assert.AreEqual("1", dataTable.Rows[0]["SEP"], "Enrolled coverages are recorded.");
            Assert.AreEqual("1", dataTable.Rows[0]["OCT"], "Enrolled coverages are recorded.");
            Assert.AreEqual("1", dataTable.Rows[0]["NOV"], "Enrolled coverages are recorded.");
            Assert.AreEqual("1", dataTable.Rows[0]["DEC"], "Enrolled coverages are recorded.");

        }

        [Test]
        public void MorphFileIntoAFcomplyCoverageStructureDependentSSNGoesToSSNField()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("OFFER_ROW_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EMPLOYER_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EMPLOYEE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_PLANYEAR_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EETYPE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_NAME", typeof(String));
            dataTable.Columns.Add("OFFER_CLASS_ID", typeof(String));
            dataTable.Columns.Add("OFFER_AVERAGE_HOURS", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_INSURANCE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_CONTRIBUTION_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EFFECTIVE_DATE", typeof(String));
            dataTable.Columns.Add("OFFER_HRA_FLEX", typeof(String));
            dataTable.Columns.Add("Coverage Date Start", typeof(String));
            dataTable.Columns.Add("Coverage Date End", typeof(String));
            dataTable.Columns.Add("Member SSN", typeof(String));
            dataTable.Columns.Add("Subscriber SSN", typeof(String));
            dataTable.Columns.Add("Dependent SSN", typeof(String));
            dataTable.Columns.Add("EnrollStatus", typeof(String));
            dataTable.Columns.Add("Accepted", typeof(String));
            dataTable.Columns.Add("Offered", typeof(String));
            dataTable.Columns.Add("Offered On", typeof(String));
            dataTable.Columns.Add("Accepted/Declined On", typeof(String));
            dataTable.Columns.Add("InsuredMember", typeof(String));
            dataTable.Columns.Add("CoveredName", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("LAST NAME", typeof(String));
            dataTable.Columns.Add("FIRST NAME", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("JAN", typeof(String));
            dataTable.Columns.Add("FEB", typeof(String));
            dataTable.Columns.Add("MAR", typeof(String));
            dataTable.Columns.Add("APR", typeof(String));
            dataTable.Columns.Add("MAY", typeof(String));
            dataTable.Columns.Add("JUN", typeof(String));
            dataTable.Columns.Add("JUL", typeof(String));
            dataTable.Columns.Add("AUG", typeof(String));
            dataTable.Columns.Add("SEP", typeof(String));
            dataTable.Columns.Add("OCT", typeof(String));
            dataTable.Columns.Add("NOV", typeof(String));
            dataTable.Columns.Add("DEC", typeof(String));
            dataTable.Columns.Add("TOTAL", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["Dependent SSN"] = "000056789";
            dataRow["InsuredMember"] = "D";
            dataRow["EnrollStatus"] = "E";
            dataRow["Accepted/Declined On"] = "2016-01-01";
            dataRow["Coverage Date Start"] = "2016-01-01";
            dataRow["Coverage Date End"] = "2016-12-31";
            dataTable.Rows.Add(dataRow);

            legacyConverterService.MorphFileIntoAFcomplyCoverageStructure(dataTable, 2016);

            Assert.AreEqual("000056789", dataTable.Rows[0]["SSN"], "Dependent SSN's go in the SSN fields.");

        }

        [Test]
        public void MorphFileIntoAFcomplyCoverageStructureSubscriberSSNGoesToSSNField()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("OFFER_ROW_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EMPLOYER_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EMPLOYEE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_PLANYEAR_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EETYPE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_NAME", typeof(String));
            dataTable.Columns.Add("OFFER_CLASS_ID", typeof(String));
            dataTable.Columns.Add("OFFER_AVERAGE_HOURS", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_INSURANCE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_CONTRIBUTION_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EFFECTIVE_DATE", typeof(String));
            dataTable.Columns.Add("OFFER_HRA_FLEX", typeof(String));
            dataTable.Columns.Add("Coverage Date Start", typeof(String));
            dataTable.Columns.Add("Coverage Date End", typeof(String));
            dataTable.Columns.Add("Member SSN", typeof(String));
            dataTable.Columns.Add("Subscriber SSN", typeof(String));
            dataTable.Columns.Add("Dependent SSN", typeof(String));
            dataTable.Columns.Add("EnrollStatus", typeof(String));
            dataTable.Columns.Add("Accepted", typeof(String));
            dataTable.Columns.Add("Offered", typeof(String));
            dataTable.Columns.Add("Offered On", typeof(String));
            dataTable.Columns.Add("Accepted/Declined On", typeof(String));
            dataTable.Columns.Add("InsuredMember", typeof(String));
            dataTable.Columns.Add("CoveredName", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("LAST NAME", typeof(String));
            dataTable.Columns.Add("FIRST NAME", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("JAN", typeof(String));
            dataTable.Columns.Add("FEB", typeof(String));
            dataTable.Columns.Add("MAR", typeof(String));
            dataTable.Columns.Add("APR", typeof(String));
            dataTable.Columns.Add("MAY", typeof(String));
            dataTable.Columns.Add("JUN", typeof(String));
            dataTable.Columns.Add("JUL", typeof(String));
            dataTable.Columns.Add("AUG", typeof(String));
            dataTable.Columns.Add("SEP", typeof(String));
            dataTable.Columns.Add("OCT", typeof(String));
            dataTable.Columns.Add("NOV", typeof(String));
            dataTable.Columns.Add("DEC", typeof(String));
            dataTable.Columns.Add("TOTAL", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["Subscriber SSN"] = "000056789";
            dataRow["InsuredMember"] = "E";
            dataRow["EnrollStatus"] = "E";
            dataRow["Accepted/Declined On"] = "2016-01-01";
            dataRow["Coverage Date Start"] = "2016-01-01";
            dataRow["Coverage Date End"] = "2016-12-31";
            dataTable.Rows.Add(dataRow);

            legacyConverterService.MorphFileIntoAFcomplyCoverageStructure(dataTable, 2016);

            Assert.AreEqual("000056789", dataTable.Rows[0]["SSN"], "Subscriber SSN's go in the SSN fields.");

        }

        [Test]
        public void MorphFileIntoAFcomplyCoverageStructureSubscriberSSNGoesToSUBIDFieldEmployee()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("OFFER_ROW_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EMPLOYER_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EMPLOYEE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_PLANYEAR_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EETYPE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_NAME", typeof(String));
            dataTable.Columns.Add("OFFER_CLASS_ID", typeof(String));
            dataTable.Columns.Add("OFFER_AVERAGE_HOURS", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_INSURANCE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_CONTRIBUTION_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EFFECTIVE_DATE", typeof(String));
            dataTable.Columns.Add("OFFER_HRA_FLEX", typeof(String));
            dataTable.Columns.Add("Coverage Date Start", typeof(String));
            dataTable.Columns.Add("Coverage Date End", typeof(String));
            dataTable.Columns.Add("Member SSN", typeof(String));
            dataTable.Columns.Add("Subscriber SSN", typeof(String));
            dataTable.Columns.Add("Dependent SSN", typeof(String));
            dataTable.Columns.Add("EnrollStatus", typeof(String));
            dataTable.Columns.Add("Accepted", typeof(String));
            dataTable.Columns.Add("Offered", typeof(String));
            dataTable.Columns.Add("Offered On", typeof(String));
            dataTable.Columns.Add("Accepted/Declined On", typeof(String));
            dataTable.Columns.Add("InsuredMember", typeof(String));
            dataTable.Columns.Add("CoveredName", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("LAST NAME", typeof(String));
            dataTable.Columns.Add("FIRST NAME", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("JAN", typeof(String));
            dataTable.Columns.Add("FEB", typeof(String));
            dataTable.Columns.Add("MAR", typeof(String));
            dataTable.Columns.Add("APR", typeof(String));
            dataTable.Columns.Add("MAY", typeof(String));
            dataTable.Columns.Add("JUN", typeof(String));
            dataTable.Columns.Add("JUL", typeof(String));
            dataTable.Columns.Add("AUG", typeof(String));
            dataTable.Columns.Add("SEP", typeof(String));
            dataTable.Columns.Add("OCT", typeof(String));
            dataTable.Columns.Add("NOV", typeof(String));
            dataTable.Columns.Add("DEC", typeof(String));
            dataTable.Columns.Add("TOTAL", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["Subscriber SSN"] = "000056789";
            dataRow["InsuredMember"] = "E";
            dataRow["EnrollStatus"] = "E";
            dataRow["Accepted/Declined On"] = "2016-01-01";
            dataRow["Coverage Date Start"] = "2016-01-01";
            dataRow["Coverage Date End"] = "2016-12-31";
            dataTable.Rows.Add(dataRow);

            legacyConverterService.MorphFileIntoAFcomplyCoverageStructure(dataTable, 2016);

            Assert.AreEqual("000056789", dataTable.Rows[0]["SUBID"], "Subscriber SSN's go in the SUBID fields.");

        }

        [Test]
        public void MorphFileIntoAFcomplyCoverageStructureSubscriberSSNGoesToSUBIDFieldDependent()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("OFFER_ROW_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EMPLOYER_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EMPLOYEE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_PLANYEAR_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EETYPE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_NAME", typeof(String));
            dataTable.Columns.Add("OFFER_CLASS_ID", typeof(String));
            dataTable.Columns.Add("OFFER_AVERAGE_HOURS", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_INSURANCE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_CONTRIBUTION_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EFFECTIVE_DATE", typeof(String));
            dataTable.Columns.Add("OFFER_HRA_FLEX", typeof(String));
            dataTable.Columns.Add("Coverage Date Start", typeof(String));
            dataTable.Columns.Add("Coverage Date End", typeof(String));
            dataTable.Columns.Add("Member SSN", typeof(String));
            dataTable.Columns.Add("Subscriber SSN", typeof(String));
            dataTable.Columns.Add("Dependent SSN", typeof(String));
            dataTable.Columns.Add("EnrollStatus", typeof(String));
            dataTable.Columns.Add("Accepted", typeof(String));
            dataTable.Columns.Add("Offered", typeof(String));
            dataTable.Columns.Add("Offered On", typeof(String));
            dataTable.Columns.Add("Accepted/Declined On", typeof(String));
            dataTable.Columns.Add("InsuredMember", typeof(String));
            dataTable.Columns.Add("CoveredName", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("LAST NAME", typeof(String));
            dataTable.Columns.Add("FIRST NAME", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("JAN", typeof(String));
            dataTable.Columns.Add("FEB", typeof(String));
            dataTable.Columns.Add("MAR", typeof(String));
            dataTable.Columns.Add("APR", typeof(String));
            dataTable.Columns.Add("MAY", typeof(String));
            dataTable.Columns.Add("JUN", typeof(String));
            dataTable.Columns.Add("JUL", typeof(String));
            dataTable.Columns.Add("AUG", typeof(String));
            dataTable.Columns.Add("SEP", typeof(String));
            dataTable.Columns.Add("OCT", typeof(String));
            dataTable.Columns.Add("NOV", typeof(String));
            dataTable.Columns.Add("DEC", typeof(String));
            dataTable.Columns.Add("TOTAL", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["Subscriber SSN"] = "000056789";
            dataRow["InsuredMember"] = "D";
            dataRow["EnrollStatus"] = "E";
            dataRow["Accepted/Declined On"] = "2016-01-01";
            dataRow["Coverage Date Start"] = "2016-01-01";
            dataRow["Coverage Date End"] = "2016-12-31";
            dataTable.Rows.Add(dataRow);

            legacyConverterService.MorphFileIntoAFcomplyCoverageStructure(dataTable, 2016);

            Assert.AreEqual("000056789", dataTable.Rows[0]["SUBID"], "Subscriber SSN's go in the SUBID fields.");

        }

        [Test]
        public void SplitPersonalNameOnSpacesReturnsForSimpleFirstnameLastname()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            String[] names = legacyConverterService.SplitPersonalNameOnSpaces("first last");

            Assert.AreEqual("first", names[0], "Names should match post split.");

            Assert.AreEqual("last", names[1], "Names should match post split.");

        }

        [Test]
        public void SplitPersonalNameOnSpacesReturnsForSimpleFirstnameMiddleInitialLastname()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            String[] names = legacyConverterService.SplitPersonalNameOnSpaces("first M last");

            Assert.AreEqual("first", names[0], "Names should match post split.");

            Assert.AreEqual("last", names[1], "Names should match post split.");

        }

        [Test]
        public void SplitPersonalNameOnSpacesReturnsForReturnsFullMiddleNameAsPartOfLastNameSinceWeLackBetterOptions()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            String[] names = legacyConverterService.SplitPersonalNameOnSpaces("first middle last");

            Assert.AreEqual("first", names[0], "Names should match post split.");

            Assert.AreEqual("middle last", names[1], "Names should match post split.");

        }

        [Test]
        public void SplitPersonalNameOnSpacesReturnsForSimpleFirstnameMiddleInitialLastnameWithExtraInitialSpaces()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            String[] names = legacyConverterService.SplitPersonalNameOnSpaces("first  M last");

            Assert.AreEqual("first", names[0], "Names should match post split.");

            Assert.AreEqual("last", names[1], "Names should match post split.");

        }

        [Test]
        public void SplitPersonalNameOnSpacesReturnsForSimpleFirstnameMiddleInitialLastnameWithExtraTrialingSpaces()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            String[] names = legacyConverterService.SplitPersonalNameOnSpaces("first M  last");

            Assert.AreEqual("first", names[0], "Names should match post split.");

            Assert.AreEqual("last", names[1], "Names should match post split.");

        }

        [Test]
        public void ParsePersonalNameHandlesSpaceDelimitedFirstnameLastname()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            String firstName = String.Empty;
            String lastName = String.Empty;

            legacyConverterService.ParsePersonalName("first last", out firstName, out lastName);

            Assert.AreEqual("first", firstName, "Names should match post split.");

            Assert.AreEqual("last", lastName, "Names should match post split.");

        }

        [Test]
        public void ParsePersonalNameDropsExtraQuotesFromNamesSpaceSplit()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            String firstName = String.Empty;
            String lastName = String.Empty;

            legacyConverterService.ParsePersonalName("\"first last\"", out firstName, out lastName);

            Assert.AreEqual("first", firstName, "Names should match post split.");

            Assert.AreEqual("last", lastName, "Names should match post split.");

        }

        [Test]
        public void SplitPersonalNameOnCommasHandlesLastCommaFirst()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            String[] names = legacyConverterService.SplitPersonalNameOnCommas("last, first");

            Assert.AreEqual("first", names[0], "Names should match post split.");

            Assert.AreEqual("last", names[1], "Names should match post split.");

        }

        [Test]
        public void SplitPersonalNameOnCommasHandlesLastCommaFirstSpaceMiddle()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            String[] names = legacyConverterService.SplitPersonalNameOnCommas("last, first middle");

            Assert.AreEqual("first", names[0], "Names should match post split.");

            Assert.AreEqual("middle last", names[1], "Names should match post split.");

        }

        [Test]
        public void SplitPersonalNameOnCommasHandlesLastCommaFirstSpaceMiddleInitial()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            String[] names = legacyConverterService.SplitPersonalNameOnCommas("last, first m");

            Assert.AreEqual("first", names[0], "Names should match post split.");

            Assert.AreEqual("last", names[1], "Names should match post split.");

        }

        [Test]
        public void ParsePersonalNameHandlesCommaDelimitedLastnameFirstname()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            String firstName = String.Empty;
            String lastName = String.Empty;

            legacyConverterService.ParsePersonalName("last, first", out firstName, out lastName);

            Assert.AreEqual("first", firstName, "Names should match post split.");

            Assert.AreEqual("last", lastName, "Names should match post split.");

        }

        [Test]
        public void ParsePersonalNameDropsExtraQuotesFromNamesCommaSplits()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            String firstName = String.Empty;
            String lastName = String.Empty;

            legacyConverterService.ParsePersonalName("\"last, first\"", out firstName, out lastName);

            Assert.AreEqual("first", firstName, "Names should match post split.");

            Assert.AreEqual("last", lastName, "Names should match post split.");

        }

        [Test]
        public void SplitPersonalNameOnCommasHandlesLastCommaFirstSpaceMiddleInitialLeavesExtraQuotes()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            String[] names = legacyConverterService.SplitPersonalNameOnCommas("\"last, first m\"");

            Assert.AreEqual("first", names[0], "Names should match post split.");

            Assert.AreEqual("m\" \"last", names[1], "Names should match post split.");

        }

        [Test]
        public void SplitPersonalNameOnSpacesLeavesExtraQuotes()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            String[] names = legacyConverterService.SplitPersonalNameOnSpaces("\"first M last\"");

            Assert.AreEqual("\"first", names[0], "Names should match post split.");

            Assert.AreEqual("last\"", names[1], "Names should match post split.");

        }

        [Test]
        public void MorphFileIntoAFcomplyCoverageStructureCoveredNameIsSplitSpaces()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("OFFER_ROW_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EMPLOYER_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EMPLOYEE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_PLANYEAR_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EETYPE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_NAME", typeof(String));
            dataTable.Columns.Add("OFFER_CLASS_ID", typeof(String));
            dataTable.Columns.Add("OFFER_AVERAGE_HOURS", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_INSURANCE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_CONTRIBUTION_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EFFECTIVE_DATE", typeof(String));
            dataTable.Columns.Add("OFFER_HRA_FLEX", typeof(String));
            dataTable.Columns.Add("Coverage Date Start", typeof(String));
            dataTable.Columns.Add("Coverage Date End", typeof(String));
            dataTable.Columns.Add("Member SSN", typeof(String));
            dataTable.Columns.Add("Subscriber SSN", typeof(String));
            dataTable.Columns.Add("Dependent SSN", typeof(String));
            dataTable.Columns.Add("EnrollStatus", typeof(String));
            dataTable.Columns.Add("Accepted", typeof(String));
            dataTable.Columns.Add("Offered", typeof(String));
            dataTable.Columns.Add("Offered On", typeof(String));
            dataTable.Columns.Add("Accepted/Declined On", typeof(String));
            dataTable.Columns.Add("InsuredMember", typeof(String));
            dataTable.Columns.Add("CoveredName", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("LAST NAME", typeof(String));
            dataTable.Columns.Add("FIRST NAME", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("JAN", typeof(String));
            dataTable.Columns.Add("FEB", typeof(String));
            dataTable.Columns.Add("MAR", typeof(String));
            dataTable.Columns.Add("APR", typeof(String));
            dataTable.Columns.Add("MAY", typeof(String));
            dataTable.Columns.Add("JUN", typeof(String));
            dataTable.Columns.Add("JUL", typeof(String));
            dataTable.Columns.Add("AUG", typeof(String));
            dataTable.Columns.Add("SEP", typeof(String));
            dataTable.Columns.Add("OCT", typeof(String));
            dataTable.Columns.Add("NOV", typeof(String));
            dataTable.Columns.Add("DEC", typeof(String));
            dataTable.Columns.Add("TOTAL", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["CoveredName"] = "John Smith";
            dataRow["Subscriber SSN"] = "000056789";
            dataRow["InsuredMember"] = "D";
            dataRow["EnrollStatus"] = "E";
            dataRow["Accepted/Declined On"] = "2016-01-01";
            dataRow["Coverage Date Start"] = "2016-01-01";
            dataRow["Coverage Date End"] = "2016-12-31";
            dataTable.Rows.Add(dataRow);

            legacyConverterService.MorphFileIntoAFcomplyCoverageStructure(dataTable, 2016);

            Assert.AreEqual("John", dataTable.Rows[0]["FIRST NAME"], "Names should be split.");
            Assert.AreEqual("Smith", dataTable.Rows[0]["LAST NAME"], "Names should be split.");

        }

        [Test]
        public void MorphFileIntoAFcomplyCoverageStructureCoveredNameIsSplitCommas()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("OFFER_ROW_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EMPLOYER_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EMPLOYEE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_PLANYEAR_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EETYPE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_NAME", typeof(String));
            dataTable.Columns.Add("OFFER_CLASS_ID", typeof(String));
            dataTable.Columns.Add("OFFER_AVERAGE_HOURS", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_INSURANCE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_CONTRIBUTION_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EFFECTIVE_DATE", typeof(String));
            dataTable.Columns.Add("OFFER_HRA_FLEX", typeof(String));
            dataTable.Columns.Add("Coverage Date Start", typeof(String));
            dataTable.Columns.Add("Coverage Date End", typeof(String));
            dataTable.Columns.Add("Member SSN", typeof(String));
            dataTable.Columns.Add("Subscriber SSN", typeof(String));
            dataTable.Columns.Add("Dependent SSN", typeof(String));
            dataTable.Columns.Add("EnrollStatus", typeof(String));
            dataTable.Columns.Add("Accepted", typeof(String));
            dataTable.Columns.Add("Offered", typeof(String));
            dataTable.Columns.Add("Offered On", typeof(String));
            dataTable.Columns.Add("Accepted/Declined On", typeof(String));
            dataTable.Columns.Add("InsuredMember", typeof(String));
            dataTable.Columns.Add("CoveredName", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("LAST NAME", typeof(String));
            dataTable.Columns.Add("FIRST NAME", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("JAN", typeof(String));
            dataTable.Columns.Add("FEB", typeof(String));
            dataTable.Columns.Add("MAR", typeof(String));
            dataTable.Columns.Add("APR", typeof(String));
            dataTable.Columns.Add("MAY", typeof(String));
            dataTable.Columns.Add("JUN", typeof(String));
            dataTable.Columns.Add("JUL", typeof(String));
            dataTable.Columns.Add("AUG", typeof(String));
            dataTable.Columns.Add("SEP", typeof(String));
            dataTable.Columns.Add("OCT", typeof(String));
            dataTable.Columns.Add("NOV", typeof(String));
            dataTable.Columns.Add("DEC", typeof(String));
            dataTable.Columns.Add("TOTAL", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["CoveredName"] = "SMITH, JOHN";
            dataRow["Subscriber SSN"] = "000056789";
            dataRow["InsuredMember"] = "D";
            dataRow["EnrollStatus"] = "E";
            dataRow["Accepted/Declined On"] = "2016-01-01";
            dataRow["Coverage Date Start"] = "2016-01-01";
            dataRow["Coverage Date End"] = "2016-12-31";
            dataTable.Rows.Add(dataRow);

            legacyConverterService.MorphFileIntoAFcomplyCoverageStructure(dataTable, 2016);

            Assert.AreEqual("JOHN", dataTable.Rows[0]["FIRST NAME"], "Names should be split.");
            Assert.AreEqual("SMITH", dataTable.Rows[0]["LAST NAME"], "Names should be split.");

        }

        [Test]
        public void AddAFcomplyOfferColumnsAddsColumnsMissing()
        {

            DataTable dataTable = new DataTable("CSVTable");


            new LegacyConverterService().AddAFcomplyOfferColumns(dataTable);

            foreach (String columnName in "OFFER_ROW_ID,OFFER_EMPLOYER_ID,OFFER_EMPLOYEE_ID,OFFER_PLANYEAR_ID,OFFER_NAME,OFFER_CLASS_ID,OFFER_AVERAGE_HOURS,OFFER_OFFERED,OFFER_OFFERED_ON,OFFER_ACCEPTED,OFFER_ACCEPTED_ON,OFFER_INSURANCE_ID,OFFER_CONTRIBUTION_ID,OFFER_EFFECTIVE_DATE,OFFER_HRA_FLEX".Split(','))
            {
                Assert.IsTrue(dataTable.Columns.Contains(columnName), String.Format("Requested Column {0} should be present.", columnName));
            }

        }

        [Test]
        public void MorphFileIntoAFcomplyCoverageStructureShouldLeaveOfferDataFieldsBlank()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("OFFER_ROW_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EMPLOYER_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EMPLOYEE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_PLANYEAR_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EETYPE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_NAME", typeof(String));
            dataTable.Columns.Add("OFFER_CLASS_ID", typeof(String));
            dataTable.Columns.Add("OFFER_AVERAGE_HOURS", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_INSURANCE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_CONTRIBUTION_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EFFECTIVE_DATE", typeof(String));
            dataTable.Columns.Add("OFFER_HRA_FLEX", typeof(String));
            dataTable.Columns.Add("Coverage Date Start", typeof(String));
            dataTable.Columns.Add("Coverage Date End", typeof(String));
            dataTable.Columns.Add("Member SSN", typeof(String));
            dataTable.Columns.Add("Subscriber SSN", typeof(String));
            dataTable.Columns.Add("Dependent SSN", typeof(String));
            dataTable.Columns.Add("EnrollStatus", typeof(String));
            dataTable.Columns.Add("Accepted", typeof(String));
            dataTable.Columns.Add("Offered", typeof(String));
            dataTable.Columns.Add("Offered On", typeof(String));
            dataTable.Columns.Add("Accepted/Declined On", typeof(String));
            dataTable.Columns.Add("InsuredMember", typeof(String));
            dataTable.Columns.Add("CoveredName", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("LAST NAME", typeof(String));
            dataTable.Columns.Add("FIRST NAME", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("JAN", typeof(String));
            dataTable.Columns.Add("FEB", typeof(String));
            dataTable.Columns.Add("MAR", typeof(String));
            dataTable.Columns.Add("APR", typeof(String));
            dataTable.Columns.Add("MAY", typeof(String));
            dataTable.Columns.Add("JUN", typeof(String));
            dataTable.Columns.Add("JUL", typeof(String));
            dataTable.Columns.Add("AUG", typeof(String));
            dataTable.Columns.Add("SEP", typeof(String));
            dataTable.Columns.Add("OCT", typeof(String));
            dataTable.Columns.Add("NOV", typeof(String));
            dataTable.Columns.Add("DEC", typeof(String));
            dataTable.Columns.Add("TOTAL", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["CoveredName"] = "John Smith";
            dataRow["Subscriber SSN"] = "000056789";
            dataRow["InsuredMember"] = "D";
            dataRow["EnrollStatus"] = "E";
            dataRow["Accepted/Declined On"] = "2016-01-01";
            dataRow["Coverage Date Start"] = "2016-01-01";
            dataRow["Coverage Date End"] = "2016-12-31";
            dataTable.Rows.Add(dataRow);

            legacyConverterService.MorphFileIntoAFcomplyCoverageStructure(dataTable, 2016);

            String[] columnNames = "OFFER_ROW_ID,OFFER_EMPLOYER_ID,OFFER_EMPLOYEE_ID,OFFER_PLANYEAR_ID,OFFER_EETYPE_ID,OFFER_CLASS_ID,OFFER_AVERAGE_HOURS,OFFER_INSURANCE_ID,OFFER_CONTRIBUTION_ID".Split(',');

            foreach(String columnName in columnNames)
            {
                Assert.AreEqual(String.Empty, dataTable.Rows[0][columnName], String.Format("Column {0} should be blank, but is not.", columnName));
            }

        }

        [Test]
        public void MorphFileIntoAFcomplyCoverageStructureShouldFillInOfferEffectiveDateWithStartDateIfAccepted()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("OFFER_ROW_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EMPLOYER_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EMPLOYEE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_PLANYEAR_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EETYPE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_NAME", typeof(String));
            dataTable.Columns.Add("OFFER_CLASS_ID", typeof(String));
            dataTable.Columns.Add("OFFER_AVERAGE_HOURS", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_INSURANCE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_CONTRIBUTION_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EFFECTIVE_DATE", typeof(String));
            dataTable.Columns.Add("OFFER_HRA_FLEX", typeof(String));
            dataTable.Columns.Add("Coverage Date Start", typeof(String));
            dataTable.Columns.Add("Coverage Date End", typeof(String));
            dataTable.Columns.Add("Member SSN", typeof(String));
            dataTable.Columns.Add("Subscriber SSN", typeof(String));
            dataTable.Columns.Add("Dependent SSN", typeof(String));
            dataTable.Columns.Add("EnrollStatus", typeof(String));
            dataTable.Columns.Add("Accepted", typeof(String));
            dataTable.Columns.Add("Offered", typeof(String));
            dataTable.Columns.Add("Offered On", typeof(String));
            dataTable.Columns.Add("Accepted/Declined On", typeof(String));
            dataTable.Columns.Add("InsuredMember", typeof(String));
            dataTable.Columns.Add("CoveredName", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("LAST NAME", typeof(String));
            dataTable.Columns.Add("FIRST NAME", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("JAN", typeof(String));
            dataTable.Columns.Add("FEB", typeof(String));
            dataTable.Columns.Add("MAR", typeof(String));
            dataTable.Columns.Add("APR", typeof(String));
            dataTable.Columns.Add("MAY", typeof(String));
            dataTable.Columns.Add("JUN", typeof(String));
            dataTable.Columns.Add("JUL", typeof(String));
            dataTable.Columns.Add("AUG", typeof(String));
            dataTable.Columns.Add("SEP", typeof(String));
            dataTable.Columns.Add("OCT", typeof(String));
            dataTable.Columns.Add("NOV", typeof(String));
            dataTable.Columns.Add("DEC", typeof(String));
            dataTable.Columns.Add("TOTAL", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["CoveredName"] = "John Smith";
            dataRow["Subscriber SSN"] = "000056789";
            dataRow["InsuredMember"] = "D";
            dataRow["EnrollStatus"] = "E";
            dataRow["Accepted/Declined On"] = "2016-01-01";
            dataRow["Coverage Date Start"] = "2016-01-01";
            dataRow["Coverage Date End"] = "2016-12-31";
            dataTable.Rows.Add(dataRow);

            legacyConverterService.MorphFileIntoAFcomplyCoverageStructure(dataTable, 2016);

            Assert.AreEqual(DateTime.Parse(dataTable.Rows[0]["Coverage Date Start"].ToString()), DateTime.Parse(dataTable.Rows[0]["OFFER_EFFECTIVE_DATE"].ToString()), "Should match safe defaults.");

        }

        [Test]
        public void MorphFileIntoAFcomplyCoverageStructureShouldFillInOfferOfferedOnWithStartDate()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("OFFER_ROW_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EMPLOYER_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EMPLOYEE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_PLANYEAR_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EETYPE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_NAME", typeof(String));
            dataTable.Columns.Add("OFFER_CLASS_ID", typeof(String));
            dataTable.Columns.Add("OFFER_AVERAGE_HOURS", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_INSURANCE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_CONTRIBUTION_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EFFECTIVE_DATE", typeof(String));
            dataTable.Columns.Add("OFFER_HRA_FLEX", typeof(String));
            dataTable.Columns.Add("Coverage Date Start", typeof(String));
            dataTable.Columns.Add("Coverage Date End", typeof(String));
            dataTable.Columns.Add("Member SSN", typeof(String));
            dataTable.Columns.Add("Subscriber SSN", typeof(String));
            dataTable.Columns.Add("Dependent SSN", typeof(String));
            dataTable.Columns.Add("EnrollStatus", typeof(String));
            dataTable.Columns.Add("Accepted", typeof(String));
            dataTable.Columns.Add("Offered", typeof(String));
            dataTable.Columns.Add("Offered On", typeof(String));
            dataTable.Columns.Add("Accepted/Declined On", typeof(String));
            dataTable.Columns.Add("InsuredMember", typeof(String));
            dataTable.Columns.Add("CoveredName", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("LAST NAME", typeof(String));
            dataTable.Columns.Add("FIRST NAME", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("JAN", typeof(String));
            dataTable.Columns.Add("FEB", typeof(String));
            dataTable.Columns.Add("MAR", typeof(String));
            dataTable.Columns.Add("APR", typeof(String));
            dataTable.Columns.Add("MAY", typeof(String));
            dataTable.Columns.Add("JUN", typeof(String));
            dataTable.Columns.Add("JUL", typeof(String));
            dataTable.Columns.Add("AUG", typeof(String));
            dataTable.Columns.Add("SEP", typeof(String));
            dataTable.Columns.Add("OCT", typeof(String));
            dataTable.Columns.Add("NOV", typeof(String));
            dataTable.Columns.Add("DEC", typeof(String));
            dataTable.Columns.Add("TOTAL", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["CoveredName"] = "John Smith";
            dataRow["Subscriber SSN"] = "000056789";
            dataRow["InsuredMember"] = "D";
            dataRow["EnrollStatus"] = "E";
            dataRow["Accepted/Declined On"] = "2016-01-01";
            dataRow["Coverage Date Start"] = "2016-01-01";
            dataRow["Coverage Date End"] = "2016-12-31";
            dataTable.Rows.Add(dataRow);

            legacyConverterService.MorphFileIntoAFcomplyCoverageStructure(dataTable, 2016);

            Assert.AreEqual(DateTime.Parse(dataTable.Rows[0]["Coverage Date Start"].ToString()), DateTime.Parse(dataTable.Rows[0]["OFFER_OFFERED_ON"].ToString()), "Should match safe defaults.");

        }

        [Test]
        public void MorphFileIntoAFcomplyCoverageStructureShouldFillInOfferAcceptedOnWithStartDate()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("OFFER_ROW_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EMPLOYER_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EMPLOYEE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_PLANYEAR_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EETYPE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_NAME", typeof(String));
            dataTable.Columns.Add("OFFER_CLASS_ID", typeof(String));
            dataTable.Columns.Add("OFFER_AVERAGE_HOURS", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_INSURANCE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_CONTRIBUTION_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EFFECTIVE_DATE", typeof(String));
            dataTable.Columns.Add("OFFER_HRA_FLEX", typeof(String));
            dataTable.Columns.Add("Coverage Date Start", typeof(String));
            dataTable.Columns.Add("Coverage Date End", typeof(String));
            dataTable.Columns.Add("Member SSN", typeof(String));
            dataTable.Columns.Add("Subscriber SSN", typeof(String));
            dataTable.Columns.Add("Dependent SSN", typeof(String));
            dataTable.Columns.Add("EnrollStatus", typeof(String));
            dataTable.Columns.Add("Accepted", typeof(String));
            dataTable.Columns.Add("Offered", typeof(String));
            dataTable.Columns.Add("Offered On", typeof(String));
            dataTable.Columns.Add("Accepted/Declined On", typeof(String));
            dataTable.Columns.Add("InsuredMember", typeof(String));
            dataTable.Columns.Add("CoveredName", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("LAST NAME", typeof(String));
            dataTable.Columns.Add("FIRST NAME", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("JAN", typeof(String));
            dataTable.Columns.Add("FEB", typeof(String));
            dataTable.Columns.Add("MAR", typeof(String));
            dataTable.Columns.Add("APR", typeof(String));
            dataTable.Columns.Add("MAY", typeof(String));
            dataTable.Columns.Add("JUN", typeof(String));
            dataTable.Columns.Add("JUL", typeof(String));
            dataTable.Columns.Add("AUG", typeof(String));
            dataTable.Columns.Add("SEP", typeof(String));
            dataTable.Columns.Add("OCT", typeof(String));
            dataTable.Columns.Add("NOV", typeof(String));
            dataTable.Columns.Add("DEC", typeof(String));
            dataTable.Columns.Add("TOTAL", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["CoveredName"] = "John Smith";
            dataRow["Subscriber SSN"] = "000056789";
            dataRow["InsuredMember"] = "D";
            dataRow["EnrollStatus"] = "E";
            dataRow["Accepted/Declined On"] = "2016-01-01";
            dataRow["Coverage Date Start"] = "2016-01-01";
            dataRow["Coverage Date End"] = "2016-12-31";
            dataTable.Rows.Add(dataRow);

            legacyConverterService.MorphFileIntoAFcomplyCoverageStructure(dataTable, 2016);

            Assert.AreEqual(DateTime.Parse(dataTable.Rows[0]["Coverage Date Start"].ToString()), DateTime.Parse(dataTable.Rows[0]["OFFER_ACCEPTED_ON"].ToString()), "Should match safe defaults.");

        }

        [Test]
        public void MorphFileIntoAFcomplyCoverageStructureShouldFillInOfferEffectiveDateWithStartDateIfWaived()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("OFFER_ROW_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EMPLOYER_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EMPLOYEE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_PLANYEAR_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EETYPE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_NAME", typeof(String));
            dataTable.Columns.Add("OFFER_CLASS_ID", typeof(String));
            dataTable.Columns.Add("OFFER_AVERAGE_HOURS", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_INSURANCE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_CONTRIBUTION_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EFFECTIVE_DATE", typeof(String));
            dataTable.Columns.Add("OFFER_HRA_FLEX", typeof(String));
            dataTable.Columns.Add("Coverage Date Start", typeof(String));
            dataTable.Columns.Add("Coverage Date End", typeof(String));
            dataTable.Columns.Add("Member SSN", typeof(String));
            dataTable.Columns.Add("Subscriber SSN", typeof(String));
            dataTable.Columns.Add("Dependent SSN", typeof(String));
            dataTable.Columns.Add("EnrollStatus", typeof(String));
            dataTable.Columns.Add("Accepted", typeof(String));
            dataTable.Columns.Add("Offered", typeof(String));
            dataTable.Columns.Add("Offered On", typeof(String));
            dataTable.Columns.Add("Accepted/Declined On", typeof(String));
            dataTable.Columns.Add("InsuredMember", typeof(String));
            dataTable.Columns.Add("CoveredName", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("LAST NAME", typeof(String));
            dataTable.Columns.Add("FIRST NAME", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("JAN", typeof(String));
            dataTable.Columns.Add("FEB", typeof(String));
            dataTable.Columns.Add("MAR", typeof(String));
            dataTable.Columns.Add("APR", typeof(String));
            dataTable.Columns.Add("MAY", typeof(String));
            dataTable.Columns.Add("JUN", typeof(String));
            dataTable.Columns.Add("JUL", typeof(String));
            dataTable.Columns.Add("AUG", typeof(String));
            dataTable.Columns.Add("SEP", typeof(String));
            dataTable.Columns.Add("OCT", typeof(String));
            dataTable.Columns.Add("NOV", typeof(String));
            dataTable.Columns.Add("DEC", typeof(String));
            dataTable.Columns.Add("TOTAL", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["CoveredName"] = "John Smith";
            dataRow["Subscriber SSN"] = "000056789";
            dataRow["InsuredMember"] = "D";
            dataRow["EnrollStatus"] = "W";
            dataRow["Accepted/Declined On"] = "2016-01-01";
            dataRow["Coverage Date Start"] = "2016-01-01";
            dataRow["Coverage Date End"] = "2016-12-31";
            dataTable.Rows.Add(dataRow);

            legacyConverterService.MorphFileIntoAFcomplyCoverageStructure(dataTable, 2016);

            Assert.AreEqual(DateTime.Parse(dataTable.Rows[0]["Coverage Date Start"].ToString()), DateTime.Parse(dataTable.Rows[0]["OFFER_EFFECTIVE_DATE"].ToString()), "Should match safe defaults.");

        }

        [Test]
        public void MorphFileIntoAFcomplyCoverageStructureShouldFillInOfferHraFlexDefaultIfWaived()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("OFFER_ROW_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EMPLOYER_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EMPLOYEE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_PLANYEAR_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EETYPE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_NAME", typeof(String));
            dataTable.Columns.Add("OFFER_CLASS_ID", typeof(String));
            dataTable.Columns.Add("OFFER_AVERAGE_HOURS", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_INSURANCE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_CONTRIBUTION_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EFFECTIVE_DATE", typeof(String));
            dataTable.Columns.Add("OFFER_HRA_FLEX", typeof(String));
            dataTable.Columns.Add("Coverage Date Start", typeof(String));
            dataTable.Columns.Add("Coverage Date End", typeof(String));
            dataTable.Columns.Add("Member SSN", typeof(String));
            dataTable.Columns.Add("Subscriber SSN", typeof(String));
            dataTable.Columns.Add("Dependent SSN", typeof(String));
            dataTable.Columns.Add("EnrollStatus", typeof(String));
            dataTable.Columns.Add("Accepted", typeof(String));
            dataTable.Columns.Add("Offered", typeof(String));
            dataTable.Columns.Add("Offered On", typeof(String));
            dataTable.Columns.Add("Accepted/Declined On", typeof(String));
            dataTable.Columns.Add("InsuredMember", typeof(String));
            dataTable.Columns.Add("CoveredName", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("LAST NAME", typeof(String));
            dataTable.Columns.Add("FIRST NAME", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("JAN", typeof(String));
            dataTable.Columns.Add("FEB", typeof(String));
            dataTable.Columns.Add("MAR", typeof(String));
            dataTable.Columns.Add("APR", typeof(String));
            dataTable.Columns.Add("MAY", typeof(String));
            dataTable.Columns.Add("JUN", typeof(String));
            dataTable.Columns.Add("JUL", typeof(String));
            dataTable.Columns.Add("AUG", typeof(String));
            dataTable.Columns.Add("SEP", typeof(String));
            dataTable.Columns.Add("OCT", typeof(String));
            dataTable.Columns.Add("NOV", typeof(String));
            dataTable.Columns.Add("DEC", typeof(String));
            dataTable.Columns.Add("TOTAL", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["CoveredName"] = "John Smith";
            dataRow["Subscriber SSN"] = "000056789";
            dataRow["InsuredMember"] = "D";
            dataRow["EnrollStatus"] = "W";
            dataRow["Accepted/Declined On"] = "2016-01-01";
            dataRow["Coverage Date Start"] = "2016-01-01";
            dataRow["Coverage Date End"] = "2016-12-31";
            dataTable.Rows.Add(dataRow);

            legacyConverterService.MorphFileIntoAFcomplyCoverageStructure(dataTable, 2016);

            Assert.AreEqual("0.00", dataTable.Rows[0]["OFFER_HRA_FLEX"].ToString(), "Should match safe defaults.");

        }

        [Test]
        public void MorphFileIntoAFcomplyCoverageStructureShouldFillInOfferHraFlexDefaultIfAccepted()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("OFFER_ROW_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EMPLOYER_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EMPLOYEE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_PLANYEAR_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EETYPE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_NAME", typeof(String));
            dataTable.Columns.Add("OFFER_CLASS_ID", typeof(String));
            dataTable.Columns.Add("OFFER_AVERAGE_HOURS", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_INSURANCE_ID", typeof(String));
            dataTable.Columns.Add("OFFER_CONTRIBUTION_ID", typeof(String));
            dataTable.Columns.Add("OFFER_EFFECTIVE_DATE", typeof(String));
            dataTable.Columns.Add("OFFER_HRA_FLEX", typeof(String));
            dataTable.Columns.Add("Coverage Date Start", typeof(String));
            dataTable.Columns.Add("Coverage Date End", typeof(String));
            dataTable.Columns.Add("Member SSN", typeof(String));
            dataTable.Columns.Add("Subscriber SSN", typeof(String));
            dataTable.Columns.Add("Dependent SSN", typeof(String));
            dataTable.Columns.Add("EnrollStatus", typeof(String));
            dataTable.Columns.Add("Accepted", typeof(String));
            dataTable.Columns.Add("Offered", typeof(String));
            dataTable.Columns.Add("Offered On", typeof(String));
            dataTable.Columns.Add("Accepted/Declined On", typeof(String));
            dataTable.Columns.Add("InsuredMember", typeof(String));
            dataTable.Columns.Add("CoveredName", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("LAST NAME", typeof(String));
            dataTable.Columns.Add("FIRST NAME", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("JAN", typeof(String));
            dataTable.Columns.Add("FEB", typeof(String));
            dataTable.Columns.Add("MAR", typeof(String));
            dataTable.Columns.Add("APR", typeof(String));
            dataTable.Columns.Add("MAY", typeof(String));
            dataTable.Columns.Add("JUN", typeof(String));
            dataTable.Columns.Add("JUL", typeof(String));
            dataTable.Columns.Add("AUG", typeof(String));
            dataTable.Columns.Add("SEP", typeof(String));
            dataTable.Columns.Add("OCT", typeof(String));
            dataTable.Columns.Add("NOV", typeof(String));
            dataTable.Columns.Add("DEC", typeof(String));
            dataTable.Columns.Add("TOTAL", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["CoveredName"] = "John Smith";
            dataRow["Subscriber SSN"] = "000056789";
            dataRow["InsuredMember"] = "D";
            dataRow["EnrollStatus"] = "E";
            dataRow["Accepted/Declined On"] = "2016-01-01";
            dataRow["Coverage Date Start"] = "2016-01-01";
            dataRow["Coverage Date End"] = "2016-12-31";
            dataTable.Rows.Add(dataRow);

            legacyConverterService.MorphFileIntoAFcomplyCoverageStructure(dataTable, 2016);

            Assert.AreEqual("0.00", dataTable.Rows[0]["OFFER_HRA_FLEX"].ToString(), "Should match safe defaults.");

        }

        [Test]
        public void ShouldOfferBeRejectedShouldBeTrueIfSSNInTheList()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["SSN"] = "ABCDEFGHI";
            dataTable.Rows.Add(dataRow);

            IList<String> rejectedSocials = new List<String>();
            rejectedSocials.Add("ABCDEFGHI");

            Assert.IsTrue(legacyConverterService.ShouldOfferBeRejected(dataRow, rejectedSocials), "Already tagged social should automatically be true.");

        }

        [Test]
        public void ShouldOfferBeRejectedShouldBeTrueIfHireDateIsAfterOfferedOn()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED", typeof(String));
            dataTable.Columns.Add("EMPLOYEE_HIRE_DATE", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_EFFECTIVE_DATE", typeof(String));
            dataTable.Columns.Add("REJECTION-REASON", typeof(String));

            DateTime hiredOn = DateTime.Parse("2016-05-01");
            DateTime offeredOn = DateTime.Parse("2016-01-01");
            DateTime effectiveOn = DateTime.Parse("2016-05-01");

            DataRow dataRow = dataTable.NewRow();
            dataRow["SSN"] = "ABCDEFGHI";
            dataRow["OFFER_OFFERED"] = true;
            dataRow["EMPLOYEE_HIRE_DATE"] = hiredOn.ToShortDateString();
            dataRow["OFFER_OFFERED_ON"] = offeredOn.ToShortDateString();
            dataRow["OFFER_EFFECTIVE_DATE"] = effectiveOn.ToShortDateString();
            dataTable.Rows.Add(dataRow);

            IList<String> rejectedSocials = new List<String>();

            Assert.IsTrue(legacyConverterService.ShouldOfferBeRejected(dataRow, rejectedSocials), "Dates before hire date should be rejected.");

        }

        [Test]
        public void ShouldOfferBeRejectedShouldBeTrueIfHireDateIsAfterEffectiveOn()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED", typeof(String));
            dataTable.Columns.Add("EMPLOYEE_HIRE_DATE", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_EFFECTIVE_DATE", typeof(String));
            dataTable.Columns.Add("REJECTION-REASON", typeof(String));

            DateTime hiredOn = DateTime.Parse("2016-05-01");
            DateTime offeredOn = DateTime.Parse("2016-05-01");
            DateTime effectiveOn = DateTime.Parse("2016-01-01");

            DataRow dataRow = dataTable.NewRow();
            dataRow["SSN"] = "ABCDEFGHI";
            dataRow["OFFER_OFFERED"] = true;
            dataRow["EMPLOYEE_HIRE_DATE"] = hiredOn.ToShortDateString();
            dataRow["OFFER_OFFERED_ON"] = offeredOn.ToShortDateString();
            dataRow["OFFER_EFFECTIVE_DATE"] = effectiveOn.ToShortDateString();
            dataTable.Rows.Add(dataRow);

            IList<String> rejectedSocials = new List<String>();

            Assert.IsTrue(legacyConverterService.ShouldOfferBeRejected(dataRow, rejectedSocials), "Dates before hire date should be rejected.");

        }

        [Test]
        public void ShouldOfferBeRejectedShouldBeTrueIfHireDateIsAfterWaivedOn()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED", typeof(String));
            dataTable.Columns.Add("EMPLOYEE_HIRE_DATE", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_EFFECTIVE_DATE", typeof(String));
            dataTable.Columns.Add("REJECTION-REASON", typeof(String));

            DateTime hiredOn = DateTime.Parse("2016-05-01");
            DateTime offeredOn = DateTime.Parse("2016-05-01");
            DateTime effectiveOn = DateTime.Parse("2016-05-01");
            DateTime acceptedOn = DateTime.Parse("2016-01-01");

            DataRow dataRow = dataTable.NewRow();
            dataRow["SSN"] = "ABCDEFGHI";
            dataRow["OFFER_OFFERED"] = true;
            dataRow["EMPLOYEE_HIRE_DATE"] = hiredOn.ToShortDateString();
            dataRow["OFFER_OFFERED_ON"] = offeredOn.ToShortDateString();
            dataRow["OFFER_EFFECTIVE_DATE"] = effectiveOn.ToShortDateString();
            dataRow["OFFER_ACCEPTED"] = false;
            dataRow["OFFER_ACCEPTED_ON"] = acceptedOn.ToShortDateString();
            dataTable.Rows.Add(dataRow);

            IList<String> rejectedSocials = new List<String>();

            Assert.IsTrue(legacyConverterService.ShouldOfferBeRejected(dataRow, rejectedSocials), "Dates before hire date should be rejected.");

        }

        [Test]
        public void ShouldOfferBeRejectedShouldBeTrueIfHireDateIsAfterAcceptedOn()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED", typeof(String));
            dataTable.Columns.Add("EMPLOYEE_HIRE_DATE", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_EFFECTIVE_DATE", typeof(String));
            dataTable.Columns.Add("REJECTION-REASON", typeof(String));

            DateTime hiredOn = DateTime.Parse("2016-05-01");
            DateTime offeredOn = DateTime.Parse("2016-05-01");
            DateTime effectiveOn = DateTime.Parse("2016-05-01");
            DateTime acceptedOn = DateTime.Parse("2016-01-01");

            DataRow dataRow = dataTable.NewRow();
            dataRow["SSN"] = "ABCDEFGHI";
            dataRow["OFFER_OFFERED"] = true;
            dataRow["EMPLOYEE_HIRE_DATE"] = hiredOn.ToShortDateString();
            dataRow["OFFER_OFFERED_ON"] = offeredOn.ToShortDateString();
            dataRow["OFFER_EFFECTIVE_DATE"] = effectiveOn.ToShortDateString();
            dataRow["OFFER_ACCEPTED"] = true;
            dataRow["OFFER_ACCEPTED_ON"] = acceptedOn.ToShortDateString();
            dataTable.Rows.Add(dataRow);

            IList<String> rejectedSocials = new List<String>();

            Assert.IsTrue(legacyConverterService.ShouldOfferBeRejected(dataRow, rejectedSocials), "Dates before hire date should be rejected.");

        }

        [Test]
        public void ShouldOfferBeRejectedShouldBeTrueIfAcceptedOnIsBeforeOfferedOn()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED", typeof(String));
            dataTable.Columns.Add("EMPLOYEE_HIRE_DATE", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_EFFECTIVE_DATE", typeof(String));
            dataTable.Columns.Add("REJECTION-REASON", typeof(String));

            DateTime hiredOn = DateTime.Parse("2016-05-01");
            DateTime offeredOn = DateTime.Parse("2016-07-01");
            DateTime effectiveOn = DateTime.Parse("2016-07-01");
            DateTime acceptedOn = DateTime.Parse("2016-06-01");

            DataRow dataRow = dataTable.NewRow();
            dataRow["SSN"] = "ABCDEFGHI";
            dataRow["OFFER_OFFERED"] = true;
            dataRow["EMPLOYEE_HIRE_DATE"] = hiredOn.ToShortDateString();
            dataRow["OFFER_OFFERED_ON"] = offeredOn.ToShortDateString();
            dataRow["OFFER_EFFECTIVE_DATE"] = effectiveOn.ToShortDateString();
            dataRow["OFFER_ACCEPTED"] = true;
            dataRow["OFFER_ACCEPTED_ON"] = acceptedOn.ToShortDateString();
            dataTable.Rows.Add(dataRow);

            IList<String> rejectedSocials = new List<String>();

            Assert.IsTrue(legacyConverterService.ShouldOfferBeRejected(dataRow, rejectedSocials), "Dates before offer date should be rejected.");

        }

        [Test]
        public void ShouldOfferBeRejectedShouldBeFalseIfAllDatesAreGood()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED", typeof(String));
            dataTable.Columns.Add("EMPLOYEE_HIRE_DATE", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_EFFECTIVE_DATE", typeof(String));

            DateTime hiredOn = DateTime.Parse("2016-07-01");
            DateTime offeredOn = DateTime.Parse("2016-07-01");
            DateTime effectiveOn = DateTime.Parse("2016-07-01");
            DateTime acceptedOn = DateTime.Parse("2016-07-01");

            DataRow dataRow = dataTable.NewRow();
            dataRow["SSN"] = "ABCDEFGHI";
            dataRow["OFFER_OFFERED"] = true;
            dataRow["EMPLOYEE_HIRE_DATE"] = hiredOn.ToShortDateString();
            dataRow["OFFER_OFFERED_ON"] = offeredOn.ToShortDateString();
            dataRow["OFFER_EFFECTIVE_DATE"] = effectiveOn.ToShortDateString();
            dataRow["OFFER_ACCEPTED"] = true;
            dataRow["OFFER_ACCEPTED_ON"] = acceptedOn.ToShortDateString();
            dataTable.Rows.Add(dataRow);

            IList<String> rejectedSocials = new List<String>();

            Assert.IsFalse(legacyConverterService.ShouldOfferBeRejected(dataRow, rejectedSocials), "Good dates are good.");

        }

        [Test]
        public void ShouldOfferBeRejectedShouldBeFalseIfAllDatesAreGoodWaived()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED", typeof(String));
            dataTable.Columns.Add("EMPLOYEE_HIRE_DATE", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_EFFECTIVE_DATE", typeof(String));

            DateTime hiredOn = DateTime.Parse("2016-07-01");
            DateTime offeredOn = DateTime.Parse("2016-07-01");
            DateTime effectiveOn = DateTime.Parse("2016-07-01");
            DateTime acceptedOn = DateTime.Parse("2016-07-01");

            DataRow dataRow = dataTable.NewRow();
            dataRow["SSN"] = "ABCDEFGHI";
            dataRow["OFFER_OFFERED"] = true;
            dataRow["EMPLOYEE_HIRE_DATE"] = hiredOn.ToShortDateString();
            dataRow["OFFER_OFFERED_ON"] = offeredOn.ToShortDateString();
            dataRow["OFFER_EFFECTIVE_DATE"] = effectiveOn.ToShortDateString();
            dataRow["OFFER_ACCEPTED"] = false;
            dataRow["OFFER_ACCEPTED_ON"] = acceptedOn.ToShortDateString();
            dataTable.Rows.Add(dataRow);

            IList<String> rejectedSocials = new List<String>();

            Assert.IsFalse(legacyConverterService.ShouldOfferBeRejected(dataRow, rejectedSocials), "Good dates are good.");

        }

        [Test]
        public void RemoveNoOfferOfferLinesShouldLeaveEmptyTableAlone()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");

            legacyConverterService.RemoveNoOfferOfferLines(dataTable);

        }

        [Test]
        public void RemoveNoOfferOfferLinesShouldLeaveOfferedLinesAlone()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("OFFER_OFFERED", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["OFFER_OFFERED"] = true;
            dataTable.Rows.Add(dataRow);

            legacyConverterService.RemoveNoOfferOfferLines(dataTable);

            Assert.AreEqual(1, dataTable.Rows.Count, "Should have matching row counts.");

        }

        [Test]
        public void RemoveNoOfferOfferLinesShouldRemoveNoOfferedLines()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("OFFER_OFFERED", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["OFFER_OFFERED"] = false;
            dataTable.Rows.Add(dataRow);

            legacyConverterService.RemoveNoOfferOfferLines(dataTable);

            Assert.AreEqual(0, dataTable.Rows.Count, "Should have matching row counts.");

        }

        [Test]
        public void ResetCoverageEndDateBasedUponPlanYearEndLeavesEmptyTableAlone()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DateTime planYearEnd = DateTime.Parse("2016-12-31");

            DataTable dataTable = new DataTable("CSVTable");

            legacyConverterService.ResetCoverageEndDateBasedUponPlanYearEnd(dataTable, planYearEnd);

            Assert.AreEqual(0, dataTable.Rows.Count, "Should have matching row counts.");

        }

        [Test]
        public void ResetCoverageEndDateBasedUponPlanYearEndClearsDatesWithExactMatch()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DateTime planYearEnd = DateTime.Parse("2016-12-31");

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("Coverage Date End", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["Coverage Date End"] = planYearEnd.ToShortDateString();
            dataTable.Rows.Add(dataRow);

            legacyConverterService.ResetCoverageEndDateBasedUponPlanYearEnd(dataTable, planYearEnd);

            Assert.AreEqual(String.Empty, dataRow["Coverage Date End"].ToString(), "Values should match.");

        }

        [Test]
        public void ResetCoverageEndDateBasedUponPlanYearEndLeavesOneDayEarly()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DateTime planYearEnd = DateTime.Parse("2016-12-31");
            DateTime coverageEnd = planYearEnd.AddDays(-1);

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("Coverage Date End", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["Coverage Date End"] = coverageEnd.ToShortDateString();
            dataTable.Rows.Add(dataRow);

            legacyConverterService.ResetCoverageEndDateBasedUponPlanYearEnd(dataTable, planYearEnd);

            Assert.AreEqual(coverageEnd.ToShortDateString(), dataRow["Coverage Date End"].ToString(), "Values should match.");

        }

        [Test]
        public void ResetCoverageEndDateBasedUponPlanYearEndClearsOneDayAfter()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DateTime planYearEnd = DateTime.Parse("2016-12-31");
            DateTime coverageEnd = planYearEnd.AddDays(1);

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("Coverage Date End", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["Coverage Date End"] = coverageEnd.ToShortDateString();
            dataTable.Rows.Add(dataRow);

            legacyConverterService.ResetCoverageEndDateBasedUponPlanYearEnd(dataTable, planYearEnd);

            Assert.AreEqual(String.Empty, dataRow["Coverage Date End"].ToString(), "Values should match.");

        }

        [Test]
        public void ClassifyInsuranceChangeEventsLeavesEmptyDataTableAlone()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            IList<String> socialsToTag = new List<String>();

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("EVENT_ROW_ID", typeof(long));

            legacyConverterService.ClassifyInsuranceChangeEvents(dataTable, socialsToTag);

        }

        [Test]
        public void ClassifyInsuranceChangeEventsDataTableAloneWithDifferentSocials()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            IList<String> socialsToTag = new List<String>();

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("EVENT_ROW_ID", typeof(long));
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("TYPE_OF_EVENT", typeof(String));
            dataTable.Columns.Add("OFFER_EFFECTIVE_DATE", typeof(String));

            RowBuilder.BuildRow(dataTable, 1, "ABCDEFGHI", OfferChangeEvents.SimpleOffer, DateTime.Parse("2016-01-01"));

            legacyConverterService.ClassifyInsuranceChangeEvents(dataTable, socialsToTag);

            Assert.AreEqual(1, dataTable.Rows.Count, "Row counts should match.");

        }

        [Test]
        public void ClassifyInsuranceChangeEventsTagsSecondEventAsChangeEvent()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            IList<String> socialsToTag = new List<String>();
            socialsToTag.Add("ABCDEFGHI");

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("EVENT_ROW_ID", typeof(long));
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("TYPE_OF_EVENT", typeof(String));
            dataTable.Columns.Add("OFFER_EFFECTIVE_DATE", typeof(String));

            RowBuilder.BuildRow(dataTable, 1, "ABCDEFGHI", OfferChangeEvents.SimpleOffer, DateTime.Parse("2016-01-01"));
            RowBuilder.BuildRow(dataTable, 2, "ABCDEFGHI", OfferChangeEvents.SimpleOffer, DateTime.Parse("2016-01-02"));

            DataTable insuranceChangeEventsTagged = legacyConverterService.ClassifyInsuranceChangeEvents(dataTable, socialsToTag);

            Assert.AreEqual(2, insuranceChangeEventsTagged.Rows.Count, "Row counts should match.");
            Assert.AreEqual(OfferChangeEvents.SimpleOffer, insuranceChangeEventsTagged.Rows[0]["TYPE_OF_EVENT"], "Should be left alone.");
            Assert.AreEqual(OfferChangeEvents.InsuranceChange, insuranceChangeEventsTagged.Rows[1]["TYPE_OF_EVENT"], "Should be changed.");

        }

        [Test]
        public void ClassifyInsuranceChangeEventsRemovesDuplicateEffectiveDateOffers()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            IList<String> socialsToTag = new List<String>();
            socialsToTag.Add("ABCDEFGHI");

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("EVENT_ROW_ID", typeof(long));
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("TYPE_OF_EVENT", typeof(String));
            dataTable.Columns.Add("OFFER_EFFECTIVE_DATE", typeof(String));

            RowBuilder.BuildRow(dataTable, 1, "ABCDEFGHI", OfferChangeEvents.SimpleOffer, DateTime.Parse("2016-01-01"));
            RowBuilder.BuildRow(dataTable, 2, "ABCDEFGHI", OfferChangeEvents.SimpleOffer, DateTime.Parse("2016-01-02"));
            RowBuilder.BuildRow(dataTable, 3, "ABCDEFGHI", OfferChangeEvents.SimpleOffer, DateTime.Parse("2016-01-02"));

            DataTable insuranceChangeEventsTagged = legacyConverterService.ClassifyInsuranceChangeEvents(dataTable, socialsToTag);

            Assert.AreEqual(2, insuranceChangeEventsTagged.Rows.Count, "Row counts should match.");
            Assert.AreEqual(OfferChangeEvents.SimpleOffer, insuranceChangeEventsTagged.Rows[0]["TYPE_OF_EVENT"], "Should be left alone.");
            Assert.AreEqual(OfferChangeEvents.InsuranceChange, insuranceChangeEventsTagged.Rows[1]["TYPE_OF_EVENT"], "Should be changed.");

        }

        [Test]
        public void ClassifyInsuranceRejectionsEventsLeavesEmptyDataTableAlone()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            IList<String> employeesWithMoreThanEntry = new List<String>();

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("EVENT_ROW_ID", typeof(long));

            legacyConverterService.ClassifyInsuranceRejections(dataTable, employeesWithMoreThanEntry);

        }

        [Test]
        public void ClassifyInsuranceRejectionsDataTableAloneWithDifferentSocials()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            IList<String> socialsToTag = new List<String>();

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("EVENT_ROW_ID", typeof(long));
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("TYPE_OF_EVENT", typeof(String));
            dataTable.Columns.Add("OFFER_EFFECTIVE_DATE", typeof(String));

            RowBuilder.BuildRow(dataTable, 1, "ABCDEFGHI", OfferChangeEvents.SimpleOffer, DateTime.Parse("2016-01-01"));

            DataTable retaggedAsRejections = legacyConverterService.ClassifyInsuranceRejections(dataTable, socialsToTag);

            Assert.AreEqual(1, retaggedAsRejections.Rows.Count, "Row counts should match.");
            Assert.AreEqual(OfferChangeEvents.SimpleOffer, retaggedAsRejections.Rows[0]["TYPE_OF_EVENT"].ToString(), "Should be left alone.");

        }

        [Test]
        public void ClassifyInsuranceRejectionsShouldOnlyTagRejectedEventsBySSN()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            IList<String> socialsToTag = new List<String>();
            socialsToTag.Add("ABCDEFGHI");

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("EVENT_ROW_ID", typeof(long));
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("TYPE_OF_EVENT", typeof(String));
            dataTable.Columns.Add("OFFER_EFFECTIVE_DATE", typeof(String));
            dataTable.Columns.Add("REJECTION-REASON", typeof(String));

            RowBuilder.BuildRow(dataTable, 1, "ABCDEFGHI", OfferChangeEvents.SimpleOffer, DateTime.Parse("2016-01-01"));
            RowBuilder.BuildRow(dataTable, 2, "ABCDEFGHI", OfferChangeEvents.SimpleOffer, DateTime.Parse("2016-01-02"));
            RowBuilder.BuildRow(dataTable, 3, "ABCDEFGHJ", OfferChangeEvents.SimpleOffer, DateTime.Parse("2016-01-02"));

            DataTable retaggedAsRejections = legacyConverterService.ClassifyInsuranceRejections(dataTable, socialsToTag);

            Assert.AreEqual(3, retaggedAsRejections.Rows.Count, "Row counts should match.");
            Assert.AreEqual(OfferChangeEvents.Discrepancy, retaggedAsRejections.Rows[0]["TYPE_OF_EVENT"], "Should be changed.");
            Assert.AreEqual(OfferChangeEvents.Discrepancy, retaggedAsRejections.Rows[1]["TYPE_OF_EVENT"], "Should be changed.");
            Assert.AreEqual(OfferChangeEvents.SimpleOffer, retaggedAsRejections.Rows[2]["TYPE_OF_EVENT"], "Should be left alone.");

        }

        [Test]
        public void CorrectEffectiveDatesBeforePlanYearStartDatesLeavesOriginalIfGreaterThanPlanYearStart()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("EVENT_ROW_ID", typeof(long));
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("TYPE_OF_EVENT", typeof(String));
            dataTable.Columns.Add("OFFER_EFFECTIVE_DATE", typeof(String));

            DateTime planYearStart = DateTime.Parse("2015-01-01");
            DateTime effectiveDate = DateTime.Parse("2016-01-01");

            RowBuilder.BuildRow(dataTable, 1, "ABCDEFGHI", OfferChangeEvents.SimpleOffer, effectiveDate);

            legacyConverterService.CorrectEffectiveDatesBeforePlanYearStartDates(dataTable.Rows[0], effectiveDate, planYearStart);

            Assert.AreEqual(effectiveDate.ToShortDateString(), dataTable.Rows[0]["OFFER_EFFECTIVE_DATE"], "Should be left alone.");

        }

        [Test]
        public void CorrectEffectiveDatesBeforePlanYearStartDatesChangesEffectiveDateIfBeforePlanYearStart()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("EVENT_ROW_ID", typeof(long));
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("TYPE_OF_EVENT", typeof(String));
            dataTable.Columns.Add("OFFER_EFFECTIVE_DATE", typeof(String));

            DateTime planYearStart = DateTime.Parse("2016-01-01");
            DateTime effectiveDate = DateTime.Parse("2015-01-01");

            RowBuilder.BuildRow(dataTable, 1, "ABCDEFGHI", OfferChangeEvents.SimpleOffer, effectiveDate);

            legacyConverterService.CorrectEffectiveDatesBeforePlanYearStartDates(dataTable.Rows[0], effectiveDate, planYearStart);

            Assert.AreEqual(planYearStart.ToShortDateString(), dataTable.Rows[0]["OFFER_EFFECTIVE_DATE"], "Should be changed.");

        }

        [Test]
        public void SetBlankCoverageEndDateToPlanYearEndLeavesEmptyTableAlone()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DateTime planYearEnd = DateTime.Parse("2016-12-31");

            DataTable dataTable = new DataTable("CSVTable");

            legacyConverterService.SetBlankCoverageEndDateToPlanYearEnd(dataTable, planYearEnd);

            Assert.AreEqual(0, dataTable.Rows.Count, "Should have matching row counts.");

        }

        [Test]
        public void SetBlankCoverageEndDateToPlanYearEndLeavesFilledInValue()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DateTime planYearEnd = DateTime.Parse("2016-12-31");
            DateTime coverageEnd = planYearEnd.AddDays(-1);

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("Coverage Date End", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["Coverage Date End"] = coverageEnd.ToShortDateString();
            dataTable.Rows.Add(dataRow);

            legacyConverterService.SetBlankCoverageEndDateToPlanYearEnd(dataTable, planYearEnd);

            Assert.AreEqual(coverageEnd.ToShortDateString(), dataRow["Coverage Date End"].ToString(), "Values should match.");

        }

        [Test]
        public void SetBlankCoverageEndDateToPlanYearEndSetsBlankToPlanYearEnd()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DateTime planYearEnd = DateTime.Parse("2016-12-31");

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("Coverage Date End", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["Coverage Date End"] = String.Empty;
            dataTable.Rows.Add(dataRow);

            legacyConverterService.SetBlankCoverageEndDateToPlanYearEnd(dataTable, planYearEnd);

            Assert.AreEqual(planYearEnd.ToShortDateString(), dataRow["Coverage Date End"].ToString(), "Values should match.");

        }

        [Test]
        public void SetBlankCoverageEndDateToPlanYearEndAndResetblahShouldCancelEachOtherOut()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DateTime planYearEnd = DateTime.Parse("2016-12-31");

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("Coverage Date End", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["Coverage Date End"] = String.Empty;
            dataTable.Rows.Add(dataRow);

            legacyConverterService.SetBlankCoverageEndDateToPlanYearEnd(dataTable, planYearEnd);

            Assert.AreEqual(planYearEnd.ToShortDateString(), dataRow["Coverage Date End"].ToString(), "Values should match.");

            legacyConverterService.ResetCoverageEndDateBasedUponPlanYearEnd(dataTable, planYearEnd);

            Assert.AreEqual(String.Empty, dataRow["Coverage Date End"].ToString(), "Values should match.");

        }

        [Test]
        public void IsCoverageDatesWithinThisPlanYearIsFalseForCoverageStartAfterPlanYearEnd()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DateTime planYearStart = DateTime.Parse("2016-01-01");
            DateTime planYearEnd = DateTime.Parse("2016-12-31");

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("Coverage Date Start", typeof(String));
            dataTable.Columns.Add("Coverage Date End", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["Coverage Date Start"] = planYearEnd.AddDays(1).ToShortDateString();
            dataRow["Coverage Date End"] = planYearEnd.AddDays(10).ToShortDateString();
            dataTable.Rows.Add(dataRow);

            Assert.IsFalse(legacyConverterService.IsCoverageDatesWithinThisPlanYear(dataRow, planYearStart, planYearEnd), "Start coverage after plan year end is not in the plan year.");

        }

        [Test]
        public void IsCoverageDatesWithinThisPlanYearIsFalseForCoverageEndABeforePlanYearStart()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DateTime planYearStart = DateTime.Parse("2016-01-01");
            DateTime planYearEnd = DateTime.Parse("2016-12-31");

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("Coverage Date Start", typeof(String));
            dataTable.Columns.Add("Coverage Date End", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["Coverage Date Start"] = planYearStart.AddDays(-10).ToShortDateString();
            dataRow["Coverage Date End"] = planYearStart.AddDays(-1).ToShortDateString();
            dataTable.Rows.Add(dataRow);

            Assert.IsFalse(legacyConverterService.IsCoverageDatesWithinThisPlanYear(dataRow, planYearStart, planYearEnd), "End coverage before plan year end is not in the plan year.");

        }

        [Test]
        public void IsCoverageDatesWithinThisPlanYearIsTrueForPlanYearStartCoverageStartSameEndDateAfterPlanYearEnd()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DateTime planYearStart = DateTime.Parse("2016-01-01");
            DateTime planYearEnd = DateTime.Parse("2016-12-31");

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("Coverage Date Start", typeof(String));
            dataTable.Columns.Add("Coverage Date End", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["Coverage Date Start"] = planYearStart.ToShortDateString();
            dataRow["Coverage Date End"] = planYearEnd.AddDays(365).ToShortDateString();
            dataTable.Rows.Add(dataRow);

            Assert.IsTrue(legacyConverterService.IsCoverageDatesWithinThisPlanYear(dataRow, planYearStart, planYearEnd), "Coverage dates spanning plan year is in plan year.");

        }

        [Test]
        public void IsCoverageDatesWithinThisPlanYearIsTrueForPlanYearEndCoverageEndSameStartDateBeforePlanYearStart()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DateTime planYearStart = DateTime.Parse("2016-01-01");
            DateTime planYearEnd = DateTime.Parse("2016-12-31");

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("Coverage Date Start", typeof(String));
            dataTable.Columns.Add("Coverage Date End", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["Coverage Date Start"] = planYearStart.AddDays(-365).ToShortDateString();
            dataRow["Coverage Date End"] = planYearEnd.ToShortDateString();
            dataTable.Rows.Add(dataRow);

            Assert.IsTrue(legacyConverterService.IsCoverageDatesWithinThisPlanYear(dataRow, planYearStart, planYearEnd), "Coverage dates spanning plan year is in plan year.");

        }

        [Test]
        public void IsCoverageDatesWithinThisPlanYearIsTrueForCoverageStartEndBeforeStartAndAfterEnd()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DateTime planYearStart = DateTime.Parse("2016-01-01");
            DateTime planYearEnd = DateTime.Parse("2016-12-31");

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("Coverage Date Start", typeof(String));
            dataTable.Columns.Add("Coverage Date End", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["Coverage Date Start"] = planYearStart.AddDays(-365).ToShortDateString();
            dataRow["Coverage Date End"] = planYearEnd.AddDays(365).ToShortDateString();
            dataTable.Rows.Add(dataRow);

            Assert.IsTrue(legacyConverterService.IsCoverageDatesWithinThisPlanYear(dataRow, planYearStart, planYearEnd), "Coverage dates spanning plan year is in plan year.");

        }

        [Test]
        public void IsCoverageDatesWithinThisPlanYearIsTrueForCoverageStartEndWithinStartAndEndDates()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DateTime planYearStart = DateTime.Parse("2016-01-01");
            DateTime planYearEnd = DateTime.Parse("2016-12-31");

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("Coverage Date Start", typeof(String));
            dataTable.Columns.Add("Coverage Date End", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["Coverage Date Start"] = planYearStart.AddDays(1).ToShortDateString();
            dataRow["Coverage Date End"] = planYearEnd.AddDays(-1).ToShortDateString();
            dataTable.Rows.Add(dataRow);

            Assert.IsTrue(legacyConverterService.IsCoverageDatesWithinThisPlanYear(dataRow, planYearStart, planYearEnd), "Coverage dates spanning plan year is in plan year.");

        }

        [Test]
        public void ShouldOfferBeRejectedShouldBeTrueIfHireDateIsAfterAcceptedOnVariant2()
        {

            LegacyConverterService legacyConverterService = new LegacyConverterService(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED", typeof(String));
            dataTable.Columns.Add("EMPLOYEE_HIRE_DATE", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED", typeof(String));
            dataTable.Columns.Add("OFFER_OFFERED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_ACCEPTED_ON", typeof(String));
            dataTable.Columns.Add("OFFER_EFFECTIVE_DATE", typeof(String));
            dataTable.Columns.Add("REJECTION-REASON", typeof(String));

            DateTime hiredOn = DateTime.Parse("2016-08-01");
            DateTime offeredOn = DateTime.Parse("2016-01-01");
            DateTime effectiveOn = DateTime.Parse("2016-01-01");
            DateTime acceptedOn = DateTime.Parse("2016-01-01");

            DataRow dataRow = dataTable.NewRow();
            dataRow["SSN"] = "ABCDEFGHI";
            dataRow["OFFER_OFFERED"] = true;
            dataRow["EMPLOYEE_HIRE_DATE"] = hiredOn.ToShortDateString();
            dataRow["OFFER_OFFERED_ON"] = offeredOn.ToShortDateString();
            dataRow["OFFER_EFFECTIVE_DATE"] = effectiveOn.ToShortDateString();
            dataRow["OFFER_ACCEPTED"] = true;
            dataRow["OFFER_ACCEPTED_ON"] = acceptedOn.ToShortDateString();
            dataTable.Rows.Add(dataRow);

            IList<String> rejectedSocials = new List<String>();

            Assert.IsTrue(legacyConverterService.ShouldOfferBeRejected(dataRow, rejectedSocials), "Dates before hire date should be rejected.");

        }

    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    static class RowBuilder
    {

        internal static DataRow BuildRow(DataTable dataTable, long eventRowId, String social, String typeOfEvent, DateTime effectiveDate)
        {

            DataRow dataRow = dataTable.NewRow();
            dataRow["EVENT_ROW_ID"] = eventRowId;
            dataRow["SSN"] = social;
            dataRow["TYPE_OF_EVENT"] = typeOfEvent;
            dataRow["OFFER_EFFECTIVE_DATE"] = effectiveDate.ToShortDateString();
            dataTable.Rows.Add(dataRow);

            return dataRow;

        }

    }
}
