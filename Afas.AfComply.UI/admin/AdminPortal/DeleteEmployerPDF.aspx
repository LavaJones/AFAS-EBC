<%@ Page EnableSessionState="ReadOnly" Title="Clear Employer PDF Files" Language="C#" 
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="DeleteEmployerPDF.aspx.cs" 
    Inherits="Afas.AfComply.UI.admin.AdminPortal.DeleteEmployerPDF" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" AsyncPostBackTimeout="3600"></asp:ToolkitScriptManager>
    
    <h2>CLear All PDFs for Employer + TaxYear</h2>
    <br />
    <label class="lbl3">Select Employer</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2"></asp:DropDownList>
    <br />
    <label class="lbl3">Select Tax Year</label>
    <asp:DropDownList ID="DdlCalendarYear" runat="server" CssClass="ddl2">
        <asp:ListItem Text="Select" Value="" Selected="True"></asp:ListItem>
        <asp:ListItem Text="2015" Value="2015" ></asp:ListItem>
        <asp:ListItem Text="2016" Value="2016" ></asp:ListItem>
        <asp:ListItem Text="2017" Value="2017" ></asp:ListItem>
        <asp:ListItem Text="2018" Value="2018" ></asp:ListItem>
        <asp:ListItem Text="2019" Value="2019" ></asp:ListItem>
        <asp:ListItem Text="2020" Value="2020" ></asp:ListItem>
    </asp:DropDownList>
    <br />
    <br />

    <asp:Button ID="BtnDeletePDF" runat="server" Text="Clear PDFs" OnClick="BtnDeletePDF_Click" />
    <br />

    <asp:Label ID="lblMsg" runat="server"></asp:Label> 

</asp:Content>
