<%@ Page EnableSessionState="ReadOnly" Title="Employer Plan Years" Language="C#"
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="EmployerPlanYear.aspx.cs"
    Inherits="Afas.AfComply.UI.admin.AdminPortal.EmployerPlanYear" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <link rel="stylesheet" type="text/css" href="/_js/jquery-ui-themes-1.10.2/themes/ui-lightness/jquery-ui.css" />


    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>

    <h2>Edit Plan Years</h2>
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

        <asp:Panel ID="PnlPlanYear" runat="server">
            <asp:Button ID="BtnNewPlanYear" CssClass="btn" runat="server" Text="New" />
            <h3>Stability Period</h3>
            <i>You are viewing page
                       
                                    <%=GvPlanYears.PageIndex + 1%>
                        of
                       
                                    <%=GvPlanYears.PageCount%>
            </i>
            <br />
            <i>Showing 
                       
                                    <asp:Literal ID="LitPyShow" runat="server"></asp:Literal>
                Stability Periods of 
                       
                                    <asp:Literal ID="LitPyTotal" runat="server"></asp:Literal>
            </i>
            <asp:GridView ID="GvPlanYears" runat="server" AutoGenerateColumns="false" CssClass="gridviewHeader" ShowHeaderWhenEmpty="true" CellPadding="0" ForeColor="#333333" EmptyDataText="There are currently no active Stability Periods." GridLines="None" Width="500px" Font-Size="10px" AllowPaging="true" PageSize="10" OnRowUpdating="GvPlanYears_RowUpdating" OnRowEditing="GvPlanYears_RowEditing" OnSelectedIndexChanged="GvPlanYears_SelectedIndexChanged" OnPageIndexChanging="GvPlanYears_PageIndexChanging">
                <AlternatingRowStyle BackColor="White" />
                <EditRowStyle BackColor="yellow" />
                <FooterStyle BackColor="LightGray" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="LightGray" Font-Bold="True" ForeColor="black" />
                <PagerStyle BackColor="#FF0083AF" HorizontalAlign="Center" />
                <PagerSettings FirstPageImageUrl="~/design/first.png" Mode="NextPreviousFirstLast" LastPageImageUrl="/design/last.png" NextPageImageUrl="/design/next.png" PreviousPageImageUrl="/design/prev.png" PageButtonCount="25" Position="TopAndBottom" />
                <RowStyle BackColor="#EFF3FB" />
                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                <EmptyDataTemplate>
                    <asp:Label ID="LblEmptyDataSource" runat="server" Text="There are currently no active Stability Periods."></asp:Label>
                </EmptyDataTemplate>
                <Columns>
                    <asp:TemplateField HeaderStyle-Width="75px">
                        <ItemTemplate>
                            <asp:ImageButton ID="ImgBtn_epy_edit" runat="server" ImageUrl="~/images/edit_notes.png" Height="25px" CommandName="Edit" />
                            <asp:ImageButton ID="ImgBtn_hpy_view" runat="server" ImageUrl="~/images/history.png" Height="25px" CommandName="Select" />
                            <asp:HiddenField ID="HfDummyTrigger" runat="server" />
                            <asp:HiddenField ID="HfDummyTrigger2" runat="server" />
                            <asp:HiddenField ID="HfPlanYearID" runat="server" Value='<%# Eval("PLAN_YEAR_ID") %>' />
                            <asp:Panel ID="Pnl_hpy" runat="server">
                                <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                                </div>
                                <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                                    <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                        <asp:ImageButton ID="ImageButton3" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" />
                                    </div>
                                    <h3>Stability Period History</h3>
                                    <asp:TextBox ID="TxtPlanYearHistory" runat="server" Width="575px" Height="300px" TextMode="MultiLine" Font-Size="12px" BorderStyle="None" ReadOnly="true" Text='<%# Eval("PLAN_YEAR_HISTORY") %>'></asp:TextBox>
                                </div>
                            </asp:Panel>
                            <asp:ModalPopupExtender ID="MpePlanYearHistory" runat="server" TargetControlID="HfDummyTrigger" PopupControlID="Pnl_hpy"></asp:ModalPopupExtender>
                            <asp:Panel ID="Pnl_epy" runat="server" DefaultButton="Btn_epy_Update">
                                <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                                </div>
                                <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                                    <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                        <asp:ImageButton ID="ImgBtn_epy_Cancel" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" />
                                    </div>
                                    <h3>Edit Stability Period</h3>
                                    <label class="lbl3">Name</label>
                                    <asp:TextBox ID="Txt_epy_Name" runat="server" CssClass="txt3" Text='<%# Eval("PLAN_YEAR_DESCRIPTION") %>'></asp:TextBox>
                                    <br />
                                    <label class="lbl3">Start Date</label>
                                    <asp:TextBox ID="Txt_epy_StartDate" runat="server" CssClass="txt3" Text='<%# Eval("PLAN_YEAR_START", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                    <br />
                                    <label class="lbl3">End Date</label>
                                    <asp:TextBox ID="Txt_epy_EndDate" runat="server" CssClass="txt3" Text='<%# Eval("PLAN_YEAR_END", "{0:MM/dd/yyyy}") %>'></asp:TextBox>

                                    <br />


                                    <label class="lbl4">Default Measurement Period:</label>
                                    <asp:TextBox ID="Txt_M_Meas_start" runat="server" Text='<%# Eval("Default_Meas_Start", "{0:MM/dd/yyyy}") %>' CssClass="txt" Width="75px"></asp:TextBox>
                                    to
                       
                                                        <asp:TextBox ID="Txt_M_Meas_end" runat="server" Text='<%# Eval("Default_Meas_End", "{0:MM/dd/yyyy}") %>' CssClass="txt" Width="75px"></asp:TextBox>

                                    <br />
                                    <label class="lbl4">Default  Administrative Period:</label>
                                    <asp:TextBox ID="Txt_M_Admin_start" runat="server" Text='<%# Eval("Default_Admin_Start", "{0:MM/dd/yyyy}") %>' CssClass="txt" Width="75px"></asp:TextBox>
                                    to
                       
                                                        <asp:TextBox ID="Txt_M_Admin_end" runat="server" Text='<%# Eval("Default_Admin_End", "{0:MM/dd/yyyy}") %>' CssClass="txt" Width="75px"></asp:TextBox>
                                    <br />
                                    <label class="lbl4">Default  Open Enrollment:</label>
                                    <asp:TextBox ID="Txt_M_Open_start" runat="server" Text='<%# Eval("Default_Open_Start", "{0:MM/dd/yyyy}") %>' CssClass="txt" Width="75px"></asp:TextBox>
                                    to
                       
                                                        <asp:TextBox ID="Txt_M_Open_end" runat="server" Text='<%# Eval("Default_Open_End", "{0:MM/dd/yyyy}") %>' CssClass="txt" Width="75px"></asp:TextBox>
                                    <br />
                                    <label class="lbl4">Default Stability Period:</label>
                                    <asp:TextBox ID="Txt_M_Stab_start" runat="server" Text='<%# Eval("Default_Stability_Start", "{0:MM/dd/yyyy}") %>' CssClass="txt" Width="75px"></asp:TextBox>
                                    to
                       
                                                        <asp:TextBox ID="Txt_M_Stab_end" runat="server" Text='<%# Eval("Default_Stability_End", "{0:MM/dd/yyyy}") %>' CssClass="txt" Width="75px"></asp:TextBox>
                                    <br />
                                    <br />
                                    <asp:HiddenField ID="HfPlanYearGroupId" runat="server" Value='<%# Eval("PlanYearGroupId") %>'></asp:HiddenField>
                                    <label class="lbl4">Plan Year Group:</label>
                                    <asp:DropDownList ID="DdlPlanYearGroupEdit" runat="server" CssClass="ddl2"></asp:DropDownList>
                                    <br />

                                    <br />
                                    <label class="lbl3">Notes</label>
                                    <asp:TextBox ID="Txt_epy_Notes" runat="server" Width="400px" Height="100px" TextMode="MultiLine" Text='<%# Eval("PLAN_YEAR_NOTES") %>'></asp:TextBox>
                                    <br />
                                    <br />
                                    <label class="lbl3" style="background-color: white; color: white;">.</label>
                                    <asp:Button ID="Btn_epy_Update" CssClass="btn" runat="server" Text="Submit" CommandName="Update" />
                                    <br />
                                    <br />
                                    <br />
                                    <label class="lbl3">Message</label>
                                    <asp:Label ID="Lbl_npy_Message" runat="server" ForeColor="Red" Font-Bold="true" Height="20px" Style="line-height: 20px"></asp:Label>
                                    <br />
                                    <br />
                                </div>
                            </asp:Panel>
                            <asp:ModalPopupExtender ID="Mpe_epy" runat="server" PopupControlID="Pnl_epy" TargetControlID="HfDummyTrigger2"></asp:ModalPopupExtender>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Stability Period Name" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="LblGvPlanName" runat="server" Text='<%# Eval("PLAN_YEAR_DESCRIPTION") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Stability Start Date" HeaderStyle-Width="125px" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="LblGvRenewalDate" runat="server" Text='<%# Eval("PLAN_YEAR_START", "{0:MM/dd/yyyy}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Stability End Date" HeaderStyle-Width="125px" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="LblGvEndDate" runat="server" Text='<%# Eval("PLAN_YEAR_END", "{0:MM/dd/yyyy}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <asp:Panel ID="PnlNewPlanYear" runat="server" DefaultButton="BtnPlanYearUpdate">
                <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                </div>
                <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                    <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                        <asp:ImageButton ID="ImgBtnEquivEditCancel" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" />
                    </div>
                    <h3>New Stability Period</h3>
                    <label class="lbl3">Name</label>
                    <asp:TextBox ID="Txt_npy_Name" runat="server" CssClass="txt3"></asp:TextBox>
                    <br />
                    <label class="lbl3">Start Date</label>
                    <asp:TextBox ID="Txt_npy_StartDate" runat="server" CssClass="txt3"></asp:TextBox>
                    <br />
                    <label class="lbl4">Default Measurement Period:</label>
                    <asp:TextBox ID="Txt_M_Meas_start" runat="server" Text='<%# Eval("Default_Meas_Start", "{0:MM/dd/yyyy}") %>' CssClass="txt" Width="75px"></asp:TextBox>
                    to
                       
                                                        <asp:TextBox ID="Txt_M_Meas_end" runat="server" Text='<%# Eval("Default_Meas_End", "{0:MM/dd/yyyy}") %>' CssClass="txt" Width="75px"></asp:TextBox>

                    <br />
                    <label class="lbl4">Default  Administrative Period:</label>
                    <asp:TextBox ID="Txt_M_Admin_start" runat="server" Text='<%# Eval("Default_Admin_Start", "{0:MM/dd/yyyy}") %>' CssClass="txt" Width="75px"></asp:TextBox>
                    to
                       
                                                        <asp:TextBox ID="Txt_M_Admin_end" runat="server" Text='<%# Eval("Default_Admin_End", "{0:MM/dd/yyyy}") %>' CssClass="txt" Width="75px"></asp:TextBox>
                    <br />
                    <label class="lbl4">Default  Open Enrollment:</label>
                    <asp:TextBox ID="Txt_M_Open_start" runat="server" Text='<%# Eval("Default_Open_Start", "{0:MM/dd/yyyy}") %>' CssClass="txt" Width="75px"></asp:TextBox>
                    to
                       
                                                        <asp:TextBox ID="Txt_M_Open_end" runat="server" Text='<%# Eval("Default_Open_End", "{0:MM/dd/yyyy}") %>' CssClass="txt" Width="75px"></asp:TextBox>
                    <br />
                    <label class="lbl4">Default Stability Period:</label>
                    <asp:TextBox ID="Txt_M_Stab_start" runat="server" Text='<%# Eval("Default_Stability_Start", "{0:MM/dd/yyyy}") %>' CssClass="txt" Width="75px"></asp:TextBox>
                    to
                       
                                                        <asp:TextBox ID="Txt_M_Stab_end" runat="server" Text='<%# Eval("Default_Stability_End", "{0:MM/dd/yyyy}") %>' CssClass="txt" Width="75px"></asp:TextBox>
                    <br />
                    <br />
                    <label class="lbl4">Plan Year Group:</label>
                    <asp:DropDownList ID="DdlPlanYearGroupNew" runat="server" CssClass="ddl2"></asp:DropDownList>
                    <br />
                    <br />
                    <label class="lbl3">Notes</label>
                    <asp:TextBox ID="Txt_npy_Notes" runat="server" Width="400px" Height="100px" TextMode="MultiLine"></asp:TextBox>
                    <br />
                    <br />
                    <label class="lbl3" style="background-color: white; color: white;">.</label>
                    <asp:Button ID="BtnPlanYearUpdate" CssClass="btn" runat="server" Text="Submit" OnClick="BtnPlanYearUpdate_Click" />
                    <br />
                    <br />
                    <br />
                    <label class="lbl3">Message</label>
                    <asp:Label ID="Lbl_npy_Message" runat="server" ForeColor="Red" Font-Bold="true" Height="20px" Style="line-height: 20px"></asp:Label>
                    <br />
                    <br />
                </div>
            </asp:Panel>

            <asp:ModalPopupExtender ID="MpeNewPlanYear" runat="server" TargetControlID="BtnNewPlanYear" PopupControlID="PnlNewPlanYear"></asp:ModalPopupExtender>
        </asp:Panel>

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
