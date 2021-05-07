<%@ Page EnableSessionState="ReadOnly" Title="Setup Employer" Language="C#" 
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="BulkEmployerMeasurementPeriod.aspx.cs" 
    Inherits="Afas.AfComply.UI.admin.AdminPortal.BulkEmployerMeasurementPeriod" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <h3>All Employer Measurement Period File Import</h3>
    <h4>Upload File</h4>
    <asp:FileUpload ID="MeasurementFile" runat="server" Width="350px" />
    <br />
    <br />
    <span>
        <ul>
            <li>The import must be a "Tab delimited" .tsv file.</li>
        </ul>
    </span>
    <br />
    <br />


    <h2>Submit the file</h2>
    <br />
    <asp:Button ID="BtnUploadFile" runat="server" CssClass="btn" Text="Submit" OnClick="BtnUploadFile_Click" />
    <br />
    <asp:Label ID="LblFileUploadMessage" runat="server"></asp:Label>



</asp:Content>
