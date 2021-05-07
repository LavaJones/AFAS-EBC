<%@ Page Title="" Language="C#" MasterPageFile="~/securepages/SecurePages.Master" AutoEventWireup="true" CodeBehind="NoticeInformation.aspx.cs" Inherits="NoticeInformation" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="bodycontent" runat="server">
    
    <link rel="stylesheet" type="text/css" href="/leftnav.css" />

    <asp:HiddenField ID="HfDistrictID" runat="server" />

    <div style="margin-left: 286px; padding: 20px;">
        <asp:Literal id="NoticeText" 
               Text="Failed to load Notice."
               runat="server" />
    </div>

    <div class="left_ebc" style="width: 286px; top: 0; position: absolute;  background-color: transparent;">
        <%= demo.getLeftLinks(null != Session["CurrentDistrict"] && ((employer)Session["CurrentDistrict"]).IrsEnabled) %>
    </div>

</asp:Content>