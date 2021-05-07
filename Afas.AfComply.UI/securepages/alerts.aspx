<%@ Page Title="" Language="C#" MasterPageFile="~/securepages/SecurePages.Master" AutoEventWireup="true" CodeBehind="alerts.aspx.cs" Inherits="securepages_alerts" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="bodycontent" runat="server">
    <link rel="stylesheet" type="text/css" href="/leftnav.css?1.4.0.100" />
    <link rel="stylesheet" type="text/css" href="/alerts.css?1.4.0.100" />
    <style>
        /*left navigation bar was hard cocded input this page, please change it in the future.*/
        .sidenav {
            /*height:100%;
    width: 225px;*/
            /*position:fixed;*/
            /*z-index:1;
    top:225px;
    left:0;
    background-color:#dcddde;
    overflow-x:hidden;
    padding-top:20px;*/
            width: 20%;
            height: 100%;
            z-index: 1;
            top: 25px;
            left: 0;
            background-color: #fff;
            border-bottom: #ddd;
            overflow-x: hidden;
            padding-top: 10px;
            font-family: "Myriad Pro", "DejaVu Sans Condensed", Helvetica, Arial, "sans-serif";
            text-align: left;
            font-weight: 200;
            display: inline;
        }

            .sidenav a {
                padding: 8px 8px 8px 8px;
                text-decoration: none;
                font-size: 14px;
                color: #6e6d71;
                display: block;
            }

                .sidenav a:hover {
                    text-decoration: underline;
                    border-left: #eb0029 4px solid;
                }
    </style>
    <div id="header">
        <asp:HiddenField ID="HfUserName" runat="server" />
        <asp:HiddenField ID="HfDistrictID" runat="server" />
    </div>
    <div id="content">

     
