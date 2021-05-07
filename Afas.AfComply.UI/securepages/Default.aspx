<%@ Page Title="" Language="C#" MasterPageFile="~/securepages/SecurePages.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Afas.AfComply.UI.securepages.Default" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="bodycontent" runat="server">

    <style>
        iframe {
            border: 0;
            position: absolute;
            width: -webkit-calc(100% - 286px);
            width: -moz-calc(100% - 286px);
            width: calc(100% - 286px);
            height: -webkit-calc(100% - 156px);
            height: -moz-calc(100% - 156px);
            height: calc(100% - 156px);
            margin-left: 286px;
        }
    </style>


    <link rel="stylesheet" type="text/css" href="/leftnav.css" />


    <asp:HiddenField ID="HfDistrictID" runat="server" />


    <%if (Feature.HomePageMessageEnabled)
        { %>
    <div class="SystemMessage" style="margin-left: 286px; background-color: #ffffcc; padding: 10px; box-shadow: 3px 3px 5px 6px #ccc;">
        <p>
            <%= Feature.HomePageMessage %>
            <%if (Feature.HomePageMessageLink != null && Feature.HomePageMessageLink != "")
                { %>
            <a href="<%= Feature.HomePageMessageLink %>" target="_blank">Click here for more details.</a>
            <% } %>
        </p>
    </div>
    <br />
    <% } %>


    <iframe src="<%= MyValue %>">
    </iframe>


    <div class="left_ebc" style="width: 286px; top: 0; position: absolute; background-color: transparent;">
        <%= demo.getLeftLinks(null != Session["CurrentDistrict"] && ((employer)Session["CurrentDistrict"]).IrsEnabled) %>
    </div>


</asp:Content>
