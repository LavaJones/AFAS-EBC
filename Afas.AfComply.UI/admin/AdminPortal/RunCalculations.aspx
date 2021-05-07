<%@ Page EnableSessionState="ReadOnly" Title="Run Calculations" Language="C#" 
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="RunCalculations.aspx.cs" 
    Inherits="Afas.AfComply.UI.admin.AdminPortal.RunCalculations" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    
    
    <h2>Fix the data</h2>
    <br />
    <label class="lbl3">Select Employer</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged"  AutoPostBack="true"></asp:DropDownList>
    <br />
    <asp:Label ID="cofein" runat="server"></asp:Label>
    <br />
    <asp:Button ID="BtnRun" runat="server" CssClass="btn" Text="Run Calculations" OnClick="BtnRun_Click" />
    <br />
    <asp:Label ID="lblMsg" runat="server"></asp:Label>    

</asp:Content>