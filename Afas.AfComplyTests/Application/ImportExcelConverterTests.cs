using System;

using System.Data;

using log4net;

using NUnit.Framework;
using Rhino.Mocks;

using Afas.AfComply.Application;

namespace Afas.AfComply.ApplicationTests
{
    
    [TestFixture]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ImportExcelConverterTests
    {

        [Test]
        public void SplitPersonalNameOnSpacesReturnsForSimpleFirstnameLastname()
        {
            
            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            String[] names = importExcelConverter.SplitPersonalNameOnSpaces("first last");

            Assert.AreEqual("first", names[0], "Names should match post split.");

            Assert.AreEqual("last", names[1], "Names should match post split.");

        }

        [Test]
        public void SplitPersonalNameOnSpacesReturnsForSimpleFirstnameMiddleInitialLastname()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            String[] names = importExcelConverter.SplitPersonalNameOnSpaces("first M last");

            Assert.AreEqual("first", names[0], "Names should match post split.");

            Assert.AreEqual("last", names[1], "Names should match post split.");

        }

        [Test]
        public void SplitPersonalNameOnSpacesReturnsForReturnsFullMiddleNameAsPartOfLastNameSinceWeLackBetterOptions()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            String[] names = importExcelConverter.SplitPersonalNameOnSpaces("first middle last");

            Assert.AreEqual("first", names[0], "Names should match post split.");

