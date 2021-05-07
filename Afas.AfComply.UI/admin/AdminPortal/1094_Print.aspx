<%@ Page Title="" Language="C#" MasterPageFile="~/admin/AdminPortal/AdminPortal.Master" AutoEventWireup="true" CodeBehind="1094_Print.aspx.cs" 
    Inherits="Afas.AfComply.UI.admin.AdminPortal._1094_Print" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>

<h2>1094 Print</h2>
<br />
<label class="lbl3">Select Year</label>
<asp:DropDownList ID ="DdlYears" runat="server" CssClass="ddl2">
 <asp:ListItem Text="2017" Value="2017"></asp:ListItem>
 <asp:ListItem Text="2018" Value="2018"></asp:ListItem>
</asp:DropDownList>
<br/>

 <asp:Button ID="BtnPrint" runat="server" Text=" Print1094" OnClick="Print1094_Click" />
</asp:Content>