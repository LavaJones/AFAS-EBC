<%@ Page EnableSessionState="ReadOnly" Title="Import Files" Language="C#" 
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="ImportConverter.aspx.cs" 
    Inherits="Afas.AfComply.UI.admin.AdminPortal.ImportConverter" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <br />
    <label class="lbl3">Select Employer</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged"  AutoPostBack="true"></asp:DropDownList>
    <br />
    <asp:Label ID="cofein" runat="server"></asp:Label>

    <h3>Demographic File Import</h3>
    <h4>Upload File</h4>
    <asp:FileUpload ID="DemographicsFile" runat="server" Width="350px" />
    <br />
    <br />
    <span>
        <ul>
            <li>The import must be a "comma delimited" .csv file.</li>
        </ul>
    </span>
    <br />
    <br />

    <h3>Coverage File Import</h3>
    <h4>Upload File</h4>
    <asp:FileUpload ID="CoverageFile" runat="server" Width="350px" />
    <br />
    <br />
    <span>
        <ul>
            <li>The import must be a "comma delimited" .csv file.</li>
        </ul>
    </span>
    <br />
    <br />

    <h3>Payroll File Import</h3>
    <h4>Upload File</h4>
    <asp:FileUpload ID="PayrollFile" runat="server" Width="350px" />
    <br />
    <br />
    <span>
        <ul>
            <li>The import must be a "comma delimited" .csv file.</li>
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
