<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="admin_py_rollover.aspx.cs" Inherits="admin_admin_py_rollover" %>
<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html>
<head>
    <title>Employee Import</title>
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
                    <h4>Administrative Period Rollover</h4>
                    <label class="lbl3">Employer</label>
                    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    <br />
                    <br />
                    <label class="lbl3">Plan Year moving to Stability Period</label>
                    <asp:DropDownList ID="DdlPlanYearCurrent" runat="server" CssClass="ddl2"></asp:DropDownList>
                    <br />
                    <br />
                    <asp:Button ID="BtnProcessFile" runat="server" Text="Submit" CssClass="btn" OnClick="BtnProcessFile_Click" />
                    <br />
                    <br />
                </div>
                <div id="tbright">
                    <h4>Instructions</h4>
                    <dl>
                        <dt>Step 1:</dt>
                        <dd>- Select the EMPLOYER that has had the Administrative Period Expire.</dd>
                        <dt>Step 2:</dt>
                        <dd>- Select the EMPLOYER's PLAN YEAR that this Administrative Period pertains too.</dd>
                        <dt>Step 3:</dt>
                       <dd>- Click the SUBMIT button.</dd>
                    </dl>
                </div>
            </div>
        </div>


        <asp:Panel ID="PnlRollover" runat="server">
            <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
            </div>
            <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white; z-index: 10">
                <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                    <asp:ImageButton ID="ImageButton2" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" BorderStyle="None" />
                </div>
                <h3 style="color: black;">Meaurement Rollover Message</h3>
                <p style="color: darkgray">
                    <asp:Label ID="LblRolloverMessage" runat="server" Font-Bold="true" Height="20px" Style="line-height: 20px"></asp:Label>
                </p>
                <br />
                <br />
            </div>
        </asp:Panel>
        <asp:HiddenField ID="HfDummyTrigger" runat="server" />
        <asp:ModalPopupExtender ID="MpeRolloverMessage" runat="server" TargetControlID="HfDummyTrigger" OkControlID="ImageButton2" PopupControlID="PnlRollover"></asp:ModalPopupExtender>

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
