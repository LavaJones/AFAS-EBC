using Afc.Marketing.Models;
using System;
using System.ComponentModel.DataAnnotations;


namespace Afas.AfComply.Reporting.Core.Models
{ 
    /// <summary>
    /// A single Individuals 1095C Part 3 information
    /// </summary>
    public class Approved1095FinalPart3Model : Model
    {

        public Approved1095FinalPart3Model()
        {
            Enrolled = new bool[13];
        }

        /// <summary>
        /// Individuals Row ID from Insurance Coverage table
        /// </summary>
        public virtual int InsuranceCoverageRowID { get; set; }

        /// <summary>
        /// if the person is Employee then Employee ID
        ///</summary>

        [Required]
        public virtual int EmployeeID { get; set; }

        /// <summary>
        /// if the person is Dependant then Dependant ID
        ///</summary>
        ///
        [Required]
        public virtual int DependantID { get; set; }

        /// <summary>
        /// The Tax year that this data is for.
        /// </summary>
        [Required]
        public virtual int TaxYear { get; set; }

        /// <summary>
        /// Individuals First Name
        /// </summary>
        [Required]
        public virtual string FirstName { get; set; }

        /// <summary>
        /// Individuals Middle Name
        /// </summary>
        public virtual string MiddleName { get; set; }

        /// <summary>
        /// Individuals Last Name
        /// </summary>
        [Required]
        public virtual string LastName { get; set; }

        /// <summary>
        /// SSN
        /// </summary>
        public virtual string SSN { get; set; }

        /// <summary>
        /// The individuals Date of Birth
        /// </summary>
        public virtual DateTime? Dob { get; set; }

        /// <summary>
        /// A list of enrollments for all 12 months
        /// </summary>
        [Required]
        public virtual bool[] Enrolled { get; set; }

    }
}