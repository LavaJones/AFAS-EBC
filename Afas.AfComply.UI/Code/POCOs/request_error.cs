using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class request_error
{
    public int error_id { get; set; }
    public String error_type { get; set; }
    public int status_request_id { get; set; }
    public int transmitter_error_id { get; set; }
    public String error_message_code { get; set; }
    public String error_message_text { get; set; }
    public String x_path_content { get; set; }

}