            Assert.AreEqual("middle last", names[1], "Names should match post split.");

        }

        [Test]
        public void SplitPersonalNameOnSpacesReturnsForSimpleFirstnameMiddleInitialLastnameWithExtraInitialSpaces()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            String[] names = importExcelConverter.SplitPersonalNameOnSpaces("first  M last");

            Assert.AreEqual("first", names[0], "Names should match post split.");

            Assert.AreEqual("last", names[1], "Names should match post split.");

        }

        [Test]
        public void SplitPersonalNameOnSpacesReturnsForSimpleFirstnameMiddleInitialLastnameWithExtraTrialingSpaces()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            String[] names = importExcelConverter.SplitPersonalNameOnSpaces("first M  last");

            Assert.AreEqual("first", names[0], "Names should match post split.");

            Assert.AreEqual("last", names[1], "Names should match post split.");

        }

        [Test]
        public void ParsePersonalNameHandlesSpaceDelimitedFirstnameLastname()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            String firstName = String.Empty;
            String lastName = String.Empty;

            importExcelConverter.ParsePersonalName("first last", out firstName, out lastName);

            Assert.AreEqual("first", firstName, "Names should match post split.");

            Assert.AreEqual("last", lastName, "Names should match post split.");

        }

        [Test]
        public void ParsePersonalNameDropsExtraQuotesFromNamesSpaceSplit()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            String firstName = String.Empty;
            String lastName = String.Empty;

            importExcelConverter.ParsePersonalName("\"first last\"", out firstName, out lastName);

            Assert.AreEqual("first", firstName, "Names should match post split.");

            Assert.AreEqual("last", lastName, "Names should match post split.");

        }

        [Test]
        public void SplitPersonalNameOnCommasHandlesLastCommaFirst()
        {
            
            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            String[] names = importExcelConverter.SplitPersonalNameOnCommas("last, first");

            Assert.AreEqual("first", names[0], "Names should match post split.");

            Assert.AreEqual("last", names[1], "Names should match post split.");

        }

        [Test]
        public void SplitPersonalNameOnCommasHandlesLastCommaFirstSpaceMiddle()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            String[] names = importExcelConverter.SplitPersonalNameOnCommas("last, first middle");

            Assert.AreEqual("first", names[0], "Names should match post split.");

            Assert.AreEqual("middle last", names[1], "Names should match post split.");

        }

        [Test]
        public void SplitPersonalNameOnCommasHandlesLastCommaFirstSpaceMiddleInitial()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            String[] names = importExcelConverter.SplitPersonalNameOnCommas("last, first m");

            Assert.AreEqual("first", names[0], "Names should match post split.");

            Assert.AreEqual("last", names[1], "Names should match post split.");

        }

        [Test]
        public void ParsePersonalNameHandlesCommaDelimitedLastnameFirstname()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            String firstName = String.Empty;
            String lastName = String.Empty;

            importExcelConverter.ParsePersonalName("last, first", out firstName, out lastName);

            Assert.AreEqual("first", firstName, "Names should match post split.");

            Assert.AreEqual("last", lastName, "Names should match post split.");

        }

        [Test]
        public void ParsePersonalNameDropsExtraQuotesFromNamesCommaSplits()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            String firstName = String.Empty;
            String lastName = String.Empty;

            importExcelConverter.ParsePersonalName("\"last, first\"", out firstName, out lastName);

            Assert.AreEqual("first", firstName, "Names should match post split.");

            Assert.AreEqual("last", lastName, "Names should match post split.");

        }

        [Test]
        public void SplitPersonalNameOnCommasHandlesLastCommaFirstSpaceMiddleInitialLeavesExtraQuotes()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            String[] names = importExcelConverter.SplitPersonalNameOnCommas("\"last, first m\"");

            Assert.AreEqual("first", names[0], "Names should match post split.");

            Assert.AreEqual("m\" \"last", names[1], "Names should match post split.");

        }

        [Test]
        public void SplitPersonalNameOnSpacesLeavesExtraQuotes()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            String[] names = importExcelConverter.SplitPersonalNameOnSpaces("\"first M last\"");

            Assert.AreEqual("\"first", names[0], "Names should match post split.");

            Assert.AreEqual("last\"", names[1], "Names should match post split.");

        }

        [Test]
        public void Determine2015CoverageDatesReturnsJan1IfBefore2015()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DateTime start = DateTime.MinValue;
            DateTime end = DateTime.MaxValue;

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("StartDate", typeof(String));
            dataTable.Columns.Add("EndDate", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["StartDate"] = "2014-01-01";
            dataRow["EndDate"] = "2015-12-31";

            importExcelConverter.Determine2015CoverageDates(dataRow, out start, out end);

            Assert.AreEqual(DateTime.Parse("2015-01-01"), start, "Should be January first.");

        }

        [Test]
        public void Determine2015CoverageDatesReturnsJan1IfOnJan1()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DateTime start = DateTime.MinValue;
            DateTime end = DateTime.MaxValue;

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("StartDate", typeof(String));
            dataTable.Columns.Add("EndDate", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["StartDate"] = "2015-01-01";
            dataRow["EndDate"] = "2015-12-31";

            importExcelConverter.Determine2015CoverageDates(dataRow, out start, out end);

            Assert.AreEqual(DateTime.Parse("2015-01-01"), start, "Should be January first.");

        }

        [Test]
        public void Determine2015CoverageDatesReturnsDecember31IfOnDecember31()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DateTime start = DateTime.MinValue;
            DateTime end = DateTime.MaxValue;

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("StartDate", typeof(String));
            dataTable.Columns.Add("EndDate", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["StartDate"] = "2013-01-01";
            dataRow["EndDate"] = "2015-12-31";

            importExcelConverter.Determine2015CoverageDates(dataRow, out start, out end);

            Assert.AreEqual(DateTime.Parse("2015-12-31"), end, "Should be December thirty-first.");

        }

        [Test]
        public void Determine2015CoverageDatesReturnsDecember31IfAfterDecember31()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DateTime start = DateTime.MinValue;
            DateTime end = DateTime.MaxValue;

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("StartDate", typeof(String));
            dataTable.Columns.Add("EndDate", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["StartDate"] = "2013-01-01";
            dataRow["EndDate"] = "2016-12-31";

            importExcelConverter.Determine2015CoverageDates(dataRow, out start, out end);

            Assert.AreEqual(DateTime.Parse("2015-12-31"), end, "Should be December thirty-first.");

        }

        [Test]
        public void MorphFileIntoAFcomplyCoverageStructurePadsSSNs()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("StartDate", typeof(String));
            dataTable.Columns.Add("EndDate", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("Last_Name", typeof(String));
            dataTable.Columns.Add("First_Name", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
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
            dataTable.Columns.Add("FIRST NAME");
            dataTable.Columns.Add("LAST NAME");
            dataTable.Columns.Add("CoverageStatus");

            DataRow dataRow = dataTable.NewRow();
            dataRow["SSN"] = "56789";
            dataRow["StartDate"] = "2013-01-01";
            dataRow["EndDate"] = "2016-12-31";
            dataRow["DOB"] = "2010-12-31";
            dataRow["Last_Name"] = "Last";
            dataRow["First_Name"] = "First";
            dataTable.Rows.Add(dataRow);

            importExcelConverter.MorphFileIntoAFcomplyCoverageStructure(dataTable);

            Assert.AreEqual("000056789", dataTable.Rows[0]["SSN"], "Should be formated post morph.");

        }

        [Test]
        public void MorphFileIntoAFcomplyCoverageStructureFormatsDOB()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("StartDate", typeof(String));
            dataTable.Columns.Add("EndDate", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("Last_Name", typeof(String));
            dataTable.Columns.Add("First_Name", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
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
            dataTable.Columns.Add("FIRST NAME");
            dataTable.Columns.Add("LAST NAME");
            dataTable.Columns.Add("CoverageStatus");

            DataRow dataRow = dataTable.NewRow();
            dataRow["SSN"] = "000056789";
            dataRow["StartDate"] = "2013-01-01";
            dataRow["EndDate"] = "2016-12-31";
            dataRow["DOB"] = "12/31/2010";
            dataRow["Last_Name"] = "Last";
            dataRow["First_Name"] = "First";
            dataTable.Rows.Add(dataRow);

            importExcelConverter.MorphFileIntoAFcomplyCoverageStructure(dataTable);

            Assert.AreEqual("20101231", dataTable.Rows[0]["DOB"], "Should be formated post morph.");

        }

        [Test]
        public void MorphFileIntoAFcomplyCoverageStructureUsesFirstLastNameOverGenericNameField()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("StartDate", typeof(String));
            dataTable.Columns.Add("EndDate", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("Last_Name", typeof(String));
            dataTable.Columns.Add("First_Name", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
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
            dataTable.Columns.Add("FIRST NAME");
            dataTable.Columns.Add("LAST NAME");
            dataTable.Columns.Add("CoverageStatus");
            dataTable.Columns.Add("Name");

            DataRow dataRow = dataTable.NewRow();
            dataRow["SSN"] = "000056789";
            dataRow["StartDate"] = "2013-01-01";
            dataRow["EndDate"] = "2016-12-31";
            dataRow["DOB"] = "2010-12-31";
            dataRow["Last_Name"] = "Last";
            dataRow["First_Name"] = "First";
            dataRow["Name"] = "wrong, name";
            dataTable.Rows.Add(dataRow);

            importExcelConverter.MorphFileIntoAFcomplyCoverageStructure(dataTable);

            Assert.AreEqual("First", dataTable.Rows[0]["FIRST NAME"], "Should be formated post morph.");
            Assert.AreEqual("Last", dataTable.Rows[0]["LAST NAME"], "Should be formated post morph.");

        }

        [Test]
        public void MorphFileIntoAFcomplyCoverageStructureWillParseGenericNameFieldIfItIsTheOnlyNameComma()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("StartDate", typeof(String));
            dataTable.Columns.Add("EndDate", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
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
            dataTable.Columns.Add("FIRST NAME");
            dataTable.Columns.Add("LAST NAME");
            dataTable.Columns.Add("CoverageStatus");
            dataTable.Columns.Add("Name");

            DataRow dataRow = dataTable.NewRow();
            dataRow["SSN"] = "000056789";
            dataRow["StartDate"] = "2013-01-01";
            dataRow["EndDate"] = "2016-12-31";
            dataRow["DOB"] = "2010-12-31";
            dataRow["Name"] = "Last, First";
            dataTable.Rows.Add(dataRow);

            importExcelConverter.MorphFileIntoAFcomplyCoverageStructure(dataTable);

            Assert.AreEqual("First", dataTable.Rows[0]["FIRST NAME"], "Should be formated post morph.");
            Assert.AreEqual("Last", dataTable.Rows[0]["LAST NAME"], "Should be formated post morph.");

        }

        [Test]
        public void MorphFileIntoAFcomplyCoverageStructureWillParseGenericNameFieldIfItIsTheOnlyNameSpace()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("StartDate", typeof(String));
            dataTable.Columns.Add("EndDate", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
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
            dataTable.Columns.Add("FIRST NAME");
            dataTable.Columns.Add("LAST NAME");
            dataTable.Columns.Add("CoverageStatus");
            dataTable.Columns.Add("Name");

            DataRow dataRow = dataTable.NewRow();
            dataRow["SSN"] = "000056789";
            dataRow["StartDate"] = "2013-01-01";
            dataRow["EndDate"] = "2016-12-31";
            dataRow["DOB"] = "2010-12-31";
            dataRow["Name"] = "First Last";
            dataTable.Rows.Add(dataRow);

            importExcelConverter.MorphFileIntoAFcomplyCoverageStructure(dataTable);

            Assert.AreEqual("First", dataTable.Rows[0]["FIRST NAME"], "Should be formated post morph.");
            Assert.AreEqual("Last", dataTable.Rows[0]["LAST NAME"], "Should be formated post morph.");

        }

        [Test]
        public void MorphFileIntoAFcomplyCoverageStructureRemovesWaivedCoverage()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("StartDate", typeof(String));
            dataTable.Columns.Add("EndDate", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
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
            dataTable.Columns.Add("FIRST NAME");
            dataTable.Columns.Add("LAST NAME");
            dataTable.Columns.Add("CoverageStatus");
            dataTable.Columns.Add("Name");

            DataRow dataRow = dataTable.NewRow();
            dataRow["SSN"] = "000056789";
            dataRow["StartDate"] = "2013-01-01";
            dataRow["EndDate"] = "2016-12-31";
            dataRow["DOB"] = "2010-12-31";
            dataRow["Name"] = "First Last";
            dataRow["CoverageStatus"] = "W";
            dataTable.Rows.Add(dataRow);

            importExcelConverter.MorphFileIntoAFcomplyCoverageStructure(dataTable);

            Assert.AreEqual(0, dataTable.Rows.Count, "Wiaved coverages are dropped.");

        }

        [Test]
        public void MorphFileIntoAFcomplyCoverageStructureLeavesEnrolledCoverage()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("StartDate", typeof(String));
            dataTable.Columns.Add("EndDate", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
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
            dataTable.Columns.Add("FIRST NAME");
            dataTable.Columns.Add("LAST NAME");
            dataTable.Columns.Add("CoverageStatus");
            dataTable.Columns.Add("Name");

            DataRow dataRow = dataTable.NewRow();
            dataRow["SSN"] = "000056789";
            dataRow["StartDate"] = "2013-01-01";
            dataRow["EndDate"] = "2016-12-31";
            dataRow["DOB"] = "2010-12-31";
            dataRow["Name"] = "First Last";
            dataRow["CoverageStatus"] = "E";
            dataTable.Rows.Add(dataRow);

            importExcelConverter.MorphFileIntoAFcomplyCoverageStructure(dataTable);

            Assert.AreEqual(1, dataTable.Rows.Count, "Enrolled coverages are saved.");

        }

        [Test]
        public void FillCoverageColumnsSetsJanuaryOnlyForOneMonthsCoverage()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("StartDate", typeof(String));
            dataTable.Columns.Add("EndDate", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
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
            dataTable.Columns.Add("FIRST NAME");
            dataTable.Columns.Add("LAST NAME");
            dataTable.Columns.Add("CoverageStatus");
            dataTable.Columns.Add("Name");

            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            importExcelConverter.FillCoverageColumns(dataRow, DateTime.Parse("2015-01-01"), DateTime.Parse("2015-01-31"));

            Assert.AreEqual("1", dataTable.Rows[0]["JAN"], "Enrolled coverages are recorded.");

        }

        [Test]
        public void FillCoverageColumnsSetsFebruaryOnlyForOneMonthsCoverage()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("StartDate", typeof(String));
            dataTable.Columns.Add("EndDate", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
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
            dataTable.Columns.Add("FIRST NAME");
            dataTable.Columns.Add("LAST NAME");
            dataTable.Columns.Add("CoverageStatus");
            dataTable.Columns.Add("Name");

            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            importExcelConverter.FillCoverageColumns(dataRow, DateTime.Parse("2015-02-01"), DateTime.Parse("2015-02-28"));

            Assert.AreEqual("1", dataTable.Rows[0]["FEB"], "Enrolled coverages are recorded.");

        }

        [Test]
        public void FillCoverageColumnsSetsMarchOnlyForOneMonthsCoverage()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("StartDate", typeof(String));
            dataTable.Columns.Add("EndDate", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
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
            dataTable.Columns.Add("FIRST NAME");
            dataTable.Columns.Add("LAST NAME");
            dataTable.Columns.Add("CoverageStatus");
            dataTable.Columns.Add("Name");

            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            importExcelConverter.FillCoverageColumns(dataRow, DateTime.Parse("2015-03-01"), DateTime.Parse("2015-03-31"));

            Assert.AreEqual("1", dataTable.Rows[0]["MAR"], "Enrolled coverages are recorded.");

        }

        [Test]
        public void FillCoverageColumnsSetsAprilOnlyForOneMonthsCoverage()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("StartDate", typeof(String));
            dataTable.Columns.Add("EndDate", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
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
            dataTable.Columns.Add("FIRST NAME");
            dataTable.Columns.Add("LAST NAME");
            dataTable.Columns.Add("CoverageStatus");
            dataTable.Columns.Add("Name");

            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            importExcelConverter.FillCoverageColumns(dataRow, DateTime.Parse("2015-04-01"), DateTime.Parse("2015-04-30"));

            Assert.AreEqual("1", dataTable.Rows[0]["APR"], "Enrolled coverages are recorded.");

        }

        [Test]
        public void FillCoverageColumnsSetsMayOnlyForOneMonthsCoverage()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("StartDate", typeof(String));
            dataTable.Columns.Add("EndDate", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
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
            dataTable.Columns.Add("FIRST NAME");
            dataTable.Columns.Add("LAST NAME");
            dataTable.Columns.Add("CoverageStatus");
            dataTable.Columns.Add("Name");

            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            importExcelConverter.FillCoverageColumns(dataRow, DateTime.Parse("2015-05-01"), DateTime.Parse("2015-05-31"));

            Assert.AreEqual("1", dataTable.Rows[0]["MAY"], "Enrolled coverages are recorded.");

        }

        [Test]
        public void FillCoverageColumnsSetsJuneOnlyForOneMonthsCoverage()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("StartDate", typeof(String));
            dataTable.Columns.Add("EndDate", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
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
            dataTable.Columns.Add("FIRST NAME");
            dataTable.Columns.Add("LAST NAME");
            dataTable.Columns.Add("CoverageStatus");
            dataTable.Columns.Add("Name");

            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            importExcelConverter.FillCoverageColumns(dataRow, DateTime.Parse("2015-06-01"), DateTime.Parse("2015-06-30"));

            Assert.AreEqual("1", dataTable.Rows[0]["JUN"], "Enrolled coverages are recorded.");

        }

        [Test]
        public void FillCoverageColumnsSetsJulyOnlyForOneMonthsCoverage()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("StartDate", typeof(String));
            dataTable.Columns.Add("EndDate", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
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
            dataTable.Columns.Add("FIRST NAME");
            dataTable.Columns.Add("LAST NAME");
            dataTable.Columns.Add("CoverageStatus");
            dataTable.Columns.Add("Name");

            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            importExcelConverter.FillCoverageColumns(dataRow, DateTime.Parse("2015-07-01"), DateTime.Parse("2015-07-31"));

            Assert.AreEqual("1", dataTable.Rows[0]["JUL"], "Enrolled coverages are recorded.");

        }

        [Test]
        public void FillCoverageColumnsSetsAugustOnlyForOneMonthsCoverage()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("StartDate", typeof(String));
            dataTable.Columns.Add("EndDate", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
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
            dataTable.Columns.Add("FIRST NAME");
            dataTable.Columns.Add("LAST NAME");
            dataTable.Columns.Add("CoverageStatus");
            dataTable.Columns.Add("Name");

            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            importExcelConverter.FillCoverageColumns(dataRow, DateTime.Parse("2015-08-01"), DateTime.Parse("2015-08-31"));

            Assert.AreEqual("1", dataTable.Rows[0]["AUG"], "Enrolled coverages are recorded.");

        }

        [Test]
        public void FillCoverageColumnsSetsSeptemberOnlyForOneMonthsCoverage()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("StartDate", typeof(String));
            dataTable.Columns.Add("EndDate", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
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
            dataTable.Columns.Add("FIRST NAME");
            dataTable.Columns.Add("LAST NAME");
            dataTable.Columns.Add("CoverageStatus");
            dataTable.Columns.Add("Name");

            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            importExcelConverter.FillCoverageColumns(dataRow, DateTime.Parse("2015-09-01"), DateTime.Parse("2015-09-30"));

            Assert.AreEqual("1", dataTable.Rows[0]["SEP"], "Enrolled coverages are recorded.");

        }

        [Test]
        public void FillCoverageColumnsSetsOctoberOnlyForOneMonthsCoverage()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("StartDate", typeof(String));
            dataTable.Columns.Add("EndDate", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
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
            dataTable.Columns.Add("FIRST NAME");
            dataTable.Columns.Add("LAST NAME");
            dataTable.Columns.Add("CoverageStatus");
            dataTable.Columns.Add("Name");

            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            importExcelConverter.FillCoverageColumns(dataRow, DateTime.Parse("2015-10-01"), DateTime.Parse("2015-10-31"));

            Assert.AreEqual("1", dataTable.Rows[0]["OCT"], "Enrolled coverages are recorded.");

        }

        [Test]
        public void FillCoverageColumnsSetsNovemberOnlyForOneMonthsCoverage()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("StartDate", typeof(String));
            dataTable.Columns.Add("EndDate", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
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
            dataTable.Columns.Add("FIRST NAME");
            dataTable.Columns.Add("LAST NAME");
            dataTable.Columns.Add("CoverageStatus");
            dataTable.Columns.Add("Name");

            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            importExcelConverter.FillCoverageColumns(dataRow, DateTime.Parse("2015-11-01"), DateTime.Parse("2015-11-30"));

            Assert.AreEqual("1", dataTable.Rows[0]["NOV"], "Enrolled coverages are recorded.");

        }

        [Test]
        public void FillCoverageColumnsSetsDecemberOnlyForOneMonthsCoverage()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("StartDate", typeof(String));
            dataTable.Columns.Add("EndDate", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
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
            dataTable.Columns.Add("FIRST NAME");
            dataTable.Columns.Add("LAST NAME");
            dataTable.Columns.Add("CoverageStatus");
            dataTable.Columns.Add("Name");

            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            importExcelConverter.FillCoverageColumns(dataRow, DateTime.Parse("2015-12-01"), DateTime.Parse("2015-12-31"));

            Assert.AreEqual("1", dataTable.Rows[0]["DEC"], "Enrolled coverages are recorded.");

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForJanuaryStartEnd()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("January", DateTime.Parse("2015-01-01"), DateTime.Parse("2015-01-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForJanuaryStartEndDecember()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("January", DateTime.Parse("2015-01-01"), DateTime.Parse("2015-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForFebruaryStartEnd()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("February", DateTime.Parse("2015-02-01"), DateTime.Parse("2015-02-28")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForFebruaryStartEndDecember()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("February", DateTime.Parse("2015-02-01"), DateTime.Parse("2015-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForFebruaryJanuaryStartEndDecember()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("February", DateTime.Parse("2015-01-01"), DateTime.Parse("2015-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForMarchStartEnd()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("March", DateTime.Parse("2015-03-01"), DateTime.Parse("2015-03-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForMarchStartEndDecember()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("March", DateTime.Parse("2015-03-01"), DateTime.Parse("2015-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForMarchJanuaryStartEndDecember()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("March", DateTime.Parse("2015-01-01"), DateTime.Parse("2015-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForAprilStartEnd()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("April", DateTime.Parse("2015-04-01"), DateTime.Parse("2015-04-30")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForAprilStartEndDecember()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("April", DateTime.Parse("2015-04-01"), DateTime.Parse("2015-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForAprilJanuaryStartEndDecember()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("April", DateTime.Parse("2015-01-01"), DateTime.Parse("2015-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForMayStartEnd()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("May", DateTime.Parse("2015-05-01"), DateTime.Parse("2015-05-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForMayStartEndDecember()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("May", DateTime.Parse("2015-05-01"), DateTime.Parse("2015-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForMayJanuaryStartEndDecember()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("May", DateTime.Parse("2015-01-01"), DateTime.Parse("2015-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForJuneStartEnd()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("June", DateTime.Parse("2015-06-01"), DateTime.Parse("2015-06-30")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForJuneStartEndDecember()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("June", DateTime.Parse("2015-06-01"), DateTime.Parse("2015-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForJuneJanuaryStartEndDecember()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("June", DateTime.Parse("2015-01-01"), DateTime.Parse("2015-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForJulyStartEnd()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("July", DateTime.Parse("2015-07-01"), DateTime.Parse("2015-07-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForJulyStartEndDecember()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("July", DateTime.Parse("2015-07-01"), DateTime.Parse("2015-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForJulyJanuaryStartEndDecember()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("July", DateTime.Parse("2015-01-01"), DateTime.Parse("2015-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForAugustStartEnd()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("August", DateTime.Parse("2015-08-01"), DateTime.Parse("2015-08-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForAugustStartEndDecember()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("August", DateTime.Parse("2015-08-01"), DateTime.Parse("2015-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForAugustJanuaryStartEndDecember()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("August", DateTime.Parse("2015-01-01"), DateTime.Parse("2015-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForSeptemberStartEnd()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("September", DateTime.Parse("2015-09-01"), DateTime.Parse("2015-09-30")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForSeptemberStartEndDecember()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("September", DateTime.Parse("2015-09-01"), DateTime.Parse("2015-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForSeptemberJanuaryStartEndDecember()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("September", DateTime.Parse("2015-01-01"), DateTime.Parse("2015-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForOctoberStartEnd()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("October", DateTime.Parse("2015-10-01"), DateTime.Parse("2015-10-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForOctoberStartEndDecember()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("October", DateTime.Parse("2015-10-01"), DateTime.Parse("2015-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForOctoberJanuaryStartEndDecember()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("October", DateTime.Parse("2015-01-01"), DateTime.Parse("2015-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForNovemberStartEnd()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("November", DateTime.Parse("2015-11-01"), DateTime.Parse("2015-11-30")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForNovemberStartEndDecember()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("November", DateTime.Parse("2015-11-01"), DateTime.Parse("2015-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForNovemberJanuaryStartEndDecember()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("November", DateTime.Parse("2015-01-01"), DateTime.Parse("2015-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForDecemberStartEndDecember()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("December", DateTime.Parse("2015-12-01"), DateTime.Parse("2015-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void DateRangeCoversMonthIsTrueForDecemberJanuaryStartEndDecember()
        {

            Assert.IsTrue(
                    new ImportExcelConverter(MockRepository.GenerateStub<ILog>()).DateRangeCoversMonth("December", DateTime.Parse("2015-01-01"), DateTime.Parse("2015-12-31")),
                    "Should be true for the matching months."
            );

        }

        [Test]
        public void FillCoverageColumnsSetsJanuaryAndFebruaryForSpans()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("StartDate", typeof(String));
            dataTable.Columns.Add("EndDate", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
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
            dataTable.Columns.Add("FIRST NAME");
            dataTable.Columns.Add("LAST NAME");
            dataTable.Columns.Add("CoverageStatus");
            dataTable.Columns.Add("Name");

            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            importExcelConverter.FillCoverageColumns(dataRow, DateTime.Parse("2015-01-01"), DateTime.Parse("2015-02-28"));

            Assert.AreEqual("1", dataTable.Rows[0]["JAN"], "Enrolled coverages are recorded.");
            Assert.AreEqual("1", dataTable.Rows[0]["FEB"], "Enrolled coverages are recorded.");

        }

        [Test]
        public void FillCoverageColumnsSetsJanuaryThroughMarchForSpans()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("StartDate", typeof(String));
            dataTable.Columns.Add("EndDate", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
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
            dataTable.Columns.Add("FIRST NAME");
            dataTable.Columns.Add("LAST NAME");
            dataTable.Columns.Add("CoverageStatus");
            dataTable.Columns.Add("Name");

            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            importExcelConverter.FillCoverageColumns(dataRow, DateTime.Parse("2015-01-01"), DateTime.Parse("2015-03-31"));

            Assert.AreEqual("1", dataTable.Rows[0]["JAN"], "Enrolled coverages are recorded.");
            Assert.AreEqual("1", dataTable.Rows[0]["FEB"], "Enrolled coverages are recorded.");
            Assert.AreEqual("1", dataTable.Rows[0]["MAR"], "Enrolled coverages are recorded.");

        }

        [Test]
        public void FillCoverageColumnsSetsFebruaryThroughAprilForSpans()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("StartDate", typeof(String));
            dataTable.Columns.Add("EndDate", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
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
            dataTable.Columns.Add("FIRST NAME");
            dataTable.Columns.Add("LAST NAME");
            dataTable.Columns.Add("CoverageStatus");
            dataTable.Columns.Add("Name");

            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            importExcelConverter.FillCoverageColumns(dataRow, DateTime.Parse("2015-02-01"), DateTime.Parse("2015-04-30"));

            Assert.IsTrue(dataTable.Rows[0].IsNull("JAN"), "Gaps in coverages are recorded.");
            Assert.AreEqual("1", dataTable.Rows[0]["FEB"], "Enrolled coverages are recorded.");
            Assert.AreEqual("1", dataTable.Rows[0]["MAR"], "Enrolled coverages are recorded.");
            Assert.AreEqual("1", dataTable.Rows[0]["APR"], "Enrolled coverages are recorded.");

        }

        [Test]
        public void FillCoverageColumnsSetsMarchThroughMayForSpans()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("StartDate", typeof(String));
            dataTable.Columns.Add("EndDate", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
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
            dataTable.Columns.Add("FIRST NAME");
            dataTable.Columns.Add("LAST NAME");
            dataTable.Columns.Add("CoverageStatus");
            dataTable.Columns.Add("Name");

            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            importExcelConverter.FillCoverageColumns(dataRow, DateTime.Parse("2015-03-01"), DateTime.Parse("2015-05-31"));

            Assert.IsTrue(dataTable.Rows[0].IsNull("JAN"), "Gaps in coverages are recorded.");
            Assert.IsTrue(dataTable.Rows[0].IsNull("FEB"), "Gaps in coverages are recorded.");
            Assert.AreEqual("1", dataTable.Rows[0]["MAR"], "Enrolled coverages are recorded.");
            Assert.AreEqual("1", dataTable.Rows[0]["APR"], "Enrolled coverages are recorded.");
            Assert.AreEqual("1", dataTable.Rows[0]["MAY"], "Enrolled coverages are recorded.");

        }

        [Test]
        public void FillCoverageColumnsSetsAprilThroughJuneForSpans()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("StartDate", typeof(String));
            dataTable.Columns.Add("EndDate", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
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
            dataTable.Columns.Add("FIRST NAME");
            dataTable.Columns.Add("LAST NAME");
            dataTable.Columns.Add("CoverageStatus");
            dataTable.Columns.Add("Name");

            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            importExcelConverter.FillCoverageColumns(dataRow, DateTime.Parse("2015-04-01"), DateTime.Parse("2015-06-30"));

            Assert.IsTrue(dataTable.Rows[0].IsNull("JAN"), "Gaps in coverages are recorded.");
            Assert.IsTrue(dataTable.Rows[0].IsNull("FEB"), "Gaps in coverages are recorded.");
            Assert.IsTrue(dataTable.Rows[0].IsNull("MAR"), "Gaps in coverages are recorded.");
            Assert.AreEqual("1", dataTable.Rows[0]["APR"], "Enrolled coverages are recorded.");
            Assert.AreEqual("1", dataTable.Rows[0]["MAY"], "Enrolled coverages are recorded.");
            Assert.AreEqual("1", dataTable.Rows[0]["JUN"], "Enrolled coverages are recorded.");

        }

        [Test]
        public void FillCoverageColumnsSetsMayThroughJulyForSpans()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("StartDate", typeof(String));
            dataTable.Columns.Add("EndDate", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
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
            dataTable.Columns.Add("FIRST NAME");
            dataTable.Columns.Add("LAST NAME");
            dataTable.Columns.Add("CoverageStatus");
            dataTable.Columns.Add("Name");

            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            importExcelConverter.FillCoverageColumns(dataRow, DateTime.Parse("2015-05-01"), DateTime.Parse("2015-07-31"));

            Assert.IsTrue(dataTable.Rows[0].IsNull("JAN"), "Gaps in coverages are recorded.");
            Assert.IsTrue(dataTable.Rows[0].IsNull("FEB"), "Gaps in coverages are recorded.");
            Assert.IsTrue(dataTable.Rows[0].IsNull("MAR"), "Gaps in coverages are recorded.");
            Assert.IsTrue(dataTable.Rows[0].IsNull("APR"), "Gaps in coverages are recorded.");
            Assert.AreEqual("1", dataTable.Rows[0]["MAY"], "Enrolled coverages are recorded.");
            Assert.AreEqual("1", dataTable.Rows[0]["JUN"], "Enrolled coverages are recorded.");
            Assert.AreEqual("1", dataTable.Rows[0]["JUL"], "Enrolled coverages are recorded.");

        }

        [Test]
        public void FillCoverageColumnsSetsJuneThroughAugustForSpans()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("StartDate", typeof(String));
            dataTable.Columns.Add("EndDate", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
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
            dataTable.Columns.Add("FIRST NAME");
            dataTable.Columns.Add("LAST NAME");
            dataTable.Columns.Add("CoverageStatus");
            dataTable.Columns.Add("Name");

            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            importExcelConverter.FillCoverageColumns(dataRow, DateTime.Parse("2015-06-01"), DateTime.Parse("2015-08-31"));

            Assert.IsTrue(dataTable.Rows[0].IsNull("JAN"), "Gaps in coverages are recorded.");
            Assert.IsTrue(dataTable.Rows[0].IsNull("FEB"), "Gaps in coverages are recorded.");
            Assert.IsTrue(dataTable.Rows[0].IsNull("MAR"), "Gaps in coverages are recorded.");
            Assert.IsTrue(dataTable.Rows[0].IsNull("APR"), "Gaps in coverages are recorded.");
            Assert.IsTrue(dataTable.Rows[0].IsNull("MAY"), "Gaps in coverages are recorded.");
            Assert.AreEqual("1", dataTable.Rows[0]["JUN"], "Enrolled coverages are recorded.");
            Assert.AreEqual("1", dataTable.Rows[0]["JUL"], "Enrolled coverages are recorded.");
            Assert.AreEqual("1", dataTable.Rows[0]["AUG"], "Enrolled coverages are recorded.");

        }

        [Test]
        public void FillCoverageColumnsSetsJanuaryThroughDecemberForSpans()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("StartDate", typeof(String));
            dataTable.Columns.Add("EndDate", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
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
            dataTable.Columns.Add("FIRST NAME");
            dataTable.Columns.Add("LAST NAME");
            dataTable.Columns.Add("CoverageStatus");
            dataTable.Columns.Add("Name");

            DataRow dataRow = dataTable.NewRow();
            dataTable.Rows.Add(dataRow);

            importExcelConverter.FillCoverageColumns(dataRow, DateTime.Parse("2015-01-01"), DateTime.Parse("2015-12-31"));

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
        public void MorphFileIntoAFcomplyCoverageStructureFillsInCoverageDatesFullYear()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("SSN", typeof(String));
            dataTable.Columns.Add("StartDate", typeof(String));
            dataTable.Columns.Add("EndDate", typeof(String));
            dataTable.Columns.Add("DOB", typeof(String));
            dataTable.Columns.Add("SUBID", typeof(String));
            dataTable.Columns.Add("MEMBER", typeof(String));
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
            dataTable.Columns.Add("FIRST NAME");
            dataTable.Columns.Add("LAST NAME");
            dataTable.Columns.Add("CoverageStatus");
            dataTable.Columns.Add("Name");

            DataRow dataRow = dataTable.NewRow();
            dataRow["SSN"] = "000056789";
            dataRow["StartDate"] = "2013-01-01";
            dataRow["EndDate"] = "2016-12-31";
            dataRow["DOB"] = "2010-12-31";
            dataRow["Name"] = "First Last";
            dataRow["CoverageStatus"] = "E";
            dataTable.Rows.Add(dataRow);

            importExcelConverter.MorphFileIntoAFcomplyCoverageStructure(dataTable);

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
        public void RemoveZeroHourEntriesFromPayrollConversionRemovesZeroHourRecords()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("ACA Hour", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["ACA Hour"] = "0";
            dataTable.Rows.Add(dataRow);

            importExcelConverter.RemoveZeroHourEntriesFromPayrollConversion(dataTable);

            Assert.AreEqual(0, dataTable.Rows.Count, "Zero hours removed.");

        }

        [Test]
        public void RemoveZeroHourEntriesFromPayrollConversionLeavesPositiveHourRecords()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("ACA Hour", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["ACA Hour"] = "0.01";
            dataTable.Rows.Add(dataRow);

            importExcelConverter.RemoveZeroHourEntriesFromPayrollConversion(dataTable);

            Assert.AreEqual(1, dataTable.Rows.Count, "Postive hours stay.");

        }

        [Test]
        public void RemoveZeroHourEntriesFromPayrollConversionLeavesNegativeHourRecords()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("ACA Hour", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["ACA Hour"] = "-0.01";
            dataTable.Rows.Add(dataRow);

            importExcelConverter.RemoveZeroHourEntriesFromPayrollConversion(dataTable);

            Assert.AreEqual(1, dataTable.Rows.Count, "Negative hours stay.");

        }

        [Test]
        public void RemoveNegativeHourEntriesFromPayrollConversionRemovesZeroHourRecords()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("ACA Hour", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["ACA Hour"] = "0";
            dataTable.Rows.Add(dataRow);

            importExcelConverter.RemoveNegativeHourEntriesFromPayrollConversion(dataTable);

            Assert.AreEqual(0, dataTable.Rows.Count, "Zero hours go.");

        }

        [Test]
        public void RemoveNegativeHourEntriesFromPayrollConversionLeavesPositiveHourRecords()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("ACA Hour", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["ACA Hour"] = "0.01";
            dataTable.Rows.Add(dataRow);

            importExcelConverter.RemoveNegativeHourEntriesFromPayrollConversion(dataTable);

            Assert.AreEqual(1, dataTable.Rows.Count, "Postive hours stay.");

        }

        [Test]
        public void RemoveNegativeHourEntriesFromPayrollConversionRemovesNegativeHourRecords()
        {

            ImportExcelConverter importExcelConverter = new ImportExcelConverter(MockRepository.GenerateStub<ILog>());

            DataTable dataTable = new DataTable("CSVTable");
            dataTable.Columns.Add("ACA Hour", typeof(String));

            DataRow dataRow = dataTable.NewRow();
            dataRow["ACA Hour"] = "-0.01";
            dataTable.Rows.Add(dataRow);

            importExcelConverter.RemoveNegativeHourEntriesFromPayrollConversion(dataTable);

            Assert.AreEqual(0, dataTable.Rows.Count, "Negative hours go.");

        }

    }

}
