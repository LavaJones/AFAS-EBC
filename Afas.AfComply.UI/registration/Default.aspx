<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="registration_Default" %>
<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>
<html>
<head>
    <title><%= Branding.ProductName %> - <%= Branding.CompanyName %></title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link rel="stylesheet" type="text/css" href="../_js/slidingForm/style_slider.css?v=<%= globalData.getVersion() %>" />
    <link rel="stylesheet" type="text/css" href="../default.css?v=<%= globalData.getVersion() %>" />
    <link rel="stylesheet" type="text/css" href="../_js/jquery-ui-themes-1.10.2/themes/ui-lightness/jquery-ui.css?v=<%= globalData.getVersion() %>" />
    <link rel="stylesheet" type="text/css" href="../design/acs_working.css?v=<%= globalData.getVersion() %>" />
    <link rel="stylesheet" type="text/css" href="../password.css?v=<%= globalData.getVersion() %>" />
    <style type="text/css">
        h3 {
            color: black;
        }
    </style>


    <script src="../_js/jquery-ui-1.10.2/jquery-1.9.1.js?v=<%= globalData.getVersion() %>" type="text/javascript"></script>
    <script src="../_js/jquery-ui-1.10.2/ui/jquery-ui.js?v=<%= globalData.getVersion() %>" type="text/javascript"></script>
    <script src="../_js/slidingForm/sliding.form.js?v=<%= globalData.getVersion() %>" type="text/javascript"></script>
    <script src="../_js/validation/validation.js?v=<%= globalData.getVersion() %>" type="text/javascript"></script>

    <style type="text/css">
        .txtOV {
            background-color: transparent;
            font-size: 12px;
            width: 25px;
        }

        .radioLabel {
            text-align: right;
            width: 100px;
            float: left;
        }

        .radioCheck {
            float: left;
            width: 25px;
            margin-left: -100px;
            text-align: left;
        }

        .pwd {
            font-size: 10px;
            padding-left: 15px;
        }
    </style>
    <script>
        function show2ndDate() {
            document.getElementById("renewal").style.display = 'block';
        }
        function hide2ndDate() {
            document.getElementById("renewal").style.display = 'none';
        }
        function showBillingUser() {
            document.getElementById("billing").style.display = 'block';
        }
        function hideBillingUser() {
            document.getElementById("billing").style.display = 'none';
        }
    </script>
