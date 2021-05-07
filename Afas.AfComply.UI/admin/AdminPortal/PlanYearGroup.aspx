<%@ Page EnableSessionState="ReadOnly" Title="Plan Year Group" Language="C#"
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="PlanYearGroup.aspx.cs"
    Inherits="Afas.AfComply.UI.admin.AdminPortal.PlanYearGroupPage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" AsyncPostBackTimeout="3600"></asp:ToolkitScriptManager>

    <h2>Plan Year Group</h2>
    <br />
    <label class="lbl3">Select Employer</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    <br />
    <asp:Label ID="cofein" runat="server"></asp:Label>
    <br />

    <asp:HiddenField ID="HiddenEmployerId" runat="server" />

    Add New Plan Year Group:
    <asp:TextBox ID="TxtNewGroupName" runat="server" ></asp:TextBox>
    <asp:Button ID="BtnNewType" runat="server" Text="Save" OnClick="BtnNewType_Click" />

    <asp:GridView ID="Gv_gv_Groups" runat="server" AutoGenerateColumns="false"
        EmptyDataText="There are currently NO Plan Year Groups setup for this employer!" BackColor="White" BorderColor="#336666"
        BorderStyle="Double" BorderWidth="3px" CellPadding="4" GridLines="Horizontal"
        OnRowDeleting="Gv_gv_Groups_RowDeleting"
        OnRowEditing="Gv_gv_Groups_RowEditing"
        OnRowCancelingEdit="Gv_gv_Groups_RowCancelingEdit"
        OnRowUpdating="Gv_gv_Groups_RowUpdating">
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

            <asp:TemplateField HeaderText="Group Name" HeaderStyle-Width="225px" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:HiddenField ID="HiddenId" runat="server" Value='<%# Eval("PlanYearGroupId") %>' />
                    <asp:Literal ID="LitGroupName" runat="server" Text='<%# Eval("GroupName") %>'></asp:Literal>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:HiddenField ID="HiddenId" runat="server" Value='<%# Eval("PlanYearGroupId") %>' />
                    <asp:TextBox ID="TxtGroupName" runat="server" Text='<%# Eval("GroupName") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderStyle-Width="25px" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:ImageButton Width="25px" ID="ImgBtnDelete" runat="server" ImageUrl="~/images/close_box_red.png" Height="20px" CommandName="Delete" ToolTip="Delete this Plan Year Group" />
                    <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="ImgBtnDelete" ConfirmText="Are you sure you want to DELETE this Plan Year Group?"></asp:ConfirmButtonExtender>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <br />
    <asp:Label ID="lblMsg" runat="server"></asp:Label>

</asp:Content>
