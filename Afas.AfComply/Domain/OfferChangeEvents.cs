using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Domain
{

    /// <summary>
    /// The different types of insurance/offer/change events that can be detected during extended offer conversions.
    /// </summary>
    public static class OfferChangeEvents
    {
        public static String Discrepancy { get { return "OFFER-FILE-REJECTION"; } }
        public static String InsuranceChange { get { return "INSURANCE-CHANGE"; } }
        public static String SimpleOffer { get { return "SIMPLE-OFFER"; } }
    }

}
