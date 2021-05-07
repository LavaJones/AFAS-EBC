using Afas.AfComply.Reporting.Domain.Approvals;
using Afas.AfComply.Reporting.Domain.Printing;
using Afc.Core;
using Afc.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afas.AfComply.Reporting.Domain.Reporting
{
    public class UserEditPart2 : BaseReportingModel
    {

        /// <summary>
        /// 
        /// </summary>
        public virtual string OldValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string NewValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual int MonthId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual int TaxYear { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual int LineId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual int EmployeeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual int EmployerId { get; set; }
       
        public override IList<ValidationMessage> EnsureIsWellFormed
        {

            get
            {

                IList<ValidationMessage> validationMessages = base.EnsureIsWellFormed;

                //SharedUtilities.ValidateString(this.OldValue, "OldValue", validationMessages);// old value can be null

                //SharedUtilities.ValidateString(this.NewValue, "NewValue", validationMessages);

                if (this.MonthId >= 0 && this.MonthId > 12)
                {
                    validationMessages.Add(new ValidationMessage("MonthId", "Month Id is invalid", ValidationMessageSeverity.Error));
                }
                
                if (this.LineId != 0 && this.LineId != 14 && this.LineId != 15 && this.LineId != 16)
                {
                    validationMessages.Add(new ValidationMessage("LineId", "Line Id is invalid", ValidationMessageSeverity.Error));
                }

                if (this.TaxYear < 2000 || this.TaxYear > 2025)
                {
                    validationMessages.Add(new ValidationMessage("TaxYear", "TaxYear Id is invalid", ValidationMessageSeverity.Error));
                }

                if (this.EmployeeId <= 0)
                {
                    validationMessages.Add(new ValidationMessage("EmployeeId", "Employee Id is invalid", ValidationMessageSeverity.Error));
                }

                if (this.EmployerId <= 0)
                {
                    validationMessages.Add(new ValidationMessage("EmployerId", "Employer Id is invalid", ValidationMessageSeverity.Error));
                }

                return validationMessages;

            }
        }
    }
}
