using Afc.Core;
using Afc.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.Approvals
{
    public class Approved1094Final : BaseReportingModel
    {

        [Required]
        public virtual int EmployerID { set; get; }

        [Required]
        public virtual Guid EmployerResourceId { set; get; }

        [Required]
        public virtual int EIN { set; get; }

        [Required]
        public string BusinessNameLine1 { get; set; }

        [Required]
        public string BusinessAddressLine1Txt { get; set; }

        [Required]
        public string BusinessCityNm { get; set; }

        [Required]
        public string BusinessUSStateCd { get; set; }

        [Required]
        public string BusinessUSZipCd { get; set; }

        [Required]
        public string ContactPhoneNum { get; set; }


        //TODO other 1094 data


        public override IList<ValidationMessage> EnsureIsWellFormed
        {

            get
            {

                IList<ValidationMessage> validationMessages = base.EnsureIsWellFormed;

                //SharedUtilities.ValidateDate(this.ApprovedOn, "ApprovedOn", validationMessages);
                //SharedUtilities.ValidateString(this.ApprovedBy, "ApprovedBy", validationMessages);

                return validationMessages;

            }

        }
    }
}
