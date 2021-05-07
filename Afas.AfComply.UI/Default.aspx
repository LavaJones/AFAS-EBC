<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="_Default" %>

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

    #Login-form #ForgotPassword {
        margin-left: 204px;
        color: #eb0029;
    }

    #Login-form #ForgetUserName {
        margin-left: 200px;
        color: #eb0029;
    }

    #Login-form {
        padding-left: 41%;
        line-height: 2.0;
    }

    #Dataonloginpage h1, h3 {
        text-align: justify;
        color: #eb0029;
    }

    #Dataonloginpage p {
        text-align: justify;
    }

    #Dataonloginpage {
        padding: 31px;
        width: 50%;
        left: 50%;
        top: 50%;
        text-align: center;
        margin: auto;
        margin-left: 440px;
    }

    #Login-form #BtnLogin {
        position: relative;
        top: 0px;
        left: 33px;
        width: 198px;
        background-color: #eb0029;
        color: white;
        border: 3px solid #f44336;
    }

    #Container {
        position: relative;
        width: 1510px;
        padding: 10px;
        overflow: auto;
        min-height: 1500px;
        margin-left: 171px;
    }

</style>

<form id="form1" runat="server" defaultbutton="BtnLogin">



    <div id="dvheader">
    </div>

    <br />
    <div id="Container">
        <div id="Compnay_logo">
            <a href="#">
                <img src="<%= Page.ResolveUrl(Branding.LogoUri) %>" alt="<%= Branding.ProductName %> Logo" class="logo" />
            </a>
        </div>


        <div id="Login-form">


            <p>
                <asp:label id="Label1" runat="server" text="Label" font-bold="true">User Name:</asp:label>
                <br />
                <asp:textbox id="TxtUserName" runat="server" placeholder="Enter User Name" cssclass="form-control input-lg" width="300px"></asp:textbox>

            </p>
            <p>
                <asp:label id="Label2" runat="server" text="Label" font-bold="true">Password:</asp:label>
                <br />
                <asp:textbox id="TxtPassword" runat="server" placeholder="Enter Password" textmode="Password" autocomplete="off" autocompletetype="Disabled" cssclass="form-control input-lg" width="300px"></asp:textbox>
                <br />
            </p>
            <p>
                <asp:label id="LblMessage" runat="server"></asp:label>
            </p>
            <p>
                <asp:button id="BtnLogin" runat="server" text="Login" onclick="BtnLogin_Click" />
            </p>


            <asp:hyperlink id="ForgotPassword" runat="server" navigateurl="~/Forgotpassword.aspx" cssclass="active">Forgot Password</asp:hyperlink>
            <br />

            <asp:hyperlink id="ForgetUserName" runat="server" navigateurl="~/ForgotUsername.aspx" cssclass="active">Forgot User Name</asp:hyperlink>

        </div>
        <%  // Note: this is causing the log spam from the F5, so I've disabled the log message. RCM: 2019-12-18
            if (true == Feature.QlikEnabled)
            {  %>

        <div id="Dataonloginpage">
            <h1>Welcome to the new AFcomply.com!</h1>
            <p>
                We have made improvements to the site to help you navigate easily, readily view account information,
      <br />
                and access our help resources with a single click.
      <br />
                <br />
                As we continue to work to improve the site, we will provide access to additional training on new features.
      <br />
                If you have any questions about these changes or have feedback you would like to share,
      <br />
                you can reach out via our <a href="https://consulting.americanfidelity.com/contact-form/contact-form-folder/contact-us/" style="color: #eb0029;">Contact page.</a>
                <br />
                <br />

            </p>

        </div>
        <% } %>
    </div>
</form>
