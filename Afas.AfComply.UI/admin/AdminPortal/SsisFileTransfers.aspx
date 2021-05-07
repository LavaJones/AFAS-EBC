<%@ Page Title="Ssis File Transferred" Language="C#" MasterPageFile="~/admin/AdminPortal/AdminPortal.Master" AutoEventWireup="true" CodeBehind="SsisFileTransfers.aspx.cs" Inherits="Afas.AfComply.UI.admin.AdminPortal.SsisFileTransfers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
<link rel="stylesheet" type="text/css" href="/_js/jquery-ui-themes-1.10.2/themes/ui-lightness/jquery-ui.css" />
<asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <div>
 <h2>File Transferred through SSIS</h2>
    <br />
<h4> </h4>
 <label class="lbl3">StartDate</label>:
<asp:TextBox ID="Txt_Start_Date" runat="server" CssClass="txt3"></asp:TextBox>
<br />

 <label class="lbl3">EndDate</label>:
<asp:TextBox ID="Txt_End_Date" runat="server" CssClass="txt3"></asp:TextBox>
<br />
<br />
<asp:ImageButton ID="ImgBtnExportCSV" runat="server" align="left" Height="30px" ImageUrl="~/images/csv.png" OnClick="ImgBtnExportCSV_Click" ToolTip="Export Enabled SssiFileTransferReport to .CSV file" />

</div>
<script src="/_js/jquery-ui-1.10.2/jquery-1.9.1.js" type="text/javascript"></script>
<script src="/_js/jquery-ui-1.10.2/ui/jquery-ui.js" type="text/javascript"></script>

 <script type="text/javascript">
        window.onload = function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
        }

        function endRequestHandler(sender, args) {
            init();
        }

        function init() {
            $("*[id*='Txt_Start_Date']").each(function () {
                $(this).datepicker(
                {
                    showOn: "button",
                    buttonImage: "/images/calendar.png",
                    buttonImageOnly: true,
                    yearRange: "-3:+5",
                    changeMonth: true,
                    changeYear: true,
                });
            });

            $("*[id*='Txt_End_Date']").each(function () {
                $(this).datepicker(
                {

                    showOn: "button",
                    buttonImage: "/images/calendar.png",
                    buttonImageOnly: true,
                    yearRange: "-3:+5",
                    changeMonth: true,
                    changeYear: true,
                });
            });
  }

 $(function () { 
            init();
        });
</script>
</asp:Content>
