<%@ Page EnableSessionState="ReadOnly" Title="Classification Insurance" Language="C#"
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="employee_classification_insurance.aspx.cs"
    Inherits="Afas.AfComply.UI.admin.AdminPortal.securepages_employee_classification_insurance" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" AsyncPostBackTimeout="3600"></asp:ToolkitScriptManager>
    
    <asp:HiddenField ID="HfDistrictID" runat="server" />
    
    <asp:UpdatePanel ID="UpEmployerView" runat="server">
        <ContentTemplate>

            <div id="topbox">
                <h4>Employee Classification Insurance</h4>

                <label class="lbl">Select Employer</label>
                <asp:DropDownList ID="DdlFilterEmployers" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlFilterEmployers_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                <br />

                <label class="lbl">Select Year</label>
                <asp:DropDownList ID="DdlPlanYearCurrent" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlPlanYearCurrent_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                <br />
                <br />
                <asp:GridView ID="gvEmployeeClassificationInsurance" runat="server" AutoGenerateColumns="false"
                    HeaderStyle-Height="30" RowStyle-Height="30" CellPadding="5" BorderColor="White">
                    <Columns>
                        <asp:TemplateField HeaderText="Class_ID" HeaderStyle-Width="25px" SortExpression="classification_id" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Literal ID="LitClassificationID" runat="server" Text='<%# Eval("classification_id") %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Class_Description" HeaderStyle-Width="50%" SortExpression="classification_description" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Literal ID="LitClassificationDescription" runat="server" Text='<%# Eval("classification_description") %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ins_ID" HeaderStyle-Width="25px" SortExpression="insurance_id" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="LitInsuranceID" runat="server" Text='<%# Eval("insurance_id") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ins_Type" HeaderStyle-Width="50px" SortExpression="insurance_id" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Label ID="LitInsuranceType" runat="server" Text='<%# Eval("insurance_type") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ins_Description" HeaderStyle-Width="50%" SortExpression="insurance_description" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Literal ID="LitInsuranceDescription" runat="server" Text='<%# Eval("insurance_description") %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Contribution" HeaderStyle-Width="75px" SortExpression="amount" HeaderStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <asp:Literal ID="LitAmount" runat="server" Text='<%# Eval("employee_contribution") %>'></asp:Literal>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
