<%@ Page EnableSessionState="ReadOnly" Title="Rescan Alerts" Language="C#" 
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="RescanAlerts.aspx.cs" 
    Inherits="Afas.AfComply.UI.admin.AdminPortal.RescanAlerts" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    
    
    <h2>Fix the data</h2>
    <br />
    <label class="lbl3">Select Employer</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged"  AutoPostBack="true"></asp:DropDownList>
    <br />
    <asp:Label ID="cofein" runat="server"></asp:Label>
    <br />
    <asp:Button ID="BtnRescan" runat="server" CssClass="btn" Text="Rescan Alerts" OnClick="BtnFix_Click" />
    <br />
    <asp:Label ID="lblMsg" runat="server"></asp:Label>    

</asp:Content>