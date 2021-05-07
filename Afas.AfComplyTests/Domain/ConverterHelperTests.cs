using System;

using NUnit.Framework;
using Rhino.Mocks;

using Afas.AfComply.Domain;

namespace Afas.AfComply.DomainTests
{

    [TestFixture]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class ConverterHelperTests
    {

        [Test]
        public void BuildHrStatusDescriptionLeavesValuesAloneCommasDescription()
        {
            Assert.AreEqual("status_id - status,description", ConverterHelper.BuildHrStatusDescription("status_id", "status,description"), "Should not mangle values, leave as is.");
        }

        [Test]
        public void BuildHrStatusDescriptionLeavesValuesAloneQuotesDescription()
        {
            Assert.AreEqual("status_id - status'description", ConverterHelper.BuildHrStatusDescription("status_id", "status'description"), "Should not mangle values, leave as is.");
        }

        [Test]
        public void BuildHrStatusDescriptionLeavesValuesAloneDoubleQuotesDescription()
        {
            Assert.AreEqual(@"status_id - status""description", ConverterHelper.BuildHrStatusDescription("status_id", @"status""description"), "Should not mangle values, leave as is.");
        }

        [Test]
        public void BuildHrStatusDescriptionLeavesValuesAloneCommasId()
        {
            Assert.AreEqual("status,id - status description", ConverterHelper.BuildHrStatusDescription("status,id", "status description"), "Should not mangle values, leave as is.");
        }

        [Test]
        public void BuildHrStatusDescriptionLeavesValuesAloneQuotesId()
        {
            Assert.AreEqual("status'id - status description", ConverterHelper.BuildHrStatusDescription("status'id", "status description"), "Should not mangle values, leave as is.");
        }

        [Test]
        public void BuildHrStatusDescriptionLeavesValuesAloneDoubleQuotesId()
        {
            Assert.AreEqual(@"status""id - status description", ConverterHelper.BuildHrStatusDescription(@"status""id", "status description"), "Should not mangle values, leave as is.");
        }

        [Test]
        public void BuildHrStatusIdLeavesValuesAloneCommasId()
        {
            Assert.AreEqual("status,id-878F517694", ConverterHelper.BuildHrStatusId("status,id", "status description"), "Should not mangle values, leave as is.");
        }

        [Test]
        public void BuildHrStatusIdLeavesValuesAloneQuotesId()
        {
            Assert.AreEqual("status'id-878F517694", ConverterHelper.BuildHrStatusId("status'id", "status description"), "Should not mangle values, leave as is.");
        }

        [Test]
        public void BuildHrStatusIdLeavesValuesAloneDoubleQuotesId()
        {
            Assert.AreEqual(@"status""id-878F517694", ConverterHelper.BuildHrStatusId(@"status""id", "status description"), "Should not mangle values, leave as is.");
        }

        [Test]
        public void BuildHrStatusIdLeavesValuesAloneCommasDescription()
        {
            Assert.AreEqual("status_id-ECE1D472EF", ConverterHelper.BuildHrStatusId("status_id", "status,description"), "Should not mangle values, leave as is.");
        }

        [Test]
        public void BuildHrStatusIdLeavesValuesAloneQuotesDescription()
        {
            Assert.AreEqual("status_id-D082606DF9", ConverterHelper.BuildHrStatusId("status_id", "status'description"), "Should not mangle values, leave as is.");
        }

        [Test]
        public void BuildHrStatusIdLeavesValuesAloneDoubleQuotesDescription()
        {
            Assert.AreEqual("status_id-44865E1485", ConverterHelper.BuildHrStatusId("status_id", @"status""description"), "Should not mangle values, leave as is.");
        }

    }

}
