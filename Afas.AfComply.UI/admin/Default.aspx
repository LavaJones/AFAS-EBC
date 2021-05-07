<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="admin_Default" %>
<%@ Import Namespace="Afas.AfComply.Domain" %>

<!DOCTYPE html>
<html>
<head>
    <title><%= Branding.ProductName %> - <%= Branding.CompanyName %></title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" type="text/css" href="../default.css" />
    <link rel="stylesheet" type="text/css" href="../menu.css" />
    <link rel="stylesheet" type="text/css" href="../v_menu.css" />

</head>
<body>
    <form id="form1" runat="server">
        <div id="container">
            <div id="header">
                <a href="../securepages/Default.aspx">
                    <img src="<%= Page.ResolveUrl(Branding.LogoUri) %>" style="height: 70px" alt="<%= Branding.ProductName %> Logo" class="logo" />
                </a>
                <ul id="toplinks">
                    <li>Need Help? Call <%= Branding.PhoneNumber %></li>
                    <li>
                        <asp:Literal ID="LitUserName" runat="server"></asp:Literal>
                    </li>
                    <li>
                        <asp:Button ID="BtnLogout" CssClass="btn" runat="server" Text="Log Out" OnClick="BtnLogout_Click" UseSubmitBehavior="false" />
                    </li>               
                </ul>
                <asp:HiddenField ID="HfDistrictID" runat="server" />
            </div>
            <div id="nav2">
                <nav>
                    <%= demo.getAdminLinks() %>
                </nav>
            </div>
            <div id="topbox">
                <a href="import_payroll.aspx">
                    <div style="width: 225px; height: 275px; text-align: center; float: left;">
                        <img src="../images/payroll.png" alt="Link to Employer File Upload" style="width: 200px;" />
                        <br />
                        Payroll Import
       
                    </div>
                </a>
                <a href="import_employee.aspx">
                    <div style="width: 225px; height: 275px; text-align: center; float: left;">
                        <img src="../images/employees.png" alt="Link to Employer File Upload" style="width: 200px;" />
                        <br />
                        Employee Demographic Import
       
                    </div>
                </a>
                <a href="import_employee_che.aspx">
                    <div style="width: 225px; height: 275px; text-align: center; float: left;">
                        <img src="../images/employees_che.png" alt="Link to Employer File Upload in che format" style="width: 200px;" />
                        <br />
                        Employee Demographic Import Che
       
                    </div>
                </a>
                <a href="import_hrstatus.aspx">
                    <div style="width: 225px; height: 275px; text-align: center; position: relative; float: left;">
                        <img src="../images/hrstatus.png" alt="Link to Payroll Upload" style="width: 200px;" />
                        <br />
                        HR Status Import
       
                    </div>
                </a>
                <a href="import_grosspay.aspx">
                    <div style="width: 225px; height: 275px; text-align: center; position: relative; float: left;">
                        <img src="../images/paycode.png" alt="Link to Payroll Upload" style="width: 200px;" />
                        <br />
                        Pay Description Import
     
                    </div>
                </a>
                <a href="admin_float_user.aspx">
                    <div style="width: 225px; height: 275px; text-align: center; position: relative; float: left;">
                        <img src="../images/floating_user.jpg" alt="Link to Floating User" style="width: 200px;" />
                        <br />
                        Floating User
     
                    </div>
                </a>
                <a href="grosspay_merge_tool.aspx">
                    <div style="width: 225px; height: 275px; text-align: center; position: relative; float: left;">
                        <img src="../images/merge_gp.jpg" alt="Link to Gross Pay Merge Tool" style="width: 200px;" />
                        <br />
                        Merge Gross Pay Descriptions
     
                    </div>
                </a>
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

