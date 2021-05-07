<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="view_employer_employees.aspx.cs" Inherits="admin_view_employer_employees" %>
<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html>
<head>
    <title>Employer Employees</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" type="text/css" href="../default.css" />
    <link rel="stylesheet" type="text/css" href="../menu.css" />
    <link rel="stylesheet" type="text/css" href="../v_menu.css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
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
            <div id="topbox">
                <div id="tbleft">
                    <h4>Employee Import Errors</h4>

                    <label class="lbl3">Select Employer</label>
                    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" AutoPostBack="true" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged"></asp:DropDownList>
                </div>
                <div id="tbright">
                    <h4>Delete Employee Demographic Alerts</h4>
                    <p>
                        Note: Once these records are deleted, they are NOT recoverable.
       
                    </p>
                    <asp:Button ID="BtnDeleteDemAlerts" runat="server" CssClass="btn" Text="Delete Demographic Alerts" OnClick="BtnDeleteDemAlerts_Click" />
                    <asp:ConfirmButtonExtender ID="CbeDeleteAlerts" runat="server" TargetControlID="BtnDeleteDemAlerts" ConfirmText="This will DELETE all Employee Demographic Alerts for this Employer."></asp:ConfirmButtonExtender>

                </div>
            </div>

            <br />
            <br />
            <h3>Employee Demographic Import Errors</h3>
            <i>You are viewing page
       
                <%=GvPayrollData.PageIndex + 1%>
        of
       
                <%=GvPayrollData.PageCount%>
    </i>
            <asp:GridView ID="GvPayrollData" runat="server" AutoGenerateColumns="False" AllowPaging="True" PageSize="1000" AllowSorting="True" OnPageIndexChanging="GvPayrollData_PageIndexChanging" OnSorting="GvPayrollData_Sorting" CellPadding="4" ForeColor="#333333" GridLines="None">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="EMPLOYEE_EXT_ID" HeaderText="Payroll ID" SortExpression="EMPLOYEE_EXT_ID" HeaderStyle-Width="175px" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                    <asp:BoundField DataField="EMPLOYEE_FIRST_NAME" HeaderText="First Name" SortExpression="EMPLOYEE_FIRST_NAME" HeaderStyle-Width="125px" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                    <asp:BoundField DataField="EMPLOYEE_LAST_NAME" HeaderText="Last Name" SortExpression="EMPLOYEE_LAST_NAME" HeaderStyle-Width="125px" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                    <asp:BoundField DataField="EMPLOYEE_I_HIRE_DATE" HeaderText="Hire Date" SortExpression="EMPLOYEE_I_HIRE_DATE" HeaderStyle-Width="125px" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                    <asp:BoundField DataField="EMPLOYEE_I_C_DATE" HeaderText="Change Date" HeaderStyle-Width="125px" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                    <asp:BoundField DataField="EMPLOYEE_I_TERM_DATE" HeaderText="Term. Date" SortExpression="EMPLOYEE_I_TERM_DATE" HeaderStyle-Width="125px" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                    <asp:BoundField DataField="EMPLOYEE_I_DOB" HeaderText="DOB" SortExpression="EMPLOYEE_I_DOB" HeaderStyle-Width="125px" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                    <asp:BoundField DataField="Employee_SSN_Hidden" HeaderText="SSN" SortExpression="Employee_SSN_Hidden" HeaderStyle-Width="125px" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>

                </Columns>
                <EditRowStyle BackColor="#2461BF" />
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerSettings FirstPageImageUrl="~/design/first.png" LastPageImageUrl="~/design/last.png" Mode="NextPreviousFirstLast" NextPageImageUrl="~/design/next.png" PageButtonCount="25" Position="TopAndBottom" PreviousPageImageUrl="~/design/prev.png" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#EFF3FB" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                <SortedDescendingHeaderStyle BackColor="#4870BE" />
            </asp:GridView>
        </div>

        <div style="clear: both;">&nbsp;</div>
        <div id="footer">
            Copyright &copy; <%= Branding.CopyrightYears %> <a href="<%= Branding.CompanyWebSite %>"><%= Branding.CompanyName %></a> - All Rights Reserved   
            <br />
            <div style="clear: both;">&nbsp;</div>
        </div>
        <asp:HiddenField ID="HfDummyTrigger" runat="server" />
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
