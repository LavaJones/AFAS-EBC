<%@ Page EnableSessionState="ReadOnly" Title="Employee Type" Language="C#"
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="EmployeeType.aspx.cs"
    Inherits="Afas.AfComply.UI.admin.AdminPortal.EmployeeType" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" AsyncPostBackTimeout="3600"></asp:ToolkitScriptManager>

    <h2>Employee Type</h2>
    <br />
    <label class="lbl3">Select Employer</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    <br />
    <asp:Label ID="cofein" runat="server"></asp:Label>
    <br />

    <asp:HiddenField ID="HiddenEmployerId" runat="server" />

    Add New Employee Type:
    <asp:TextBox ID="TxtNewTypeName" runat="server" ></asp:TextBox>
    <asp:Button ID="BtnNewType" runat="server" Text="Save" OnClick="BtnNewType_Click" />

    <asp:GridView ID="Gv_gv_Types" runat="server" AutoGenerateColumns="false"
        EmptyDataText="There are currently NO Employee Types setup for this employer!" BackColor="White" BorderColor="#336666"
        BorderStyle="Double" BorderWidth="3px" CellPadding="4" GridLines="Horizontal"
        OnRowDeleting="Gv_gv_Types_RowDeleting"
        OnRowEditing="Gv_gv_Types_RowEditing"
        OnRowCancelingEdit="Gv_gv_Types_RowCancelingEdit"
        OnRowUpdating="Gv_gv_Types_RowUpdating">
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
                    <asp:LinkButton ID="LnkBtnEdit" runat="server" Text="Edit" CommandName="Edit"></asp:LinkButton>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:LinkButton ID="LnkBtnUpdate" runat="server" Text="Update" CommandName="Update"></asp:LinkButton>
                    <asp:LinkButton ID="LnkBtnCancel" runat="server" Text="Cancel" CommandName="Cancel"></asp:LinkButton>
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Type Name" HeaderStyle-Width="225px" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:HiddenField ID="HiddenTypeId" runat="server" Value='<%# Eval("EMPLOYEE_TYPE_ID") %>' />
                    <asp:Literal ID="LitTypeName" runat="server" Text='<%# Eval("EMPLOYEE_TYPE_NAME") %>'></asp:Literal>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:HiddenField ID="HiddenTypeId" runat="server" Value='<%# Eval("EMPLOYEE_TYPE_ID") %>' />
                    <asp:TextBox ID="TxtTypeName" runat="server" Text='<%# Eval("EMPLOYEE_TYPE_NAME") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderStyle-Width="25px" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:ImageButton Width="25px" ID="ImgBtnDelete" runat="server" ImageUrl="~/images/close_box_red.png" Height="20px" CommandName="Delete" ToolTip="Delete this Employee Type" />
                    <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="ImgBtnDelete" ConfirmText="Are you sure you want to DELETE this Employee Type?"></asp:ConfirmButtonExtender>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <br />
    <asp:Label ID="lblMsg" runat="server"></asp:Label>

</asp:Content>
