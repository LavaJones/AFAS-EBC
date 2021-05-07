<%@ Page EnableSessionState="ReadOnly" Title="Employers Current IRS Tax Year Status" Language="C#" 
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="EmployersCurrentIRSTaxYearStatus.aspx.cs" 
    Inherits="Afas.AfComply.UI.admin.AdminPortal.EmployersCurrentIRSTaxYearStatus" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" AsyncPostBackTimeout="3600"></asp:ToolkitScriptManager>

    <h2>Current IRS Status for <%= DateTime.Now.AddYears(-1).Year.ToString() %></h2>
    <br />

    <label class="lbl">Select Employer</label>
    <asp:DropDownList ID="DdlFilterEmployers" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlFilterEmployers_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    <br />

    <asp:HiddenField ID="HiddenField1" runat="server" />
    <br />
    <asp:HiddenField ID="HiddenField2" runat="server" />
    
    <br />
    <asp:GridView ID="gEmployerIRSStatuses" runat="server" AutoGenerateColumns="false"
        HeaderStyle-Height="30" RowStyle-Height="30" CellPadding="5" PageSize="100">
        <Columns>
            <asp:TemplateField HeaderText="Employer Name" HeaderStyle-Width="25%" SortExpression="name" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:Literal ID="LitName" runat="server" Text='<%# Eval("name") %>'></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="FEIN" HeaderStyle-Width="10%" SortExpression="ein" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:Literal ID="LitEin" runat="server" Text='<%# Eval("ein") %>'></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>

             <asp:TemplateField HeaderText="Receipt Id" HeaderStyle-Width="10%" SortExpression="receipt_id" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:Literal ID="LitReceiptId" runat="server" Text='<%# Eval("receipt_id") %>'></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="IRS Status" HeaderStyle-Width="15%" SortExpression="status_code" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:Literal ID="LitIRSStatusCode" runat="server" Text='<%# Eval("status_code") %>'></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Date Time" HeaderStyle-Width="15%" SortExpression="return_time_local" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:Literal ID="LitReturnTimeLocal" runat="server" Text='<%# Eval("return_time_local") %>'></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
           
        </Columns>
    </asp:GridView>

    <br />
    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>


</asp:Content>
