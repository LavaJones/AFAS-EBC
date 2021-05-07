using System;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Afc.Marketing.Models;


namespace Afas.AfComply.Reporting.Core.Models
{
    public class Approved1094FinalPart1Model : Model
    {
       

       

        [Required]
        public virtual List<Approved1094FinalPart3Model> Approved1094FinalPart3Models { get; set; }

        [Required]
        public virtual List<Approved1094FinalPart4Model> Approved1094FinalPart4Models { get; set; }

        [Required]
        public virtual String Address { get; set; }

        [Required]
        public virtual String City { get; set; }

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
        public virtual Guid EmployerResourceId { get; set; }
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
        [Required]
        public virtual int Total1095Forms { get; set; }

        [Required]
        public virtual Boolean IsAggregatedAleGroup { get; set; }

    }
}

