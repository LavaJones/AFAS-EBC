<%@ Page EnableSessionState="ReadOnly" Title="Nuke Employer" Language="C#" 
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="NukeEmployer.aspx.cs" 
    Inherits="Afas.AfComply.UI.admin.AdminPortal.NukeEmployer" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <h2>Nuke all of the Employee data!</h2>
    <br />
    <label class="lbl3">Select Employer</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged"  AutoPostBack="true"></asp:DropDownList>
    <br />
    <asp:Label ID="cofein" runat="server"></asp:Label>
    <br />
    <asp:CheckBox ID="chkConfirm" runat="server" Checked="false" Text="This will completely remove this employer and all data associated with it, I herby certify that I am authorized to complete this action."/>
    <br />
    <asp:Button ID="BtnNuke" runat="server" CssClass="btn" Text="Nuke" OnClick="BtnNuke_Click" />
    <br />
    <asp:Label ID="lblMsg" runat="server"></asp:Label>    
</asp:Content>