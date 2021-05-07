<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Forgotpassword.aspx.cs" Inherits="Afas.AfComply.UI.Forgotpassword" %>

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

    #Heading {
        color: #eb0029;
        padding-left: 790px;
    }

    #Forgotpassword-form {
        padding-left: 41%;
        line-height: 2.6;
    }

        #Forgotpassword-form #BtnSaveNewPassword {
            position: relative;
            top: 0px;
            left: 29px;
            width: 203px;
            background-color: #eb0029;
            color: white;
            border: 3px solid #f44336;
        }

    #footer a {
        color: #eb0029;
    }

    #footer {
        position: relative;
        margin-top: 260px;
        /* negative value of footer height */
        height: 180px;
        clear: both;
        text-align: center;
    }
</style>
<form id="Forgotpassword" runat="server">

    <div id="dvheader">
    </div>

    <div id="Compnay_logo">
        <a href="#">
            <img src="<%= Page.ResolveUrl(Branding.LogoUri) %>" alt="<%= Branding.ProductName %> Logo" class="logo" />
        </a>
    </div>









    <div id="Forgotpassword-form">
        <p>
            <asp:label id="Label1" runat="server" text="Label" font-bold="true">User Name:</asp:label>
            <br />
            <asp:textbox id="TxtResetUsername" runat="server" placeholder="Enter User Name" cssclass="form-control input-lg" width="300px"></asp:textbox>
        </p>

        <p>
            <asp:label id="Label2" runat="server" text="Label" font-bold="true">Email Address:</asp:label>
            <br />
            <asp:textbox id="TxtResetEmail" runat="server" placeholder="Enter Email Address" cssclass="form-control input-lg" width="300px"></asp:textbox>
        </p>
        <p>
            <asp:label id="LblNewUserMessage" runat="server"></asp:label>
        </p>
        <p>
            <asp:label id="LblNewUserMessage1" runat="server"></asp:label>
        </p>
        <p>
            <asp:button id="BtnSaveNewPassword" runat="server" text="Submit" onclick="BtnSaveNewPassword_Click" />
        </p>
    </div>
    <div id="footer">
        Copyright &copy; <%= Branding.CopyrightYears %> <a href="<%= Branding.CompanyWebSite %>"><%= Branding.CompanyName %></a> - All Rights Reserved   
            <br />

        <div style="clear: both;">&nbsp;</div>
    </div>

</form>
