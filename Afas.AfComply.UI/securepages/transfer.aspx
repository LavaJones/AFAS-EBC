<%@ Page Title="" Language="C#" MasterPageFile="~/securepages/SecurePages.Master" AutoEventWireup="true" CodeBehind="transfer.aspx.cs" Inherits="securepages_transfer" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="bodycontent" runat="server">
    <link rel="stylesheet" type="text/css" href="/Body.css" />
    <link rel="stylesheet" type="text/css" href="/leftnav.css" />
    <asp:HiddenField ID="HfUserName" runat="server" />
    <asp:HiddenField ID="HfDistrictID" runat="server" />
    <div class="left_ebc">
        <%= demo.getLeftLinks(null != Session["CurrentDistrict"] && ((employer)Session["CurrentDistrict"]).IrsEnabled) %>
    </div>
    <div class="middle_ebc" style="margin: auto;">
        <h1>Upload File</h1>
        <h3>STEPS FOR UPLOADING FILES</h3>
        * Step 1: Select a file type.<br />
        * Step 2: Click the Browse Button  or Choose File Button & find the correct file
        <br />
        * Step 3: Click the SAVE Button.
        <br />
        <br />

        <label class="lbl">File Type</label>
        <asp:DropDownList ID="DdlFileType" runat="server" CssClass="ddl" AutoPostBack="true" OnSelectedIndexChanged="DdlFileType_SelectedIndexChanged">
            <asp:ListItem Text="Payroll" Value="PAY"></asp:ListItem>
            <asp:ListItem Text="Payroll Modification" Value="PAY_MOD"></asp:ListItem>
            <asp:ListItem Text="Demographic" Value="DEM"></asp:ListItem>
            <asp:ListItem Text="Gross Pay Descriptions" Value="GP"></asp:ListItem>
            <asp:ListItem Text="HR Status Descriptions" Value="HR"></asp:ListItem>
            <asp:ListItem Text="Employee Classification" Value="EC"></asp:ListItem>
            <asp:ListItem Text="Insurance Offers" Value="IO"></asp:ListItem>
            <asp:ListItem Text="Insurance Carrier Report" Value="IC"></asp:ListItem>
            <asp:ListItem Text="Select" Value="Select" Selected="True"></asp:ListItem>
        </asp:DropDownList>
        <br />
        <br />
        <div style="padding-left: 10%;">
            <asp:FileUpload ID="FuFileTransfer" runat="server" Width="300px" />
        </div>
        <br />
        <br />
        <asp:Button ID="BtnSaveFile" runat="server" Text="Save File" OnClick="BtnSaveFile_Click" Width="285px" CssClass="btn" />
        <br />

        <asp:Label ID="LblFileUploadMessage" runat="server"></asp:Label>
        <asp:HiddenField ID="HfDummyTrigger" runat="server" />

        <asp:Panel ID="PnlMessage" runat="server">
            <div style="position: fixed; top: 0; left: 0; background-color: gray; opacity: 0.8; width: 100%; height: 100%;">
            </div>
            <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white; z-index: 10">

                <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                    <asp:ImageButton ID="ImgBtnClose" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" BorderStyle="None" />
                </div>

                <h3 style="color: black;">Webpage Message</h3>
                <p style="color: black">
                    <asp:Literal ID="LitMessage" runat="server"></asp:Literal>
                </p>
                <br />
            </div>
        </asp:Panel>

        <asp:ModalPopupExtender ID="MpeWebMessage" runat="server" TargetControlID="HfDummyTrigger" OkControlID="ImgBtnClose" PopupControlID="PnlMessage"></asp:ModalPopupExtender>

    </div>

    <div class="right_ebc">
        <h3>Current Files</h3>
        <div style="position: absolute; top: 15px; right: 85px; width: 200px; text-align: right;">
        </div>
        <div style="padding-left: 0%;">
            <asp:GridView ID="GvCurrentFiles" runat="server" CellPadding="4" ForeColor="#eb0029" GridLines="None" Width="500px" AutoGenerateColumns="false" OnRowDeleting="GvCurrentFiles_RowDeleting">
                <AlternatingRowStyle BackColor="White" />
                <EditRowStyle BackColor="#2461BF" />
                <FooterStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="#f8f8f8" ForeColor="White" HorizontalAlign="Center" />
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
                            <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="ImgBtnDelete" ConfirmText="Are you sure you want to DELETE this file?"></asp:ConfirmButtonExtender>
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
        </div>
    </div>

    <script>
        setTimeout(AutoLogout, <%= Feature.AutoLogoutTime %> );

        function AutoLogout() {
            alert("<%= Branding.AutoLogoutMessage %>");
            window.location = window.location.href;
        }
    </script>
</asp:Content>

