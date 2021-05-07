<%@ Page EnableSessionState="ReadOnly" Title="Employer IRS Transmission" Language="C#"
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="EmployerIRSTransmission.aspx.cs"
    Inherits="Afas.AfComply.UI.admin.AdminPortal.EmployerIRSTransmission" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" AsyncPostBackTimeout="3600"></asp:ToolkitScriptManager>
    <h2>Employer IRS Transmission</h2>
    <br />

    <label class="lbl">Select Year</label>
    <asp:DropDownList ID="DdlTaxYear" runat="server" CssClass="ddl2"></asp:DropDownList>
    <br />
    <br />

    <br />
    <label class="lbl">Select Employer</label>
    <asp:DropDownList ID="DdlFilterEmployers" runat="server" CssClass="ddl2" Width="500px"></asp:DropDownList>
    <br />
    <br />
    <label class="lbl">Prior Year</label>
    <asp:CheckBox ID="chbPriorYearInd" runat="server" />
    <br />
    <br />
    <label class="lbl">Transmission Type</label>
    <asp:DropDownList ID="DdlTransmissionType" runat="server" AutoPostBack="True" CssClass="ddl2" OnSelectedIndexChanged="DdlTransmissionType_SelectedIndexChanged"></asp:DropDownList>
    <br />
    <br />
    <asp:Panel ID="pnlRejection" Visible="false" runat="server">
        Please select if this Rejection is a Transmission Replacement or Submission Replacement
        <br />
        <asp:RadioButton ID="RbTransmissionReplacement" runat="server" Text="Transmission Replacement" GroupName="rejection" AutoPostBack="true" OnCheckedChanged="RbTransmissionReplacement_CheckedChanged" />
        (Replace employers initial transmission, this is basically a restart on the transmission status. This requires the initial receipt id, Example: 1095C-17-00900000)
        <br />
        <asp:RadioButton ID="RbSubmissionReplacement" runat="server" Text="Submission Replacement" GroupName="rejection" AutoPostBack="true" OnCheckedChanged="RbSubmissionReplacement_CheckedChanged" />
        (Replace an employers specific transmission, this requires the submission id, Example: 1095C-17-00900000|1)
        <br style="clear: both;" />
        <br />
        <asp:Panel ID="pnlOriginalReceiptId" Visible="false" runat="server">
            <label class="lbl">Original Receipt Id</label>
            <asp:DropDownList ID="DdlOriginalSubmissionData" runat="server" CssClass="ddl2"></asp:DropDownList>
        </asp:Panel>
    </asp:Panel>

    <asp:Panel ID="pnlCorrection" Visible="false" runat="server">

        <label class="lbl3">Receipt ID</label>
        <asp:DropDownList ID="DdlReceiptID" runat="server" CssClass="ddl2" AutoPostBack="True" OnSelectedIndexChanged="DdlReceiptID_SelectedIndexChanged"></asp:DropDownList>
        <br />
        <asp:RadioButtonList ID="rblCorrections" runat="server">
            <asp:ListItem Text="1094C Corrections" Value="1094C"></asp:ListItem>
            <asp:ListItem Text="1095C Corrections" Value="1095C"></asp:ListItem>
        </asp:RadioButtonList>
    </asp:Panel>

    <br />
    <br />

    <asp:Button ID="btnTransmit" runat="server" Text="Transmit to IRS" OnClick="btnTransmit_Click" Visible="false" />&nbsp&nbsp
    <asp:Button ID="btnGenerateACAForms" runat="server" Text="Generate ACA Forms" OnClick="btnGenerateACAForms_Click" />

    <asp:HiddenField ID="HiddenField1" runat="server" />
    <br />
    <asp:HiddenField ID="HiddenField2" runat="server" />

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
    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>

    <asp:HiddenField ID="HfDummyTrigger" runat="server" />
    <br />
    <asp:HiddenField ID="HfDummyTrigger2" runat="server" />
</asp:Content>
