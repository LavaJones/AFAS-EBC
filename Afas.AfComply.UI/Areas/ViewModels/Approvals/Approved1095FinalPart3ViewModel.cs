using Afc.Marketing.Models;
using System;
using System.ComponentModel.DataAnnotations;


namespace Afas.AfComply.UI.Areas.ViewModels
{
    /// <summary>
    /// A single Individuals 1095C Part 3 information
    /// </summary>
    public class Approved1095FinalPart3ViewModel : BaseViewModel
    {

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