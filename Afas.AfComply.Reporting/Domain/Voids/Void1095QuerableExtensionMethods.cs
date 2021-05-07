using Afc.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.Voids
{
    public static class Void1095QuerableExtensionMethods
    {

        /// <summary>
        /// Filter the Time Frames for ony those in a specific year.
        /// </summary>
        /// <param name="Void1095">The object to filter.</param>
        /// <param name="username">The username to filter for.</param>
        /// <returns>The filtered query.</returns>
        public static IQueryable<Void1095> FilterForVoidedBy(this IQueryable<Void1095> voids, string username)
        {
            return (
                    from voided in voids
                    where voided.VoidedBy == username
                    select voided
                    );
        }
    }
}

