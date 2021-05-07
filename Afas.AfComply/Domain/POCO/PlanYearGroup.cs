using Afas.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Domain.POCO
{
    public class PlanYearGroup : BaseAfasModel
    {
        public int PlanYearGroupId { get; set; }

        public string GroupName { get; set; }

        public int Employer_id { get; set; }
    }
}
