using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using System.IO;
using System.Text;
using System.Xml;
using Afas.AfComply.Application;
using Afas.AfComply.Domain;

namespace Afas.AfComply.UI.admin
{
    public partial class admin_10941095_ack_import : Afas.AfComply.UI.admin.AdminPageBase
    {
        private ILog Log = LogManager.GetLogger(typeof(admin_admin_float_user));

        protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
        {
            LitUserName.Text = user.User_UserName;
            HfDistrictID.Value = user.User_District_ID.ToString();

            loadEmployers();
            loadTaxYears();
            loadSubmissionStatus();
        }

        /// <summary>
        /// This will load employers into the Dropdown List. This will filter the list as well if the user has entered anything into the 
        /// search textbox. 
        /// </summary>
        private void loadEmployers()
        {
            string searchText = TxtSearchEmployer.Text;
            List<employer> tempList = employerController.getAllEmployers();
            List<employer> filteredList = null;
            if (TxtSearchEmployer.Text.Count() > 0)
            {
                filteredList = employerController.FilterEmployerBySearch(searchText, tempList);
            }
            else
            {
                filteredList = tempList;
            }


            DdlEmployer.DataSource = filteredList;
            DdlEmployer.DataTextField = "EMPLOYER_NAME";
            DdlEmployer.DataValueField = "EMPLOYER_ID";
            DdlEmployer.DataBind();

            DdlEmployer.Items.Add("Select");
            DdlEmployer.SelectedIndex = DdlEmployer.Items.Count - 1;
        }

        /// <summary>
        /// Load all available Tax Years. 
        /// </summary>
        private void loadTaxYears()
        {
            DdlYears.DataSource = employerController.getTaxYears();
            DdlYears.DataBind();

            DdlYears.Items.Add("Select");
            DdlYears.SelectedIndex = DdlYears.Items.Count - 1;
        }

        /// <summary>
        /// Loads the IRS Submission Response Statuses
        /// This is now pulling from the ACA database.
        /// </summary>
        private void loadSubmissionStatus()
        {
            DdlSubmissionStatus.DataSource = airController.manufactureSubmissionStatuses();
            DdlSubmissionStatus.DataTextField = "STATUS_NAME";
            DdlSubmissionStatus.DataValueField = "STATUS_ID";
            DdlSubmissionStatus.DataBind();

            DdlSubmissionStatus.Items.Add("Select");
            DdlSubmissionStatus.SelectedIndex = DdlSubmissionStatus.Items.Count - 1;
        }

        /// <summary>
        /// Loads all the available Receipts that have not been acknowledged for a specified client.
        /// </summary>
        /// <param name="_ein"></param>
        /// <param name="_year"></param>
        private void loadReceipts(int _employerID, int _year)
        {
            List<taxYearEmployerTransmission> tempList = airController.manufactureEmployerTransmissions(_employerID, _year);
            List<taxYearEmployerTransmission> filteredList = new List<taxYearEmployerTransmission>();

            foreach (taxYearEmployerTransmission tyet in tempList)
            {
                if (tyet.transmission_status_code_id == 4)
                {
                    filteredList.Add(tyet);
                }
            }

            GvReceipts.DataSource = filteredList;
            GvReceipts.DataBind();

            DdlReceiptID.DataSource = filteredList;
            DdlReceiptID.DataTextField = "ReceiptID";
            DdlReceiptID.DataValueField = "tax_year_employer_transmissionID";
            DdlReceiptID.DataBind();

            DdlReceiptID.Items.Add("Select");
            DdlReceiptID.SelectedIndex = DdlReceiptID.Items.Count - 1;

            loadAvailableAckFiles();
        }

        /// <summary>
        /// If a new client is selected, the screen will revert back to all the default selections/data.  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
        {
            resetEmployerData(false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DdlYears_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool validData = true;
            int employerID = 0;
            int taxyear = 0;

            validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
            validData = errorChecking.validateDropDownSelection(DdlYears, validData);

            if (validData == true)
            {
                int.TryParse(DdlEmployer.SelectedItem.Value, out employerID);
                int.TryParse(DdlYears.SelectedItem.Value, out taxyear);
                loadReceipts(employerID, taxyear);
            }
        }

        /// <summary>
        /// Load the employers based on search values. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            loadEmployers();
            resetEmployerData(true);
        }

