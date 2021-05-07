<%@ Page Title="" Language="C#" MasterPageFile="~/admin/AdminPortal/AdminPortal.Master" AutoEventWireup="true"
     CodeBehind="ReportPortalStatus.aspx.cs" Inherits="Afas.AfComply.UI.admin.ReportPortalStatus" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <asp:ToolkitScriptManager ID="ToolkitScriptManager" runat="server"></asp:ToolkitScriptManager>
    
    <h2>Export Portal Status Report</h2>
    <br />
    <br />
    <asp:Button ID="BtnDownloadReport" runat="server" Text="Report all employer status" Width="300" CssClass="btn"  OnClick="BtnDownload_Click" />
    
 </asp:Content>


