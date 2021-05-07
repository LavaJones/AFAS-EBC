<%@ Page EnableSessionState="ReadOnly" Title="Error Log" Language="C#" 
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="ErrorLog.aspx.cs" 
    Inherits="Afas.AfComply.UI.admin.AdminPortal.ErrorLog" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">


    <h2>Dump To Log</h2>
    <br />
    <asp:Button ID="BtnDump" runat="server" CssClass="btn" Text="Dump" OnClick="BtnDump_Click" />
    <br />
    <asp:Label ID="LblFileUploadMessage" runat="server"></asp:Label>
    

</asp:Content>