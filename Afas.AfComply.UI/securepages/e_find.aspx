<%@ Page Title="" Language="C#" MasterPageFile="~/securepages/SecurePages.Master" AutoEventWireup="true" CodeBehind="e_find.aspx.cs" Inherits="securepages_e_find" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="bodycontent" runat="server">
    <link rel="stylesheet" type="text/css" href="/employee.css" />
     <link rel="stylesheet" type="text/css" href="/leftnav.css?1.4.0.100" />

    <style type="text/css">
        .lbl {
            width: 100px;
            float: left;
            text-align: right;
            clear: left;
            padding: 5px;
            /*background-color: lightgray;*/
            height: 15px;
        }

        .lbl2 {
            padding: 5px;
        }

        .txt {
            padding: 12px;
            width: 146px;
            height: 15px;
        }

        .sidenav {
            width: 20%;
            height: 100%;
            z-index: 1;
            top: 225px;
            left: 0;
            background-color: #fff;
            border-bottom: #ddd;
            overflow-x: hidden;
            padding-top: 10px;
            font-family: "Myriad Pro", "DejaVu Sans Condensed", Helvetica, Arial, "sans-serif";
            text-align: left;
            font-weight: 200;
        }

            .sidenav a {
                padding: 8px 8px 8px 8px;
                text-decoration: none;
                font-size: 14px;
                color: #6e6d71;
                display: block;
            }

                .sidenav a:hover {
                    /*color: #eb0029;*/
                    text-decoration: underline;
                    border-left: #eb0029 4px solid;
                }
    </style>
    <div id="container">
        <div id="header">

            <asp:HiddenField ID="HfUserName" runat="server" />
            <asp:HiddenField ID="HfDistrictID" runat="server" />
        </div>

        <div id="content">
            <div class="search_ebc">
                <asp:Panel ID="PnlFindEmployee" runat="server" DefaultButton="ImgBtnSearch">
                    Find by Last Name:
                                    
                               
                               <asp:TextBox ID="txtSearch" runat="server" placeholder="Enter Last Name" CssClass="form-control input-lg" Width="200px"></asp:TextBox>&nbsp;&nbsp;<asp:ImageButton ID="ImgBtnSearch" runat="server" ImageUrl="/images/search-icon.png" Height="16px" OnClick="ImgBtnSearch_Click" />

                </asp:Panel>
                <asp:Panel ID="PnlFineEmployee2" runat="server" DefaultButton="ImgBtnSearch2">
                    Find by Payroll ID:
                              &nbsp;&nbsp;<asp:TextBox ID="TxtSearch2" runat="server" autocomplete="off" AutoCompleteType="Disabled" placeholder="Enter Payroll ID" CssClass="form-control input-lg" Width="200px"></asp:TextBox>&nbsp;&nbsp;<asp:ImageButton ID="ImgBtnSearch2" runat="server" ImageUrl="/images/search-icon.png" Height="16px" OnClick="ImgBtnSearch2_Click" />
                </asp:Panel>

            </div>

           
