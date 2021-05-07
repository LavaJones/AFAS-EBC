using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Afas.AfComply.UI.Areas.ViewModels.Reporting
{
    /// <summary>
    /// Summary and 1095C Part 1 information for the employee
    /// </summary>
    public class Employee1095summaryViewModel : BaseViewModel
    {

        /// <summary>
        /// If the Employee's 1095 data has been reviewed
        /// </summary>
        public bool Reviewed { get; set; }

        /// <summary>
        /// If the employee is Receiving a 1095
        /// </summary>
        public bool Receiving1095 { get; set; }

        /// <summary>
        /// The employee's first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The employee's middle name
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// The employee's last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The last for of the employee's SSN
        /// </summary>
        public string SsnHidden { get; set; }

        /// <summary>
        /// The Street Portion of the employees address
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// The City portion of the employees address
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// The State portion of the employee's address
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// The first 5 of the employee's zipcode
        /// </summary>
        public int Zip { get; set; }

        /// <summary>
        /// The employee's Zipcode's +4 section
        /// </summary>
        public int ZipPlus4 { get; set; }

        /// <summary>
        /// The Employee's Hire Date 
        /// </summary>
        public DateTime HireDate { get; set; }

        /// <summary>
        /// The employee's Termination Date if they have one
        /// </summary>
        public DateTime? TermDate { get; set; }

        /// <summary>
        /// The Tax year that this data is for.
        /// </summary>
        public int TaxYear { get; set; }

        /// <summary>
        /// The Employee's Classification, for filtering
        /// </summary>
        public string EmployeeClass { get; set; }

        /// <summary>
        /// All the Part 2 information 
        /// </summary>
        public List<Employee1095detailsPart2ViewModel> EmployeeMonthlyDetails { get; set; }

        /// <summary>
        /// Any Part 3 information for this employee
        /// </summary>
        public List<Employee1095detailsPart3ViewModel> CoveredIndividuals { get; set; }

        /// <summary>
        /// Value for the UI to store if the Part 2 has been loaded for this item
        /// </summary>
        public bool IsPart2Loaded = true;

        /// <summary>
        /// Value for the UI to store if the Part 3 has been loaded for this item
        /// </summary>
        public bool IsPart3Loaded = true;

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

        private string _thisUrlParameter = null;

        /// <summary>
        /// Link to the Update Part 2 method with the encrypted parameters attached to the end
        /// </summary>
        public virtual string UpdatePart1Link
        {
            get
            {
                return this.GetEncyptedLink("Update");
            }
        }

        /// <summary>
        /// Link to the update Part 2 method with the encrypted parameters attached to the end
        /// </summary>
        public virtual string UpdateMultiplePart2Link
        {
            get
            {
                return this.GetEncyptedLink("UpadatePart2s");
            }
        }

        /// <summary>
        /// Link to the Load Part 2 method with the encrypted parameters attached to the end
        /// </summary>
        public virtual string LoadPart2ItemLink
        {
            get
            {
                return this.GetEncyptedLink("LoadPart2");
            }
        }

        /// <summary>
        /// Link to the Load Part 3 method with the encrypted parameters attached to the end
        /// </summary>
        public virtual string LoadPart3ItemLink
        {
            get
            {
                return this.GetEncyptedLink("LoadPart3");
            }
        }

        public virtual string AddPart3ICItemLink
        {
            get
            {

                return this.GetEncyptedLink("AddPart3IC");
            }
        }

        public virtual string Finalize1095ItemLink
        {
            get
            {
                return this.GetEncyptedLink("Finalize1095");
            }
        }

        public virtual string Review1095ItemLink
        {
            get
            {
                return this.GetEncyptedLink("Review1095");
            }
        }        

    }

}