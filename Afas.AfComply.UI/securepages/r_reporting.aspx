<%@ Page Title="" Language="C#" MasterPageFile="~/securepages/SecurePages.Master" AutoEventWireup="true" CodeBehind="r_reporting.aspx.cs" Inherits="securepages_r_reporting" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="bodycontent" runat="server">

    <style type="text/css">
        .lbl {
            width: 150px;
            float: left;
            text-align: right;
            clear: left;
            padding: 5px;
            background-color: lightgray;
            height: 15px;
        }

        .lbl2 {
            padding: 5px;
        }

        .txt {
            padding: 5px;
            width: 50px;
            height: 15px;
        }
    </style>

    <iframe style="width: 100%; height: 1400px" src="<%= MyValue %>"></iframe>

</asp:Content>
