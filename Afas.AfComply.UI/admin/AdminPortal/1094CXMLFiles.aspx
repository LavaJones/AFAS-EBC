<%@ Page Title="" Language="C#" MasterPageFile="~/admin/AdminPortal/AdminPortal.Master" AutoEventWireup="true" 
    CodeBehind="1094CXMLFiles.aspx.cs" Inherits="Afas.AfComply.UI.admin.AdminPortal._1094CXMLFiles" %>
<%@ Import Namespace="Afas.AfComply.Domain" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    
    <h2>1094C XML</h2>
    <br />
    <label class="lbl3">Select Employer</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged"  AutoPostBack="true"></asp:DropDownList>
    <br />
    <asp:Button ID="BtnDownload" runat="server" Text="Download" Width="125" CssClass="btn"  OnClick="BtnDownload_Click" />
     <br />
    <asp:Label ID="lblMessage" runat="server" Text="0 Files" Width="125" Visible="false"/>
     <br />
 </asp:Content>