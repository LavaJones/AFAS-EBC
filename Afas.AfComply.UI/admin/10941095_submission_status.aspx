<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="10941095_submission_status.aspx.cs" Inherits="Afas.AfComply.UI.admin._10941095_submission_status" %>
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
            <div id="header">
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
                        <h3>Employer</h3>
                            <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        <br />

                        <h3>Tax Year</h3>
                        <p>
                            Select the year you would like to view the electronic submissions for.
                        </p>   
                        <asp:DropDownList ID="DdlYears" runat="server" CssClass="ddl" AutoPostBack="true" OnSelectedIndexChanged="DdlYears_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <div id="tbright">

                    </div>
                </div>
                <br />
                <hr />
                <table>
                <tr>
                    <td colspan="10">
                        <asp:GridView ID="GvCurrentReceipts" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" Width="1024px" ShowFooter="false" 
                            ShowHeader="false" EmptyDataText="No submissions could be found." AutoGenerateColumns="false" 
                            OnRowDataBound="GvCurrentReceipts_RowDataBound" 
                            OnRowUpdating="GvCurrentReceipts_RowEditing" >

                            <AlternatingRowStyle BackColor="White" />
                            <EditRowStyle BackColor="#2461BF" />
                            <RowStyle BackColor="#EFF3FB" />
                            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#F5F7FB" />
                            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                            <SortedDescendingCellStyle BackColor="#E9EBEF" />
                            <SortedDescendingHeaderStyle BackColor="#4870BE" />
                            <Columns>
                                <asp:TemplateField ShowHeader="false">
                                    <ItemTemplate>
                                        <tr style="background-color: lightgreen; font-weight: bold;">
                                            <td colspan="1"></td>
                                            <td colspan="2">Transmission ID</td>
                                            <td colspan="2">Transmission Type</td>
                                            <td colspan="2">Transmission Status</td>
                                            <td colspan="2">Receipt ID</td>
                                            <td colspan="2">Submission Date</td>
                                        </tr>
                                        <tr>
                                            <td colspan="1">
                                                <asp:Image ID="ImgStatus" runat="server" ImageUrl="~/images/info.jpg" ToolTip="Currently being Processed" />
                                                <asp:HiddenField ID="HfID" runat="server" Value='<%# Eval("tax_year_employer_transmissionID") %>' />
                                            </td>
                                            <td colspan="2"><asp:Label ID="LblUTID" runat="server" Text='<%# Eval("UniqueTransmissionId") %>'></asp:Label></td>
                                            <td colspan="2"><asp:Label ID="LblSubmissionType" runat="server" Text='<%# Eval("TransmissionType") %>'></asp:Label></td>
                                            <td colspan="2">
                                                <asp:Label ID="LblTransmissionStatus" runat="server" Text='<%# Eval("transmission_status_code_id") %>'></asp:Label> - 
                                                <asp:Literal ID="LitTstatus" runat="server" Text="Processing"></asp:Literal>
                                            </td>
                                            <td colspan="2"><asp:Label ID="LblReceiptID" runat="server" Text='<%# Eval("ReceiptID") %>'></asp:Label></td>
                                            <td colspan="2"><asp:Label ID="LblSubDate" runat="server" Text='<%# Eval("createdOn", "{0:MM-dd-yyyy}") %>'></asp:Label></td>
                                        </tr>
                                        <tr style="text-align: right; font-weight: bold;">
                                            <td colspan="6"></td>
                                            <td colspan="2" style="background-color: #EFF3FB;">Error Count</td>
                                            <td colspan="2" style="background-color: #EFF3FB;">View Error Report</td>
                                        </tr>
                                        <tr style="text-align: right;">
                                            <td colspan="2">
                                                <asp:LinkButton ID="DownloadFiles" runat="server" Text="Download Files" CommandName="Update"></asp:LinkButton>
                                            </td>
                                            <td colspan="4"></td>
                                            <td colspan="2"><asp:Label ID="LblErrorCount" runat="server"></asp:Label></td>
                                            <td colspan="2"><asp:HyperLink ID="HlErrorReport" enabled="false" runat="server" Text="View Errors" NavigateUrl="#"></asp:HyperLink></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr> 
        </table>
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
