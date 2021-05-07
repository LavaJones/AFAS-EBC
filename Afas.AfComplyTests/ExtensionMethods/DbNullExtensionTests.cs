using Afas.AfComply.Domain;
using NUnit.Framework;
using System;

namespace Afas.AfComplyTests.ExtensionMethods
{
    [TestFixture]
    public class DbNullExtensionMethodsTests
    {
        [TestCase(null, null)]
        [TestCase("Yes", true)]
        [TestCase("yes", true)]
        [TestCase("Yes ", true)]
        [TestCase(" yes", true)]
        [TestCase("YEs", true)]
        [TestCase("YES", true)]
        [TestCase("YeS", true)]
        [TestCase("yEs", true)]
        [TestCase("True", true)]
        [TestCase("TRue", true)]
        [TestCase("TRUe", true)]
        [TestCase("TRUE", true)]
        [TestCase(true, true)]
        public void DbNullExtensionMethodsTests_convertYesAndNoToBoolTest_YesAndTrueShouldBetrue(object PassingValue, bool? Result)
        {
            bool? Actual = DbNullExtensionMethods.convertYesAndNoToBool(PassingValue);
            Assert.AreEqual(Result, Actual);
        }

        [TestCase(null, null)]
        [TestCase("No ", false)]
        [TestCase(" NO", false)]
        [TestCase("No", false)]
        [TestCase("NO", false)]
        [TestCase("no", false)]
        [TestCase("nO", false)]
        [TestCase("False", false)]
        [TestCase("false", false)]
        [TestCase("FALSE", false)]
        [TestCase("FaLsE", false)]
        [TestCase(false, false)]
        public void DbNullExtensionMethodsTests_convertYesAndNoToBoolTest_NoandFalseShouldBefalse(object PassingValue, bool? Result)
        {
            bool? Actual = DbNullExtensionMethods.convertYesAndNoToBool(PassingValue);
            Assert.AreEqual(Result, Actual);
        }

        [TestCase(null, null)]
        [TestCase("AFAS ", null)]
        [TestCase(" AFAS", null)]
        [TestCase("Af-comply", null)]
        [TestCase("Long", null)]
        [TestCase("Ryan", null)]
        [TestCase("Abhi", null)]
        [TestCase("Hannah", null)]
        public void DbNullExtensionMethodsTests_convertYesAndNoToBoolTest_StringShouldBeNull(object PassingValue, bool? Result)
        {
            bool? Actual = DbNullExtensionMethods.convertYesAndNoToBool(PassingValue);
            Assert.AreEqual(Result, Actual);
        }

        [TestCase(true ,true)]
        [TestCase(false,false)]
        [TestCase(null,null)]
        public void DbNullExtensionMethodsTests_convertYesAndNoToBoolTest_trueshouldbetrue(bool? passingValue, bool? Result)
        {
            bool? Actual = DbNullExtensionMethods.convertYesAndNoToBool(passingValue);
            Assert.AreEqual(Result, Actual);
        }

        [TestCase(true ,true)]
        [TestCase(false,false)]
        public void DbNullExtensionMethodsTests_checkForDBNullTest_trueshouldbetrue(bool? passingValue, bool? Result)
        {
            object Actual = DbNullExtensionMethods.checkForDBNull(passingValue);
            Assert.AreEqual(Result, Actual);
        }

        [Test]
        public void DbNullExtensionMethodsTests_checkForDBNullTest_EmptystringShouldBeDBNull()
        {
            object TestValue = "";
            object Actual = DbNullExtensionMethods.checkForDBNull(TestValue);
            Assert.AreEqual(DBNull.Value, Actual);
        }

        [Test]
        public void DbNullExtensionMethodsTests_checkForDBNullTest_NullstringShouldBeDBNull()
        {
            object TestValue = null;
            object Actual = DbNullExtensionMethods.checkForDBNull(TestValue);
            Assert.AreEqual(DBNull.Value, Actual);
        }

    }
}
