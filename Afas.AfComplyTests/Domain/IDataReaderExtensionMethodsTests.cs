using System;

using System.Data;

using NUnit.Framework;
using Rhino.Mocks;

using Afas.AfComply.Domain;

namespace Afas.AfComply.DomainTests
{

    [TestFixture]
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class IDataReaderExtensionMethodsTests
    {

        [Test]
        public void ReturnsDefaultObjectWhenColumnIsMissing()
        {

            DataTable dataTable = new DataTable();

            MockRepository mockRepository = new MockRepository();

            IDataReader dataReader = mockRepository.Stub<IDataReader>();
            SetupResult.For(dataReader.GetSchemaTable()).Return(dataTable);

            mockRepository.ReplayAll();

            Object returnObject = dataReader.GetFieldForNamedColumn("table", "missing_column");

            Assert.AreEqual(default(object), returnObject, "Should be the default() version of the object.");

        }

        [Test]
        public void ReturnsObjectIfColumnExists()
        {

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Name", typeof(String));

            MockRepository mockRepository = new MockRepository();

            IDataReader dataReader = mockRepository.Stub<IDataReader>();
            SetupResult.For(dataReader.GetSchemaTable()).Return(dataTable);
            SetupResult.For(dataReader["name"]).Return("NameValue");

            mockRepository.ReplayAll();

            String returnObject = dataReader.GetFieldForNamedColumn("table", "name") as String;

            Assert.AreEqual(typeof(String), returnObject.GetType(), "Should be the same type/object field.");
            Assert.AreEqual("NameValue", returnObject, "Should be current row's field value.");

        }

    }

}
