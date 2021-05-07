using Afas.AfComply.Reporting.Domain.Approvals;
using Afas.AfComply.Reporting.Domain.Corrections;
using Afas.AfComply.Reporting.Domain.Voids;
using Afc.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.Printing
{
    /// <summary>
    /// Common Data Filtering extension methods for the TimeFrame object
    /// </summary>
    public static class Print1094QuerableExtensionMethods
    {

        /// <summary>
        /// Filter the Print1094 by the Batch.
        /// </summary>
        /// <param name="print1094s">The object list to filter.</param>
        /// <param name="printBatch">The actual batch object to filter on.</param>
        /// <returns>The filtered query.</returns>
        public static IQueryable<Print1094> FilterForPrintBatch(this IQueryable<Print1094> print1094s, PrintBatch printBatch)
        {

            return (
                    from print1094 in print1094s
                    where print1094.PrintBatch == printBatch
                    select print1094
                    );

        }

        /// <summary>
        /// Filter the Print1094 by the correction status.
        /// </summary>
        /// <param name="print1094s">The object list to filter.</param>
        /// <param name="approved1094">The approved object to filter by.</param>
        /// <returns>The filtered query.</returns>
        public static IQueryable<Print1094> FilterForApproval1094(this IQueryable<Print1094> print1094s, Approved1094FinalPart1 approval1094)
        {

            return (
                    from print1094 in print1094s
                    where print1094.Approved1094 == approval1094
                    select print1094
                    );

        }
    }
}
