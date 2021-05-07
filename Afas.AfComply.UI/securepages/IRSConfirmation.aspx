<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IRSConfirmation.aspx.cs" Inherits="IRSConfirmation" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" type="text/css" href="/default.css" />
    <link rel="stylesheet" type="text/css" href="/menu.css" />
    <link rel="stylesheet" type="text/css" href="/v_menu.css" />
    <title>IRS Confirmation Dashboard</title>

    <style>
        p {
            font-size: 14px;
            margin-bottom: 10px;
            margin-left: 10px;
        }

        .p-flush {
            margin-left: 0;
        }

        .irs-ver-para > p.irs-ver-para > br {
            margin-bottom: 10px;
            line-height: 2.0;
        }

        #Gv_SafeHarbor {
            background-color: rgba(255,242,1,.25) !important;
        }

        .irs-button {
            background-color: #0088B5;
            padding: 7px 10px;
            border-radius: 5px;
            width: 300px;
            text-align: center;
            margin-bottom: 0;
            margin-top: 15px;
        }

            .irs-button > a {
                color: #ffffff;
                text-decoration: none;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
        <div id="container">
            <div id="header" style =" height:90px">
                <a href="default.aspx">
                    <img src="<%= Page.ResolveUrl(Branding.LogoUri) %>" style="height: 70px" alt="<%= Branding.ProductName %> Logo" class="logo" />
                </a>
                <ul id="toplinks">
                    <li>Stability Period: 
                        <asp:HiddenField ID="HfEmployerTypeID" runat="server" />
                        <asp:HiddenField ID="HfUserName" runat="server" />
                    </li>
                    <li>User Name:<asp:Literal ID="LitUserName" runat="server"></asp:Literal></li>
                    <li>
                        <asp:Button ID="BtnLogout" CssClass="btn" runat="server" Text="Log Out" OnClick="BtnLogout_Click" UseSubmitBehavior="false" /></li>

                     </ul>
                <asp:HiddenField ID="HfDistrictID" runat="server" />
            </div>
            <div id="nav2">
                <nav>
                    <ul>
                        <li><a href="default.aspx">Home</a></li>
                        <li><a href="e_find.aspx">Employee</a></li>
                        <li><a href="r_reporting.aspx">Reporting</a></li>
                        <li><a href="s_setup.aspx">Employer Setup</a></li>
                        <li><a href="t_terms.aspx">ACA Terms</a></li>
                        <li><a href="contact.aspx">Help</a></li>
                    </ul>
                </nav>
                <ul class="right">
                    <li></li>
                </ul>
            </div>

            <div id="content">
                <h1>Confirmation Page</h1>
                <h2>Step 1: Confirm Data is Correct</h2>

                <p>
                    Please review the information below, make any necessary adjustments, and then Confirm at the bottom of this page that everything is correct.
                   Note that if you discover issues with the data described below after pushing the Confirm button, you may have to do significant rework.
                   As such, please take time to complete this step carefully.    
                </p>

                <h3>A. Confirm Legal Name</h3>

                <p>
                    Your employer’s legal name in <%= Branding.ProductName %> must match the records the IRS has for your organization.
                    Mistakes in the legal name can result in rejection of your IRS filing.
                    Please ensure that the name listed below is exactly the name the IRS would have on file for your organization, 
                    including punctuation, spacing, etc. You may correct any errors <a href="/securepages/s_setup.aspx">here</a>.
                </p>

                <div class="irs-ver-para">
                    <p style="display: inline; color: White; background-color: #336666; font-weight: bold;">&nbsp<asp:Literal ID="LabelLegalName" runat="server"></asp:Literal>&nbsp</p>
                </div>

                <h3>B. Confirm IRS Contact</h3>

                <p>
                    Please ensure that you have <b>ONLY ONE</b> individual designated as your IRS contact.
                    This person should be able to answer any questions from employees or the IRS about your forms.
                    Please verify the name and contact information listed below is correct for the person you want to designate as your organization’s IRS contact.
                    You may correct errors <a href="/securepages/s_setup.aspx">here</a>.
                </p>

                <asp:GridView ID="gvContacts" runat="server" AutoGenerateColumns="false"
                    EmptyDataText="There are currently NO IRS Contacts for this employer!" BackColor="White" BorderColor="#336666"
                    BorderStyle="Double" BorderWidth="3px" CellPadding="4" GridLines="Horizontal">
                    <FooterStyle BackColor="White" ForeColor="#333333" />
                    <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="White" ForeColor="#333333" Height="30px" />
                    <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                    <SortedAscendingHeaderStyle BackColor="#487575" />
                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                    <SortedDescendingHeaderStyle BackColor="#275353" />
                    <Columns>
                        <asp:TemplateField HeaderText="First Name" HeaderStyle-Width="225px" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Literal ID="LitFName" runat="server" Text='<%# Eval("User_First_Name") %>'></asp:Literal>
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Last Name" HeaderStyle-Width="225px" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Literal ID="LitLName" runat="server" Text='<%# Eval("User_Last_Name") %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Phone" HeaderStyle-Width="225px" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Literal ID="LitPhone" runat="server" Text='<%# Eval("User_Phone") %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <h3>C. Confirm Data </h3>

                <p>
                    All data files must be complete and loaded into <%= Branding.ProductName %> before you start the verification process to approve and send your forms.
                    Data requirements for your organization may differ depending upon whether you have elected to use a “Tracking and Reporting” or “Reporting Only” method for 2016.
                    At this point, we believe we have uploaded all of your data applicable for the 2016 reporting into the system.  
                </p>

                <p>
                    Please ensure that complete data has been submitted for the applicable measurement/reporting period(s) and includes the following:
                </p>

                <ul>
                    <li>
                        <p style="margin-left: 25px"><span style="font-weight: bold">For all customers: </span>Up to date employee demographic (census) information</p>
                    </li>
                    <li>
                        <p style="margin-left: 25px"><span style="font-weight: bold">For all customers: </span>Complete insurance offer</p>
                    </li>
                    <li>
                        <p style="margin-left: 25px"><span style="font-weight: bold">For customers with self-funded coverage only: </span>Carrier enrollment information</p>
                    </li>
                    <li>
                        <p style="margin-left: 25px">
                            <span style="font-weight: bold">For customers with both tracking and reporting services: </span>All applicable hours, including unpaid leaves of absence that qualify as USERRA military or FMLA leave, and jury duty.
                             (More detailed information about special unpaid leaves is included in our <a href="<%= Feature.IrsInstructionsLink %>">detailed instruction guide</a>.)
                        </p>
                    </li>
                </ul>

                <p>
                    You can spot check your employee demographic, offer, and carrier enrollment information in the system by going to the <a href="/securepages/export_employees.aspx">Employee Status/Class Data Export</a> page
                    and clicking on the CSV icons on the top right of the screen, which will download a spreadsheet for you to review. 
                </p>

                <p>
                    You may load any missing data via the File Import process <a href="/securepages/transfer.aspx">here</a>.  
                </p>

                <h3>D. Confirm Affordability Safe Harbor Codes</h3>

                <p>
                    Employees receiving a 1095-C form will be assigned an affordability safe harbor code, if applicable, using the employee class information stored in AFcomply.
                    Please review and verify that the affordability safe harbor designated for each employee classification shown below is correct.
                    (More information is available in the <a href="<%= Feature.IrsInstructionsLink %>">detailed instruction guide</a>.) 
                </p>

                <p>
                    It is important to remember that sometimes no safe harbor may apply; if this is the case,
                    you should leave it blank and not assign any affordability safe harbor to that employee class.
                    For the right safe harbor to appear on the form, employees must be assigned to the correct employee class on the employee demographic (census) data file.
                    Your employee classes and safe harbor codes, if any, are shown below. If you need to make changes to the safe harbor codes, start <a href="/securepages/s_setup.aspx">here</a>
                    and follow the instructions in the instruction guide.
                </p>

                <h3>Safe Harbor Codes</h3>

                <asp:Literal ID="LitMessage" runat="server"></asp:Literal>

                <asp:GridView ID="Gv_SafeHarbor" runat="server" AutoGenerateColumns="false"
                    EmptyDataText="There are currently NO Employee Classifications setup for this employer!" BackColor="White" BorderColor="#336666"
                    BorderStyle="Double" BorderWidth="3px" CellPadding="4" GridLines="Horizontal">
                    <FooterStyle BackColor="White" ForeColor="#333333" />
                    <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="White" ForeColor="#333333" Height="30px" />
                    <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                    <SortedAscendingHeaderStyle BackColor="#487575" />
                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                    <SortedDescendingHeaderStyle BackColor="#275353" />
                    <Columns>
                        <asp:TemplateField HeaderText="Classification" HeaderStyle-Width="225px" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:HiddenField ID="HiddenTypeId" runat="server" Value='<%# Eval("CLASS_ID") %>' />
                                <asp:Literal ID="LitTypeName" runat="server" Text='<%# Eval("CLASS_DESC") %>'></asp:Literal>
                            </ItemTemplate>

                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Safe Harbor" HeaderStyle-Width="225px" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Literal ID="Lit_gv_harbor" runat="server" Text='<%# Eval("CLASS_AFFORDABILITY_CODE") %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <br />

                <h3>E. Clear All Alerts</h3>

                <p>
                    <asp:Literal ID="txtAlert" runat="server" Text=""></asp:Literal>
                </p>


                <br />
                <h3>F. Confirm Non-Full Time Employee Status for Recent New Hires </h3>

                <p>
                    Please review and confirm that the following employees are not considered full-time; 
                    these are individuals who were hired since the beginning of your last ongoing measurement period who were not classified as full-time. 
                    You may adjust the status for an individual by using the drop down boxes below.  
                </p>
                <div>
                    <asp:GridView ID="GridView_NewHires" runat="server" AutoGenerateColumns="false"
                        EmptyDataText="There are currently NO New Hires that need to be confirmed." BackColor="White" BorderColor="#336666"
                        BorderStyle="Double" BorderWidth="3px" CellPadding="4" GridLines="Horizontal">
                        <FooterStyle BackColor="White" ForeColor="#333333" />
                        <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="White" ForeColor="#333333" Height="30px" />
                        <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F7F7F7" />
                        <SortedAscendingHeaderStyle BackColor="#487575" />
                        <SortedDescendingCellStyle BackColor="#E5E5E5" />
                        <SortedDescendingHeaderStyle BackColor="#275353" />
                        <Columns>
                            <asp:TemplateField HeaderText="Employee Name">
                                <ItemTemplate>
                                    <asp:Literal ID="Lit_gv_name" runat="server" Text='<%# Eval("EMPLOYEE_FULL_NAME") %>'></asp:Literal>
                                    <asp:HiddenField ID="Hf_gv_id" runat="server" Value='<%# Eval("EMPLOYEE_ID") %>' />
                                    <asp:HiddenField ID="Hf_gv_class_id" runat="server" Value='<%# Eval("EMPLOYEE_CLASS_ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" HeaderStyle-Width="225px" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:HiddenField ID="Hf_gv_AcaTypeId" runat="server" Value='<%# Eval("EMPLOYEE_ACT_STATUS_ID") %>' />
                                    <asp:DropDownList ID="Ddl_gv_AcaType" runat="server"></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:Button ID="BtnNewHire" runat="server" CssClass="btn" Text="Save" OnClick="BtnNewHire_Click" />
                </div>


                <br />
                <h3>G. Confirm Spousal Offer is Correct</h3>

                <p>
                    If you make a conditional offer of coverage for spouses of eligible employees, you need to confirm that has been recorded correctly in the system.
                    A conditional offer is an offer of coverage that is subject to one or more reasonable, 
                    objective conditions (for example, the employer offers to cover an employee’s spouse only if the spouse is not eligible for coverage under Medicare or a group health plan sponsored by another employer).  
                    If an eligible employee is able to enroll their spouse without limitation, you do not have a conditional offer and there is nothing further you need to do on this step; 
                    you may proceed to the next step
                </p>

                <p>
                    If spouses are conditionally eligible under your plan(s), 
                    click <a href="/securepages/s_setup.aspx">here</a> and follow the instructions in the <a href="<%= Feature.IrsInstructionsLink %>">guide</a> to ensure in the Medical Plan setup, 
                    under the question “Is this plan offered to spouses?”, you have selected “Conditionally”.
                </p>

                <h3>H. Confirmation </h3>
                
                <asp:Panel ID="PnlMessage" runat="server">
                    <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                    </div>
                    <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white; z-index: 10">
                        <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                            <asp:ImageButton ID="ImgBtnClose" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" BorderStyle="None" />
                        </div>
                        <h3 style="color: black;">Webpage Message</h3>
                        <p style="color: darkgray">
                            <asp:Literal ID="PopupMessage" runat="server"></asp:Literal>
                        </p>
                        <br />
                    </div>
                </asp:Panel>

                <asp:ModalPopupExtender ID="MpeWebMessage" runat="server" TargetControlID="HfDummyTrigger" OkControlID="ImgBtnClose" PopupControlID="PnlMessage"></asp:ModalPopupExtender>

                <asp:HiddenField ID="HiddenField1" runat="server" />
                <asp:HiddenField ID="HfDummyTrigger2" runat="server" />

                <p>
                    I hereby confirm I have reviewed the information above and it is complete and accurate to the best of my knowledge.
                </p>
                <asp:Button ID="btnConfirm" runat="server" Text="Confirm" OnClick="btnConfirm_Click" />
            </div>
        </div>

        <asp:HiddenField ID="HfDummyTrigger" runat="server" />
        <div style="clear: both;">&nbsp;</div>
        <div id="footer">
            Copyright &copy; <%= Branding.CopyrightYears %> <a href="<%= Branding.CompanyWebSite %>"><%= Branding.CompanyName %></a> - All Rights Reserved   
            <br />
            <div style="clear: both;">&nbsp;</div>
        </div>
    </form>

    <script>
        setTimeout(AutoLogout, <%= Feature.AutoLogoutTime %> );
        
        function AutoLogout() {
            alert("<%= Branding.AutoLogoutMessage %>");
            window.location = window.location.href;
        }
    </script>
</body>
</html>
