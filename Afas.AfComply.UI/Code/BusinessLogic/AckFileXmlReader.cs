using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Xml;


public class AckFileXmlReader
{

    public AckFileXmlReader() { }


    private XmlDocument getXMLData(int employerID, int taxYear, int etytID)
    {
        taxYearEmployerTransmission taxyearemployertransmission = null;
        XmlDocument xml = new XmlDocument();

        List<taxYearEmployerTransmission> tempList = airController.manufactureEmployerTransmissions(employerID, taxYear);
        foreach (taxYearEmployerTransmission i in tempList)
        {
            if (i.tax_year_employer_transmissionID == etytID)
            {
                taxyearemployertransmission = i;
                string ackFileXml = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(i.AckFile));
                xml.LoadXml(ackFileXml);
                break;
            }
        }


        return xml;
    }
    /// <summary>
    /// Reads the XML file to pull the error code and message for Employee Errors. 
    /// </summary>
    /// <param name="recordID"></param>
    /// <returns></returns>
    public string findEmployeeErrorMessageInXML(int recordID, int employerID, int taxYear, int etytID)
    {
        string errorCodeMessage = null;
        XmlDocument xml = getXMLData(employerID, taxYear, etytID);

        #region Read XML Nodes
        foreach (XmlNode node in xml.DocumentElement.ChildNodes)
        {
            string xmlErrorMessage = null;
            string xmlErrorCode = null;
            int xmlRecordID = 0;

            foreach (XmlNode node2 in node.ChildNodes)
            {
                if (node2.Name == "TransmitterErrorDetailGrp")
                {
                    foreach (XmlNode node4 in node2.ChildNodes)
                    {
                        switch (node4.Name)
                        {
                            case "UniqueRecordId":                                         
                                string uniqueRecordID = node4.InnerText;
                                string[] subID2 = uniqueRecordID.Split('|');
                                xmlRecordID = int.Parse(subID2[2]);
                                break;
                            case "ns2:ErrorMessageDetail":                            
                                foreach (XmlNode node5 in node4.ChildNodes)
                                {
                                    switch (node5.Name)
                                    {
                                        case "ns2:ErrorMessageCd":                 
                                            xmlErrorCode = node5.InnerText;
                                            break;
                                        case "ns2:ErrorMessageTxt":                
                                            xmlErrorMessage = node5.InnerText;
                                            break;
                                        case "":
                                            break;
                                    }
                                }
                                break;
                        }
                    }

                    if (recordID == xmlRecordID)
                    {
                        string errorMessage2 = xmlErrorMessage;
                        string errorCode = xmlErrorCode;
                        errorCodeMessage = errorCode + "|" + errorMessage2;
                        break;
                    }
                }
            }
        }
        #endregion
        return errorCodeMessage;
    }

    /// <summary>
    /// Reads the XML file to pull the error code and message for the Employer Errors.
    /// </summary>
    /// <returns></returns>
    public DataTable findEmployerErrorMessageInXML(int employerID, int taxYear, int etytID)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add(new DataColumn("ErrorCode", typeof(string)));
        dt.Columns.Add(new DataColumn("ErrorMessage", typeof(string)));
        XmlDocument xml = getXMLData(employerID, taxYear, etytID);
        #region Read XML Nodes
        foreach (XmlNode node in xml.DocumentElement.ChildNodes)
        {
            string xmlErrorMessage = null;
            string xmlErrorCode = null;

            foreach (XmlNode node2 in node.ChildNodes)
            {
                bool employerError = false;

                if (node2.Name == "TransmitterErrorDetailGrp")
                {
                    foreach (XmlNode node4 in node2.ChildNodes)
                    {
                        switch (node4.Name)
                        {
                            case "UniqueSubmissionId":            
                                employerError = true;
                                break;
                            case "SubmissionLevelStatusCd":
                                employerError = true;
                                xmlErrorCode = "Transmission Status:";
                                xmlErrorMessage = node4.InnerText;
                                break;
                            case "ns2:ErrorMessageDetail":
                                foreach (XmlNode node5 in node4.ChildNodes)
                                {
                                    switch (node5.Name)
                                    {
                                        case "ns2:ErrorMessageCd":                  
                                            xmlErrorCode = node5.InnerText;
                                            break;
                                        case "ns2:ErrorMessageTxt":                 
                                            xmlErrorMessage = node5.InnerText;
                                            break;
                                    }
                                }
                                break;
                        }
                    }

                    if (employerError == true)
                    {
                        dt.Rows.Add(xmlErrorCode, xmlErrorMessage);

                        int rowCount = dt.Rows.Count;
                    }
                }
            }
        }
        #endregion
        return dt;
    }
}