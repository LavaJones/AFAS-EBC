using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class status_request
{
    public int status_request_id { get; set; }
    public int header_id { get; set; }
    public String receipt_id { get; set; }
    public int status_code_id { get; set; }
    public String sr_base_64 { get; set; }
    public String return_time_utc { get; set; }
    public DateTime return_time_local { get; set; }
    public String return_time_zone { get; set; }

}