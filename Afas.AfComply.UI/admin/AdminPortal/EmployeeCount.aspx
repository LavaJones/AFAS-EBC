<%@ Page EnableSessionState="ReadOnly" Title="Employee Count" Language="C#" 
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="EmployeeCount.aspx.cs" 
    Inherits="Afas.AfComply.UI.admin.AdminPortal.EmployeeCount" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    
    
    <h2>Employee count .csv export</h2>
    <br />
   <asp:ImageButton ID="ImgBtnExportCSV" runat="server" align="left" Height="30px" ImageUrl="~/images/csv.png" OnClick="ImgBtnExportCSV_Click" ToolTip="Export employee count to .CSV file" />
    <br />
    <br />
    
</asp:Content>