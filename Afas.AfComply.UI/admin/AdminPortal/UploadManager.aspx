<%@ Page EnableSessionState="ReadOnly" Title="UploadManager" Language="C#" MasterPageFile="AdminPortal.Master" AutoEventWireup="true"
    CodeBehind="UploadManager.aspx.cs" Inherits="Afas.AfComply.UI.admin.AdminPortal.UploadManager" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>

    <asp:ModalPopupExtender ID="ModalPopupExtender" runat="server" PopupControlID="PnlReUpload" TargetControlID="HfDummyTrigger"></asp:ModalPopupExtender>

    <asp:Panel ID="PnlReUpload" runat="server" DefaultButton="BtnUploadFile">
        <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
        </div>
        <div style="position: relative; margin-left: auto; padding: 50px; margin-right: auto; border-radius: 10px; width: 800px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
            <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                <asp:ImageButton ID="ImageButton1" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" />
            </div>
            <h2>Upload Corrected File</h2>
            <asp:FileUpload ID="FuReUpload" runat="server" Width="350px" />
            <asp:Button ID="BtnUploadFile" runat="server" CssClass="btn" Text="Submit" OnClick="BtnUploadFile_Click" />
            <asp:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" TargetControlID="BtnUploadFile" ConfirmText="Are you sure that you want to replace the original File with this one?"></asp:ConfirmButtonExtender>
        </div>

    </asp:Panel>

    <asp:ModalPopupExtender ID="MpeUploadDetails" runat="server" PopupControlID="UploadDetails" TargetControlID="HfDummyTrigger"></asp:ModalPopupExtender>

    <asp:Panel ID="UploadDetails" runat="server">
        <div style="padding: 10px">
            <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
            </div>
            <div style="position: relative; margin-left: auto; padding: 50px; margin-right: auto; border-radius: 10px; width: 800px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                    <asp:ImageButton ID="ImageButton2" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" />
                </div>
                <h2>File Processing Details</h2>
                <asp:HiddenField ID="Hf_gv_Up_id" runat="server" />
                <asp:GridView ID="GvStaging" runat="server" AutoGenerateColumns="false" OnRowCommand="GvStaging_RowCommand"
                    EmptyDataText="This file has not been processed" Width="100%">
                    <Columns>
                        <asp:TemplateField HeaderText="Last Modified">
                            <ItemTemplate>
                                <div style="padding: 5px">
                                    <asp:Literal ID="Lit_gv_Stage_Date" runat="server" Text='<%# Eval("ModifiedDate") %>'></asp:Literal>
                                    <asp:HiddenField ID="Hf_gv_Stage_id" runat="server" Value='<%# Eval("StagingImportId") %>' />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Row Count">
                            <ItemTemplate>
                                <div style="padding: 5px">
                                    <asp:Literal ID="Lit_gv_Stage_RowCount" runat="server" Text='<%# Eval("RowCount") %>'></asp:Literal>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="View/Edit Data" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="ViewData" runat="server" Height="30px" ImageUrl="~/images/edit_notes.png"
                                    Width="30px" BorderStyle="None" AlternateText="View or Edit Data"
                                    CommandName="ViewEdit" CommandArgument='<%# Eval("StagingImportId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Download" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="download" runat="server" Height="30px" ImageUrl="~/design/Download.png"
                                    Width="30px" BorderStyle="None" AlternateText="Download File"
                                    CommandName="DownloadFile" CommandArgument='<%# Eval("StagingImportId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Reprocess" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:ImageButton ID="Reporcess" runat="server" Height="30px" ImageUrl="~/images/back-icon.png"
                                    Width="30px" BorderStyle="None" AlternateText="View or Edit Data"
                                    CommandName="Reprocess" CommandArgument='<%# Eval("StagingImportId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </asp:Panel>

    <h2>Manage Uploads</h2>
    <asp:GridView ID="GvEmployers" runat="server" AutoGenerateColumns="false"
        EmptyDataText="No Unprocessed Files Exist" Width="100%">
        <Columns>
            <asp:TemplateField HeaderText="Employer Name">
                <ItemTemplate>
                    <h2>
                        <asp:Literal ID="Lit_gv_name" runat="server" Text='<%# Eval("EMPLOYER_NAME") %>'></asp:Literal>
                    </h2>
                    <asp:HiddenField ID="Hf_gv_Emp_id" runat="server" Value='<%# Eval("EMPLOYER_ID") %>' />
                    <div style="padding: 10px">
                        <asp:GridView ID="GvUploads" runat="server" AutoGenerateColumns="false"
                            EmptyDataText="No Unprocessed Files Exist" Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemTemplate>
                                        <h3>
                                            <asp:Literal ID="Lit_gv_Up_name" runat="server" Text='<%# Eval("FileNameNoPath") %>'></asp:Literal>
                                        </h3>
                                        <asp:HiddenField ID="Hf_gv_Up_id" runat="server" Value='<%# Eval("UploadedFileInfoId") %>' />
                                        <div style="padding: 10px">
                                            <asp:GridView ID="GvFile" runat="server" AutoGenerateColumns="false" OnRowCommand="GvFile_RowCommand"
                                                EmptyDataText="No Uploads Exist" Width="100%">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Upload Status">
                                                        <ItemTemplate>
                                                            <div style="padding: 5px">
                                                                <asp:Literal ID="Lit_gv_Up_name" runat="server" Text='<%# Eval("Status") %>'></asp:Literal>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Download" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="download" runat="server" Height="30px" ImageUrl="~/design/Download.png"
                                                                Width="30px" BorderStyle="None" AlternateText="Download File"
                                                                CommandName="DownloadFile" CommandArgument='<%# Eval("UploadedFileInfoId") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="View Details" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="viewDetails" runat="server" Height="30px" ImageUrl="~/design/eyeopen.png"
                                                                Width="30px" BorderStyle="None" AlternateText="View Details"
                                                                CommandName="ViewDetails" CommandArgument='<%# Eval("UploadedFileInfoId") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Re-Upload" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="reUpload" runat="server" Height="30px" ImageUrl="~/design/Upload.png"
                                                                Width="30px" BorderStyle="None" AlternateText="Re-Upload File"
                                                                CommandName="ReuploadFile" CommandArgument='<%# Eval("UploadedFileInfoId") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Archive" ItemStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="archiveFile" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png"
                                                                Width="30px" BorderStyle="None" AlternateText="Archive File"
                                                                CommandName="ArchiveFile" CommandArgument='<%# Eval("UploadedFileInfoId") %>' />
                                                            <asp:ConfirmButtonExtender ID="ConfirmButtonExtender" runat="server" ConfirmText="Are you sure you want to Archive this File?" TargetControlID="archiveFile"></asp:ConfirmButtonExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Upload Type">
                                                        <ItemTemplate>
                                                            <div style="padding: 5px">
                                                                <asp:Literal ID="Lit_gv_UpType" runat="server" Text='<%# Eval("UploadTypeDescription") %>'></asp:Literal>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Uploaded By">
                                                        <ItemTemplate>
                                                            <div style="padding: 5px">
                                                                <asp:Literal ID="Lit_gv_UpBy" runat="server" Text='<%# Eval("UploadedByUser") %>'></asp:Literal>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Uploaded On">
                                                        <ItemTemplate>
                                                            <div style="padding: 5px">
                                                                <asp:Literal ID="Lit_gv_UpTime" runat="server" Text='<%# Eval("UploadTime") %>'></asp:Literal>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Uploaded From">
                                                        <ItemTemplate>
                                                            <div style="padding: 5px">
                                                                <asp:Literal ID="Lit_gv_UpFrom" runat="server" Text='<%# Eval("UploadSourceDescription") %>'></asp:Literal>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
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
