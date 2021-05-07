<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Employee_Insurance_Offer.aspx.cs" Inherits="admin_Employee_Insurance_Offer" %>
<%@ Import Namespace="Afas.AfComply.Domain" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>

<html>
<head>
    <title>Employee Insurance Offer</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" type="text/css" href="../default.css" />
    <link rel="stylesheet" type="text/css" href="../menu.css" />
    <link rel="stylesheet" type="text/css" href="../v_menu.css" />

</head>
<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" ></asp:ToolkitScriptManager>
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
                        <h4>View Employee Insurance Offers</h4>
                        <label class="lbl">Select Employer</label>
                        <asp:DropDownList ID="DdlFilterEmployers" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlFilterEmployers_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                         <br />
                        <label class="lbl">Select Employee</label>
                        <asp:DropDownList ID="DdlFilterEmployees" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlFilterEmployees_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>                        
            </div>
            <div id="gridbox">
                 <asp:GridView ID="gvEmployeeInsuranceOffer" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" Width="1024px" 
                     AutoGenerateColumns="false" AllowPaging="True" PageSize="10" onpageindexchanging="Gridview1_PageIndexChanging"
                     ShowHeaderWhenEmpty="true">
                 <AlternatingRowStyle BackColor="White" />
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
                    
                 <asp:TemplateField HeaderText="Insurance Description" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                      <ItemTemplate>
                          <asp:Label ID="LblInsuranceDescription" runat="server" Text='<%# Eval("InsuranceDescription") %>'></asp:Label>
                      </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Plan Year Descryption" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                      <ItemTemplate>
                          <asp:Label ID="LblPlanYearDescryption" runat="server" Text='<%# Eval("PlanYearDescryption") %>'></asp:Label>
                      </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Insurance Contribution Amount" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                      <ItemTemplate>
                         <asp:Label ID="LblInsuranceContributionAmount" runat="server" Text='<%# Eval("InsuranceContributionAmount") %>'></asp:Label>
                      </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Monthly Cost" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                      <ItemTemplate>
                         <asp:Label ID="LblMonthlyCost" runat="server" Text='<%# Eval("Monthlycost") %>'></asp:Label>
                      </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="HRA flex contribution" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                     <ItemTemplate>
                         <asp:Label ID="LblHRAFlexContribution" runat="server" Text='<%# Eval("hra_flex_contribution") %>'></asp:Label>
                     </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Offered" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                     <ItemTemplate>
                         <asp:CheckBox ID="Cb_gvOffered" runat="server" Checked='<%#Eval("offeredbool") %>' CssClass="smallCheckBox" Enabled="false" />
                        
                     </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Offered On" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                     <ItemTemplate>
                         <asp:Label ID="LblFileCreatedOn" runat="server" Text='<%# Eval("offeredDatePart") %>'></asp:Label>
                     </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Accepted" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                     <ItemTemplate>
                         <asp:CheckBox ID="Cb_gvOffered" runat="server" Checked='<%#Eval("accepted") %>' CssClass="smallCheckBox" Enabled="false" />
                        
                        </ItemTemplate>
                 </asp:TemplateField>
                 <asp:TemplateField HeaderText="Accepted On" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="LblAcceptedOn" runat="server" Text='<%# Eval("acceptedDatePart") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Effective Date" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="LblEffectiveDate" runat="server" Text='<%# Eval("effectiveDatePart") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>                   
                </Columns>
            </asp:GridView>
                    </div>
            </div>
        </form>
                        
</body>
</html>
