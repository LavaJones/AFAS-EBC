<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="_Error" %>
<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<!DOCTYPE html>
<html>
<head>
    <title><%= Branding.ProductName %> - <%= Branding.CompanyName %></title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link rel="stylesheet" type="text/css" href="/default.css" />
</head>
<body>
    <div id="container ">
        <div id="header">
            <a href="#">
                <img src="<%= Page.ResolveUrl(Branding.LogoUri) %>" style="height: 70px" alt="<%= Branding.ProductName %> Logo" class="logo" />
            </a>
            <ul id="toplinks">
                <li>Need Help? Call <%= Branding.PhoneNumber %></li>
            </ul>
        </div>
        <div id="nav">
            <ul>
                <li><a href="/">Home</a></li>
            </ul>
        </div>
        <div id="topbox">
            <hgroup>
                <h1>Site Status</h1>
                <h2>An error occurred while processing your request.</h2>
            </hgroup>
        </div>
    </div>
    <div id="footer">
        Copyright &copy; <%= Branding.CopyrightYears %> <a href="<%= Branding.CompanyWebSite %>"><%= Branding.CompanyName %></a> - All Rights Reserved
       
        <div style="clear: both;">&nbsp;</div>
    </div>
</body>
</html>

