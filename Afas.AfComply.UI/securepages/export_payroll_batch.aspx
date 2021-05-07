<%@ Page Title="" Language="C#" MasterPageFile="~/securepages/SecurePages.Master" AutoEventWireup="true" CodeBehind="export_payroll_batch.aspx.cs" Inherits="securepages_export_payroll_batch" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="bodycontent" runat="server">

    <link rel="stylesheet" type="text/css" href="/Body.css" />
    <link rel="stylesheet" type="text/css" href="/leftnav.css" />
    <link rel="stylesheet" type="text/css" href="/leftnav.css" />

    <asp:HiddenField ID="HfUserName" runat="server" />
    <asp:HiddenField ID="HfDistrictID" runat="server" />
    <div style="float: right; padding-right: 318px;">
        Export:
           
                    <asp:ImageButton ID="ImgBtnExport" runat="server" ToolTip="Export to .CSV file" Height="30px" ImageUrl="/design/csv-file-icon.png" OnClick="ImgBtnExport_Click" />
    </div>
    <div class="left_ebc">
        <%= demo.getLeftLinks(null != Session["CurrentDistrict"] && ((employer)Session["CurrentDistrict"]).IrsEnabled) %>
    </div>

    <div id="content">
        <asp:UpdatePanel ID="upPayrollAlerts" runat="server" style="width: 50%; margin-left: 22%; position: absolute; top: 15px;">
            <ContentTemplate>
                <h3 style="padding-left: 228px;">Payroll Update by Batch ID / Export</h3>
                <hr />
                <asp:Panel ID="TempPanel" runat="server" DefaultButton="BtnApplyFilters">
                    <asp:Label ID="LblBatchID2" runat="server" Text="Select Batch ID"></asp:Label>
                    <asp:HiddenField ID="hfLastBatchID" runat="server" Value="0" />
                    <asp:DropDownList ID="Ddl_f_BatchID" runat="server" CssClass="ddl2"></asp:DropDownList>
                    <br />
                    <br />
                    <asp:Button ID="BtnViewBatchData" runat="server" Text="Show Batch Data" CssClass="btn" Width="20%" OnClick="BtnViewBatchData_Click" />
                    <hr />

                    <h3 style="padding-left: 310px;">Filter Batch Data</h3>
                    <table style="width: 100%;">

                        <tr style="font-weight: bold;">

                            <td style="width: 10%">
                                <asp:CheckBox ID="Cb_f_PayrollID" runat="server" Text="Employee #" AutoPostBack="true" OnCheckedChanged="Cb_f_PayrollID_CheckedChanged" />
                                <br />
                                <asp:TextBox ID="Txt_f_PayrollID" runat="server" Text="n/a" Enabled="false"></asp:TextBox>
                            </td>

                            <td style="width: 10%">
                                <asp:CheckBox ID="Cb_f_LastName" runat="server" Text="Last Name" AutoPostBack="true" OnCheckedChanged="Cb_f_LastName_CheckedChanged" />
                                <br />
                                <asp:TextBox ID="Txt_f_LastName" runat="server" Text="n/a" Enabled="false"></asp:TextBox>
                            </td>

                            <td style="width: 10%">
                                <asp:CheckBox ID="Cb_f_ActHours" runat="server" Text="Hours" AutoPostBack="true" OnCheckedChanged="Cb_f_ActHours_CheckedChanged" />
                                <br />
                                <asp:TextBox ID="Txt_f_Hours" runat="server" Text="n/a" Enabled="false"></asp:TextBox>
                            </td>

                            <td style="width: 10%">
                                <asp:CheckBox ID="Cb_f_sdate" runat="server" Text="Start Date" AutoPostBack="true" OnCheckedChanged="Cb_f_sdate_CheckedChanged" />
                                <br />
                                <asp:TextBox ID="Txt_f_Sdate" runat="server" Text="n/a" Enabled="false"></asp:TextBox>
                            </td>

                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                        <tr style="font-weight: bold;">
                            <td style="width: 10%">
                                <asp:CheckBox ID="Cb_f_edate" runat="server" Text="End Date" AutoPostBack="true" OnCheckedChanged="Cb_f_edate_CheckedChanged" />
                                <br />
                                <asp:TextBox ID="Txt_f_Edate" runat="server" Text="n/a" Enabled="false"></asp:TextBox>
                            </td>

                            <td style="width: 15%">
                                <asp:CheckBox ID="Cb_f_PayDesc" runat="server" Text="Pay Description" AutoPostBack="true" OnCheckedChanged="Cb_f_PayDesc_CheckedChanged" />
                                <br />
                                <asp:DropDownList ID="Ddl_f_PayDesc" runat="server" Width="71%" Enabled="false"></asp:DropDownList>
                            </td>

                            <td style="width: 11%">
                                <asp:CheckBox ID="Cb_f_CheckDate" runat="server" Text="Check Date" AutoPostBack="true" OnCheckedChanged="Cb_f_CheckDate_CheckedChanged" />
                                <br />
                                <asp:TextBox ID="Txt_f_Cdate" runat="server" Text="n/a" Enabled="false"></asp:TextBox>
                            </td>

                        </tr>

                    </table>

                    <br />

                    <asp:Button ID="BtnApplyFilters" runat="server" Text="Apply Filters" CssClass="btn" Width="15%" OnClick="BtnApplyFilters_Click" />
                </asp:Panel>


                <br />
                <hr />

                <asp:Panel ID="PnlUpdate" runat="server" DefaultButton="BtnSave">
                    <h3 style="padding-left: 310px;">Fields to update</h3>

                    <table style="width: 100%;">
                        <tr style="font-weight: bold;">
                            <td style="width: 10%">
                                <asp:CheckBox ID="Cb_u_ACTHours" runat="server" Text="Hours" AutoPostBack="true" OnCheckedChanged="Cb_u_ACTHours_CheckedChanged" />
                            </td>

                            <td style="width: 10%">
                                <asp:CheckBox ID="Cb_u_PayrollStart" runat="server" Text="Payroll Start Date" AutoPostBack="true" OnCheckedChanged="Cb_u_PayrollStart_CheckedChanged" />
                            </td>

                            <td style="width: 10%">
                                <asp:CheckBox ID="Cb_u_PayrollEnd" runat="server" Text="Payroll End Date" AutoPostBack="true" OnCheckedChanged="Cb_u_PayrollEnd_CheckedChanged" />
                            </td>

                        </tr>

                        <tr>
                            <td style="width: 10%">
                                <asp:TextBox ID="Txt_u_Hours" runat="server" Enabled="false" Text="n/a"></asp:TextBox>
                                <br />
                                Format: 25.25 = 25 1/4 hours
                            </td>

                            <td style="width: 10%">
                                <asp:TextBox ID="Txt_u_Start" runat="server" Enabled="false" Text="n/a"></asp:TextBox>
                                <br />
                                Format: mm/dd/yyyy
                            </td>

                            <td style="width: 10%">
                                <asp:TextBox ID="Txt_u_End" runat="server" Enabled="false" Text="n/a"></asp:TextBox>
                                <br />
                                Format: mm/dd/yyyy
                            </td>

                        </tr>

                    </table>
                    <br />

                    <asp:Button ID="BtnSave" runat="server" Text="Update ALL selected records" Width="25%" CssClass="btn" OnClick="BtnSave_Click" />

                    <br />

                    <br />
                </asp:Panel>
                <br />
                <hr />

                <h3>Current Payroll Data</h3>
                <i>You are viewing page
       
                            <%=GvPayrollData.PageIndex + 1%>
        of
       
                            <%=GvPayrollData.PageCount%>
                </i>
                <br />
                <asp:CheckBox ID="CbCheckAll" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="CbCheckAll_CheckedChanged" />
                &nbsp;&nbsp;&nbsp;&nbsp; 
    Showing 
   
                        <asp:Literal ID="litAlertsShown" runat="server"></asp:Literal>
                Payroll records of 
   
                        <asp:Literal ID="litAlertCount" runat="server"></asp:Literal>
                <asp:GridView ID="GvPayrollData" runat="server" AutoGenerateColumns="False" OnPageIndexChanging="GvPayrollData_PageIndexChanging" OnSorting="GvPayrollData_Sorting" CellPadding="4" ForeColor="#333333" GridLines="None" AllowPaging="True" AllowSorting="True" PageSize="100">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:TemplateField HeaderText="" HeaderStyle-Width="30px" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:CheckBox ID="Cb_gv_Selected" runat="server" Checked="false" />
                                <asp:HiddenField ID="Hf_gv_RowID" runat="server" Value='<%# Eval("ROW_ID") %>' />
                                <asp:HiddenField ID="Hf_gv_EmployeeID" runat="server" Value='<%# Eval("PAY_EMPLOYEE_ID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="PAY_EMPLOYEE_EXT_ID" HeaderText="Payroll ID" SortExpression="PAY_EMPLOYEE_EXT_ID" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="PAY_F_NAME" HeaderText="First Name" SortExpression="PAY_F_NAME" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="75px"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="PAY_M_NAME" HeaderText="Middle Name" SortExpression="PAY_M_NAME" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="75px"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="PAY_L_NAME" HeaderText="Last Name" SortExpression="PAY_L_NAME" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="125px"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="PAY_HOURS" HeaderText="Hours" SortExpression="PAY_HOURS" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="75px"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="PAY_SDATE" HeaderText="Start Date" DataFormatString="{0:d}" SortExpression="PAY_SDATE" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="70px"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="PAY_EDATE" HeaderText="End Date" DataFormatString="{0:d}" SortExpression="PAY_EDATE" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="70px"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="PAY_GP_NAME" HeaderText="Pay Description" SortExpression="PAY_GP_NAME" HeaderStyle-Width="125px" HeaderStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="150px"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="PAY_GP_EXT_ID" HeaderText="Pay Code" SortExpression="PAY_GP_EXT_ID" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="50px"></HeaderStyle>
                        </asp:BoundField>
                        <asp:BoundField DataField="PAY_CDATE" HeaderText="Check Date" DataFormatString="{0:d}" SortExpression="PAY_CDATE" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Left">
                            <HeaderStyle Width="70px"></HeaderStyle>
                        </asp:BoundField>
                    </Columns>
                    <EditRowStyle BackColor="#2461BF" />
                    <FooterStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
                    <PagerSettings FirstPageImageUrl="~/design/first.png" Mode="NextPreviousFirstLast" LastPageImageUrl="/design/last.png" NextPageImageUrl="/design/next.png" PreviousPageImageUrl="/design/prev.png" PageButtonCount="25" Position="TopAndBottom" />
                    <PagerStyle BackColor="#eb0029" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#EFF3FB" />
                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                    <SortedAscendingHeaderStyle BackColor="#eb0029" />
                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
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
        <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upPayrollAlerts" DynamicLayout="true" DisplayAfter="500">
            <ProgressTemplate>
                <div style="position: fixed; top: 0; left: 0; background-color: white; width: 100%; height: 100%; opacity: .85; filter: alpha(opacity=85); -moz-opacity: 0.85; text-align: center;">
                    <div style="position: relative; margin-left: auto; margin-right: auto; background-color: white; padding-top: 100px;">
                        <h4>Processing your data..... This may take a minute.....</h4>
                        <asp:Image ID="ImgSearching" runat="server" ImageUrl="~/design/icon-loading-animated.gif" />
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </div>
    <div style="clear: both;">
        &nbsp;
    </div>
</asp:Content>
