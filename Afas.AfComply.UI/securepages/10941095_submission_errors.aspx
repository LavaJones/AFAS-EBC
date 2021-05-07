<%@ Page Title="" Language="C#" MasterPageFile="~/securepages/SecurePages.Master" AutoEventWireup="true" CodeBehind="10941095_submission_errors.aspx.cs" Inherits="Afas.AfComply.UI.securepages._10941095_submission_errors" %>

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
            To correct the errors listed below, click the following button: 
                        <asp:Button ID="btn1095Corrections" runat="server" Text="1095 Corrections" OnClick="btn1095Corrections_Click" />
        </div>
        <div id="tbright" style="float: right; padding-right: 500px;">
            <h3>Export Errors (.csv)</h3>
            <asp:ImageButton ID="ImgBtnExport" runat="server" ImageUrl="~/design/csv-file-icon.png" Height="25px" Width="25px" OnClick="ImgBtnExport_Click" />
        </div>
    </div>
    <br />
    <hr />
    <h3>Employer Transmission Errors</h3>
    <asp:GridView ID="GvEmployerErrors" runat="server" AutoGenerateColumns="false" Width="1025px" PageSize="50" EmptyDataText="No errors could be found.">
        <EditRowStyle BackColor="#eb0029" />
        <FooterStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="White" ForeColor="Black" />
        <PagerStyle BackColor="#eb0029" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#EFF3FB" />
        <SelectedRowStyle BackColor="white" Font-Bold="True" ForeColor="black" />
        <SortedAscendingCellStyle BackColor="#F5F7FB" />
        <SortedAscendingHeaderStyle BackColor="#eb0029" />
        <SortedDescendingCellStyle BackColor="#E9EBEF" />
        <SortedDescendingHeaderStyle BackColor="#eb0029" />
        <Columns>
            <asp:TemplateField HeaderText="Error Code" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px">
                <ItemTemplate>
                    <asp:Label ID="Lbl_rpt_error_code" runat="server" Text='<%# Eval("ErrorCode") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Message" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="650px">
                <ItemTemplate>
                    <asp:Label ID="Lbl_rpt_error_text" runat="server" Text='<%# Eval("ErrorMessage") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>



    <h3>Employee Transmission Errors</h3>
    <asp:GridView ID="GvErrors" runat="server" AutoGenerateColumns="false" Width="1025px" AllowPaging="true" AllowSorting="false" PageSize="5" OnSorting="GvErrors_Sorting" OnPageIndexChanging="GvErrors_PageIndexChanging" OnRowDataBound="GvErrors_RowDataBound">
        <EditRowStyle BackColor="#eb0029" />
        <FooterStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="White" ForeColor="Black" />
        <PagerStyle BackColor="#eb0029F" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#EFF3FB" />
        <SelectedRowStyle BackColor="white" Font-Bold="True" ForeColor="black" />
        <SortedAscendingCellStyle BackColor="#F5F7FB" />
        <SortedAscendingHeaderStyle BackColor="#eb0029" />
        <SortedDescendingCellStyle BackColor="#E9EBEF" />
        <SortedDescendingHeaderStyle BackColor="#eb0029" />
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="50px" SortExpression="RE_ID">
                <ItemTemplate>
                    <asp:Label ID="Lbl_rpt_id" runat="server" Text='<%# Eval("tax_year_employee_transmissionID") %>'></asp:Label>
                    <asp:HiddenField ID="Hf_rpt_employeeID" runat="server" Value='<%# Eval("employee_id") %>' />
                    <asp:HiddenField ID="Hf_rpt_recordID" runat="server" Value='<%# Eval("RecordID") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="First Name" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" SortExpression="RE_ERROR_FIRST_NAME">
                <ItemTemplate>
                    <asp:Label ID="Lbl_rpt_fname" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Last Name" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" SortExpression="RE_ERROR_LAST_NAME">
                <ItemTemplate>
                    <asp:Label ID="Lbl_rpt_lname" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Transmission Status" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px">
                <ItemTemplate>
                    <asp:Label ID="Lbl_rpt_TS" runat="server" Text='<%# Eval("transmission_status_code_id") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Error Code" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" SortExpression="RE_ERROR_CODE">
                <ItemTemplate>
                    <asp:Label ID="Lbl_rpt_error_code" runat="server" Text="n/a"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Message" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="650px" SortExpression="RE_ERROR_TEXT">
                <ItemTemplate>
                    <asp:Label ID="Lbl_rpt_error_text" runat="server" Text="n/a"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <br />
    <asp:HiddenField ID="HfDummyTrigger" runat="server" />
    <br />
    <asp:HiddenField ID="HfDummyTrigger2" runat="server" />

    <asp:Panel ID="PnlMessage" runat="server">
        <div style="position: fixed; top: 0; left: 0; background-color: gray; opacity: 0.8; width: 100%; height: 100%;">
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

</asp:Content>

