<%@ Page EnableSessionState="ReadOnly" Title="Edit Employer" Language="C#"
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="EditEmployer.aspx.cs"
    Inherits="Afas.AfComply.UI.admin.AdminPortal.EditEmployer" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">


    <h2>Update Information</h2>
    <br />
    <label class="lbl3">Select Employer</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    <br />
    <asp:Label ID="cofein" runat="server"></asp:Label>
    <br />

    <h3>Profile</h3>
    <label class="lbl">EIN</label>
    <asp:TextBox ID="TxtEmployerEIN" runat="server" CssClass="txt3"></asp:TextBox>xx-xxxxxxx                   
    <br />
    <label class="lbl">Legal Name:</label>
    <asp:TextBox ID="TxtEmployerIrsName" runat="server" CssClass="txt3"></asp:TextBox>
    <br />
    <label class="lbl">DBA Name:</label>
    <asp:TextBox ID="TxtEmployerDbaName" runat="server" CssClass="txt3"></asp:TextBox>
    <br />
    <label class="lbl">Address</label>
    <asp:TextBox ID="TxtEmployerAddress" runat="server" CssClass="txt3"></asp:TextBox>
    <br />
    <label class="lbl">City</label>
    <asp:TextBox ID="TxtEmployerCity" runat="server" CssClass="txt3"></asp:TextBox>
    <br />
    <label class="lbl">State</label>
    <asp:DropDownList ID="DdlEmployerState" runat="server" CssClass="ddl2"></asp:DropDownList>
    <br />
    <label class="lbl">Zip</label>
    <asp:TextBox ID="TxtEmployerZip" runat="server" CssClass="txt3"></asp:TextBox>
    <br />
    <label class="lbl">Employer Type</label>
    <asp:DropDownList ID="DdlEmployerType" runat="server" CssClass="ddl2"></asp:DropDownList>
    <br />

    <br />
    <br />

    <asp:Button ID="BtnSave" runat="server" CssClass="btn" Text="Save" OnClick="BtnSave_Click" />
    <br />
    <asp:Label ID="lblMsg" runat="server"></asp:Label>

</asp:Content>
