using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Afas.AfComply.Reporting.Domain.Approvals
{
    /// <summary>
    /// A single Individuals 1095C Part 3 information
    /// </summary>
    public class Approved1095FinalPart3 : BaseReportingModel
    {

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


        [NotMapped]
        public virtual bool? EnrolledAll12
        {
            get
            {
                if (EnrolledJan && EnrolledFeb && EnrolledMar && EnrolledApr && EnrolledMay && EnrolledJun
                    && EnrolledJul && EnrolledAug && EnrolledSep && EnrolledOct && EnrolledNov && EnrolledDec
                    ||
                    !EnrolledJan && !EnrolledFeb && !EnrolledMar && !EnrolledApr && !EnrolledMay && !EnrolledJun
                     && !EnrolledJul && !EnrolledAug && !EnrolledSep && !EnrolledOct && !EnrolledNov && !EnrolledDec)
                {
                    return EnrolledJan;
                }

                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    EnrolledJan = value.Value;
                    EnrolledFeb = value.Value;
                    EnrolledMar = value.Value;
                    EnrolledApr = value.Value;
                    EnrolledMay = value.Value;
                    EnrolledJun = value.Value;
                    EnrolledJul = value.Value;
                    EnrolledAug = value.Value;
                    EnrolledSep = value.Value;
                    EnrolledOct = value.Value;
                    EnrolledNov = value.Value;
                    EnrolledDec = value.Value;
                }
            }
        }

        [Required]
        public virtual bool EnrolledJan { get; set; }

        [Required]
        public virtual bool EnrolledFeb { get; set; }        

        [Required]
        public virtual bool EnrolledMar { get; set; }

        [Required]
        public virtual bool EnrolledApr { get; set; }

        [Required]
        public virtual bool EnrolledMay { get; set; }

        [Required]
        public virtual bool EnrolledJun { get; set; }

        [Required]
        public virtual bool EnrolledJul { get; set; }

        [Required]
        public virtual bool EnrolledAug { get; set; }

        [Required]
        public virtual bool EnrolledSep { get; set; }

        [Required]
        public virtual bool EnrolledOct { get; set; }

        [Required]
        public virtual bool EnrolledNov { get; set; }

        [Required]
        public virtual bool EnrolledDec { get; set; }

    }
}