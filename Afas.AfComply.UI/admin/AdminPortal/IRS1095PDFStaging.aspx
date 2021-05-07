<%@ Page EnableSessionState="ReadOnly" Title="Transfer PDFs" Language="C#"
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="IRS1095PDFStaging.aspx.cs"
    Inherits="Afas.AfComply.UI.admin.AdminPortal.IRS1095PDFStaging" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" AsyncPostBackTimeout="3600"></asp:ToolkitScriptManager>

    <h2>Import Printed 1095's</h2>
    <br />

    <label class="lbl3">Select Employer:</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    <br />
    <br />

    <label class="lbl3">Select Tax Year:</label>
    <asp:DropDownList ID="DdlTaxYear" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlTaxYear_SelectedIndexChanged"></asp:DropDownList>
    <br />

    <asp:Label ID="LblPdfCount" runat="server"></asp:Label>
    <br />
    <br />
    <asp:Label ID="LblPdfCount1094" runat="server"></asp:Label>
    <br />
    <br />
    <asp:Button ID="BtnMove1095" runat="server" Text="Move PDFs" OnClick="BtnMove1095_Click" />
    <br />
    <br />
    <asp:Button ID="BtnMoveAll1095" runat="server" Text="Move ALL PDFs" OnClick="BtnMoveAll1095_Click" />
    <br />
    <br />
    <asp:Button ID="BtnMove1094" runat="server" Text="Move 1094 PDFs" OnClick="BtnMove1094_Click" />
    <br />
    <br />
    <asp:Button ID="BtnMoveAll1094" runat="server" Text="Move ALL 1094 PDFs" OnClick="BtnMoveAll1094_Click" />
    <br />

    <asp:Label ID="lblMsg" runat="server"></asp:Label>
    
    









    <h3>Clear Folders</h3>
    <asp:GridView ID="GvCurrentDirectories" runat="server" CellPadding="4" ForeColor="Black" GridLines="None" AllowPaging="true" Width="1024px" AutoGenerateColumns="false" PageSize="50" OnRowDeleting="GvCurrentDirectories_RowDeleting" AllowSorting="true">
        <AlternatingRowStyle BackColor="White" />
        <EditRowStyle BackColor="transparent" />
        <FooterStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#eb0029" ForeColor="White" HorizontalAlign="Center" />
        <PagerSettings FirstPageImageUrl="~/design/first.png" Mode="NextPreviousFirstLast" LastPageImageUrl="../design/last.png" NextPageImageUrl="../design/next.png" PreviousPageImageUrl="../design/prev.png" PageButtonCount="25" Position="TopAndBottom" />
        <RowStyle BackColor="#EFF3FB" />
        <SelectedRowStyle BackColor="white" Font-Bold="True" ForeColor="black" />
        <SortedAscendingCellStyle BackColor="#F5F7FB" />
        <SortedAscendingHeaderStyle BackColor="#eb0029" />
        <SortedDescendingCellStyle BackColor="#E9EBEF" />
        <SortedDescendingHeaderStyle BackColor="#eb0029" />
        <Columns>
            <asp:TemplateField HeaderStyle-Width="25px" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:ImageButton ID="ImgBtnDelete" runat="server" ImageUrl="~/images/close_box_red.png" Height="20px" CommandName="Delete" />
                    <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="ImgBtnDelete" ConfirmText="Are you sure you want to DELETE this Folder?"></asp:ConfirmButtonExtender>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Folder Name" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" SortExpression="Name">
                <ItemTemplate>
                    <asp:Label ID="LblFileName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                    <asp:HiddenField ID="HfFilePath" runat="server" Value='<%# Eval("FullName") %>' />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Created On" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:Label ID="LblFileCreatedOn" runat="server" Text='<%# Eval("LastWriteTime") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

        </Columns>

    </asp:GridView>


    <h3>Clear Files</h3>

    <asp:GridView ID="GvCurrentFiles" runat="server" CellPadding="4" ForeColor="Black" GridLines="None" AllowPaging="true" Width="1024px" AutoGenerateColumns="false" PageSize="50" OnRowDeleting="GvCurrentFiles_RowDeleting" AllowSorting="true">
        <AlternatingRowStyle BackColor="White" />
        <EditRowStyle BackColor="transparent" />
        <FooterStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#eb0029" ForeColor="White" HorizontalAlign="Center" />
        <PagerSettings FirstPageImageUrl="~/design/first.png" Mode="NextPreviousFirstLast" LastPageImageUrl="../design/last.png" NextPageImageUrl="../design/next.png" PreviousPageImageUrl="../design/prev.png" PageButtonCount="25" Position="TopAndBottom" />
        <RowStyle BackColor="#EFF3FB" />
        <SelectedRowStyle BackColor="white" Font-Bold="True" ForeColor="black" />
        <SortedAscendingCellStyle BackColor="#F5F7FB" />
        <SortedAscendingHeaderStyle BackColor="#eb0029" />
        <SortedDescendingCellStyle BackColor="#E9EBEF" />
        <SortedDescendingHeaderStyle BackColor="#eb0029" />
        <Columns>

            <asp:TemplateField HeaderStyle-Width="25px" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:ImageButton ID="ImgBtnDelete" runat="server" ImageUrl="~/images/close_box_red.png" Height="20px" CommandName="Delete" />
                    <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="ImgBtnDelete" ConfirmText="Are you sure you want to DELETE this file?"></asp:ConfirmButtonExtender>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="File Name" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" SortExpression="Name">
                <ItemTemplate>
                    <asp:Label ID="LblFileName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                    <asp:HiddenField ID="HfFilePath" runat="server" Value='<%# Eval("FullName") %>' />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="File Created On" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:Label ID="LblFileCreatedOn" runat="server" Text='<%# Eval("CreationTimeUtc") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="File Size" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:Label ID="LblFileLength" runat="server" Text='<%# Eval("Length") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>

    </asp:GridView>


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





</asp:Content>
