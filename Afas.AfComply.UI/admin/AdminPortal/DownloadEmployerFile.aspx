<%@ Page EnableSessionState="ReadOnly" Title="Download Employer File" Language="C#"
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="DownloadEmployerFile.aspx.cs"
    Inherits="Afas.AfComply.UI.admin.AdminPortal.DownloadEmployerFile" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>



<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <label class="lbl3">Select Employer</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    <br />
    <label class="lbl3">Select Files</label>
    <asp:DropDownList ID="DdlFileLocation" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlFileLocation_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    <br />
    <br />
    <asp:Button ID="BtnDownload" runat="server" Text="Download All" Width="125" CssClass="btn" OnClick="BtnDownload_Click" />
    <br />
    <br />
    <div>
        <asp:CheckBox ID="CbCheckAll" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="CbCheckAll_CheckedChanged" />
        <br />
        <br />
        <asp:Button ID="BtnDelete" runat="server" Text="Delete selected records" Width="15%" CssClass="btn" OnClick="BtnDelete_Click" />
        <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="BtnDelete" ConfirmText="Are you sure you want to DELETE all of the selected records? They are not able to be recovered once you DELETE them."></asp:ConfirmButtonExtender>
        <p>
            <asp:Label ID="LblMessage" runat="server"></asp:Label>
        </p>
    </div>
    <br />
    <div style="float: left">
        <h4>(<asp:Literal ID="LitPayrollCorrectCount" runat="server"></asp:Literal>) - 
              Records
        </h4>
    </div>
    <asp:GridView ID="GvCurrentFiles" runat="server" CellPadding="4" AllowSorting="true" ForeColor="#333333" GridLines="None"
        Width="1024px" AutoGenerateColumns="false" OnRowDeleting="GvCurrentFiles_RowDeleting" OnRowUpdating="GvCurrentFiles_RowUpdating"
        OnSorting="ComponentGridView_Sorting" EnableViewState="true">
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
            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:CheckBox ID="Cb_gv_Selected" runat="server" Checked="false" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderStyle-Width="25px" HeaderStyle-HorizontalAlign=" Left">
                <ItemTemplate>
                    <asp:ImageButton ID="ImgBtnDelete" runat="server" ImageUrl="~/images/close_box_red.png" Height="20px" CommandName="Delete" />
                    <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="ImgBtnDelete" ConfirmText="Are you sure you want to DELETE this file?"></asp:ConfirmButtonExtender>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderStyle-Width="25px" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:ImageButton ID="ImgBtnDownload" runat="server" ImageUrl="~/design/Download.png" Height="20px" CommandName="Update" ToolTip="Download this file" CommandArgument='<%# Eval("FileName") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Employer Name" HeaderStyle-Width="200px" SortExpression="EmployerName" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:Label ID="LblEmployerName" runat="server" Text='<%# Eval("EmployerName") %>'></asp:Label>
                    <asp:HiddenField ID="HfEmployerId" runat="server" Value='<%# Eval("employerId") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="File Name" HeaderStyle-Width="200px" SortExpression="FileName" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:Label ID="LblFileName" runat="server" Text='<%# Eval("FileName") %>'></asp:Label>
                    <asp:HiddenField ID="HfFilePath" runat="server" Value='<%# Eval("FileFullPath") %>' />
                    <asp:HiddenField ID="HfFileName" runat="server" Value='<%# Eval("FileName") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="File Size" SortExpression="Length" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:Label ID="LblFileSize" runat="server" Text='<%# Eval("Length") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="File Created On" SortExpression="CreationTimeUtc" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:Label ID="LblFileCreatedOn" runat="server" Text='<%# Eval("CreationTimeUtc") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

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


</asp:Content>
