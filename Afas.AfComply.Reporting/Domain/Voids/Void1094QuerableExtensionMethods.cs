using Afc.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.Voids
{
    /// <summary>
    /// Common Data Filtering extension methods for the TimeFrame object
    /// </summary>
    public static class Void1094QuerableExtensionMethods
    {

        /// <summary>
        /// Filter the Time Frames for ony those in a specific year.
        /// </summary>
        /// <param name="Void1094">The object to filter.</param>
        /// <param name="username">The username to filter for.</param>
        /// <returns>The filtered query.</returns>
        public static IQueryable<Void1094> FilterForVoidedBy(this IQueryable<Void1094> voids, string username)
        {
            return (
                    from voided in voids
                    where voided.VoidedBy == username
                    select voided
                    );
        }
    }
}
