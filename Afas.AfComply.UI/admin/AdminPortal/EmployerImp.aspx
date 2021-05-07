<%@ Page EnableSessionState="ReadOnly" Title="Initial Measurement Period" Language="C#"
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="EmployerImp.aspx.cs"
    Inherits="Afas.AfComply.UI.admin.AdminPortal.EmployerImp" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>

    <h2>Edit Plan Years</h2>
    <br />
    <label class="lbl3">Select Employer</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    <br />
    <asp:Label ID="cofein" runat="server"></asp:Label>
    <br />

    <asp:HiddenField ID="HfDistrictID" runat="server" />
    <asp:HiddenField ID="HfEmployerTypeID" runat="server" />
    <asp:HiddenField ID="HfUserName" runat="server" />

    <div style="position: absolute; top: 15px; right: 85px;">
        <asp:Button ID="BtnInitialMeasurement" runat="server" CssClass="btn" Text="Submit" Enabled="true" OnClick="BtnInitialMeasurement_Click" />
    </div>

    <h3>Initial Measurement Cycle</h3>
    <p>
        Note: This period can be changed, but you will need contact your <%= Branding.CompanyShortName%> Consultant to have this measurement period changed.
                   
    </p>
    <label class="lbl4">Initial Measurement Period Length</label>
    <asp:DropDownList ID="DdlInitialLength" runat="server" CssClass="ddl" Enabled="true"></asp:DropDownList>
    <br />
    <br />

</asp:Content>
