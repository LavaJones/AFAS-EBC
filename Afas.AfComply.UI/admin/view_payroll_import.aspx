<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="view_payroll_import.aspx.cs" Inherits="admin_view_payroll_import" %>
<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html>
<head>
    <title>View Payroll Import</title>
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
                    <h4>View Payroll Errors</h4>

                    <label class="lbl3">Select Employer</label>
                    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" AutoPostBack="true" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged"></asp:DropDownList>
                </div>
                <div id="tbright">
                    <h4>Export Data</h4>
                    <asp:Button ID="BtnExport" runat="server" Text="Export Errors" CssClass="btn" OnClick="BtnExport_Click" />
                    <asp:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" TargetControlID="BtnExport" ConfirmText="Please delete this file once you have downloaded it."></asp:ConfirmButtonExtender>
                    <br />
                    <br />
                    <h4>Delete Payroll Alerts</h4>
                    <p>
                        Note: Once these records are deleted, they are NOT recoverable.
       
                    </p>
                    <asp:Button ID="BtnDeletePayrollAlerts" runat="server" CssClass="btn" Text="Delete Payroll Alerts" OnClick="BtnDeletePayrollAlerts_Click" />
                    <asp:ConfirmButtonExtender ID="CbeDeleteAlerts" runat="server" TargetControlID="BtnDeletePayrollAlerts" ConfirmText="This will DELETE all payroll errors for this Employer."></asp:ConfirmButtonExtender>
                </div>
            </div>

            <br />
            <br />
            <h3>Exported Files</h3>
            <asp:GridView ID="GvCurrentFiles" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" Width="1024px" AutoGenerateColumns="false" OnRowDeleting="GvCurrentFiles_RowDeleting" OnRowUpdating="GvCurrentFiles_RowUpdating">
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
                    <asp:TemplateField HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImgBtnDelete" runat="server" ImageUrl="~/images/close_box_red.png" Height="20px" CommandName="Delete" ToolTip="Click to DELETE file." />
                            <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="ImgBtnDelete" ConfirmText="Are you sure you want to DELETE this file?"></asp:ConfirmButtonExtender>
                            <asp:ImageButton ID="ImgBtnDownload" runat="server" ImageUrl="~/design/Download.png" Height="20px" CommandName="Update" ToolTip="Click to download file." />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="File Name" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="LblFileName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                            <asp:HiddenField ID="HfFilePath" runat="server" Value='<%# Eval("FullName") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="File Size" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="LblFileSize" runat="server" Text='<%# Eval("Length") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="File Created On" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="LblFileCreatedOn" runat="server" Text='<%# Eval("CreationTimeUtc") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <br />
            <h3>Data that has been imported</h3>
            <i>You are viewing page
       
                <%=GvPayrollData.PageIndex + 1%>
        of
       
                <%=GvPayrollData.PageCount%>
    </i>
            <asp:GridView ID="GvPayrollData" runat="server" AutoGenerateColumns="False" AllowPaging="True" PageSize="1000" AllowSorting="True" OnPageIndexChanging="GvPayrollData_PageIndexChanging" OnSorting="GvPayrollData_Sorting" CellPadding="4" ForeColor="#333333" GridLines="None">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="PAY_BATCH_ID" HeaderText="Batch" SortExpression="PAY_BATCH_ID" HeaderStyle-Width="50px">
                        <HeaderStyle Width="50px"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="PAY_F_NAME" HeaderText="First Name" SortExpression="PAY_F_NAME" HeaderStyle-Width="100px">
                        <HeaderStyle Width="100px"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="PAY_M_NAME" HeaderText="Middle Name" SortExpression="PAY_M_NAME" HeaderStyle-Width="75px">
                        <HeaderStyle Width="75px"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="PAY_L_NAME" HeaderText="Last Name" SortExpression="PAY_L_NAME" HeaderStyle-Width="125px">
                        <HeaderStyle Width="125px"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="PAY_I_HOURS" HeaderText="Hours" SortExpression="PAY_I_HOURS" HeaderStyle-Width="75px">
                        <HeaderStyle Width="75px"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="PAY_I_SDATE" HeaderText="Start Date" SortExpression="PAY_I_SDATE" HeaderStyle-Width="75px">
                        <HeaderStyle Width="75px"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="PAY_I_EDATE" HeaderText="End Date" SortExpression="PAY_I_EDATE" HeaderStyle-Width="75px">
                        <HeaderStyle Width="75px"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="PAY_GP_DESC" HeaderText="Pay Description" SortExpression="PAY_GP_DESC" HeaderStyle-Width="150px">
                        <HeaderStyle Width="150px"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="PAY_GP_EXT_ID" HeaderText="Pay Code" SortExpression="PAY_GP_EXT_ID" HeaderStyle-Width="75px">
                        <HeaderStyle Width="75px"></HeaderStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="PAY_I_CDATE" HeaderText="Check Date" SortExpression="PAY_I_CDATE" HeaderStyle-Width="75px">
                        <HeaderStyle Width="75px"></HeaderStyle>
                    </asp:BoundField>
                </Columns>
                <EditRowStyle BackColor="#2461BF" />
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <PagerSettings FirstPageImageUrl="~/design/first.png" LastPageImageUrl="~/design/last.png" Mode="NextPreviousFirstLast" NextPageImageUrl="~/design/next.png" PageButtonCount="25" Position="TopAndBottom" PreviousPageImageUrl="~/design/prev.png" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#EFF3FB" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                <SortedDescendingHeaderStyle BackColor="#4870BE" />
            </asp:GridView>
        </div>

        <div style="clear: both;">&nbsp;</div>
        <div id="footer">
            Copyright &copy; <%= Branding.CopyrightYears %> <a href="<%= Branding.CompanyWebSite %>"><%= Branding.CompanyName %></a> - All Rights Reserved   
            <br />
            <div style="clear: both;">&nbsp;</div>
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
