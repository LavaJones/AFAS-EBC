<%@ Page EnableSessionState="ReadOnly" Title="Qlik Access" Language="C#" 
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="QlikAccess.aspx.cs" 
    Inherits="Afas.AfComply.UI.admin.AdminPortal.QlikAccess" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    
    
    <h2>Qlik Demo</h2>

    <iframe style="width:100%;height:1400px" src="<%= MyValue %>"></iframe>
    
</asp:Content>