using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Afas.AfComply.Domain;

 public class taxYearEmployerTransmission
    {
        public int tax_year_employer_transmissionID { get; set; }
        public int tax_year { get; set; }
        public int employee_id { get; set; }
        public int employer_id { get; set; }
        public Guid ResourceId { get; set; }
        public string TransmissionType { get; set; }
        public string UniqueTransmissionId { get; set; }
        public string ReceiptID { get; set; }
        public string UniqueSubmissionId { get; set; }
        public int RecordID { get; set; }
        public int transmission_status_code_id { get; set; }
        public string OriginalReceiptId { get; set; }
        public string OriginalUniqueSubmissionId { get; set; }
        public int EntityStatusId { get; set; }
        public string createdBy { get; set; }
        public DateTime createdOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string BulkFile { get; set; }
        public string manifestFile { get; set; }
        public string AckFile { get; set; }

        public int FormCount { get; set; }
        public bool Is1094Only { get { return FormCount <= 0; } }

        public string getUSID
        {
            get
            {
                return ReceiptID + '|' + UniqueSubmissionId;
            }
        }
    }
