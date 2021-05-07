<%@ Page EnableSessionState="ReadOnly" Title="Confirm New Hires" Language="C#" 
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="ConsultantInput.aspx.cs" 
    Inherits="Afas.AfComply.UI.admin.AdminPortal.ConfirmNewHires" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    
    
    <h2>Adding in consultant/data team</h2>
    <br />
    <label class="lbl3">Select Employer</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged"  AutoPostBack="true"></asp:DropDownList>
    <br />
    <label class="lbl">Name</label>
                        <asp:TextBox ID="ConsultantName" runat="server" CssClass="txt3"></asp:TextBox>
    <br />
    <label class="lbl">Phone Number</label>
                        <asp:TextBox ID="ConsultantPhoneNum" runat="server" CssClass="txt3"></asp:TextBox> 
                        <br />
    <label class="lbl">Title</label>
                        <asp:TextBox ID="Title" runat="server" CssClass="txt3"></asp:TextBox>
    <br />
        <asp:Button ID="BtnRun" runat="server" CssClass="btn" Text="Save Data" OnClick="BtnRun_Click" />
    <br />
    <asp:Label ID="ClblMsg" runat="server"></asp:Label>   
    <br />
    <br />
    <asp:GridView ID="gvEmployerConsultant" runat="server" AutoGenerateColumns="True"
                    HeaderStyle-Height="30" RowStyle-Height="30" CellPadding="5" BorderColor="White">
    </asp:GridView>

</asp:Content>