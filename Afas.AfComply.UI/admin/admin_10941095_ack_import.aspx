<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="admin_10941095_ack_import.aspx.cs" Inherits="Afas.AfComply.UI.admin.admin_10941095_ack_import" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>1094/1095 IRS Submission Ack Import</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" type="text/css" href="/default.css" />
    <link rel="stylesheet" type="text/css" href="/menu.css" />
    <link rel="stylesheet" type="text/css" href="/v_menu.css" />

</head>
<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
        <div id="container">
            <asp:UpdatePanel ID="upImport" runat="server">
                <ContentTemplate>
                    <div id="header">
                        <a href="default.aspx">
                            <img src="<%= Page.ResolveUrl(Branding.LogoUri) %>" style="height: 70px" alt="<%= Branding.ProductName %> Logo" class="logo" />
                        </a>
                        <ul id="toplinks">
                            <li>Need Help? Call <%= System.Configuration.ConfigurationManager.AppSettings["Branding.PhoneNumber"] %></li>
                            <li>
                                <asp:Literal ID="LitUserName" runat="server"></asp:Literal></li>
                            <li>
                                <asp:Button ID="BtnLogout" CssClass="btn" runat="server" Text="Log Out" UseSubmitBehavior="false" OnClick="BtnLogout_Click" /></li>
                        </ul>
                        <asp:HiddenField ID="HfDistrictID" runat="server" />
                    </div>
                    <div id="nav2">
                        <nav>
                            <%= demo.getAdminLinks() %>
                        </nav>
                    </div>
                    <div id="topbox">
                        <div id="tbleft">
                            <h4>Employer Search</h4>
                            <asp:Panel ID="PnlSearch" runat="server" DefaultButton="BtnSearch">
                                <label class="lbl3">Search Employer</label>
                                <asp:TextBox ID="TxtSearchEmployer" runat="server" CssClass="txtLong"></asp:TextBox>
                                <asp:Button ID="BtnSearch" runat="server" Text="Search" Width="50px" OnClick="BtnSearch_Click" />
                            </asp:Panel>
                            <br />
                            <asp:Panel ID="PnlImport" runat="server" DefaultButton="BtnSubmit">
                                <h4>Import Ack File</h4>
                                <label class="lbl3">Employer</label>
                                <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" AutoPostBack="true" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged"></asp:DropDownList>
                                <br />
                                <label class="lbl3">Tax Year</label>
                                <asp:DropDownList ID="DdlYears" runat="server" CssClass="ddl2" AutoPostBack="true" OnSelectedIndexChanged="DdlYears_SelectedIndexChanged">
                                </asp:DropDownList>
                                <br />
                                <label class="lbl3">Receipt ID</label>
                                <asp:DropDownList ID="DdlReceiptID" runat="server" CssClass="ddl2" AutoPostBack="true" OnSelectedIndexChanged="DdlReceiptID_SelectedIndexChanged">
                                </asp:DropDownList>
                                <br />
                                <label class="lbl3">Submission Status</label>
                                <asp:DropDownList ID="DdlSubmissionStatus" runat="server" CssClass="ddl2" AutoPostBack="true" OnSelectedIndexChanged="DdlSubmissionStatus_SelectedIndexChanged">
                                </asp:DropDownList>
                                <br />
                                <label class="lbl3">Ack File</label>
                                <asp:DropDownList ID="DdlAckFile" runat="server" CssClass="ddl2">
                                </asp:DropDownList>
                                <br />
                                <asp:Button ID="BtnSubmit" runat="server" Text="Submit" OnClick="BtnSubmit_Click" />
                            </asp:Panel>
                        </div>
                        <div id="tbright">
                            <h4>Import Ack Files</h4>
                            <asp:AjaxFileUpload ID="AfuAck" runat="server" OnUploadComplete="AfuAck_UploadComplete" />
                        </div>
                    </div>
                    <br />
                    <br />
                    <h3>Current Receipt ID's with no Ack File</h3>
                    <asp:GridView ID="GvReceipts" runat="server" EmptyDataText="No submissions/receipts are currently available." AutoGenerateColumns="false">
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
                            <asp:TemplateField HeaderText="TYET ID" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="Lbl_gv_TYET" runat="server" Text='<%# Eval("tax_year_employer_transmissionID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="UTID" HeaderStyle-Width="400px" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="Lbl_gv_UTID" runat="server" Text='<%# Eval("UniqueTransmissionId") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Receipt" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="Lbl_gv_Receipt" runat="server" Text='<%# Eval("ReceiptID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Submission Date" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="Lbl_gv_SubmittedOn" runat="server" Text='<%# Eval("createdOn") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <h3>Available Acknowledgment Files</h3>
                    <asp:GridView ID="GvAckFiles" runat="server" AutoGenerateColumns="false" EmptyDataText="Currently no Acknowlegment files could be found." OnRowDeleting="GvAckFiles_RowDeleting">
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
                            <asp:TemplateField HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:ImageButton ID="ImgBtnDelete" runat="server" ImageUrl="~/images/close_box_red.png" CommandName="Delete" Height="20px" Width="20px" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="File Name" HeaderStyle-Width="500px" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="Lbl_gv_FileName" runat="server" Text='<%# Eval("NAME") %>'></asp:Label>
                                    <asp:HiddenField ID="Hf_gv_FilePath" runat="server" Value='<%# Eval("FullName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="File Type" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="Lbl_gv_FileType" runat="server" Text='<%# Eval("Extension") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Create Date" HeaderStyle-Width="300px" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="Lbl_gv_Created" runat="server" Text='<%# Eval("CreationTime") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>
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
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upImport" DynamicLayout="true" DisplayAfter="500">
                <ProgressTemplate>
                    <div style="position: fixed; top: 0; left: 0; background-color: white; width: 100%; height: 100%; opacity: .85; filter: alpha(opacity=85); -moz-opacity: 0.85; text-align: center;">
                        <div style="position: relative; margin-left: auto; margin-right: auto; background-color: white; padding-top: 100px;">
                            <h4>Processing the data..... This can take a few minutes.....</h4>
                            <asp:Image ID="ImgSearching" runat="server" ImageUrl="~/design/icon-loading-animated.gif" />
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>

        <div style="clear: both;">&nbsp;</div>
        <div id="footer">
            Copyright &copy; <%= Branding.CopyrightYears %> <a href="<%= Branding.CompanyWebSite %>"><%= Branding.CompanyName %></a> - All Rights Reserved   
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