</head>
<body>
    <form id="formElem" runat="server" defaultbutton="BtnDefault">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
        <div id="container">
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
                    <li><a href="../default.aspx">Home</a></li>
                    <li><a href="#">Registration</a></li>
                </ul>
            </div>
            <div id="topbox">
                <h4>Sign-up Process</h4>
                <div id="slidercontent">
                    <div id="wrapper">
                        <asp:UpdatePanel ID="UpRegistration" runat="server">
                            <ContentTemplate>
                                <div id="navigation" style="display: none;">
                                    <ul>
                                        <li class="selected">
                                            <a href="#">Step 1: Employer Profile</a><span id="sp_step1"></span>
                                        </li>
                                        <li>
                                            <a href="#">Step 2: Primary Software User </a><span id="sp_step2"></span>
                                        </li>
                                        <li>
                                            <a href="#">Step 3: Billing Contact </a><span id="sp_step3"></span>
                                        </li>
                                        <li>
                                            <a href="#">Step 4: Review &amp; Submit </a><span id="sp_step4"></span>
                                        </li>
                                    </ul>

                                </div>
                                <div id="sliderHelp">
                                    <img src="../images/info.jpg" alt="Help Icon" style="height: 1px; padding-top: 3px; height: 15px; padding-left: 5px;" />
                                    <span id="ProfileMessage"></span>
                                </div>
                                <div id="sliderNext">
                                    <span>NEXT</span>
                                </div>
                                <div id="sliderPrev">
                                    <span>PREV</span>
                                </div>
                                <div id="steps">
                                    <fieldset class="step">
                                        <legend>Employer Profile</legend>
                                        <div class="leftSliderColumn">
                                            <h3>Employer Type</h3>
                                            <label class="lbl3">Employer Type</label>
                                            <asp:DropDownList ID="DdlEmployerType" runat="server" CssClass="ddl2"></asp:DropDownList>
                                            <br />
                                            <label class="lbl3">Payroll Software</label>
                                            <asp:TextBox ID="TxtEmployerPayrollSoftware" runat="server" CssClass="txtLong"></asp:TextBox>
                                            <br />
                                            <h3>Employer Name &amp; Address</h3>
                                            <label class="lbl3">Employer Name</label>
                                            <asp:TextBox ID="TxtDistName" runat="server" CssClass="txtLong"></asp:TextBox>
                                            <br />
                                            <label class="lbl3">EIN #</label>
                                            <asp:TextBox ID="TxtEmployerEIN" runat="server" CssClass="txtLong"></asp:TextBox>
                                            xx-xxxxxxx
                               
                                            <br />
                                            <label class="lbl3">Address</label>
                                            <asp:TextBox ID="TxtDistAddress" runat="server" CssClass="txtLong"></asp:TextBox>
                                            <br />
                                            <label class="lbl3">City</label>
                                            <asp:TextBox ID="TxtDistCity" runat="server" CssClass="txtLong"></asp:TextBox>
                                            <br />
                                            <label class="lbl3">State</label>
                                            <asp:DropDownList ID="DdlDistState" runat="server" CssClass="ddl2"></asp:DropDownList>
                                            <br />
                                            <label class="lbl3">Zip</label>
                                            <asp:TextBox ID="TxtDistZip" runat="server" CssClass="txtLong"></asp:TextBox>

                                        </div>
                                        <div class="rightSliderColumn">
                                            <h3>Health Insurance Renewal Date</h3>
                                            <label class="lbl3">Plan Description</label>
                                            <asp:TextBox ID="TxtDistRenewalDescription" runat="server" CssClass="txtLong"></asp:TextBox>
                                            <br />
                                            <label class="lbl3">2015 Renewal Month</label>
                                            <asp:DropDownList ID="DdlRenewalDate1" runat="server" CssClass="ddl2">
                                            </asp:DropDownList>
                                            <p>
                                                Do you offer another medical plan with a different plan renewal date?
                               
                                            </p>

                                            <asp:Label runat="server" ID="lbl" Text="Yes" CssClass="radioLabel" AssociatedControlID="RbtnYes" />
                                            <asp:RadioButton runat="server" CssClass="radioCheck" ID="RbtnYes" GroupName="hi" />
                                            <br />
                                            <br />
                                            <asp:Label runat="server" ID="Label1" Text="No" CssClass="radioLabel" AssociatedControlID="RbtnNo" />
                                            <asp:RadioButton runat="server" CssClass="radioCheck" ID="RbtnNo" Checked="true" GroupName="hi" />
                                            <br />
                                            <br />
                                            <span id="renewal" style="display: none;">
                                                <label class="lbl3">2nd Plan Description</label>
                                                <asp:TextBox ID="TxtDistRenewalDescription2" runat="server" CssClass="txtLong"></asp:TextBox>
                                                <br />
                                                <label class="lbl3">2nd 2015 Renewal Month</label>
                                                <asp:DropDownList ID="DdlRenewalDate2" runat="server" CssClass="ddl2">
                                                </asp:DropDownList>
                                                <br />
                                                <p>
                                                    Note: If you have more than two plan year renewal dates, they will need to be added from the profile in the set-up pages, once you login.
                                         Feel free to call or email us if you have any questions.
                                   
                                                </p>
                                            </span>
                                            <br />

                                            <asp:TextBox ID="TxtTab1" runat="server" BackColor="Transparent" BorderStyle="None" Height="1px" Width="1px"></asp:TextBox>
                                        </div>
                                    </fieldset>

                                    <fieldset class="step">
                                        <legend>Primary Software User</legend>
                                        <div class="sliderSingleColumn">
                                            <h3>Primary User</h3>
                                            <label class="lbl3">First Name</label>
                                            <asp:TextBox ID="TxtUserFname" runat="server" CssClass="txtLong"></asp:TextBox>
                                            <br />
                                            <label class="lbl3">Last Name</label>
                                            <asp:TextBox ID="TxtUserLname" runat="server" CssClass="txtLong"></asp:TextBox>
                                            <br />
                                            <label class="lbl3">Email</label>
                                            <asp:TextBox ID="TxtUserEmail" runat="server" CssClass="txtLong"></asp:TextBox>
                                            <br />
                                            <label class="lbl3">Phone</label>
                                            <asp:TextBox ID="TxtUserPhone" runat="server" CssClass="txtLong"></asp:TextBox>Ex: 555-555-5555
                               
                                            <h3>Username/Password</h3>
                                            <label class="lbl3">Username</label>
                                            <asp:TextBox ID="TxtUserName" runat="server" CssClass="txtLong"></asp:TextBox>6 character min
                               
                                            <br />
                                            <label class="lbl3">Password</label>
                                            <asp:TextBox ID="TxtUserPass" runat="server" CssClass="txtLong" TextMode="Password"></asp:TextBox>
                                            <asp:PasswordStrength ID="PasswordStrength1" runat="server" Enabled="true" TargetControlID="TxtUserPass" DisplayPosition="RightSide"
                                                MinimumNumericCharacters="1"
                                                MinimumSymbolCharacters="1" HelpStatusLabelID="LblPassword" BarBorderCssClass="border" RequiresUpperAndLowerCaseCharacters="true"
                                                MinimumLowerCaseCharacters="1"
                                                MinimumUpperCaseCharacters="1" PreferredPasswordLength="6" CalculationWeightings="25;25;15;35"
                                                StrengthIndicatorType="BarIndicator" TextStrengthDescriptions="Very Poor; Weak; Average; Strong; Excellent" StrengthStyles="VeryPoor; Weak; Average; Strong; Excellent">
                                            </asp:PasswordStrength>
                                            <asp:Label ID="LblPassword" runat="server" CssClass="pwd"></asp:Label>
                                            <br />
                                            <label class="lbl3">Re-enter Password</label>
                                            <asp:TextBox ID="TxtUserPass2" runat="server" CssClass="txtLong" TextMode="Password"></asp:TextBox>
                                            Accepted Special Characters: @#$%^&+=_!*-
                               
                                            <br />
                                            <asp:TextBox ID="TxtTab2" runat="server" BackColor="Transparent" BorderStyle="None" Height="1px" Width="1px"></asp:TextBox>
                                        </div>
                                    </fieldset>

                                    <fieldset class="step">
                                        <legend>Billing Contact</legend>
                                        <div class="rightSliderColumn">
                                            <h3>Billing Location</h3>
                                            <label class="lbl3">Address</label>
                                            <asp:TextBox ID="TxtBillAddress" runat="server" CssClass="txtLong"></asp:TextBox>
                                            <br />
                                            <label class="lbl3">City</label>
                                            <asp:TextBox ID="TxtBillCity" runat="server" CssClass="txtLong"></asp:TextBox>
                                            <br />
                                            <label class="lbl3">State</label>
                                            <asp:DropDownList ID="DdlBillState" runat="server" CssClass="ddl2"></asp:DropDownList>
                                            <br />
                                            <label class="lbl3">Zip</label>
                                            <asp:TextBox ID="TxtBillZip" runat="server" CssClass="txtLong"></asp:TextBox>Ex: 55555
                               
                                            <br />
                                            <asp:TextBox ID="TxtTab4" runat="server" BackColor="Transparent" BorderStyle="None" Height="1px" Width="1px"></asp:TextBox>
                                        </div>
                                        <div class="leftSliderColumn">
                                            <h3>Billing User</h3>
                                            Is the Billing User the same as Primary Software User?
                               
                                            <br />
                                            <asp:Label runat="server" ID="Label3" Text="Yes" CssClass="radioLabel" AssociatedControlID="RbtnBillYes" />
                                            <asp:RadioButton runat="server" CssClass="radioCheck" ID="RbtnBillYes" Checked="true" GroupName="bi" />
                                            <br />
                                            <br />
                                            <asp:Label runat="server" ID="Label4" Text="No" CssClass="radioLabel" AssociatedControlID="RbtnBillNo" />
                                            <asp:RadioButton runat="server" CssClass="radioCheck" ID="RbtnBillNo" Checked="false" GroupName="bi" />
                                            <br />
                                            <br />
                                            <span id="billing" style="display: none;" />
                                            <label class="lbl3">First Name</label>
                                            <asp:TextBox ID="TxtBillFName" runat="server" CssClass="txtLong"></asp:TextBox>
                                            <br />
                                            <label class="lbl3">Last Name</label>
                                            <asp:TextBox ID="TxtBillLName" runat="server" CssClass="txtLong"></asp:TextBox>
                                            <br />
                                            <label class="lbl3">Email</label>
                                            <asp:TextBox ID="TxtBillEmail" runat="server" CssClass="txtLong"></asp:TextBox>
                                            <br />
                                            <label class="lbl3">Phone</label>
                                            <asp:TextBox ID="TxtBillPhone" runat="server" CssClass="txtLong"></asp:TextBox><br />
                                            Ex: 555-555-5555
                                   
                                            <h3>Username/Password</h3>
                                            <label class="lbl3">Username</label>
                                            <asp:TextBox ID="TxtBillUsername" runat="server" CssClass="txtLong"></asp:TextBox><br />
                                            6 character min
                                   
                                            <br />
                                            <label class="lbl3">Password</label>
                                            <asp:TextBox ID="TxtBillPassword" runat="server" CssClass="txtLong" TextMode="Password"></asp:TextBox>
                                            <asp:PasswordStrength ID="PasswordStrength2" runat="server" Enabled="true" TargetControlID="TxtBillPassword" DisplayPosition="RightSide"
                                                MinimumNumericCharacters="1"
                                                MinimumSymbolCharacters="1" HelpStatusLabelID="LblPassword" BarBorderCssClass="border" RequiresUpperAndLowerCaseCharacters="true"
                                                MinimumLowerCaseCharacters="1"
                                                MinimumUpperCaseCharacters="1" PreferredPasswordLength="6" CalculationWeightings="25;25;15;35"
                                                StrengthIndicatorType="BarIndicator" TextStrengthDescriptions="Very Poor; Weak; Average; Strong; Excellent" StrengthStyles="VeryPoor; Weak; Average; Strong; Excellent">
                                            </asp:PasswordStrength>
                                            <asp:Label ID="Label2" runat="server" CssClass="pwd"></asp:Label>
                                            <br />
                                            <label class="lbl3">Re-enter Password</label>
                                            <asp:TextBox ID="TxtBillPassword2" runat="server" CssClass="txtLong" TextMode="Password"></asp:TextBox>
                                            <br />
                                            Accepted Special Characters: @#$%^&+=_!*-
                                   
                                            <br />
                                            </span>
                           
                                        </div>
                                        <asp:TextBox ID="TxtTab3" runat="server" BackColor="Transparent" BorderStyle="None" Height="1px" Width="1px"></asp:TextBox>
                                    </fieldset>

                                    <fieldset class="step">
                                        <legend>Review &amp; Submit Application</legend>
                                        <div class="slider1stColumn">
                                            <h3>Employer Profile</h3>
                                            <span id="s_district" style="font-weight: bold"></span>
                                            <br />
                                            <span id="s_address"></span>
                                            <br />
                                            <span id="s_city"></span>, 
                                   
                                            <span id="s_state"></span>
                                            <span id="s_zip"></span>
                                            <br />
                                            <br />
                                            EIN:
                                   
                                            <span id="s_employer_ein"></span>
                                            <br />
                                            <h3>Software Admin Credentials</h3>
                                            <p>
                                                *Note: Additional USERS can be added when your logged into the system.
                                   
                                            </p>
                                            <span id="s_fname"></span>
                                            <span id="s_lname"></span>
                                            <br />
                                            <span id="s_email"></span>
                                            <br />
                                            <span id="s_phone"></span>
                                        </div>
                                        <div class="slider2ndColumn">
                                            <h3>Billing Admin/Location</h3>
                                            <p>
                                                *Note: All invoices are sent electronically through email. 
                                   
                                            </p>
                                            <span id="b_fname"></span>
                                            <span id="b_lname"></span>
                                            <br />
                                            <span id="b_email"></span>
                                            <br />
                                            <span id="b_phone"></span>
                                            <br />
                                            <span id="b_address"></span>
                                            <br />
                                            <span id="b_city"></span>,
                                   
                                            <span id="b_state"></span>
                                            <span id="b_zip"></span>
                                            <br />
                                            <h3>Health Insurance Renewal Date</h3>
                                            <span id="s_insurance_name"></span>
                                            <br />
                                            <span id="s_insurance"></span>
                                            <br />
                                            <br />
                                            <span id="s_insurance_name2"></span>
                                            <br />
                                            <span id="s_insurance2"></span>
                                        </div>
                                        <div class="slider3rdColumn">
                                            <asp:ImageButton ID="ImgBtnSubmit" BorderStyle="None" runat="server" ImageUrl="~/images/submit_false.png" OnClick="ImgBtnSubmit_Click" />
                                            <br />
                                            <span id="message"></span>
                                            <asp:Button ID="BtnDefault" runat="server" Enabled="false" Height="5px" Width="5px" BorderStyle="None" />
                                        </div>
                                    </fieldset>
                                </div>

                                <asp:HiddenField ID="HfDummyTrigger2" runat="server" />
                                <asp:ModalPopupExtender ID="MpeError2" runat="server" PopupControlID="PnlError2" TargetControlID="HfDummyTrigger2" CancelControlID="ImgBtnError2Close"></asp:ModalPopupExtender>
                                <asp:Panel ID="PnlError2" runat="server">
                                    <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                                    </div>
                                    <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white; text-align: left;">
                                        <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                            <asp:ImageButton ID="ImgBtnError2Close" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" />
                                        </div>
                                        <h3>Registration Error #2</h3>
                                        <p style="color: black;">
                                            An error has occurred during the registration process. 
                           
                                        </p>
                                        <p style="color: black;">
                                            The cause of this error is generally one of the following:
                           
                                        </p>
                                        <ul style="color: black;">
                                            <li>(Step 2) The Primary Software or Billing User's USERNAME or PASSWORD has already been used.</li>
                                            <li>(Step 2) The Primary Software or Billing User's EMAIL ADDRESS has already been registered with the system.</li>
                                        </ul>
                                        <p>
                                            If you have any questions feel free to call or email us.
                           
                                        </p>
                                    </div>
                                </asp:Panel>

                                <asp:HiddenField ID="HfDummyTrigger1" runat="server" />
                                <asp:ModalPopupExtender ID="MpeError1" runat="server" PopupControlID="PnlError1" TargetControlID="HfDummyTrigger1" CancelControlID="ImgBtnError1Close"></asp:ModalPopupExtender>
                                <asp:Panel ID="PnlError1" runat="server">
                                    <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                                    </div>
                                    <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white; text-align: left;">
                                        <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                            <asp:ImageButton ID="ImgBtnError1Close" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" />
                                        </div>
                                        <h3>Registration Error #1</h3>
                                        <p style="color: black;">
                                            An error has occurred during the registration process. 
                           
                                        </p>
                                        <p style="color: black;">
                                            The cause of this error is generally caused by bad data. Please review all data in the sliding form to see if any are highlighed in red.
                           
                                        </p>
                                        <p>
                                            If you have any questions feel free to call or email us.
                           
                                        </p>
                                    </div>
                                </asp:Panel>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdateProgress ID="UpdateProgress1" runat="server" DynamicLayout="true" DisplayAfter="500">
                            <ProgressTemplate>
                                <div id="OuterTableCellOverlay">
                                    <div id="InnerTableCellOverlay">
                                        <br />
                                        <br />
                                        <br />
                                        <b>... validating records ...</b>
                                        <br />
                                        <br />
                                        <br />
                                        <asp:Image ID="ImgSearching" runat="server" ImageUrl="~/design/icon-loading-animated.gif" />
                                    </div>
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </div>
                </div>
            </div>
        </div>
        <div style="clear: both;">&nbsp;</div>
        <div id="footer">
            Copyright &copy; <%= Branding.CopyrightYears %> <a href="<%= Branding.CompanyWebSite %>"><%= Branding.CompanyName %></a> - All Rights Reserved   
            <br />
            <div style="clear: both;">&nbsp;</div>
        </div>
    </form>
</body>
</html>

