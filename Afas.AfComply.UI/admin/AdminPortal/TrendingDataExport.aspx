<%@ Page EnableSessionState="ReadOnly" Title="Trending Data Export" Language="C#" 
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="TrendingDataExport.aspx.cs"
     Inherits="Afas.AfComply.UI.admin.AdminPortal.TrendingDataExport" %>
<%@ Import Namespace="Afas.AfComply.Domain" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <label class="lbl3">Select Employer</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" AutoPostBack="true"></asp:DropDownList>
    
     <br />
    <asp:Button ID="BtnExportToCSV" CssClass="btn" runat="server" Text="Download" OnClick="BtnExportToCSV_Click"/>
</asp:Content>