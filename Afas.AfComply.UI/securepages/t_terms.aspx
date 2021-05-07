<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="t_terms.aspx.cs" Inherits="securepages_t_terms" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" type="text/css" href="/default.css" />
    <link rel="stylesheet" type="text/css" href="/menu.css" />
    <link rel="stylesheet" type="text/css" href="/v_menu.css" />
    <title><%= Branding.ProductName %> - <%= Branding.CompanyName %></title>
    <style type="text/css">
        .lbl {
            width: 325px;
            float: left;
            text-align: left;
            clear: left;
            padding: 5px;
            background-color: lightgray;
            height: 15px;
            font-weight: bold;
        }

        .lbl2 {
            padding: 5px;
        }

        .txt {
            padding: 5px;
            width: 135px;
            height: 15px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
        <div id="container">

            <div id="header" style="height: 90px">

                <a href="/securepages/">
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

        </div>
        <asp:UpdatePanel ID="UpReports" runat="server">
            <ContentTemplate>
                <div class="middle_ebc">
                    <h3>Set-up</h3>

                    <asp:GridView ID="GvReports" runat="server" CssClass="gridviewHeader" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" CellPadding="1" Font-Size="10px" ForeColor="#333333" GridLines="None" Width="300px" OnSelectedIndexChanged="GvReports_SelectedIndexChanged">
                        <AlternatingRowStyle BackColor="White" />
                        <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="Transparent" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EFF3FB" />
                        <SelectedRowStyle BackColor="yellow" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                        <Columns>
                            <asp:TemplateField HeaderText="Name" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:LinkButton ID="LbReportName" runat="server" Text='<%# Eval("TERM_NAME") %>' CommandName="Select"></asp:LinkButton>
                                    <asp:HiddenField ID="HfReportID" runat="server" Value='<%# Eval("TERM_DEF") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div class="right_ebc">
                    <h3>Term Definitions</h3>
                    <div style="position: absolute; top: 15px; right: 85px;">
                    </div>
                    <asp:Panel ID="PnlDefinition" runat="server">
                        <asp:Label ID="LblTerm" runat="server" CssClass="lbl"></asp:Label>
                        <br />
                        <br />
                        <br />
                        <asp:Literal ID="LitDefinition" runat="server"></asp:Literal>
                    </asp:Panel>

                </div>




            </ContentTemplate>
        </asp:UpdatePanel>



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
