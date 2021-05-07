<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="passwordreset.aspx.cs" Inherits="securepages_passwordreset" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <script src="Scripts/popper.js"></script>
    <script src="Scripts/jquery-1.10.2.min.js"></script>
    <script src="Scripts/bootstrap.js"></script>
    <script src="/_js/jquery-ui-1.10.2/jquery-1.9.1.js?v=<%= globalData.getVersion() %>" type="text/javascript"></script>
    <script src="/_js/jquery-ui-1.10.2/ui/jquery-ui.js?v=<%= globalData.getVersion() %>" type="text/javascript"></script>
    <script src="/_js/validation/validation.js?v=<%= globalData.getVersion() %>" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#TxtResetNewPassword').blur(
                function () {
                    var input = $('#TxtResetNewPassword').val();
                    var obj = $('#TxtResetNewPassword');

                    validData = validInitialPassword(obj, input, null);
                });

            $('#TxtResetNewPassword2').blur(
                function () {
                    var input = $('#TxtResetNewPassword').val();
                    var obj = $('#TxtResetNewPassword');

                    var input2 = $('#TxtResetNewPassword2').val();
                    var obj2 = $('#TxtResetNewPassword2');
                    validData = validPassword(obj, input, obj2, input2, null);
                });
        });



    </script>
    <title>Password Change - <%= Branding.ProductName %> - <%= Branding.CompanyName %></title>
    <style>
        #Topcontainer {
            position: relative;
            width: 1510px;
            overflow: auto;
            min-height: 156px;
            left: 0px;
        }

        .header {
            overflow: hidden;
            background-color: #eb0029;
            /*padding: 15px 5px;*/
            height: 10px;
        }

            .header a {
                float: left;
                color: black;
                text-align: center;
                /*padding: 15px;*/
                text-decoration: none;
                list-style-type: none;
                font-size: 30px;
                /*border-radius: 4px;*/
            }

        div.img {
            content: url(C:\Development\Afas.AfComply\branches\Sanja2\Afas.AfComply.UI\images);
        }

        #toplinks {
            /*margin:10px;*/
            text-decoration: none;
            font-size: 5px;
            list-style: none;
            float: right;
        }

        .header a.logo {
            font-size: 25px;
            /*font-weight: bold;*/
        }

        .header a:hover {
            background-color: #ddd;
            color: black;
        }

        .header a.active {
            background-color: dodgerblue;
            color: white;
        }

        .header-right {
            float: right;
        }

        @media screen and (max-width: 500px) {
            .header a {
                float: none;
                display: block;
                text-align: left;
            }

            .header-right {
                float: none;
            }
        }

        .btn {
            width: 103px;
            background-color: #eb0029;
            color: white;
            border: 3px solid #f44336;
        }

        .logo {
            margin: 0;
            float: left;
        }

        #container {
            padding-left: 41%;
            line-height: 2.6;
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

        #container #BtnSubmit {
            position: relative;
            top: 0px;
            left: 29px;
            width: 203px;
            background-color: #eb0029;
            color: white;
            border: 3px solid #f44336;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="header">
            <div class=" image">
            </div>
        </div>

        <div id="Topcontainer">
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>

            <div class="header-right" style="padding-right: 80px;">
                <div id="toplinks">
                    <h1>
                        <p style="font-size: 12px; color: #eb0029">Need Help? Call <%= Branding.PhoneNumber %></p>
                        <p style="font-size: 12px; color: #eb0029">
                            <asp:Literal ID="LitUserName" runat="server"></asp:Literal>
                        </p>
                        <p style="font-size: 12px; color: #eb0029">
                            <asp:Literal ID="LitEmployer" runat="server"></asp:Literal>
                        </p>
                        <p>
                            <asp:HiddenField ID="HfDistrictID" runat="server" />
                        </p>
                    </h1>
                </div>
            </div>
            <div style="padding-left: 20px">
                <a href="/securepages/">
                    <img src="<%= Page.ResolveUrl(Branding.LogoUri) %>" style="height: 70px" alt="<%= Branding.ProductName %> Logo" class="logo" />
                </a>
            </div>
        </div>

        <div id="container">
            <h3>Change Password</h3>
            <p>* Your password was recently reset, please enter a new password different from the one your received in your email.</p>
            <p>
                <asp:Label ID="Label1" runat="server" Text="Label" Font-Bold="true">New Password:</asp:Label><br />
                <asp:TextBox ID="TxtResetNewPassword" runat="server" TextMode="Password" CssClass="form-control input-lg" Width="300px"></asp:TextBox>
            </p>
            <asp:PasswordStrength ID="PasswordStrength1"
                runat="server" Enabled="true" TargetControlID="TxtResetNewPassword" DisplayPosition="RightSide"
                MinimumNumericCharacters="1"
                MinimumSymbolCharacters="1" HelpStatusLabelID="LblPassword" BarBorderCssClass="border" RequiresUpperAndLowerCaseCharacters="true"
                MinimumLowerCaseCharacters="1"
                MinimumUpperCaseCharacters="1" PreferredPasswordLength="6" CalculationWeightings="25;25;15;35"
                StrengthIndicatorType="BarIndicator" TextStrengthDescriptions="Very Poor; Weak; Average; Strong; Excellent" StrengthStyles="VeryPoor; Weak; Average; Strong; Excellent">
            </asp:PasswordStrength>
            <asp:Label ID="LblPassword" runat="server" CssClass="pwd"></asp:Label>

            <br />
            <p>
                <asp:Label ID="Label2" runat="server" Text="Label" Font-Bold="true">Validate New Password </asp:Label><br />
                <asp:TextBox ID="TxtResetNewPassword2" runat="server" TextMode="Password" CssClass="form-control input-lg" Width="300px"></asp:TextBox>Accepted Special Characters: @#$%^&+=_!*-
                <br />
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="BtnSubmit" runat="server" CssClass="btn" Text="Submit" OnClick="BtnSubmit_Click" />
                <br />
                <br />
                <asp:Label ID="LblUserMessage" runat="server" ForeColor="Red"></asp:Label>
        </div>

        <div style="clear: both;">&nbsp;</div>
        <div id="footer">
            Copyright &copy; <%= Branding.CopyrightYears %> <a href="<%= Branding.CompanyWebSite %>"><%= Branding.CompanyName %></a> - All Rights Reserved   
            <br />
            <div style="clear: both;">&nbsp;</div>
        </div>
    </form>
    <script>
        setTimeout(AutoLogout, <%= Feature.AutoLogoutTime %> );

        function AutoLogout() {
            alert("<%= Branding.AutoLogoutMessage %>");
            window.location = window.location.href;
        }
    </script>
</body>
</html>
