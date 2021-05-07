using Afas.AfComply.Reporting.Domain.Approvals;
using Afas.AfComply.Reporting.Domain.Voids;
using Afc.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.Printing
{
    public static class Print1095QuerableExtensionMethods
    {

        /// <summary>
        /// Filter the Print1095 by the Batch.
        /// </summary>
        /// <param name="Print1095s">The object list to filter.</param>
        /// <param name="printBatch">The actual batch object to filter on.</param>
        /// <returns>The filtered query.</returns>
        public static IQueryable<Print1095> FilterForPrintBatchIds(this IQueryable<Print1095> Print1095s, IList<long> BatchIds)
        {

            return (
                    from Print1095 in Print1095s
                    where BatchIds.Contains( Print1095.PrintBatch.ID )
                    select Print1095
                    );

        }

        /// <summary>
        /// Filter the Print1095 by the Batch.
        /// </summary>
        /// <param name="Print1095s">The object list to filter.</param>
        /// <param name="printBatch">The actual batch object to filter on.</param>
        /// <returns>The filtered query.</returns>
        public static IQueryable<Print1095> FilterForPrintBatch(this IQueryable<Print1095> Print1095s, PrintBatch printBatch)
        {

            return (
                    from Print1095 in Print1095s
                    where Print1095.PrintBatch == printBatch
                    select Print1095
                    );

        }

        /// <summary>
        /// Filter the Print1095 by the correction status.
        /// </summary>
        /// <param name="print1095s">The object list to filter.</param>
        /// <param name="approved1095">The approved object to filter by.</param>
        /// <returns>The filtered query.</returns>
        public static IQueryable<Print1095> FilterForApproval1095(this IQueryable<Print1095> print1095s, Approved1095Final approval1095)
        {

            return (
                    from print1095 in print1095s
                    where print1095.Approved1095 == approval1095
                    select print1095
                    );

        }

        /// <summary>
        /// Filter the Print1095 by the Batch.
        /// </summary>
        /// <param name="Print1095s">The object list to filter.</param>
        /// <param name="printBatch">The actual batch object to filter on.</param>
        /// <returns>The filtered query.</returns>
        public static IQueryable<long> FilterToOnlyApproved1095Ids(this IQueryable<Print1095> Print1095s)
        {

            return (
                    from Print1095 in Print1095s
                    select Print1095.Approved1095.ID
                    );

        }


    }
}