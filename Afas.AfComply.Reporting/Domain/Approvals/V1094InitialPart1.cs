using System;
using System.ComponentModel.DataAnnotations;

using Afc.Core;
using Afc.Core.Domain;

namespace Afas.AfComply.Reporting.Domain.Approvals
{

    /// <summary>
    /// Pulls the live data for the employer, tax year approval to complete the 1094 Part 1.
    /// </summary>
    public class V1094InitialPart1 : BaseReportingModel
    {

        public V1094InitialPart1() : base()
        {

        }

        [Required]
        public virtual String Address { get; set; }

        [Required]
        public virtual String City { get; set; }

        public virtual String DBAName { get; set; }

        public virtual String DgeAddress { get; set; }

        public virtual String DgeCity { get; set; }

        public virtual String DgeContactName { get; set; }

        public virtual String DgeContactPhone { get; set; }

        public virtual String DgeEIN { set; get; }

        public virtual String DgeName { get; set; }

        public virtual int DgeStateId { set; get; }

        public virtual String DgeZipCode { get; set; }

        [Required]
        public virtual String EIN { set; get; }

        [Required]
        public virtual int EmployerId { set; get; }

        public virtual String EmployerDBAName { get; set; }

        [Required]
        public virtual String EmployerName { get; set; }

        [Required]
        public virtual String IrsContactName { get; set; }

        [Required]
        public virtual String IrsContactPhone { get; set; }

        [Required]
        public virtual Boolean IsAuthoritiveTransmission { set; get; }

        [Required]
        public virtual Boolean IsDge { set; get; }

        [Required]
        public virtual int TaxYearId { set; get; }

        [Required]
        public virtual int TransmissionTotal1095Forms { set; get; }

        [Required]
        public virtual int StateId { set; get; }

        [Required]
        public virtual String ZipCode { get; set; }

    }

}
