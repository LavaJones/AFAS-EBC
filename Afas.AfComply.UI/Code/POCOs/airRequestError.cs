using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for airRequestError
/// </summary>
public class airRequestError
{
    private int id = 0;
    private int employer_id = 0;
    private int employee_id = 0;
    private string errorType = null;
    private int statusRequestID = 0;
    private int transmitterErrorID = 0;
    private string errorMessageCode = null;
    private string errorMessageText = null;
    private string firstName = null;
    private string lastName = null;
    private string originalUniqueSubmissionId;
    private string correctedUniqueSubmissionId;
    private string correctedUniqueRecordId;

	public airRequestError(int _id, string _errorType, int _statusRequestID, int _transmitterErrorID, string _errorMessageCode, string _errorMessageText)
	{
        this.id = _id;
        this.errorType = _errorType;
        this.statusRequestID = _statusRequestID;
        this.transmitterErrorID = _transmitterErrorID;
        this.errorMessageCode = _errorMessageCode;
        this.errorMessageText = _errorMessageText;
	}

    public airRequestError(int _id, string _errorType, int _statusRequestID, int _transmitterErrorID, string _errorMessageCode, string _errorMessageText, string _originalUniqueSubmissionId, string _correctedUniqueRecordId)
    {
        this.id = _id;
        this.errorType = _errorType;
        this.statusRequestID = _statusRequestID;
        this.transmitterErrorID = _transmitterErrorID;
        this.errorMessageCode = _errorMessageCode;
        this.errorMessageText = _errorMessageText;
        this.originalUniqueSubmissionId = _originalUniqueSubmissionId;
        this.correctedUniqueRecordId = _correctedUniqueRecordId;
    }


    public airRequestError(int _id, string _errorType, int _statusRequestID, int _transmitterErrorID, string _errorMessageCode, string _errorMessageText, string _originalUniqueSubmissionId, string _correctedUniqueSubmissionId, string _correctedUniqueRecordId)
    {
        this.id = _id;
        this.errorType = _errorType;
        this.statusRequestID = _statusRequestID;
        this.transmitterErrorID = _transmitterErrorID;
        this.errorMessageCode = _errorMessageCode;
        this.errorMessageText = _errorMessageText;
        this.originalUniqueSubmissionId = _originalUniqueSubmissionId;
        this.correctedUniqueSubmissionId = _correctedUniqueSubmissionId;
        this.correctedUniqueRecordId = _correctedUniqueRecordId;
    }

    /// <summary>
    /// Error ID, unique to each error row in the database. 
    /// </summary>
    public int RE_ID
    {
        get { return this.id; }
        set { this.id = value; }
    }

    public int EMPLOYER_ID
    {
        get { return this.employer_id; }
        set { this.employer_id = value; }
    }

    public int EMPLOYEE_ID
    {
        get { return this.employee_id; }
        set { this.employee_id = value; }
    }

    /// <summary>
    /// Error TYPE, looks like this is a single char, not exactly sure what it represents at this point. 
    /// </summary>
    public string RE_TYPE
    {
        get { return this.errorType; }
        set { this.errorType = value; }
    }

    /// <summary>
    /// Links the individual error line to the actual Status_Request table in the database. 
    /// </summary>
    public int RE_STATUS_REQUEST_ID
    {
        get { return this.statusRequestID; }
        set { this.statusRequestID = value; }
    }

    /// <summary>
    /// Shows the line item integer value, essentially looks like it's just a count for the rows of errors in the Ack file. 
    /// </summary>
    public int RE_TRANSMITTER_ERROR_ID
    {
        get { return this.transmitterErrorID; }
        set { this.transmitterErrorID = value; }
    }

    /// <summary>
    /// Displays the actual Error Code. 
    /// </summary>
    public string RE_ERROR_CODE
    {
        get { return this.errorMessageCode; }
        set { this.errorMessageCode = value; }
    }

    /// <summary>
    /// Displays the actual Error Message. 
    /// </summary>
    public string RE_ERROR_TEXT
    {
        get { return this.errorMessageText; }
        set { this.errorMessageText = value; }
    }

    /// <summary>
    /// Displays the actual Error Message. 
    /// </summary>
    public string RE_ERROR_FIRST_NAME
    {
        get { return this.firstName; }
        set { this.firstName = value; }
    }

    /// <summary>
    /// Displays the actual Error Message. 
    /// </summary>
    public string RE_ERROR_LAST_NAME
    {
        get { return this.lastName; }
        set { this.lastName = value; }
    }

    public string ORIGINAL_UNIQUE_SUBMISSION_ID
    {
        get { return this.originalUniqueSubmissionId; }
        set { this.originalUniqueSubmissionId = value; }
    }

    public string CORRECTED_UNIQUE_SUBMISSION_ID
    {
        get { return this.correctedUniqueSubmissionId; }
        set { this.correctedUniqueSubmissionId = value; }
    }

    public string CORRECTED_UNIQUE_RECORD_ID
    {
        get { return this.correctedUniqueRecordId; }
        set { this.correctedUniqueRecordId = value; }
    }

}