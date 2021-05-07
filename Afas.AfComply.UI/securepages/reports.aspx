<%@ Page Title="" Language="C#" MasterPageFile="~/securepages/SecurePages.Master" AutoEventWireup="true" CodeBehind="reports.aspx.cs" Inherits="securepages_reports" %>
<%@ Import Namespace="Afas.AfComply.Domain" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="bodycontent" runat="server">
 <link rel="stylesheet" type="text/css" href="/leftnav.css?1.4.0.100" />
 <link rel="stylesheet" type="text/css" href="/reports.css" />
    
    <style type="text/css">
        .lbl {
            width: 150px;
            float: left;
            text-align: right;
            clear: left;
            padding: 5px;
            /*background-color: lightgray;*/
            height: 15px;
        }

        .lbl2 {
            padding: 5px;
        }

        .txt {
            padding: 5px;
            /*width: 50px;*/
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
                    text-decoration: underline;
                    border-left: #eb0029 4px solid;
                }

</style>


 <div id="header" style =" height:90px">
                        <asp:HiddenField ID="HfUserName" runat="server"/>
                         <asp:HiddenField ID="HfDistrictID" runat="server" />
  </div>
    <div id="dropdown">
 Stability Period: 
<asp:DropDownList ID="DdlPlanYear" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DdlPlanYear_SelectedIndexChanged"></asp:DropDownList>
 <br />
Export:
 <asp:ImageButton ID="ImgBtnExport" runat="server" ToolTip="Export to .CSV file" Height="30px" ImageUrl="~/design/csv-file-icon.png" OnClick="ImgBtnExport_Click" />
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
<asp:UpdatePanel ID="UpReports" runat="server">
                    <ContentTemplate>
                     <div class="middle_ebc">
                            <h3>Reports</h3>
                            <asp:GridView ID="GvReports" runat="server" CssClass="gridviewHeader" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" CellPadding="1" Font-Size="14px" ForeColor="black" GridLines="None" Width="300px" OnSelectedIndexChanged="GvReports_SelectedIndexChanged">
                                <AlternatingRowStyle BackColor="White" />
                                <EditRowStyle BackColor="transparent" />
                                <FooterStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#eb0029" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#EFF3FB" />
                                <SelectedRowStyle BackColor="white" Font-Bold="True" ForeColor="black" />
                                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                <SortedAscendingHeaderStyle BackColor="#eb0029" />
                                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                <SortedDescendingHeaderStyle BackColor="#eb0029" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Name" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LbReportName" runat="server" Text='<%# Eval("REPORT_NAME") %>' CommandName="Select"></asp:LinkButton>
                                            <asp:HiddenField ID="HfReportID" runat="server" Value='<%# Eval("REPORT_ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>

                        </div>
<div class="right_ebc">






















                            <asp:Panel ID="PnlOngoing" runat="server">
                                <h3>
                                    <asp:Literal ID="LitTrendingHeader" runat="server"></asp:Literal>
                                </h3>


                                <label class="lbl">On-Track:</label>
                                <asp:TextBox ID="TxtOngoingOnTrack" runat="server" CssClass="txt" Enabled="false" Text="80"></asp:TextBox>
                                <br />
                                <label class="lbl">Caution:</label>
                                <asp:TextBox ID="TxtOngoingCaution" runat="server" CssClass="txt" Enabled="false" Text="8"></asp:TextBox>
                                <br />
                                <label class="lbl">Not On-Track:</label>
                                <asp:TextBox ID="TxtOngoingNotOnTrack" runat="server" CssClass="txt" Enabled="false" Text="15"></asp:TextBox>
                                <br />
                                <asp:Panel ID="TrendingDisclaimer" Visible="false" runat="server">
                                    <div>
                                        <p>This represents the average number of hours worked by employee per month during the elapsed months of this current measurement period through the last uploaded payroll data.  It also takes into account any break in service weeks that might be part of this date range.</p>
                                    </div>
                                </asp:Panel>
                                <br />
                                <asp:Chart ID="Chart" runat="server" Width="475px" Height="500px">
                                    <Titles>
                                        <asp:Title Text="Ongoing Employee Trend"></asp:Title>
                                    </Titles>

                                    <Series>
                                        <asp:Series Name="Series1" YValueType="Int32" XValueType="String">
                                        </asp:Series>
                                    </Series>
                                    <ChartAreas>
                                        <asp:ChartArea Name="ChartArea1" Area3DStyle-Enable3D="true" Area3DStyle-PointDepth="15">
                                            <AxisX Title="Employee Categories" Interval="1"></AxisX>
                                            <AxisY Title="Employee Count" Interval="10" TextOrientation="Rotated270"></AxisY>
                                        </asp:ChartArea>
                                    </ChartAreas>
                                </asp:Chart>
                            </asp:Panel>

                            <asp:Panel ID="PnlPayorPlay" runat="server">
                                In-Progress
                            </asp:Panel>
                        </div>







                    </ContentTemplate>
                </asp:UpdatePanel>
          
      

     

    <script>
        setTimeout(AutoLogout, <%= Feature.AutoLogoutTime %> );
        
        function AutoLogout() {
            alert("<%= Branding.AutoLogoutMessage %>");
            window.location = window.location.href;
        }
    </script>

</asp:Content>