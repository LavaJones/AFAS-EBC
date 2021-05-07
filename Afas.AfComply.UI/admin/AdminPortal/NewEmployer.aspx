<%@ Page EnableSessionState="ReadOnly" Title="Create New Employer" Language="C#" 
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="NewEmployer.aspx.cs" 
    Inherits="Afas.AfComply.UI.admin.AdminPortal.NewEmployer" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>

    <div>

        <h2>Create New Employer</h2>
        <br />
        <br />
        <label class="lbl">Federal Employer Identification Number: </label>
        <asp:TextBox ID="TxtEmployerEIN" runat="server" CssClass="txt3" ></asp:TextBox>
        xx-xxxxxxx
        <br />
                
        <label class="lbl">Legal Name: </label>
        <asp:TextBox ID="TxtEmployerIrsName" runat="server" CssClass="txt3"></asp:TextBox>
        <br />

        <label class="lbl">DBA Name: </label>
        <asp:TextBox ID="TxtEmployerDbaName" runat="server" CssClass="txt3"></asp:TextBox>
        <br />

        <label class="lbl">Address: </label>
        <asp:TextBox ID="TxtAddress" runat="server" CssClass="txt3"></asp:TextBox>
        <br />

        <label class="lbl">City: </label>
        <asp:TextBox ID="TxtCity" runat="server" CssClass="txt3"></asp:TextBox>
        <br />

        <label class="lbl">State</label>
        <asp:DropDownList ID="DdlState" runat="server" CssClass="ddl2"></asp:DropDownList>
        <br />

        <label class="lbl">Zip</label>
        <asp:TextBox ID="TxtZip" runat="server" CssClass="txt3"></asp:TextBox>
        <br />
        <br />

        <label class="lbl">Employer Type</label>
        <asp:DropDownList ID="DdlEmployerType" runat="server" CssClass="ddl2"></asp:DropDownList>
        <br />

        <h2>Default User: </h2>
        <br />
        <label class="lbl">First Name</label>
        <asp:TextBox ID="TxtUserFname" runat="server" CssClass="txt3"></asp:TextBox>
        <br />
        <label class="lbl">Last Name</label>
        <asp:TextBox ID="TxtUserLname" runat="server" CssClass="txt3"></asp:TextBox>
        <br />
        <label class="lbl">Email</label>
        <asp:TextBox ID="TxtUserEmail" runat="server" CssClass="txt3"></asp:TextBox>
        <br />
        <label class="lbl">Phone</label>
        <asp:TextBox ID="TxtUserPhone" runat="server" CssClass="txt3"></asp:TextBox>
        Ex: 555-555-5555

        <h3>Username/Password</h3>
        <label class="lbl">Username</label>
        <asp:TextBox ID="TxtUserName" runat="server" CssClass="txt3"></asp:TextBox>
        6 character min
                               
        <br />
        <label class="lbl">Password</label>
        <asp:TextBox ID="TxtUserPass" runat="server" TextMode="Password" CssClass="txt3"></asp:TextBox>
        <asp:PasswordStrength ID="PasswordStrength1" runat="server" Enabled="true" TargetControlID="TxtUserPass" DisplayPosition="RightSide"
            MinimumNumericCharacters="1"
            MinimumSymbolCharacters="1" HelpStatusLabelID="LblPassword" BarBorderCssClass="border" RequiresUpperAndLowerCaseCharacters="true"
            MinimumLowerCaseCharacters="1"
            MinimumUpperCaseCharacters="1" PreferredPasswordLength="6" CalculationWeightings="25;25;15;35"
            StrengthIndicatorType="BarIndicator" TextStrengthDescriptions="Very Poor; Weak; Average; Strong; Excellent" StrengthStyles="VeryPoor; Weak; Average; Strong; Excellent">
        </asp:PasswordStrength>
        <asp:Label ID="LblPassword" runat="server" CssClass="pwd"></asp:Label>
        <br />
        <label class="lbl">Re-enter Password</label>
        <asp:TextBox ID="TxtUserPass2" runat="server" TextMode="Password" CssClass="txt3"></asp:TextBox>
        Accepted Special Characters: @#$%^&+=_!*-
                               
        <br />
        <br />
        <asp:Button ID="BtnUploadFile" runat="server" CssClass="btn" Text="Submit" OnClick="BtnUploadFile_Click" />
        <br />
        <asp:Label ID="LblFileUploadMessage" runat="server"></asp:Label>


    </div>
</asp:Content>
