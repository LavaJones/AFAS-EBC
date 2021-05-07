<%@ Page Title="" Language="C#" MasterPageFile="~/admin/AdminPortal/AdminPortal.Master" AutoEventWireup="true" CodeBehind="InactiveEmployeeAveHours.aspx.cs" Inherits="Afas.AfComply.UI.admin.AdminPortal.InactiveEmployeeAveHours" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
       <h2>Inactive Employee Hours</h2>
    <br />
    <label class="lbl3">Select Employer</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    <br />
    <asp:Label ID="cofein" runat="server"></asp:Label>
    <br />
    <asp:Button ID="BtnRun" runat="server" CssClass="btn"  Text="Inactive" OnClick="BtnRun_Click" />
    <br />
    <asp:Label ID="lblMsg" runat="server"></asp:Label>    


</asp:Content>
