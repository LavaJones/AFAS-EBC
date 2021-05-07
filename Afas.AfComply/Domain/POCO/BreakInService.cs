using Afas.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Domain.POCO
{
    public class BreakInService : BaseAfasModel
    {
        public int BreakInServiceId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Weeks 
        { 
            get 
            {
                //We talked with Che and this math should work for now.
                return (int)Math.Ceiling((EndDate - StartDate).TotalDays / 7.0);
            } 
        }
        public int MeasurementId { get; set; }




    }
}
