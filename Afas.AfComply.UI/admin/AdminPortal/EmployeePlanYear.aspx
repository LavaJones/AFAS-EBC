<%@ Page EnableSessionState="ReadOnly" Title="" Language="C#" MasterPageFile="AdminPortal.Master" AutoEventWireup="true"
    CodeBehind="EmployeePlanYear.aspx.cs" Inherits="Afas.AfComply.UI.admin.AdminPortal.EmployeePlanYear" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <h2>Select an Employer</h2>

    <br />
    <label class="lbl3">Select Employer</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    <br />
    <asp:Label ID="cofein" runat="server"></asp:Label>

    <h2>RollBACK Measurement</h2>
    <br />
    <label class="lbl3">Current Plan Year</label>
    <asp:DropDownList ID="DdlPlanYearCurrent" runat="server" CssClass="ddl2"></asp:DropDownList>
    <br />
    <br />
    <label class="lbl3">RollBack to Plan Year</label>
    <asp:DropDownList ID="DdlPlanYearNew" runat="server" CssClass="ddl2"></asp:DropDownList>
    <br />
    <br />
    <asp:Button ID="BtnProcessFile" runat="server" Text="ROLLBACK Data" CssClass="btn" OnClick="BtnProcessFile_Click" />
    <br />
    <br />



    <h2>Choose Correct Plan Years</h2>
    <label>Change All Employees</label>
    <asp:DropDownList ID="Ddl_gv_PlanYears_All" runat="server" OnSelectedIndexChanged="PlanYears_All_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    <asp:CheckBox ID="ShowAllEmployees" Checked="true" runat="server" AutoPostBack="true" Text="Show All Employees" OnCheckedChanged="DdlEmployer_SelectedIndexChanged" />
    <asp:GridView ID="GvPlanYearErrors" runat="server" AutoGenerateColumns="false"
        EmptyDataText="No Errors Exist" Width="95%">
        <Columns>
            <asp:TemplateField HeaderText="Employee Name">
                <ItemTemplate>
                    <asp:Literal ID="Lit_gv_name" runat="server" Text='<%# Eval("EMPLOYEE_FULL_NAME") %>'></asp:Literal>
                    <asp:HiddenField ID="Hf_gv_id" runat="server" Value='<%# Eval("EMPLOYEE_ID") %>' />
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Plan Year">
                <ItemTemplate>
                    <asp:HiddenField ID="Hf_gv_measID" runat="server" Value='<%# Eval("EMPLOYEE_PLAN_YEAR_ID_MEAS") %>' />
                    <asp:DropDownList ID="Ddl_gv_PlanYears" runat="server"></asp:DropDownList>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <br />
    <asp:Button ID="BtnFix" runat="server" CssClass="btn" Text="Submit" OnClick="BtnFix_Click" />
    <br />
    <asp:Label ID="lblMsg" runat="server"></asp:Label>

</asp:Content>
