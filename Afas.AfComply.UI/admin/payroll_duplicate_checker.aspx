<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="payroll_duplicate_checker.aspx.cs" Inherits="admin_payroll_duplicate_checker" %>
<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>Payroll Checker</title>
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
                    <h4>Duplicate Payroll Errors</h4>

                    <label class="lbl3">Select Employer</label>
                    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" AutoPostBack="true" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged"></asp:DropDownList>
                    <br />
                    <br />
                    <asp:Button ID="BtnValidateRecords" runat="server" Text="Delete duplicate values" OnClick="BtnValidateRecords_Click" />
                    <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="BtnValidateRecords" ConfirmText="Are you sure you want to remove ALL duplicate records."></asp:ConfirmButtonExtender>
                    <br />
                    <asp:Literal ID="LitMessage" runat="server"></asp:Literal>
                </div>
                <div id="tbright">
                    <h4>Page Purpose</h4>
                    <p>
                        The purpose of this page, is to make removing duplicate payroll records easy. 
       
                    </p>
                    <p>
                        The following conditions must all be the exact same for a record to be considered a duplicate:
           
                        <br />
                        1) Gross Pay Description
           
                        <br />
                        2) Hours Reported
           
                        <br />
                        3) Employee ID
           
                        <br />
                        4) Pay Period Start Date
           
                        <br />
                        5) Pay Period End Date
           
                        <br />
                        6) Check Date
       
                    </p>
                    <h4>Instructions</h4>
                    Step 1: Select an Employer
       
                    <br />
                    Step 2: Review the data
       
                    <br />
                    Step 3: Click the DELETE DUPLICATE VALUES button. 
       
                    <br />
                    <br />
                    * Note: If any records remain after running this process, that means that there were more than 2 duplicates. Repeat the above process until all records have been removed. 
   
                </div>
            </div>

            <br />
            <br />
            <h3>Data that has been imported</h3>
            <br />
            <i>There are currently
                <asp:Literal ID="LitCount" runat="server" Text="0"></asp:Literal>
                duplicate payroll records.
            </i>
            <br />
            <i>You are viewing page
       
                <%=GvPayrollData.PageIndex + 1%>
        of
       
                <%=GvPayrollData.PageCount%>
            </i>
            <asp:GridView ID="GvPayrollData" runat="server" AutoGenerateColumns="False" AllowPaging="True" PageSize="1000" AllowSorting="True" OnPageIndexChanging="GvPayrollData_PageIndexChanging" OnSorting="GvPayrollData_Sorting" CellPadding="4" ForeColor="#333333" GridLines="None">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="ROW_ID" HeaderText="Row ID" SortExpression="ROW_ID" HeaderStyle-Width="50px">
                        <HeaderStyle Width="50px"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="PAY_EMPLOYEE_ID" HeaderText="Employee ID" SortExpression="PAY_EMPLOYEE_ID" HeaderStyle-Width="50px">
                        <HeaderStyle Width="50px"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="PAY_HOURS" HeaderText="Hours" SortExpression="PAY_HOURS" HeaderStyle-Width="75px">
                        <HeaderStyle Width="75px"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="PAY_SDATE" HeaderText="Start Date" SortExpression="PAY_SDATE" HeaderStyle-Width="75px" DataFormatString="{0:MM-dd-yyyy}" HtmlEncode="false">
                        <HeaderStyle Width="75px"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="PAY_EDATE" HeaderText="End Date" SortExpression="PAY_EDATE" HeaderStyle-Width="75px" DataFormatString="{0:MM-dd-yyyy}" HtmlEncode="false">
                        <HeaderStyle Width="75px"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="PAY_CDATE" HeaderText="Check Date" SortExpression="PAY_CDATE" HeaderStyle-Width="75px" DataFormatString="{0:MM-dd-yyyy}" HtmlEncode="false">
                        <HeaderStyle Width="75px"></HeaderStyle>
                    </asp:BoundField>
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

