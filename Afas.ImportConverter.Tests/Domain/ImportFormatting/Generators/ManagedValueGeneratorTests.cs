using Afas.Application;
using Afas.ImportConverter.Domain.ImportFormatting.Generators;
using Afas.ImportConverter.Domain.ImportFormatting.Generators.Implementation;
using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.ImportConverter.Tests.Domain.ImportFormatting.Generators
{
    [TestFixture]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ManagedValueGeneratorTests
    {   
        private DataRow testRow;

        private string GenReturn;

        private IDefaultFactory<IValueGenerator> stubedGeneratorFactory;

        private IValueGenerator stubedGenerator;

        private ManagedValueGenerator TestObject;

        /// <summary>
        /// Test setup that should run before every test to make sure that the Mocks are in a consistent state
        /// </summary>
        [SetUp]
        public void Init()
        {
            GenReturn = "TestGenValue";
            DataTable testData = new DataTable();
            testData.Columns.Add("TestColumn", typeof(string));
            testData.Columns.Add("OtherColumn", typeof(string));
            testData.Columns.Add("LastColumn", typeof(string));
            testData.Columns.Add("ControlColumn", typeof(string));
            testRow = testData.NewRow();
            testRow["TestColumn"] = "TestColumnValue";
            testRow["OtherColumn"] = "OtherColumnValue";
            testRow["LastColumn"] = "LastColumnValue";
            testRow["ControlColumn"] = "OtherColumn";

            stubedGenerator = MockRepository.GenerateMock<IValueGenerator>();
            stubedGenerator.Stub(gen => gen.GenerateValue(Arg<string>.Is.Anything))
                .WhenCalled(
                    gen =>
                    {
                        gen.ReturnValue = gen.Arguments[0].ToString().Replace("[TestPlaceholder]", GenReturn); 
                    }
                ).Return(GenReturn);
            stubedGenerator.Stub(gen => gen.GenerateValue()).Return(GenReturn);
            stubedGenerator.Stub(gen => gen.GetPlaceholder).Return("[TestPlaceholder]");
            
            stubedGeneratorFactory = MockRepository.GenerateDynamicMockWithRemoting<IDefaultFactory<IValueGenerator>>();
            stubedGeneratorFactory.Stub(genFac => genFac.GetById(Arg<string>.Is.Anything)).Return(stubedGenerator);

            TestObject = new ManagedValueGenerator(stubedGeneratorFactory);

        }

        [Test]
        public void ManagedValueGenerator_ConstructorTest_NullDependency()
        {

            Assert.Throws<ArgumentNullException>(
                () => { new ManagedValueGenerator(null); },
                "Did not recieve expected Null Argument exception on a null dependency."
                );

        }

        [Test]
        public void ManagedValueGenerator_ConstructorTest_HappyPath()
        {

            Assert.IsNotNull(new ManagedValueGenerator(stubedGeneratorFactory)); 

        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase(",,,")]
        [TestCase("bob,larry,harry")]
        [TestCase(" , bob")]
        public void ManagedValueGenerator_GenerateDefaultValue_BadGeneratorValues(string value)
        {
            
            Assert.Throws<ArgumentException>(
                () => { string result = TestObject.GenerateDefaultValue(value, "[TestPlaceholder]", testRow); },
                "Did not recieve expected Argument Exception on Bad generatorTypeValue. " + value
                );

        }

        [TestCase("RandomGuid")]
        [TestCase("RandomNumber")]
        [TestCase("UniqueNumber")]
        [TestCase("UniqueDate")]
        [TestCase("ValueCombination")]
        [TestCase(" RandomGuid")]
        [TestCase("  RandomGuid     ")]
        public void ManagedValueGenerator_GenerateDefaultValue_GoodGeneratorValues(string value)
        {

            string result = TestObject.GenerateDefaultValue(value, "[TestPlaceholder]", testRow);
            Assert.AreEqual(GenReturn, result);

        }

        [TestCase("RandomGuid,RandomNumber", "[RandomGuid][RandomNumber]", "GenValueRandomGuidGenValueRandomNumber")]
        [TestCase("RandomNumber, UniqueNumber", "[RandomNumber][UniqueNumber]", "GenValueRandomNumberGenValueUniqueNumber")]
        [TestCase("RandomNumber, UniqueNumber", "[UniqueNumber][RandomNumber]", "GenValueUniqueNumberGenValueRandomNumber")]
        [TestCase("RandomNumber, UniqueNumber, UniqueDate, RandomGuid", "[RandomNumber][UniqueNumber][UniqueDate][RandomGuid]", "GenValueRandomNumberGenValueUniqueNumberGenValueUniqueDateGenValueRandomGuid")]
        public void ManagedValueGenerator_GenerateDefaultValue_MultipleGoodGeneratorValues(string value, string placeHolder, string expectedValue)
        {
            stubedGenerator.BackToRecord(BackToRecordOptions.All);
            stubedGenerator.Replay();

            stubedGeneratorFactory.BackToRecord(BackToRecordOptions.All);
            stubedGeneratorFactory.Replay();

            stubedGeneratorFactory.Stub(genFac => genFac.GetById(Arg<string>.Is.Anything)).WhenCalled(
                    fac =>
                    {

                        string type = fac.Arguments[0].ToString();
                        stubedGenerator = MockRepository.GenerateMock<IValueGenerator>();
                        stubedGenerator.Stub(gen => gen.GenerateValue(Arg<string>.Is.Anything))
                            .WhenCalled(
                                gen =>
                                {
                                    gen.ReturnValue = gen.Arguments[0].ToString().Replace("[" + type + "]", "GenValue" + type);
                                }
                            ).Return("GenValue" + type);
                        stubedGenerator.Stub(gen => gen.GenerateValue()).Return("GenValue" + type);
                        stubedGenerator.Stub(gen => gen.GetPlaceholder).Return("[" + type + "]");


                        fac.ReturnValue = stubedGenerator;
                    }
                ).Return(stubedGenerator);

            string result = TestObject.GenerateDefaultValue(value, placeHolder, testRow);
            Assert.AreEqual(expectedValue, result);

        }

        [TestCase("RandomGuid, bob")]
        [TestCase("bob, RandomGuid")]
        [TestCase("bob,RandomGuid")]
        [TestCase("bob, RandomGuid, larry, rich")]
        [TestCase("RandomNumber, bob, UniqueDate, RandomGuid")]
        public void ManagedValueGenerator_GenerateDefaultValue_GoodAndBadTypeValues(string value)
        {

            Assert.Throws<ArgumentException>(
                () => { string result = TestObject.GenerateDefaultValue(value, "[TestPlaceholder]", testRow); },
                "Did not recieve expected Argument Exception on Bad generatorTypeValue. " + value
                );

        }
        
        [Test]
        public void ManagedValueGenerator_GenerateDefaultValue_NoGeneratorFound()
        {
            stubedGeneratorFactory.BackToRecord();
            stubedGeneratorFactory.Replay();
            stubedGeneratorFactory.Stub(genFac => genFac.GetById(Arg<string>.Is.Anything)).Return(null);

            string result = TestObject.GenerateDefaultValue("RandomGuid", "", testRow);
            Assert.AreEqual(string.Empty, result);

        }

        [Test]
        public void ManagedValueGenerator_GenerateDefaultValue_NullPatterns()
        {

            string result = TestObject.GenerateDefaultValue("RandomGuid", null, testRow);
            Assert.AreEqual(string.Empty, result);

        }

        [TestCase("")]
        [TestCase("sdfdsf")]
        [TestCase("[sdfdsf]")]
        [TestCase("{sdfdsf}")]
        public void ManagedValueGenerator_GenerateDefaultValue_BadPatterns(string value)
        {

            string result = TestObject.GenerateDefaultValue("RandomGuid", value, testRow);
            Assert.AreEqual(value, result);

        }
        
        [TestCase("[TestPlaceholder]", "TestGenValue")]
        [TestCase("[TestPlaceholder][TestPlaceholder]", "TestGenValueTestGenValue")]
        [TestCase("[TestPlaceholder]---[TestPlaceholder]", "TestGenValue---TestGenValue")]
        [TestCase("[TestAsdfPlaceholder]---[TestPlaceholder]", "[TestAsdfPlaceholder]---TestGenValue")]
        public void ManagedValueGenerator_GenerateDefaultValue_GoodPatternsPlaceholder(string value, string expectedValue)
        {

            string result = TestObject.GenerateDefaultValue("RandomGuid", value, testRow);
            Assert.AreEqual(expectedValue, result);

        }

        [TestCase("{TestColumn}", "TestColumnValue")]
        [TestCase("{OtherColumn}", "OtherColumnValue")]
        [TestCase("{LastColumn}", "LastColumnValue")]
        [TestCase("{TestColumn  }", "TestColumnValue")]
        [TestCase("123{LastColumn}456", "123LastColumnValue456")]
        [TestCase("{{LastColumn}}", "{LastColumnValue}")]
        [TestCase("{TestColumn}{OtherColumn}", "TestColumnValueOtherColumnValue")]
        [TestCase("{TestColumn}{OtherColumn}{LastColumn}", "TestColumnValueOtherColumnValueLastColumnValue")]
        [TestCase("{TestColumn} - {OtherColumn}::{LastColumn}", "TestColumnValue - OtherColumnValue::LastColumnValue")]
        public void ManagedValueGenerator_GenerateDefaultValue_GoodRowValueReplacement(string value, string expectedValue)
        {

            string result = TestObject.GenerateDefaultValue("RandomGuid", value, testRow);
            Assert.AreEqual(expectedValue, result);

        }

        [TestCase("{1OtherColumn}")]
        [TestCase("{TestColumn{s}}")]
        [TestCase("{asdf}")]
        [TestCase("{}")]
        public void ManagedValueGenerator_GenerateDefaultValue_BadRowValueReplacement(string value)
        {

            string result = TestObject.GenerateDefaultValue("RandomGuid", value, testRow);
            Assert.AreEqual(value, result);

        }

        [TestCase("{{ControlColumn}}", "OtherColumnValue")]
        [TestCase("{TestColumn{LastColumn}}", "{TestColumnLastColumnValue}")]
        [TestCase("{TestColumn} - {OtherAsfdColumn}::{LastColumn}", "TestColumnValue - {OtherAsfdColumn}::LastColumnValue")]
        public void ManagedValueGenerator_GenerateDefaultValue_GoodAndBadRowValueReplacement(string value, string expectedValue)
        {

            string result = TestObject.GenerateDefaultValue("RandomGuid", value, testRow);
            Assert.AreEqual(expectedValue, result);

        }

        [TestCase("[TestPlaceholder]{LastColumn}", "TestGenValueLastColumnValue")]
        [TestCase("{TestColumn} - [TestPlaceholder]::{LastColumn}", "TestColumnValue - TestGenValue::LastColumnValue")]
        public void ManagedValueGenerator_GenerateDefaultValue_GenerationAndValueReplacement(string value, string expectedValue)
        {

            string result = TestObject.GenerateDefaultValue("RandomGuid", value, testRow);
            Assert.AreEqual(expectedValue, result);

        }

        [TestCase("[TestPlaceholder]{LastColumn}", "OtherColumnValueLastColumnValue")]
        [TestCase("{TestColumn} - [TestPlaceholder]::{LastColumn}", "TestColumnValue - OtherColumnValue::LastColumnValue")]
        public void ManagedValueGenerator_GenerateDefaultValue_GenerationAndValueReplacementSpecialCase(string value, string expectedValue)
        {

            GenReturn = "{OtherColumn}"; 
            string result = TestObject.GenerateDefaultValue("RandomGuid", value, testRow);
            Assert.AreEqual(expectedValue, result);

        }
    }
}
