<%@ Page EnableSessionState="ReadOnly" Title="Employer Measurement Periods" Language="C#"
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="EmployerMeasurementPeriods.aspx.cs"
    Inherits="Afas.AfComply.UI.admin.AdminPortal.EmployerMeasurementPeriods" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <link rel="stylesheet" type="text/css" href="/_js/jquery-ui-themes-1.10.2/themes/ui-lightness/jquery-ui.css" />


    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>

    <h2>Edit Measurement Periods</h2>
    <br />
    <label class="lbl3">Select Employer</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    <br />
    <asp:Label ID="cofein" runat="server"></asp:Label>
    <br />

    <asp:HiddenField ID="HfDistrictID" runat="server" />
    <asp:HiddenField ID="HfEmployerTypeID" runat="server" />
    <asp:HiddenField ID="HfUserName" runat="server" />

    <div>
        <asp:Panel ID="PnlMeasurement" runat="server">
            <asp:ModalPopupExtender ID="MpeMeasurementPeriod" runat="server" PopupControlID="PnlNewMeasurement" TargetControlID="BtnSaveMeasurmentPeriod"></asp:ModalPopupExtender>
            <asp:Panel ID="PnlNewMeasurement" runat="server" DefaultButton="BtnSaveMeasurmentPeriod">
                <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                </div>
                <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                    <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                        <asp:ImageButton ID="ImageButton2" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" />
                    </div>
                    <h3>Step 1: Choose Stability Period</h3>
                    <label class="lbl4">Stability Period</label>
                    <asp:DropDownList ID="Ddl_M_PlanYear" runat="server" CssClass="ddl" Width="310px" AutoPostBack="true" OnSelectedIndexChanged="Ddl_M_PlanYear_SelectedIndexChanged"></asp:DropDownList>
                    <br />
                    <h3>Step 2: Choose Employee Type</h3>
                    <label class="lbl4">Employee Type</label>
                    <asp:DropDownList ID="Ddl_M_EmployeeType" runat="server" CssClass="ddl" Width="310px"></asp:DropDownList>
                    <br />
                    <h3>Step 3: Choose Measurement Type</h3>
                    <label class="lbl4">Measurement Type</label>
                    <asp:DropDownList ID="Ddl_M_MeasurementType" runat="server" CssClass="ddl" Width="310px"></asp:DropDownList>
                    <br />
                    <h3>Step 4: Measurement Periods</h3>
                    <br />
                    <label class="lbl4">Measurement Period:</label>
                    <asp:TextBox ID="Txt_M_Meas_start" runat="server" CssClass="txt" Width="75px"></asp:TextBox>
                    to
                       
                                        <asp:TextBox ID="Txt_M_Meas_end" runat="server" CssClass="txt" Width="75px"></asp:TextBox>

                    <br />
                    <label class="lbl4">Administrative Period:</label>
                    <asp:TextBox ID="Txt_M_Admin_start" runat="server" CssClass="txt" Width="75px"></asp:TextBox>
                    to
                       
                                        <asp:TextBox ID="Txt_M_Admin_end" runat="server" CssClass="txt" Width="75px"></asp:TextBox>
                    <br />
                    <label class="lbl4">Open Enrollment:</label>
                    <asp:TextBox ID="Txt_M_Open_start" runat="server" CssClass="txt" Width="75px"></asp:TextBox>
                    to
                       
                                        <asp:TextBox ID="Txt_M_Open_end" runat="server" CssClass="txt" Width="75px"></asp:TextBox>
                    <br />
                    <label class="lbl4">Stability Period:</label>
                    <asp:TextBox ID="Txt_M_Stab_start" runat="server" CssClass="txt" Width="75px"></asp:TextBox>
                    to
                       
                                        <asp:TextBox ID="Txt_M_Stab_end" runat="server" CssClass="txt" Width="75px"></asp:TextBox>
                    <br />

                    <asp:Panel ID="PnlSummerWindow" runat="server" Visible="false">
                        <h3>Step 5: Breaks In Service</h3>
                        <label class="lbl">New Break:</label>
                        <asp:TextBox ID="TxtSummerBreakStart" runat="server" CssClass="txt" Width="75px"></asp:TextBox>
                        to                       
                                            <asp:TextBox ID="TxtSummerBreakEnd" runat="server" CssClass="txt" Width="75px"></asp:TextBox>
                        <asp:Button ID="AddNewTempBreak" CssClass="btn" Text="Add" runat="server" OnClick="AddNewTempBreak_Click" />
                        <br />

                        <asp:GridView ID="Gv_TempBreakOfService" runat="server" AutoGenerateColumns="false"
                            EmptyDataText="There are currently NO breaks in service." BackColor="White" BorderColor="#336666"
                            BorderStyle="Double" BorderWidth="3px" CellPadding="4" GridLines="Horizontal"
                            OnRowDeleting="Gv_TempBreakOfService_RowDeleting"
                            OnRowEditing="Gv_TempBreakOfService_RowEditing"
                            OnRowCancelingEdit="Gv_TempBreakOfService_RowCancelingEdit"
                            OnRowUpdating="Gv_TempBreakOfService_RowUpdating">
                            <FooterStyle BackColor="White" ForeColor="#333333" />
                            <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="White" ForeColor="#333333" Height="30px" />
                            <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                            <SortedAscendingHeaderStyle BackColor="#487575" />
                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                            <SortedDescendingHeaderStyle BackColor="#275353" />
                            <Columns>
                                <asp:TemplateField HeaderText="Edit">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImgBtnEdit" runat="server" CommandName="Edit" ImageUrl="~/images/edit_notes.png" Height="25px" Style="float: right;" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton ID="ImgBtnUpdate" runat="server" CommandName="Update" ImageUrl="~/images/disk-save.png" Height="25px" />
                                        <asp:ImageButton ID="ImgBtnCancel" runat="server" CommandName="Cancel" ImageUrl="~/images/error.png" Height="25px" />
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Start Date" HeaderStyle-Width="125px" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="HiddenBreakId" runat="server" Value='<%# Eval("BreakInServiceId") %>' />
                                        <asp:Literal ID="TxtSummerBreakStart" runat="server" Text='<%# Eval("StartDate", "{0:MM/dd/yyyy}") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:HiddenField ID="HiddenBreakId" runat="server" Value='<%# Eval("BreakInServiceId") %>' />
                                        <asp:TextBox ID="TxtSummerBreakStart" runat="server" Text='<%# Eval("StartDate", "{0:MM/dd/yyyy}") %>' CssClass="txt" Width="75px"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="End Date" HeaderStyle-Width="125px" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Literal ID="TxtSummerBreakEnd" runat="server" Text='<%# Eval("EndDate", "{0:MM/dd/yyyy}") %>'></asp:Literal>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="TxtSummerBreakEnd" runat="server" Text='<%# Eval("EndDate", "{0:MM/dd/yyyy}") %>' CssClass="txt" Width="75px"></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Weeks" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Literal ID="Weeks" runat="server" Text='<%# Eval("Weeks") %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderStyle-Width="25px" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:ImageButton Width="25px" ID="ImgBtnDelete" runat="server" ImageUrl="~/images/close_box_red.png" Height="20px" CommandName="Delete" ToolTip="Delete this Break In Service?" />
                                        <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="ImgBtnDelete" ConfirmText="Are you sure you want to DELETE this Break In Service?"></asp:ConfirmButtonExtender>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                    <br />
                    <label class="lbl4">.</label>
                    <asp:Button ID="BtnSaveMeasurementPeriod" runat="server" CssClass="btn" Text="Submit" OnClick="BtnSaveMeasurementPeriod_Click" />
                    <br />
                    <asp:Label ID="Lbl_M_message" runat="server" BackColor="Yellow"></asp:Label>
                    <br />
                    <br />
                </div>
            </asp:Panel>
            <h3>Measurement Periods</h3>
            <asp:Button ID="BtnSaveMeasurmentPeriod" runat="server" CssClass="btn" Text="New" />
            <br />
            * Measurement periods are specific to each Stability Period &amp; EMPLOYEE TYPE. If you have multiple Stability Periods or EMPLOYEE TYPES, use the dropdown boxes to view those measurement periods.
       
         <br />
            <label class="lbl4">Stability Period: </label>
            <asp:DropDownList ID="DdlPlanYear" runat="server" CssClass="ddl2" AutoPostBack="True" OnSelectedIndexChanged="DdlPlanYear_SelectedIndexChanged"></asp:DropDownList>

                                <br />
            <label class="lbl4">Employee Type</label>
            <asp:DropDownList ID="DdlEmployeeType" runat="server" CssClass="ddl2" AutoPostBack="true" OnSelectedIndexChanged="DdlEmployeeType_SelectedIndexChanged" onChange="init()"></asp:DropDownList>

            <asp:Panel ID="PnlTransitionMeasurement" runat="server">
                <asp:Repeater ID="RptTransitionMeasurments" runat="server" OnItemDataBound="RptTransitionMeasurments_ItemDataBound" OnItemCommand="RptTransitionMeasurments_ItemCommand">
                    <ItemTemplate>
                        <h3>
                            <asp:Literal ID="LitMeasurementType" runat="server"></asp:Literal>
                            <asp:ImageButton ID="ImgBtnHistoryTransitionMeasurement" runat="server" ImageUrl="~/images/history.png" Height="25px" Style="padding: 5px;" />
                            <asp:ImageButton ID="ImgBtnEditTransitionMeasurement" runat="server" ImageUrl="~/images/edit_notes.png" Height="25px" Style="padding: 5px;" />
                        </h3>
                        <asp:HiddenField ID="Hf_M_MeasurementID" runat="server" Value='<%# Eval("MEASUREMENT_ID") %>' />
                        <asp:HiddenField ID="Hf_M_MeasurementTypeID" runat="server" Value='<%# Eval("MEASUREMENT_TYPE_ID") %>' />
                        <label class="lbl4">Measurement Period :</label>
                        <asp:TextBox ID="Txt_tra_meas_start" runat="server" CssClass="txt2" Enabled="false" Text='<%# Eval("MEASUREMENT_START", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                        to
       
                                            <asp:TextBox ID="Txt_tra_meas_end" runat="server" CssClass="txt2" Enabled="false" Text='<%# Eval("MEASUREMENT_END", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                        <br />
                        <label class="lbl4">Administrative Period :</label>
                        <asp:TextBox ID="Txt_tra_admin_start" runat="server" CssClass="txt2" Enabled="false" Text='<%# Eval("MEASUREMENT_ADMIN_START", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                        to
       
                                            <asp:TextBox ID="Txt_tra_admin_end" runat="server" CssClass="txt2" Enabled="false" Text='<%# Eval("MEASUREMENT_ADMIN_END", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                        <br />
                        <label class="lbl4">Open Enrollment :</label>
                        <asp:TextBox ID="Txt_tra_open_start" runat="server" CssClass="txt2" Enabled="false" Text='<%# Eval("MEASUREMENT_OPEN_START", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                        to
       
                                            <asp:TextBox ID="Txt_tra_open_end" runat="server" CssClass="txt2" Enabled="false" Text='<%# Eval("MEASUREMENT_OPEN_END", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                        <br />
                        <label class="lbl4">Stability Period :</label>
                        <asp:TextBox ID="Txt_tra_stab_start" runat="server" CssClass="txt2" Enabled="false" Text='<%# Eval("MEASUREMENT_STAB_START", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                        to
       
                                            <asp:TextBox ID="Txt_tra_stab_end" runat="server" CssClass="txt2" Enabled="false" Text='<%# Eval("MEASUREMENT_STAB_END", "{0:MM/dd/yyyy}") %>'></asp:TextBox>

                        <asp:Panel ID="PnlSummerWindow3" runat="server" Visible="false">
                            <h3>Breaks In Service</h3>
                            <asp:GridView ID="Gv_DisplayBreakOfService" runat="server" AutoGenerateColumns="false"
                                EmptyDataText="There are currently NO breaks in service." BackColor="White" BorderColor="#336666"
                                BorderStyle="Double" BorderWidth="3px" CellPadding="4" GridLines="Horizontal"
                                OnRowDeleting="Gv_BreakOfService_RowDeleting"
                                OnRowEditing="Gv_BreakOfService_RowEditing"
                                OnRowCancelingEdit="Gv_BreakOfService_RowCancelingEdit"
                                OnRowUpdating="Gv_BreakOfService_RowUpdating">
                                <FooterStyle BackColor="White" ForeColor="#333333" />
                                <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="White" ForeColor="#333333" Height="30px" />
                                <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                                <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                <SortedAscendingHeaderStyle BackColor="#487575" />
                                <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                <SortedDescendingHeaderStyle BackColor="#275353" />
                                <Columns>

                                    <asp:TemplateField HeaderText="Start Date" HeaderStyle-Width="125px" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="HiddenBreakId" runat="server" Value='<%# Eval("BreakInServiceId") %>' />
                                            <asp:Literal ID="TxtSummerBreakStart" runat="server" Text='<%# Eval("StartDate", "{0:MM/dd/yyyy}") %>'></asp:Literal>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:HiddenField ID="HiddenBreakId" runat="server" Value='<%# Eval("BreakInServiceId") %>' />
                                            <asp:TextBox ID="TxtSummerBreakStart" runat="server" Text='<%# Eval("StartDate", "{0:MM/dd/yyyy}") %>' CssClass="txt" Width="75px"></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="End Date" HeaderStyle-Width="125px" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Literal ID="TxtSummerBreakEnd" runat="server" Text='<%# Eval("EndDate", "{0:MM/dd/yyyy}") %>'></asp:Literal>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="TxtSummerBreakEnd" runat="server" Text='<%# Eval("EndDate", "{0:MM/dd/yyyy}") %>' CssClass="txt" Width="75px"></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                                                        
                                    <asp:TemplateField HeaderText="Weeks" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Literal ID="Weeks" runat="server" Text='<%# Eval("Weeks") %>'></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                </Columns>
                            </asp:GridView>
                        </asp:Panel>

                        <asp:Panel ID="PnlEditTransitionMeasurement" runat="server">
                            <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                            </div>
                            <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                                <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                    <asp:ImageButton ID="ImageButton2" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" />
                                </div>
                                <h3>Edit Measurement Period</h3>
                                <label class="lblClear">Stability Period:</label>
                                <asp:Literal ID="LitPlanName" runat="server"></asp:Literal>
                                <br />
                                <label class="lblClear">Employee Type:</label>
                                <asp:Literal ID="LitEmployeeType" runat="server"></asp:Literal>
                                <br />
                                <label class="lblClear">Start/End Date:</label>
                                <asp:Literal ID="LitStartDate" runat="server"></asp:Literal>
                                to 
           
                                                    <asp:Literal ID="LitEndDate" runat="server"></asp:Literal>
                                <br />
                                <label class="lbl4">Measurement Period:</label>
                                <asp:TextBox ID="Txt_M_Meas_start" runat="server" CssClass="txt2" Text='<%# Eval("MEASUREMENT_START", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                to
           
                                                    <asp:TextBox ID="Txt_M_Meas_end" runat="server" CssClass="txt2" Text='<%# Eval("MEASUREMENT_END", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                <br />
                                <label class="lbl4">Administrative Period:</label>
                                <asp:TextBox ID="Txt_M_Admin_start" runat="server" CssClass="txt2" Text='<%# Eval("MEASUREMENT_ADMIN_START", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                to
           
                                                    <asp:TextBox ID="Txt_M_Admin_end" runat="server" CssClass="txt2" Text='<%# Eval("MEASUREMENT_ADMIN_END", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                <br />
                                <label class="lbl4">Open Enrollment:</label>
                                <asp:TextBox ID="Txt_M_Open_start" runat="server" CssClass="txt2" Text='<%# Eval("MEASUREMENT_OPEN_START", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                to
           
                                                    <asp:TextBox ID="Txt_M_Open_end" runat="server" CssClass="txt2" Text='<%# Eval("MEASUREMENT_OPEN_END", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                <br />
                                <label class="lbl4">Stability Period:</label>
                                <asp:TextBox ID="Txt_M_Stab_start" runat="server" CssClass="txt2" Text='<%# Eval("MEASUREMENT_STAB_START", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                to
           
                                                    <asp:TextBox ID="Txt_M_Stab_end" runat="server" CssClass="txt2" Text='<%# Eval("MEASUREMENT_STAB_END", "{0:MM/dd/yyyy}") %>'></asp:TextBox>


                                <asp:Panel ID="PnlEditSummerWindow" runat="server" Visible="false">

                                    <h3>Edit Breaks In Service</h3>
                                    <label class="lbl">New Break:</label>
                                    <asp:TextBox ID="TxtSummerBreakStart" runat="server" CssClass="txt" Width="75px"></asp:TextBox>
                                    to                       
                                                        <asp:TextBox ID="TxtSummerBreakEnd" runat="server" CssClass="txt" Width="75px"></asp:TextBox>
                                    <asp:Button ID="AddNewEditBreak" CssClass="btn" Text="Add" runat="server" OnClick="AddNewEditBreak_Click" />
                                    <br />

                                    <asp:GridView ID="Gv_BreakOfService" runat="server" AutoGenerateColumns="false"
                                        EmptyDataText="There are currently NO breaks in service." BackColor="White" BorderColor="#336666"
                                        BorderStyle="Double" BorderWidth="3px" CellPadding="4" GridLines="Horizontal"
                                        OnRowDeleting="Gv_BreakOfService_RowDeleting"
                                        OnRowEditing="Gv_BreakOfService_RowEditing"
                                        OnRowCancelingEdit="Gv_BreakOfService_RowCancelingEdit"
                                        OnRowUpdating="Gv_BreakOfService_RowUpdating">
                                        <FooterStyle BackColor="White" ForeColor="#333333" />
                                        <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="White" ForeColor="#333333" Height="30px" />
                                        <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                                        <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                        <SortedAscendingHeaderStyle BackColor="#487575" />
                                        <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                        <SortedDescendingHeaderStyle BackColor="#275353" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Edit">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgBtnEdit" runat="server" CommandName="Edit" ImageUrl="~/images/edit_notes.png" Height="25px" Style="float: right;" />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:ImageButton ID="ImgBtnUpdate" runat="server" CommandName="Update" ImageUrl="~/images/disk-save.png" Height="25px" />
                                                    <asp:ImageButton ID="ImgBtnCancel" runat="server" CommandName="Cancel" ImageUrl="~/images/error.png" Height="25px" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="Start Date" HeaderStyle-Width="125px" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:HiddenField ID="HiddenBreakId" runat="server" Value='<%# Eval("BreakInServiceId") %>' />
                                                    <asp:Literal ID="TxtSummerBreakStart" runat="server" Text='<%# Eval("StartDate", "{0:MM/dd/yyyy}") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:HiddenField ID="HiddenBreakId" runat="server" Value='<%# Eval("BreakInServiceId") %>' />
                                                    <asp:TextBox ID="TxtSummerBreakStart" runat="server" Text='<%# Eval("StartDate", "{0:MM/dd/yyyy}") %>' CssClass="txt" Width="75px"></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderText="End Date" HeaderStyle-Width="125px" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="TxtSummerBreakEnd" runat="server" Text='<%# Eval("EndDate", "{0:MM/dd/yyyy}") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="TxtSummerBreakEnd" runat="server" Text='<%# Eval("EndDate", "{0:MM/dd/yyyy}") %>' CssClass="txt" Width="75px"></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                                                                                
                                            <asp:TemplateField HeaderText="Weeks"  HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Literal ID="Weeks" runat="server" Text='<%# Eval("Weeks") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField HeaderStyle-Width="25px" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:ImageButton Width="25px" ID="ImgBtnDelete" runat="server" ImageUrl="~/images/close_box_red.png" Height="20px" CommandName="Delete" ToolTip="Delete this Break In Service?" />
                                                    <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="ImgBtnDelete" ConfirmText="Are you sure you want to DELETE this Break In Service?"></asp:ConfirmButtonExtender>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                                <br />

                                <label class="lbl4">Notes:</label>
                                <asp:TextBox ID="Txt_M_Notes" runat="server" Width="275px" Height="100px" TextMode="MultiLine" Text='<%# Eval("MEASUREMENT_NOTES") %>'></asp:TextBox>
                                <br />
                                <br />
                                <label class="lbl4">.</label>
                                <asp:Button ID="BtnSaveMeasurementPeriod" runat="server" CssClass="btn" Text="Submit" CommandName="Update" />
                                <br />
                                <asp:Label ID="Lbl_M_message" runat="server" BackColor="Yellow"></asp:Label>
                                <br />
                                <br />
                            </div>
                        </asp:Panel>
                        <asp:Panel ID="PnlTransitionMeasurementHistory" runat="server">
                            <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                            </div>
                            <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                                <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                    <asp:ImageButton ID="ImageButton3" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" />
                                </div>
                                <h3>Measurement History</h3>
                                <asp:TextBox ID="TxtTransitionMeasurementHistory" runat="server" Width="575px" Height="300px" TextMode="MultiLine" Font-Size="12px" BorderStyle="None" ReadOnly="true" Text='<%# Eval("MEASUREMENT_HISTORY") %>'></asp:TextBox>
                            </div>
                        </asp:Panel>
                        <asp:ModalPopupExtender ID="MpeEditTransitionPeriod" runat="server" PopupControlID="PnlEditTransitionMeasurement" TargetControlID="ImgBtnEditTransitionMeasurement"></asp:ModalPopupExtender>
                        <asp:ModalPopupExtender ID="MpeHistoryTransitionPeriod" runat="server" PopupControlID="PnlTransitionMeasurementHistory" TargetControlID="ImgBtnHistoryTransitionMeasurement"></asp:ModalPopupExtender>
                    </ItemTemplate>
                </asp:Repeater>
            </asp:Panel>
        </asp:Panel>

        <asp:HiddenField ID="HfDummyTrigger" runat="server" />
        <asp:Panel ID="PnlWebPageMessage" runat="server">
            <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
            </div>
            <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                    <asp:ImageButton ID="ImageButton5" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" OnClick="ImgBtnCancel_Click" />
                </div>
                <h3>Webpage Message</h3>
                <asp:Literal ID="LitMessage" runat="server"></asp:Literal>
                <br />
                <br />
            </div>
        </asp:Panel>
        <asp:ModalPopupExtender ID="MpeWebPageMessage" runat="server" TargetControlID="HfDummyTrigger" PopupControlID="PnlWebPageMessage"></asp:ModalPopupExtender>

    </div>



    <script src="/_js/jquery-ui-1.10.2/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="/_js/jquery-ui-1.10.2/ui/jquery-ui.js" type="text/javascript"></script>

    <script type="text/javascript">
        window.onload = function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
        }

        function endRequestHandler(sender, args) {
            init();
        }

        function init() {
            $("*[id*='Txt_M_Meas_start']").each(function () {
                $(this).datepicker(
                {
                    showOn: "button",
                    buttonImage: "/images/calendar.png",
                    buttonImageOnly: true,
                    yearRange: "-3:+5",
                    changeMonth: true,
                    changeYear: true,
                });
            });
            $("*[id*='Txt_M_Meas_end']").each(function () {
                $(this).datepicker(
                {
                    showOn: "button",
                    buttonImage: "/images/calendar.png",
                    buttonImageOnly: true,
                    yearRange: "-3:+5",
                    changeMonth: true,
                    changeYear: true,
                });
            });
            $("*[id*='Txt_M_Admin_start']").each(function () {
                $(this).datepicker(
                {
                    showOn: "button",
                    buttonImage: "/images/calendar.png",
                    buttonImageOnly: true,
                    yearRange: "-3:+5",
                    changeMonth: true,
                    changeYear: true,
                });
            });
            $("*[id*='Txt_M_Admin_end']").each(function () {
                $(this).datepicker(
                {
                    showOn: "button",
                    buttonImage: "/images/calendar.png",
                    buttonImageOnly: true,
                    yearRange: "-3:+5",
                    changeMonth: true,
                    changeYear: true,
                });
            });
            $("*[id*='Txt_M_Open_start']").each(function () {
                $(this).datepicker(
                {
                    showOn: "button",
                    buttonImage: "/images/calendar.png",
                    buttonImageOnly: true,
                    yearRange: "-3:+5",
                    changeMonth: true,
                    changeYear: true,
                });
            });
            $("*[id*='Txt_M_Open_end']").each(function () {
                $(this).datepicker(
                {
                    showOn: "button",
                    buttonImage: "/images/calendar.png",
                    buttonImageOnly: true,
                    yearRange: "-3:+5",
                    changeMonth: true,
                    changeYear: true,
                });
            });
            $("*[id*='Txt_M_Stab_start']").each(function () {
                $(this).datepicker(
                {
                    showOn: "button",
                    buttonImage: "/images/calendar.png",
                    buttonImageOnly: true,
                    yearRange: "-3:+5",
                    changeMonth: true,
                    changeYear: true,
                });
            });
            $("*[id*='Txt_M_Stab_end']").each(function () {
                $(this).datepicker(
                {
                    showOn: "button",
                    buttonImage: "/images/calendar.png",
                    buttonImageOnly: true,
                    yearRange: "-3:+5",
                    changeMonth: true,
                    changeYear: true,
                });
            });
            $("*[id*='TxtSummerBreakStart']").each(function () {
                $(this).datepicker(
                {
                    showOn: "button",
                    buttonImage: "/images/calendar.png",
                    buttonImageOnly: true,
                    yearRange: "-3:+5",
                    changeMonth: true,
                    changeYear: true,
                });
            });
            $("*[id*='TxtSummerBreakStart2']").each(function () {
                $(this).datepicker(
                {
                    showOn: "button",
                    buttonImage: "/images/calendar.png",
                    buttonImageOnly: true,
                    yearRange: "-3:+5",
                    changeMonth: true,
                    changeYear: true,
                });
            });
            $("*[id*='TxtSummerBreakEnd']").each(function () {
                $(this).datepicker(
                {
                    showOn: "button",
                    buttonImage: "/images/calendar.png",
                    buttonImageOnly: true,
                    yearRange: "-3:+5",
                    changeMonth: true,
                    changeYear: true,
                });
            });
            $("*[id*='TxtSummerBreakEnd2']").each(function () {
                $(this).datepicker(
                {
                    showOn: "button",
                    buttonImage: "/images/calendar.png",
                    buttonImageOnly: true,
                    yearRange: "-3:+5",
                    changeMonth: true,
                    changeYear: true,
                });
            });
            $("*[id*='Txt_npy_StartDate']").each(function () {
                $(this).datepicker(
                {
                    showOn: "button",
                    buttonImage: "/images/calendar.png",
                    buttonImageOnly: true,
                    yearRange: "-3:+5",
                    changeMonth: true,
                    changeYear: true,
                });
            });
        }

        $(function () { // DOM ready
            init();
        });

        function uploadError(sender, args) {
            document.getElementById('lblStatus').innerText = args.get_fileName(),
            "<span style='color:red;'>" + args.get_errorMessage() + "</span>";
        }

        function StartUpload(sender, args) {
            document.getElementById('lblStatus').innerText = 'Uploading Started.';
        }

        function UploadComplete(sender, args) {
            var filename = args.get_fileName();
            var contentType = args.get_contentType();
            var text = "Size of " + filename + " is " + args.get_length() + " bytes";
            if (contentType.length > 0) {
                text += " and content type is '" + contentType + "'.";
            }
            document.getElementById('lblStatus').innerText = text;
        }

</script>
</asp:Content>
