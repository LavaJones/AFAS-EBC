<%@ Page Title="" Language="C#" MasterPageFile="~/securepages/SecurePages.Master" AutoEventWireup="true" CodeBehind="mass_insurance_offering.aspx.cs" Inherits="securepages_mass_insurance_offering" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="bodycontent" runat="server">
    <link rel="stylesheet" type="text/css" href="/Body.css" />
    <link rel="stylesheet" type="text/css" href="/leftnav.css" />
    <asp:HiddenField ID="HfEmployerTypeID" runat="server" />
    <asp:HiddenField ID="HfUserName" runat="server" />
    <asp:HiddenField ID="HfUName" runat="server" />
    <asp:HiddenField ID="HfDistrictID" runat="server" />
    <div class="left_ebc">
        <%= demo.getLeftLinks(null != Session["CurrentDistrict"] && ((employer)Session["CurrentDistrict"]).IrsEnabled) %>
    </div>
    <div id="content" style="margin-left: 462px; top: 20px; position: absolute; width: 50%">

        <div style="float: right; padding-right: 5px;">
            Export Alerts:
           
                    <asp:ImageButton ID="ImgBtnExport" runat="server" ToolTip="Export Alerts to .CSV file" Height="30px" ImageUrl="/design/csv-file-icon.png" OnClick="ImgBtnExport_Click" />
            <asp:ConfirmButtonExtender ID="CbeConfirmOfferFileDownload" runat="server" TargetControlID="ImgBtnExport" ConfirmText="<%# this.DisclaimerMessage %>"></asp:ConfirmButtonExtender>

        </div>
        <div style="clear: both;">
            <div style="float: right; padding-right: 20px; padding-top: 20px;">
                Stability Period: 
                       
                        <asp:UpdatePanel ID="UpPlanYear" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="DdlPlanYear" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DdlPlanYear_SelectedIndexChanged"></asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>


            </div>

            <asp:UpdatePanel ID="UpEmployee" runat="server">

                <ContentTemplate>

                    <asp:Panel ID="PnlFilters" runat="server">

                        <h3>Filter By</h3>

                        <table style="width: 100%;">

                            <tr style="font-weight: bold;">

                                <td style="width: 20%">

                                    <asp:CheckBox ID="Cb_f_Classification" runat="server" Text="Classification" AutoPostBack="true" OnCheckedChanged="Cb_f_Classification_CheckedChanged" />

                                    <asp:DropDownList ID="Ddl_f_Classification" runat="server" Width="60%" CssClass="ddl2" AutoPostBack="true" OnSelectedIndexChanged="Ddl_f_Classification_SelectedIndexChanged" Enabled="false"></asp:DropDownList>
                                </td>


                                <td style="width: 20%">
                                    <asp:CheckBox ID="Cb_f_HrStatus" runat="server" Text="HR Status" AutoPostBack="true" OnCheckedChanged="Cb_f_HrStatus_CheckedChanged" />
                                    <br />
                                    <asp:DropDownList ID="Ddl_f_HRStatus" runat="server" Width="50%" CssClass="ddl2" AutoPostBack="true" OnSelectedIndexChanged="Ddl_f_HRStatus_SelectedIndexChanged" Enabled="false"></asp:DropDownList>
                                </td>

                                <td style="width: 20%">
                                    <asp:CheckBox ID="Cb_f_Hours" runat="server" Text="Hours" AutoPostBack="true" OnCheckedChanged="Cb_f_Hours_CheckedChanged" />
                                    <br />
                                    <asp:DropDownList ID="Ddl_f_Hours" runat="server" Width="50%" CssClass="ddl2" AutoPostBack="true" OnSelectedIndexChanged="Ddl_f_Hours_SelectedIndexChanged" Enabled="false">
                                        <asp:ListItem Text="Greater or equal to 130 hours" Value="129.9999"></asp:ListItem>
                                        <asp:ListItem Text="Greater or equal to 120 hours" Value="119.9999"></asp:ListItem>
                                        <asp:ListItem Text="Greater or equal to 110 hours" Value="109.9999"></asp:ListItem>
                                        <asp:ListItem Text="Greater or equal to 100 hours" Value="99.9999"></asp:ListItem>
                                        <asp:ListItem Text="Greater or equal to 90 hours" Value="89.9999"></asp:ListItem>
                                        <asp:ListItem Text="Greater or equal to 80 hours" Value="79.9999"></asp:ListItem>
                                        <asp:ListItem Text="Greater or equal to 70 hours" Value="69.9999"></asp:ListItem>
                                        <asp:ListItem Text="Greater or equal to 60 hours" Value="59.9999"></asp:ListItem>
                                        <asp:ListItem Text="Greater or equal to 50 hours" Value="49.9999"></asp:ListItem>
                                        <asp:ListItem Text="Greater or equal to 40 hours" Value="39.9999"></asp:ListItem>
                                        <asp:ListItem Text="Greater or equal to 30 hours" Value="29.9999"></asp:ListItem>
                                        <asp:ListItem Text="Greater or equal to 20 hours" Value="19.9999"></asp:ListItem>
                                        <asp:ListItem Text="Greater or equal to 10 hours" Value="9.9999"></asp:ListItem>
                                        <asp:ListItem Text="Greater or equal to 0 hours" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Less than 130 hours" Value="130"></asp:ListItem>
                                        <asp:ListItem Text="Less than 120 hours" Value="120"></asp:ListItem>
                                        <asp:ListItem Text="Less than 110 hours" Value="110"></asp:ListItem>
                                        <asp:ListItem Text="Less than 100 hours" Value="100"></asp:ListItem>
                                        <asp:ListItem Text="Less than 90 hours" Value="90"></asp:ListItem>
                                        <asp:ListItem Text="Less than 80 hours" Value="80"></asp:ListItem>
                                        <asp:ListItem Text="Less than 70 hours" Value="70"></asp:ListItem>
                                        <asp:ListItem Text="Less than 60 hours" Value="60"></asp:ListItem>
                                        <asp:ListItem Text="Less than 50 hours" Value="50"></asp:ListItem>
                                        <asp:ListItem Text="Less than 40 hours" Value="40"></asp:ListItem>
                                        <asp:ListItem Text="Less than 30 hours" Value="30"></asp:ListItem>
                                        <asp:ListItem Text="Less than 20 hours" Value="20"></asp:ListItem>
                                        <asp:ListItem Text="Less than 10 hours" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="Select" Value="Select" Selected="true"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>

                            </tr>

                        </table>

                        <br />
                        <asp:Button ID="BtnApplyFilters" runat="server" Text="Apply Filters" CssClass="btn" Width="15%" OnClick="BtnApplyFilters_Click" />
                    </asp:Panel>
                    <br />
                    <hr />
                    <asp:Panel ID="PnlUpdate" runat="server" DefaultButton="BtnSave">
                        <h3>Fields to update</h3>
                        <div style="width: 100%">
                            <div style="width: 45%; padding-left: 0%; font-weight: bold;">
                                #1) Was this employee offered medical coverage?
           
                                    <br />
                                <asp:DropDownList ID="Ddl_io_Offered" runat="server" CssClass="ddl2" AutoPostBack="true" OnSelectedIndexChanged="Ddl_io_Offered_SelectedIndexChanged">
                                    <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                                    <asp:ListItem Text="No" Value="false"></asp:ListItem>
                                    <asp:ListItem Text="Select" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <br />
                            <br />
                            <div style="width: 45%; float: right; font-weight: bold">
                                <asp:Panel ID="Pnl_io_DateOffered" runat="server" Visible="false">
                                    #2) Enter Date Offered - (Deprecated)
       
                                        <br />
                                    <asp:TextBox ID="Txt_io_DateOffered" runat="server" Enabled="false" CssClass="txt3"></asp:TextBox>
                                </asp:Panel>
                            </div>
                        </div>
                        <br style="clear: left;" />

                        <asp:Panel ID="Pnl_io_Accepted" runat="server" Visible="false">
                            <div style="width: 100%">
                                <div style="width: 45%; float: left; font-weight: bold;">
                                    #3) Did this employee accept the offer insurance?
       
                                        <br />
                                    <asp:DropDownList ID="Ddl_io_Accepted2" runat="server" CssClass="ddl2" AutoPostBack="true" OnSelectedIndexChanged="Ddl_io_Accepted2_SelectedIndexChanged">
                                        <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="false"></asp:ListItem>
                                        <asp:ListItem Text="Select" Selected="true"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <br />

                                <div style="width: 45%; float: right; font-weight: bold">
                                    #4) Date Accepted/Declined - (Deprecated)
   
                                        <br />
                                    <asp:TextBox ID="Txt_io_AcceptedOffer" runat="server" Enabled="false" CssClass="txt3"></asp:TextBox>
                                </div>
                            </div>
                            <br style="clear: left;" />
                        </asp:Panel>
                        <asp:Panel ID="Pnl_io_Plan" runat="server" Visible="false">
                            <div style="width: 100%">
                                <div style="width: 45%; float: left; font-weight: bold">
                                    <asp:Panel ID="Pnl_io_PlanOffered" runat="server">
                                        #5) Which Plan was
                                            <asp:Literal ID="LitName" runat="server"></asp:Literal>
                                        offered?      
                                            <br />
                                        <asp:DropDownList ID="Ddl_io_InsurancePlan" runat="server" Width="70%" CssClass="ddl2" AutoPostBack="true" OnSelectedIndexChanged="Ddl_io_InsurancePlan_SelectedIndexChanged"></asp:DropDownList>
                                    </asp:Panel>
                                </div>
                                <div style="width: 45%; float: right; font-weight: bold">
                                    #6) Effective Date of Insurance
                                        <br />
                                    <asp:TextBox ID="Txt_io_InsuranceEffectiveDate" runat="server" CssClass="txt3"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender6" runat="server" TargetControlID="Txt_io_InsuranceEffectiveDate" DefaultView="Days"></asp:CalendarExtender>
                                </div>
                            </div>
                            <br style="clear: left;" />
                        </asp:Panel>

                        <asp:Panel ID="Pnl_io_Effective" runat="server" Visible="false">
                            <div style="width: 100%">
                                <div style="width: 45%; float: left; font-weight: bold">
                                    #7) Select EMPLOYER Contribution:
       
                                        <br />
                                    <asp:DropDownList ID="Ddl_io_Contribution" runat="server" Width="80%" CssClass="ddl2"></asp:DropDownList>
                                </div>
                                <div style="width: 45%; float: right; font-weight: bold">
                                    #8) HRA/Flex Additional Monthly Contributions
       
                                        <br />
                                    <asp:TextBox ID="Txt_io_HraFlex" runat="server" CssClass="txt3"></asp:TextBox>
                                </div>
                            </div>
                            <br style="clear: left;" />
                        </asp:Panel>

                        <div style="width: 100%">
                            <div style="width: 95%; float: left;">
                                Explanation/Notes:
       
                                    <br />
                                <asp:TextBox ID="Txt_io_Comments" runat="server" TextMode="MultiLine" Height="50px" Width="95%"></asp:TextBox>
                                <br />
                                <asp:Label ID="Lbl_io_Message" runat="server" BackColor="Yellow"></asp:Label>
                            </div>
                        </div>

                        <br style="clear: left;" />
                        <br style="clear: left;" />
                        <asp:Button ID="BtnSave" runat="server" Text="Update ALL selected records" Width="20%" CssClass="btn" OnClick="BtnSave_Click" />
                    </asp:Panel>
                    <br />
                    <hr />
                    <asp:CheckBox ID="CbCheckAll" runat="server" Text="Select All" AutoPostBack="true" OnCheckedChanged="CbCheckAll_CheckedChanged" />
                    &nbsp;&nbsp;&nbsp;&nbsp; 
