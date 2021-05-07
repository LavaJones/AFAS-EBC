using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Afc.Marketing.Models;

namespace Afas.AfComply.UI.Areas.ViewModels
{
    public class Approved1094FinalViewModel : BaseViewModel
    {

        public Approved1094FinalViewModel() : base() { }

        public virtual int EmployerID { set; get; }
        public virtual string EmployerName { get; set; }
        public virtual string EmployerDBAName { get; set; }
        public virtual bool IsDge { get; set; }
        public virtual int TaxYearId { get; set; }
        public virtual string EIN { set; get; }
        public virtual Guid EmployerResourceId { set; get; }
        public virtual string Address { get; set; }
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        public virtual string ZipCode { get; set; }
        public virtual string IrsContactName { get; set; }
        public virtual string IrsContactPhone { get; set; }
        public virtual int? StateId { set; get; }

        public virtual string DgeName { get; set; }
        public virtual string DgeEIN { set; get; }
        public virtual string DgeAddress { get; set; }
        public virtual string DgeCity { get; set; }
        public virtual int DgeStateId { set; get; }
        public virtual string DgeZipCode { get; set; }
        public virtual string DgeContactName { get; set; }
        public virtual string DgeContactPhone { get; set; }
        public virtual int TransmissionTotal1095Forms { get; set; }
        public virtual bool IsAuthoritiveTransmission { get; set; }
        public virtual bool IsAggregatedAleGroup { get; set; }
        public virtual int Total1095Forms { get; set; }
        public virtual List<Approved1094FinalPart3ViewModel> Employer1094Part3s { get; set; }
        public virtual List<Approved1094FinalPart4ViewModel> Employer1094Part4s { get; set; }
        private string _thisUrlParameter = null;
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
    }
}