<%
 if (null != Session["CurrentDistrict"] && ((employer)Session["CurrentDistrict"]).IrsEnabled == true)
{
%>
<%
                        
}
%>
<%= demo.getLeftLinks(null != Session["CurrentDistrict"] && ((employer)Session["CurrentDistrict"]).IrsEnabled) %> 




       
    </div>
    <div class="middle_ebc">
        <h3>Alerts</h3>
        <asp:GridView ID="GvAlerts" runat="server" CssClass="gridviewHeader" AutoGenerateColumns="false" CellPadding="1"
            ForeColor="black" GridLines="None" Width="300px" Font-Size="14px" ShowHeader="True"
            OnSelectedIndexChanged="GvAlerts_SelectedIndexChanged" EmptyDataRowStyle-BackColor="white"
            EmptyDataText="Your Employer has no Alerts!">
            <AlternatingRowStyle BackColor="White" />
            <EditRowStyle BackColor="transparent" />
            <FooterStyle BackColor="#eb0029" Font-Bold="true" ForeColor="White" />
            <HeaderStyle BackColor="#eb0029" Font-Bold="true" ForeColor="White" />
            <PagerStyle BackColor="#eb0029" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" />
            <SelectedRowStyle BackColor="white" Font-Bold="true" ForeColor="black" />
            <SortedAscendingCellStyle BackColor="#F5F7FB" />
            <SortedAscendingHeaderStyle BackColor="#eb0029" />
            <SortedDescendingCellStyle BackColor="#E9EBEF" />
            <SortedDescendingHeaderStyle BackColor="#eb0029" />
            <Columns>

                <asp:TemplateField HeaderStyle-Width="30px" HeaderStyle-HorizontalAlign="Left">

                    <ItemTemplate>
                        <asp:LinkButton ID="LbSelect" runat="server" CommandName="Select">
                            <asp:HiddenField ID="HfAlertTypeID" runat="server" Value='<%# Eval("ALERT_TYPE_ID") %>' />
                            <asp:Image ID="BtnWarning" runat="server" ImageUrl='<%# Eval("ALERT_TYPE_IMG_URL") %>' Height="20px" />
                        </asp:LinkButton>

                    </ItemTemplate>

                </asp:TemplateField>

                <asp:TemplateField HeaderText="Alert Description" HeaderStyle-Width="175px" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>

                        <asp:LinkButton ID="LBtnDescription" runat="server" Text='<%# Eval("ALERT_NAME") %>' CommandName="Select"></asp:LinkButton>
                    </ItemTemplate>

                </asp:TemplateField>
                <asp:TemplateField HeaderText="# records" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>

                        <asp:Label ID="LblGvID" runat="server" Text='<%# Eval("ALERT_COUNT") %>'></asp:Label>
                    </ItemTemplate>

                </asp:TemplateField>

            </Columns>
        </asp:GridView>
      
    </div>
    <div class="right_ebc">
        <h3>Alert Details</h3>
        <asp:Panel ID="Pnl_EmployeeAlertDetails" runat="server" Visible="false">
            <asp:GridView ID="GvAlertDetail" runat="server" CssClass="gridviewHeader" AutoGenerateColumns="false" CellPadding="1" ForeColor="black" GridLines="None" Width="500px" Font-Size="14px" ShowHeader="True" AllowPaging="True" PageSize="30" OnSelectedIndexChanged="GvAlertDetail_SelectedIndexChanged" OnPageIndexChanging="GvAlertDetail_PageIndexChanging" OnRowUpdating="GvAlertDetail_RowUpdating" OnRowDeleting="GvAlertDetail_RowDeleting" EmptyDataRowStyle-BackColor="Yellow">
                <AlternatingRowStyle BackColor="White" />
                <FooterStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" VerticalAlign="Bottom" />
                <RowStyle BackColor="#EFF3FB" />
                <SelectedRowStyle BackColor="white" Font-Bold="false" ForeColor="black" />
                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                <EmptyDataTemplate>
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField HeaderText="Payroll ID" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Literal Text='<%# Eval("EMPLOYEE_EXT_ID") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Employee" HeaderStyle-Width="395px" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:LinkButton ID="LBtnEmployeeName" runat="server" Text='<%# Eval("EMPLOYEE_FULL_NAME") %>' CommandName="Select"></asp:LinkButton>
                            <asp:HiddenField ID="HfDummyTrigger" runat="server" />
                            <asp:HiddenField ID="HfDummyTriggerPR" runat="server" />
                            <asp:HiddenField ID="HfDummyTriggerIns" runat="server" />
                            <asp:HiddenField ID="HfDummyTriggerCarrier" runat="server" />
                            <asp:HiddenField ID="HfRowID" runat="server" Value='<%# Eval("ROW_ID") %>' />
                            <asp:Panel ID="PnlEmployeeError" runat="server" DefaultButton="BtnSaveEmployeeCorrection" Style="display: none;">
                                <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                                </div>
                                <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 800px; height: auto; margin-top: 0px; font-size: 11px; background-color: white;">
                                    <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                        <asp:ImageButton ID="ImageButton2" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" />
                                    </div>
                                    <div style="width: 100%;">
                                        <h3 style="font-weight:bold;">Correct Employee Import Error</h3>
                                    </div>
                                    <div style="width: 100%">
                                        <div style="width: 48%; float: left;">
                                            <label class="lbl">ID</label>
                                            <asp:TextBox ID="Txt_ei_EmployeeID" runat="server" CssClass="txt" Enabled="false"></asp:TextBox>
                                            <br />
                                            <label class="lbl">Employee #</label>
                                            <asp:TextBox ID="Txt_ei_PayrollID" runat="server" CssClass="txt"></asp:TextBox>
                                            <br />
                                            <label class="lbl">First Name</label>
                                            <asp:TextBox ID="Txt_ei_FirstName" runat="server" CssClass="txtLong"></asp:TextBox>
                                            <br />
                                            <label class="lbl">Last Name</label>
                                            <asp:TextBox ID="Txt_ei_LastName" runat="server" CssClass="txtLong"></asp:TextBox>
                                            <br />
                                            <label class="lbl">Address</label>
                                            <asp:TextBox ID="Txt_ei_Address" runat="server" CssClass="txtLong"></asp:TextBox>
                                            <br />
                                            <label class="lbl">City</label>
                                            <asp:TextBox ID="Txt_ei_City" runat="server" CssClass="txtLong"></asp:TextBox>
                                            <br />
                                            <label class="lbl">State</label>
                                            <asp:DropDownList ID="Ddl_ei_State" runat="server" CssClass="ddl2"></asp:DropDownList>
                                            <br />
                                            <label class="lbl">Zip</label>
                                            <asp:TextBox ID="Txt_ei_Zip" runat="server" CssClass="txtLong"></asp:TextBox>
                                        </div>
                                        <div style="width: 48%; float: right;">
                                            <label class="lbl">
                                                <asp:ImageButton ID="ImgBtnViewSSN" runat="server" Height="15px" ImageUrl="~/design/eyeclosed.png" OnClick="ImgBtnViewSSN_Click" />
                                                SSN:
                                            </label>
                                            <asp:TextBox ID="Txt_ei_SSN" runat="server" CssClass="txt3"></asp:TextBox>
                                            <br />
                                            Format: 9 digits - No dashes - xxxxxxxxx

                                            <br />
                                            <label class="lbl">DOB</label>
                                            <asp:TextBox ID="Txt_ei_DOB" runat="server" CssClass="txtLong"></asp:TextBox>
                                            <br />
                                            format: 01/03/1995

                                                                <br />
                                            <label class="lbl">Hire Date</label>
                                            <asp:TextBox ID="Txt_ei_Hdate" runat="server" CssClass="txtLong"></asp:TextBox>
                                            <br />
                                            format: 01/23/1995
               
                                                                <br />
                                            <label class="lbl">Meas Plan Year</label>
                                            <asp:DropDownList ID="Ddl_ei_Plan" runat="server" CssClass="ddl2"></asp:DropDownList>
                                            <br />
                                            <label class="lbl">Employee Type</label>
                                            <asp:DropDownList ID="Ddl_ei_EmpType" runat="server" CssClass="ddl2"></asp:DropDownList>
                                            <br />
                                            <label class="lbl">HR Status</label>
                                            <asp:DropDownList ID="Ddl_ei_HRStatus" runat="server" CssClass="ddl2"></asp:DropDownList>
                                            <br />
                                            <label class="lbl">Classification</label>
                                            <asp:DropDownList ID="Ddl_ei_Classification" runat="server" CssClass="ddl2"></asp:DropDownList>
                                            <br />
                                            <label class="lbl">Status</label>
                                            <asp:DropDownList ID="Ddl_ei_ActStatus" runat="server" CssClass="ddl2"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <br style="clear: left" />
                                    <asp:Panel ID="PnlNewHireInsurance" runat="server" Visible="false">
                                        <div style="width: 100%;">
                                            <h3>New Hire - Offer of Insurance</h3>
                                            <span style="color: red">
                                                <asp:Literal ID="LitOfferMessage" runat="server"></asp:Literal></span>
                                            <br />
                                            1) Is this employee immediately eligible for insurance following your waiting period? 
           
                                                                <br />
                                            <asp:DropDownList ID="Ddl_ei_Insurance" runat="server" CssClass="ddl2">
                                                <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="false"></asp:ListItem>
                                                <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                                            </asp:DropDownList>
                                            <br />
                                            <br />
                                            2) If Yes, please select the Stability Period that this plan resides in. (Note: This will not be the same Stability Period that the employee was hired.)
       
                                                                <br />
                                            <asp:DropDownList ID="Ddl_ei_PlanYear_Coverage" runat="server" CssClass="ddl2"></asp:DropDownList>
                                            <br />
                                            <span style="background-color: yellow">
                                                <asp:Literal ID="lit_ei_message" runat="server"></asp:Literal>
                                            </span>
                                        </div>
                                    </asp:Panel>
                                    <div style="width: 100%;">
                                        <br style="clear: left;" />
                                        <br />
                                        <asp:Button ID="BtnSaveEmployeeCorrection" runat="server" CssClass="btn" Text="Submit" CommandName="Update" />
                                       
                                        <asp:Button ID="BtnDeleteEmployeeCorrection" runat="server" CssClass="btn" Text="Delete" CommandName="Delete" />
                                        <asp:ConfirmButtonExtender ID="CbePopupMessage" runat="server" TargetControlID="BtnDeleteEmployeeCorrection" ConfirmText="Are you sure you want to DELETE this record."></asp:ConfirmButtonExtender>
                                        <br />
                                        <asp:Label ID="Lbl_message" runat="server" BackColor="Yellow"></asp:Label>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="PnlPayrollError" runat="server" DefaultButton="BtnPayrollUpdate" Style="display: none;">
                                <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                                </div>
                                <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                                    <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                        <asp:ImageButton ID="ImageButton1" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" />
                                    </div>
                                    <h3>Correct Payroll Import Error</h3>
                                    <span style="color: grey;">Name:</span>
                                    <asp:Literal ID="Lit_Pay_Name" runat="server"></asp:Literal>
                                    <br />
                                    <span style="color: grey;">Employee #:</span>
                                    <asp:Literal ID="Lit_Pay_EmpExtID" runat="server"></asp:Literal>
                                    <br />
                                    <br />

                                    <label class="lbl">
                                        <asp:ImageButton ID="ImgBtnViewSSNPay" runat="server" Height="15px" ImageUrl="~/design/eyeclosed.png" OnClick="ImgBtnViewSSNPay_Click" />
                                        SSN:
                                    </label>
                                    <asp:TextBox ID="Txt_pay_ssn" runat="server" CssClass="txt3"></asp:TextBox>
                                    <br />
                                    Format: 9 digits - No dashes - xxxxxxxxx
                                                        
                                                        <br />
                                    <label class="lbl">Start Date:</label>
                                    <asp:TextBox ID="Txt_pay_sdate" runat="server" CssClass="txtLong"></asp:TextBox>
                                    format: 01/03/1995
   
                                                        <br />
                                    <label class="lbl">End Date:</label>
                                    <asp:TextBox ID="Txt_pay_edate" runat="server" CssClass="txtLong"></asp:TextBox>
                                    format: 01/23/1995
   
                                                        <br />
                                    <label class="lbl">Gross Pay Desc:</label>
                                    <asp:DropDownList ID="Ddl_pay_description" runat="server" CssClass="ddl2"></asp:DropDownList>
                                    <br />
                                    <label class="lbl">Hours Worked:</label>
                                    <asp:TextBox ID="Txt_pay_hours" runat="server" CssClass="txtLong"></asp:TextBox>
                                    format: 20.50
   
                                                        <br />
                                    <label class="lbl">Check Date:</label>
                                    <asp:TextBox ID="Txt_pay_cdate" runat="server" CssClass="txtLong"></asp:TextBox>
                                    format: 01/23/1995
   
                                                        <br />
                                    <label class="lbl4">.</label>

                                    <br />

                                    <asp:Button ID="BtnPayrollUpdate" runat="server" CssClass="btn" Text="Submit" CommandName="Update" />
                                    <asp:Button ID="BtnPayrollDelete" runat="server" CssClass="btn" Text="Delete" CommandName="Delete" />
                                    <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="BtnPayrollDelete" ConfirmText="Are you sure you want to DELETE this record."></asp:ConfirmButtonExtender>
                                    <br />
                                    <br />
                                    <asp:Label ID="LblPayrollMessage" runat="server" BackColor="Yellow"></asp:Label>
                                    <br />
                                    <br />
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="PnlInsuranceCarrier" runat="server" Style="display: none;">
                                <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                                </div>
                                <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                                    <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                        <asp:ImageButton ID="ImageButton5" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" />
                                    </div>
                                    <h3>Correct Insurance Carrier Alerts</h3>
                                    <asp:RadioButton ID="Rb_IC_employee" runat="server" Text="Plan Carrier" GroupName="employee" />
                                    <asp:RadioButton ID="Rb_IC_dependent" runat="server" Text="Dependent" GroupName="employee" />
                                    <br />
                                    <br />
                                    <asp:Panel ID="PnlEmployeeSearch" runat="server" DefaultButton="ImgBtnEmployeeSearch">
                                        <asp:Label ID="Lbl6" runat="server" CssClass="lbl3">Employee: </asp:Label>
                                        <asp:DropDownList ID="Ddl_IC_Employees" runat="server" CssClass="ddl2"></asp:DropDownList>
                                        <asp:TextBox ID="Txt_IC_EmployeeFilterSelection" runat="server" CssClass="txtSmall"></asp:TextBox>
                                        <asp:ImageButton ID="ImgBtnEmployeeSearch" runat="server" ImageUrl="/images/search-icon.png" OnClick="ImgBtnEmployeeSearch_Click" />
                                        <asp:HiddenField ID="Hf_IC_EmployeeID" runat="server" />
                                    </asp:Panel>
                                    <br />
                                    <asp:Label ID="Lbl7" runat="server" CssClass="lbl3">Dependent ID: </asp:Label>
                                    <asp:TextBox ID="Txt_IC_DependentID" runat="server" CssClass="txt" Enabled="false"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="Lbl8" runat="server" CssClass="lbl3">Tax Year</asp:Label>
                                    <asp:TextBox ID="Txt_IC_TaxYear" runat="server" CssClass="txt" Enabled="false"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="Lbl2" runat="server" CssClass="lbl3">First Name: </asp:Label>
                                    <asp:TextBox ID="Txt_IC_FirstName" runat="server" CssClass="txt"></asp:TextBox>
                                    <br />
                                    <asp:Label ID="Lbl3" runat="server" CssClass="lbl3">Last Name: </asp:Label>
                                    <asp:TextBox ID="Txt_IC_LastName" runat="server" CssClass="txt"></asp:TextBox>
                                    <br />

                                    <label class="lbl">
                                        <asp:ImageButton ID="ImgBtnViewSSNIC" runat="server" Height="15px" ImageUrl="~/design/eyeclosed.png" OnClick="ImgBtnViewSSNIC_Click" />
                                        SSN:
                                    </label>
                                    <asp:TextBox ID="Txt_IC_SSN" runat="server" CssClass="txt3"></asp:TextBox>
                                    <br />
                                    Format: 9 digits - No dashes - xxxxxxxxx
                                                        
                                                        <br />

                                    <asp:Label ID="Lbl5" runat="server" CssClass="lbl3">DOB: </asp:Label>
                                    <asp:TextBox ID="Txt_IC_DOB" runat="server" CssClass="txt"></asp:TextBox>
                                    <br />
                                    <br />
                                    <asp:Label ID="Label1" runat="server" CssClass="lbl3">. </asp:Label>
                                    <asp:Button ID="Btn_IC_Save" runat="server" Text="Save" CommandName="Update" />
                                    <asp:Button ID="Btn_IC_Delete" runat="server" Text="Delete" CommandName="Delete" />
                                    <asp:Button ID="Btn_IC_GenerateIA" runat="server" Text="Generate Missing Employee Alerts" OnClick="Btn_IC_GenerateIA_Click" />
                                    <asp:ConfirmButtonExtender ID="ConfirmButtonExtender3" runat="server" TargetControlID="Btn_IC_Delete" ConfirmText="Are you sure you want to DELETE this record? If this data is not available to the system, this individual's 1095c may not be produced or show the correct information."></asp:ConfirmButtonExtender>
                                    <br style="clear: both;" />
                                    <br />
                                    <span style="color: red; font-weight: bold">
                                        <asp:Literal ID="Lit_IC_Message" runat="server"></asp:Literal>
                                    </span>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="PnlInsuranceOffer" runat="server" Style="display: none;" DefaultButton="BtnUpdateInsurance">
                                <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                                </div>
                                <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                                    <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                        <asp:ImageButton ID="ImageButton3" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" />
                                    </div>
                                    <h3>Offer Of Insurance</h3>
                                    Stability Period:
   
                                                        <asp:Literal ID="Lit_io_PlanYear" runat="server"></asp:Literal>
                                    <asp:HiddenField ID="Hf_io_EmployeeID" runat="server" />
                                    <asp:HiddenField ID="Hf_io_PlanYearID" runat="server" />
                                    <br />
                                    Employee:
   
                                                        <asp:Literal ID="Lit_io_EmployeeName" runat="server"></asp:Literal>
                                    <br />
                                    Payroll ID:
   
                                                        <asp:Literal ID="Lit_io_PayrollID" runat="server"></asp:Literal>
                                    <br />
                                    Avg Hours/month:
   
                                                        <asp:Literal ID="Lit_io_MonthlyAvg" runat="server"></asp:Literal>
                                    <br />
                                    <hr />
                                    <br />
                                    <div style="padding-bottom: 20px; width: 595px;">
                                        <div style="float: left; width: 345px;">
                                            #1) Was this employee offered medical coverage?
           
                                                                <br />
                                            <asp:DropDownList ID="Ddl_io_Offered" runat="server" CssClass="ddl2" AutoPostBack="true" OnSelectedIndexChanged="Ddl_io_Offered_SelectedIndexChanged">
                                                <asp:ListItem Text="Select" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div style="float: right; width: 250px; text-align: left;">
                                            <asp:Panel ID="Pnl_io_DateOffered" runat="server" Visible="false">
                                                #2) Enter Date Offered - (Deprecated)
                                                                    <br />
                                                <asp:TextBox ID="Txt_io_DateOffered" runat="server" Enabled="false" CssClass="txt"></asp:TextBox>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                    <br />
                                    <br />
                                    <asp:Panel ID="Pnl_io_Accepted" runat="server" Visible="false">
                                        <div style="padding-bottom: 20px; width: 595px;">
                                            <div style="float: left; width: 345px;">
                                                #3) Did this employee accept the offer insurance?
       
                                                                    <br />
                                                <asp:DropDownList ID="Ddl_io_Accepted" runat="server" CssClass="ddl2" AutoPostBack="true" OnSelectedIndexChanged="Ddl_io_Accepted_SelectedIndexChanged">
                                                    <asp:ListItem Text="Select" Selected="true"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="false"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div style="float: right; width: 250px; text-align: left;">
                                                #4) Date Accepted/Declined - (Deprecated)
   
                                                                    <br />
                                                <asp:TextBox ID="Txt_io_AcceptedOffer" runat="server" CssClass="txt" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                        <br />
                                        <br />
                                    </asp:Panel>

                                    <asp:Panel ID="Pnl_io_Plan" runat="server" Visible="false">
                                        <div style="padding-bottom: 20px; width: 595px;">
                                            <div style="float: left; width: 345px;">
                                                <asp:Panel ID="PnlInsurancePlanOffered" runat="server">
                                                    #5) Which Plan was
                                                                        <asp:Literal ID="Literal2" runat="server"></asp:Literal>
                                                    offered?      
                                                                        <br />
                                                    <asp:DropDownList ID="Ddl_io_InsurancePlan" runat="server" CssClass="ddl2" AutoPostBack="true" OnSelectedIndexChanged="Ddl_io_InsurancePlan_SelectedIndexChanged"></asp:DropDownList>
                                                </asp:Panel>
                                            </div>
                                            <div style="float: right; width: 250px; text-align: left;">
                                                #6) Effective Date of Insurance      
                                                                    <br />
                                                <asp:TextBox ID="Txt_io_InsuranceEffectiveDate" runat="server" CssClass="txt"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender7" runat="server" TargetControlID="Txt_io_InsuranceEffectiveDate" DefaultView="Days"></asp:CalendarExtender>
                                            </div>
                                        </div>
                                        <br />
                                        <br />
                                    </asp:Panel>
                                    <asp:Panel ID="Pnl_io_Effective" runat="server" Visible="false">
                                        <div style="padding-bottom: 20px; width: 595px;">
                                            <div style="float: left; width: 345px;">
                                                #7) Select EMPLOYER Contribution:
       
                                                                    <br />
                                                <asp:DropDownList ID="Ddl_io_Contribution" runat="server" CssClass="ddl2"></asp:DropDownList>
                                            </div>
                                            <div style="float: right; width: 250px; text-align: left;">
                                                #8) HRA/Flex Additional Monthly Contributions
       
                                                                    <br />
                                                <asp:TextBox ID="Txt_io_HraFlex" runat="server" CssClass="txt3"></asp:TextBox>
                                            </div>
                                        </div>
                                        <br />
                                        <br />
                                    </asp:Panel>
                                    <br />
                                    Explanation/Notes:
                                                        <br />
                                    <asp:TextBox ID="Txt_io_Comments" runat="server" TextMode="MultiLine" Height="100px" Width="500px"></asp:TextBox>
                                    <br />
                                    <br />
                                    <label class="lbl4">.</label>
                                    <asp:Button ID="BtnUpdateInsurance" runat="server" CssClass="btn" Text="Submit" CommandName="Update" />
                                    <asp:Button ID="BtnDeleteInsurance" runat="server" CssClass="btn" Text="Delete" CommandName="Delete" Enabled="false" />
                                    <asp:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" TargetControlID="BtnDeleteInsurance" ConfirmText="Are you sure you want to DELETE this record."></asp:ConfirmButtonExtender>
                                    <br />
                                    <br />
                                    <asp:Label ID="LblInsuranceMessage" runat="server" BackColor="Yellow"></asp:Label>
                                    <br />
                                    <br />
                                </div>
                            </asp:Panel>
                            <asp:ModalPopupExtender ID="mpeEditEmployee" runat="server" TargetControlID="HfDummyTrigger" PopupControlID="PnlEmployeeError"></asp:ModalPopupExtender>
                            <asp:ModalPopupExtender ID="mpeEditPayroll" runat="server" TargetControlID="HfDummyTriggerPR" PopupControlID="PnlPayrollError"></asp:ModalPopupExtender>
                            <asp:ModalPopupExtender ID="mpeEditInsurance" runat="server" TargetControlID="HfDummyTriggerIns" PopupControlID="PnlInsuranceOffer"></asp:ModalPopupExtender>
                            <asp:ModalPopupExtender ID="MpeEditCarrier" runat="server" TargetControlID="HfDummyTriggerCarrier" PopupControlID="PnlInsuranceCarrier"></asp:ModalPopupExtender>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

        </asp:Panel>
        <asp:Panel ID="Pnl_BillWarning" runat="server" Visible="false">
            <p>
                Please select a USER/Employee that will be designated as the contact person for the Billing. 
            </p>
            <br />
            <label class="lbl">Billing Contact</label>
            <asp:DropDownList ID="Ddl_bill_Users" runat="server" CssClass="ddl2"></asp:DropDownList>
            <br />
            <asp:Button ID="BtnUpdateBillContact" runat="server" CssClass="btn" Text="Submit" OnClick="BtnUpdateBillContact_Click" />
        </asp:Panel>
        <asp:Panel ID="Pnl_IRSWarning" runat="server" Visible="false">
            <p>
                Please select a USER/Employee that will be designated as the contact person for the IRS 1094/1095 reporting. 
            </p>
            <br />
            <label class="lbl">IRS Contact</label>
            <asp:DropDownList ID="Ddl_irs_Users" runat="server" CssClass="ddl2"></asp:DropDownList>
            <br />
            <asp:Button ID="BtnUpdateIrsContact" runat="server" CssClass="btn" Text="Submit" OnClick="BtnUpdateIrsContact_Click" />
        </asp:Panel>
        <asp:Panel ID="Pnl_InsuranceTypeWarning" runat="server" Visible="false">
            <p>
                Please go to the Setup Screen and update the following medical plans. For reporting purposes we will need to know 
    if these plans are Full-Insured vs. Self-Insured.
            </p>


            <asp:GridView ID="GvInsuranceAlert" runat="server" CssClass="gridviewHeader" AutoGenerateColumns="false" CellPadding="1" ForeColor="#333333" GridLines="None" Width="500px" Font-Size="10px" ShowHeader="True" AllowPaging="True" PageSize="30" OnSelectedIndexChanging="GvInsuranceAlert_SelectedIndexChanging" OnRowUpdating="GvInsuranceAlert_RowUpdating" OnPageIndexChanging="GvInsuranceAlert_PageIndexChanging">
                <AlternatingRowStyle BackColor="White" />
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="transparent" Font-Bold="True" ForeColor="White" VerticalAlign="Bottom" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#EFF3FB" />
                <SelectedRowStyle BackColor="Yellow" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                <Columns>
                    <asp:TemplateField HeaderText="Medical plan" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:LinkButton ID="LBtn_gv_insName" runat="server" Text='<%# Eval("INSURANCE_NAME") %>' CommandName="Select"></asp:LinkButton>
                            <asp:HiddenField ID="HfDummyTriggerInsType" runat="server" />
                            <asp:HiddenField ID="HfInsuranceID" runat="server" Value='<%# Eval("INSURANCE_ID") %>' />
                            <asp:Panel ID="PnlInsType" runat="server">
                                <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                                </div>
                                <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                                    <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                        <asp:ImageButton ID="ImageButton4" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" />
                                    </div>
                                    <h3>Required Field</h3>
                                    <p>
                                        Please select Fully-Insured or Self-Insured. 
       
                                    </p>
                                    <asp:DropDownList ID="Ddl_gv_InsuranceType" runat="server" CssClass="ddl2"></asp:DropDownList>
                                    <br />
                                    <asp:Button ID="Btn_gv_SaveInsuranceType" runat="server" CssClass="btn" Text="Submit" CommandName="Update" />
                                    <br />
                                    <asp:Label ID="LblInsTypeMessage" runat="server" BackColor="Yellow"></asp:Label>
                                    <br />
                                    <br />
                                </div>
                            </asp:Panel>
                            <asp:ModalPopupExtender ID="mpeEditInsType" runat="server" TargetControlID="HfDummyTriggerInsType" PopupControlID="PnlInsType"></asp:ModalPopupExtender>
                        </ItemTemplate>
                    </asp:TemplateField>


                </Columns>

            </asp:GridView>

        </asp:Panel>
    </div>
</asp:Content>
