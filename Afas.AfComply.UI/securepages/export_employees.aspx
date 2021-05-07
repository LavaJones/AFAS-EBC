<%@ Page Title="" Language="C#" MasterPageFile="~/securepages/SecurePages.Master" AutoEventWireup="true" CodeBehind="export_employees.aspx.cs" Inherits="securepages_export_employees" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="bodycontent" runat="server">
    <link rel="stylesheet" type="text/css" href="/Body.css" />
    <link rel="stylesheet" type="text/css" href="/leftnav.css" />

    <asp:HiddenField ID="HfUserName" runat="server" />
    <asp:HiddenField ID="HfDistrictID" runat="server" />
    <div id="content">
        <div style="padding-left: 60%;">
            <div style="width: 100%;">
                Export in <%= Branding.ProductName %> <% if (Feature.FullDataExportEnabled)
                                                          {%>extended <% } %>demographic format:
           
                        <asp:ImageButton ID="ImgBtnExport" runat="server" ToolTip="Export to .CSV file" Height="30px" ImageUrl="/design/csv-file-icon.png" OnClick="ImgBtnExport_Click" />
                <asp:ConfirmButtonExtender ID="CbeConfirmFileDownload" runat="server" TargetControlID="ImgBtnExport" ConfirmText="This file contains Social Security Numbers and I understand it must be handled with care."></asp:ConfirmButtonExtender>
            </div>
            <% if (Feature.EmployeeExportOfferFileEnabled)
                {%>

            <div style="width: 100%;">
                Export Offer File for all Employees in Stability Period: 
                        <asp:DropDownList ID="DdlPlanYear" runat="server"></asp:DropDownList>
                <asp:ImageButton ID="ImgBtnExportOffer" runat="server" ToolTip="Export all employees to the extended offer format as a .CSV file" Height="30px" ImageUrl="/design/csv-file-icon.png" OnClick="ImgBtnExportOffer_Click" />
                <asp:ConfirmButtonExtender ID="CbeConfirmOfferFileDownload" runat="server" TargetControlID="ImgBtnExportOffer" ConfirmText="<%# this.DisclaimerMessage %>"></asp:ConfirmButtonExtender>
            </div>

            <% } %>
            <% if (Feature.EmployeeExportCarrierFileEnabled)
                {%>
            <div style="width: 100%;">
                Export all employees in <%= Branding.ProductName %> extended carrier format:
           
                        <asp:ImageButton ID="ImgBtnExportCarrier" runat="server" ToolTip="Export all employees to the extended carrier format as a .CSV file" Height="30px" ImageUrl="/design/csv-file-icon.png" OnClick="ImgBtnExportCarrier_Click" />
                <asp:ConfirmButtonExtender ID="CbeConfirmCarrierFileDownload" runat="server" TargetControlID="ImgBtnExportCarrier" ConfirmText="This file contains Social Security Numbers and I understand it must be handled with care."></asp:ConfirmButtonExtender>
            </div>
            <% } %>
        </div>

        <div class="left_ebc">
            <%= demo.getLeftLinks(null != Session["CurrentDistrict"] && ((employer)Session["CurrentDistrict"]).IrsEnabled) %>
        </div>
        <div style='width: 50%; content: " "; display: table-cell'>
            <asp:UpdatePanel ID="upEmployee" runat="server" style="margin-left: 20%; position: absolute; top: 80px; width: 50%;">
                <ContentTemplate>
                    <asp:Panel ID="PnlFilter" runat="server" DefaultButton="BtnApplyFilters">
                        <br />
                        <h3 style="padding-left: 250px;">Employee - Mass Update / Export</h3>
                        <hr />
                        <h3 style="padding-left: 300px;">Filter By</h3>
                        <table style="width: 90%;">
                            <tr style="font-weight: bold;">
                                <td style="width: 20%">
                                    <asp:CheckBox ID="Cb_f_PayrollID" runat="server" Text="Employee #" AutoPostBack="true" OnCheckedChanged="Cb_f_PayrollID_CheckedChanged" />
                                    <br />
                                    <asp:TextBox ID="Txt_f_PayrollID" runat="server" Text="n/a" Enabled="false"></asp:TextBox>


                                </td>
                                <td style="width: 20%">
                                    <asp:CheckBox ID="Cb_f_LastName" runat="server" Text="Last Name" AutoPostBack="true" OnCheckedChanged="Cb_f_LastName_CheckedChanged" />
                                    <br />
                                    <asp:TextBox ID="Txt_f_LastName" runat="server" Text="n/a" Enabled="false"></asp:TextBox>


                                </td>
                                <td style="width: 20%">
                                    <asp:CheckBox ID="Cb_f_PayDesc" runat="server" Text="Pay Description or Pay Code" AutoPostBack="true" OnCheckedChanged="Cb_f_PayDesc_CheckedChanged" />
                                    <br />
                                    <asp:DropDownList ID="Ddl_f_PayDesc" runat="server" Width="90%" Enabled="false" CssClass="ddl2" AutoPostBack="true" OnSelectedIndexChanged="Ddl_f_PayDesc_SelectedIndexChanged"></asp:DropDownList>
                                    <br />
                                    <asp:DropDownList ID="Ddl_f_PayID" runat="server" Width="90%" Enabled="false" CssClass="ddl2" AutoPostBack="true" OnSelectedIndexChanged="Ddl_f_PayID_SelectedIndexChanged"></asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <br />

                                </td>
                            </tr>
                            <tr style="font-weight: bold;">

                                <td style="width: 20%">
                                    <asp:CheckBox ID="Cb_f_HrStatus" runat="server" Text="HR Status" AutoPostBack="true" OnCheckedChanged="Cb_f_HrStatus_CheckedChanged" />
                                    <br />
                                    <asp:DropDownList ID="Ddl_f_HrStatus" runat="server" Width="62%" Enabled="false" CssClass="ddl"></asp:DropDownList>


                                </td>
                                <td style="width: 20%">
                                    <asp:CheckBox ID="Cb_f_EmployeeClass" runat="server" Text="Employee Class" AutoPostBack="true" OnCheckedChanged="Cb_f_EmployeeClass_CheckedChanged" />
                                    <br />
                                    <asp:DropDownList ID="Ddl_f_EmployeeClass" runat="server" Width="62%" Enabled="false" CssClass="ddl"></asp:DropDownList>

                                </td>
                                <td style="width: 20%">
                                    <asp:CheckBox ID="Cb_f_AcaStatus" runat="server" Text="ACA Status" AutoPostBack="true" OnCheckedChanged="Cb_f_AcaStatus_CheckedChanged" />
                                    <br />
                                    <asp:DropDownList ID="Ddl_f_AcaStatus" runat="server" Width="90%" Enabled="false" CssClass="ddl"></asp:DropDownList>


                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Button ID="BtnApplyFilters" runat="server" Text="Apply Filters" CssClass="btn" Width="15%" OnClick="BtnApplyFilters_Click" />
                    </asp:Panel>
                    <br />
                    <hr />
                    <asp:Panel ID="PnlUpdate" runat="server" DefaultButton="BtnSave">
                        <h3 style="padding-left: 300px;">Fields to update</h3>
                        <table style="width: 69%;">
                            <tr style="font-weight: bold;">
                                <td style="width: 10%">
                                    <asp:CheckBox ID="Cb_u_EmployeeClass" runat="server" Text="Employee Class" AutoPostBack="true" OnCheckedChanged="Cb_u_EmployeeClass_CheckedChanged" />
                                    <br />
                                    <asp:DropDownList ID="Ddl_u_EmployeeClass" runat="server" Enabled="false" CssClass="ddl2"></asp:DropDownList>
                                </td>
                                <td style="width: 10%">
                                    <asp:CheckBox ID="Cb_u_ACAStatus" runat="server" Text="ACA Status" AutoPostBack="true" OnCheckedChanged="Cb_u_ACAStatus_CheckedChanged" />
                                    <br />
                                    <asp:DropDownList ID="Ddl_u_AcaStatus" runat="server" Enabled="false" CssClass="ddl2"></asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Button ID="BtnSave" runat="server" Text="Update ALL selected records" Width="25%" CssClass="btn" OnClick="BtnSave_Click" />
                        <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="BtnSave" ConfirmText="Are you sure you want to UPDATE the selected records?"></asp:ConfirmButtonExtender>
                        <br />
                        <br />
                        <asp:Button ID="BtnDelete" runat="server" Text="Delete ALL selected records" Width="25%" CssClass="btn" OnClick="BtnDelete_Click" />
                        <asp:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" TargetControlID="BtnDelete" ConfirmText="Are you sure you want to DELETE the selected records?"></asp:ConfirmButtonExtender>
                    </asp:Panel>
                    <br />
                    <hr />
                    <h3>Employee List</h3>
                    <i>You are viewing page
   
                            <%=GvEmployee.PageIndex + 1%>
    of
   
                            <%=GvEmployee.PageCount%>
                    </i>
                    <br />
                    <asp:CheckBox ID="CbCheckAll" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="CbCheckAll_CheckedChanged" />
                    &nbsp;&nbsp;&nbsp;&nbsp; 
