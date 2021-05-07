<%@ Page Title="" Language="C#" MasterPageFile="~/securepages/SecurePages.Master" AutoEventWireup="true" CodeBehind="r_details.aspx.cs" Inherits="securepages_r_details" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="bodycontent" runat="server">
 <link rel="stylesheet" type="text/css" href="/leftnav.css?1.4.0.100" />
    <link rel="stylesheet" type="text/css" href="../reports.css" />

    <style type="text/css">
        .lbl {
            width: 150px;
            float: left;
            text-align: right;
            clear: left;
            padding: 5px;
            background-color: lightgray;
            height: 15px;
        }

        .lbl2 {
            padding: 5px;
        }

        .txt {
            padding: 5px;
            width: 50px;
            height: 15px;
        }
        .sidenav {
            width: 20%;
            height: 100%;
            z-index: 1;
            top: 25px;
            left: 0;
            background-color: #fff;
            border-bottom: #ddd;
            overflow-x: hidden;
            padding-top: 10px;
            font-family: "Myriad Pro", "DejaVu Sans Condensed", Helvetica, Arial, "sans-serif";
            text-align: left;
            font-weight: 200;
            display: inline;
        }
 .sidenav a {
 padding: 8px 8px 8px 8px;
                text-decoration: none;
                font-size: 14px;
                color: #6e6d71;
                display: block;
}
  .sidenav a:hover {
                    /*color: #eb0029;*/
                    text-decoration: underline;
                    border-left: #eb0029 4px solid;
                }


    </style>

<div id="header">
 <asp:HiddenField ID="HfUserName" runat="server" />
 <asp:HiddenField ID="HfDistrictID" runat="server" />
 </div>
 <div id="content">
<%
 if (null != Session["CurrentDistrict"] && ((employer)Session["CurrentDistrict"]).IrsEnabled == true)
{
%>
<%
                        
}
%>
<%= demo.getLeftLinks(null != Session["CurrentDistrict"] && ((employer)Session["CurrentDistrict"]).IrsEnabled) %> 

 </div>
 <div class="double_right_ebc">
                    <div style="float: right; padding-right: 50px;">
                        Export:
               
                        <asp:ImageButton ID="ImgBtnExport" runat="server" ToolTip="Export to .CSV file" Height="30px" ImageUrl="~/design/csv-file-icon.png" OnClick="ImgBtnExport_Click" />
                    </div>
     <br />
                     <div>
                    <asp:LinkButton ID="LbBack" runat="server" Text="Back" CssClass="btn1" OnClick="LbBack_Click"></asp:LinkButton>
                         </div>
                   <br />
                    <h3>Report Details</h3>

                    <asp:Panel ID="TrendingDisclaimer" Visible="false" runat="server">
                        <div>
                            <p>This represents the average number of hours worked by employee per month during the elapsed months of this current measurement period through the last uploaded payroll data.  It also takes into account any break in service weeks that might be part of this date range.</p>
                        </div>
                    </asp:Panel>

                    <asp:GridView ID="GvEmployeeList"
                        runat="server"
                        AutoGenerateColumns="true"
                        CssClass="gridviewHeader"
                        Width="800px"
                        AllowPaging="True"
                        PageSize="30"
                        AllowSorting="true"
                        OnSorting="GvEmployeeList_OnSorting"
                        OnPageIndexChanging="GvEmployeeList_PageIndexChanging">

                        <AlternatingRowStyle BackColor="White" />
                        <EditRowStyle BackColor="transparent" />
                        <FooterStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#eb0029" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EFF3FB" />
                        <SelectedRowStyle BackColor="White" Font-Bold="True" ForeColor="black" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#eb0029" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#eb0029" />
                    </asp:GridView>
                </div>
            <script>
        setTimeout(AutoLogout, <%= Feature.AutoLogoutTime %> );
        
        function AutoLogout() {
            alert("<%= Branding.AutoLogoutMessage %>");
            window.location = window.location.href;
        }
    </script>
</asp:Content>
