<%@ Page EnableSessionState="ReadOnly" Title="Export Carrier Alerts" Language="C#"
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="CarrierAlertExport.aspx.cs"
    Inherits="Afas.AfComply.UI.admin.AdminPortal.CarrierAlertExport" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">


    <h2>Export Carrier Alerts</h2>
    <br />
    <label class="lbl3">Select Employer</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    <br />
    <asp:Label ID="cofein" runat="server"></asp:Label>

    <br />
    <label class="lbl3">Tax Tear:</label>
    <asp:DropDownList ID="DdlTaxYear" runat="server" CssClass="ddl2">
    </asp:DropDownList>
    <br />

    <br />
    <asp:Button ID="BtnRun" runat="server" CssClass="btn" Text="Export" OnClick="BtnRun_Click" />
    <br />
    <asp:Label ID="lblMsg" runat="server"></asp:Label>

    <br />
    <asp:CheckBox ID="chkConfirm" runat="server" Checked="false" Text="I Confim I want to clear these Alerts."/>

    <br />
    <asp:Button ID="BtnClearCarrierAlerts" runat="server" CssClass="btn" Text="Clear Alerts" OnClick="BtnClearCarrierAlerts_Click" />

</asp:Content>
