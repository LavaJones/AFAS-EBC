<%@ Page EnableSessionState="ReadOnly" Title="Duplicate Employees" Language="C#" MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="mergeDuplicateEmployee.aspx.cs" Inherits="Afas.AfComply.UI.admin.AdminPortal.mergeDuplicateEmployee" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" ></asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="upBody" runat="server">
        <ContentTemplate>
   <div style="width: 1000px; margin-left: auto; margin-right: auto;">
            <div style="float: left; width: 495px;">
                <h4>Select Employer</h4>
                <asp:Panel ID="PnlEmployerSearch" runat="server" DefaultButton="BtnSearchEmployer">
                    <label style="width: 100px; float: left; text-align: right; font-size: 10px;">
                        Filter Employer List:
                    </label>
                    <asp:TextBox ID="TxtEmployerSearch" runat="server" Width="196px"></asp:TextBox>
                    <asp:Button ID="BtnSearchEmployer" runat="server" Text="Filter" CssClass="btn" OnClientClick="this.focus()" OnClick="BtnSearchEmployer_Click" />
                    <br />
                    <label style="width: 100px; float: left; text-align: right; font-size: 10px;">
                        Select Employer:
                    </label>
                    <asp:DropDownList ID="DdlEmployer" runat="server" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged" ></asp:DropDownList>
                </asp:Panel>
            </div>
            <div style="float: right; width: 495px;">
                <h4>Page Definition</h4>
                <p style="font-size: 10px;">
                    This page is intended for an administrative user to remove a duplicate employee record. The employee list on the LEFT hand side is the employee that will remain in the system. The employee listed on the RIGHT hand side will be removed from the system, however, the user does have the option to migrate some of the in-correct employee records over to the correct employee record if needed. 
                </p>
                <dl style="font-size: 10px;">
                    <dt>Delete a record</dt>
                    <dd>This means that the selected records will be removed from the system and will not be recoverable.</dd>
                    <dt>Migrate a record</dt>
                    <dd>This means that the selected records will be moved from the in-correct employee over the correct employee.</dd>
                    <dt>Power Merge (Delete)</dt>
                    <dd>This will Delete all of the in-correct employee data from the system in one click. It will not be recoverable.</dd>
                </dl>
            </div>
        </div>
   <br style="clear: both;" />
   <hr />
   <br />
  <div style="width: 1000px; margin-left: auto; margin-right: auto;">
            <div style="float: left; width: 495px;">
                <h4>
                    Select Employee<br />
                    <span style="font-size: 10px;">
                        *Note: This will filter both the left and right employee drop down lists.
                    </span>
                </h4>
                <asp:Panel ID="PnlSearchEmployeeLeft" runat="server" DefaultButton="BtnSearchEmployeeLeft">
                    <label style="width: 100px; float: left; text-align: right; font-size: 10px;">
                        Filter by Last Name:
                    </label>
                    <asp:TextBox ID="TxtSearchEmployeeNameLeft" runat="server" Width="196px"></asp:TextBox>
                    <asp:Button ID="BtnSearchEmployeeLeft" runat="server" Text="Filter" CssClass="btn" OnClientClick="this.focus();" OnClick="BtnSearchEmployeeLeft_Click"/>
                    <br />
                    <label style="width: 100px; float: left; text-align: right; font-size: 10px;">
                        Select Employee:
                    </label>
                    <asp:DropDownList ID="DdlEmployeeLeft" runat="server" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="DdlEmployeeLeft_SelectedIndexChanged"></asp:DropDownList>
                </asp:Panel>
            </div>
            <div style="float: right; width: 495px;">
                <h4>
                    Select Employee<br />
                    <span style="font-size: 10px;">
                        *Note: Secondary search incase employee has different last names.
                    </span>
                </h4>
                
                <asp:Panel ID="PnlSearchEmployeeRight" runat="server" DefaultButton="BtnSearchEmployeeRight">
                    <label style="width: 100px; float: left; text-align: right; font-size: 10px;">
                        Filter by Last Name:
                    </label>
                    <asp:TextBox ID="TxtSearchEmployeeNameRight" runat="server" Width="196px"></asp:TextBox>
                    <asp:Button ID="BtnSearchEmployeeRight" runat="server" Text="Filter" CssClass="btn" OnClientClick="this.focus();" OnClick="BtnSearchEmployeeRight_Click" />
                    <br />
                    <label style="width: 100px; float: left; text-align: right; font-size: 10px;">
                        Select Employee:
                    </label>
                    <asp:DropDownList ID="DdlEmployeeRight" runat="server" Width="200px" AutoPostBack="true" OnSelectedIndexChanged="DdlEmployeeRight_SelectedIndexChanged"></asp:DropDownList>
                </asp:Panel>
            </div>
        </div>
  <br style="clear: both;" />
  <hr />
  <br />
  <div style="background-color: lightgray; width: 1000px; margin-left: auto; margin-right: auto; padding: 5px 5px 0px 5px;">
            <h4 style="padding: 0 0 10px 0; margin: 0 0 0 0;">
                Data View - 
                <asp:LinkButton ID="LnkBtnPowerDelete" runat="server" Text="Power Merge (Delete)" OnClick="LnkBtnPowerDelete_Click"></asp:LinkButton>
                <asp:ConfirmButtonExtender ID="ConfirmButtonExtender7" TargetControlID="LnkBtnPowerDelete" runat="server" ConfirmText="Are you sure you want DELETE this employee from the ACA system? Once this employee is DELETED, they are not recoverable!"></asp:ConfirmButtonExtender>
            </h4>
            <br />
            <nav>
                <ul>
                    <li><asp:LinkButton ID="LnkBtnPayrollView" runat="server" CssClass="minPadding" Text="Payroll" OnClick="LnkBtnPayrollView_Click" ></asp:LinkButton></li>
                    <li><asp:LinkButton ID="LnkBtnInsuranceOfferView" runat="server" CssClass="minPadding" Text="Insurance Offer" OnClick="LnkBtnInsuranceOfferView_Click" ></asp:LinkButton></li>
                    <li><asp:LinkButton ID="LnkBtnInsuranceCarrierView" runat="server" CssClass="minPadding" Text="Carrier Data - Emp" OnClick="LnkBtnInsuranceCarrierView_Click" ></asp:LinkButton></li>
                    <li><asp:LinkButton ID="LnkBtnInsuranceCarrierDependentView" runat="server" CssClass="minPadding" Text="Carrier Data - Dep" OnClick="LnkBtnInsuranceCarrierDependentView_Click" ></asp:LinkButton></li>
                    <li><asp:LinkButton ID="LnkBtnInsuranceCarrierEditableView" runat="server" CssClass="minPadding" Text="Carrier Data - Editable" OnClick="LnkBtnInsuranceCarrierEditableView_Click" ></asp:LinkButton></li>
                    <li><asp:LinkButton ID="LnkBtnDependentView" runat="server" CssClass="minPadding" Text="Dependents" OnClick="LnkBtnDependentView_Click" ></asp:LinkButton></li>
                    <li><asp:LinkButton ID="LnkBtnEmployeeView" runat="server" CssClass="minPadding" Text="Employee" OnClick="LnkBtnEmployeeView_Click" ></asp:LinkButton></li>
                </ul>
            </nav>
            <br />
            <asp:MultiView ID="MvEmployeeDetails" runat="server" ActiveViewIndex="0">
                <asp:View ID="V_payroll" runat="server">
                    <div class="leftColumn">
                        <div style="float: left">
                            <h4>
                                (<asp:Literal ID="LitPayrollCorrectCount" runat="server"></asp:Literal>) - 
                                Payroll Records
                            </h4>
                        </div>
                        <asp:GridView ID="GvPayrollCorrect" runat="server" AutoGenerateColumns="False" Width="490px" CellPadding="4" ForeColor="#333333" GridLines="None" font-size="10px" EmptyDataText="No payroll records could be found." >
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" HeaderText="Start Date">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_p_startdate" runat="server" Text='<%# Bind("PAY_SDATE", "{0:MM-dd-yyyy}") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" HeaderText="End Date">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_p_enddate" runat="server" Text='<%# Bind("PAY_EDATE", "{0:MM-dd-yyyy}") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" HeaderText="Check Date">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_p_checkdate" runat="server" Text='<%# Bind("PAY_CDATE", "{0:MM-dd-yyyy}") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" HeaderText="Hours">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_p_hours" runat="server" Text='<%# Bind("PAY_HOURS") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                            </Columns>
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
                        </asp:GridView>
                        <br />
                    </div>
                    <div class="rightColumn">
                        <div style="float: left">
                            <h4>
                                (<asp:Literal ID="LitPayrollInCorrectCount" runat="server"></asp:Literal>) - 
                                Payroll Records
                            </h4>
                        </div>
                        <div style="float: right">
                            <asp:Button ID="BtnPayrollSave" runat="server" Text="Save" CssClass="btn" OnClick="BtnPayrollSave_Click" />
                            <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" TargetControlID="BtnPayrollSave" runat="server" ConfirmText="Are you sure you want process the PAYROLL data?"></asp:ConfirmButtonExtender>
                        </div>
                        <asp:GridView ID="GvPayrollIncorrect" runat="server" AutoGenerateColumns="False" Width="490px" CellPadding="4" Font-Size="10px" ForeColor="#333333" GridLines="None" EmptyDataText="No payroll records could be found." AllowSorting="true" OnSorting="GvPayrollIncorrect_Sorting">
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="40px" SortExpression="delete" HeaderText="Delete" ControlStyle-Height="12px">
                                    <ItemTemplate>
                                        <asp:RadioButton ID="Rb_gv_p_Delete" runat="server" CssClass="smallCheckBox" GroupName='<%# Bind("ROW_ID") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="40px" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="40px" SortExpression="migrate" HeaderText="Migrate">
                                    <ItemTemplate>
                                        <asp:RadioButton ID="Rb_gv_p_Migrate" runat="server" CssClass="smallCheckBox" GroupName='<%# Bind("ROW_ID") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="40px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="90px" HeaderText="Start Date">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_p_startdate" runat="server" Text='<%# Bind("PAY_SDATE", "{0:MM-dd-yyyy}") %>'></asp:Literal>
                                        <asp:HiddenField ID="hf_gv_p_rowid" runat="server" Value='<%# Bind("ROW_ID") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="90px" HeaderText="End Date">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_p_enddate" runat="server" Text='<%# Bind("PAY_EDATE", "{0:MM-dd-yyyy}") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="90px" HeaderText="Check Date">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_p_checkdate" runat="server" Text='<%# Bind("PAY_CDATE", "{0:MM-dd-yyyy}") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="90px" HeaderText="Hours">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_p_hours" runat="server" Text='<%# Bind("PAY_HOURS") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                </asp:TemplateField>
                            </Columns>
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
                        </asp:GridView>
                        <br />
                    </div>
                    <br style="clear: both;" />
                </asp:View>

                <asp:View ID="V_insuranceOffers" runat="server">
                    <div class="leftColumn">
                        <h4>
                            (<asp:Literal ID="LitInsuranceOfferCorrectCount" runat="server"></asp:Literal>) - 
                            Insurance Offer Records
                        </h4>
                        <asp:GridView ID="GvInsuranceOfferCorrect" runat="server" AutoGenerateColumns="False" Width="490px" CellPadding="4" ForeColor="#333333" GridLines="None" font-size="10px" EmptyDataText="No insurance offer records could be found." >
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="75px" HeaderText="Offer">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_io_offered" runat="server" Checked='<%# ProcessMyDataItem(Eval("IALERT_OFFERED")) %>' CssClass="smallCheckBox" Enabled="false" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="75px" HeaderText="Accept">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_io_accepted" runat="server" Checked='<%# ProcessMyDataItem(Eval("IALERT_ACCEPTED")) %>' CssClass="smallCheckBox" Enabled="false" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" HeaderText="Accep/Dec">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_io_accepted" runat="server" Text='<%# Bind("IALERT_ACCEPTEDDATE", "{0:MM-dd-yyyy}") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" HeaderText="Effec Date">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_io_effective" runat="server" Text='<%# Bind("IALERT_EFFECTIVE_DATE", "{0:MM-dd-yyyy}") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" HeaderText="Plan Year">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_io_plan_year_id" runat="server" Text='<%# Bind("IALERT_PLANYEARID") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                            </Columns>
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
                        </asp:GridView>
                        <br />
                        <h4>
                             (<asp:Literal ID="LitInsuranceChangeEventCorrectCount" runat="server"></asp:Literal>) - 
                            Insurance Change Events
                        </h4>
                        <asp:GridView ID="GvInsuranceOfferChangeEventCorrect" runat="server" AutoGenerateColumns="False" Width="490px" CellPadding="4" ForeColor="#333333" GridLines="None" font-size="10px" EmptyDataText="No insurance offer change events could be found." >
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="75px" HeaderText="Offer">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_cec_offered" runat="server" CssClass="smallCheckBox" Checked='<%# ProcessMyDataItem(Eval("IALERT_OFFERED")) %>' Enabled="false" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="75px" HeaderText="Accept">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_cec_accepted" runat="server" CssClass="smallCheckBox" Checked='<%# ProcessMyDataItem(Eval("IALERT_ACCEPTED")) %>' Enabled="false" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" HeaderText="Accep/Dec">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_cec_accepted" runat="server" Text='<%# Bind("IALERT_ACCEPTEDDATE", "{0:MM-dd-yyyy}") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" HeaderText="Effec Date">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_cec_effective" runat="server" Text='<%# Bind("IALERT_EFFECTIVE_DATE", "{0:MM-dd-yyyy}") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" HeaderText="Plan Year">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_cec_plan_year_id" runat="server" Text='<%# Bind("IALERT_PLANYEARID") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                            </Columns>
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
                        </asp:GridView>
                    </div>
                    <div class="rightColumn">
                        <div style="float: left">
                            <h4>
                                (<asp:Literal ID="LitInsuranceOfferInCorrectCount" runat="server"></asp:Literal>) - 
                                Insurance Offer Records
                            </h4>
                        </div>
                        <div style="float: right">
                            <asp:Button ID="BtnInsuranceOfferSave" runat="server" Text="Save" CssClass="btn" OnClick="BtnInsuranceOfferSave_Click"/>
                             <asp:ConfirmButtonExtender ID="ConfirmButtonExtender2" TargetControlID="BtnInsuranceOfferSave" runat="server" ConfirmText="Are you sure you want process the INSURANCE OFFER data?"></asp:ConfirmButtonExtender>
                        </div>
                        <asp:GridView ID="GvInsuranceOfferInCorrect" runat="server" AutoGenerateColumns="False" Width="490px" CellPadding="4" ForeColor="#333333" GridLines="None" font-size="10px" EmptyDataText="No insurance offer records could be found." AllowSorting="true" OnSorting="GvInsuranceOfferInCorrect_Sorting" >
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                 <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="40px" SortExpression="delete" HeaderText="Delete" >
                                    <ItemTemplate>
                                        <asp:RadioButton ID="Rb_gv_io_Delete" runat="server" GroupName="rb1" CssClass="smallCheckBox" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="40px" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="40px" SortExpression="migrate" HeaderText="Migrate">
                                    <ItemTemplate>
                                        <asp:RadioButton ID="Rb_gv_io_Migrate" runat="server" GroupName="rb1" CssClass="smallCheckBox" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="40px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="50px" HeaderText="Offer">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_io_offered" runat="server" Checked='<%# ProcessMyDataItem(Eval("IALERT_OFFERED")) %>' CssClass="smallCheckBox" Enabled="false" />
                                        <asp:HiddenField ID="Hf_gv_io_rowID" runat="server" Value='<%# Bind("ROW_ID") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="50px" HeaderText="Accept">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_io_accepted" runat="server" Checked='<%# ProcessMyDataItem(Eval("IALERT_ACCEPTED")) %>' CssClass="smallCheckBox" Enabled="false" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="75px" HeaderText="Acc/Dec">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_io_accepted" runat="server" Text='<%# Bind("IALERT_ACCEPTEDDATE", "{0:MM-dd-yyyy}") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="75px" HeaderText="Effec Date">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_io_effective" runat="server" Text='<%# Bind("IALERT_EFFECTIVE_DATE", "{0:MM-dd-yyyy}") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="75px" HeaderText="Plan Year">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_io_plan_year_id" runat="server" Text='<%# Bind("IALERT_PLANYEARID") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                </asp:TemplateField>
                            </Columns>
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
                        </asp:GridView>
                        <br />
                         <h4>
                             (<asp:Literal ID="LitInsuranceChangeEventInCorrectCount" runat="server"></asp:Literal>) - 
                            Insurance Change Events
                        </h4>
                        <span style="font-size: 8px">
                            *Note: Insurance Change Events will follow their parent Insurance Offer. 
                            <br />
                            Example 1: If the parent Insurance offer is deleted, all change events related to it will be deleted as well.
                            <br />
                            Example 2: If the parent Insurance offer is migrated, all change events related to it will be migrated as well. 
                        </span>
                        <asp:GridView ID="GvInsuranceOfferChangeEventInCorrect" runat="server" AutoGenerateColumns="False" Width="490px" CellPadding="4" ForeColor="#333333" GridLines="None" font-size="10px" EmptyDataText="No insurance offer change event could be found." AllowSorting="true" OnSorting="GvInsuranceOfferInCorrect_Sorting" >
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="50px" HeaderText="Offer">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_cec_offered" runat="server" CssClass="smallCheckBox" Checked='<%# ProcessMyDataItem(Eval("IALERT_OFFERED")) %>' Enabled="false" />
                                        <asp:HiddenField ID="Hf_gv_cec_rowID" runat="server" Value='<%# Bind("ROW_ID") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="50px" HeaderText="Accept">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_cec_accepted" runat="server" CssClass="smallCheckBox" Checked='<%# ProcessMyDataItem(Eval("IALERT_ACCEPTED")) %>' Enabled="false" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="75px" HeaderText="Acc/Dec">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_cec_accepted" runat="server" Text='<%# Bind("IALERT_ACCEPTEDDATE", "{0:MM-dd-yyyy}") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="75px" HeaderText="Effec Date">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_cec_effective" runat="server" Text='<%# Bind("IALERT_EFFECTIVE_DATE", "{0:MM-dd-yyyy}") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="75px" HeaderText="Plan Year">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_cec_plan_year_id" runat="server" Text='<%# Bind("IALERT_PLANYEARID") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                </asp:TemplateField>
                            </Columns>
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
                        </asp:GridView>
                    </div>
                    <br style="clear: both;" />
                </asp:View>

                <asp:View ID="V_insuranceCarrierData" runat="server">
                    <div class="leftColumn">
                         <h4>
                                (<asp:Literal ID="LitInsuranceCarrierRecordCountCorrect" runat="server"></asp:Literal>) - 
                                Insurance Carrier Records - (All)
                        </h4>
                        <asp:GridView ID="GvInsuranceCarrierDataCorrect" runat="server" AutoGenerateColumns="False" Width="490px" CellPadding="4" ForeColor="#333333" GridLines="None" font-size="10px" EmptyDataText="No insurance carrier records could be found." >
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="25px" HeaderText="Year">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_c_taxyear" runat="server" Text='<%# Bind("IC_TAX_YEAR") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="25px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" HeaderText="Name">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_c_name" runat="server" Text='<%# Bind("IC_FULL_NAME") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="1">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_c_Jan" runat="server" Enabled="false" CssClass="smallCheckBox" Checked='<%# Bind("IC_JAN") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="2">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_c_Feb" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_FEB") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="3">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_c_Mar" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_MAR") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="4">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_c_Apr" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_APR") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="5">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_c_May" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_MAY") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="6">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_c_Jun" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_JUN") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="7">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_c_Jul" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_JUL") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="8">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_c_Aug" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_AUG") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="9">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_c_Sep" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_SEP") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="10">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_c_Oct" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_OCT") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="11">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_c_Nov" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_NOV") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="12">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_c_Dec" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_DEC") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                            </Columns>
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
                        </asp:GridView>
                        <br />
                    </div>
                    <div class="rightColumn">
                         <div style="float: left">
                            <h4>
                                (<asp:Literal ID="LitInsuranceCarrierRecordCountInCorrect" runat="server"></asp:Literal>) - 
                                Insurance Carrier Records - (All)
                            </h4>
                        </div>
                        <div style="float: right">
                            <asp:Button ID="BtnInsuranceCarrierSave" runat="server" Text="Save" CssClass="btn" OnClick="BtnInsuranceCarrierSave_Click"/>
                             <asp:ConfirmButtonExtender ID="ConfirmButtonExtender3" TargetControlID="BtnInsuranceCarrierSave" runat="server" ConfirmText="Are you sure you want process the INSURANCE CARRIER - EMPLOYEE data?"></asp:ConfirmButtonExtender>
                        </div>
                        <asp:GridView ID="GvInsuranceCarrierDataInCorrect" runat="server" AutoGenerateColumns="False" Width="490px" CellPadding="4" ForeColor="#333333" GridLines="None" font-size="10px" EmptyDataText="No insurance carrier records could be found." AllowSorting="true" OnSorting="GvInsuranceCarrierDataInCorrect_Sorting" >
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                 <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="30px" SortExpression="delete" HeaderText="Delete" >
                                    <ItemTemplate>
                                        <asp:RadioButton ID="Rb_gv_ic_Delete" runat="server" CssClass="smallCheckBox" GroupName="rb1" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="30px" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="30px" SortExpression="migrate" HeaderText="Migrate">
                                    <ItemTemplate>
                                        <asp:RadioButton ID="Rb_gv_ic_Migrate" runat="server" CssClass="smallCheckBox" GroupName="rb1" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="30px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="25px" HeaderText="Year">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_ic_taxyear" runat="server" Text='<%# Bind("IC_TAX_YEAR") %>'></asp:Literal>
                                        <asp:HiddenField ID="Hf_gv_ic_rowid" runat="server" Value='<%# Bind("ROW_ID") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="25px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" HeaderText="Name">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_ic_name" runat="server" Text='<%# Bind("IC_FULL_NAME") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="1">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_ic_Jan" runat="server" Enabled="false" CssClass="smallCheckBox" Checked='<%# Bind("IC_JAN") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="2">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_ic_Feb" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_FEB") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="3">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_ic_Mar" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_MAR") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="12px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="4">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_ic_Apr" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_APR") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="12px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="5">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_ic_May" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_MAY") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="12px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="6">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_ic_Jun" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_JUN") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="12px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="7">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_ic_Jul" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_JUL") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="12px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="8">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_ic_Aug" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_AUG") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="12px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="9">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_ic_Sep" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_SEP") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="12px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="10">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_ic_Oct" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_OCT") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="12px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="11">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_ic_Nov" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_NOV") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="12px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="12">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_ic_Dec" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_DEC") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="12px" />
                                </asp:TemplateField>
                            </Columns>
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
                        </asp:GridView>
                        <br />
                    </div>
                    <br style="clear: both;" />
                </asp:View>

                <asp:View ID="V_insuranceCarrierDataDependent" runat="server">
                    <div style="width: 1000px; margin-left: auto; margin-right: auto">
                        This dependent's 
                        <asp:DropDownList ID="DdlInsuranceCarrierDependentsInCorrect" runat="server"></asp:DropDownList>
                        insurance carrier record  should be attached too
                        <asp:DropDownList ID="DdlInsuranceCarrierDependentsCorrect" runat="server"></asp:DropDownList> 
                        <asp:Button ID="BtnMergeInsuranceCarrierDependentRecord" runat="server" Text="Merge" CssClass="btn" OnClick="BtnMergeInsuranceCarrierDependentRecord_Click" />
                    </div>
                    <div class="leftColumn">
                         <h4>
                                (<asp:Literal ID="LitInsuranceCarrierDependentCorrectCount" runat="server"></asp:Literal>) - 
                                Insurance Carrier Dependent Records
                        </h4>
                        <asp:GridView ID="GvInsuranceCarrierDataDependentCorrect" runat="server" AutoGenerateColumns="False" Width="490px" CellPadding="4" ForeColor="#333333" GridLines="None" font-size="10px" EmptyDataText="No insurance carrier dependent records could be found." >
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="25px" HeaderText="Year">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_icd_taxyear" runat="server" Text='<%# Bind("IC_TAX_YEAR") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="25px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" HeaderText="Name">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_icd_name" runat="server" Text='<%# Bind("IC_FULL_NAME") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="1">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Jan" runat="server" Enabled="false" CssClass="smallCheckBox" Checked='<%# Bind("IC_JAN") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="2">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Feb" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_FEB") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="3">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Mar" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_MAR") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="4">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Apr" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_APR") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="5">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_May" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_MAY") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="6">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Jun" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_JUN") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="7">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Jul" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_JUL") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="8">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Aug" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_AUG") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="9">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Sep" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_SEP") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="10">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Oct" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_OCT") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="11">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Nov" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_NOV") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="12">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Dec" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_DEC") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                            </Columns>
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
                        </asp:GridView>
                        <br />
                    </div>
                    <div class="rightColumn">
                         <div style="float: left">
                            <h4>
                                (<asp:Literal ID="LitInsuranceCarrierDependentInCorrectCount" runat="server"></asp:Literal>) - 
                                Insurance Carrier Dependent Records
                            </h4>
                        </div>
                        <div style="float: right">
                            <asp:Button ID="BtnInsuranceCarrierDependentSave" runat="server" Text="Save" CssClass="btn" OnClick="BtnInsuranceCarrierDependentSave_Click" />
                            <asp:ConfirmButtonExtender ID="ConfirmButtonExtender4" TargetControlID="BtnInsuranceCarrierDependentSave" runat="server" ConfirmText="Are you sure you want process the INSURANCE CARRIER - DEPENDENT data?"></asp:ConfirmButtonExtender>
                        </div>
                        <asp:GridView ID="GvInsuranceCarrierDataDependentInCorrect" runat="server" AutoGenerateColumns="False" Width="490px" CellPadding="4" ForeColor="#333333" GridLines="None" font-size="10px" EmptyDataText="No insurance carrier dependent records could be found." AllowSorting="true" OnSorting="GvInsuranceCarrierDataDependentInCorrect_Sorting" >
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                 <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="30px" SortExpression="delete" HeaderText="Delete" >
                                    <ItemTemplate>
                                        <asp:RadioButton ID="Rb_gv_icd_Delete" runat="server" CssClass="smallCheckBox" GroupName="rb1" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="30px" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="30px" HeaderText="n/a">
                                    <ItemTemplate>
                                        <asp:RadioButton ID="Rb_gv_icd_Migrate" runat="server" CssClass="smallCheckBox" GroupName="rb1" Enabled="false" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="30px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="25px" HeaderText="Year">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_icd_taxyear" runat="server" Text='<%# Bind("IC_TAX_YEAR") %>'></asp:Literal>
                                        <asp:HiddenField ID="Hf_gv_icd_rowid" runat="server" Value='<%# Bind("ROW_ID") %>' />
                                        <asp:HiddenField ID="Hf_gv_icd_dependentID" runat="server" Value='<%# Bind("IC_DEPENDENT_ID") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="25px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" HeaderText="Name">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_icd_name" runat="server" Text='<%# Bind("IC_FULL_NAME") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="1">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Jan" runat="server" Enabled="false" CssClass="smallCheckBox" Checked='<%# Bind("IC_JAN") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="2">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Feb" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_FEB") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="3">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Mar" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_MAR") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="12px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="4">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Apr" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_APR") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="12px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="5">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_May" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_MAY") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="12px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="6">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Jun" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_JUN") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="12px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="7">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Jul" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_JUL") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="12px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="8">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Aug" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_AUG") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="12px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="9">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Sep" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_SEP") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="12px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="10">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Oct" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_OCT") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="12px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="11">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Nov" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_NOV") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="12px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="12">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Dec" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_DEC") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="12px" />
                                </asp:TemplateField>
                            </Columns>
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
                        </asp:GridView>
                        <br />
                    </div>
                    <br style="clear: both;" />
                </asp:View>

                <asp:View ID="V_insuranceCarrierDataEditable" runat="server">
                    <div class="leftColumn">
                         <h4>
                                (<asp:Literal ID="LitInsuranceCarrierEditableCorrectCount" runat="server"></asp:Literal>) - 
                                Insurance Carrier Editable Records (1095c Part III)
                        </h4>
                        <asp:GridView ID="GvInsuranceCarrierDataEditableCorrect" runat="server" AutoGenerateColumns="False" Width="490px" CellPadding="4" ForeColor="#333333" GridLines="None" font-size="10px" EmptyDataText="No insurance carrier dependent records could be found." >
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="25px" HeaderText="Year">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_icd_taxyear" runat="server" Text='<%# Bind("IC_TAX_YEAR") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="25px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" HeaderText="Name">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_icd_name" runat="server" Text='<%# Bind("IC_FULL_NAME") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="1">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Jan" runat="server" Enabled="false" CssClass="smallCheckBox" Checked='<%# Bind("IC_JAN") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="2">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Feb" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_FEB") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="3">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Mar" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_MAR") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="4">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Apr" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_APR") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="5">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_May" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_MAY") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="6">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Jun" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_JUN") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="7">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Jul" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_JUL") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="8">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Aug" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_AUG") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="9">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Sep" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_SEP") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="10">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Oct" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_OCT") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="11">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Nov" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_NOV") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="15px" HeaderText="12">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Dec" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_DEC") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                            </Columns>
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
                        </asp:GridView>
                        <br />
                    </div>
                    <div class="rightColumn">
                         <div style="float: left">
                            <h4>
                                (<asp:Literal ID="LitInsuranceCarrierEditableInCorrectCount" runat="server"></asp:Literal>) - 
                                Insurance Carrier Editable Records (1095c Part III)
                            </h4>
                        </div>
                        <div style="float: right">
                            <asp:Button ID="BtnInsuranceCarrierEditableSave" runat="server" Text="Save" CssClass="btn" OnClick="BtnInsuranceCarrierEditableSave_Click" />
                            <asp:ConfirmButtonExtender ID="ConfirmButtonExtender5" TargetControlID="BtnInsuranceCarrierEditableSave" runat="server" ConfirmText="Are you sure you want process the INSURANCE CARRIER - EDITABLE data?"></asp:ConfirmButtonExtender>
                        </div>
                        <asp:GridView ID="GvInsuranceCarrierDataEditableInCorrect" runat="server" AutoGenerateColumns="False" Width="490px" CellPadding="4" ForeColor="#333333" GridLines="None" font-size="10px" EmptyDataText="No insurance carrier editable records could be found." AllowSorting="true" OnSorting="GvInsuranceCarrierDataEditableInCorrect_Sorting" >
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                 <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="30px" SortExpression="delete" HeaderText="Delete" >
                                    <ItemTemplate>
                                        <asp:RadioButton ID="Rb_gv_icd_Delete" runat="server" CssClass="smallCheckBox" GroupName="rb1" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="30px" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="30px" HeaderText="n/a">
                                    <ItemTemplate>
                                        <asp:RadioButton ID="Rb_gv_icd_Migrate" runat="server" CssClass="smallCheckBox" GroupName="rb1" Enabled="false" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="30px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="25px" HeaderText="Year">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_icd_taxyear" runat="server" Text='<%# Bind("IC_TAX_YEAR") %>'></asp:Literal>
                                        <asp:HiddenField ID="Hf_gv_icd_rowid" runat="server" Value='<%# Bind("ROW_ID") %>' />
                                        <asp:HiddenField ID="Hf_gv_icd_dependentID" runat="server" Value='<%# Bind("IC_DEPENDENT_ID") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="25px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" HeaderText="Name">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_icd_name" runat="server" Text='<%# Bind("IC_FULL_NAME") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="1">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Jan" runat="server" Enabled="false" CssClass="smallCheckBox" Checked='<%# Bind("IC_JAN") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="2">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Feb" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_FEB") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="3">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Mar" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_MAR") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="12px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="4">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Apr" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_APR") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="12px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="5">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_May" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_MAY") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="12px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="6">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Jun" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_JUN") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="12px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="7">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Jul" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_JUL") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="12px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="8">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Aug" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_AUG") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="12px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="9">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Sep" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_SEP") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="12px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="10">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Oct" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_OCT") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="12px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="11">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Nov" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_NOV") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="12px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="12px" HeaderText="12">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Cb_gv_icd_Dec" runat="server" Enabled="false" CssClass="smallCheckBox"  Checked='<%# Bind("IC_DEC") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="12px" />
                                </asp:TemplateField>
                            </Columns>
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
                        </asp:GridView>
                        <br />
                    </div>
                    <br style="clear: both;" />
                </asp:View>

                <asp:View ID="V_dependents" runat="server">
                    <div class="leftColumn">
                        <h4>
                            (<asp:Literal ID="LitDependentsCorrectCount" runat="server"></asp:Literal>) - 
                            Dependents
                        </h4>
                        <asp:GridView ID="GvDependentsCorrect" runat="server" AutoGenerateColumns="False" Width="490px" CellPadding="4" ForeColor="#333333" GridLines="None" font-size="10px" EmptyDataText="No dependents could be found." >
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" HeaderText="Name">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_c_startdate" runat="server" Text='<%# Bind("DEPENDENT_FULL_NAME") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" HeaderText="SSN">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_c_enddate" runat="server" Text='<%# Bind("DEPENDENT_SSN_MASKED") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" HeaderText="DOB">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_c_checkdate" runat="server" Text='<%# Bind("DEPENDENT_DOB", "{0:MM-dd-yyyy}") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                            </Columns>
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
                        </asp:GridView>
                        <br />
                    </div>
                    <div class="rightColumn">
                        <div style="float: left">
                             <h4>
                                 (<asp:Literal ID="LitDependentsInCorrectCount" runat="server"></asp:Literal>) - 
                                Dependents
                            </h4>
                        </div>
                        <div style="float: right">
                            <asp:Button ID="BtnDependentsSave" runat="server" Text="Save" CssClass="btn" OnClick="BtnDependentsSave_Click"/>
                            <asp:ConfirmButtonExtender ID="ConfirmButtonExtender6" TargetControlID="BtnDependentsSave" runat="server" ConfirmText="Are you sure you want process the DEPENDENT data?"></asp:ConfirmButtonExtender>
                        </div>
                        
                        <asp:GridView ID="GvDependentsInCorrect" runat="server" AutoGenerateColumns="False" Width="490px" CellPadding="4" ForeColor="#333333" GridLines="None" font-size="10px" EmptyDataText="No dependents could be found." AllowSorting="true" OnSorting="GvDependentsInCorrect_Sorting" >
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                 <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="40px" HeaderText="Delete" ControlStyle-Height="12px" SortExpression="delete">
                                    <ItemTemplate>
                                        <asp:RadioButton ID="Rb_gv_d_Delete" runat="server" CssClass="smallCheckBox" GroupName="rb1" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="40px" />
                                </asp:TemplateField>
                                 <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="40px" HeaderText="Migrate" SortExpression="migrate">
                                    <ItemTemplate>
                                        <asp:RadioButton ID="Rb_gv_d_Migrate" runat="server" CssClass="smallCheckBox" GroupName="rb1" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="40px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" HeaderText="Name">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_d_fullname" runat="server" Text='<%# Bind("DEPENDENT_FULL_NAME") %>'></asp:Literal>
                                        <asp:HiddenField ID="Hf_gv_d_dependentid" runat="server" Value='<%# Bind("DEPENDENT_ID") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" HeaderText="SSN">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_d_ssn" runat="server" Text='<%# Bind("DEPENDENT_SSN_MASKED") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" HeaderText="DOB">
                                    <ItemTemplate>
                                        <asp:Literal ID="lit_gv_d_dob" runat="server" Text='<%# Bind("DEPENDENT_DOB", "{0:MM-dd-yyyy}") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                </asp:TemplateField>
                            </Columns>
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
                        </asp:GridView>
                        <br />
                    </div>
                    <br style="clear: both;" />
                </asp:View>

                <asp:View ID="V_employee" runat="server">
                    <div class="leftColumn">
                         <br style="clear: both;" />
                    </div>
                    <div class="rightColumn">
                        <h4>
                            Employee that will be removed from the database: <asp:Literal ID="LitEmployeeName" runat="server"></asp:Literal>
                        </h4>
                        <asp:Button ID="BtnDeleteEmployee" runat="server" Text="Remove Duplicate Employee" CssClass="btn" Width="250px" OnClick="BtnDeleteEmployee_Click" />
                        <asp:ConfirmButtonExtender ID="CbeDelete" TargetControlID="BtnDeleteEmployee" runat="server" ConfirmText="Are you sure you want DELETE this employee from the ACA system? Once this employee is DELETED, they are not recoverable!"></asp:ConfirmButtonExtender>
                        <br style="clear: both;" />
                    </div>
                    <br style="clear: both;" />
                </asp:View>
            </asp:MultiView>
            <br />
        </div>

        <asp:Panel ID="PnlMessage" runat="server">
            <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
            </div>
            <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                    <asp:ImageButton ID="ImgBtnMessageCancel" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" />
                </div>
                <h3>Webpage Message</h3>
                <p>
                    <asp:Literal ID="LitMessage" runat="server"></asp:Literal>
                </p>
            </div>
        </asp:Panel>
        <asp:HiddenField ID="HfDummyTrigger" runat="server" />
        <asp:ModalPopupExtender ID="MpeMessage" runat="server" TargetControlID="HfDummyTrigger" PopupControlID="PnlMessage"></asp:ModalPopupExtender>
        
        </ContentTemplate>
        <Triggers>

        </Triggers>
    </asp:UpdatePanel>
</asp:Content>

