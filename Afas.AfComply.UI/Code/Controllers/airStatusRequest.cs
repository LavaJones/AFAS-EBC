using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// This is the Acknowledgment File downloaded from the WebUI when checking on submission status of a specific receipt ID. 
/// </summary>
public class airStatusRequest
{
    private int id = 0;
    private int headerID = 0;
    private string receiptID = null;
    private int statusCodeID = 0;
    private string xmlBase64 = null;
    private DateTime? timestamp = null;

	public airStatusRequest(int _id, int _headerID, string _receiptID, int _statusCodeID, string _xmlBase64, DateTime? _timestamp)
	{
        this.id = _id;
        this.headerID = _headerID;
        this.receiptID = _receiptID;
        this.statusCodeID = _statusCodeID;
        this.xmlBase64 = _xmlBase64;
        this.timestamp = _timestamp;
	}

    /// <summary>
    /// This is the unique id for the status_request table. 
    /// </summary>
    public int SR_ID
    {
        get { return this.id; }
        set { this.id = value; }
    }

    /// <summary>
    /// This is the unique id from the manifest table. It links the ack file back to what file was submitted. 
    /// </summary>
    public int SR_HEADER_ID
    {
        get { return this.headerID; }
        set { this.headerID = value; }
    }

    /// <summary>
    /// This is the unique id from the manifest table. It links the ack file back to what file was submitted. 
    /// </summary>
    public string SR_RECEIPT_ID
    {
        get { return this.receiptID; }
        set { this.receiptID = value; }
    }

    /// <summary>
    /// This is the unique id from the sr.status_request table. It is the status of the file. 
    /// </summary>
    public int SR_STATUS_CODE_ID
    {
        get { return this.statusCodeID; }
        set { this.statusCodeID = value; }
    }

    /// <summary>
    /// This is the XML Ackknowledgment file stored in Base 64, the getter converts it back to XML.  
    /// </summary>
    public string SR_ACK_XML
    {
        get { return System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(this.xmlBase64)); }
        set { this.xmlBase64 = value; }
    }

    /// <summary>
    /// This is the Datetime that the Ack file was uploaded into the AIR system.  
    /// </summary>
    public DateTime? SR_ACK_TIMESTAMP
    {
        get { return this.timestamp; }
        set { this.timestamp = value; }
    }
}