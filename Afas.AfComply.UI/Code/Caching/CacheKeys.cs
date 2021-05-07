using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Afas.AfComply.UI.Code.Caching
{
    
    public static class CacheKeys
    {

        public static class Employee
        {

            public static String CacheKeyHeader { get { return "ZZEE"; } }

            public static String EmployeeAirCoverage { get { return "ZZEE_E316298A-A1DE-4A0E-8799-5FA9EECB5473"; } }

            public static String EmployeeCoverageFromCarrierReport { get { return "ZZEE_8691A8E5-FBAF-4E6C-A32D-FF347A7B3B37"; } }

            public static String EmployeeIdsFlaggedFor1095 { get { return "ZZEE_38282649-58E2-4182-8B81-10DF0EF5BD85"; } }

            public static String EmployeeIdsInCarrierReport { get { return "ZZEE_07A8E352-FCFA-46C5-99AC-5AD61543F4D7"; } }

            public static String EmployeeMonthlyDetailList { get { return "ZZEE_AAC70794-EC43-45C5-87A0-169E96420FB8"; } }

            public static String EmployeesPending1095Review { get { return "ZZEE_3A712312-0599-4729-B06C-33BD1F67B141"; } }

            public static String EmployeesPending1095Corrections { get { return "ZZEE_AF88A4D9-69C5-4384-AE32-01BAB204C2A9"; } }

            public static String RemainingEmployeesNeeding1095Approval { get { return "ZZEE_944520F7-840C-4113-883E-99B57B134982"; } }
            
        }

        public static class Employer
        {

            public static String CacheKeyHeader { get { return "ZZER"; } }

            public static String EmployeeClassifications { get { return "ZZER_29F703E9-C81A-4758-9284-528F73169A8C"; } }

        }

        public static class Generic
        {

            public static String CacheKeyHeader { get { return "ZZGN"; } }

            public static String AffordableSafeHarborCodes { get { return "ZZGN_1FE36ED4-A642-4CCC-800F-028F19C5DCC3"; } }

            public static String AirTimeFrameIds { get { return "ZZGN_DB7D510E-B940-4C5A-887F-C8BA3448096B"; } }

            public static String InsuranceTypes { get { return "ZZGN_1848AD98-0E37-470B-87DD-816726A9B163"; } }

            public static String OfferOfCoverageCodes { get { return "ZZGN_443507A8-95A1-41F3-9838-19E43F5E4EE3"; } }

            public static String StatusList { get { return "ZZGN_0CFB95D9-45E1-4ACD-9CE4-39CE526673E8"; } }

        }

        public static class Irs1095
        {

            public static String CacheKeyHeader { get { return "ZZIR"; } }

        }

    }

}