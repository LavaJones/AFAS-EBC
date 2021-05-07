<%@ Page EnableSessionState="ReadOnly" Title="Clear Offer Alerts" Language="C#" 
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="ClearOfferAlerts.aspx.cs" 
    Inherits="Afas.AfComply.UI.admin.AdminPortal.ClearOfferAlerts" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    
    
    <h2>Clear Offer Alerts</h2>
    <br />
    <label class="lbl3">Select Employer</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged"  AutoPostBack="true"></asp:DropDownList>
    <br />
    <asp:Label ID="cofein" runat="server"></asp:Label>

    <br />
    <label class="lbl3">Plan Year (Stability Period) To Clear:</label>
    <asp:DropDownList ID="DdlPlanYearNew" runat="server" CssClass="ddl2"></asp:DropDownList>
    <br />
    
    <br />
    <asp:Button ID="BtnRun" runat="server" CssClass="btn" Text="Clear Offers" OnClick="BtnRun_Click" />
    <br />
    <asp:Label ID="lblMsg" runat="server"></asp:Label>    

</asp:Content>