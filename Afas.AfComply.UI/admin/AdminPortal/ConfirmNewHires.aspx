<%@ Page EnableSessionState="ReadOnly" Title="Confirm New Hires" Language="C#" 
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="ConfirmNewHires.aspx.cs" 
    Inherits="Afas.AfComply.UI.admin.AdminPortal.ConfirmNewHires" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    
    
    <h2>Confirm the New Hires</h2>
    <br />
    <label class="lbl3">Select Employer</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged"  AutoPostBack="true"></asp:DropDownList>
    <br />
    <asp:Label ID="cofein" runat="server"></asp:Label>
    <br />
    <asp:Button ID="BtnSave" runat="server" CssClass="btn" Text="Confirm New Hires" OnClick="BtnSave_Click" />
    <br />
    <asp:Label ID="lblMsg" runat="server"></asp:Label>    

</asp:Content>