<%@ Page Title="" Language="C#" MasterPageFile="~/admin/AdminPortal/AdminPortal.Master" AutoEventWireup="true" CodeBehind="1095_Upload.aspx.cs" Inherits="Afas.AfComply.UI.admin.AdminPortal._1095_Upload" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>

<h2> 1095 uploadc</h2>
<br />

        <asp:FileUpload ID="Pdf" runat="server" Width="350px" />
        <br />
        <br />
      <asp:Button ID="BtnUploadFile" runat="server" CssClass="btn" Text="Submit" OnClick="BtnUploadFile_Click" Height="27px" />
      <br />
       <br />
    <asp:Label ID="LblFileUploadMessage" runat="server"></asp:Label>
    <br />
       <br />
        <asp:Label ID="LblFileUploadErrorMessage" runat="server"></asp:Label>
 </asp:Content>
