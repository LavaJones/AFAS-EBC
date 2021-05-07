using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Afc.Marketing.Models;

namespace Afas.AfComply.UI.Areas.ViewModels
{

    /// <summary>
    /// The Final approved data for the employer
    /// </summary>
    public class Approved1095FinalViewModel : BaseViewModel
    {

        public virtual bool Receiving1095 { get; set; }

        public virtual bool Printed { get; set; }

        [Required]
        public virtual string FirstName { set; get; }

        public virtual string MiddleName { set; get; }

        [Required]
        public virtual string LastName { set; get; }

        [Required]
        public virtual string SSN { set; get; }

        [Required]
        public virtual string StreetAddress { set; get; }

        [Required]
        public virtual string City { set; get; }

        [Required]
        public virtual string State { set; get; }

        [Required]
        public virtual string Zip { set; get; }

        [Required]
        public virtual bool SelfInsured { set; get; }

        [Required]
        public virtual int TaxYear { set; get; }

        public virtual List<Approved1095FinalPart2ViewModel> part2s { set; get; }

        public virtual List<Approved1095FinalPart3ViewModel> part3s { set; get; }

        /// <summary>
        /// This is the url parameter for this object
        /// </summary>
        public override string ThisUrlParameter
        {
            get
            {
                if (this._thisUrlParameter != null)
                {
                    return _thisUrlParameter;
                }

                Dictionary<string, string> dict = new Dictionary<string, string>();
                dict.Add("TaxYear", this.TaxYear.ToString());
                this._thisUrlParameter = GetEncryptedParameters(dict);
                return this._thisUrlParameter;
            }
        }

        // private backing variable to cache encrypted parameters results
        private string _thisUrlParameter = null;

        public virtual string Unfinalize1095ItemLink
        {
            get
            {
                return this.GetEncyptedLink("UnFinalize1095");
            }
        }

        
    }
}
