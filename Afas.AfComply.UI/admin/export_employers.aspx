<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="export_employers.aspx.cs" Inherits="admin_export_employers" %>
<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html>
<head>
    <title>Export Employers</title>
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
                        <h4>Export Employers</h4>
                        <label class="lbl">Select Form</label>
                        <asp:DropDownList ID="DdlForm" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlFilterForms_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                        <br />
                        <label class="lbl">Select Employer</label>
                        <asp:DropDownList ID="DdlFilterEmployers" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlFilterEmployers_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                        <br />
                        <label class="lbl">Select Year</label>
                        <asp:DropDownList ID="DdlTaxYear" runat="server" CssClass="ddl2"></asp:DropDownList>
                        <br />
                        <asp:HiddenField ID="HfDummyTrigger" runat="server" />
                        <br />
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
            <br />
            <asp:Button ID="btnPrintTextFile" runat="server" Text="Print File" OnClick="btnPrintFile_Click" />
            <asp:Label ID="lblMsg" runat="server"></asp:Label>    
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
