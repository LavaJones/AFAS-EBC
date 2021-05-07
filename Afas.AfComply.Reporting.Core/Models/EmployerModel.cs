using System;
using Afc.Marketing.Models;
using System.Collections.Generic;

namespace Afas.AfComply.Reporting.Core.Models
{
    public class EmployerModel : Model
    {
        public int EmployerId { set; get; }
        public string Name { set; get; }
        public string Address { set; get; }
        public string City { set; get; }
        public int? StateId { set; get; }
        public string Zip { set; get; }
        public string ImgLogo { set; get; }
        public string BillAddress { set; get; }
        public string BillCity { set; get; }
        public int? BillState { set; get; }
        public string BillZip { set; get; }
        public int? EmployerTypeId { set; get; }
        public string EmployerType { set; get; }
        public string EIN { set; get; }
        public int? InitialMeasurementId { set; get; }
        public string ImportDemo { set; get; }
        public string ImportPayroll { set; get; }
        public string IEI { set; get; }
        public string IEC { set; get; }
        public string FTPEI { set; get; }
        public string FTPEC { set; get; }
        public string IPI { set; get; }
        public string IPC { set; get; }
        public string FTPPI { set; get; }
        public string FTPPC { set; get; }
        public string ImportProcess { set; get; }
        public int? VendorId { set; get; }
        public bool AutoUpload { set; get; }
        public bool AutoBill { set; get; }
        public bool? SuBilled { set; get; }
        public string ImportGP { set; get; }
        public string ImportHR { set; get; }
        public string ImportEC { set; get; }
        public string ImportIO { set; get; }
        public string ImportIC { set; get; }
        public string ImportPayMod { set; get; }
        public Guid ResourceId { set; get; }
        public string DBAName { set; get; }
        public bool IrsEnabled { set; get; }
        public int Feeid { set; get; }
    }
}
