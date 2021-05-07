<%@ Page EnableSessionState="ReadOnly" Title="Alert Export" Language="C#" 
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="EmployerTransmitReport.aspx.cs"
     Inherits="Afas.AfComply.UI.admin.AdminPortal.EmployerTransmitReport" %>
<%@ Import Namespace="Afas.AfComply.Domain" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <script>
        $(function () {
            $("#txtsdate").datepicker({
                defaultDate: "+1w",
                changeMonth: true,
                onClose: function (selectedDate) {
                    $("#txtedate").datepicker("option", "minDate", selectedDate);
                }
            });

            $("#txtedate").datepicker({
                defaultDate: "+1w",
                changeMonth: true,
                onClose: function (selectedDate) {
                    $("#txtsdate").datepicker("option", "maxDate", selectedDate);
                }

            });
        })
    </script>
    <div>
        Start Date: <asp:TextBox ID="txtsdate" runat="server"></asp:TextBox>End Date:<asp:TextBox ID="txtedate" runat="server"></asp:TextBox>
    </div>
    <asp:Button ID="BtnExportToCSV" CssClass="btn" runat="server" Text="Download" OnClick="BtnExportToCSV_Click"/>
</asp:Content>