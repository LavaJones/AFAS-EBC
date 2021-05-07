<%@ Page EnableSessionState="ReadOnly" Title="Alert Export" Language="C#" 
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="AlertsExport.aspx.cs"
     Inherits="Afas.AfComply.UI.admin.AdminPortal.AlertsExport" %>
<%@ Import Namespace="Afas.AfComply.Domain" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <asp:Button ID="BtnExportToCSV" CssClass="btn" runat="server" Text="Download" OnClick="BtnExportToCSV_Click"/>
</asp:Content>