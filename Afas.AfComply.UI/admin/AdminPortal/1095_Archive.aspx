<%@ Page Title="" Language="C#" EnableSessionState="ReadOnly" MasterPageFile="~/admin/AdminPortal/AdminPortal.Master" AutoEventWireup="true" CodeBehind="1095_Archive.aspx.cs"
    Inherits="Afas.AfComply.UI.admin.AdminPortal._1095_Archive" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <h2>1095 Archive</h2>
    <br />
    <label class="lbl3">Select Year</label>
    <asp:DropDownList ID="DdlYears" runat="server" CssClass="ddl2">
        <asp:ListItem Text="2016" Value="2016"></asp:ListItem>
        <asp:ListItem Text="2017" Value="2017"></asp:ListItem>
        <asp:ListItem Text="2018" Value="2018"></asp:ListItem>
    </asp:DropDownList>
    <br />
    <br />
    <asp:Button ID="BtnArchive" runat="server" Text=" Archive1095" OnClick="Archive1095_Click" />
    <br />
    <br />
     <asp:label id="LblMessage" runat="server"></asp:label>
</asp:Content>
