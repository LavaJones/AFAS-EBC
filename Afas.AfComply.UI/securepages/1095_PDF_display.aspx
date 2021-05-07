<%@ Page Title="" Language="C#" MasterPageFile="~/securepages/SecurePages.Master" AutoEventWireup="true" CodeBehind="1095_PDF_display.aspx.cs" Inherits="Afas.AfComply.UI.securepages.securepages_1095_PDF_display" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="bodycontent" runat="server">
    <style>
        .btn1 {
            background-color: #eb0029;
            color: white;
            border: 3px solid #f44336;
        }
    </style>
    <link rel="stylesheet" type="text/css" href="/leftnav.css?1.4.0.100" />
    <div id="header">
        <asp:HiddenField ID="HfEmployerTypeID" runat="server" />
        <asp:HiddenField ID="HfUserName" runat="server" />
        <asp:HiddenField ID="HfDistrictID" runat="server" />
    </div>
    <div id="content">

        <div id="topbox">
            <div id="tbleft" style="padding-bottom: 20px;">
                <br />
                <label class="lbl3">Select Tax Year:</label>
                <asp:DropDownList ID="DdlTaxYear" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlTaxYear_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                <br />


                <div id="1095C" style="float: left">
                    <h3>1095C - Download Forms</h3>
                    <br />
                    <p>
                        Download .zip file of all individual forms.
                    </p>
                    <asp:Button ID="DownloadFiles" Text="Download All PDFs" runat="server" CssClass="btn1" OnClick="DownloadFiles_Click" />
                    <br />

                    <asp:GridView ID="GvFiles" runat="server" CellPadding="4" ForeColor="Black" GridLines="None" AllowPaging="true" Width="1024px" AutoGenerateColumns="false" OnRowUpdating="GvFiles_RowUpdating" PageSize="50" OnRowDeleting="GvFiles_RowDeleting" AllowSorting="true">
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
                            <asp:TemplateField HeaderStyle-Width="25px" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgBtnDownload" runat="server" ImageUrl="~/design/Download.png" Height="20px" CommandName="Update" ToolTip="Download this file" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="File Name" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" SortExpression="Name">
                                <ItemTemplate>
                                    <asp:Label ID="Lb1094lFileName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
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
                    <h3>(<asp:Literal ID="LitCount" runat="server"></asp:Literal>)  -1095Records
                    </h3>
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
                <br />
                <br />

                <asp:GridView ID="GvCurrentFiles" runat="server" CellPadding="4" ForeColor="Black" GridLines="None" AllowPaging="true" Width="1024px" AutoGenerateColumns="false" OnRowUpdating="GvCurrentFiles_RowUpdating" PageSize="50" OnPageIndexChanging="GvCurrentFiles_PageIndexChanging" OnSorting="GvCurrentFiles_Sorting" OnRowDeleting="GvCurrentFiles_RowDeleting" AllowSorting="true">
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
                        <asp:TemplateField HeaderStyle-Width="25px" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgBtnDownload" runat="server" ImageUrl="~/design/Download.png" Height="20px" CommandName="Update" ToolTip="Download this file" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" SortExpression="Name">
                            <ItemTemplate>
                                <asp:Label ID="LblEmployeeName" runat="server" Text='<%# string.Concat(Regex.Split(Eval("Name").ToString(),"_")[0]," " ,Regex.Split(Eval("Name").ToString(),"_")[1])%> '></asp:Label>

                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="File Name" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" SortExpression="Name">
                            <ItemTemplate>
                                <asp:Label ID="LblFileName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="File Created On" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="LblFileCreatedOn" runat="server" Text='<%# Eval("CreationTimeUtc") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>

            </div>


        </div>
        <asp:HiddenField ID="HfDummyTrigger" runat="server" />
    </div>

    <script>
        setTimeout(AutoLogout, <%= Feature.AutoLogoutTime %> );
        
        function AutoLogout() {
            alert("<%= Branding.AutoLogoutMessage %>");
            window.location = window.location.href;
        }
    </script>
</asp:Content>
