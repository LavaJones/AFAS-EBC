using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class manifest
{
    public int header_id { get; set; }
    public String unique_transmission_id { get; set; }
    public String payment_year { get; set; }
    public bool prior_year_indicator { get; set; }
    public String ein { get; set; }
    public char br_type_code { get; set; }
    public char test_file_indicator { get; set; }
    public String original_unique_transmission_id { get; set; }
    public bool transmitter_foreign_entity_indicator { get; set; }
    public String original_receipt_id { get; set; }
    public char vendor_indicator { get; set; }
    public int vendor_id { get; set; }
    public int payee_count {get; set;}
    public int payer_record_count { get; set; }
    public String software_id { get; set; }
    public String form_type { get; set; }
    public String binary_format { get; set; }
    public String checksum { get; set; }
    public int attachment_byte_size { get; set; }
    public String document_system_file_name { get; set; }
    public String mtom { get; set; }
    public String manifest_file_name { get; set; }

}