        /// <summary>
        /// Reset the screen back to the default view.
        /// </summary>
        private void resetEmployerData(bool alldata)
        {
            if (alldata == true)
            {
                DdlEmployer.SelectedIndex = DdlEmployer.Items.Count - 1;
                DdlEmployer.BackColor = System.Drawing.Color.White;
            }

            DdlYears.SelectedIndex = DdlYears.Items.Count - 1;
            DdlYears.BackColor = System.Drawing.Color.White;

            DdlSubmissionStatus.SelectedIndex = DdlSubmissionStatus.Items.Count - 1;
            DdlSubmissionStatus.BackColor = System.Drawing.Color.White;

            List<submission> tempList2 = new List<submission>();
            DdlAckFile.DataSource = tempList2;
            DdlAckFile.DataBind();
            GvAckFiles.DataSource = tempList2;
            GvAckFiles.DataBind();
            DdlReceiptID.DataSource = tempList2;
            DdlReceiptID.DataBind();

            List<manifest> tempList = new List<manifest>();
            GvReceipts.DataSource = tempList;
            GvReceipts.DataBind();
        }

        /// <summary>
        /// Logout out. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnLogout_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Logout.aspx", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DdlSubmissionStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            int statusID = 0;
            bool validData = true;

            validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
            validData = errorChecking.validateDropDownSelection(DdlYears, validData);
            validData = errorChecking.validateDropDownSelection(DdlSubmissionStatus, validData);

            if (validData == true)
            {
                int.TryParse(DdlSubmissionStatus.SelectedItem.Value, out statusID);
            }
        }

        /// <summary>
        /// Load the Ack File into the dropdown list. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DdlReceiptID_SelectedIndexChanged(object sender, EventArgs e)
        {
            string receiptID = null;
            bool validData = true;

            validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
            validData = errorChecking.validateDropDownSelection(DdlYears, validData);
            validData = errorChecking.validateDropDownSelection(DdlReceiptID, validData);

            if (validData == true)
            {
                receiptID = DdlReceiptID.SelectedItem.Text;
                List<FileInfo> currFiles = FileProcessing.getFtpAckFiles(receiptID);

                DdlAckFile.DataSource = currFiles;
                DdlAckFile.DataTextField = "NAME";
                DdlAckFile.DataValueField = "NAME";
                DdlAckFile.DataBind();
            }
            else
            {
                MpeWebMessage.Show();
                LitMessage.Text = "Please correct fields highlighted in red.";
            }
        }

        /// <summary>
        /// Process the status request data. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            FormatLblMsg(String.Empty);

            bool validData = true;
            int statusID = 0;
            int headerID = 0;
            int employerID = 0;
            int taxyear = 0;
            string receiptid = null;
            List<FileInfo> currFiles = new List<FileInfo>();
            FileInfo currFile = null;

            validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);
            validData = errorChecking.validateDropDownSelection(DdlYears, validData);
            validData = errorChecking.validateDropDownSelection(DdlReceiptID, validData);
            validData = errorChecking.validateDropDownSelection(DdlSubmissionStatus, validData);

            if (validData == true)
            {
                int.TryParse(DdlSubmissionStatus.SelectedItem.Value, out statusID);
                int.TryParse(DdlReceiptID.SelectedItem.Value, out headerID);
                int.TryParse(DdlEmployer.SelectedItem.Value, out employerID);
                int.TryParse(DdlYears.SelectedItem.Value, out taxyear);
                receiptid = DdlReceiptID.SelectedItem.Text;

                if (statusID == 2 || statusID == 3)
                {
                    validData = errorChecking.validateDropDownSelection(DdlAckFile, validData);
                    if (validData == true)
                    {
                        currFiles = FileProcessing.getFtpAckFiles(receiptid);
                        if (currFiles.Count == 0) { validData = false; }
                        else { currFile = currFiles[0]; }
                    }
                }

                if (validData == true)
                {
                    processAcknowledgement(currFile, statusID, receiptid, taxyear, employerID);
                    MpeWebMessage.Show();
                    LitMessage.Text = "The Status Request has been saved.";
                    resetEmployerData(true);
                }
                else
                {
                    MpeWebMessage.Show();
                    LitMessage.Text = "The XML Ack file is missing or there is a duplicate. Anytime a file has been Rejected or Accepted with Errors, a single Ack file is required. ";
                }

                //*********************************************************************************************************
                StatusCodeEnum statusCodeEnum = (StatusCodeEnum)statusID;
                if(statusCodeEnum == StatusCodeEnum.NotFound){
                    MpeWebMessage.Show();
                    LitMessage.Text = "No Status Code is Associated with this Acknowlegdment File";
                }
                else{

                    TransmissionStatusEnum newTransmissionStatusEnum = TransmissionStatusEnum.Transmitted; 

                    switch (statusCodeEnum)
                    {
                        case StatusCodeEnum.Accepted:
                            newTransmissionStatusEnum = TransmissionStatusEnum.Accepted;
                            break;
                        case StatusCodeEnum.AcceptedWithErrors:
                            newTransmissionStatusEnum = TransmissionStatusEnum.AcceptedWithErrors;
                            break;
                        case StatusCodeEnum.Rejected:
                            newTransmissionStatusEnum = TransmissionStatusEnum.Rejected;
                            break;
                    }
                    

                    EmployerTaxYearTransmissionStatus currentEployerTaxYearTransmissionStatus = employerController.getCurrentEmployerTaxYearTransmissionStatusByEmployerIdAndTaxYearId(employerID, taxyear);
                    if (currentEployerTaxYearTransmissionStatus != null)
                    {
                        User User = (User)Session["CurrentUser"];
                        employerController.endEmployerTaxYearTransmissionStatus(currentEployerTaxYearTransmissionStatus, User.User_UserName);

                        var newEmployerTaxYearTransmissionStatus = new EmployerTaxYearTransmissionStatus(
                                 currentEployerTaxYearTransmissionStatus.EmployerTaxYearTransmissionId,
                                 newTransmissionStatusEnum,
                                 User.User_UserName,
                                 DateTime.Now
                             );

                        newEmployerTaxYearTransmissionStatus = employerController.insertUpdateEmployerTaxYearTransmissionStatus(newEmployerTaxYearTransmissionStatus);
                    }
                }
                //*********************************************************************************************
            }
            else
            {
                MpeWebMessage.Show();
                LitMessage.Text = "Please correct fields highlighted in red.";
            }
        }

        /// <summary>
        /// This will process and update the status for all submissions linked to a receipt ID. 
        /// </summary>
        /// <param name="fi"></param>
        private bool processAcknowledgement(FileInfo fi, int statusid, string receiptID, int taxyear, int employerid)
        {
            int errorID = 0;
            string uniqueSubmissionID = null;
            string errorCode = null;
            string errorText = null;
            List<int> errantRecordIDs = new List<int>();
            DateTime modOn = DateTime.Now;
            string modBy = LitUserName.Text;
            bool validTransaction = false;
            string fileName = null;
            string base64 = null;
            int employerID = 0;

            int.TryParse(DdlEmployer.SelectedItem.Value, out employerID);
            employer _employer = employerController.getEmployer(employerID);

            if (fi != null)
            {
                base64 = convertAckToBase64(fi);
                string xmlString = fi.FullName;
                fileName = "~/ftps/submission_ack/" + fi.Name;
                XmlDocument xml = new XmlDocument();
                xml.Load(xmlString);

                #region Read XML Nodes
                foreach (XmlNode node in xml.DocumentElement.ChildNodes)
                {
                    errorID = 0;

                    foreach (XmlNode node2 in node.ChildNodes)
                    {
                        uniqueSubmissionID = null;
                        errorText = null;
                        errorCode = null;

                        if (node2.Name == "TransmitterErrorDetailGrp")
                        {
                            bool employerError = false;

                            foreach (XmlNode node4 in node2.ChildNodes)
                            {
                                switch (node4.Name)
                                {
                                    case "UniqueSubmissionId":
                                        uniqueSubmissionID = node4.InnerText;
                                        string[] subID = uniqueSubmissionID.Split('|');
                                        employerError = true;
                                        foreach (XmlNode node5 in node4.ChildNodes)
                                        {
                                            switch (node5.Name)
                                            {
                                                case "ns2:ErrorMessageCd":                  
                                                    errorCode = node5.InnerText;
                                                    break;
                                                case "ns2:ErrorMessageTxt":                 
                                                    errorText = node5.InnerText;
                                                    break;
                                                case "ns2:SubmissionLevelStatusCd":             
                                                    break;
                                            }
                                        }
                                        break;
                                    case "SubmissionLevelStatusCd":                            
                                        errorText = node4.InnerText;
                                        break;
                                    case "UniqueRecordId":                                         
                                        uniqueSubmissionID = node4.InnerText;
                                        string[] subID2 = uniqueSubmissionID.Split('|');
                                        errorID = int.Parse(subID2[2]);
                                        break;
                                    case "ns2:ErrorMessageDetail":                            
                                        foreach (XmlNode node5 in node4.ChildNodes)
                                        {
                                            switch (node5.Name)
                                            {
                                                case "ns2:ErrorMessageCd":                 
                                                    errorCode = node5.InnerText;
                                                    break;
                                                case "ns2:ErrorMessageTxt":                
                                                    errorText = node5.InnerText;
                                                    break;
                                                case "":
                                                    break;
                                            }
                                        }
                                        break;
                                }
                                if (employerError == false) { errantRecordIDs.Add(errorID); }
                            }
                        }
                    }
                }
                #endregion
            }

            validTransaction = employerController.updateTaxYearEmployerTransmissionStatus(employerid, taxyear, receiptID, modBy, modOn, statusid, errantRecordIDs, base64);

            if (validTransaction == true && fi != null)
            {
                FileArchiverWrapper faw = new FileArchiverWrapper();
                faw.ArchiveFile(fi.FullName, _employer.ResourceId, "Process Deleted Ack File", _employer.EMPLOYER_ID);
            }

            return validTransaction;
        }


        /// <summary>
        /// Ajax File Upload Process. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AfuAck_UploadComplete(object sender, AjaxControlToolkit.AjaxFileUploadEventArgs e)
        {
            string filepath = Server.MapPath("..\\ftps\\submission_ack\\") + e.FileName;
            try
            {
                AfuAck.SaveAs(filepath);
            }
            catch (Exception ex)
            {

            }
        }


        /// <summary>
        /// Search the FTP folder directory for ack.xml files that contain the receipt id and display them. 
        /// </summary>
        private void loadAvailableAckFiles()
        {
            List<FileInfo> tempList = new List<FileInfo>();

            foreach (GridViewRow row in GvReceipts.Rows)
            {
                Label LblReceipt = (Label)row.FindControl("Lbl_gv_Receipt");
                string receiptID = LblReceipt.Text;
                List<FileInfo> currFiles = FileProcessing.getFtpAckFiles(receiptID);

                foreach (FileInfo fi in currFiles)
                {
                    tempList.Add(fi);
                }

            }

            GvAckFiles.DataSource = tempList;
            GvAckFiles.DataBind();
        }

        private void FormatLblMsg(String receipt, bool? result = null)
        {
            if (result == null)
            {
                lblMsg.Text = receipt;
                lblMsg.ForeColor = System.Drawing.Color.Black;
                lblMsg.BackColor = System.Drawing.Color.Transparent;
            }
            else if (result.Value)
            {
                lblMsg.Text = String.Format("Receipt Id {0} updated SUCCESSFULLY", receipt);
                lblMsg.ForeColor = System.Drawing.Color.White;
                lblMsg.BackColor = System.Drawing.Color.Green;
            }
            else
            {
                lblMsg.Text = String.Format("Receipt Id {0} updated FAILED", receipt);
                lblMsg.ForeColor = System.Drawing.Color.White;
                lblMsg.BackColor = System.Drawing.Color.Red;
            }
        }

        protected void GvAckFiles_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int employerID = 0;
            GridViewRow row = GvAckFiles.Rows[e.RowIndex];
            HiddenField hfFilePath = (HiddenField)row.FindControl("Hf_gv_FilePath");

            int.TryParse(DdlEmployer.SelectedItem.Value, out employerID);
            employer _employer = employerController.getEmployer(employerID);
            string fullFilePath = hfFilePath.Value;

            if (System.IO.File.Exists(fullFilePath))
            {
                try
                {
                    FileArchiverWrapper faw = new FileArchiverWrapper();
                    faw.ArchiveFile(fullFilePath, _employer.ResourceId, "User Delete Ack File", _employer.EMPLOYER_ID);
                    loadAvailableAckFiles();
                }
                catch (Exception exception)
                {
                    Log.Warn("Suppressing errors.", exception);
                }
            }
        }

        /// <summary>
        /// Convert the XML file to a String than to Base64
        /// </summary>
        /// <param name="fi"></param>
        /// <returns></returns>
        private string convertAckToBase64(FileInfo fi)
        {
            string base64 = null;
            string filePath = fi.FullName;
            string xmlString = null;
            StringWriter sw = new StringWriter();
            XmlDocument xml = new XmlDocument();
            xml.Load(filePath);

            xmlString = xml.OuterXml;

            base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(xmlString));
            return base64;
        }
    }
}