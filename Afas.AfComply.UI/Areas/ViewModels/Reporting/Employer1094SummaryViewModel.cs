using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Afas.AfComply.UI.Areas.ViewModels.Reporting
{
    public class Employer1094SummaryViewModel : BaseViewModel
    {
     
        public string EmployerName { get; set; }
        public string EIN { set; get; }
        public Guid EmployerResourceId { set; get; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string IrsContactName { get; set; }
        public string IrsContactPhone { get; set; }
        public int? StateId { set; get; }
        public string EmployerDBAName { get; set; }
        public bool IsDge { get; set; }
        public int TaxYearId { get; set; }

        //Designated Government Entity(Dge)
        public string DgeName { get; set; }
        public string DgeEIN { set; get; }
        public string DgeAddress { get; set; }
        public string DgeCity { get; set; }
        public int DgeStateId { set; get; }
        public string DgeState { get; set; }
        public string DgeZipCode { get; set; }
        public string DgeContactName { get; set; }
        public string DgeContactPhone { get; set; }
        public int TransmissionTotal1095Forms { get; set; }
        public bool IsAuthoritiveTransmission { get; set; }
        public bool IsAggregatedAleGroup { get; set; }
        public int Total1095Forms { get; set; }
        public List<Employer1094detailsPart3ViewModel> Employer1094Part3s { get; set; }
        public List<Employer1094detailsPart4ViewModel> Employer1094Part4s { get; set; }
        public override string ThisUrlParameter
        {
            get
            {
                if (this._thisUrlParameter != null)
                {
                    return _thisUrlParameter;
                }

                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("TaxYear", this.TaxYearId.ToString());
                this._thisUrlParameter = GetEncryptedParameters(dict);
                return this._thisUrlParameter;
            }
        }
        private string _thisUrlParameter = null;
        public virtual string Finalize1094ItemLink
        {
            get
            {
                return this.GetEncyptedLink("Finalize1094");
            }
        }
    }
}