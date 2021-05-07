using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

    public class submission
    {
        private int headerID = 0;
        private string uniqueTransmissionID = null;
        private string uniqueTransmitterCode = null;
        private string tcc = null;
        private string shipmentRecordID = null;
        private string receiptID = null;
        private string formType = null;
        private DateTime? responseTime = null;
        private int statusID = 0;
        private string fileName = null;
        private string checksum = null;
        private int byteSize = 0;
        private string errorCode = null;
        private string errorMessage = null;
        private string xpath_content = null;

        public submission(int _headerID, string _uniqueTransmissionID, string _uniqueTransmitterID, string _tcc, string _shipmentID, string _receiptID, string _formType, DateTime? _timeStamp, int _statusCodeID, string _fileName, string _checksum, int _attachmentSize, string _errorCode, string _errorMessage, string _errorFilePath)
        {
            this.headerID = _headerID;
            this.uniqueTransmissionID = _uniqueTransmissionID;
            this.uniqueTransmitterCode = _uniqueTransmitterID;
            this.tcc = _tcc;
            this.shipmentRecordID = _shipmentID;
            this.receiptID = _receiptID;
            this.formType = _formType;
            this.responseTime = _timeStamp;
            this.statusID = _statusCodeID;
            this.fileName = _fileName;
            this.checksum = _checksum;
            this.byteSize = _attachmentSize;
            this.errorCode = _errorCode;
            this.errorMessage = _errorMessage;
            this.xpath_content = _errorFilePath;
        }

        public int SUB_HEADER_ID
        {
            get { return this.headerID; }
            set { this.headerID = value; }
        }

        public string SUB_UNIQUE_TRANSMISSION_ID
        {
            get { return this.uniqueTransmissionID; }
            set { this.uniqueTransmissionID = value; }
        }

        public string SUB_UNIQUE_TRANSMITTER_CODE
        {
            get { return this.uniqueTransmitterCode; }
            set { this.uniqueTransmitterCode = value; }
        }

        public string SUB_TCC
        {
            get { return this.tcc; }
            set { this.tcc = value; }
        }

        public string SUB_SHIPMENT_ID
        {
            get { return this.shipmentRecordID; }
            set { this.shipmentRecordID = value; }
        }

        public string SUB_RECEIPT_ID
        {
            get { return this.receiptID; }
            set { this.receiptID = value; }
        }

        public string SUB_FORM_TYPE
        {
            get { return this.formType; }
            set { this.formType = value; }
        }

        public DateTime? SUB_DATETIME
        {
            get { return this.responseTime; }
            set { this.responseTime = value; }
        }

        public int SUB_STATUS_CODE_ID
        {
            get { return this.statusID; }
            set { this.statusID = value; }
        }

        public string SUB_FILE_NAME
        {
            get { return this.fileName; }
            set { this.fileName = value; }
        }

        public string SUB_CHECKSUM
        {
            get { return this.checksum; }
            set { this.checksum = value; }
        }

        public int SUB_FILE_SIZE
        {
            get { return this.byteSize; }
            set { this.byteSize = value; }
        }

        public string SUB_ERROR_MESSAGE_CODE
        {
            get { return this.errorCode; }
            set { this.errorCode = value; }
        }

        public string SUB_ERROR_MESSAGE
        {
            get { return this.errorMessage; }
            set { this.errorMessage = value; }
        }

        public string SUB_ERROR_FILE_PATH
        {
            get { return this.xpath_content; }
            set { this.xpath_content = value; }
        }
    }