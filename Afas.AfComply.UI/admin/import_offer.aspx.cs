using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OleDb;
using System.IO;

using log4net;

using Afas.AfComply.Application;
using Afas.AfComply.Domain;

using Afas.AfComply.UI.Code.AFcomply.DataAccess;
using Afas.Application.Archiver;
using Afas.AfComply.UI.App_Start;

public partial class admin_import_offer : Afas.AfComply.UI.admin.AdminPageBase
{
    private ILog Log = LogManager.GetLogger(typeof(admin_import_offer));

    protected override void PageLoadLoggedInAsAdmin(User user, employer employer)
    {

        Server.ScriptTimeout = 1800;

        LitUserName.Text = user.User_UserName;
        loadEmployers();
    }

    /*********************************************************************************************
     GROUP 1: All functions that load data into dropdown lists & gridviews. ****************** 
    *********************************************************************************************/
    /// <summary>
    /// 1-1) Load all existing employers into a dropdown list. 
    /// </summary>
    private void loadEmployers()
    {
        DdlEmployer.DataSource = employerController.getAllEmployers();
        DdlEmployer.DataTextField = "EMPLOYER_NAME";
        DdlEmployer.DataValueField = "EMPLOYER_ID";
        DdlEmployer.DataBind();

        DdlEmployer.Items.Add("Select");
        DdlEmployer.SelectedIndex = DdlEmployer.Items.Count - 1;
    }

    /*********************************************************************************************
     GROUP 2: All dropdown list SelectedIndex Change Functions. ********************************** 
    *********************************************************************************************/
    /// <summary>
    /// 2-1) When the Employer is changed. 
    ///         - A) Get the new EMPLOYER ID.
    ///         - B) Get the new EMPLOYER OBJECT.
    ///         - C) Load all EMPLOYER - EMPLOYEE TYPES.
    ///         - D) Load all EMPLOYER - MEASUREMENT PERIODS.
    ///         - E) Load all EMPLOYER - PLAN YEARS.
    ///         - F) Load all EMPLOYER - DEMOGRAPHIC FILES (FTP FOLDER).
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void DdlEmployer_SelectedIndexChanged(object sender, EventArgs e)
    {
        int _employerID = 0;
        employer _employer = null;

        if (DdlEmployer.SelectedItem.Text != "Select")
        {
            _employerID = int.Parse(DdlEmployer.SelectedItem.Value);

            _employer = employerController.getEmployer(_employerID);


            DdlPlanYear.DataSource = PlanYear_Controller.getEmployerPlanYear(_employerID);
            DdlPlanYear.DataTextField = "PLAN_YEAR_DESCRIPTION";
            DdlPlanYear.DataValueField = "PLAN_YEAR_ID";
            DdlPlanYear.DataBind();

            DdlPlanYear.Items.Add("Select");
            DdlPlanYear.SelectedIndex = DdlPlanYear.Items.Count - 1;
        }

    }

    /*********************************************************************************************
    *****  GROUP 3: All File Import/Processing Functions ***************************************** 
    *********************************************************************************************/
    /// <summary>
    /// 3-1) This will upload a file to the FTPS folder. This should be automated from the 
    /// payroll companies, but incase we ever need to manually upload a file, this will allow 
    /// for it. 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void BtnUploadFile_Click(object sender, EventArgs e)
    {
        bool validData = true;
        validData = errorChecking.validateDropDownSelection(DdlEmployer, validData);

        if (validData == true)
        {
            if (FuGrossPayFile.HasFile)
            {
                int _employerID = int.Parse(DdlEmployer.SelectedItem.Value);
                employer emp = employerController.getEmployer(_employerID);
                String _appendFile = emp.EMPLOYER_IMPORT_EMPLOYEE;
                String _filePath = Server.MapPath("..\\ftps\\");
                String savedFileName = String.Empty;
                FileProcessing.SaveFile(FuGrossPayFile, _filePath, LblFileUploadMessage, _appendFile, out savedFileName);

                try
                {
                    if (false == errorChecking.validateDropDownSelection(DdlPlanYear, true))
                    {
                        MpeWebMessage.Show();
                        LitMessage.Text = "You must select a plan year.";
                        Gv_ValidationErrors.Visible = false;

                        return;

                    }
                    int _planYearID = int.Parse(DdlPlanYear.SelectedItem.Value);

                    User currUser = (User)Session["CurrentUser"];
                    AfComplyFileDataImporter importer = ContainerActivator._container.Resolve<AfComplyFileDataImporter>();
                    importer.Setup(emp.EMPLOYER_ID, savedFileName, _planYearID); 
                    if (importer.ImportData(currUser.User_UserName, "Offer Upload Admin", "Offer"))
                    {
                        MpeWebMessage.Show();
                        LitMessage.Text = "The File has been Processed.";
                        Gv_ValidationErrors.Visible = false;

                        return;
                    }
                    else
                    {
                        MpeWebMessage.Show();
                        LitMessage.Text = "The File Failed Processing.";
                        if (importer.DataValidationMessages.Count > 0)
                        {
                            Gv_ValidationErrors.Visible = true;

                            Gv_ValidationErrors.DataSource = importer.DataValidationMessages;
                            Gv_ValidationErrors.DataBind();

                        }
                        else
                        {
                            Gv_ValidationErrors.Visible = false;
                        }
                        return;
                    }
                }
                catch (Exception exception)
                {
                    Log.Error("Hit 'Contact IT' Error in [BtnUploadFile_Click]", exception);
                    MpeWebMessage.Show();
                    LitMessage.Text = "Error: Please Contact IT.";
                }

                String archivePath = HttpContext.Current.Server.MapPath(Archive.ArchiveFolder);

                try
                {
                    DataValidation.FileIsForEmployer(savedFileName, emp.EMPLOYER_EIN, new FileArchiverWrapper(), emp.ResourceId, emp.EMPLOYER_ID);
                }
                catch (Exception exception)
                {
                    Log.Error("Hit 'Contact IT' Error in [BtnUploadFile_Click]", exception);
                    MpeWebMessage.Show();
                    LitMessage.Text = exception.Message;

                    if (LitMessage.Text.Contains("Did Not match"))
                    {
                        LitMessage.Text = "<br/><span style='color: red;font-weight: bold;'>This file does NOT belong to this employer. Contact your manager for assistance!</span><br/>" + LitMessage.Text;
                    }
                    Gv_ValidationErrors.Visible = false;

                    return;
                }

                FuGrossPayFile.BackColor = System.Drawing.Color.White;
            }
            else
            {
                FuGrossPayFile.BackColor = System.Drawing.Color.Red;
                LblFileUploadMessage.Text = "Please select a file";
                MpeWebMessage.Show();
                LblFileUploadMessage.Text = "Please correct all red fields.";
                LitMessage.Text = "You must select a file. Use the browse button to find a file to upload.";
                Gv_ValidationErrors.Visible = false;

            }
        }
        else
        {
            MpeWebMessage.Show();
            LblFileUploadMessage.Text = "Please correct all red fields.";
            LitMessage.Text = "You must select an EMPLOYER before you can upload a file.";
            Gv_ValidationErrors.Visible = false;

        }
    }

    protected void BtnLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Logout.aspx", false);
    }

}