<%@ Page EnableSessionState="ReadOnly" Title="Stage 1095 Data" Language="C#" 
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="StageForCorrectionRetransmission.aspx.cs" 
    Inherits="Afas.AfComply.UI.admin.AdminPortal.StageForCorrectionRetransmission" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" AsyncPostBackTimeout="3600"></asp:ToolkitScriptManager>
    
    <h2>Stage For IRS Correction Retransmission</h2>
    <br />
    <label class="lbl3">Select Employer</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2"></asp:DropDownList>
    <br />
    <label class="lbl3">Select Tax Year</label>
    <asp:DropDownList ID="DdlCalendarYear" runat="server" CssClass="ddl2">
        <asp:ListItem Text="2015" Value="2015" ></asp:ListItem>
        <asp:ListItem Text="2016" Value="2016" Selected="True"></asp:ListItem>
        <asp:ListItem Text="2017" Value="2017" ></asp:ListItem>
        <asp:ListItem Text="2018" Value="2018" ></asp:ListItem>
    </asp:DropDownList>
    <br />
    <br />

    <asp:CheckBox ID="chkConfirm" runat="server" Checked="false" Text="This stages the 1095 Corrected data for this client to be Retransmitted!"/>
    <br />

    <asp:Button ID="BtnReTransmit" runat="server" Text="ReTransmit" OnClick="BtnReTransmit_Click" />
    <br />

    <asp:Label ID="lblMsg" runat="server"></asp:Label> 

</asp:Content>
