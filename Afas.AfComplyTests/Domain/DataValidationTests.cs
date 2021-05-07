using System;
using NUnit.Framework;
using Afas.AfComply.Domain;

namespace Afas.AfComplyTests.Domain
{
    [TestFixture]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class DataValidationTests
    {
        [Test]
        public void StringArrayContainsOnlyBlanksReturnsTrueWithEmptyArray()
        {
            String [] testCase = new String [0];
            bool result = DataValidation.StringArrayContainsOnlyBlanks(testCase);
            Assert.IsTrue(result);
        }

        [Test]
        public void StringArrayContainsOnlyBlanksReturnsTrueWithArrayOfTwoBlanks()
        {
            String[] testCase = { "","" };
            bool result = DataValidation.StringArrayContainsOnlyBlanks(testCase);
            Assert.IsTrue(result);
        }

        [Test]
        public void StringArrayContainsOnlyBlanksReturnsTrueArrayOfNulls()
        {
            String[] testCase = {null,null};
            bool result = DataValidation.StringArrayContainsOnlyBlanks(testCase);
            Assert.IsTrue(result);
        }

        [Test]
        public void StringArrayContainsOnlyBlanksReturnsFalseArrayOfBlanksAndNonBlanks()
        {
            String[] testCase = {"test",""};
            bool result = DataValidation.StringArrayContainsOnlyBlanks(testCase);
            Assert.IsFalse(result);
        }

        [Test]
        public void StringArrayContainsOnlyBlanksReturnsFalseArrayOfNonBlanks()
        {
            String[] testCase = {"test","case"};
            bool result = DataValidation.StringArrayContainsOnlyBlanks(testCase);
            Assert.IsFalse(result);
        }
		
		[Test]
        public void IsStartBeforeEndDatePassesWithGoodDataTest()
        {
            string _start = "20150101";
            string _end = "20150131";
            bool result = DataValidation.IsStartBeforeEndDate(_start, _end);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsStartBeforeEndDateFailsWithBadDataTest()
        {
            string _start = "20150201";
            string _end = "20150131";
            bool result = DataValidation.IsStartBeforeEndDate(_start, _end);
            Assert.IsFalse(result);
        }

        public void IsStartBeforeEndDateFailsWithUnparsableDataBothTest()
        {
            string _start = "Gandalf The Grey";
            string _end = "Gandalf The White";
            bool result = DataValidation.IsStartBeforeEndDate(_start, _end);
            Assert.IsFalse(result);

            Throws.TypeOf<System.ArgumentException>();
        }
        
        [Test]
        public void IsStartBeforeEndDatePassesWithVariusFormats1()
        {
            string _start = "2015/01/01";
            string _end = "01/31/2015";
            bool result = DataValidation.IsStartBeforeEndDate(_start, _end);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsStartBeforeEndDatePassesWithVariusFormats2()
        {
            string _start = "2015-01-01";
            string _end = "01/31/2015 12:00:00 AM";
            bool result = DataValidation.IsStartBeforeEndDate(_start, _end);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsStartBeforeEndDatePassesWithVariusFormats3()
        {
            string _start = "01/1/2015 12:00:00 AM";
            string _end = "01/31/2015 1:00:00 AM";
            bool result = DataValidation.IsStartBeforeEndDate(_start, _end);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsStartBeforeEndDatePassesWithVariusFormats4()
        {
            string _start = "01/1/2015 1:00:00 AM";
            string _end = "1/31/2015 12:00:00 AM";
            bool result = DataValidation.IsStartBeforeEndDate(_start, _end);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsStartBeforeEndDatePassesWithVariusFormats5()
        {
            string _start = "1/1/2015 12:00:00 AM";
            string _end = "01/31/2015 11:00";
            bool result = DataValidation.IsStartBeforeEndDate(_start, _end);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsStartBeforeEndDatePassesWithVariusFormats6()
        {
            string _start = "2015/01/01";
            string _end = "01312015";
            bool result = DataValidation.IsStartBeforeEndDate(_start, _end);
            Assert.IsTrue(result);
        }

        public void IsStartBeforeEndDateFailsWithUnparsableDataStartTest()
        {
            string _start = "Gandalf The Grey";
            string _end = "2015/01/31";
            bool result = DataValidation.IsStartBeforeEndDate(_start, _end);
            Assert.IsFalse(result);

            Throws.TypeOf<System.ArgumentException>();
        }

        public void IsStartBeforeEndDateFailsWithUnparsableDataEndTest()
        {
            string _start = "2015/01/01";
            string _end = "Gandalf The White";
            bool result = DataValidation.IsStartBeforeEndDate(_start, _end);
            Assert.IsFalse(result);

            Throws.TypeOf<System.ArgumentException>();
        }
    }
}
