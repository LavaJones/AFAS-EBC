<%@ Page Title="" Language="C#" MasterPageFile="~/admin/AdminPortal/AdminPortal.Master" AutoEventWireup="true"
     CodeBehind="EmployerExportToCSV.aspx.cs" Inherits="Afas.AfComply.UI.admin.EmployerExportToCSV" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    
    <h2>Employer Export</h2>
    <br />
    <br />
    <asp:Button ID="BtnDownload" runat="server" Text="Download All Employers Info" Width="300" CssClass="btn"  OnClick="BtnDownload_Click" />
    
 </asp:Content>


