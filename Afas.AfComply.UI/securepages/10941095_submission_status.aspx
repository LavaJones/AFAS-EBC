<%@ Page Title="" Language="C#" MasterPageFile="~/securepages/SecurePages.Master" AutoEventWireup="true" CodeBehind="10941095_submission_status.aspx.cs" Inherits="Afas.AfComply.UI.securepages._10941095_submission_status" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="bodycontent" runat="server">
    <div id="header">
        <asp:HiddenField ID="HfEmployerTypeID" runat="server" />
        <asp:HiddenField ID="HfUserName" runat="server" />
        <asp:HiddenField ID="HfDistrictID" runat="server" />
    </div>

    <div id="topbox">
        <div id="tbleft" style="padding-bottom: 20px; float: left;">
            <h3>Tax Year</h3>
            <p>
                Select the year you would like to view the electronic submissions for.
            </p>
            <asp:DropDownList ID="DdlYears" runat="server" CssClass="ddl" AutoPostBack="true" OnSelectedIndexChanged="DdlYears_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
        <div id="tbright" style="float: right; padding-right: 500px;">
            <h2>Submission Status </h2>
            <h2>
                <asp:Literal ID="litStatus" runat="server"></asp:Literal>
            </h2>
        </div>
    </div>
    <br />
    <div>
        <asp:Panel ID="Errors1094" runat="server">
            <h3>1094 Errors</h3>
            <span>Error Count:</span>
            <asp:Label ID="LblErrorCount1094" runat="server"></asp:Label>
            <br />
            <span>Error Report:</span>
            <asp:HyperLink ID="HlErrorReport1094" Enabled="false" runat="server" Text="View Errors" NavigateUrl="#"></asp:HyperLink>
            <hr />

        </asp:Panel>
        <br />
        <br />
        <asp:Panel ID="Errors1095" runat="server">
            <h3>1095 Errors</h3>
            <span style="background-color: #EFF3FB;">Error Count:</span>
            <asp:Label ID="LblErrorCount1095" runat="server"></asp:Label>
            <br />
            <span style="background-color: #EFF3FB;">Error Report:</span>
            <asp:HyperLink ID="HlErrorReport1095" Enabled="false" runat="server" Text="View Errors" NavigateUrl="#"></asp:HyperLink>
            <hr />

        </asp:Panel>
    </div>
    <br />
    <br />
    <table>
        <tr>
            <td colspan="10">
                <asp:GridView ID="GvCurrentReceipts" runat="server" CellPadding="4" ForeColor="black" GridLines="None" Width="1024px" ShowFooter="false" ShowHeader="false" EmptyDataText="No submissions could be found." AutoGenerateColumns="false" OnRowDataBound="GvCurrentReceipts_RowDataBound">
                    <AlternatingRowStyle BackColor="White" />
                    <EditRowStyle BackColor="transparent" />
                    <RowStyle BackColor="#EFF3FB" />
                    <SelectedRowStyle BackColor="white" Font-Bold="True" ForeColor="black" />
                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                    <SortedAscendingHeaderStyle BackColor="#eb0029" />
                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                    <SortedDescendingHeaderStyle BackColor="#eb0029" />
                    <Columns>
                        <asp:TemplateField ShowHeader="false">
                            <ItemTemplate>
                                <tr style="background-color: #eb0029; color: white; font-weight: bold;">
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
                                    <td colspan="2">
                                        <asp:Label ID="LblUTID" runat="server" Text='<%# Eval("UniqueTransmissionId") %>'></asp:Label></td>
                                    <td colspan="2">
                                        <asp:Label ID="LblSubmissionType" runat="server" Text='<%# Eval("TransmissionType") %>'></asp:Label></td>
                                    <td colspan="2">
                                        <asp:HiddenField ID="LblTransmissionStatus" runat="server" Value='<%# Eval("transmission_status_code_id") %>'></asp:HiddenField>
                                        <asp:Literal ID="LitTstatus" runat="server" Text="Processing"></asp:Literal>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="LblReceiptID" runat="server" Text='<%# Eval("ReceiptID") %>'></asp:Label></td>
                                    <td colspan="2">
                                        <asp:Label ID="LblSubDate" runat="server" Text='<%# Eval("createdOn", "{0:MM-dd-yyyy}") %>'></asp:Label></td>
                                </tr>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="HfDummyTrigger" runat="server" />
</asp:Content>
