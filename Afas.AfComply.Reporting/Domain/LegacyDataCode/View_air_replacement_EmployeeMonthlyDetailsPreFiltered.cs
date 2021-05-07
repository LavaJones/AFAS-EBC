using Afas.AfComply.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.LegacyData
{
    public partial class View_air_replacement_EmployeeMonthlyDetailsPreFiltered
    {

        public bool? Receiving1095C
        {
            get
            {
                if (MonthlyAverageHours >= 130 
                    || 
                        (this.aca_status_id == (int)ACAStatusEnum.FullTime 
                        && 
                        this.IsNewHire))
                {
                    return true;
                }

                return false;
            }
        }
    }
}
