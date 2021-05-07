<%@ Page Title="" Language="C#" MasterPageFile="~/admin/AdminPortal/AdminPortal.Master" AutoEventWireup="true" CodeBehind="batch_management_insurance_carrier_imports.aspx.cs" Inherits="Afas.AfComply.UI.admin.batch_management_insurance_carrier_imports" %>
<%@ Import Namespace="Afas.AfComply.Domain" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" AsyncPostBackTimeout="3600"></asp:ToolkitScriptManager>
     <asp:UpdatePanel ID="UpBatch" runat="server">
                <ContentTemplate>
                    <div id="topbox">
                        <div id="tbleft">
                            <h4>Batch Import Removal</h4>
                            <asp:Panel ID="PnlEmployerSearch" runat="server" DefaultButton="BtnSearchEmployer">
                                <label style="width: 100px; float: left; text-align: right; font-size: 10px;">
                                    Filter Employer List:
                                </label>
                                <asp:TextBox ID="TxtEmployerSearch" runat="server" Width="125px"></asp:TextBox>
                                <asp:Button ID="BtnSearchEmployer" runat="server" Text="Filter" CssClass="btn" OnClientClick="this.focus()" OnClick="BtnSearchEmployer_Click" />
                                <br />
                                <label style="width: 100px; float: left; text-align: right; font-size: 10px;">
                                    Select Employer:
                                </label>
                                <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged" ></asp:DropDownList>
                            </asp:Panel>
                            <br />
                            <br />
                        </div>
                        <div id="tbright">
                            <h4>Please Read</h4>
                            <p>
                                Be very careful when deleting a Batch ID. This will remove all records related to that Batch ID and they are not recoverable!!!!
       
                            </p>
                        </div>
                    </div>
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


                    <br />
                    <br />
                    <h3>Last 100 Batch ID's Directory</h3>
                    <p>
                        If the Batch ID you are looking for isn't on this list, it is still available. Contact IT and they can change the value to show more records.
   
                    </p>
                    <asp:GridView ID="GvBatchFiles" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" Width="1024px" AutoGenerateColumns="false" OnRowDeleting="GvBatchFiles_RowDeleting">
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
                                    <asp:ImageButton ID="ImgBtnDelete" runat="server" ImageUrl="~/images/close_box_red.png" Height="20px" CommandName="Delete" ToolTip="Delete this file" />
                                    <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="ImgBtnDelete" ConfirmText="Are you sure you want to DELETE this file? All Insurance Carrier Data attached this batch number will be DELETED from the system!"></asp:ConfirmButtonExtender>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Batch Number" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="LblBatchID" runat="server" Text='<%# Eval("BATCH_ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Date Created" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="LblFileSize" runat="server" Text='<%# Eval("BATCH_MODON") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Created By" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="LblFileCreatedOn" runat="server" Text='<%# Eval("BATCH_MODBY") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Deleted On" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="LblFileDeletedOn" runat="server" Text='<%# Eval("BATCH_DELETED_ON") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Deleted By" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Label ID="LblFileDeletedBy" runat="server" Text='<%# Eval("BATCH_DELETED_BY") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
</asp:Content>