Showing 
                        <asp:Literal ID="litAlertsShown" runat="server"></asp:Literal>
                    Employees of 
                        <asp:Literal ID="litAlertCount" runat="server"></asp:Literal>
                    <asp:GridView ID="GvEmployee" runat="server" AutoGenerateColumns="False" CellPadding="4" ForeColor="#eb0029" GridLines="None" AllowPaging="True" AllowSorting="True" PageSize="100" OnSorting="GvEmployee_Sorting" OnPageIndexChanging="GvEmployee_PageIndexChanging">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>

                                    <asp:CheckBox ID="Cb_gv_Selected" runat="server" Checked="false" />
                                </ItemTemplate>

                            </asp:TemplateField>
                            <asp:BoundField DataField="EMPLOYEE_EXT_ID" SortExpression="EMPLOYEE_EXT_ID" HeaderText="Employee #" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Left" />
                            <asp:TemplateField HeaderText="ID" HeaderStyle-Width="75px" SortExpression="EMPLOYEE_ID" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>

                                    <asp:Literal ID="LitEmployeeID" runat="server" Text='<%# Eval("EMPLOYEE_ID") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:BoundField DataField="EMPLOYEE_FULL_NAME" SortExpression="EMPLOYEE_FULL_NAME" HeaderText="Employee" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" />

                            <asp:BoundField DataField="EX_HR_STATUS_NAME" SortExpression="EX_HR_STATUS_NAME" HeaderText="HR Status" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" />

                            <asp:TemplateField HeaderText="Class Name" HeaderStyle-Width="150px" SortExpression="EX_CLASS_NAME" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>

                                    <asp:Literal ID="Lit_gv_ClassName" runat="server" Text='<%# Eval("EX_CLASS_NAME") %>'></asp:Literal>
                                    <asp:HiddenField ID="HfClassID" runat="server" Value='<%# Eval("EMPLOYEE_CLASS_ID") %>' />
                                </ItemTemplate>

                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="ACA Name" HeaderStyle-Width="150px" SortExpression="EX_ACA_NAME" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>

                                    <asp:Literal ID="LitAcaName" runat="server" Text='<%# Eval("EX_ACA_NAME") %>'></asp:Literal>

                                    <asp:HiddenField ID="HfAcaID" runat="server" Value='<%# Eval("EMPLOYEE_ACT_STATUS_ID") %>' />
                                </ItemTemplate>

                            </asp:TemplateField>

                        </Columns>
                        <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
                        <PagerSettings FirstPageImageUrl="~/design/first.png" Mode="NextPreviousFirstLast" LastPageImageUrl="/design/last.png" NextPageImageUrl="/design/next.png" PreviousPageImageUrl="/design/prev.png" PageButtonCount="25" Position="TopAndBottom" />
                        <PagerStyle BackColor="#eb0029" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EFF3FB" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#eb0029" />
                        <SortedAscendingHeaderStyle BackColor="#eb0029" />
                        <SortedDescendingCellStyle BackColor="#eb0029" />
                        <SortedDescendingHeaderStyle BackColor="#eb0029" />
                    </asp:GridView>
                    <asp:HiddenField ID="HfDummyTrigger" runat="server" />
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
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpEmployee" DynamicLayout="true" DisplayAfter="500">
        <ProgressTemplate>
            <div style="position: fixed; top: 0; left: 0; background-color: white; width: 100%; height: 100%; opacity: .85; filter: alpha(opacity=85); -moz-opacity: 0.85; text-align: center;">
                <div style="position: relative; margin-left: auto; margin-right: auto; background-color: white; padding-top: 100px;">
                    <h4>Processing your data..... This may take a minute.....</h4>
                    <asp:Image ID="ImgSearching" runat="server" ImageUrl="~/design/icon-loading-animated.gif" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <div style="clear: both;">
        &nbsp;
    </div>

</asp:Content>
