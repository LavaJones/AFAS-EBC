<%@ Page EnableSessionState="ReadOnly" Title="Clone Plan Year" Language="C#" 
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="ClonePlanYear.aspx.cs" 
    Inherits="Afas.AfComply.UI.admin.AdminPortal.ClonePlanYear" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:HiddenField ID="HfDistrictID" runat="server" />

    <h2>Clone a Plan Year</h2>
    <br />
    <label class="lbl3">Select Employer</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged"  AutoPostBack="true"></asp:DropDownList>
    <br />
    <asp:Label ID="cofein" runat="server"></asp:Label>
    <br />
    <br />
    <asp:DropDownList ID="DdlPlanYear" runat="server" CssClass="ddl2"></asp:DropDownList>
    <br />
    <asp:DropDownList ID="FuturePrevious" runat="server" CssClass="ddl2">
        <asp:ListItem Value="-1" Text="Previous"></asp:ListItem>
        <asp:ListItem Value="1" Text="Future"></asp:ListItem>
    </asp:DropDownList>

    <br />
    <asp:Button ID="BtnClone" runat="server" CssClass="btn" Text="Clone" OnClick="BtnClone_Click" />
    <br />
    <asp:Label ID="lblMsg" runat="server"></asp:Label> 

    

</asp:Content>