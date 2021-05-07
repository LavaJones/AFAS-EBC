using Afc.Marketing.Models;
using System;
using System.Collections.Generic;

namespace Afas.AfComply.Reporting.Core.Models
{
     public class Employer1094SummaryModel : Model
    {
        public Employer1094SummaryModel() : base()
        { }
        public  int EmployerID { set; get; }
        public string EmployerName { get; set; }
        public string EmployerDBAName { get; set; }
        public bool? IsDge { get; set; }
        public int TaxYearId { get; set; }
        public string EIN { set; get; }
        public  Guid EmployerResourceId { set; get; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string IrsContactName { get; set; }
        public string IrsContactPhone { get; set; }
        public  int? StateId { set; get; }

        //Designated Government Entity(Dge)
        public string DgeName { get; set; }
        public string DgeEIN { set; get; }
        public string DgeAddress { get; set; }
        public string DgeCity { get; set; }
        public  int? DgeStateId { set; get; }
        public string DgeState { get; set; }
        public string DgeZipCode { get; set; }
        public string DgeContactName { get; set; }
        public string DgeContactPhone { get; set; }
        public int TransmissionTotal1095Forms { get; set; }
        public bool IsAuthoritiveTransmission { get; set; }
        public bool? IsAggregatedAleGroup { get; set; }
        public int? Total1095Forms { get; set; }
        public List<Employer1094detailsPart3Model> Employer1094Part3s { get; set; }
        public List<Employer1094detailsPart4Model> Employer1094Part4s { get; set; }

    }
}
