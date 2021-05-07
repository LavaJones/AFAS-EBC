<%@ Page EnableSessionState="ReadOnly" Title="Locate Missing Employee Demographics" Language="C#" MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="OfferMissingEmployees.aspx.cs" Inherits="Afas.AfComply.UI.admin.AdminPortal.OfferMissingEmployees" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <br />
    <label class="lbl3">Select Employer</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged"  AutoPostBack="true"></asp:DropDownList>
    <br />
    <asp:Label ID="cofein" runat="server"></asp:Label>

    <h3>Extended Offer File To Look for Missing Employees</h3>
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

    <h2>Submit Files</h2>
    <br />
    <asp:Button ID="BtnUploadFile" runat="server" CssClass="btn" Text="Submit" OnClick="BtnUploadFile_Click" />
    <br />
    <asp:Label ID="LblFileUploadMessage" runat="server"></asp:Label>
    <asp:Label ID="lblMsg" runat="server"></asp:Label>
</asp:Content>
