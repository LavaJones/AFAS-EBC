using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class header
{
    public int header_id { get; set; }
    public Guid universally_unique_id { get; set; }
    public String transmitter_control_code { get; set; }
    public String unique_transmission_id { get; set; }
    public DateTime transmission_timestamp { get; set; }
    public int message_type_id { get; set; }
    public int? _1094c_id { get; set; }
    public String transmitted_base_64 { get; set; }
}