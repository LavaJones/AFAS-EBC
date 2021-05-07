<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="view_employers.aspx.cs" Inherits="admin_view_employers" %>
<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html>
<head>
    <title>View Employers</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" type="text/css" href="../default.css" />
    <link rel="stylesheet" type="text/css" href="../menu.css" />
    <link rel="stylesheet" type="text/css" href="../v_menu.css" />

</head>
<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" AsyncPostBackTimeout="3600"></asp:ToolkitScriptManager>
        <div id="container">
            <div id="header">
                <a href="default.aspx">
                    <img src="<%= Page.ResolveUrl(Branding.LogoUri) %>" style="height: 70px" alt="<%= Branding.ProductName %> Logo" class="logo" />
                </a>
                <ul id="toplinks">
                    <li>Need Help? Call <%= Branding.PhoneNumber %></li> 
                    <li>
                        <asp:Literal ID="LitUserName" runat="server"></asp:Literal></li>
                    <li>
                        <asp:Button ID="BtnLogout" CssClass="btn" runat="server" Text="Log Out" OnClick="BtnLogout_Click" UseSubmitBehavior="false" /></li>
                </ul>
                <asp:HiddenField ID="HfDistrictID" runat="server" />
            </div>
            <div id="nav2">
                <nav>
                    <%= demo.getAdminLinks() %>
                </nav>
            </div>
            <asp:UpdatePanel ID="UpEmployerView" runat="server">
                <ContentTemplate>

                    <div id="topbox">
                        <h4>View Employers</h4>
                        <label class="lbl">Select Employer</label>
                        <asp:DropDownList ID="DdlFilterEmployers" runat="server" OnSelectedIndexChanged="DdlFilterEmployers_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                        <br />
                        <br />
                        <asp:Button ID="BtnUpdateAverages" runat="server" Text="Re-Calculate Batch Reporting" OnClick="BtnUpdateAverages_Click" />
                        <br />
                        <asp:GridView ID="GvEmployers" runat="server" AutoGenerateColumns="False" AllowPaging="True" PageSize="1000" OnRowDataBound="GvEmployers_RowDataBound" CellPadding="4" ForeColor="#333333" GridLines="None" Width="975px" OnRowUpdating="GvEmployers_RowUpdating" OnRowDeleting="GvEmployers_RowDeleting">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField HeaderText="Employer Name" HeaderStyle-Width="300px" HeaderStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top">
                                    <ItemTemplate>
                                        <h3>Payroll Vendor</h3>
                                        <asp:DropDownList ID="DdlPayrollVendors" runat="server" CssClass="ddl2"></asp:DropDownList>
                                        <br />
                                        <h3>Automated Controls</h3>
                                        <asp:CheckBox ID="CbSetupFeePaid" runat="server" Text="Set-up Fee Paid" TextAlign="Left" Checked='<%# Eval("EMPLOYER_SU_BILLED") %>' Enabled="false" />
                                        <asp:ToggleButtonExtender ID="TbeSetupFeePaid" runat="server" TargetControlID="CbSetupFeePaid" CheckedImageAlternateText="This cannot be changed on this screen." UncheckedImageAlternateText="This cannot be changed on this screen." CheckedImageUrl="~/images/circle_green.png" UncheckedImageUrl="~/images/circle_red.png" ImageWidth="25" ImageHeight="24"></asp:ToggleButtonExtender>
                                        <br />
                                        <br />
                                        <asp:CheckBox ID="CbMonthlyBillingOn" runat="server" TextAlign="Left" Text="Monthly Bill On" Checked='<%# Eval("EMPLOYER_AUTO_BILL") %>' />
                                        <asp:ToggleButtonExtender ID="ToggleButtonExtender1" runat="server" TargetControlID="CbMonthlyBillingOn" CheckedImageAlternateText="Checked" UncheckedImageAlternateText="UnChecked" CheckedImageUrl="~/images/circle_green.png" UncheckedImageUrl="~/images/circle_red.png" ImageWidth="25" ImageHeight="24"></asp:ToggleButtonExtender>
                                        <br />
                                        <br />
                                        <asp:CheckBox ID="CbAutoFileUploadOn" runat="server" Text="Auto File Upload" TextAlign="Left" Checked='<%# Eval("EMPLOYER_AUTO_UPLOAD") %>' />
                                        <asp:ToggleButtonExtender ID="ToggleButtonExtender2" runat="server" TargetControlID="CbAutoFileUploadOn" CheckedImageAlternateText="Checked" UncheckedImageAlternateText="UnChecked" CheckedImageUrl="~/images/circle_green.png" UncheckedImageUrl="~/images/circle_red.png" ImageWidth="25" ImageHeight="24"></asp:ToggleButtonExtender>
                                        <h3>File Import S/U</h3>
                                        <dl style="text-align: right;">
                                            <dt style="font-weight: bold;">Payroll Import:
                                                <asp:TextBox ID="TxtPaySU" runat="server" Width="75px" Height="15px" Text='<%# Eval("EMPLOYER_IMPORT_PAYROLL") %>'></asp:TextBox></dt>
                                            <dt style="font-weight: bold;">Demographic Import:
                                                <asp:TextBox ID="TxtDemSU" runat="server" Width="75px" Height="15px" Text='<%# Eval("EMPLOYER_IMPORT_EMPLOYEE") %>'></asp:TextBox></dt>
                                            <dt style="font-weight: bold;">Pay Code Import:
                                                <asp:TextBox ID="TxtPcSU" runat="server" Width="75px" Height="15px" Text='<%# Eval("EMPLOYER_IMPORT_GP") %>'></asp:TextBox></dt>
                                            <dt style="font-weight: bold;">HR Status Import:
                                                <asp:TextBox ID="TxtHrSU" runat="server" Width="75px" Height="15px" Text='<%# Eval("EMPLOYER_IMPORT_HR") %>'></asp:TextBox></dt>
                                            <dt style="font-weight: bold;">Employee Class Import:
                                                <asp:TextBox ID="TxtEcSU" runat="server" Width="75px" Height="15px" Text='<%# Eval("EMPLOYER_IMPORT_EC") %>'></asp:TextBox>
                                            </dt>
                                            <dt style="font-weight: bold;">Insurance Offer Import:
                                                <asp:TextBox ID="TxtIoSU" runat="server" Width="75px" Height="15px" Text='<%# Eval("EMPLOYER_IMPORT_IO") %>'></asp:TextBox>
                                            </dt>
                                            <dt style="font-weight: bold;">Insurance Carrier Import:
                                                <asp:TextBox ID="TxtIcSu" runat="server" Width="75px" Height="15px" Text='<%# Eval("EMPLOYER_IMPORT_IC") %>'></asp:TextBox>
                                            </dt>
                                            <dt style="font-weight: bold;">Payroll Modification Import:
                                                <asp:TextBox ID="TxtPayModSu" runat="server" Width="75px" Height="15px" Text='<%# Eval("EMPLOYER_IMPORT_PAY_MOD") %>'></asp:TextBox>
                                            </dt>
                                        </dl>
                                        <h3>Set-up Process</h3>
                                        <dl style="text-align: right;">
                                            <dt style="font-weight: bold;">Initial Employee Import:
                                                <asp:TextBox ID="TxtEmpInit_I" runat="server" Width="75px" Height="15px" Text='<%# Eval("EMPLOYER_IEI") %>'></asp:TextBox></dt>
                                            <dt style="font-weight: bold;">Initial Employee Cleaned Up:
                                                <asp:TextBox ID="TxtEmpInit_C" runat="server" Width="75px" Height="15px" Text='<%# Eval("EMPLOYER_IEC") %>'></asp:TextBox></dt>
                                            <dt style="font-weight: bold;">FTP Employee Import:
                                                <asp:TextBox ID="TxtEmpFTP_I" runat="server" Width="75px" Height="15px" Text='<%# Eval("EMPLOYER_FTPEI") %>'></asp:TextBox></dt>
                                            <dt style="font-weight: bold;">FTP Employee Cleaned Up:
                                                <asp:TextBox ID="TxtEmpFPT_C" runat="server" Width="75px" Height="15px" Text='<%# Eval("EMPLOYER_FTPEC") %>'></asp:TextBox></dt>
                                            <dt style="font-weight: bold;">Initial Payroll Import:
                                                <asp:TextBox ID="TxtPayInit_I" runat="server" Width="75px" Height="15px" Text='<%# Eval("EMPLOYER_IPI") %>'></asp:TextBox></dt>
                                            <dt style="font-weight: bold;">Initial Payroll Data Cleaned Up:
                                                <asp:TextBox ID="TxtPayInit_C" runat="server" Width="75px" Height="15px" Text='<%# Eval("EMPLOYER_IPC") %>'></asp:TextBox></dt>
                                            <dt style="font-weight: bold;">FTP Payroll Import:
                                                <asp:TextBox ID="TxtPayFTP_I" runat="server" Width="75px" Height="15px" Text='<%# Eval("EMPLOYER_FTPPI") %>'></asp:TextBox></dt>
                                            <dt style="font-weight: bold;">FTP Payroll Data Cleaned Up:
                                                <asp:TextBox ID="TxtPayFTP_C" runat="server" Width="75px" Height="15px" Text='<%# Eval("EMPLOYER_FTPPC") %>'></asp:TextBox></dt>
                                            <dt style="font-weight: bold;">Process Confirmed Complete:
                                                <asp:TextBox ID="TxtComplete" runat="server" Width="75px" Height="15px" Text='<%# Eval("EMPLOYER_IMPORT") %>'></asp:TextBox></dt>
                                        </dl>
                                        <asp:Button ID="BtnSaveEmployer" runat="server" Text="Save" Width="300px" CssClass="btn" CommandName="Update" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Plan Years" HeaderStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top">
                                    <ItemTemplate>
                                        <h3>
                                            <asp:Literal ID="LitEmployerName" runat="server" Text='<%# Eval("EMPLOYER_NAME") %>'></asp:Literal>
                                        </h3>
                                        ID:
                                        <asp:Literal ID="LitEmployerID" runat="server" Text='<%# Eval("EMPLOYER_ID") %>'></asp:Literal>
                                        <asp:GridView ID="Gv_gv_PlanYears" runat="server" AutoGenerateColumns="false" BackColor="White" BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" CellPadding="4" GridLines="Horizontal" OnRowDataBound="Gv_gv_PlanYears_RowDataBound">
                                            <FooterStyle BackColor="White" ForeColor="#333333" />
                                            <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                                            <RowStyle BackColor="White" ForeColor="#333333" />
                                            <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                            <SortedAscendingHeaderStyle BackColor="#487575" />
                                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                            <SortedDescendingHeaderStyle BackColor="#275353" />
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top">
                                                    <ItemTemplate>
                                                        <asp:Image ID="img_gv_MP" runat="server" ImageUrl="~/images/circle_green.png" Height="15px" />
                                                        <asp:HiddenField ID="Hf_gv_employerID" runat="server" Value='<%# Eval("PLAN_YEAR_EMPLOYER_ID") %>' />
                                                        <asp:HiddenField ID="Hf_gv_planID" runat="server" Value='<%# Eval("PLAN_YEAR_ID") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Plan" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Lbl_gv_Plan" runat="server" Text='<%# Eval("PLAN_YEAR_DESCRIPTION") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Renewal" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Lbl_gv_Renewal" runat="server" Text='<%# Eval("PLAN_YEAR_START", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="MP" HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top">
                                                    <ItemTemplate>
                                                        MP:
                                       
                                                        <asp:Literal ID="Lit_gv_MP_start" runat="server"></asp:Literal>
                                                        to 
                                       
                                                        <asp:Literal ID="Lit_gv_MP_end" runat="server"></asp:Literal>
                                                        <br />
                                                        AP:
                                       
                                                        <asp:Literal ID="Lit_gv_AP_start" runat="server"></asp:Literal>
                                                        to 
                                       
                                                        <asp:Literal ID="Lit_gv_AP_end" runat="server"></asp:Literal>
                                                        <br />
                                                        OE:
                                       
                                                        <asp:Literal ID="Lit_gv_OE_start" runat="server"></asp:Literal>
                                                        to 
                                       
                                                        <asp:Literal ID="Lit_gv_OE_end" runat="server"></asp:Literal>
                                                        <br />
                                                        SP:
                                       
                                                        <asp:Literal ID="Lit_gv_SP_start" runat="server"></asp:Literal>
                                                        to
                                       
                                                        <asp:Literal ID="Lit_gv_SP_end" runat="server"></asp:Literal>
                                                        <br />
                                                        SW:
                                       
                                                        <asp:Literal ID="Lit_gv_SW_start" runat="server"></asp:Literal>
                                                        to
                                       
                                                        <asp:Literal ID="Lit_gv_SW_end" runat="server"></asp:Literal>
                                                        <br />
                                                        SW 2:
                                       
                                                        <asp:Literal ID="Lit_gv_SW2_start" runat="server"></asp:Literal>
                                                        to
                                       
                                                        <asp:Literal ID="Lit_gv_SW2_end" runat="server"></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <h3>Users</h3>
                                        <asp:GridView ID="Gv_gv_Users" runat="server" AutoGenerateColumns="false" BackColor="White" BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" CellPadding="4" GridLines="Horizontal">
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
                                                <asp:TemplateField HeaderText="First Name" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblGvFirstName" runat="server" Text='<%# Eval("User_First_Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Last Name" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblGvLastName" runat="server" Text='<%# Eval("User_Last_Name") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Email" HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="LblGvEmail" runat="server" Text='<%# Eval("User_Email") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Admin" HeaderStyle-Width="30px" HeaderStyle-HorizontalAlign="Left" ControlStyle-Width="75px">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CbPowerUser" runat="server" Checked='<%# Eval("User_Power") %>' Enabled="false" />
                                                        <asp:ToggleButtonExtender ID="ToggleButtonExtender3" runat="server" TargetControlID="CbPowerUser" CheckedImageAlternateText="Checked" UncheckedImageAlternateText="UnChecked" CheckedImageUrl="~/images/circle_green.png" UncheckedImageUrl="~/images/circle_red.png" ImageWidth="25" ImageHeight="24"></asp:ToggleButtonExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Billing" HeaderStyle-Width="30px" HeaderStyle-HorizontalAlign="Left" ControlStyle-Width="75px">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="CbBillingUser" runat="server" Checked='<%# Eval("User_Billing") %>' Enabled="false" />
                                                        <asp:ToggleButtonExtender ID="ToggleButtonExtender4" runat="server" TargetControlID="CbBillingUser" CheckedImageAlternateText="Checked" UncheckedImageAlternateText="UnChecked" CheckedImageUrl="~/images/circle_green.png" UncheckedImageUrl="~/images/circle_red.png" ImageWidth="25" ImageHeight="24"></asp:ToggleButtonExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <h3>Alerts - 
                           
                                            <asp:Button ID="BtnNewAlert" runat="server" Text="New" CssClass="btn" />
                                        </h3>
                                        <asp:Panel ID="PnlNewAlert" runat="server">
                                            <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                                            </div>
                                            <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white; z-index: 10">
                                                <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                                    <asp:ImageButton ID="ImgBtnClose2" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" BorderStyle="None" />
                                                </div>
                                                <h3 style="color: black;">Add New Alert</h3>
                                                <label class="lbl2">Alert Type</label>
                                                <asp:DropDownList ID="DdlAlertType" runat="server" CssClass="ddl2"></asp:DropDownList>
                                                <br />
                                                <asp:Button ID="BtnSaveAlert" runat="server" Text="Save Alert" CssClass="btn" CommandName="Delete" />
                                                <p style="color: darkgray">
                                                    <asp:Literal ID="LitAlertMessage" runat="server"></asp:Literal>
                                                </p>
                                                <br />
                                            </div>
                                        </asp:Panel>
                                        <asp:ModalPopupExtender ID="MpeNewAlert" runat="server" TargetControlID="BtnNewAlert" OkControlID="ImgBtnClose2" PopupControlID="PnlNewAlert"></asp:ModalPopupExtender>
                                        <asp:GridView ID="Gv_gv_Alerts" runat="server" AutoGenerateColumns="false"
                                            EmptyDataText="There are currently NO ALERTS setup for this employer!" BackColor="White" BorderColor="#336666"
                                            BorderStyle="Double" BorderWidth="3px" CellPadding="4" GridLines="Horizontal"
                                            OnRowDeleting="Gv_gv_Alerts_RowDeleting">
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
                                                <asp:TemplateField HeaderText="Alert Name" HeaderStyle-Width="225px" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="HiddenAlertId" runat="server" Value='<%# Eval("ALERT_TYPE_ID") %>' />
                                                        <asp:Literal ID="LitAlertName" runat="server" Text='<%# Eval("ALERT_TYPE_NAME") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Alert Count" HeaderStyle-Width="225px" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Literal ID="LitAlertCount" runat="server" Text='<%# Eval("ALERT_COUNT") %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="25px" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:ImageButton Width="25px" ID="ImgBtnDelete" runat="server" ImageUrl="~/images/close_box_red.png" Height="20px" CommandName="Delete" ToolTip="Delete this Alert" />
                                                        <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="ImgBtnDelete" ConfirmText="Are you sure you want to DELETE this Alert?"></asp:ConfirmButtonExtender>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>

                                        <h3>Employee Count</h3>
                                        Employee Count: <asp:Literal ID="EmployeeCount" runat="server"></asp:Literal>
                                    
                                        <h3>Batch Count</h3>
                                        Batch Count: <asp:Literal ID="BatchCount" runat="server"></asp:Literal>
                                    
                                        <h3>HR Status Count</h3>
                                        Hr Status Count: <asp:Literal ID="HrStatusCount" runat="server"></asp:Literal>                                       
                                        
                                        <h3>Pay Description Count</h3>
                                        Pay Description  Count: <asp:Literal ID="PayDescCount" runat="server"></asp:Literal>
                                                                                                           
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EditRowStyle BackColor="#2461BF" />
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EFF3FB" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#F5F7FB" />
                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                            <SortedDescendingCellStyle BackColor="#E9EBEF" />
                            <SortedDescendingHeaderStyle BackColor="#4870BE" />
                        </asp:GridView>
                        <asp:HiddenField ID="HfDummyTrigger" runat="server" />
                        <asp:HiddenField ID="HfDummyTrigger2" runat="server" />

                        <asp:Panel ID="PnlMessage" runat="server">
                            <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                            </div>
                            <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white; z-index: 10">
                                <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                    <asp:ImageButton ID="ImgBtnClose" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" BorderStyle="None" />
                                </div>
                                <h3 style="color: black;">Webpage Message</h3>
                                <p style="color: darkgray">
                                    <asp:Literal ID="LitMessage" runat="server"></asp:Literal>
                                </p>
                                <br />
                            </div>
                        </asp:Panel>

                        <asp:ModalPopupExtender ID="MpeWebMessage" runat="server" TargetControlID="HfDummyTrigger" OkControlID="ImgBtnClose" PopupControlID="PnlMessage"></asp:ModalPopupExtender>

                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpEmployerView" DynamicLayout="true" DisplayAfter="500">
                <ProgressTemplate>
                    <div style="position: fixed; top: 0; left: 0; background-color: white; width: 100%; height: 100%; opacity: .85; filter: alpha(opacity=85); -moz-opacity: 0.85; text-align: center;">
                        <div style="position: relative; margin-left: auto; margin-right: auto; background-color: white; padding-top: 100px;">
                            <h4>Calculating EMPLOYEE averages..... This can take a few minutes.....</h4>
                            <asp:Image ID="ImgSearching" runat="server" ImageUrl="~/design/icon-loading-animated.gif" />
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>

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
