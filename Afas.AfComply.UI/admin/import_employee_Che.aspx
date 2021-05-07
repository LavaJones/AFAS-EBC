<%@ Page EnableSessionState="ReadOnly" Language="C#" AutoEventWireup="true" CodeBehind="import_employee_Che.aspx.cs" Inherits="admin_employee_import_Che" %>

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
                    <h4>Employee Import Che</h4>
                    <label class="lbl3">Select Employer</label>
                    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                    <br />
                    <asp:FileUpload ID="FuGrossPayFile" runat="server" Width="350px" />
                    <br />
                    <br />
                    <span>
                        <ul>
                            <li>The Employee Demographics must be a .dat text file</li>
                            <li>File Specifications</li>
                        </ul>
                    </span>
                    <br />
                    <br />
                    <asp:Button ID="BtnUploadFile" runat="server" CssClass="btn" Text="Submit" OnClick="BtnUploadFile_Click" />
                    <asp:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" TargetControlID="BtnUploadFile" ConfirmText="MAKE SURE THE SELECTED FILE IS FOR THE EMPLOYER THAT IS SELECTED IN THE DROP DOWN LIST. The data will end up in the wrong Employer if this is not correct."></asp:ConfirmButtonExtender>
                    <br />
                    <asp:Label ID="LblFileUploadMessage" runat="server"></asp:Label>

                </div>
            </div>
            <asp:HiddenField ID="HfDummyTrigger" runat="server" />
            <asp:Panel ID="PnlMessage" runat="server">
                <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                </div>
                <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 800px; height: auto; margin-top: 0px; font-size: 12px; background-color: white; z-index: 10">
                    <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                        <asp:ImageButton ID="ImgBtnClose" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" BorderStyle="None" />
                    </div>
                    <h3 style="color: black;">Webpage Message</h3>
                    <p style="color: darkgray">
                        <asp:Literal ID="LitMessage" runat="server"></asp:Literal>
                    </p>
                    <br />
                    <div style="width: 100%; max-height: 600px; overflow-y: auto;">

                        <asp:GridView ID="Gv_ValidationErrors" runat="server" AutoGenerateColumns="false"
                            EmptyDataText="There are currently NO Validation Errors." BackColor="White" BorderColor="#336666"
                            BorderStyle="Double" BorderWidth="3px" CellPadding="4" GridLines="Horizontal">
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
                                <asp:TemplateField HeaderText="Validation Type" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Literal ID="LitTypeName" runat="server" Text='<%# Eval("ValidationType") %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Identifier" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Literal ID="LitIdentifier" runat="server" Text='<%# Eval("Identifier") %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Validation Message" HeaderStyle-Width="400px" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Literal ID="LitMessage" runat="server" Text='<%# Eval("ValidationMessage") %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>

                </div>
            </asp:Panel>

            <asp:ModalPopupExtender ID="MpeWebMessage" runat="server" TargetControlID="HfDummyTrigger" OkControlID="ImgBtnClose" PopupControlID="PnlMessage"></asp:ModalPopupExtender>

            <br />
            <br />
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