<%
 if (null != Session["CurrentDistrict"] && ((employer)Session["CurrentDistrict"]).IrsEnabled == true)
{
%>
<%
                        
}
%>
<%= demo.getLeftLinks(null != Session["CurrentDistrict"] && ((employer)Session["CurrentDistrict"]).IrsEnabled) %> 



          

            <asp:UpdatePanel ID="UpEmployee" runat="server">
                <ContentTemplate>
                    <div class="middle_ebc">

                        <h3>Employee List</h3>
                        Employee Total: 
       
                            <asp:Literal ID="litEmployeeCount" runat="server"></asp:Literal>
                        <br />
                        Employees Shown:
       
                            <asp:Literal ID="litEmployeeShown" runat="server"></asp:Literal>
                        <asp:GridView ID="GvEmployees" runat="server" CssClass="gridviewHeader" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" CellPadding="1" Font-Size="14px" ForeColor="black" GridLines="None" Width="300px" OnSelectedIndexChanged="GvEmployees_SelectedIndexChanged" AllowPaging="True" PageSize="15" OnPageIndexChanging="GvEmployees_PageIndexChanging" EmptyDataRowStyle-ForeColor="black" EmptyDataText="You can search for employee by Last Name or Payroll ID by using the search boxes above ." OnRowDataBound="GvEmployees_RowDataBound">
                            <AlternatingRowStyle BackColor="white" />
                            <EditRowStyle BackColor="#eb0029" />
                            <FooterStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#eb0029" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EFF3FB" />
                            <SelectedRowStyle BackColor="white" Font-Bold="true" ForeColor="black" />
                            <SortedAscendingCellStyle BackColor="#F5F7FB" />
                            <SortedAscendingHeaderStyle BackColor="#eb0029" />
                            <SortedDescendingCellStyle BackColor="#E9EBEF" />
                            <SortedDescendingHeaderStyle BackColor="#eb0029" />
                            <Columns>
                                <asp:TemplateField HeaderText="" HeaderStyle-Width="20px" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/images/circle_green.png" Height="20px" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Name" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="HfEmployeeID" runat="server" Value='<%# Eval("EMPLOYEE_ID") %>' />
                                        <asp:LinkButton ID="LbEmployee" runat="server" Text='<%# Eval("EMPLOYEE_FULL_NAME") %>' CommandName="Select"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Payroll ID" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="LblGvPayrollID" runat="server" Text='<%# Eval("EMPLOYEE_EXT_ID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="DOB" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Label ID="LblGvDescription2" runat="server" Text='<%# Eval("EMPLOYEE_DOB", "{0:M/d/yyyy}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>


                    <div class="right_ebc">
                        <div style="position: absolute; top: 15px; right: 85px;">
                            <asp:Button ID="BtnNewEmployee" runat="server" CssClass="btn1" Text="New" OnClick="BtnNewEmployee_Click" />
                            <asp:Button ID="BtnSaveEmployeeInfo" runat="server" CssClass="btn1" Text="Save" OnClick="BtnSaveEmployeeInfo_Click" />
                            <asp:HyperLink NavigateUrl="~/securepages/edit_employee.aspx" runat="server" Text="Edit Employees"></asp:HyperLink>
                            <asp:HiddenField ID="HfDummyTrigger" runat="server" />
                            <asp:HiddenField ID="HfDummyTrigger2" runat="server" />
                            <asp:Panel ID="PnlNewEmployee" runat="server" Style="display: none">
                                <div style="position: fixed; top: 0; left: 0; background-color: gray; opacity: 0.8; width: 100%; height: 100%;">
                                </div>
                                <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 800px; height: auto; margin-top: 0px; font-size: 12px; background-color: white; z-index: 10">
                                    <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                        <asp:ImageButton ID="ImgBtnClose2" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" BorderStyle="None" />
                                    </div>
                                    <div style="width: 100%">
                                        <h3 style="color: black;">Create NEW Employee</h3>
                                    </div>
                                    <div style="width: 100%">
                                        <div style="width: 48%; float: left;">
                                            <label class="lbl">Stability Period:</label>
                                            <asp:DropDownList ID="Ddl_new_PlanYear" runat="server" CssClass="ddl2"></asp:DropDownList>
                                            <br />
                                            <label class="lbl">Employee Type:</label>
                                            <asp:DropDownList ID="Ddl_new_Type" runat="server" CssClass="ddl2"></asp:DropDownList>
                                            <br />

                                            <label class="lbl">First Name:</label>
                                            <asp:TextBox ID="Txt_new_FirstName" runat="server" CssClass="txt3"></asp:TextBox>
                                            <br />
                                            <label class="lbl">Last Name:</label>
                                            <asp:TextBox ID="Txt_new_LastName" runat="server" CssClass="txt3"></asp:TextBox>
                                            <br />
                                            <label class="lbl">Address:</label>
                                            <asp:TextBox ID="Txt_new_Address" runat="server" CssClass="txt3"></asp:TextBox>
                                            <br />
                                            <label class="lbl">City:</label>
                                            <asp:TextBox ID="Txt_new_City" runat="server" CssClass="txt3"></asp:TextBox>
                                            <br />
                                            <label class="lbl">State:</label>
                                            <asp:DropDownList ID="Ddl_new_State" runat="server" CssClass="ddl2"></asp:DropDownList>
                                            <br />
                                            <label class="lbl">Zip:</label>
                                            <asp:TextBox ID="Txt_new_Zip" runat="server" CssClass="txt3"></asp:TextBox>
                                        </div>
                                        <div style="width: 48%; float: right;">
                                            <label class="lbl">SSN:</label>
                                            <asp:TextBox ID="Txt_new_SSN" autocomplete="off" AutoCompleteType="Disabled" runat="server" CssClass="txt3" TextMode="Password"></asp:TextBox>
                                            <br />
                                            Format: 9 digits - No dashes - xxxxxxxxx

                                                <br />
                                            <label class="lbl">DOB:</label>
                                            <asp:TextBox ID="Txt_new_DOB" runat="server" CssClass="txt3"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="Txt_new_DOB" DefaultView="Years"></asp:CalendarExtender>
                                            <br />
                                            <label class="lbl">Hire Date:</label>
                                            <asp:TextBox ID="Txt_new_Hdate" runat="server" CssClass="txt3"></asp:TextBox>
                                            <asp:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="Txt_new_Hdate" DefaultView="Years"></asp:CalendarExtender>
                                            <br />
                                            <label class="lbl">Employee #:</label>
                                            <asp:TextBox ID="Txt_new_PayrollID" runat="server" CssClass="txt3"></asp:TextBox>
                                            <br />
                                            This is the ID from your payroll software.
                   
                                                <br />
                                            <label class="lbl">HR Status:</label>
                                            <asp:DropDownList ID="Ddl_new_HRStatus" runat="server" CssClass="ddl2"></asp:DropDownList>
                                            <br />
                                            <label class="lbl">Classification:</label>
                                            <asp:DropDownList ID="Ddl_new_Classification" runat="server" CssClass="ddl2"></asp:DropDownList>
                                            <br />
                                            <label class="lbl">Status:</label>
                                            <asp:DropDownList ID="Ddl_new_ActStatus" runat="server" CssClass="ddl2"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div style="width: 100%; clear: left;"></div>
                                    <br />
                                    <asp:Button ID="BtnSaveNewEmployee" runat="server" Text="Save" CssClass="btn" Width="15%" OnClick="BtnSaveNewEmployee_Click" />
                                    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                    <br />
                                    .
       
                                        <br />
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="PnlMessage" runat="server" Style="display: none">
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
                            <asp:ModalPopupExtender ID="MpeNewEmployee" runat="server" TargetControlID="HfDummyTrigger2" OkControlID="ImgBtnClose2" PopupControlID="PnlNewEmployee"></asp:ModalPopupExtender>
                            <asp:ModalPopupExtender ID="MpeWebMessage" runat="server" TargetControlID="HfDummyTrigger" OkControlID="ImgBtnClose" PopupControlID="PnlMessage"></asp:ModalPopupExtender>
                        </div>
                        <div style="float: left; min-height: 400px; width: 200px;">

                            <h3>Employee Details</h3>
                            <br />

                            <asp:Literal ID="LitFirstname" runat="server"></asp:Literal>
                            <asp:Literal ID="LitMiddleName" runat="server"></asp:Literal>
                            <asp:Literal ID="LitLastName" runat="server"></asp:Literal>
                            <br />
                            <asp:Literal ID="LitAddress" runat="server"></asp:Literal>
                            <br />
                            <asp:Literal ID="LitCity" runat="server"></asp:Literal>

                            <asp:Literal ID="LitState" runat="server"></asp:Literal>
                            <asp:Literal ID="LitZip" runat="server"></asp:Literal>
                            <br />
                            <asp:LinkButton ID="LbViewDependents" runat="server" Text="View Dependents" OnClick="LbViewDependents_Click"></asp:LinkButton>
                            <br />
                            <br />
                            <label style="width: 65px; text-align: right; font-weight: bold; font-size: 13px;">Employee #: </label>
                            <span style="font-size: 13px;">
                                <asp:Literal ID="LitPayrollID" runat="server"></asp:Literal></span>
                            <br />
                            <label style="width: 65px; text-align: right; font-weight: bold; font-size: 13px;">ID: </label>
                            <span style="font-size: 13px;">
                                <asp:Literal ID="LitActID" runat="server"></asp:Literal></span>

                            <% if (Feature.ShowDOB)
                                { %>
                            <br />
                            <label style="width: 65px; text-align: right; font-weight: bold; font-size: 13px;">DOB: </label>
                            <span style="font-size: 13px;">
                                <asp:Literal ID="LitDOB" runat="server"></asp:Literal></span>
                            <% } %>

                            <br />
                            <label style="width: 65px; text-align: right; font-weight: bold; font-size: 13px;">Hire Date: </label>
                            <span style="font-size: 13px;">
                                <asp:Literal ID="LitHireDate" runat="server"></asp:Literal></span>
                            <br />
                            <label style="width: 65px; text-align: right; font-weight: bold; font-size: 13px;">Term Date: </label>
                            <span style="font-size: 13px;">
                                <asp:Literal ID="LitTermDate" runat="server"></asp:Literal></span>
                            <br />

                            <label style="width: 65px; text-align: right; font-weight: bold; font-size: 13px;">*Change: </label>
                            <span style="font-size: 13px;">
                                <asp:Literal ID="LitStatusChange" runat="server"></asp:Literal></span>
                            <br />

                            <span style="font-size: 13px;">*Last time HR Status was changed.</span>
                        </div>

                        <div style="float: right; min-height: 400px; width: 320px;">
                            <h3>&nbsp</h3>
                            <p>
                                <label style="width: 65px; float: left; text-align: right; font-weight: bold; font-size: 10px;">Class: </label>
                                <asp:DropDownList ID="DdlClassification" runat="server" CssClass="ddl2"></asp:DropDownList>
                            </p>
                            <p>
                                <label style="width: 65px; float: left; text-align: right; font-weight: bold; font-size: 10px;">
                                    <asp:ImageButton ID="ImgBtnViewSSN" runat="server" ImageUrl="~/design/eyeclosed.png" Height="15px" OnClick="ImgBtnViewSSN_Click" />
                                    SSN:
                                </label>
                                <asp:TextBox ID="TxtSSN" runat="server" CssClass="txt3"></asp:TextBox>
                            </p>
                            <p>
                                <label style="width: 65px; float: left; text-align: right; font-weight: bold; font-size: 10px;">HR Desc: </label>
                                <asp:DropDownList ID="DdlHrStatus" runat="server" CssClass="ddl2"></asp:DropDownList>
                            </p>
                            <p>
                                <label style="width: 65px; float: left; text-align: right; font-weight: bold; font-size: 10px;">ACA Status: </label>
                                <asp:DropDownList ID="DdlActStatus" runat="server" OnSelectedIndexChanged="DdlActStatus_SelectedIndexChanged" AutoPostBack="true" CssClass="ddl2">
                                </asp:DropDownList>
                            </p>

                            <asp:HiddenField ID="HfActStatus" runat="server" />

                            <asp:ModalPopupExtender ID="mpAcaStatus" PopupControlID="pnlAcaStatusMessage" TargetControlID="HfActStatus" OkControlID="btnOK" runat="server">
                            </asp:ModalPopupExtender>

                            <asp:Panel ID="pnlAcaStatusMessage" runat="server">
                                <div style="position: fixed; top: 0; left: 0; background-color: white; opacity: 0.8; width: 100%; height: 100%;"></div>
                                <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white; z-index: 10">
                                    <p>
                                        Changing the ACA Status alone will not terminate the employee from the system unless a termination date is provided.  Please enter the termination date in Edit Eployee page.
                                    </p>
                                    <p>
                                        <asp:Button ID="btnOK" Text="OK" runat="server" /></p>
                                </div>
                            </asp:Panel>


                        </div>
                        <br />
                        <br />
                        <div style="position: absolute; top: 315px; width: 525px; height: 200px;">
                            <hr />
                            <label style="width: 100px; float: left; text-align: right; font-weight: bold;">New Hire = </label>
                            "Employee is in Initial Measurement Period"
   
                                <br />
                            <label style="width: 100px; float: left; text-align: right; font-weight: bold;">Current SP = </label>
                            "Stability Period the Employee is in"
   
                                <br />
                            <label style="width: 100px; float: left; text-align: right; font-weight: bold;">Current AP = </label>
                            "Administrative Period the Employee is in"
   
                                <br />
                            <label style="width: 100px; float: left; text-align: right; font-weight: bold;">Current MP = </label>
                            "Measurement Period the Employee is in"
   
                                <br />
                            <br />
                            <nav>
                                <ul>
                                    <li>
                                        <asp:LinkButton ID="LbInitialStatus" CssClass="minPadding" runat="server" Text="" OnClick="LbInitialStatus_Click"></asp:LinkButton>
                                    </li>
                                    <li>
                                        <asp:LinkButton ID="LbCurrentStatus" CssClass="minPadding" runat="server" Text="" OnClick="LbCurrentStatus_Click"></asp:LinkButton></li>
                                    <li>
                                        <asp:LinkButton ID="LbLimboStatus" CssClass="minPadding" runat="server" Text="" OnClick="LbLimboStatus_Click"></asp:LinkButton></li>
                                    <li>
                                        <asp:LinkButton ID="LbUpcomingStatus" CssClass="minPadding" runat="server" Text="" OnClick="LbUpcomingStatus_Click"></asp:LinkButton></li>
                                </ul>
                            </nav>
                            <br />
                            <asp:Literal ID="LitPlanName" runat="server"></asp:Literal>
                            <asp:LinkButton ID="LnBtnInsuranceDetails" runat="server" Text=" - Insurance Details" OnClick="LnBtnInsuranceDetails_Click" Visible="false"></asp:LinkButton>
                            <asp:HiddenField ID="HfPlanYearID" runat="server" />
                            <br />
                            <asp:MultiView ID="MvEmployeeStatus" runat="server" ActiveViewIndex="0">
                                <asp:View ID="View_Curr" runat="server">
                                    <asp:Panel ID="PnlStabCurr" runat="server">
                                        <br />
                                        <b>Average Hours per Month</b> - 
                                            <asp:LinkButton ID="LbHourDetails" runat="server" Text="Payroll Details" Font-Size="10px" OnClick="LbHourDetails_Click"></asp:LinkButton>
                                        - 
                                            <asp:LinkButton ID="LbHourAdd" runat="server" Text="Add Payroll Record" Font-Size="10px" OnClick="LbHourAdd_Click"></asp:LinkButton>
                                        <div id="measHours" runat="server" style="height: 15px; width: 500px; border-radius: 5px; background-color: lightgray;">
                                            <asp:Literal ID="LitMeasHoursCurr" runat="server"></asp:Literal>
                                        </div>
                                        <div id="Div6" runat="server" style="height: 15px; width: 500px;">
                                            <div style="float: left">
                                                <asp:Literal ID="LitMeasHourCurrMin" runat="server"></asp:Literal>
                                            </div>
                                            <div style="float: right">
                                                <asp:Literal ID="LitMeasHourCurrMax" runat="server"></asp:Literal>
                                            </div>
                                        </div>
                                        <br />
                                        <b>Measurement Period</b>
                                        <div id="measCurr" runat="server" style="height: 15px; width: 500px; border-radius: 5px; background-color: lightgray;">
                                            <asp:Literal ID="LitMeasPercentCurr" runat="server"></asp:Literal>
                                        </div>
                                        <div id="Div7" runat="server" style="height: 15px; width: 500px;">
                                            <div style="float: left">
                                                <asp:Literal ID="LitMeasStartCurr" runat="server"></asp:Literal>
                                            </div>
                                            <div style="float: right">
                                                <asp:Literal ID="LitMeasEndCurr" runat="server"></asp:Literal>
                                            </div>
                                        </div>
                                        <br />
                                        <b>Administrative/Open Enrollment Period</b>
                                        <div id="adminCurr" runat="server" style="height: 15px; width: 500px; border-radius: 5px; background-color: lightgray;">
                                            <asp:Literal ID="LitAdminPercentCurr" runat="server"></asp:Literal>
                                        </div>
                                        <div id="Div3" runat="server" style="height: 15px; width: 500px;">
                                            <div style="float: left">
                                                <asp:Literal ID="LitAdminStartCurr" runat="server"></asp:Literal>
                                            </div>
                                            <div style="float: right">
                                                <asp:Literal ID="LitAdminEndCurr" runat="server"></asp:Literal>
                                            </div>
                                        </div>
                                        <br />
                                        <b>Stability Period</b>
                                        <div id="stabCurr" runat="server" style="height: 15px; width: 500px; border-radius: 5px; background-color: lightgray;">
                                            <asp:Literal ID="LitStabPercentCurr" runat="server"></asp:Literal>
                                        </div>
                                        <div id="Div5" runat="server" style="height: 15px; width: 500px;">
                                            <div style="float: left">
                                                <asp:Literal ID="LitStabStartCurr" runat="server"></asp:Literal>
                                            </div>
                                            <div style="float: right">
                                                <asp:Literal ID="LitStabEndCurr" runat="server"></asp:Literal>
                                            </div>
                                        </div>
                                        <br />
                                        <br />
                                    </asp:Panel>
                                </asp:View>
                            </asp:MultiView>


                            <asp:Panel ID="PnlPayrollDetails" runat="server">
                                <div style="position: fixed; top: 0; left: 0; background-color: gray; opacity: 0.8; width: 100%; height: 100%;">
                                </div>
                                <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white; z-index: 10">
                                    <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                        <asp:ImageButton ID="ImgBtnClosePR" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" BorderStyle="None" />
                                    </div>
                                    <h3 style="color: black;">Payroll Details</h3>
                                    <span style="color: black">Name: 
       
                                            <asp:Literal ID="Lit_pr_Name" runat="server"></asp:Literal>
                                        <br />
                                        Employee #: 
       
                                            <asp:Literal ID="Lit_pr_ExtID" runat="server"></asp:Literal>
                                        <br />
                                        Measurement Period:
       
                                            <asp:Literal ID="Lit_pr_MP" runat="server"></asp:Literal>
                                    </span>
                                    <p style="color: darkgray">
                                        <asp:Literal ID="LitPayrollMessage" runat="server"></asp:Literal>
                                    </p>
                                    <br />
                                    <asp:GridView ID="GvPayrollDetails" runat="server" AutoGenerateColumns="False" OnRowEditing="GvPayrollDetails_RowEditing" OnRowDeleting="GvPayrollDetails_RowDeleting" OnRowCancelingEdit="GvPayrollDetails_RowCancelingEdit" OnPageIndexChanging="GvPayrollDetails_PageIndexChanging" OnRowUpdating="GvPayrollDetails_RowUpdating" CellPadding="4" ForeColor="#333333" GridLines="None" EmptyDataText="Currently no payroll records have been imported for this employee." AllowPaging="True" PageSize="15">
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgBtnEditPayroll" runat="server" ImageUrl="~/images/edit_notes.png" Width="20px" CommandName="Edit" ToolTip="EDIT payroll record." />
                                                    <asp:ImageButton ID="ImgBtnDeletePayroll" runat="server" ImageUrl="~/images/close_box_red.png" Width="20px" CommandName="Delete" ToolTip="Delete payroll record." />
                                                    <asp:ConfirmButtonExtender ID="CbeDelete" runat="server" TargetControlID="ImgBtnDeletePayroll" ConfirmText="Are you sure you want to DELETE this payroll record?"></asp:ConfirmButtonExtender>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:ImageButton ID="ImgBtnCancelPayroll" runat="server" ImageUrl="~/images/back-icon.png" CommandName="Cancel" Width="20px" ToolTip="CANCEL editing." />
                                                    <asp:ImageButton ID="ImgBtnSavePayroll" runat="server" ImageUrl="~/images/disk-save.png" CommandName="Update" Width="20px" ToolTip="SAVE changes." />
                                                </EditItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Batch" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="Lbl_pr_BatchID" runat="server" Text='<%# Bind("PAY_BATCH_ID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="40px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Hours" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="Lbl_pr_Hours" runat="server" Text='<%# Bind("PAY_HOURS") %>'></asp:Label>
                                                    <asp:HiddenField ID="Hf_pr_RowID" runat="server" Value='<%# Bind("ROW_ID") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="Txt_pr_Hours" runat="server" Text='<%# Bind("PAY_HOURS") %>' Width="70px"></asp:TextBox>
                                                    <asp:HiddenField ID="Hf_pr_RowID2" runat="server" Value='<%# Bind("ROW_ID") %>' />
                                                </EditItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pay Start" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="Lbl_pr_PStart" runat="server" Text='<%# Bind("PAY_SDATE", "{0:MM-dd-yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="Txt_pr_PStart" runat="server" Text='<%# Bind("PAY_SDATE", "{0:MM-dd-yyyy}")  %>' Width="75px"></asp:TextBox>
                                                    <asp:CalendarExtender ID="Ce_pr_PStart" runat="server" TargetControlID="Txt_pr_PStart"></asp:CalendarExtender>
                                                </EditItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pay End" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="Lbl_pr_PEnd" runat="server" Text='<%# Bind("PAY_EDATE", "{0:MM-dd-yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="Txt_pr_PEnd" runat="server" Text='<%# Bind("PAY_EDATE", "{0:MM-dd-yyyy}") %>' Width="75px"></asp:TextBox>
                                                </EditItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Check Date" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="Lbl_pr_CDate" runat="server" Text='<%# Bind("PAY_CDATE", "{0:MM-dd-yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Pay Description" HeaderStyle-Width="125px" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="Lbl_pr_PDesc" runat="server" Text='<%# Bind("PAY_GP_NAME") %>'></asp:Label>
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="125px" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <EditRowStyle BackColor="#eb0029" />
                                        <FooterStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#eb0029" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#EFF3FB" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                        <SortedAscendingHeaderStyle BackColor="#eb0029" />
                                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                        <SortedDescendingHeaderStyle BackColor="#eb0029" />
                                    </asp:GridView>

                                </div>
                            </asp:Panel>


                            <asp:Panel ID="PnlAddPayroll" runat="server">
                                <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                                </div>
                                <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white; z-index: 10">
                                    <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                        <asp:ImageButton ID="ImgBtnCloseAP" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" BorderStyle="None" />
                                    </div>
                                    <h3 style="color: black;">Add Payroll Record</h3>
                                    <p style="color: darkgray">
                                        <asp:Literal ID="LitName" runat="server"></asp:Literal>
                                    </p>
                                    <br />
                                    <label class="lbl">Start Date</label>
                                    <asp:TextBox ID="TxtStartDate" runat="server" CssClass="txt"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="TxtStartDate"></asp:CalendarExtender>
                                    mm-dd-yyyy
   
                                        <br />
                                    <label class="lbl">End Date</label>
                                    <asp:TextBox ID="TxtEndDate" runat="server" CssClass="txt"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="TxtEndDate"></asp:CalendarExtender>
                                    mm-dd-yyyy
   
                                        <br />
                                    <label class="lbl">Check Date</label>
                                    <asp:TextBox ID="TxtCheckDate" runat="server" CssClass="txt"></asp:TextBox>
                                    <asp:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="TxtCheckDate"></asp:CalendarExtender>
                                    mm-dd-yyyy
   
                                        <br />
                                    <label class="lbl">ACA Hours</label>
                                    <asp:TextBox ID="TxtACAhours" runat="server" CssClass="txt"></asp:TextBox>
                                    80 hours 15 minutes = 80.25
   
                                        <br />
                                    <label class="lbl">Pay Description</label>
                                    <asp:DropDownList ID="DdlGrossPayDesc" runat="server" CssClass="ddl"></asp:DropDownList>
                                    <br />
                                    <br />
                                    <label class="lbl">.</label>
                                    <asp:Button ID="BtnSavePayroll" runat="server" CssClass="btn" Text="Submit" OnClick="BtnSavePayroll_Click" />
                                    <br />
                                    <br />
                                    <asp:Literal ID="LitMessageNewPayroll" runat="server"></asp:Literal>
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="PnlDependents" runat="server">
                                <div style="position: fixed; top: 0; left: 0; background-color: gray; opacity: 0.8; width: 100%; height: 100%;">
                                </div>
                                <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white; z-index: 10">
                                    <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                        <asp:ImageButton ID="ImgBtnCloseDependents" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" BorderStyle="None" />
                                    </div>
                                    <h3 style="color: black;">Employee Dependents</h3>
                                    <p style="color: darkgray">
                                        <asp:Literal ID="Lit_dep_employeeName" runat="server"></asp:Literal>
                                    </p>
                                    <br />
                                    <asp:GridView ID="GvDependents" runat="server" AutoGenerateColumns="false" Width="590px" EmptyDataText="No Dependents could be found." CellPadding="4" ForeColor="#333333" GridLines="None" OnRowCancelingEdit="GvDependents_RowCancelingEdit" OnRowEditing="GvDependents_RowEditing" OnRowDeleting="GvDependents_RowDeleting" OnRowUpdating="GvDependents_RowUpdating">
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="45px" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="ImgBtnEditDependent" runat="server" ImageUrl="~/images/edit_notes.png" CommandName="Edit" Width="20px" ToolTip="EDIT dependent record." />
                                                    <asp:ImageButton ID="ImgBtnDeleteDependent" runat="server" ImageUrl="~/images/close_box_red.png" CommandName="Delete" Width="20px" ToolTip="Delete dependent record." />
                                                    <asp:ConfirmButtonExtender ID="CbeDelete" runat="server" TargetControlID="ImgBtnDeleteDependent" ConfirmText="Are you sure you want to DELETE this dependent record?"></asp:ConfirmButtonExtender>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:ImageButton ID="ImgBtnCancelDependent" runat="server" ImageUrl="~/images/back-icon.png" Width="20px" CommandName="Cancel" ToolTip="CANCEL editing." />
                                                    <asp:ImageButton ID="ImgBtnSaveDependent" runat="server" ImageUrl="~/images/disk-save.png" Width="20px" CommandName="Update" ToolTip="SAVE changes." />
                                                </EditItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="45px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="First" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="Lbl_dep_Fname" runat="server" Text='<%# Bind("DEPENDENT_FIRST_NAME") %>'></asp:Label>
                                                    <asp:HiddenField ID="Hf_dep_id" runat="server" Value='<%# Bind("DEPENDENT_ID") %>' />
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="Txt_dep_Fname" runat="server" Text='<%# Bind("DEPENDENT_FIRST_NAME") %>' Width="75px"></asp:TextBox>
                                                    <asp:HiddenField ID="Hf_dep_id2" runat="server" Value='<%# Bind("DEPENDENT_ID") %>' />
                                                </EditItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Middle" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="Lbl_dep_Mname" runat="server" Text='<%# Bind("DEPENDENT_MIDDLE_NAME") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="Txt_dep_Mname" runat="server" Text='<%# Bind("DEPENDENT_MIDDLE_NAME") %>' Width="75px"></asp:TextBox>
                                                </EditItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Last" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="Lbl_dep_Lname" runat="server" Text='<%# Bind("DEPENDENT_LAST_NAME") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="Txt_dep_Lname" runat="server" Text='<%# Bind("DEPENDENT_LAST_NAME") %>' Width="75px"></asp:TextBox>
                                                </EditItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="SSN" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="Lbl_dep_SSN" runat="server" Text='<%# Bind("DEPENDENT_SSN_MASKED") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="Txt_dep_SSN" runat="server" Text='<%# Bind("DEPENDENT_SSN") %>' Width="75px"></asp:TextBox>
                                                </EditItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="DOB" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <asp:Label ID="Lbl_dep_DOB" runat="server" Text='<%# Bind("DEPENDENT_DOB", "{0:MM-dd-yyyy}") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="Txt_dep_DOB" runat="server" Text='<%# Bind("DEPENDENT_DOB", "{0:MM-dd-yyyy}") %>' Width="75px"></asp:TextBox>
                                                </EditItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <EditRowStyle BackColor="#eb0029" />
                                        <FooterStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#eb0029" ForeColor="White" HorizontalAlign="Center" />
                                        <RowStyle BackColor="#eb0029" />
                                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                        <SortedAscendingHeaderStyle BackColor="#eb0029" />
                                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                        <SortedDescendingHeaderStyle BackColor="#eb0029" />
                                    </asp:GridView>
                                    <br />
                                    <p style="color: darkgray">
                                        <asp:Literal ID="Lit_dep_message" runat="server"></asp:Literal>
                                    </p>
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="PnlInsuranceOffer" runat="server" Style="display: none;">
                                <div style="position: fixed; top: 0; left: 0; background-color: gray; opacity: 0.8; width: 100%; height: 100%;">
                                </div>
                                <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 700px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                                    <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                        <asp:ImageButton ID="ImageButton3" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" OnClick="ImageButton3_Click" />
                                    </div>
                                    <div style="float: left; width: 245px;">
                                        <h3>Offer Of Insurance</h3>
                                        <asp:HiddenField ID="Hf_io_rowID" runat="server" />
                                        <asp:HiddenField ID="Hf_io_planYearID" runat="server" />
                                        Stability Period:
       
                                            <asp:Literal ID="Lit_io_PlanYear" runat="server"></asp:Literal>
                                        <br />
                                        Employee:
       
                                            <asp:Literal ID="Lit_io_EmployeeName" runat="server"></asp:Literal>
                                        <br />
                                        Employee #:
       
                                            <asp:Literal ID="Lit_io_PayrollID" runat="server"></asp:Literal>
                                        <br />
                                        Avg Hours/month:
       
                                            <asp:Literal ID="Lit_io_MonthlyAvg" runat="server"></asp:Literal>
                                        <br />
                                    </div>
                                    <div style="float: right; width: 445px;">
                                        <h3>Edit/Change Event</h3>
                                        <asp:Label ID="lbl_io_ChangeEvent" runat="server" CssClass="lbl" Text="Edit/Change Event"></asp:Label>
                                        <asp:DropDownList ID="Ddl_io_ChangeEvent" runat="server" CssClass="ddl2" AutoPostBack="true" OnSelectedIndexChanged="Ddl_io_ChangeEvent_SelectedIndexChanged">
                                            <asp:ListItem Text="Change Event" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Correction" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Select" Value="0" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                        <br />
                                        <asp:Label ID="Lbl_io_ChangeEventDate" runat="server" CssClass="lbl" Text="Date of Change" Visible="false"></asp:Label>
                                        <asp:TextBox ID="Txt_io_ChangeDate" runat="server" CssClass="txt3" Visible="false"></asp:TextBox>
                                    </div>
                                    <br style="clear: left;" />
                                    <hr />
                                    <br />
                                    <div style="padding-bottom: 20px; width: 595px;">
                                        <div style="float: left; width: 345px;">
                                            #1) Was this employee offered insurance?
       
                                                <br />
                                            <asp:DropDownList ID="Ddl_io_Offered" runat="server" CssClass="ddl2" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="Ddl_io_Offered_SelectedIndexChanged">
                                                <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="false"></asp:ListItem>
                                                <asp:ListItem Text="Select" Value="100" Selected="True"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div style="float: right; width: 250px; text-align: left;">
                                            <asp:Panel ID="Pnl_io_DateOffered" runat="server">
                                                #2) Enter Date Offered (Deprecated)
                                                    <br />
                                                <asp:TextBox ID="Txt_io_DateOffered" runat="server" CssClass="txt" Enabled="false"></asp:TextBox>
                                                <asp:CalendarExtender ID="CExtDateOffered" runat="server" TargetControlID="Txt_io_DateOffered" DefaultView="Days"></asp:CalendarExtender>
                                            </asp:Panel>
                                        </div>
                                    </div>
                                    <br />
                                    <br />
                                    <asp:Panel ID="Pnl_io_Accepted" runat="server">
                                        <div style="padding-bottom: 20px; width: 595px;">
                                            <div style="float: left; width: 345px;">
                                                #3) Did this employee accept the offer insurance?
       
                                                    <br />
                                                <asp:DropDownList ID="Ddl_io_Accepted" runat="server" CssClass="ddl2" Enabled="false">
                                                    <asp:ListItem Text="Select" Selected="true"></asp:ListItem>
                                                    <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                                                    <asp:ListItem Text="No" Value="false"></asp:ListItem>
                                                </asp:DropDownList>
                                            </div>
                                            <div style="float: right; width: 250px; text-align: left;">
                                                #4) Date Accepted/Declined: (Deprecated)
   
                                                    <br />
                                                <asp:TextBox ID="Txt_io_AcceptedOffer" runat="server" CssClass="txt" Enabled="false"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender6" runat="server" TargetControlID="Txt_io_AcceptedOffer" DefaultView="Days"></asp:CalendarExtender>
                                            </div>
                                        </div>
                                        <br />
                                        <br />
                                    </asp:Panel>

                                    <asp:Panel ID="Pnl_io_Plan" runat="server">
                                        <div style="padding-bottom: 20px; width: 595px;">
                                            <div style="float: left; width: 345px;">
                                                <asp:Panel ID="PnlInsurancePlanOffered" runat="server">
                                                    #5) Which Plan was
                                                        <asp:Literal ID="Literal2" runat="server"></asp:Literal>
                                                    offered?      
                                                        <br />
                                                    <asp:DropDownList ID="Ddl_io_InsurancePlan" runat="server" CssClass="ddl2" Enabled="false" AutoPostBack="true" OnSelectedIndexChanged="Ddl_io_InsurancePlan_SelectedIndexChanged"></asp:DropDownList>
                                                </asp:Panel>
                                            </div>
                                            <div style="float: right; width: 250px; text-align: left;">
                                                #6) Effective Date of Insurance      
                                                    <br />
                                                <asp:TextBox ID="Txt_io_InsuranceEffectiveDate" runat="server" CssClass="txt" Enabled="false"></asp:TextBox>
                                                <asp:CalendarExtender ID="CalendarExtender7" runat="server" TargetControlID="Txt_io_InsuranceEffectiveDate" DefaultView="Days"></asp:CalendarExtender>
                                            </div>
                                        </div>
                                        <br />
                                        <br />
                                    </asp:Panel>
                                    <asp:Panel ID="Pnl_io_Effective" runat="server">
                                        <div style="padding-bottom: 20px; width: 595px;">
                                            <div style="float: left; width: 345px;">
                                                #7) Select EMPLOYER Contribution:
       
                                                    <br />
                                                <asp:DropDownList ID="Ddl_io_Contribution" runat="server" CssClass="ddl2" Enabled="false"></asp:DropDownList>
                                            </div>
                                            <div style="float: right; width: 250px; text-align: left;">
                                                #8) Enter any additional HRA/Flex Monthly Contributions:
       
                                                    <br />
                                                <asp:TextBox ID="Txt_io_HraFlex" runat="server" CssClass="txt" Enabled="false"></asp:TextBox>
                                            </div>
                                        </div>
                                        <br />
                                        <br />
                                    </asp:Panel>
                                    <br />
                                    Explanation/Notes:
                                        <br />
                                    <asp:TextBox ID="Txt_io_Comments" runat="server" TextMode="MultiLine" Height="100px" Width="500px" Enabled="false"></asp:TextBox>
                                    <br />
                                    <asp:Button ID="Btn_io_update" runat="server" Text="Update" CssClass="btn" Visible="false" OnClick="Btn_io_update_Click" />
                                    <br />
                                    <asp:Label ID="LblInsuranceMessage" runat="server" BackColor="Yellow"></asp:Label>
                                    <br />
                                    <br />
                                </div>
                            </asp:Panel>
                            <asp:HiddenField ID="HfDummyIO" runat="server" />
                            <asp:HiddenField ID="HfDummyDep" runat="server" />
                            <asp:HiddenField ID="HfDummyPayroll2" runat="server" />
                            <asp:HiddenField ID="HfDummyPayroll" runat="server" />
                            <asp:ModalPopupExtender ID="MpeDependents" runat="server" TargetControlID="HfDummyDep" OkControlID="ImgBtnCloseDependents" PopupControlID="PnlDependents"></asp:ModalPopupExtender>
                            <asp:ModalPopupExtender ID="MpePayrollDetails" runat="server" TargetControlID="HfDummyPayroll" OkControlID="ImgBtnClosePR" PopupControlID="PnlPayrollDetails"></asp:ModalPopupExtender>
                            <asp:ModalPopupExtender ID="MpeNewPayroll" runat="server" TargetControlID="HfDummyPayroll2" OkControlID="ImgBtnCloseAP" PopupControlID="PnlAddPayroll"></asp:ModalPopupExtender>
                            <asp:ModalPopupExtender ID="mpeEditInsurance" runat="server" TargetControlID="HfDummyIO" PopupControlID="PnlInsuranceOffer"></asp:ModalPopupExtender>
                        </div>
                    </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpEmployee" DynamicLayout="true" DisplayAfter="500">
                <ProgressTemplate>
                    <div style="position: fixed; top: 0; left: 0; background-color: white; width: 100%; height: 100%; opacity: .85; filter: alpha(opacity=85); -moz-opacity: 0.85; text-align: center;">
                        <div style="position: relative; margin-left: auto; margin-right: auto; background-color: white; padding-top: 100px;">
                            <h4>Calculating the EMPLOYEE data..... This may take a minute.....</h4>
                            <asp:Image ID="ImgSearching" runat="server" ImageUrl="~/design/icon-loading-animated.gif" />
                        </div>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
    </div>

    <div style="clear: both;">
        &nbsp;
    </div>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
</asp:Content>
