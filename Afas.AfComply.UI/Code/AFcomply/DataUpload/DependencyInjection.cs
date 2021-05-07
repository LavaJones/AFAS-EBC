using System;

using Afas.AfComply.Application;
using Afas.AfComply.Domain;
using Afas.Application.CSV;

using Afas.AfComply.UI.Code.AFcomply.DataAccess;
using Afas.Application.FileAccess;
using Afas.Application.CSV;
using Afas.ImportConverter.Application;
using Afas.ImportConverter.Application.Implementation;

namespace Afas.AfComply.UI.Code.AFcomply.DataUpload
{

    /// <summary>
    /// Implements our current dependency injection strategy until we are able to integrate with Castle.
    /// </summary>
    public static class DependencyInjection
    {

        public static IFileAccess GetFileAccess() 
        {
            return new FileAccess();
        }

        public static ICsvParser GetCsvParser() 
        {
            return new CsvParse();
        }

        public static IHeaderValidator GetHeaderValidator() 
        {
            return new HeaderValidator();
        }

        public static IDataTableBuilder GetDataTableBuilder() 
        {
            return new DataTableBuilder(GetHeaderValidator());
        }

        public static ICsvFileDataTableBuilder GetCsvFileDataTableBuilder() 
        {
            return new CsvDataTableBuilder(GetFileAccess(), GetDataTableBuilder(), GetCsvParser());
        }

        public static DemographicDataTableImporter GetDemoImporter() 
        {
            return new DemographicDataTableImporter();
        }

        public static OfferDataTableImporter GetOfferImporter()
        {
            return new OfferDataTableImporter();
        }
        public static Edit1095TableImporter GetEdit1095Importer()
        {
            return new Edit1095TableImporter();
        }
        
    }
}