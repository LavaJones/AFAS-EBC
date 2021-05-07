<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminAuto_py_rollover.aspx.cs" Inherits="AdminAuto_py_rollover" %>
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
            <asp:UpdatePanel ID="UpRollover" runat="server">
                <ContentTemplate>
                     <h2>Auto Measurement Period Rollover</h2>
                     <asp:Button ID="btnRollover" CssClass="btn" runat="server" Text="Roll over" OnClick="BtnRollover_Click" UseSubmitBehavior="false" /></li>
                     <asp:Label ID="LblRolloverMessage" runat="server"/></li>
                            <asp:GridView ID="GvEmployerAdminDetails" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" Width="1024px" AutoGenerateColumns="false" AllowPaging="true" PageSize="20" OnPageIndexChanging="GvEmployerAdminDetails_PageIndexChanged">
                            <AlternatingRowStyle BackColor="White" />
                            <EditRowStyle BackColor="#2461BF" />
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EFF3FB" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#F5F7FB" />
                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                            <SortedDescendingCellStyle BackColor="#E9EBEF" />
                            <SortedDescendingHeaderStyle BackColor="#4870BE" />
                            <Columns>
                                <asp:TemplateField HeaderText="Employer Name" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="LblEmprName" runat="server" Text='<%# Eval("EmployerName") %>'></asp:Label>
                                        <asp:HiddenField ID="HfEmployerId" runat="server" Value='<%# Eval("EmployerId") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Plan Year" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPlanYear" runat="server" Text='<%# Eval("PlanYear") %>'></asp:Label>
                                         <asp:HiddenField ID="HfPlanYearID" runat="server" Value='<%# Eval("PlanYearID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Admin Start Date" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="MEASUREMENT_START_END" runat="server" Text='<%# Eval("AdminStart", "{0:MM-dd-yyyy}") %>'></asp:Label>                           
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Admin End Date" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="MEASUREMENT_ADMIN_END" runat="server" Text='<%# Eval("AdminEnd", "{0:MM-dd-yyyy}") %>'></asp:Label>                           
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Stability Start Date" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="STABILITY_START" runat="server" Text='<%# Eval("StabilityStart", "{0:MM-dd-yyyy}") %>'></asp:Label>                           
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Stability End Date" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="STABILITY_END" runat="server" Text='<%# Eval("StabilityEnd", "{0:MM-dd-yyyy}") %>'></asp:Label>                           
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                 </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
