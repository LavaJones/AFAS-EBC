<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotUsername.aspx.cs" Inherits="Afas.AfComply.UI.ForgotUsername" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<link href="Content/bootstrap.min.css" rel="stylesheet" />
<link href="Content/bootstrap.css" rel="stylesheet" />
<script src="Scripts/popper.js"></script>
<script src="Scripts/jquery-1.10.2.min.js"></script>
<script src="Scripts/bootstrap.js"></script>
<style>
    #dvheader {
        background-color: #eb0029;
        height: 80px;
    }

    #Compnay_logo {
        padding-left: 43%;
        height: 110px;
    }

    #ForgotUsername-form {
        padding-left: 41%;
        line-height: 2.6;
    }

        #ForgotUsername-form #BtnRetrieveUsername {
            position: relative;
            top: 2px;
            left: 67px;
            width: 147px;
            background-color: #eb0029;
            color: white;
            border: 3px solid #f44336;
            height: 44px;
        }

    #Heading {
        color: #eb0029;
        padding-left: 790px;
    }

    #footer {
        position: relative;
        margin-top: 260px;
        /* negative value of footer height */
        height: 180px;
        clear: both;
        text-align: center;
    }

        #footer a {
            color: #eb0029;
        }
</style>



<form id="ForgotUsername" runat="server">
    <div id="dvheader">
    </div>

    <br />

    <div id="Compnay_logo">
        <a href="#">
            <img src="<%= Page.ResolveUrl(Branding.LogoUri) %>" alt="<%= Branding.ProductName %> Logo" class="logo" />
        </a>

    </div>

    <div id="ForgotUsername-form">

        <p>
            <asp:label id="Label1" runat="server" text="Label" font-bold="true">Email Address:</asp:label>
            <br />
            <asp:textbox id="TxtUNEmail" runat="server" placeholder="Enter Email Address" cssclass="form-control input-lg" width="277px"></asp:textbox>
        </p>
        <p>
            <asp:label id="LblUserMessage2" runat="server"></asp:label>
        </p>
        <p>
            <asp:label id="LblUserMessage3" runat="server"></asp:label>
        </p>
        <p>
        </p>
        <p>
            <asp:button id="BtnRetrieveUsername" runat="server" text="Submit" onclick="BtnRetrieveUsername_Click" />
        </p>







    </div>

    <div id="footer">
        Copyright &copy; <%= Branding.CopyrightYears %> <a href="<%= Branding.CompanyWebSite %>"><%= Branding.CompanyName %></a> - All Rights Reserved   
            <br />

        <div style="clear: both;">&nbsp;</div>
    </div>



</form>
