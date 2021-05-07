<%@ Page EnableSessionState="ReadOnly" Title="Transmission Report" Language="C#" 
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="TransmissionReport.aspx.cs" 
    Inherits="Afas.AfComply.UI.admin.AdminPortal.TransmissionReport" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    
    
    <h2>Transmission Report .csv export</h2>
    <br />
   <asp:ImageButton ID="ImgBtnExportCSVEnableTransmission" runat="server" align="left" Height="30px" ImageUrl="~/images/csv.png" OnClick="ImgBtnExportCSVEnableTransmission_Click" ToolTip="Export Enabled Transmission to .CSV file" />
    <br />
    <br />
    <br />
    <asp:ImageButton ID="ImgBtnExportHoldTransmission" runat="server" align="left" Height="30px" ImageUrl="~/images/csv.png" OnClick="ImgBtnExportHoldTransmission_Click" ToolTip="Export Hold Transmission to .CSV file" />
    <br />
    
</asp:Content>






