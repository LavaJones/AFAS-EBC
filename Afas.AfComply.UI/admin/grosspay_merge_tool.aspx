<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="grosspay_merge_tool.aspx.cs" Inherits="admin_grosspay_tool" %>
<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html>
<head>
    <title>Gross Pay Merge Tool</title>
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
            <asp:UpdatePanel ID="UpGP" runat="server">
                <ContentTemplate>
                    <div id="topbox">
                        <div id="tbleft">
                            <h4>Gross Pay Merge</h4>
                            <p>
                                The dropdownlists below will show the Description and External ID in them. 
                   
                                <br />
                                Format shown below:
                   
                                <br />
                                Description - External ID
               
                            </p>
                            <br />
                            <label class="lbl3">Select Employer</label>
                            <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" AutoPostBack="true" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged"></asp:DropDownList>
                            <br />
                            <br />

                            <label class="lbl3">Gross Pay Description 1</label>
                            <asp:DropDownList ID="DdlGp1" runat="server" CssClass="ddl2"></asp:DropDownList>
                            <br />
                            *NOTE: Gross Pay Description 1 will remain in the System.
               
                            <br />
                            <label class="lbl3">Gross Pay Description 2</label>
                            <asp:DropDownList ID="DdlGp2" runat="server" CssClass="ddl2"></asp:DropDownList>
                            <br />
                            *NOTE: All Gross Pay Description 2 records will become linked to Gross Pay Description 1. Gross Pay Description 2 will be removed from the system.
               
                            <br />
                            <asp:Button ID="BtnUpdate" runat="server" Text="Update" CssClass="btn" OnClick="BtnUpdate_Click" />
                            <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="BtnUpdate" ConfirmText="Are you sure you want to Merge the two Gross Pay Descriptions? This CANNOT be undone!"></asp:ConfirmButtonExtender>
                            <br />
                            <br />
                        </div>
                        <div id="tbright">
                            <h4>Merge Gross Pay Descriptions</h4>
                            <p>
                                *Every once in a while, the school will wind up with a duplicate Gross Pay Description in the System. Generally this is due to 
                    manual manipulation of a Payroll file and a leading zero will drop of the External Gross Pay Description.  
               
                            </p>
                            <h4>Steps</h4>
                            <ul>
                                <li>Step 1: Select an EMPLOYER. </li>
                                <li>Step 2: Select Gross Pay Description 1. (The system will keep this one)</li>
                                <li>Step 3: Select Gross Pay Description 2. (The system will merge this one to appear as Gross Pay Description 1.)</li>
                            </ul>
                        </div>
                    </div>

                    <asp:HiddenField ID="HfDummyTrigger" runat="server" />
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


                    <br />
                    <br />
                    <h3>Current Gross Pay Descriptions</h3>
                    <i>You are viewing page
               
                        <%=GvGrossPayDescriptions.PageIndex + 1%>
                of
               
                        <%=GvGrossPayDescriptions.PageCount%>
            </i>
                    <br />
                    <i>Showing 
               
                        <asp:Literal ID="LitShowing" runat="server"></asp:Literal>
                        Gross Pay Descriptions of 
               
                        <asp:Literal ID="LitToal" runat="server"></asp:Literal>
                    </i>
                    <asp:GridView ID="GvGrossPayDescriptions" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" Width="1024px" AllowPaging="true" PageSize="100" AutoGenerateColumns="false" EmptyDataText="No Gross Pay Descriptions currently exist." OnPageIndexChanging="GvGrossPayDescriptions_PageIndexChanging">
                        <AlternatingRowStyle BackColor="White" />
                        <EditRowStyle BackColor="yellow" />
                        <FooterStyle BackColor="LightGray" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="LightGray" Font-Bold="True" ForeColor="black" />
                        <PagerStyle BackColor="#FF0083AF" HorizontalAlign="Center" />
                        <PagerSettings FirstPageImageUrl="~/design/first.png" Mode="NextPreviousFirstLast" LastPageImageUrl="../design/last.png" NextPageImageUrl="../design/next.png" PreviousPageImageUrl="../design/prev.png" PageButtonCount="25" Position="TopAndBottom" />
                        <RowStyle BackColor="#EFF3FB" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
                        <Columns>
                            <asp:TemplateField HeaderText="Description" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="LblFileSize" runat="server" Text='<%# Eval("GROSS_PAY_DESCRIPTION") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="External ID" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="LblFileName" runat="server" Text='<%# Eval("GROSS_PAY_EXTERNAL_ID") %>'></asp:Label>
                                    <asp:HiddenField ID="HfGpID" runat="server" Value='<%# Eval("GROSS_PAY_ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>

                    </asp:GridView>

                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpGP" DynamicLayout="true" DisplayAfter="500">
                <ProgressTemplate>
                    <div style="position: fixed; top: 0; left: 0; background-color: white; width: 100%; height: 100%; opacity: .85; filter: alpha(opacity=85); -moz-opacity: 0.85; text-align: center;">
                        <div style="position: relative; margin-left: auto; margin-right: auto; background-color: white; padding-top: 100px;">
                            <h4>Merging the two Gross Pay Descriptions..... This can take a few minutes.....</h4>
                            <asp:Image ID="ImgSearching" runat="server" ImageUrl="~/design/icon-loading-animated.gif" />
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>

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