Showing 
                        <asp:Literal ID="litAlertsShown" runat="server"></asp:Literal>
                    Insurance Alerts of 
                        <asp:Literal ID="litAlertCount" runat="server"></asp:Literal>
                    <br />
                    <asp:GridView ID="GvOffers" runat="server" AutoGenerateColumns="false" CellPadding="1" ForeColor="#333333" GridLines="None" Width="95%" Font-Size="10px" ShowHeader="True" AllowPaging="True" PageSize="100" EmptyDataRowStyle-BackColor="Yellow" OnPageIndexChanging="GvOffers_PageIndexChanging" OnSorting="GvOffers_Sorting" AllowSorting="true">
                        <AlternatingRowStyle BackColor="White" />
                        <FooterStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" VerticalAlign="Bottom" />
                        <PagerStyle BackColor="#eb0029" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EFF3FB" />
                        <SelectedRowStyle BackColor="#eb0029" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#eb0029" />
                        <SortedAscendingHeaderStyle BackColor="#eb0029" />
                        <SortedDescendingCellStyle BackColor="#eb0029" />
                        <SortedDescendingHeaderStyle BackColor="#eb0029" />
                        <EmptyDataTemplate>
                            No records were found.
                        </EmptyDataTemplate>

                        <Columns>
                            <asp:TemplateField HeaderText="" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <asp:CheckBox ID="Cb_gv_Selected" runat="server" Checked="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Payroll ID" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" SortExpression="EMPLOYEE_EXT_ID">
                                <ItemTemplate>
                                    <asp:Literal Text='<%# Eval("EMPLOYEE_EXT_ID") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Employee" HeaderStyle-Width="395px" HeaderStyle-HorizontalAlign="Left" SortExpression="EMPLOYEE_FULL_NAME">
                                <ItemTemplate>
                                    <asp:Literal ID="LitEmployeeName" runat="server" Text='<%# Eval("EMPLOYEE_FULL_NAME") %>'></asp:Literal>
                                    <asp:HiddenField ID="HfDummyTriggerIns" runat="server" />
                                    <asp:HiddenField ID="HfRowID" runat="server" Value='<%# Eval("ROW_ID") %>' />
                                    <asp:HiddenField ID="HfEmployeeID" runat="server" Value='<%# Eval("IALERT_EMPLOYEEID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Avg Hours/Month" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" SortExpression="IALERT_AVG_HOURS">
                                <ItemTemplate>
                                    <asp:Literal ID="LitAvgHours" Text='<%# Eval("EMPLOYEE_AVG_HOURS") %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
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
            <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpPlanYear" DynamicLayout="true" DisplayAfter="500">
                <ProgressTemplate>
                    <div style="position: fixed; top: 0; left: 0; background-color: white; width: 100%; height: 100%; opacity: .85; filter: alpha(opacity=85); -moz-opacity: 0.85; text-align: center;">
                        <div style="position: relative; margin-left: auto; margin-right: auto; background-color: white; padding-top: 100px;">
                            <h4>Processing your data..... This may take a minute.....</h4>
                            <asp:Image ID="ImgSearching2" runat="server" ImageUrl="~/design/icon-loading-animated.gif" />
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </div>

    <div style="clear: both;">
        &nbsp;
    </div>

</asp:Content>
