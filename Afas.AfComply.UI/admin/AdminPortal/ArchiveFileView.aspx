<%@ Page EnableSessionState="ReadOnly" Title="View Archived Files" Language="C#" 
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" 
    CodeBehind="ArchiveFileView.aspx.cs" Inherits="Afas.AfComply.UI.admin.AdminPortal.ArchiveFileView" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>


    <h2>View Archived Files</h2>
    <br />
    <label class="lbl3">Select Employer</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged"  AutoPostBack="true"></asp:DropDownList>
    <br />
    <asp:Label ID="cofein" runat="server"></asp:Label>
    <br />


    
    <h3>Current Files in Archive</h3>
            <asp:GridView ID="GvCurrentFiles" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" Width="1024px" AutoGenerateColumns="false" OnRowUpdating="GvCurrentFiles_RowUpdating">
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
                    <asp:TemplateField HeaderStyle-Width="25px" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImgBtnDownload" runat="server" ImageUrl="~/design/Download.png" Height="20px" CommandName="Update" ToolTip="Download this file" />
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="File Name" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="LblFileName" runat="server" Text='<%# Eval("FileName") %>'></asp:Label>
                            <asp:HiddenField ID="HfFilePath" runat="server" Value='<%# Eval("ArchiveFilePath") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Archived On" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="LblFileCreatedOn" runat="server" Text='<%# Eval("ArchivedTime") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Reason" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="LblFileArchiveReason" runat="server" Text='<%# Eval("ArchiveReason") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                </Columns>
            </asp:GridView>




</asp:Content>
