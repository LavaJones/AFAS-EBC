<%@ Page EnableSessionState="ReadOnly" Title="Current Printed PDFs for All Employers" Language="C#" 
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="EmployersCurrentPdfStatus.aspx.cs" 
    Inherits="Afas.AfComply.UI.admin.AdminPortal.EmployersCurrentPdfStatus" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" AsyncPostBackTimeout="3600"></asp:ToolkitScriptManager>

    <h2>Current Printed PDFs for All Employers</h2>
    <br />

    <label class="lbl">Select Year</label>
    <asp:DropDownList ID="DdlTaxYear" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlTaxYear_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    <br />

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

    <asp:GridView ID="GridEmployerPdfCount" runat="server" AutoGenerateColumns="false"
        HeaderStyle-Height="30" RowStyle-Height="30" CellPadding="5" PageSize="100">
        <Columns>
            <asp:TemplateField HeaderText="Employer Name" HeaderStyle-Width="25%" SortExpression="name" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:HiddenField ID="HfEmployerID" runat="server" Value='<%# Eval("EmployerId") %>' />                    
                    <asp:Literal ID="LitName" runat="server" Text='<%# Eval("EmployerName") %>'></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="PDF Count" HeaderStyle-Width="25%" SortExpression="TransmissionStatusId" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:Literal ID="LitPdfCount" runat="server" Text='<%# Eval("PdfCount") %>'></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>

    <asp:HiddenField ID="HfDummyTrigger" runat="server" />
    <br />
    <asp:HiddenField ID="HfDummyTrigger2" runat="server" />

</asp:Content>