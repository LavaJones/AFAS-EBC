<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="contact.aspx.cs" Inherits="securepages_contact" %>
<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" type="text/css" href="/default.css" />
    <link rel="stylesheet" type="text/css" href="/menu.css" />
    <title><%= Branding.ProductName %> - <%= Branding.CompanyName %></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
        <div id="container">
            

            <div id="header" style =" height:90px">
              



                <a href="/securepages/">
                   

                    <img src="<%= Page.ResolveUrl(Branding.LogoUri) %>" style="height: 70px" alt="<%= Branding.ProductName %> Logo" class="logo" />
                </a>
                <ul id="toplinks">
                  

                  <li>Need Help? Call <%= Branding.PhoneNumber %></li>
                    <li>
                        <asp:Literal ID="LitUserName" runat="server"></asp:Literal></li>
                    <li>
                        <asp:Button ID="BtnLogout" CssClass="btn" runat="server" Text="Log Out" OnClick="BtnLogout_Click" UseSubmitBehavior="false"  /></li>

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
                <h1>Contact Information</h1>
                <ul id="promobox">
                    <li>
                        <h3>Phone</h3>
                        <br />
                        <br />
                        <br />
                        Toll-Free: 
           
                        <br />
                        <a href="#"><%= Branding.PhoneNumber %></a>
                    </li>
                    <li class="two">
                        <h3>Email</h3>
                        <br />
                        <br />
                        <br />
                        Support: 
           
                        <br />
                        Email:
           
                        <br />
                        <a style="text-decoration: underline" href="mailto:<%= Branding.EmailAddress %>"><%= Branding.EmailAddress %></a>
                        <div>For assistance with items that contain PII or PHI please contact us by phone.</div>
                    </li>
                    <li class="three">
                        <h3>US Postal</h3>
                        <br />
                        <br />
                        <br />
                        <address>
                            <%= Branding.CompanyName %>
                            <br />
                            <%= Branding.AddressAttentionLine %>
                            <br />
                            <%= Branding.AddressStreet %>
                            <br />
                            <%= Branding.AddressCityState %>
                        </address>

                    </li>
                </ul>
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
                <h4><%= Feature.HomePageExternalMessage %></h4>
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
