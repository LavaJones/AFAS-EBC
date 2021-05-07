<%@ Page EnableSessionState="ReadOnly" Title="" Language="C#" 
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="InsertDeleteLog.aspx.cs" 
    Inherits="Afas.AfComply.UI.admin.AdminPortal.InsertDeleteLog" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" AsyncPostBackTimeout="3600"></asp:ToolkitScriptManager>
    
    <h2>Export Insert/Delete Log</h2>
    <br />
    <label class="lbl3">Pick Insert/Delete Log</label>
    <asp:DropDownList ID="DdlLog" runat="server" CssClass="ddl">
                                        <asp:ListItem Text="Covered Individual Log" Value="1"  Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Covered Individual Month Log" Value="2" ></asp:ListItem>
                                        <asp:ListItem Text="Employee Monthly Log" Value="3" ></asp:ListItem>
                                        <asp:ListItem Text="Employee Yearly Log" Value="4" ></asp:ListItem>
                                        <asp:ListItem Text="Tax Year 1095c Log" Value="5" ></asp:ListItem>
                                        <asp:ListItem Text ="Ale Employer Log" Value ="6" ></asp:ListItem>
                                        <asp:ListItem Text ="Emp Employee Log" Value ="7" ></asp:ListItem>
                                    </asp:DropDownList>
    <br />
    <br />
    <label class="lbl4">Input Employer ID</label>
    <asp:TextBox ID="txtEmployerId" runat="server" MaxLength="6" CssClass="txtSmall"></asp:TextBox>
    <asp:RegularExpressionValidator ID ="regularExpressionEmployerId" ControlToValidate ="txtEmployerId" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+"></asp:RegularExpressionValidator>
    <br />
    <br />

    <asp:Button ID="BtnExport" runat="server" CssClass="btn" Text="Export Log" OnClick="BtnExport_Click" />
    <br />

</asp:Content>
