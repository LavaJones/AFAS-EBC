<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="reg_confirmation.aspx.cs" Inherits="registration_reg_confirmation" %>
<%@ Import Namespace="Afas.AfComply.Domain" %>

<!DOCTYPE html>
<html>
<head>
    <title>Regestration Confirmation</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link rel="stylesheet" type="text/css" href="../default.css" />
    <style type="text/css">
        h3 {
            color: black;
        }
    </style>

    <script type="text/javascript">
        function printWindow() {
            window.print();
        }
    </script>

</head>
<body>
    <form id="formElem" runat="server">
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
                <div style="width: 900px; min-height: 25px;">
                    <h4>Confirmation #:
                        <asp:Literal ID="LitConfirmationNum" runat="server"></asp:Literal></h4>
                    <a href="#" onclick="javaScript:printWindow();">
                        <img src="../images/printer.png" alt="Click to print page" />
                    </a>
                    <p>
                        Thanks for registering to use our software. <%= Branding.CompanyShortName %> will be in touch with you shortly to continue this process.
         
                    </p>
                </div>
                <div id="tbleft">
                    <h3>Employer Name &amp; Address</h3>
                    <label class="lbl3">Employer Name</label>
                    <asp:TextBox ID="TxtDistName" runat="server" CssClass="txtLong" ReadOnly="true"></asp:TextBox>
                    <br />
                    <label class="lbl3">Address</label>
                    <asp:TextBox ID="TxtDistAddress" runat="server" CssClass="txtLong" ReadOnly="true"></asp:TextBox>
                    <br />
                    <label class="lbl3">City</label>
                    <asp:TextBox ID="TxtDistCity" runat="server" CssClass="txtLong" ReadOnly="true"></asp:TextBox>
                    <br />
                    <label class="lbl3">State</label>
                    <asp:TextBox ID="TxtDistState" runat="server" CssClass="txtLong" ReadOnly="true"></asp:TextBox>
                    <br />
                    <label class="lbl3">Zip</label>
                    <asp:TextBox ID="TxtDistZip" runat="server" CssClass="txtLong" ReadOnly="true"></asp:TextBox>
                    <h3>Health Insurance Renewal Date</h3>
                    <label class="lbl3">Renewal Plan Description</label>
                    <asp:TextBox ID="TxtDistRenewalDescription" runat="server" CssClass="txtLong" ReadOnly="true"></asp:TextBox>
                    <br />
                    <label class="lbl3">Renewal Date</label>
                    <asp:TextBox ID="TxtDistRenewalDate" runat="server" CssClass="txtLong" ReadOnly="true"></asp:TextBox>
                    <br />
                    <label class="lbl3">* 2nd Renewal Plan Description</label>
                    <asp:TextBox ID="TxtDistRenewalDescription2" runat="server" CssClass="txtLong" ReadOnly="true"></asp:TextBox>
                    <br />
                    <label class="lbl3">* 2nd Renewal Date</label>
                    <asp:TextBox ID="TxtDistRenewalDate2" runat="server" CssClass="txtLong" ReadOnly="true"></asp:TextBox>
                    <br />
                    * Note: These will show N/A if you only have a single plan year. 
         
                    <h3>Billing Contact</h3>
                    <label class="lbl3">Username</label>
                    <asp:TextBox ID="TxtBillUsername" runat="server" CssClass="txtLong" ReadOnly="true"></asp:TextBox>
                    <label class="lbl3">First Name</label>
                    <asp:TextBox ID="TxtBillFName" runat="server" CssClass="txtLong" ReadOnly="true"></asp:TextBox>
                    <br />
                    <label class="lbl3">Last Name</label>
                    <asp:TextBox ID="TxtBillLName" runat="server" CssClass="txtLong" ReadOnly="true"></asp:TextBox>
                    <br />
                    <label class="lbl3">Address</label>
                    <asp:TextBox ID="TxtBillAddress" runat="server" CssClass="txtLong" ReadOnly="true"></asp:TextBox>
                    <br />
                    <label class="lbl3">City</label>
                    <asp:TextBox ID="TxtBillCity" runat="server" CssClass="txtLong" ReadOnly="true"></asp:TextBox>
                    <br />
                    <label class="lbl3">State</label>
                    <asp:TextBox ID="TxtBillState" runat="server" CssClass="txtLong" ReadOnly="true"></asp:TextBox>
                    <br />
                    <label class="lbl3">Zip</label>
                    <asp:TextBox ID="TxtBillZip" runat="server" CssClass="txtLong" ReadOnly="true"></asp:TextBox>
                    <br />
                    <label class="lbl3">Email</label>
                    <asp:TextBox ID="TxtBillEmail" runat="server" CssClass="txtLong" ReadOnly="true"></asp:TextBox>
                    <br />
                    <label class="lbl3">Phone</label>
                    <asp:TextBox ID="TxtBillPhone" runat="server" CssClass="txtLong" ReadOnly="true"></asp:TextBox>

                    <h3>Primary User</h3>
                    <label class="lbl3">First Name</label>
                    <asp:TextBox ID="TxtUserFname" runat="server" CssClass="txtLong" ReadOnly="true"></asp:TextBox>
                    <br />
                    <label class="lbl3">Last Name</label>
                    <asp:TextBox ID="TxtUserLname" runat="server" CssClass="txtLong" ReadOnly="true"></asp:TextBox>
                    <br />
                    <label class="lbl3">Email</label>
                    <asp:TextBox ID="TxtUserEmail" runat="server" CssClass="txtLong" ReadOnly="true"></asp:TextBox>
                    <br />
                    <label class="lbl3">Phone</label>
                    <asp:TextBox ID="TxtUserPhone" runat="server" CssClass="txtLong" ReadOnly="true"></asp:TextBox>
                    <br />
                    <label class="lbl3">Username</label>
                    <asp:TextBox ID="TxtUserName" runat="server" CssClass="txtLong" ReadOnly="true"></asp:TextBox>
                </div>
                <div id="tbright">
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
