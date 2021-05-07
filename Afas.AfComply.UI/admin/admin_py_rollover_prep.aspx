<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="admin_py_rollover_prep.aspx.cs" Inherits="admin_admin_py_rollover_prep" %>
<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html>
<head>
    <title>Employee Import</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" type="text/css" href="../default.css" />
    <link rel="stylesheet" type="text/css" href="../menu.css" />
    <link rel="stylesheet" type="text/css" href="../v_menu.css" />

</head>
<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" AsyncPostBackTimeout="3600"></asp:ToolkitScriptManager>
        <div id="container">
            <div id="header">
                <a href="default.aspx">
                    <img src="<%= Page.ResolveUrl(Branding.LogoUri) %>" style="height: 70px" alt="<%= Branding.ProductName %> Logo" class="logo" />
                </a>
                <ul id="toplinks">
                    <li>Need Help? Call <%= Branding.PhoneNumber %></li> 
                    <li>
                        <asp:Literal ID="LitUserName" runat="server"></asp:Literal></li>
                    <li>
                        <asp:Button ID="BtnLogout" CssClass="btn" runat="server" Text="Log Out" OnClick="BtnLogout_Click" UseSubmitBehavior="false" /></li>
                </ul>
                <asp:HiddenField ID="HfDistrictID" runat="server" />
            </div>
            <div id="nav2">
                <nav>
                    <%= demo.getAdminLinks() %>
                </nav>
            </div>
            <asp:UpdatePanel ID="UpRollover" runat="server">
                <ContentTemplate>
                    <div id="topbox">
                        <div id="tbleft">
                            <h4>Measurement Period Rollover Prep</h4>

                            <label class="lbl3">Employer</label>
                            <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            <br />
                            <br />
                            <label class="lbl3">Current Plan Year</label>
                            <asp:DropDownList ID="DdlPlanYearCurrent" runat="server" CssClass="ddl2"></asp:DropDownList>
                            <br />
                            <br />
                            <label class="lbl3">New Plan Year</label>
                            <asp:DropDownList ID="DdlPlanYearNew" runat="server" CssClass="ddl2"></asp:DropDownList>
                            <br />
                            <br />
                            <asp:Button ID="BtnProcessFile" runat="server" Text="Validate Data" CssClass="btn" OnClick="BtnProcessFile_Click" />
                            <br />
                            <br />
                        </div>
                        <div id="tbright">
                            <h4>Instructions</h4>
                            <dl>
                                <dt>Step 1:</dt>
                                <dd>- Select the EMPLOYER that will be rolling over to a new plan year.</dd>
                                <dt>Step 2:</dt>
                                <dd>- Select the EMPLOYER's current PLAN YEAR.</dd>
                                <dt>Step 3:</dt>
                                <dd>- Select the EMPLOYER's new PLAN YEAR.</dd>
                                <dt>Step 4:</dt>
                                <dd>- Click the VALIDATE DATA button.</dd>
                            </dl>
                        </div>

                    </div>
                    <asp:Panel ID="PnlRollover" runat="server">
                        <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                        </div>
                        <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white; z-index: 10">
                            <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                <asp:ImageButton ID="ImageButton2" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" BorderStyle="None" />
                            </div>
                            <h3 style="color: black;">Meaurement Rollover Message</h3>
                            <p style="color: darkgray">
                                <asp:Label ID="LblRolloverMessage" runat="server" Font-Bold="true" Height="20px" Style="line-height: 20px"></asp:Label>
                            </p>

                            <br />
                            <br />
                        </div>
                    </asp:Panel>
                    <asp:HiddenField ID="HfDummyTrigger" runat="server" />
                    <asp:ModalPopupExtender ID="MpeRolloverMessage" runat="server" TargetControlID="HfDummyTrigger" OkControlID="ImageButton2" PopupControlID="PnlRollover"></asp:ModalPopupExtender>
                    <h3>Validation Steps</h3>
                    <dl>
                        <dt>Step 1: Validate New Hires in Current Plan Year: 
                   
                           

                            <asp:Image ID="ImgStep1" runat="server" ImageUrl="~/images/circle_red.png" Height="15px" />
                            <asp:LinkButton ID="LnkBtnStep1" runat="server">
                                <asp:ImageButton ID="ImgBtnStep1" runat="server" ImageUrl="~/design/eyeclosed.png" AlternateText="View" Height="15px" />
                            </asp:LinkButton>
                        </dt>
                        <dd>Measurement Period: 
                   
                           

                            <asp:Literal ID="LitStep1Start" runat="server"></asp:Literal>
                            - 
                   
                           

                            <asp:Literal ID="LitStep1End" runat="server"></asp:Literal>
                        </dd>
                        <dd>Plan Year ID:
                   
                           

                            <asp:Literal ID="LitCurrentPlanYear" runat="server"></asp:Literal>
                        </dd>
                        <dd>
                            <asp:Panel ID="PnlStep1" runat="server">
                                <asp:GridView ID="GvNhCurrPlan" runat="server" AutoGenerateColumns="false" Width="95%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Valid">
                                            <ItemTemplate>
                                                <asp:Image ID="Img_gv_Step1" runat="server" Height="20px" Width="20px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Employee Name">
                                            <ItemTemplate>
                                                <asp:Literal ID="Lit_gv_name" runat="server" Text='<%# Eval("EMPLOYEE_FULL_NAME") %>'></asp:Literal>
                                                <asp:HiddenField ID="Hf_gv_id" runat="server" Value='<%# Eval("EMPLOYEE_ID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Hire Date">
                                            <ItemTemplate>
                                                <asp:Literal ID="Lit_gv_hDate" runat="server" Text='<%# Eval("EMPLOYEE_HIRE_DATE", "{0:M/d/yyyy}") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Stab PY ID">
                                            <ItemTemplate>
                                                <asp:Literal ID="Lit_gv_stabID" runat="server" Text='<%# Eval("EMPLOYEE_PLAN_YEAR_ID") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Admin PY ID">
                                            <ItemTemplate>
                                                <asp:Literal ID="Lit_gv_adminID" runat="server" Text='<%# Eval("EMPLOYEE_PLAN_YEAR_ID_LIMBO") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Meas PY ID">
                                            <ItemTemplate>
                                                <asp:Literal ID="Lit_gv_measID" runat="server" Text='<%# Eval("EMPLOYEE_PLAN_YEAR_ID_MEAS") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:Button ID="BtnUpdateStep1" runat="server" Text="Correct Records" OnClick="BtnUpdateStep1_Click" />
                            </asp:Panel>
                            <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender3" runat="server"
                                TargetControlID="PnlStep1" CollapsedSize="0" ExpandedSize="300"
                                ExpandControlID="LnkBtnStep1" CollapseControlID="LnkBtnStep1" ImageControlID="ImgBtnStep1" ScrollContents="true"
                                Collapsed="true" ExpandedImage="~/design/eyeopen.png" CollapsedImage="~/design/eyeclosed.png">
                            </asp:CollapsiblePanelExtender>
                        </dd>
                        <dt>Step 2: Validate New Hires in New Plan Year: 
                   
                           

                            <asp:Image ID="ImgStep2" runat="server" ImageUrl="~/images/circle_red.png" Height="15px" />
                            <asp:LinkButton ID="LnkBtnStep2" runat="server">
                                <asp:ImageButton ID="ImgBtnStep2" runat="server" ImageUrl="~/design/eyeclosed.png" AlternateText="View" Height="15px" />
                            </asp:LinkButton>
                        </dt>
                        <dd>Measurement Period: 
                   
                           

                            <asp:Literal ID="LitStep2Start" runat="server"></asp:Literal>
                            - 
                   
                           

                            <asp:Literal ID="LitStep2End" runat="server"></asp:Literal>
                        </dd>
                        <dd>Plan Year ID:
                   
                           

                            <asp:Literal ID="LitNewPlanYearID" runat="server"></asp:Literal>
                        </dd>
                        <dd>
                            <asp:Panel ID="PnlStep2" runat="server">
                                <asp:GridView ID="GvNhNewPlan" runat="server" AutoGenerateColumns="false" Width="95%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Valid">
                                            <ItemTemplate>
                                                <asp:Image ID="Img_gv_Step2" runat="server" Height="20px" Width="20px" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Employee Name">
                                            <ItemTemplate>
                                                <asp:Literal ID="Lit_gv_name" runat="server" Text='<%# Eval("EMPLOYEE_FULL_NAME") %>'></asp:Literal>
                                                <asp:HiddenField ID="Hf_gv_id" runat="server" Value='<%# Eval("EMPLOYEE_ID") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Hire Date">
                                            <ItemTemplate>
                                                <asp:Literal ID="Lit_gv_hDate" runat="server" Text='<%# Eval("EMPLOYEE_HIRE_DATE", "{0:M/d/yyyy}") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Stab PY ID">
                                            <ItemTemplate>
                                                <asp:Literal ID="Lit_gv_stabID" runat="server" Text='<%# Eval("EMPLOYEE_PLAN_YEAR_ID") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Admin PY ID">
                                            <ItemTemplate>
                                                <asp:Literal ID="Lit_gv_adminID" runat="server" Text='<%# Eval("EMPLOYEE_PLAN_YEAR_ID_LIMBO") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Meas PY ID">
                                            <ItemTemplate>
                                                <asp:Literal ID="Lit_gv_measID" runat="server" Text='<%# Eval("EMPLOYEE_PLAN_YEAR_ID_MEAS") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:Button ID="BtnUpdateStep2" runat="server" Text="Correct Records" OnClick="BtnUpdateStep2_Click" />
                            </asp:Panel>
                            <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender2" runat="server"
                                TargetControlID="PnlStep2" CollapsedSize="0" ExpandedSize="300"
                                ExpandControlID="LnkBtnStep2" CollapseControlID="LnkBtnStep2" ImageControlID="ImgBtnStep2" ScrollContents="true"
                                Collapsed="true" ExpandedImage="~/design/eyeopen.png" CollapsedImage="~/design/eyeclosed.png">
                            </asp:CollapsiblePanelExtender>
                        </dd>
                        <dt>Step 3: Validate count of employees who will be rolled over.
                   
                           

                            <asp:Image ID="ImgStep3" runat="server" ImageUrl="~/images/circle_red.png" Height="15px" />
                            <asp:LinkButton ID="LnkBtnStep3" runat="server">
                                <asp:ImageButton ID="ImgBtnStep3" runat="server" ImageUrl="~/design/eyeclosed.png" AlternateText="View" Height="15px" />
                            </asp:LinkButton>
                            <asp:Panel ID="PnlStep3" runat="server">
                                <div style="width: 45%; float: left">
                                    <h4>Employee Count Breakdown</h4>
                                    Curr Plan Year New Hire Count = 
                           
                                   

                                    <asp:Literal ID="LitCurrNewHireCount" runat="server"></asp:Literal>
                                    <br />
                                    New Plan Year New Hire Count =
                           
                                   

                                    <asp:Literal ID="LitNewNewHireCount" runat="server"></asp:Literal>
                                    <br />
                                    Current Plan Year Ongoing Employee Count = 
                           
                                   

                                    <asp:Literal ID="LitCurrOngoingCount" runat="server"></asp:Literal>
                                    <br />
                                    Sum of Employees = 
                           
                                   

                                    <asp:Literal ID="LitSum" runat="server"></asp:Literal>
                                    <p>
                                        * The total of the three values shown above should equal the Total Employee Count on the right hand side. If these do not match, an employee was placed in the incorrect Plan Year and needs to be 
                                corrected before they can be rolled over. Please contact IT. 
                           
                                    </p>
                                </div>
                                <div style="width: 45%; float: right">
                                    <h4>Total Employee Count</h4>
                                    Total Employee Count = 
                           
                                    <asp:Literal ID="LitTotalCount" runat="server"></asp:Literal>
                                </div>
                                <br style="clear: both;" />
                                <div>
                                    <asp:GridView ID="GvStep3Errors" runat="server" AutoGenerateColumns="false" EmptyDataText="No Errors Exist" Width="95%" OnRowEditing="GvStep3Errors_RowEditing" OnRowCancelingEdit="GvStep3Errors_RowCancelingEdit" OnRowUpdating="GvStep3Errors_RowUpdating">
                                        <Columns>
                                            <asp:TemplateField HeaderText="Valid">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="LnkBtnEdit" runat="server" Text="Edit" CommandName="Edit"></asp:LinkButton>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:LinkButton ID="LnkBtnUpdate" runat="server" Text="Update" CommandName="Update"></asp:LinkButton>
                                                    <asp:LinkButton ID="LnkBtnCancel" runat="server" Text="Cancel" CommandName="Cancel"></asp:LinkButton>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Valid">
                                                <ItemTemplate>
                                                    <asp:Image ID="Img_gv_Step2" runat="server" Height="20px" Width="20px" ImageUrl="~/images/circle_red.png" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Employee Name">
                                                <ItemTemplate>
                                                    <asp:Literal ID="Lit_gv_name" runat="server" Text='<%# Eval("EMPLOYEE_FULL_NAME") %>'></asp:Literal>
                                                    <asp:HiddenField ID="Hf_gv_id" runat="server" Value='<%# Eval("EMPLOYEE_ID") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Hire Date">
                                                <ItemTemplate>
                                                    <asp:Literal ID="Lit_gv_hDate" runat="server" Text='<%# Eval("EMPLOYEE_HIRE_DATE", "{0:M/d/yyyy}") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Stab PY ID">
                                                <ItemTemplate>
                                                    <asp:Literal ID="Lit_gv_stabID" runat="server" Text='<%# Eval("EMPLOYEE_PLAN_YEAR_ID") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Admin PY ID">
                                                <ItemTemplate>
                                                    <asp:Literal ID="Lit_gv_adminID" runat="server" Text='<%# Eval("EMPLOYEE_PLAN_YEAR_ID_LIMBO") %>'></asp:Literal>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Meas PY ID">
                                                <ItemTemplate>
                                                    <asp:Literal ID="Lit_gv_measID" runat="server" Text='<%# Eval("EMPLOYEE_PLAN_YEAR_ID_MEAS") %>'></asp:Literal>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:HiddenField ID="Hf_gv_measID" runat="server" Value='<%# Eval("EMPLOYEE_PLAN_YEAR_ID_MEAS") %>' />
                                                    <asp:DropDownList ID="Ddl_gv_PlanYears" runat="server"></asp:DropDownList>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </asp:Panel>
                            <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender4" runat="server"
                                TargetControlID="PnlStep3" CollapsedSize="0" ExpandedSize="300"
                                ExpandControlID="LnkBtnStep3" CollapseControlID="LnkBtnStep3" ImageControlID="ImgBtnStep3" ScrollContents="true"
                                Collapsed="true" ExpandedImage="~/design/eyeopen.png" CollapsedImage="~/design/eyeclosed.png">
                            </asp:CollapsiblePanelExtender>
                        </dt>
                        <dt>Step 4: Validate Payroll Data 
                   
                            <asp:Image ID="ImgStep4" runat="server" ImageUrl="~/images/circle_red.png" Height="15px" />
                            <asp:LinkButton ID="LnkBtnStep4" runat="server">
                                <asp:ImageButton ID="ImgBtnStep4" runat="server" ImageUrl="~/design/eyeclosed.png" AlternateText="View" Height="15px" />
                            </asp:LinkButton>
                        </dt>
                        <dd>Measurement Period: 
                   
                            <asp:Literal ID="LitStep4Start" runat="server"></asp:Literal>
                            - 
                   
                            <asp:Literal ID="LitStep4End" runat="server"></asp:Literal>
                            <br />
                            View all of the Payroll Check Dates. Look for gaps in the dates, if something is missing, we will need to get that data before the Rollover Process can occur. 
                        </dd>
                        <dd>
                            <asp:Panel ID="PnlCheckDates" runat="server">
                                <asp:GridView ID="GvCheckDates" runat="server" Width="95%">
                                </asp:GridView>
                                <asp:Button ID="BtnApprovePayroll" runat="server" Text="Approve Payroll" OnClick="BtnApprovePayroll_Click" />
                            </asp:Panel>
                            <asp:CollapsiblePanelExtender ID="CollapsiblePanelExtender1" runat="server"
                                TargetControlID="PnlCheckDates" CollapsedSize="0" ExpandedSize="300"
                                ExpandControlID="LnkBtnStep4" CollapseControlID="LnkBtnStep4" ImageControlID="ImgBtnStep4" ScrollContents="true"
                                Collapsed="true" ExpandedImage="~/design/eyeopen.png" CollapsedImage="~/design/eyeclosed.png">
                            </asp:CollapsiblePanelExtender>
                        </dd>
                        <dt>Step 5: Validate Summer Hours
                   
                            <asp:Image ID="ImgStep5" runat="server" ImageUrl="~/images/circle_red.png" Height="15px" />
                        </dt>
                        <dd>*Note: If employer does not need summer hours, than you can override this error by clicking the override button: 
                   
                            <asp:Button ID="BtnOverride" runat="server" Text="Override" OnClick="BtnOverride_Click" />
                        </dd>
                        <dd>Summer Hour Payroll files =
                   
                            <asp:Literal ID="LitSHCount" runat="server"></asp:Literal>
                        </dd>
                        <dt>Step 6: Validate Employee Import Alerts
                   
                            <asp:Image ID="ImgStep6" runat="server" ImageUrl="~/images/circle_red.png" Height="15px" />
                        </dt>
                        <dd>Employee Import Alerts =
                   
                            <asp:Literal ID="LitStep6Count" runat="server"></asp:Literal>
                        </dd>
                    </dl>
                    <asp:HiddenField ID="HfStep1Complete" runat="server" />
                    <asp:HiddenField ID="HfStep2Complete" runat="server" />
                    <asp:HiddenField ID="HfStep3Complete" runat="server" />
                    <asp:HiddenField ID="HfStep4Complete" runat="server" />
                    <asp:HiddenField ID="HfStep5Complete" runat="server" />
                    <asp:HiddenField ID="HfStep6Complete" runat="server" />
                    <asp:Button ID="BtnRolloverMP" runat="server" Text="Rollover MP" OnClick="BtnRolloverMP_Click" />
                    <br />
                    *** NOTE ***
         
                    <br />
                    When click the "Rollover MP" button, it will trigger the Batch Reporting Calculation first before Measurement Period will be rolled over. 
     
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpRollover" DynamicLayout="true" DisplayAfter="500">
                <ProgressTemplate>
                    <div style="position: fixed; top: 0; left: 0; background-color: white; width: 100%; height: 100%; opacity: .85; filter: alpha(opacity=85); -moz-opacity: 0.85; text-align: center;">
                        <div style="position: relative; margin-left: auto; margin-right: auto; background-color: white; padding-top: 100px;">
                            <h4>Calculating data..... This can take a few minutes.....</h4>
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
