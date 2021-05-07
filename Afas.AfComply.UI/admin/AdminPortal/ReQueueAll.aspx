<%@ Page EnableSessionState="ReadOnly" Title="Requeue all" Language="C#" 
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="ReQueueAll.aspx.cs" 
    Inherits="Afas.AfComply.UI.admin.AdminPortal.ReQueueAll" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">


    <h2>Dump To Log</h2>
    <br />
    <asp:Button ID="BtnReQueue" runat="server" CssClass="btn" Text="ReQueue" OnClick="BtnReQueue_Click" />
    <br />
    <asp:Label ID="LblFileUploadMessage" runat="server"></asp:Label>
    

</asp:Content>