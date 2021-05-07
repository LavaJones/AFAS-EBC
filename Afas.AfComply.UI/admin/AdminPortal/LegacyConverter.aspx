<%@ Page EnableSessionState="ReadOnly" Title="Convert Legacy Files" Language="C#" MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="LegacyConverter.aspx.cs" Inherits="Afas.AfComply.UI.admin.AdminPortal.LegacyConverter" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <br />
    <label class="lbl3">Select Employer</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged"  AutoPostBack="true"></asp:DropDownList>
    <br />
    <asp:Label ID="cofein" runat="server"></asp:Label>

    <h3>Extended Offer File To Convert</h3>
     <asp:ListBox runat="server" ID="lblPlanYearsOffer" SelectionMode="Single" Visible="false">
    </asp:ListBox>
    <h4>Upload File</h4>
    <asp:FileUpload ID="OfferFile" runat="server" Width="350px" />
    <br />
    <br />
    <span>
        <ul>
            <li>The file to convert must be a "comma delimited" .csv file.</li>
        </ul>
    </span>
    <br />
    <br />

    <h3>Demographic File To Convert</h3>
    <h4>Upload File</h4>
    <asp:FileUpload ID="DemographicsFile" runat="server" Width="350px" />
    <br />
    <br />
    <span>
        <ul>
            <li>The file to convert must be a "comma delimited" .csv file.</li>
        </ul>
    </span>
    <br />
    <br />

    <h3>Coverage File  To Convert</h3>
     <asp:ListBox runat="server" ID="lblPlanYearsCoverage" SelectionMode="Single" Visible="false">
    </asp:ListBox>
    <h4>Upload File</h4>
    <asp:FileUpload ID="CoverageFile" runat="server" Width="350px" />
    <br />
    <br />
    <span>
        <ul>
            <li>The file to convert must be a "comma delimited" .csv file.</li>
        </ul>
    </span>
    <br />
    <br />

    <h3>Payroll File To Convert</h3>
    <h4>Upload File</h4>
    <asp:FileUpload ID="PayrollFile" runat="server" Width="350px" />
    <br />
    <br />
    <br />
    Number of days in the payroll period:&nbsp;
    <asp:DropDownList ID="PayrollFileDays" runat="server">
     <asp:ListItem Selected="True" Text="Select" Value="" />
     <asp:ListItem Text="7 Days" Value="6" />
     <asp:ListItem Text="14 Days" Value="13" />
     <asp:ListItem Text="30 Days" Value="29" />
    </asp:DropDownList>
    <br />
    <span>
        <ul>
            <li>The file to convert must be a "comma delimited" .csv file.</li>
        </ul>
    </span>
    <br />

    <h3>Ohio Afford Payroll File To Convert</h3>
    <h4>Upload File</h4>
    <asp:FileUpload ID="OhioAffordPayrollFile" runat="server" Width="350px" />
    <br />
    <br />
    <br />
    <span>
        <ul>
            <li>The file to convert must be a "comma delimited" .csv file.</li>
        </ul>
    </span>
    <br />

    <h3>Alternate Ohio Afford Payroll File To Convert</h3>
    <h4>Upload File</h4>
    <asp:FileUpload ID="OhioAffordAlternatePayrollFile" runat="server" Width="350px" />
    <br />
    <br />
    <br />
    Number of days in the payroll period:&nbsp;
    <asp:DropDownList ID="OhioAffordAlternatePayrollFileDays" runat="server">
     <asp:ListItem Selected="True" Text="Select" Value="" />
     <asp:ListItem Text="7 Days" Value="6" />
     <asp:ListItem Text="14 Days" Value="13" />
    </asp:DropDownList>
    <br />
    <span>
        <ul>
            <li>The file to convert must be a "comma delimited" .csv file.</li>
        </ul>
    </span>
    <br />

    <h2>Submit Files</h2>
    <br />
    <asp:Button ID="BtnUploadFile" runat="server" CssClass="btn" Text="Submit" OnClick="BtnUploadFile_Click" />
    <br />
    <asp:Label ID="LblFileUploadMessage" runat="server"></asp:Label>
    <asp:Label ID="lblMsg" runat="server"></asp:Label>
</asp:Content>
