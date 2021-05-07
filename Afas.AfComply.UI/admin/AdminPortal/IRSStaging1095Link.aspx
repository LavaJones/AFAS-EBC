<%@ Page EnableSessionState="ReadOnly" Title="Enable 1095 Review Link" Language="C#" 
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="IRSStaging1095Link.aspx.cs" 
    Inherits="Afas.AfComply.UI.admin.AdminPortal.IRSStaging1095Link" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" AsyncPostBackTimeout="3600"></asp:ToolkitScriptManager>
    
    <h2>Enable 1095 Review Link</h2>
    <br />
    <label class="lbl3">Select Employer</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged"  AutoPostBack="true"></asp:DropDownList>
   <br />

    <asp:Label ID="cofein" runat="server"></asp:Label>
    <br />
    <asp:CheckBox ID="chkConfirm" runat="server" Checked="false" Text="This enables the client to see their 1095 forms via the Status portal."/>
    <br />

    <asp:Button ID="BtnTransmit" runat="server" Text="Enable Link" OnClick="BtnTransmit_Click" />
    <asp:Button ID="BtnEnableIRS" runat="server" Text="2018 IRS" OnClick="BtnEnableIRS_Click" />
    <br />

    <asp:Button ID="BtnEnable1095" runat="server" Text="Enable Step 3" OnClick="BtnEnable1095_Click" />
    <br />
    <asp:Button ID="BtnEnable1094" runat="server" Text="Enable Step 4" OnClick="BtnEnable1094_Click" />
    <br />
    <asp:Button ID="BtnEnableTransmit" runat="server" Text="Enable Transmission" OnClick="BtnEnableTransmit_Click" />
    <br />
    <asp:Label ID="lblMsg" runat="server"></asp:Label>    

</asp:Content>