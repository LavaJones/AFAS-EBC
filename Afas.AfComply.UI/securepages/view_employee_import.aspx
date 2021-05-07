<%@ Page Title="" Language="C#" MasterPageFile="~/securepages/SecurePages.Master" AutoEventWireup="true" CodeBehind="view_employee_import.aspx.cs" Inherits="securepages_view_employee_import" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="bodycontent" runat="server">
    <link rel="stylesheet" type="text/css" href="/Body.css" />
    <link rel="stylesheet" type="text/css" href="/leftnav.css" />

    <asp:HiddenField ID="HfDistrictID" runat="server" />

    <div class="left_ebc">
        <%= demo.getLeftLinks(null != Session["CurrentDistrict"] && ((employer)Session["CurrentDistrict"]).IrsEnabled) %>
    </div>
    <div id="content">
        <asp:UpdatePanel ID="upPayrollAlerts" runat="server">

            <ContentTemplate>

                <div class="main-content" style="margin-left: 415px; width: 50%; position: absolute; top: 5px;">
                    <h3 style="padding-left: 25%;">Employee Alert - Mass Update</h3>
                    <hr />
                    <asp:Panel ID="TempPanel" runat="server" DefaultButton="BtnApplyFilters">

                        <h3 style="padding-left: 36%;">Filter By</h3>

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
                                    <asp:CheckBox ID="Cb_f_HrDesc" runat="server" Text="HR Status Description" AutoPostBack="true" OnCheckedChanged="Cb_f_HrDesc_CheckedChanged" />
                                    <br />
                                    <asp:DropDownList ID="Ddl_f_HrDesc" runat="server" Width="74%" Enabled="false"></asp:DropDownList>
                                </td>

                            </tr>

                        </table>

                        <br />

                        <asp:Button ID="BtnApplyFilters" runat="server" Text="Apply Filters" CssClass="btn" Width="11%" OnClick="BtnApplyFilters_Click" />

                    </asp:Panel>

                    <br />
                    <hr />
                    <asp:Panel ID="PnlUpdate" runat="server" DefaultButton="BtnSave">

                        <h3 style="padding-left: 35%;">Fields to update</h3>

                        <table style="width: 100%;">

                            <tr style="font-weight: bold;">

                                <td style="width: 10%">
                                    <asp:CheckBox ID="Cb_u_HRstatus" runat="server" Text="HR Status" AutoPostBack="true" OnCheckedChanged="Cb_u_HRstatus_CheckedChanged" />
                                    <br />
                                    <asp:DropDownList ID="Ddl_u_HrStatus" runat="server" Width="52%" Enabled="false" CssClass="ddl"></asp:DropDownList>

                                </td>
                                <td style="width: 10%">
                                    <asp:CheckBox ID="Cb_u_EmployeeClass" runat="server" Text="Employee Class" AutoPostBack="true" OnCheckedChanged="Cb_u_EmployeeClass_CheckedChanged" />
                                    <br />
                                    <asp:DropDownList ID="Ddl_u_EmployeeClass" runat="server" Width="52%" Enabled="false" CssClass="ddl"></asp:DropDownList>

                                </td>

                                <td style="width: 10%">
                                    <asp:CheckBox ID="Cb_u_ACAstatus" runat="server" Text="ACA Status" AutoPostBack="true" OnCheckedChanged="Cb_u_ACAstatus_CheckedChanged" />
                                    <br />
                                    <asp:DropDownList ID="Ddl_u_AcaStatus" runat="server" Width="70%" Enabled="false" CssClass="ddl"></asp:DropDownList>

                                </td>
                            </tr>

                            <tr>
                            </tr>

                            <tr style="font-weight: bold;">
                                <td style="width: 10%">
                                    <asp:CheckBox ID="Cb_u_HireDate" runat="server" Text="Hire Date" AutoPostBack="true" OnCheckedChanged="Cb_u_HireDate_CheckedChanged" />
                                    <br />
                                    <asp:TextBox ID="Txt_u_HireDate" runat="server" Enabled="false" Text="n/a"></asp:TextBox>
                                    <br />
                                    Format: mm/dd/yyyy

                                </td>
                                <td style="width: 10%">
                                    <asp:CheckBox ID="Cb_u_TermDate" runat="server" Text="Term Date" AutoPostBack="true" OnCheckedChanged="Cb_u_TermDate_CheckedChanged" />
                                    <br />
                                    <asp:TextBox ID="Txt_u_TermDate" runat="server" Enabled="false" Text="n/a"></asp:TextBox>
                                    <br />
                                    Format: mm/dd/yyyy

                                </td>
                                <td style="width: 10%">
                                    <asp:CheckBox ID="Cb_u_PlanYear" runat="server" Text="Stability Period" AutoPostBack="true" OnCheckedChanged="Cb_u_PlanYear_CheckedChanged" />
                                    <br />
                                    <asp:DropDownList ID="Ddl_u_PlanYear" runat="server" Width="71%" Enabled="false" CssClass="ddl"></asp:DropDownList>
                                </td>

                            </tr>
                        </table>

                        <br />
                        <asp:Button ID="BtnSave" runat="server" Text="Update ALL selected records" Width="22%" CssClass="btn" OnClick="BtnSave_Click" />
                        <br />
                        <br />
                        <asp:Button ID="BtnDelete" runat="server" Text="Delete ALL selected records" Width="22%" CssClass="btn" OnClick="BtnDelete_Click" />

                        <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="BtnDelete" ConfirmText="Are you sure you want to DELETE all of the selected records? They are not able to be recovered once you DELETE them."></asp:ConfirmButtonExtender>

                    </asp:Panel>

                    <br />
                    <hr />
                    <h3>Current Employee/Demographic Alerts</h3>
                    <i>You are viewing page
                            <%=GvEmployeeData.PageIndex + 1%>
        of
                            <%=GvEmployeeData.PageCount%>
                    </i>
                    <br />
                    <asp:CheckBox ID="CbCheckAll" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="CbCheckAll_CheckedChanged" />
                    &nbsp;&nbsp;&nbsp;&nbsp; Showing 
                        <asp:Literal ID="litAlertsShown" runat="server"></asp:Literal>
                    Payroll Alerts of 
                        <asp:Literal ID="litAlertCount" runat="server"></asp:Literal>
                    <asp:GridView ID="GvEmployeeData" runat="server" AutoGenerateColumns="False" OnPageIndexChanging="GvEmployeeData_PageIndexChanging" OnSorting="GvEmployeeData_Sorting" CellPadding="4" ForeColor="#333333" GridLines="None" AllowPaging="True" AllowSorting="True" PageSize="100">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField HeaderText="" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:CheckBox ID="Cb_gv_Selected" runat="server" Checked="false" />
                                    <asp:HiddenField ID="Hf_gv_RowID" runat="server" Value='<%# Eval("ROW_ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="EMPLOYEE_EXT_ID" HeaderText="Employee #" SortExpression="EMPLOYEE_EXT_ID" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                            <asp:BoundField DataField="EMPLOYEE_FIRST_NAME" HeaderText="First Name" SortExpression="EMPLOYEE_FIRST_NAME" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                            <asp:BoundField DataField="EMPLOYEE_LAST_NAME" HeaderText="Last Name" SortExpression="EMPLOYEE_LAST_NAME" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                            <asp:BoundField DataField="EMPLOYEE_I_HIRE_DATE" HeaderText="Hire Date" SortExpression="EMPLOYEE_I_HIRE_DATE" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                            <asp:BoundField DataField="EMPLOYEE_I_TERM_DATE" HeaderText="Term Date" SortExpression="EMPLOYEE_I_TERM_DATE" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                            <asp:BoundField DataField="EMPLOYEE_HR_EXT_DESCRIPTION" HeaderText="HR Status Desc." SortExpression="EMPLOYEE_HR_EXT_STATUS_DESCRIPTION" HeaderStyle-Width="125px" HeaderStyle-HorizontalAlign="Left"></asp:BoundField>
                            <asp:TemplateField HeaderText="Class ID" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Literal ID="Lit_gv_ClassID" runat="server" Text='<%# Eval("EMPLOYEE_CLASS_ID") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status ID" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Literal ID="Lit_gv_AcaID" runat="server" Text='<%# Eval("EMPLOYEE_ACT_STATUS_ID") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Stability Period ID" HeaderStyle-Width="125px" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:Literal ID="Lit_gv_PlanYearID" runat="server" Text='<%# Eval("EMPLOYEE_PLAN_YEAR_ID_MEAS") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
                        <PagerSettings FirstPageImageUrl="~/design/first.png" Mode="NextPreviousFirstLast" LastPageImageUrl="/design/last.png" NextPageImageUrl="/design/next.png" PreviousPageImageUrl="/design/prev.png" PageButtonCount="25" Position="TopAndBottom" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EFF3FB" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#4870BE" />

                    </asp:GridView>

                    <asp:HiddenField ID="HfDummyTrigger" runat="server" />

                    <asp:Panel ID="PnlMessage" runat="server">

                        <div style="position: fixed; top: 0; left: 0; background-color: gray; opacity: 0.8; width: 100%; height: 100%;">
                        </div>
                        <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; width: 600px; height: auto; max-height: 500px; margin-top: 0px; font-size: 12px; background-color: white; z-index: 10;">
                            <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                <asp:ImageButton ID="ImgBtnClose" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" BorderStyle="None" />
                            </div>

                            <div style="overflow-x: hidden; overflow-y: scroll; max-height: 400px; background-color: white;">
                                <h3 style="color: black;">Webpage Message</h3>
                                <p style="color: black">
                                    <asp:Literal ID="LitMessage" runat="server"></asp:Literal>
                                </p>
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:ModalPopupExtender ID="MpeWebMessage" runat="server" TargetControlID="HfDummyTrigger" OkControlID="ImgBtnClose" PopupControlID="PnlMessage"></asp:ModalPopupExtender>
                </div>
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

        <div style="clear: both;">
            &nbsp;
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

