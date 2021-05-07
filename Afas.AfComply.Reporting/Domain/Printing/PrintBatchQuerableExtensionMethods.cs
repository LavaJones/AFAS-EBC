using Afc.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.Printing
{
    /// <summary>
    /// Common Data Filtering extension methods for the  object
    /// </summary>
    public static class PrintBatchQuerableExtensionMethods
    {

        public static IQueryable<PrintBatch> FilterForReprinted(this IQueryable<PrintBatch> PrintBatches)
        {

            return (
                    from batch in PrintBatches
                    where batch.Reprint == true
                    select batch
                    );

        }

        public static IQueryable<PrintBatch> FilterForOrigonal(this IQueryable<PrintBatch> PrintBatches)
        {

            return (
                    from batch in PrintBatches
                    where batch.Reprint == false
                    select batch
                    );

        }

        public static IQueryable<PrintBatch> FilterForRequestor(this IQueryable<PrintBatch> PrintBatches, string username)
        {

            return (
                    from batch in PrintBatches
                    where batch.RequestedBy == username
                    select batch
                    );

        }

        public static IQueryable<PrintBatch> FilterForEmployerId(this IQueryable<PrintBatch> PrintBatches, int employerId)
        {

            return (
                    from batch in PrintBatches
                    where batch.EmployerId == employerId
                    select batch
                    );

        }


        public static IQueryable<PrintBatch> FilterForTaxYear(this IQueryable<PrintBatch> PrintBatches, int taxYear)
        {

            return (
                    from batch in PrintBatches
                    where batch.TaxYear == taxYear
                    select batch
                    );

        }


        public static IQueryable<PrintBatch> FilterForFileName(this IQueryable<PrintBatch> PrintBatches, string fileName)
        {

            return (
                    from batch in PrintBatches
                    where batch.PrintFileName == fileName
                    select batch
                    );

        }
    }
}
