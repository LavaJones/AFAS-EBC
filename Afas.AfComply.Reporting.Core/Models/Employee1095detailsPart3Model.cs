using Afc.Marketing.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Afas.AfComply.Reporting.Core.Models
{
    /// <summary>
    /// A single Individuals 1095C Part 3 information
    /// </summary>
    public class Employee1095detailsPart3Model : Model
    {
        public Employee1095detailsPart3Model() : base() { }


        /// <summary>
        /// Individuals Row ID from Insurance Coverage table
        /// </summary>
        public int InsuranceCoverageRowID { get; set; }

        /// <summary>
        /// if the person is Employee then Employee ID
        ///</summary>
        public int EmployeeID { get; set; }

        /// <summary>
        /// if the person is Dependant then Dependant ID
        ///</summary>
        public int DependantID { get; set; }

        /// <summary>
        /// Individuals First Name
        /// </summary>
        [Required]
        public string FirstName { get; set; }

        /// <summary>
        /// Individuals Middle Name
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Individuals Last Name
        /// </summary>
        [Required]
        public string LastName { get; set; }

        /// <summary>
        /// The Last 4 of the SSN
        /// </summary>
        public string SsnHidden { get; set; }

        /// <summary>
        /// SSN
        /// </summary>
        public string SSN { get; set; }

        /// <summary>
        /// The individuals Date of Birth
        /// </summary>
        public DateTime? Dob { get; set; }

        /// <summary>
        /// A list of enrollments for all 12 months
        /// </summary>
        [Required]
        public bool[] Enrolled { get; set; }

        /// <summary>
        /// The Tax year that this data is for.
        /// </summary>
        public int TaxYear { get; set; }

        /// 
        ///     
        ///         
    }
}