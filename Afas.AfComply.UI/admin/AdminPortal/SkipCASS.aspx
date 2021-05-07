<%@ Page EnableSessionState="ReadOnly" Title="Authorize the skipping of the CASS process" Language="C#" 
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="SkipCASS.aspx.cs"
    Inherits="Afas.AfComply.UI.admin.AdminPortal.SkipCASS" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" AsyncPostBackTimeout="3600"></asp:ToolkitScriptManager>

    <h2>Skip the CASS process and go directly to Print</h2>
    <label class="lbl">Select Form</label>
    <asp:DropDownList ID="DdlForm" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlForm_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
    <br />
    <label class="lbl">Select Employer</label>
    <asp:DropDownList ID="DdlFilterEmployers" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlFilterEmployers_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
    <br />
    <label class="lbl">Select Year</label>
    <asp:DropDownList ID="DdlTaxYear" runat="server" CssClass="ddl2"></asp:DropDownList>
    <br />

    <label class="lbl">Corrected</label>
    <asp:CheckBox ID="chbCorrected" runat="server" />
    <br />

    <asp:HiddenField ID="HfDummyTrigger" runat="server" />
    <br />
    <asp:HiddenField ID="HfDummyTrigger2" runat="server" />

    <asp:Panel ID="PnlMessage" runat="server">
        <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
        </div>
        <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white; z-index: 10">
            <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                <asp:ImageButton ID="ImgBtnClose" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" BorderStyle="None" />
            </div>
            <h3 style="color: black;">Webpage Message</h3>
            <p style="color: darkgray">
                <asp:Literal ID="LitMessage" runat="server"></asp:Literal>
            </p>
            <br />
        </div>
    </asp:Panel>

    <asp:ModalPopupExtender ID="MpeWebMessage" runat="server" TargetControlID="HfDummyTrigger" OkControlID="ImgBtnClose" PopupControlID="PnlMessage"></asp:ModalPopupExtender>

    <asp:CheckBox ID="chkConfirm" runat="server" Checked="false" Text="I understand that this will skip the CASSGenerated and CASSRecived Steps and go straight to Print."/>
    <br />
    <asp:Button ID="BtnPrint" runat="server" Text="Print" OnClick="BtnPrint_Click" Width="67px" />
    <br />

    <asp:Label ID="lblMsg" runat="server"></asp:Label>    

</asp:Content>