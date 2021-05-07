<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="1095_correction.aspx.cs" Inherits="correction_1095" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" type="text/css" href="../default.css" />
    <link rel="stylesheet" type="text/css" href="../menu.css" />
    <link rel="stylesheet" type="text/css" href="../v_menu.css" />
    <title>1095 Correction</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
        <div id="container">
            <div id="header" style="height: 90px">
                <a href="default.aspx">
                    <img src="<%= Page.ResolveUrl(Branding.LogoUri) %>" style="height: 70px" alt="<%= Branding.ProductName %> Logo" class="logo" />
                </a>
                <ul id="toplinks">
                    <li>
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

                <asp:UpdatePanel ID="UpEmployee" runat="server" UpdateMode="Conditional">

                    <ContentTemplate>
                        <div id="topbox">
                            <div id="tbleft" style="padding-bottom: 20px;">
                                <asp:Panel ID="PnlFilter" runat="server" DefaultButton="BtnApplyFilters">
                                    <h3>Filter By</h3>
                                    Calendar Year:
                           
                                    <br />
                                    <asp:DropDownList ID="DdlCalendarYear" runat="server" CssClass="ddl">
                                        <asp:ListItem Text="2015" Value="2015"></asp:ListItem>
                                        <asp:ListItem Text="2016" Value="2016" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="2017" Value="2017"></asp:ListItem>
                                        <asp:ListItem Text="2018" Value="2018"></asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
                                    Filter by Preliminary 1095C Results:
                           
                                    <br />
                                    <asp:DropDownList ID="Ddl1095Data" runat="server" CssClass="ddl2">
                                        <asp:ListItem Text="Preliminarily flagged for 1095C" Value="true" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Not expected to receive 1095C" Value="false"></asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
                                    Filter by Insurance Carrier Report:
                           
                                    <br />
                                    <asp:DropDownList ID="DdlFilterList" runat="server" CssClass="ddl2">
                                        <asp:ListItem Text="Employees IN Insurance Carrier Report" Value="true"></asp:ListItem>
                                        <asp:ListItem Text="Employees NOT in Insurance Carrier Report" Value="false"></asp:ListItem>
                                        <asp:ListItem Text="Select All" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
                                    Filter by Mismatched offers/carrier import:
                           
                                    <br />
                                    <asp:DropDownList ID="DdlMismatchedData" runat="server" CssClass="ddl2">
                                        <asp:ListItem Text="Employees with mismatched records" Value="true"></asp:ListItem>
                                        <asp:ListItem Text="Select All" Selected="True"></asp:ListItem>
                                    </asp:DropDownList>
                                    <br />
                                    Filter by Employee Class:
                           
                                    <br />
                                    <asp:DropDownList ID="Ddl_f_EmployeeClass" runat="server" CssClass="ddl2"></asp:DropDownList>
                                    <br />
                                    <br />
                                    <asp:Button ID="BtnApplyFilters" runat="server" Text="Apply Filters" CssClass="btn" Width="95%" OnClick="BtnApplyFilters_Click" />
                                </asp:Panel>
                            </div>
                            <div id="tbright">
                                <asp:Panel ID="PnlSearch" runat="server" DefaultButton="BtnFindEmployees">
                                    <br />
                                    <h3>Find Employee</h3>
                                    <asp:RadioButton ID="RbLastName" runat="server" GroupName="search" Checked="true" Text="Search by Last Name" />
                                    <br />
                                    <asp:RadioButton ID="RbPayrollID" runat="server" GroupName="search" Text="Search by Payroll ID" />
                                    <br />
                                    <asp:TextBox ID="Txt_f_searchValue" runat="server" CssClass="txtLong"></asp:TextBox>
                                    <br />
                                    <br />
                                    <asp:Button ID="BtnFindEmployees" runat="server" Text="Search for Employee" CssClass="btn" Width="95%" OnClick="BtnFindEmployees_Click" />
                                </asp:Panel>

                            </div>
                        </div>
                        <br />
                        <hr />
                        <table>
                            <tr>
                                <td>
                                    <h3>Certify 1095-C Information</h3>
                                    <p>
                                        Each 1095C form must be approved by the Employer. Please view each form below in the list of Employees below. 
                           
                                    </p>
                                    Check All:
                                    <asp:CheckBox ID="CbCheckAll1095C" runat="server" AutoPostBack="true" OnCheckedChanged="CbCheckAll1095C_CheckedChanged" />
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td>&nbsp;</td>
                            </tr>
                            <tr>
                                <td>Showing 
                           
                                    <asp:Literal ID="litAlertsShown" runat="server"></asp:Literal>
                                    1095C Forms of 
                           
                                    <asp:Literal ID="litAlertCount" runat="server"></asp:Literal>
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>*Note: Check all will only flag records shown on this page.</td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td colspan="3" style="text-align: left;">Sort by Last Name
                           
                                    <asp:DropDownList ID="DdlSorting" runat="server" AutoPostBack="true" CssClass="ddlSmall" OnSelectedIndexChanged="DdlSorting_SelectedIndexChanged">
                                        <asp:ListItem Text="Asc" Value="ASC" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Desc" Value="DESC"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td colspan="3" style="text-align: left;">Page #:
                           
                                    <asp:DropDownList ID="DdlPageNumber" runat="server" AutoPostBack="true" CssClass="ddlSmall" OnSelectedIndexChanged="DdlPageNumber_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                                <td colspan="3" style="text-align: left;">Paging Size:
                           
                                    <asp:DropDownList ID="DdlPageSize" runat="server" AutoPostBack="true" CssClass="ddlSmall" OnSelectedIndexChanged="DdlPageSize_SelectedIndexChanged">
                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                        <asp:ListItem Text="25" Value="25" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                        <asp:ListItem Text="75" Value="75"></asp:ListItem>
                                        <asp:ListItem Text="100" Value="100"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="13">
                                    <asp:GridView ID="Gv1095" runat="server"
                                        AllowPaging="true" PageSize="25" AutoGenerateColumns="false"
                                        CellPadding="1" ForeColor="#333333" GridLines="None" Width="99%" Font-Size="10px" ShowHeader="false" ShowFooter="false" EmptyDataRowStyle-BackColor="Yellow"
                                        OnRowDataBound="Gv1095_RowDataBound"
                                        OnSelectedIndexChanging="Gv1095_SelectedIndexChanging"
                                        OnRowCancelingEdit="Gv1095_RowCancelingEdit"
                                        OnRowUpdating="Gv1095_RowUpdating"
                                        OnRowCommand="Gv1095_RowCommand">
                                        <AlternatingRowStyle BackColor="White" />
                                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" VerticalAlign="Bottom" />
                                        <RowStyle BackColor="#EFF3FB" />
                                        <SelectedRowStyle BackColor="Yellow" Font-Bold="True" ForeColor="#333333" />
                                        <PagerSettings Visible="false" />
                                        <EmptyDataTemplate>
                                            No records were found.
                                  
                                        </EmptyDataTemplate>
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" SortExpression="EMPLOYEE_LAST_NAME" HeaderText="Sort By Name">
                                                <ItemTemplate>
                                                    <tr style="font-weight: bold; background-color: #507CD1; color: white;">
                                                        <td colspan="1" style="width: 75px;">PART I</td>
                                                        <td colspan="1" style="width: 75px;">Get 1095C</td>
                                                        <td colspan="1" style="width: 75px;">Name</td>
                                                        <td colspan="1" style="width: 75px;">SSN</td>
                                                        <td colspan="2" style="width: 75px;">Address</td>
                                                        <td colspan="2" style="width: 75px;">City</td>
                                                        <td colspan="1" style="width: 75px;">State</td>
                                                        <td colspan="1" style="width: 75px;">Zip</td>
                                                        <td colspan="1" style="width: 75px;">Hire Date</td>
                                                        <td colspan="1" style="width: 75px;">Term Date</td>

                                                        <td style="width: 75px;"></td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="1">
                                                            <asp:CheckBox ID="Cb_gv_1095" runat="server" />
                                                            <asp:ImageButton ID="ImgBtnEdit1095" runat="server" ImageUrl="~/images/edit_notes.png" Width="20px" CommandName="Select" ToolTip="EDIT 1095C data" />
                                                            <asp:ImageButton ID="ImgBtnAll12" runat="server" ImageUrl="~/images/history.png" Width="20px" CommandName="All12" ToolTip="ADD Justification" />
                                                            <asp:ImageButton ID="ImgBtnSave1095" runat="server" ImageUrl="~/images/disk-save.png" Width="20px" CommandName="Update" ToolTip="SAVE 1095C data" Visible="false" />
                                                            <asp:ImageButton ID="ImgBtnCancel" runat="server" ImageUrl="~/images/back-icon.png" Width="20px" CommandName="Cancel" ToolTip="CANCEL EDITING" Visible="false" />
                                                        </td>
                                                        <td colspan="1" style="background-color: lightcyan;">
                                                            <asp:CheckBox ID="Cb1095c" runat="server" Enabled="false" Checked='<%# Eval("EMPLOYEE_REC_1095c") %>' />
                                                        </td>
                                                        <td colspan="1" style="background-color: lightcyan;">
                                                            <asp:TextBox Enabled="false" Width="75px" ID="Txt_gv_FirstName" runat="server" Text='<%# Eval("EMPLOYEE_FIRST_NAME") %>'></asp:TextBox>
                                                            <asp:TextBox Enabled="false" Width="75px" ID="Txt_gv_MiddleName" runat="server" Text='<%# Eval("EMPLOYEE_MIDDLE_NAME") %>'></asp:TextBox>
                                                            <asp:TextBox Enabled="false" Width="75px" ID="Txt_gv_LastName" runat="server" Text='<%# Eval("EMPLOYEE_LAST_NAME") %>'></asp:TextBox>

                                                            <asp:HiddenField ID="Hv_gv_EmployeeID" runat="server" Value='<%# Eval("EMPLOYEE_ID") %>' />
                                                        </td>
                                                        <td colspan="1" style="background-color: lightcyan;">
                                                            <asp:TextBox Enabled="false" Width="75px" ID="Txt_gv_SSN" runat="server" Text='<%# Eval("Employee_SSN_Hidden") %>'></asp:TextBox>
                                                        </td>
                                                        <td colspan="2" style="background-color: lightcyan;">
                                                            <asp:TextBox Enabled="false" Width="150px" ID="Txt_gv_Address" runat="server" Text='<%# Eval("EMPLOYEE_ADDRESS") %>'></asp:TextBox>
                                                        </td>
                                                        <td colspan="2" style="background-color: lightcyan;">
                                                            <asp:TextBox Enabled="false" ID="Txt_gv_City" runat="server" Width="150px" Text='<%# Eval("EMPLOYEE_CITY") %>'></asp:TextBox>
                                                        </td>
                                                        <td colspan="1" style="background-color: lightcyan;">
                                                            <asp:HiddenField ID="Hf_state_abrev" runat="server" Value='<%# Eval("StateAbbreviation") %>' />
                                                            <asp:DropDownList Width="75px" Enabled="false" ID="Ddl_gv_State" runat="server"></asp:DropDownList>
                                                        </td>
                                                        <td colspan="1" style="background-color: lightcyan;">
                                                            <asp:TextBox Enabled="false" ID="Txt_gv_Zip" Width="75px" runat="server" Text='<%# Eval("EMPLOYEE_ZIP") %>'></asp:TextBox>
                                                        </td>
                                                        <td colspan="1" style="background-color: lightcyan;">
                                                            <asp:Literal ID="Literal2" runat="server" Text='<%# Eval("EMPLOYEE_HIRE_DATE", "{0:MM-dd-yyyy}") %>'></asp:Literal>
                                                        </td>
                                                        <td colspan="1" style="background-color: lightcyan;">
                                                            <asp:Literal ID="Literal1" runat="server" Text='<%# Eval("EMPLOYEE_TERM_DATE", "{0:MM-dd-yyyy}") %>'></asp:Literal>
                                                        </td>
                                                        <td colspan="1" style="width: 75px; background-color: lightcyan;">&nbsp</td>
                                                    </tr>

                                                    <tr style="font-weight: bold; background-color: lightgray;">
                                                        <td style="width: 75px;">PART II</td>
                                                        <td style="width: 75px;">January</td>
                                                        <td style="width: 75px;">February</td>
                                                        <td style="width: 75px;">March</td>
                                                        <td style="width: 75px;">April</td>
                                                        <td style="width: 75px;">May</td>
                                                        <td style="width: 75px;">June</td>
                                                        <td style="width: 75px;">July</td>
                                                        <td style="width: 75px;">August</td>
                                                        <td style="width: 75px;">September</td>
                                                        <td style="width: 75px;">October</td>
                                                        <td style="width: 75px;">November</td>
                                                        <td style="width: 75px;">December</td>
                                                    </tr>
                                                    <tr style="font-weight: bold;">
                                                        <td style="width: 75px;">Line 14</td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_ooc_jan" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_ooc_feb" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_ooc_mar" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_ooc_apr" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_ooc_may" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_ooc_jun" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_ooc_jul" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_ooc_aug" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_ooc_sep" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_ooc_oct" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_ooc_nov" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_ooc_dec" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                    </tr>
                                                    <tr style="font-weight: bold;">
                                                        <td style="width: 75px;">Line 15</td>
                                                        <td style="width: 75px;">
                                                            <asp:TextBox ID="Txt_gv_lcmp_jan" runat="server" CssClass="txtSmall" Enabled="false"></asp:TextBox></td>
                                                        <td style="width: 75px;">
                                                            <asp:TextBox ID="Txt_gv_lcmp_feb" runat="server" CssClass="txtSmall" Enabled="false"></asp:TextBox></td>
                                                        <td style="width: 75px;">
                                                            <asp:TextBox ID="Txt_gv_lcmp_mar" runat="server" CssClass="txtSmall" Enabled="false"></asp:TextBox></td>
                                                        <td style="width: 75px;">
                                                            <asp:TextBox ID="Txt_gv_lcmp_apr" runat="server" CssClass="txtSmall" Enabled="false"></asp:TextBox></td>
                                                        <td style="width: 75px;">
                                                            <asp:TextBox ID="Txt_gv_lcmp_may" runat="server" CssClass="txtSmall" Enabled="false"></asp:TextBox></td>
                                                        <td style="width: 75px;">
                                                            <asp:TextBox ID="Txt_gv_lcmp_jun" runat="server" CssClass="txtSmall" Enabled="false"></asp:TextBox></td>
                                                        <td style="width: 75px;">
                                                            <asp:TextBox ID="Txt_gv_lcmp_jul" runat="server" CssClass="txtSmall" Enabled="false"></asp:TextBox></td>
                                                        <td style="width: 75px;">
                                                            <asp:TextBox ID="Txt_gv_lcmp_aug" runat="server" CssClass="txtSmall" Enabled="false"></asp:TextBox></td>
                                                        <td style="width: 75px;">
                                                            <asp:TextBox ID="Txt_gv_lcmp_sep" runat="server" CssClass="txtSmall" Enabled="false"></asp:TextBox></td>
                                                        <td style="width: 75px;">
                                                            <asp:TextBox ID="Txt_gv_lcmp_oct" runat="server" CssClass="txtSmall" Enabled="false"></asp:TextBox></td>
                                                        <td style="width: 75px;">
                                                            <asp:TextBox ID="Txt_gv_lcmp_nov" runat="server" CssClass="txtSmall" Enabled="false"></asp:TextBox></td>
                                                        <td style="width: 75px;">
                                                            <asp:TextBox ID="Txt_gv_lcmp_dec" runat="server" CssClass="txtSmall" Enabled="false"></asp:TextBox></td>
                                                    </tr>
                                                    <tr style="font-weight: bold;">
                                                        <td>Line 16</td>
                                                        <td>
                                                            <asp:DropDownList ID="Ddl_gv_ash_jan" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td>
                                                            <asp:DropDownList ID="Ddl_gv_ash_feb" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td>
                                                            <asp:DropDownList ID="Ddl_gv_ash_mar" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td>
                                                            <asp:DropDownList ID="Ddl_gv_ash_apr" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td>
                                                            <asp:DropDownList ID="Ddl_gv_ash_may" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td>
                                                            <asp:DropDownList ID="Ddl_gv_ash_jun" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td>
                                                            <asp:DropDownList ID="Ddl_gv_ash_jul" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td>
                                                            <asp:DropDownList ID="Ddl_gv_ash_aug" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td>
                                                            <asp:DropDownList ID="Ddl_gv_ash_sep" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td>
                                                            <asp:DropDownList ID="Ddl_gv_ash_oct" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td>
                                                            <asp:DropDownList ID="Ddl_gv_ash_nov" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td>
                                                            <asp:DropDownList ID="Ddl_gv_ash_dec" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                    </tr>
                                                    <tr style="font-weight: bold;">
                                                        <td style="width: 75px;">Ins. Type</td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_ins_type_jan" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_ins_type_feb" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_ins_type_mar" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_ins_type_apr" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_ins_type_may" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_ins_type_jun" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_ins_type_jul" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_ins_type_aug" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_ins_type_sep" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_ins_type_oct" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_ins_type_nov" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_ins_type_dec" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                    </tr>
                                                    <tr style="font-weight: bold;">
                                                        <td style="width: 75px;">Status</td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_status_jan" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_status_feb" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_status_mar" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_status_apr" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_status_may" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_status_jun" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_status_jul" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_status_aug" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_status_sep" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_status_oct" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_status_nov" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                        <td style="width: 75px;">
                                                            <asp:DropDownList ID="Ddl_gv_status_dec" runat="server" CssClass="ddlSmall" Enabled="false"></asp:DropDownList></td>
                                                    </tr>
                                                    <tr style="font-weight: bold;">
                                                        <td style="width: 75px;">Enrolled</td>
                                                        <td style="width: 75px;">
                                                            <asp:CheckBox ID="Cb_gv_enrolled_jan" runat="server" Enabled="false" /></td>
                                                        <td style="width: 75px;">
                                                            <asp:CheckBox ID="Cb_gv_enrolled_feb" runat="server" Enabled="false" /></td>
                                                        <td style="width: 75px;">
                                                            <asp:CheckBox ID="Cb_gv_enrolled_mar" runat="server" Enabled="false" /></td>
                                                        <td style="width: 75px;">
                                                            <asp:CheckBox ID="Cb_gv_enrolled_apr" runat="server" Enabled="false" /></td>
                                                        <td style="width: 75px;">
                                                            <asp:CheckBox ID="Cb_gv_enrolled_may" runat="server" Enabled="false" /></td>
                                                        <td style="width: 75px;">
                                                            <asp:CheckBox ID="Cb_gv_enrolled_jun" runat="server" Enabled="false" /></td>
                                                        <td style="width: 75px;">
                                                            <asp:CheckBox ID="Cb_gv_enrolled_jul" runat="server" Enabled="false" /></td>
                                                        <td style="width: 75px;">
                                                            <asp:CheckBox ID="Cb_gv_enrolled_aug" runat="server" Enabled="false" /></td>
                                                        <td style="width: 75px;">
                                                            <asp:CheckBox ID="Cb_gv_enrolled_sep" runat="server" Enabled="false" /></td>
                                                        <td style="width: 75px;">
                                                            <asp:CheckBox ID="Cb_gv_enrolled_oct" runat="server" Enabled="false" /></td>
                                                        <td style="width: 75px;">
                                                            <asp:CheckBox ID="Cb_gv_enrolled_nov" runat="server" Enabled="false" /></td>
                                                        <td style="width: 75px;">
                                                            <asp:CheckBox ID="Cb_gv_enrolled_dec" runat="server" Enabled="false" /></td>
                                                    </tr>
                                                    <tr style="font-weight: bold;">
                                                        <td style="width: 75px;">MEC</td>
                                                        <td style="width: 75px;">
                                                            <asp:CheckBox ID="Cb_gv_mec_jan" runat="server" Enabled="false" /></td>
                                                        <td style="width: 75px;">
                                                            <asp:CheckBox ID="Cb_gv_mec_feb" runat="server" Enabled="false" /></td>
                                                        <td style="width: 75px;">
                                                            <asp:CheckBox ID="Cb_gv_mec_mar" runat="server" Enabled="false" /></td>
                                                        <td style="width: 75px;">
                                                            <asp:CheckBox ID="Cb_gv_mec_apr" runat="server" Enabled="false" /></td>
                                                        <td style="width: 75px;">
                                                            <asp:CheckBox ID="Cb_gv_mec_may" runat="server" Enabled="false" /></td>
                                                        <td style="width: 75px;">
                                                            <asp:CheckBox ID="Cb_gv_mec_jun" runat="server" Enabled="false" /></td>
                                                        <td style="width: 75px;">
                                                            <asp:CheckBox ID="Cb_gv_mec_jul" runat="server" Enabled="false" /></td>
                                                        <td style="width: 75px;">
                                                            <asp:CheckBox ID="Cb_gv_mec_aug" runat="server" Enabled="false" /></td>
                                                        <td style="width: 75px;">
                                                            <asp:CheckBox ID="Cb_gv_mec_sep" runat="server" Enabled="false" /></td>
                                                        <td style="width: 75px;">
                                                            <asp:CheckBox ID="Cb_gv_mec_oct" runat="server" Enabled="false" /></td>
                                                        <td style="width: 75px;">
                                                            <asp:CheckBox ID="Cb_gv_mec_nov" runat="server" Enabled="false" /></td>
                                                        <td style="width: 75px;">
                                                            <asp:CheckBox ID="Cb_gv_mec_dec" runat="server" Enabled="false" /></td>
                                                    </tr>
                                                    <tr style="font-weight: bold;">
                                                        <td>Monthly Hours</td>
                                                        <td>
                                                            <asp:Literal ID="Lit_gv_hours_jan" runat="server"></asp:Literal></td>
                                                        <td>
                                                            <asp:Literal ID="Lit_gv_hours_feb" runat="server"></asp:Literal></td>
                                                        <td>
                                                            <asp:Literal ID="Lit_gv_hours_mar" runat="server"></asp:Literal></td>
                                                        <td>
                                                            <asp:Literal ID="Lit_gv_hours_apr" runat="server"></asp:Literal></td>
                                                        <td>
                                                            <asp:Literal ID="Lit_gv_hours_may" runat="server"></asp:Literal></td>
                                                        <td>
                                                            <asp:Literal ID="Lit_gv_hours_jun" runat="server"></asp:Literal></td>
                                                        <td>
                                                            <asp:Literal ID="Lit_gv_hours_jul" runat="server"></asp:Literal></td>
                                                        <td>
                                                            <asp:Literal ID="Lit_gv_hours_aug" runat="server"></asp:Literal></td>
                                                        <td>
                                                            <asp:Literal ID="Lit_gv_hours_sep" runat="server"></asp:Literal></td>
                                                        <td>
                                                            <asp:Literal ID="Lit_gv_hours_oct" runat="server"></asp:Literal></td>
                                                        <td>
                                                            <asp:Literal ID="Lit_gv_hours_nov" runat="server"></asp:Literal></td>
                                                        <td>
                                                            <asp:Literal ID="Lit_gv_hours_dec" runat="server"></asp:Literal></td>
                                                    </tr>
                                                    <tr style="font-weight: bold; background-color: lightgray;">
                                                        <td>
                                                            <asp:ImageButton ID="ImgBtnAdd1095" runat="server" ImageUrl="~/images/addbutton.png" Width="12px" CommandName="PartIII" Height="12px" ToolTip="ADD Part III Row" Visible="false" />
                                                            PART III
                                                       
                                                            <asp:ImageButton ID="ImgBtnAddDependent" runat="server" ImageUrl="~/images/addbuttonGreen.png" Width="12px" CommandName="Dependent" Height="12px" ToolTip="ADD Dependent" Visible="false" />
                                                        </td>
                                                        <td>Jan</td>
                                                        <td>Feb</td>
                                                        <td>Mar</td>
                                                        <td>Apr</td>
                                                        <td>May</td>
                                                        <td>Jun</td>
                                                        <td>Jul</td>
                                                        <td>Aug</td>
                                                        <td>Sep</td>
                                                        <td>Oct</td>
                                                        <td>Nov</td>
                                                        <td>Dec</td>
                                                    </tr>

                                                    <asp:Repeater ID="RptDependents" runat="server" OnItemCommand="RptDependents_ItemCommand">
                                                        <ItemTemplate>
                                                            <tr style="font-weight: bold;">
                                                                <td style="width: 75px;">
                                                                    <asp:ImageButton ID="ImgBtnEdit1095" runat="server" ImageUrl="~/images/edit_notes.png" Width="20px" CommandName="Edit" ToolTip="EDIT Dependent data" />
                                                                    <asp:ImageButton ID="ImgBtnDelete1095" runat="server" ImageUrl="~/images/close_box_red.png" Width="17px" CommandName="Delete" ToolTip="Delete Dependent" />
                                                                    <asp:ConfirmButtonExtender ID="CbeDelete" runat="server" TargetControlID="ImgBtnDelete1095" ConfirmText="Are you sure you want to DELETE this record?"></asp:ConfirmButtonExtender>
                                                                    <asp:ImageButton ID="ImgBtnSave1095" runat="server" ImageUrl="~/images/disk-save.png" Width="20px" CommandName="Update" ToolTip="SAVE Dependent data" Visible="false" />
                                                                    <asp:ImageButton ID="ImgBtnCancel" runat="server" ImageUrl="~/images/back-icon.png" Width="20px" CommandName="Cancel" ToolTip="CANCEL EDITING" Visible="false" />
                                                                    <asp:HiddenField ID="Hf_rpt_EmployeeID" runat="server" Value='<%# Eval("AIC_EMPLOYEE_ID") %>' />
                                                                </td>
                                                                <td colspan="3" style="background-color: lightblue;">First:<asp:TextBox ID="Txt_dep_Fname" runat="server" Enabled="false" Text='<%# Eval("AIC_FIRST_NAME") %>'></asp:TextBox></td>
                                                                <td colspan="3" style="background-color: lightblue;">Last:<asp:TextBox ID="Txt_dep_Lname" runat="server" Enabled="false" Text='<%# Eval("AIC_LAST_NAME") %>'></asp:TextBox></td>
                                                                <td colspan="3" style="background-color: lightblue;">SSN:<asp:TextBox ID="Txt_dep_ssn" runat="server" Enabled="false" Text='<%# Eval("AIC_SSN") %>'></asp:TextBox></td>

                                                                <td colspan="4" style="background-color: lightblue;">DOB:<asp:TextBox ID="Txt_dep_dob" runat="server" Enabled="false" Text='<%# Eval("AIC_DOB", "{0:MM-dd-yyyy}") %>'></asp:TextBox><asp:HiddenField ID="Hf_dep_rowID" runat="server" Value='<%# Eval("AIC_COVERED_IND_ID") %>' />

                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Enrolled</td>
                                                                <td>
                                                                    <asp:CheckBox ID="cb_jan" runat="server" Enabled="false" Checked='<%# Eval("AIC_JAN") %>' /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="cb_feb" runat="server" Enabled="false" Checked='<%# Eval("AIC_FEB") %>' /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="cb_mar" runat="server" Enabled="false" Checked='<%# Eval("AIC_MAR") %>' /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="cb_apr" runat="server" Enabled="false" Checked='<%# Eval("AIC_APR") %>' /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="cb_may" runat="server" Enabled="false" Checked='<%# Eval("AIC_MAY") %>' /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="cb_jun" runat="server" Enabled="false" Checked='<%# Eval("AIC_JUN") %>' /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="cb_jul" runat="server" Enabled="false" Checked='<%# Eval("AIC_JUL") %>' /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="cb_aug" runat="server" Enabled="false" Checked='<%# Eval("AIC_AUG") %>' /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="cb_sep" runat="server" Enabled="false" Checked='<%# Eval("AIC_SEP") %>' /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="cb_oct" runat="server" Enabled="false" Checked='<%# Eval("AIC_OCT") %>' /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="cb_nov" runat="server" Enabled="false" Checked='<%# Eval("AIC_NOV") %>' /></td>
                                                                <td>
                                                                    <asp:CheckBox ID="cb_dec" runat="server" Enabled="false" Checked='<%# Eval("AIC_DEC") %>' /></td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:HiddenField ID="HfDummyTrigger" runat="server" />
                        <asp:HiddenField ID="HfDummyTrigger2" runat="server" />
                        <asp:HiddenField ID="HfDummyTrigger3" runat="server" />
                        <asp:HiddenField ID="HfDummyTrigger4" runat="server" />
                        <br />
                        <h3>DISCLAIMER:</h3>
                        <p style="font-weight: bold">
                            AS AGREED UPON IN THE SERVICE AGREEMENT AS AN EMPLOYER YOU ARE ULTIMATELY RESPONSIBLE FOR THE FINAL REVIEW AND APPROVAL OF THE DATA COLLECTED VIA THE SYSTEM. <%= Branding.CompanyShortName %> IS ENTITLED TO RELY UPON THE ACCURACY AND COMPLETENESS OF INFORMATION PROVIDED TO <%= Branding.CompanyShortName %> BY THE EMPLOYER, OR ON BEHALF OF EMPLOYER, REGARDLESS OF THE FORM OF THE INFORMATION (E.G., ORAL, WRITTEN, ELECTRONIC, ETC.).  <%= Branding.CompanyShortName %> IS NOT RESPONSIBLE FOR NEGATIVE CONSEQUENCES RESULTING FROM INACCURATE, INCOMPLETE, OR VOLUNTARY OVERRIDES, ETC. INFORMATION PROVIDED TO <%= Branding.CompanyShortName %> BY THE EMPLOYER, OR ON BEHALF OF EMPLOYER.  
                        </p>
                        <p style="font-weight: bold">
                            AS AN AUTHORIZED AGENT OF THE EMPLOYER, I ATTEST THAT I HAVE REVIEWED AND I APPROVE THE SUBMISSION OF THIS DATA FOR 1095 PRODUCTION AND SUBMISSION TO THE IRS.              
                        </p>
                        <br />
                        <asp:Button ID="BtnSave" runat="server" Text="Finalize 1095C Forms" Width="95%" OnClick="BtnSave_Click" />
                        <asp:ConfirmButtonExtender ID="CbeSave" runat="server" ConfirmText="Are you sure you want to finalize the checked records?" TargetControlID="BtnSave"></asp:ConfirmButtonExtender>
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
                        <asp:Panel ID="PnlAll12" runat="server">
                            <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                            </div>
                            <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white; z-index: 10">
                                <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                    <asp:ImageButton ID="ImgBtnClose4" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" BorderStyle="None" />
                                </div>
                                <h3 style="color: black;">Correction Exception</h3>
                                <p>
                                    *Please provide a Justification for not correcting this employee in the textbox below. 
                   
                                </p>
                                Name: 
                   
                                <asp:Literal ID="Lit_all12_Name" runat="server"></asp:Literal>
                                <br />
                                ID:
                   
                                <asp:Literal ID="lit_all12_EmployeeID" runat="server"></asp:Literal>
                                <br />

                                <asp:TextBox ID="tbJustification" MaxLength="2048" TextMode="MultiLine" runat="server" Width="400px" Height="200px"></asp:TextBox>

                                <br />
                                <br />
                                <asp:Label ID="Label10" runat="server" CssClass="lbl4"></asp:Label>
                                <asp:Button ID="Btn_all12_save" runat="server" Text="Update" CssClass="btn" OnClick="Btn_all12_save_Click" />
                                <br />
                                <p style="color: darkgray">
                                    <asp:Literal ID="Lit_all12_message" runat="server"></asp:Literal>
                                </p>
                                <br />
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="PnlPartIII" runat="server">
                            <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                            </div>
                            <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white; z-index: 10">
                                <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                    <asp:ImageButton ID="ImgBtnClose2" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" BorderStyle="None" />
                                </div>
                                <h3 style="color: black;">Line III - New Entry</h3>
                                Employee From Part 1:
                   
                                <asp:Literal ID="Lit_E_EmployeeName" runat="server"></asp:Literal>
                                <br />
                                Select what type of individual you need to add to Part III.
                   
                                <br />
                                <asp:RadioButton ID="Rb_E_employee" runat="server" Text="Employee from Part I" AutoPostBack="true" Checked="true" OnCheckedChanged="Rb_E_employee_CheckedChanged" GroupName="employee" />
                                <br />
                                <asp:RadioButton ID="Rb_E_dependent" runat="server" Text="Dependent for Employee from Part I" AutoPostBack="true" OnCheckedChanged="Rb_E_dependent_CheckedChanged" GroupName="employee" />
                                <br />
                                <br />
                                <asp:Label ID="Label1" runat="server" CssClass="lbl3">Employee Name: </asp:Label>
                                <asp:TextBox ID="Txt_E_Name" runat="server" CssClass="txtLong" Enabled="false"></asp:TextBox>
                                <br />
                                <asp:Label ID="Lbl6" runat="server" CssClass="lbl3">Employee ID: </asp:Label>
                                <asp:TextBox ID="Txt_E_EmployeeID" runat="server" CssClass="txtLong" Enabled="false"></asp:TextBox>
                                <br />
                                <asp:Label ID="Lbl8" runat="server" CssClass="lbl3">Tax Year</asp:Label>
                                <asp:TextBox ID="Txt_E_TaxYear" runat="server" CssClass="txtLong" Enabled="false"></asp:TextBox>
                                <br />
                                <asp:Panel ID="Pnl_E_Dependent" runat="server" Visible="false">
                                    <asp:Label ID="Lbl7" runat="server" CssClass="lbl3">Dependent: </asp:Label>
                                    <asp:DropDownList ID="Ddl_E_dependent" runat="server" CssClass="ddl2" Enabled="false"></asp:DropDownList>
                                    <br />
                                    Note: If you need to add a dependent, click the GREEN + button in LINE III on the right hand side. 
                       
                                    <br />
                                </asp:Panel>
                                <h3>Covered Months</h3>
                                <table style="width: 600px;">
                                    <tr>
                                        <td>All 12</td>
                                        <td>Jan</td>
                                        <td>Feb</td>
                                        <td>Mar</td>
                                        <td>Apr</td>
                                        <td>May</td>
                                        <td>Jun</td>
                                        <td>Jul</td>
                                        <td>Aug</td>
                                        <td>Sept</td>
                                        <td>Oct</td>
                                        <td>Nov</td>
                                        <td>Dec</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="Cb_E_All12" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="cb_E_Jan" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="cb_E_Feb" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="cb_E_Mar" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="cb_E_Apr" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="cb_E_May" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="cb_E_Jun" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="cb_E_Jul" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="cb_E_Aug" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="cb_E_Sept" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="cb_E_Oct" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="cb_E_Nov" runat="server" /></td>
                                        <td>
                                            <asp:CheckBox ID="cb_E_Dec" runat="server" /></td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                                <asp:Label ID="Label3" runat="server" CssClass="lbl3"></asp:Label>
                                <asp:Button ID="Btn_E_Save" runat="server" Text="Save" OnClick="Btn_E_Save_Click" />
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="PnlDependent" runat="server">
                            <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                            </div>
                            <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white; z-index: 10">
                                <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                    <asp:ImageButton ID="ImgBtnClose3" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" BorderStyle="None" />
                                </div>
                                <h3 style="color: black;">Add Dependent</h3>
                                <asp:Label ID="Label2" runat="server" CssClass="lbl3">Employee Name: </asp:Label>
                                <asp:TextBox ID="Txt_D_EmployeeName" runat="server" CssClass="txtLong" Enabled="false"></asp:TextBox>
                                <br />
                                <asp:Label ID="Label4" runat="server" CssClass="lbl3">ID: </asp:Label>
                                <asp:TextBox ID="Txt_D_EmployeeID" runat="server" CssClass="txtLong" Enabled="false"></asp:TextBox>
                                <br />
                                <asp:Label ID="Lbl2" runat="server" CssClass="lbl3">First Name: </asp:Label>
                                <asp:TextBox ID="Txt_D_FirstName" runat="server" CssClass="txt"></asp:TextBox>
                                <br />
                                <asp:Label ID="Lbl3" runat="server" CssClass="lbl3">Last Name: </asp:Label>
                                <asp:TextBox ID="Txt_D_LastName" runat="server" CssClass="txt"></asp:TextBox>
                                <br />
                                <asp:Label ID="Lbl4" runat="server" CssClass="lbl3">SSN: </asp:Label>
                                <asp:TextBox ID="Txt_D_SSN" runat="server" CssClass="txt"></asp:TextBox>
                                <br />

                                <asp:Label ID="Lbl5" runat="server" CssClass="lbl3">DOB: </asp:Label>
                                <asp:TextBox ID="Txt_D_DOB" runat="server" CssClass="txt"></asp:TextBox>
                                <br />
                                <br />
                                <asp:Button ID="Btn_D_Save" runat="server" Text="Save" OnClick="Btn_D_Save_Click" />
                            </div>
                        </asp:Panel>
                        <asp:ModalPopupExtender ID="MpeAll12" runat="server" TargetControlID="HfDummyTrigger4" OkControlID="ImgBtnClose4" PopupControlID="PnlAll12"></asp:ModalPopupExtender>
                        <asp:ModalPopupExtender ID="MpeDependent" runat="server" TargetControlID="HfDummyTrigger3" OkControlID="ImgBtnClose3" PopupControlID="PnlDependent"></asp:ModalPopupExtender>
                        <asp:ModalPopupExtender ID="MpePartIII" runat="server" TargetControlID="HfDummyTrigger2" OkControlID="ImgBtnClose2" PopupControlID="PnlPartIII"></asp:ModalPopupExtender>
                        <asp:ModalPopupExtender ID="MpeWebMessage" runat="server" TargetControlID="HfDummyTrigger" OkControlID="ImgBtnClose" PopupControlID="PnlMessage"></asp:ModalPopupExtender>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpEmployee" DynamicLayout="true" DisplayAfter="500">
                    <ProgressTemplate>
                        <div style="position: fixed; top: 0; left: 0; background-color: white; width: 100%; height: 100%; opacity: .85; filter: alpha(opacity=85); -moz-opacity: 0.85; text-align: center;">
                            <div style="position: relative; margin-left: auto; margin-right: auto; background-color: white; padding-top: 100px;">
                                <h4>....Working.....</h4>
                                <asp:Image ID="ImgSearching" runat="server" ImageUrl="~/design/icon-loading-animated.gif" />
                            </div>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
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
