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
    public class Approved1095Initial : BaseReportingModel
    {

        public virtual int employerID { set; get; }

        public virtual int employeeID { set; get; }

        public virtual DateTime terminationDate { set; get; }

        public virtual int acaStatusID { set; get; }

        public virtual int classificationID { set; get; }

        public virtual int monthID { set; get; }

        public virtual int monthlyAverageHours { set; get; }

        public virtual string defaultOfferOfCoverage { set; get; }

        public virtual string mec { set; get; }

        public virtual string defaultSafeHarborCode { set; get; }

        public virtual double employeesMonthlyCost { set; get; }

        public virtual bool employeeOfferedCoverage { set; get; }

        public virtual bool employeeEnrolledInCoverage { set; get; }

        public virtual int insuranceType { set; get; }

        public virtual bool offeredToSpouse { set; get; }

        public virtual bool OfferedToSpouseConditional { set; get; }

        public virtual bool mainLand { set; get; }

        public virtual DateTime coverageEffectiveDate { set; get; }

        public virtual bool fullyPlusSelfInsured { set; get; }

        public virtual int minValue { set; get; }

        public virtual int waitingPeriodID { set; get; }

        public virtual int taxYear { set; get; }

        public virtual int planYearID { set; get; }

        public virtual DateTime hireDate { set; get; }

        public virtual string line14 { set; get; }

        public virtual string line15 { set; get; }

        public virtual string line16 {set; get; }
    }
}
