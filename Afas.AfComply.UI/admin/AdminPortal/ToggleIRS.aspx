<%@ Page EnableSessionState="ReadOnly" Title="Enable IRS Menu" Language="C#" 
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="ToggleIRS.aspx.cs" 
    Inherits="Afas.AfComply.UI.admin.AdminPortal.ToggleIRS" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    
    
    <h2>Enable the IRS Reporting Menu</h2>
    <br />
    <label class="lbl3">Select Employer</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged"  AutoPostBack="true"></asp:DropDownList>
    <br />
    <asp:Label ID="cofein" runat="server"></asp:Label>
    <br />
    <asp:CheckBox ID="IrsEnabled" runat="server" Text="IRS Options Enabled"/>
    <br />
    <asp:Button ID="BtnRun" runat="server" CssClass="btn" Text="Save" OnClick="BtnRun_Click" />
    <br />
    <br />
    <asp:Button ID="BtnStepThree" runat="server" CssClass="btn" Text="Step Three" OnClick="BtnStepThree_Click" />
    <br />
    <br />
    <asp:Button ID="BtnStepFive" runat="server" CssClass="btn" Text="Step Five" OnClick="BtnStepFive_Click" />
    <br />
    <br />
    <asp:Button ID="BtnTransmit" runat="server" CssClass="btn" Text="Transmit" OnClick="BtnTransmit_Click" />
    <br />
    <br />
    <asp:Label ID="lblMsg" runat="server"></asp:Label>    
</asp:Content>