<%@ Page EnableSessionState="ReadOnly" Title="Disable IRS Review" Language="C#"
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="DisableReview.aspx.cs"
    Inherits="Afas.AfComply.UI.admin.AdminPortal.DisableReview" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">


    <h2>Disable IRS Review</h2>
    <br />
    <label class="lbl3">Select Employer</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    <br />
    <asp:Label ID="cofein" runat="server"></asp:Label>
    <br />
    <asp:DropDownList ID="DdlCalendarYear" runat="server" CssClass="ddl">
        <asp:ListItem Text="2015" Value="2015"></asp:ListItem>
        <asp:ListItem Text="2016" Value="2016"></asp:ListItem>
        <asp:ListItem Text="2017" Value="2017"></asp:ListItem>
        <asp:ListItem Text="2018" Value="2018" Selected="True"></asp:ListItem>
    </asp:DropDownList>
    <br />
    <asp:Button ID="BtnRun" runat="server" CssClass="btn" Text="Disable" OnClick="BtnRun_Click" />
    <br />
    <asp:Label ID="lblMsg" runat="server"></asp:Label>

</asp:Content>
