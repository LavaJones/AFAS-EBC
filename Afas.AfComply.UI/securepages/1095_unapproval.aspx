<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="1095_unapproval.aspx.cs" Inherits="Afas.AfComply.UI.securepages._1095_unapproval" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link rel="stylesheet" type="text/css" href="/default.css?1.4.0.100" />
    <link rel="stylesheet" type="text/css" href="/menu.css?1.4.0.100" />
    <link rel="stylesheet" type="text/css" href="/v_menu.css?1.4.0.100" />
    <title><%= Branding.ProductName %> - <%= Branding.CompanyName %></title>
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
                    <li>
                        <asp:HiddenField ID="HfEmployerTypeID" runat="server" />
                        <asp:HiddenField ID="HfUserName" runat="server" />
                    </li>
                    <li>User Name:<asp:Literal ID="LitUserName" runat="server"></asp:Literal></li>
                    <li>
                        <asp:Button ID="BtnLogout" CssClass="btn" runat="server" Text="Log Out" OnClick="BtnLogout_Click" UseSubmitBehavior="false" />
                    </li>

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
                <div id="topbox">
                    <div id="tbleft" style="padding-bottom: 20px;">
                        <h3>1095C - Unapproval</h3>
                        <p>
                            To unapproved a 1095c record, please follow these steps:
                            <br />
                            1) Search for the employee in question by using the search feature. 
                            <br />
                            2) Click the red box to the left of the employee name in question. Confirm that you wish to reopen this record for further review. 
                            <br />
                            3) You can view/edit that record on the Step 3 1095 Approval screen after you logout and back in. 
                        </p>
                    </div>
                    <div id="tbright">
                        <asp:Panel ID="PnlSearch" runat="server" DefaultButton="BtnApplyFilters">
                            <h3>Tax Year</h3>
                            <asp:DropDownList ID="DdlTaxYear" runat="server" CssClass="ddl2" AutoPostBack="true" Enabled="false">
                                <asp:ListItem Text="2016" Value="2016" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                            <h3>Search by Last Name</h3>
                            <asp:TextBox ID="TxtSearch" runat="server" CssClass="txtLong"></asp:TextBox>
                            <br />
                            <br />
                            <asp:Button ID="BtnApplyFilters" runat="server" Text="Apply Filters" CssClass="btn" OnClick="BtnApplyFilters_Click" />
                        </asp:Panel>
                    </div>
                </div>

                <div>
                    <asp:HiddenField ID="hdnField" runat="server" />

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

                    <asp:ModalPopupExtender ID="MpeWebMessage" runat="server" TargetControlID="hdnField" OkControlID="ImgBtnClose" PopupControlID="PnlMessage"></asp:ModalPopupExtender>

                </div>
                <br />
                <hr />
                <asp:GridView ID="GvNo1095c" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" AllowPaging="true" Width="1024px" AutoGenerateColumns="false" PageSize="50" OnPageIndexChanging="GvNo1095c_PageIndexChanging" OnSorting="GvNo1095c_Sorting" OnRowDeleting="GvNo1095c_RowDeleting" AllowSorting="true">
                    <AlternatingRowStyle BackColor="White" />
                    <EditRowStyle BackColor="#2461BF" />
                    <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#FF0083AF" HorizontalAlign="Center" />
                    <PagerSettings FirstPageImageUrl="~/design/first.png" Mode="NextPreviousFirstLast" LastPageImageUrl="../design/last.png" NextPageImageUrl="../design/next.png" PreviousPageImageUrl="../design/prev.png" PageButtonCount="25" Position="TopAndBottom" />
                    <RowStyle BackColor="#EFF3FB" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="25px" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgBtnDelete" runat="server" ImageUrl="~/images/close_box_red.png" Height="20px" CommandName="Delete" />
                                <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="ImgBtnDelete" ConfirmText="Are you sure you want to review this record again?"></asp:ConfirmButtonExtender>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Payroll ID" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" SortExpression="EmpID">
                            <ItemTemplate>
                                <asp:Literal ID="LitPayrollID" runat="server" Text='<%# Eval("EMPLOYEE_EXT_ID") %>'></asp:Literal>
                                <asp:HiddenField ID="HfEmployeeID" runat="server" Value='<%# Eval("EMPLOYEE_ID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" SortExpression="LastName">
                            <ItemTemplate>
                                <asp:Literal ID="LitEmpName" runat="server" Text='<%# Eval("EMPLOYEE_FULL_NAME") %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="SSN" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" SortExpression="SSN">
                            <ItemTemplate>
                                <asp:Literal ID="LitSSN" runat="server" Text='<%# Eval("Employee_SSN_Hidden") %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:HiddenField ID="HfDummyTrigger" runat="server" />
            </div>
        </div>

        <div id="footer">
            Copyright &copy; <%= Branding.CopyrightYears %> <a href="<%= Branding.CompanyWebSite %>"><%= Branding.CompanyName %></a> - All Rights Reserved
   
            <br />
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