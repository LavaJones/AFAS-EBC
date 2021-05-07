<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="irs_transmit_hold.aspx.cs" Inherits="irs_transmit_hold" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" type="text/css" href="../default.css" />
    <link rel="stylesheet" type="text/css" href="../menu.css" />
    <link rel="stylesheet" type="text/css" href="../v_menu.css" />
    <title>IRS Transmission</title>

            <style>
                p { font-size:14px; margin-bottom:10px;margin-left:10px;}
                .p-flush {margin-left:0;}
                .irs-ver-para > p.irs-ver-para > br {margin-bottom:10px; line-height: 2.0;}
                #Gv_SafeHarbor {background-color:rgba(255,242,1,.25)!important;}
                .irs-button { background-color:#0088B5; padding: 7px 10px; border-radius:5px; width:300px; text-align:center; margin-bottom:0; margin-top:15px;}
                .irs-button > a { color:#ffffff; text-decoration:none;}
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
                <h1>IRS Transmission</h1>
                <h2>Reporting an Issue Prior to IRS Transmission</h2>

                 <p>
                     I have the following issue(s) that must be resolved before transmitting our organization’s 1094-C and 1095-C forms to the IRS:
                     (please use the space below to describe the issue(s) that must be resolved)
                 </p>
                 <br />
                 <asp:TextBox ID="tbIssues" runat="server" Width="50%" Height="300px" TextMode="MultiLine"></asp:TextBox>
                 <br />
                  <br />
                 <asp:CheckBox ID="chbVerify" runat="server" />
                    I hereby understand that by clicking Submit below, my organization’s 1094-C and 1095-C forms will NOT be submitted to the IRS until 
                    I return to the reporting process <a href="/Reporting/Verification">Completion</a> page and use the Transmit My Forms to the IRS link to approve submission of my forms.
                    I further understand that if I do not complete this step by March 27, 2017, <%= Branding.CompanyName %>
                    will not be responsible for any late fees associated with my organization’s IRS filing.  

                  <br />
                  <br />
                 <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" />

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

    <style>
        table{
            width:100%; 
            border: 1px solid black; 
            border-collapse:collapse
        }
        table td, table td * {
            vertical-align: top;
        }
    </style>
</body>
</html>

