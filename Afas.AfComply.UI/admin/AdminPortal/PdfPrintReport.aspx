<%@ Page EnableSessionState="ReadOnly" Title="Transmission Report" Language="C#" 
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="PdfPrintReport.aspx.cs" 
    Inherits="Afas.AfComply.UI.admin.AdminPortal.PdfPrintReport" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    
    
    <h2>PDF Print Report .csv export</h2>
    <br />
   <asp:ImageButton ID="ImgBtnExportCSV" runat="server" align="left" Height="30px" ImageUrl="~/images/csv.png" OnClick="ImgBtnExportCSV_Click" ToolTip="Export Enabled PdfPrintReport to .CSV file" />
    <br />
    <br />
    
</asp:Content>






