<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="s_setup.aspx.cs" Inherits="securepages_s_setup" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <link href="Content/bootstrap.min.css" rel="stylesheet" />

    <link href="Content/bootstrap.css" rel="stylesheet" />
    <script src="Scripts/popper.js"></script>
    <script src="Scripts/jquery-1.10.2.min.js"></script>
    <script src="Scripts/bootstrap.js"></script>


    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />

    <meta name="viewport" content="width=device-width, initial-scale=1" />


    <link rel="stylesheet" type="text/css" href="/employer_setup.css" />
    <link rel="stylesheet" type="text/css" href="/leftnav.css?1.4.0.100" />



    <script src="/_js/jquery-ui-1.10.2/jquery-1.9.1.js" type="text/javascript"></script>

    <script src="/_js/jquery-ui-1.10.2/ui/jquery-ui.js" type="text/javascript"></script>
    <style>
        body {
            margin: 0;
            font-family: Arial, Helvetica, sans-serif;
        }

        .header {
            overflow: hidden;
            background-color: #eb0029;
            height: 10px;
        }

            .header a {
                float: left;
                color: black;
                text-align: center;
                text-decoration: none;
                list-style-type: none;
                font-size: 30px;
            }

        div.img {
            content: url(C:\Development\Afas.AfComply\branches\Sanja2\Afas.AfComply.UI\images);
        }

        #toplinks {
            text-decoration: none;
            font-size: 5px;
            list-style: none;
            padding-right: 90px;
        }

        .logo {
            margin: 0;
            float: left;
        }

        .header a.logo {
            font-size: 25px;
        }

        .header a:hover {
            background-color: #ddd;
            color: black;
        }

        .header a.active {
            background-color: dodgerblue;
            color: white;
        }

        .header-right {
            float: right;
        }

        @media screen and (max-width: 500px) {
            .header a {
                float: none;
                display: block;
                text-align: left;
            }

            .header-right {
                float: none;
            }
        }

        ul {
            overflow: hidden;
            background-color: none;
            align-content: baseline;
            float: none;
            width: 70%;
            text-align: center;
            float: none;
        }

        li {
            position: center;
            align-content: center;
            text-align: center;
            display: inline-block;
            border-right: 1px solid #ddd;
            padding: 15px;
        }

            li:last-child {
                border-right: none;
            }

            li a {
                display: block;
                color: #6E6D71;
                text-align: center;
                text-decoration: none;
            }

                li a:hover:not(.active) {
                    text-decoration: underline;
                }

        .active {
            border-left: 4px #EB0029;
        }

        .sidenav {
            width: 20%;
            height: 100%;
            z-index: 1;
            top: 225px;
            left: 0;
            background-color: #fff;
            border-bottom: #ddd;
            overflow-x: hidden;
            padding-top: 10px;
            font-family: "Myriad Pro", "DejaVu Sans Condensed", Helvetica, Arial, "sans-serif";
            text-align: left;
            font-weight: 200;
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

        #Topcontainer {
            position: relative;
            width: 1510px;
            overflow: auto;
            min-height: 156px;
            left: 0px;
        }

        #container {
            position: relative;
            width: 1510px;
            padding: 13px;
            /*margin:10px auto;*/
            overflow: auto;
            min-height: 1500px;
            top: 4px;
            /*left: -11px;*/
        }

        .mega-menu-bar {
            background-color: #eee;
            content: " ";
            display: block;
            min-width: 100%;
            height: 7px;
            overflow: hidden;
            position: absolute;
            z-index: 10;
            -moz-transition: height 250ms ease;
            -o-transition: height 250ms ease;
            -webkit-transition: height 250ms ease;
            transition: height 250ms ease;
            -webkit-transition-delay: 500ms;
            transition-delay: 500ms;
        }

        #bottomcontainer {
            position: relative;
            width: 1510px;
            overflow: auto;
            min-height: 156px;
            left: 0px;
        }

        .w3-badge, .w3-tag {
            background-color: #eb0029;
            color: #fff;
            display: inline-block;
            padding-left: 8px;
            padding-right: 8px;
            text-align: center;
        }
    </style>
    <title><%= Branding.ProductName %> - <%= Branding.CompanyName %></title>

    <script type="text/javascript">
        window.onload = function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequestHandler);
        }

        function endRequestHandler(sender, args) {
            init();
        }

        function init() {
            $("*[id*='Txt_M_Meas_start']").each(function () {
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
            $("*[id*='Txt_M_Meas_end']").each(function () {
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
            $("*[id*='Txt_M_Admin_start']").each(function () {
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
            $("*[id*='Txt_M_Admin_end']").each(function () {
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
            $("*[id*='Txt_M_Open_start']").each(function () {
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
            $("*[id*='Txt_M_Open_end']").each(function () {
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
            $("*[id*='Txt_M_Stab_start']").each(function () {
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
            $("*[id*='Txt_M_Stab_end']").each(function () {
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
            $("*[id*='TxtSummerBreakStart']").each(function () {
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
            $("*[id*='TxtSummerBreakStart2']").each(function () {
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
            $("*[id*='TxtSummerBreakEnd']").each(function () {
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
            $("*[id*='TxtSummerBreakEnd2']").each(function () {
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
            $("*[id*='Txt_npy_StartDate']").each(function () {
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

        $(function () { // DOM ready
            init();
        });

        function uploadError(sender, args) {
            document.getElementById('lblStatus').innerText = args.get_fileName(),
                "<span style='color:red;'>" + args.get_errorMessage() + "</span>";
        }

        function StartUpload(sender, args) {
            document.getElementById('lblStatus').innerText = 'Uploading Started.';
        }

        function UploadComplete(sender, args) {
            var filename = args.get_fileName();
            var contentType = args.get_contentType();
            var text = "Size of " + filename + " is " + args.get_length() + " bytes";
            if (contentType.length > 0) {
                text += " and content type is '" + contentType + "'.";
            }
            document.getElementById('lblStatus').innerText = text;
        }

    </script>

</head>
<body>

    <form id="form1" runat="server" enctype="multipart/form-data" method="post">
        <div class="header">
            <div class=" image"></div>
        </div>
        <div id="Topcontainer">
            <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
            <div class="header-right">
                <div id="toplinks">
                    <h1>
                        <p style="font-size: 12px;">Need Help? Call <%= Branding.PhoneNumber %></p>
                        <p style="font-size: 12px;">
                            <asp:Literal ID="LitUserName" runat="server"></asp:Literal>
                        </p>
                        <p style="font-size: 12px;">
                            <asp:Literal ID="LitEmployer" runat="server"></asp:Literal>
                        </p>
                        <p>
                            <asp:Button ID="BtnLogout" CssClass="btn" runat="server" Text="Log Out" OnClick="BtnLogout_Click" UseSubmitBehavior="false" />
                        </p>
                        <p>
                            <asp:HiddenField ID="HfDistrictID" runat="server" />
                        </p>
                    </h1>
                </div>
            </div>
            <div style="padding-left: 20px">
                <a href="/securepages/">
                    <img src="<%= Page.ResolveUrl(Branding.LogoUri) %>" style="height: 70px" alt="<%= Branding.ProductName %> Logo" class="logo" />
                </a>
                <div class=" nav">
                    <ul>
                        <li><a href="default.aspx">Home</a></li>
                        <li><a href="e_find.aspx">Employee</a></li>
                        <li><a href="r_reporting.aspx">Reports</a></li>
                        <li><a href="s_setup.aspx">Employer Setup</a></li>
                        <%if (true == Feature.QlikEnabled)
                            {  %>
                        <li><a href="alerts.aspx">Alerts <span class="w3-badge w3-red">
                            <asp:Literal ID="CountAlert" runat="server" Text=""></asp:Literal>
                        </span></a></li>
                        <% } %>
                        <li><a href="<%= Feature.HomePageExternalLink %>">Help</a></li>
                        <li><a href="<%= Feature.GuidePageExternalLink %>" target="_blank">How-to Guides</a></li>
                        <li><a href="/FileCabinet/ViewFileCabinet">File Cabinet</a></li>
                        <asp:HiddenField ID="HfEmployerTypeID" runat="server" />
                        <asp:HiddenField ID="HfUserName" runat="server" />

                    </ul>
                </div>
            </div>
        </div>
        <div id="container">

            <div style="float: right; padding-right: 153px;">
                Stability Period: 
                       
                        <asp:UpdatePanel ID="UpPlanYear" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <asp:DropDownList ID="DdlPlanYear" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DdlPlanYear_SelectedIndexChanged" Style="max-width: 170px;"></asp:DropDownList>
                            </ContentTemplate>
                        </asp:UpdatePanel>

            </div>

            <div id="content">

                <%if (null != Session["CurrentDistrict"] && ((employer)Session["CurrentDistrict"]).IrsEnabled == true)
                    {
                %>
                <%
                    }
                %>
                <%= demo.getLeftLinks(null != Session["CurrentDistrict"] && ((employer)Session["CurrentDistrict"]).IrsEnabled) %>



                <asp:UpdatePanel ID="UpReports" runat="server">
                    <ContentTemplate>

                        <div class="middle_ebc">
                            <h3>Set-up</h3>
                            <asp:HiddenField ID="HfMeasurementTypeID" runat="server" Value="0" />
                            <asp:GridView ID="GvSetup" runat="server" CssClass="gridviewHeader" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" CellPadding="1" Font-Size="15px" ForeColor="#333333" GridLines="None" Width="300px" OnSelectedIndexChanged="GvSetup_SelectedIndexChanged">
                                <AlternatingRowStyle BackColor="White" />
                                <EditRowStyle BackColor="#2461BF" />
                                <FooterStyle BackColor="#eb0029" Font-Bold="true" ForeColor="White" />
                                <HeaderStyle BackColor="#eb0029" Font-Bold="true" ForeColor="White" />
                                <PagerStyle BackColor="#eb0029" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#EFF3FB" />
                                <SelectedRowStyle BackColor="white" Font-Bold="true" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#eb0029" />
                                <SortedAscendingHeaderStyle BackColor="#eb0029" />
                                <SortedDescendingCellStyle BackColor="#eb0029" />
                                <SortedDescendingHeaderStyle BackColor="#eb0029" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Name" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LbReportName" runat="server" Text='<%# Eval("SETUP_NAME") %>' CommandName="Select"></asp:LinkButton>
                                            <asp:HiddenField ID="HfReportID" runat="server" Value='<%# Eval("SETUP_ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>

                        <div class="right_ebc">
                            <asp:Panel ID="PnlInitialMeasurement" runat="server" Visible="false">

                                <h3>Initial Measurement Period</h3>
                                <p>
                                    Note: The Length of the intial Measurement  period can be changed, but you will need contact your <%= Branding.CompanyShortName%> Consultant to change it within your account.
                   
                                </p>
                                <label class="lbl4">Initial Measurement Period Length:</label>
                                <asp:DropDownList ID="DdlInitialLength" runat="server" CssClass="ddl" Enabled="false"></asp:DropDownList>
                                <br />
                                <br />
                            </asp:Panel>
                            <asp:Panel ID="PnlMeasurement" runat="server">
                                <%if (Feature.SelfMeasurementPeriodsEnabled)
                                    {%>
                                <div style="position: absolute; top: 15px; right: 85px;">
                                    <asp:Button ID="BtnSaveMeasurmentPeriod" runat="server" CssClass="btn" Text="New" />
                                </div>
                                <asp:ModalPopupExtender ID="MpeMeasurementPeriod" runat="server" PopupControlID="PnlNewMeasurement" TargetControlID="BtnSaveMeasurmentPeriod"></asp:ModalPopupExtender>
                                <asp:Panel ID="PnlNewMeasurement" runat="server" DefaultButton="BtnSaveMeasurmentPeriod">
                                    <div style="position: fixed; top: 0; left: 0; background-color: white; opacity: 0.8; width: 100%; height: 100%;">
                                    </div>
                                    <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                                        <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                            <asp:ImageButton ID="ImageButton2" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" />
                                        </div>
                                        <h3>Step 1: Choose Stability Period</h3>
                                        <label class="lbl4">Stability Period</label>
                                        <asp:DropDownList ID="Ddl_M_PlanYear" runat="server" CssClass="ddl" Width="310px"></asp:DropDownList>
                                        <br />
                                        <h3>Step 2: Choose Employee Type</h3>
                                        <label class="lbl4">Employee Type</label>
                                        <asp:DropDownList ID="Ddl_M_EmployeeType" runat="server" CssClass="ddl" Width="310px"></asp:DropDownList>
                                        <br />
                                        <h3>Step 3: Choose Measurement Type</h3>
                                        <label class="lbl4">Measurement Type</label>
                                        <asp:DropDownList ID="Ddl_M_MeasurementType" runat="server" CssClass="ddl" Width="310px" AutoPostBack="true" OnSelectedIndexChanged="Ddl_M_MeasurementType_SelectedIndexChanged"></asp:DropDownList>
                                        <br />
                                        <h3>Step 4: Measurement Periods</h3>
                                        Selected Dates:
                       
                                        <asp:Literal ID="LitStartDate" runat="server"></asp:Literal>
                                        to 
                       
                                        <asp:Literal ID="LitEndDate" runat="server"></asp:Literal>
                                        <br />
                                        <br />
                                        <p>
                                            <label class="lbl4">Measurement Period:</label>


                                            <asp:TextBox ID="Txt_M_Meas_start" runat="server" CssClass="txt" Width="85px"></asp:TextBox>
                                            to
                       
                                        <asp:TextBox ID="Txt_M_Meas_end" runat="server" CssClass="txt" Width="75px"></asp:TextBox>
                                        </p>
                                        <br />
                                        <p>
                                            <label class="lbl4">Administrative Period:</label>
                                            <asp:TextBox ID="Txt_M_Admin_start" runat="server" CssClass="txt" Width="75px"></asp:TextBox>
                                            to
                       
                                        <asp:TextBox ID="Txt_M_Admin_end" runat="server" CssClass="txt" Width="75px"></asp:TextBox>
                                        </p>
                                        <br />
                                        <p>
                                            <label class="lbl4">Open Enrollment:</label>
                                            <asp:TextBox ID="Txt_M_Open_start" runat="server" CssClass="txt" Width="75px"></asp:TextBox>
                                            to
                       
                                        <asp:TextBox ID="Txt_M_Open_end" runat="server" CssClass="txt" Width="75px"></asp:TextBox>
                                        </p>
                                        <br />
                                        <p>
                                            <label class="lbl4">Stability Period:</label>
                                            <asp:TextBox ID="Txt_M_Stab_start" runat="server" CssClass="txt" Width="75px"></asp:TextBox>
                                            to
                       
                                        <asp:TextBox ID="Txt_M_Stab_end" runat="server" CssClass="txt" Width="75px"></asp:TextBox>
                                        </p>
                                        <br />

                                        <asp:Panel ID="PnlSummerWindow" runat="server" Visible="false">
                                            <h3>Step 5: Breaks In Service</h3>
                                            <label class="lbl">New Break:</label>
                                            <asp:TextBox ID="TxtSummerBreakStart" runat="server" CssClass="txt" Width="75px"></asp:TextBox>
                                            to                       
                                            <asp:TextBox ID="TxtSummerBreakEnd" runat="server" CssClass="txt" Width="75px"></asp:TextBox>
                                            <asp:Button ID="AddNewTempBreak" CssClass="btn" Text="Add" runat="server" OnClick="AddNewTempBreak_Click" />
                                            <br />

                                            <asp:GridView ID="Gv_TempBreakOfService" runat="server" AutoGenerateColumns="false"
                                                EmptyDataText="There are currently NO breaks in service." BackColor="White" BorderColor="#336666"
                                                BorderStyle="Double" BorderWidth="3px" CellPadding="4" GridLines="Horizontal"
                                                OnRowDeleting="Gv_TempBreakOfService_RowDeleting"
                                                OnRowEditing="Gv_TempBreakOfService_RowEditing"
                                                OnRowCancelingEdit="Gv_TempBreakOfService_RowCancelingEdit"
                                                OnRowUpdating="Gv_TempBreakOfService_RowUpdating">
                                                <FooterStyle BackColor="White" ForeColor="#333333" />
                                                <HeaderStyle BackColor="#336666" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#336666" ForeColor="White" HorizontalAlign="Center" />
                                                <RowStyle BackColor="White" ForeColor="#333333" Height="30px" />
                                                <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                                                <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                <SortedAscendingHeaderStyle BackColor="#487575" />
                                                <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                <SortedDescendingHeaderStyle BackColor="#275353" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Edit">
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="ImgBtnEdit" runat="server" CommandName="Edit" ImageUrl="~/images/edit_notes.png" Height="25px" Style="float: right;" />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:ImageButton ID="ImgBtnUpdate" runat="server" CommandName="Update" ImageUrl="~/images/disk-save.png" Height="25px" />
                                                            <asp:ImageButton ID="ImgBtnCancel" runat="server" CommandName="Cancel" ImageUrl="~/images/error.png" Height="25px" />
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="Start Date" HeaderStyle-Width="225px" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:HiddenField ID="HiddenBreakId" runat="server" Value='<%# Eval("BreakInServiceId") %>' />
                                                            <asp:Literal ID="TxtSummerBreakStart" runat="server" Text='<%# Eval("StartDate", "{0:MM/dd/yyyy}") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:HiddenField ID="HiddenBreakId" runat="server" Value='<%# Eval("BreakInServiceId") %>' />
                                                            <asp:TextBox ID="TxtSummerBreakStart" runat="server" Text='<%# Eval("StartDate", "{0:MM/dd/yyyy}") %>' CssClass="txt" Width="75px"></asp:TextBox>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="End Date" HeaderStyle-Width="225px" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:Literal ID="TxtSummerBreakEnd" runat="server" Text='<%# Eval("EndDate", "{0:MM/dd/yyyy}") %>'></asp:Literal>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:TextBox ID="TxtSummerBreakEnd" runat="server" Text='<%# Eval("EndDate", "{0:MM/dd/yyyy}") %>' CssClass="txt" Width="75px"></asp:TextBox>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderStyle-Width="25px" HeaderStyle-HorizontalAlign="Left">
                                                        <ItemTemplate>
                                                            <asp:ImageButton Width="25px" ID="ImgBtnDelete" runat="server" ImageUrl="~/images/close_box_red.png" Height="20px" CommandName="Delete" ToolTip="Delete this Break In Service?" />
                                                            <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="ImgBtnDelete" ConfirmText="Are you sure you want to DELETE this Break In Service?"></asp:ConfirmButtonExtender>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                        <br />
                                        <label class="lbl4">.</label>
                                        <asp:Button ID="BtnSaveMeasurementPeriod" runat="server" CssClass="btn" Text="Submit" OnClick="BtnSaveMeasurementPeriod_Click" />
                                        <br />
                                        <asp:Label ID="Lbl_M_message" runat="server" BackColor="Yellow"></asp:Label>
                                        <br />
                                        <br />
                                    </div>
                                </asp:Panel>
                                <%} %>
                                <h3>Measurement Periods</h3>
                                * Measurement periods are specific to each Stability Period &amp; EMPLOYEE TYPE. If you have multiple Stability Periods or EMPLOYEE TYPES, use the dropdown box to view the different measurement periods.
       
      

                                <h3>Employee Type</h3>
                                Employee Type is for employers who
                                <br />
                                1)Wish to have different measurement periods for different groups of employees;<br />
                                2)Groups of employees who have a different Break In Service length (educational employers only).
      
                                <br />
                                <br />

                                <label class="lbl4">Employee Type</label>
                                <asp:DropDownList ID="DdlEmployeeType" runat="server" CssClass="ddl2" AutoPostBack="true" OnSelectedIndexChanged="DdlEmployeeType_SelectedIndexChanged" onchange="init()"></asp:DropDownList>

                                <asp:Panel ID="PnlTransitionMeasurement" runat="server">
                                    <asp:Repeater ID="RptTransitionMeasurments" runat="server" OnItemDataBound="RptTransitionMeasurments_ItemDataBound" OnItemCommand="RptTransitionMeasurments_ItemCommand">
                                        <ItemTemplate>
                                            <h3>
                                                <asp:Literal ID="LitMeasurementType" runat="server"></asp:Literal>
                                                <asp:ImageButton ID="ImgBtnHistoryTransitionMeasurement" runat="server" ImageUrl="~/images/history.png" Height="25px" Style="float: right; padding-right: 35px;" />
                                                <%if (Feature.SelfMeasurementPeriodsEnabled)
                                                    {%>
                                                <asp:ImageButton ID="ImgBtnEditTransitionMeasurement" runat="server" ImageUrl="~/images/edit_notes.png" Height="25px" Style="float: right;" />
                                                <%} %>
                                            </h3>
                                            <asp:HiddenField ID="Hf_M_MeasurementID" runat="server" Value='<%# Eval("MEASUREMENT_ID") %>' />
                                            <asp:HiddenField ID="Hf_M_MeasurementTypeID" runat="server" Value='<%# Eval("MEASUREMENT_TYPE_ID") %>' />
                                            <label class="lbl4">Measurement Period :</label>
                                            <asp:TextBox ID="Txt_tra_meas_start" runat="server" CssClass="txt2" Enabled="false" Text='<%# Eval("MEASUREMENT_START", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                            to
       
                                            <asp:TextBox ID="Txt_tra_meas_end" runat="server" CssClass="txt2" Enabled="false" Text='<%# Eval("MEASUREMENT_END", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                            <br />
                                            <label class="lbl4">Administrative Period :</label>
                                            <asp:TextBox ID="Txt_tra_admin_start" runat="server" CssClass="txt2" Enabled="false" Text='<%# Eval("MEASUREMENT_ADMIN_START", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                            to
       
                                            <asp:TextBox ID="Txt_tra_admin_end" runat="server" CssClass="txt2" Enabled="false" Text='<%# Eval("MEASUREMENT_ADMIN_END", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                            <br />
                                            <label class="lbl4">Open Enrollment :</label>
                                            <asp:TextBox ID="Txt_tra_open_start" runat="server" CssClass="txt2" Enabled="false" Text='<%# Eval("MEASUREMENT_OPEN_START", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                            to
       
                                            <asp:TextBox ID="Txt_tra_open_end" runat="server" CssClass="txt2" Enabled="false" Text='<%# Eval("MEASUREMENT_OPEN_END", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                            <br />
                                            <label class="lbl4">Stability Period :</label>
                                            <asp:TextBox ID="Txt_tra_stab_start" runat="server" CssClass="txt2" Enabled="false" Text='<%# Eval("MEASUREMENT_STAB_START", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                            to
       
                                            <asp:TextBox ID="Txt_tra_stab_end" runat="server" CssClass="txt2" Enabled="false" Text='<%# Eval("MEASUREMENT_STAB_END", "{0:MM/dd/yyyy}") %>'></asp:TextBox>

                                            <asp:Panel ID="PnlSummerWindow3" runat="server" Visible="false">
                                                <h3>Breaks In Service</h3>
                                                <asp:GridView ID="Gv_DisplayBreakOfService" runat="server" AutoGenerateColumns="false"
                                                    EmptyDataText="There are currently NO breaks in service." BackColor="White" BorderColor="#336666"
                                                    BorderStyle="Double" BorderWidth="3px" CellPadding="4" GridLines="Horizontal"
                                                    OnRowDeleting="Gv_BreakOfService_RowDeleting"
                                                    OnRowEditing="Gv_BreakOfService_RowEditing"
                                                    OnRowCancelingEdit="Gv_BreakOfService_RowCancelingEdit"
                                                    OnRowUpdating="Gv_BreakOfService_RowUpdating">
                                                    <FooterStyle BackColor="White" ForeColor="#333333" />
                                                    <HeaderStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#eb0029" ForeColor="White" HorizontalAlign="Center" />
                                                    <RowStyle BackColor="White" ForeColor="#333333" Height="30px" />
                                                    <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                                                    <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                    <SortedAscendingHeaderStyle BackColor="#487575" />
                                                    <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                    <SortedDescendingHeaderStyle BackColor="#275353" />
                                                    <Columns>

                                                        <asp:TemplateField HeaderText="Start Date" HeaderStyle-Width="225px" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:HiddenField ID="HiddenBreakId" runat="server" Value='<%# Eval("BreakInServiceId") %>' />
                                                                <asp:Literal ID="TxtSummerBreakStart" runat="server" Text='<%# Eval("StartDate", "{0:MM/dd/yyyy}") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:HiddenField ID="HiddenBreakId" runat="server" Value='<%# Eval("BreakInServiceId") %>' />
                                                                <asp:TextBox ID="TxtSummerBreakStart" runat="server" Text='<%# Eval("StartDate", "{0:MM/dd/yyyy}") %>' CssClass="txt" Width="75px"></asp:TextBox>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:TemplateField HeaderText="End Date" HeaderStyle-Width="225px" HeaderStyle-HorizontalAlign="Left">
                                                            <ItemTemplate>
                                                                <asp:Literal ID="TxtSummerBreakEnd" runat="server" Text='<%# Eval("EndDate", "{0:MM/dd/yyyy}") %>'></asp:Literal>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:TextBox ID="TxtSummerBreakEnd" runat="server" Text='<%# Eval("EndDate", "{0:MM/dd/yyyy}") %>' CssClass="txt" Width="75px"></asp:TextBox>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>

                                                    </Columns>
                                                </asp:GridView>
                                            </asp:Panel>

                                            <%if (Feature.SelfMeasurementPeriodsEnabled)
                                                {%>
                                            <asp:Panel ID="PnlEditTransitionMeasurement" runat="server">
                                                <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                                                </div>
                                                <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                                                    <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                                        <asp:ImageButton ID="ImageButton2" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" />
                                                    </div>
                                                    <h3>Edit Measurement Period</h3>
                                                    <label class="lblClear">Stability Period:</label>
                                                    <asp:Literal ID="LitPlanName" runat="server"></asp:Literal>
                                                    <br />
                                                    <label class="lblClear">Employee Type:</label>
                                                    <asp:Literal ID="LitEmployeeType" runat="server"></asp:Literal>
                                                    <br />
                                                    <label class="lblClear">Start/End Date:</label>
                                                    <asp:Literal ID="LitStartDate" runat="server"></asp:Literal>
                                                    to 
           
                                                    <asp:Literal ID="LitEndDate" runat="server"></asp:Literal>
                                                    <br />
                                                    <label class="lbl4">Measurement Period:</label>
                                                    <asp:TextBox ID="Txt_M_Meas_start" runat="server" CssClass="txt2" Text='<%# Eval("MEASUREMENT_START", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                                    to
           
                                                    <asp:TextBox ID="Txt_M_Meas_end" runat="server" CssClass="txt2" Text='<%# Eval("MEASUREMENT_END", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                                    <br />
                                                    <label class="lbl4">Administrative Period:</label>
                                                    <asp:TextBox ID="Txt_M_Admin_start" runat="server" CssClass="txt2" Text='<%# Eval("MEASUREMENT_ADMIN_START", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                                    to
           
                                                    <asp:TextBox ID="Txt_M_Admin_end" runat="server" CssClass="txt2" Text='<%# Eval("MEASUREMENT_ADMIN_END", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                                    <br />
                                                    <label class="lbl4">Open Enrollment:</label>
                                                    <asp:TextBox ID="Txt_M_Open_start" runat="server" CssClass="txt2" Text='<%# Eval("MEASUREMENT_OPEN_START", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                                    to
           
                                                    <asp:TextBox ID="Txt_M_Open_end" runat="server" CssClass="txt2" Text='<%# Eval("MEASUREMENT_OPEN_END", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                                    <br />
                                                    <label class="lbl4">Stability Period:</label>
                                                    <asp:TextBox ID="Txt_M_Stab_start" runat="server" CssClass="txt2" Text='<%# Eval("MEASUREMENT_STAB_START", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                                    to
           
                                                    <asp:TextBox ID="Txt_M_Stab_end" runat="server" CssClass="txt2" Text='<%# Eval("MEASUREMENT_STAB_END", "{0:MM/dd/yyyy}") %>'></asp:TextBox>


                                                    <asp:Panel ID="PnlEditSummerWindow" runat="server" Visible="false">

                                                        <h3>Edit Breaks In Service</h3>
                                                        <label class="lbl">New Break:</label>
                                                        <asp:TextBox ID="TxtSummerBreakStart" runat="server" CssClass="txt" Width="75px"></asp:TextBox>
                                                        to                       
                                                        <asp:TextBox ID="TxtSummerBreakEnd" runat="server" CssClass="txt" Width="75px"></asp:TextBox>
                                                        <asp:Button ID="AddNewEditBreak" CssClass="btn" Text="Add" runat="server" OnClick="AddNewEditBreak_Click" />
                                                        <br />

                                                        <asp:GridView ID="Gv_BreakOfService" runat="server" AutoGenerateColumns="false"
                                                            EmptyDataText="There are currently NO breaks in service." BackColor="White" BorderColor="#336666"
                                                            BorderStyle="Double" BorderWidth="3px" CellPadding="4" GridLines="Horizontal"
                                                            OnRowDeleting="Gv_BreakOfService_RowDeleting"
                                                            OnRowEditing="Gv_BreakOfService_RowEditing"
                                                            OnRowCancelingEdit="Gv_BreakOfService_RowCancelingEdit"
                                                            OnRowUpdating="Gv_BreakOfService_RowUpdating">
                                                            <FooterStyle BackColor="White" ForeColor="#333333" />
                                                            <HeaderStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
                                                            <PagerStyle BackColor="#eb0029" ForeColor="White" HorizontalAlign="Center" />
                                                            <RowStyle BackColor="White" ForeColor="#333333" Height="30px" />
                                                            <SelectedRowStyle BackColor="#339966" Font-Bold="True" ForeColor="White" />
                                                            <SortedAscendingCellStyle BackColor="#F7F7F7" />
                                                            <SortedAscendingHeaderStyle BackColor="#eb0029" />
                                                            <SortedDescendingCellStyle BackColor="#E5E5E5" />
                                                            <SortedDescendingHeaderStyle BackColor="#eb0029" />
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="Edit">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton ID="ImgBtnEdit" runat="server" CommandName="Edit" ImageUrl="~/images/edit_notes.png" Height="25px" Style="float: right;" />
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:ImageButton ID="ImgBtnUpdate" runat="server" CommandName="Update" ImageUrl="~/images/disk-save.png" Height="25px" />
                                                                        <asp:ImageButton ID="ImgBtnCancel" runat="server" CommandName="Cancel" ImageUrl="~/images/error.png" Height="25px" />
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="Start Date" HeaderStyle-Width="225px" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:HiddenField ID="HiddenBreakId" runat="server" Value='<%# Eval("BreakInServiceId") %>' />
                                                                        <asp:Literal ID="TxtSummerBreakStart" runat="server" Text='<%# Eval("StartDate", "{0:MM/dd/yyyy}") %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:HiddenField ID="HiddenBreakId" runat="server" Value='<%# Eval("BreakInServiceId") %>' />
                                                                        <asp:TextBox ID="TxtSummerBreakStart" runat="server" Text='<%# Eval("StartDate", "{0:MM/dd/yyyy}") %>' CssClass="txt" Width="75px"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderText="End Date" HeaderStyle-Width="225px" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Literal ID="TxtSummerBreakEnd" runat="server" Text='<%# Eval("EndDate", "{0:MM/dd/yyyy}") %>'></asp:Literal>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="TxtSummerBreakEnd" runat="server" Text='<%# Eval("EndDate", "{0:MM/dd/yyyy}") %>' CssClass="txt" Width="75px"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>

                                                                <asp:TemplateField HeaderStyle-Width="25px" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:ImageButton Width="25px" ID="ImgBtnDelete" runat="server" ImageUrl="~/images/close_box_red.png" Height="20px" CommandName="Delete" ToolTip="Delete this Break In Service?" />
                                                                        <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" TargetControlID="ImgBtnDelete" ConfirmText="Are you sure you want to DELETE this Break In Service?"></asp:ConfirmButtonExtender>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                    <br />

                                                    <label class="lbl4">Notes:</label>
                                                    <asp:TextBox ID="Txt_M_Notes" runat="server" Width="275px" Height="100px" TextMode="MultiLine" Text='<%# Eval("MEASUREMENT_NOTES") %>'></asp:TextBox>
                                                    <br />
                                                    <br />
                                                    <label class="lbl4">.</label>
                                                    <asp:Button ID="BtnSaveMeasurementPeriod" runat="server" CssClass="btn" Text="Submit" CommandName="Update" />
                                                    <br />
                                                    <asp:Label ID="Lbl_M_message" runat="server" BackColor="Yellow"></asp:Label>
                                                    <br />
                                                    <br />
                                                </div>
                                            </asp:Panel>
                                            <%} %>

                                            <asp:Panel ID="PnlTransitionMeasurementHistory" runat="server">
                                                <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                                                </div>
                                                <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                                                    <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                                        <asp:ImageButton ID="ImageButton3" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" />
                                                    </div>
                                                    <h3>Measurement History</h3>
                                                    <asp:TextBox ID="TxtTransitionMeasurementHistory" runat="server" Width="575px" Height="300px" TextMode="MultiLine" Font-Size="12px" BorderStyle="None" ReadOnly="true" Text='<%# Eval("MEASUREMENT_HISTORY") %>'></asp:TextBox>
                                                </div>
                                            </asp:Panel>
                                            <asp:ModalPopupExtender ID="MpeEditTransitionPeriod" runat="server" PopupControlID="PnlEditTransitionMeasurement" TargetControlID="ImgBtnEditTransitionMeasurement"></asp:ModalPopupExtender>
                                            <asp:ModalPopupExtender ID="MpeHistoryTransitionPeriod" runat="server" PopupControlID="PnlTransitionMeasurementHistory" TargetControlID="ImgBtnHistoryTransitionMeasurement"></asp:ModalPopupExtender>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </asp:Panel>
                            </asp:Panel>


                            <asp:Panel ID="PnlEquivalency" runat="server">
                                <div style="position: absolute; top: 21px; right: 50px;">
                                    <asp:Button ID="BtnNewEquiv" CssClass="btn" runat="server" Text="New" />
                                </div>
                                <h3>Job Activity Equivalencies</h3>
                                <br />
                                * Equivalencies are applied equally to all Stability Periods.
                   
                                <asp:ModalPopupExtender ID="MpeEquivelancy" runat="server" PopupControlID="PnlStatus" TargetControlID="BtnNewEquiv"></asp:ModalPopupExtender>

                                <asp:Panel ID="PnlStatus" runat="server" DefaultButton="BtnSaveEquiv">
                                    <div style="position: fixed; top: 0; left: 0; background-color: white; opacity: 0.8; width: 100%; height: 100%;">
                                    </div>
                                    <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                                        <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                            <asp:ImageButton ID="ImgBtnClose" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" OnClick="ImgBtnClose_Click" />
                                        </div>
                                        <h3>Step 1: Choose Equivalency Trigger</h3>
                                        How is this Equivalency going to be triggered?
                       
                                        <asp:RadioButton ID="RbEquivPayroll" Text="Payroll" runat="server" GroupName="question" Checked="true" OnCheckedChanged="RbEquivPayroll_CheckedChanged" AutoPostBack="true" />
                                        <br />
                                        <asp:Panel ID="PnlgpDescription" runat="server" Visible="true">
                                            <h3>Step 2: Select Gross Pay Trigger</h3>
                                            <label class="lbl4">Gross Pay Description :</label>
                                            <asp:DropDownList ID="DdlEquivGrossPayDesc" runat="server" CssClass="ddl" Width="310px"></asp:DropDownList>
                                            <br />
                                        </asp:Panel>

                                        <asp:Panel ID="PnlgpEquiv" runat="server" Visible="true">
                                            <h3>Step 3: Enter Equivalency Details</h3>
                                            <label class="lbl4">Name :</label>
                                            <asp:TextBox ID="TxtEquivName" runat="server" CssClass="txt" Width="300px"></asp:TextBox>
                                            <br />
                                            <label class="lbl4">Active :</label>
                                            <asp:CheckBox ID="CbEquivActive" runat="server" CssClass="largerCheckbox" Checked="true" />
                                            <br />
                                            <label class="lbl4">Notes</label>
                                            <asp:TextBox ID="TxtEquivNotes" runat="server" Width="300px" TextMode="MultiLine" Height="100px"></asp:TextBox>
                                            <br />
                                            <h3>Step 4: Set the hourly equivalency</h3>
                                            <label style="width: 100px; float: left; text-align: right; line-height: 18px;">For every&nbsp;</label>
                                            <asp:TextBox ID="TxtEquivUnits2" runat="server" Width="50px"></asp:TextBox>
                                            &nbsp;&nbsp;
                   
                                            <asp:DropDownList ID="DdlEquivUnit" runat="server"></asp:DropDownList>
                                            worked, credit
                   
                                            <asp:TextBox ID="TxtEquivUnits" runat="server" Width="50px"></asp:TextBox>
                                            hours
                   
                                            <br />
                                            <br />
                                            <label style="width: 100px; float: left; text-align: right; line-height: 18px;">.</label>
                                            <asp:Button ID="BtnSaveEquiv" CssClass="btn" runat="server" Text="Submit" OnClick="BtnSaveEquiv_Click" />
                                            <br />
                                            <asp:Label ID="LblEquivError" runat="server" BackColor="Yellow"></asp:Label>
                                            <br />
                                            <br />
                                        </asp:Panel>
                                    </div>
                                </asp:Panel>
                                <br />
                                <i>You are viewing page
           
                                    <%=GvEquivlancies.PageIndex + 1%>
            of
           
                                    <%=GvEquivlancies.PageCount%>
                                </i>
                                <br />
                                <i>Showing 
           
                                    <asp:Literal ID="LitEquivShow" runat="server"></asp:Literal>
                                    Equivalencies of 
           
                                    <asp:Literal ID="LitEquivTotal" runat="server"></asp:Literal>
                                </i>
                                <asp:GridView ID="GvEquivlancies" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" CellPadding="0" ForeColor="#333333" GridLines="None" Width="530px" Font-Size="13px" OnRowDataBound="GvEquivlancies_RowDataBound" AllowPaging="True" PageSize="8" OnPageIndexChanging="GvEquivlancies_PageIndexChanging" OnRowEditing="GvEquivlancies_RowEditing" OnRowUpdating="GvEquivlancies_RowUpdating" OnRowDeleting="GvEquivlancies_RowDeleting">
                                    <AlternatingRowStyle BackColor="White" />
                                    <EditRowStyle BackColor="#eb0029" />
                                    <FooterStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#eb0029" Font-Bold="True" ForeColor="black" />
                                    <PagerStyle BackColor="#eb0029" HorizontalAlign="Center" />
                                    <PagerSettings FirstPageImageUrl="~/design/first.png" Mode="NextPreviousFirstLast" LastPageImageUrl="/design/last.png" NextPageImageUrl="/design/next.png" PreviousPageImageUrl="/design/prev.png" PageButtonCount="25" Position="TopAndBottom" />
                                    <RowStyle BackColor="#EFF3FB" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                    <SortedAscendingHeaderStyle BackColor="#eb0029" />
                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                    <SortedDescendingHeaderStyle BackColor="#eb0029" />
                                    <EmptyDataTemplate>
                                        <asp:Label ID="LblEmptyDataSource" runat="server" Text="There are currently no equivalencies."></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgBtnEquivEdit" runat="server" ImageUrl="~/images/edit_notes.png" Height="30px" ToolTip="Edit Equivalency" CommandName="Edit" />
                                                <asp:ImageButton ID="ImgBtnEquivDelete" runat="server" ImageUrl="~/images/close_box_red.png" Height="25px" ToolTip="Delete Equivalency" CommandName="Delete" />
                                                <asp:ConfirmButtonExtender ID="ConfirmButtonExtender6" runat="server" TargetControlID="ImgBtnEquivDelete" ConfirmText="Are you sure you want to DELETE this Equivalency?"></asp:ConfirmButtonExtender>
                                                <asp:HiddenField ID="HvEquivID" runat="server" Value='<%# Eval("EQUIV_ID") %>' />
                                                <asp:HiddenField ID="HfEquivGpID" runat="server" Value='<%# Eval("EQUIV_GROSS_PAY_ID") %>' />
                                                <asp:HiddenField ID="HfEquivUnitID" runat="server" Value='<%# Eval("EQUIV_UNIT_ID") %>' />
                                                <asp:HiddenField ID="HfEquivTypeID" runat="server" Value='<%# Eval("EQUIV_TYPE_ID") %>' />
                                                <asp:HiddenField ID="HfEBCPosID" runat="server" Value='<%# Eval("EQUIV_POSITION_ID") %>' />
                                                <asp:HiddenField ID="HfEBCActID" runat="server" Value='<%# Eval("EQUIV_ACTIVITY_ID") %>' />
                                                <asp:HiddenField ID="HfEBCDetID" runat="server" Value='<%# Eval("EQUIV_DETAIL_ID") %>' />
                                                <asp:ModalPopupExtender ID="MpeEquivEdit" runat="server" PopupControlID="PnlEditEquiv" TargetControlID="HvEquivID"></asp:ModalPopupExtender>
                                                <asp:Panel ID="PnlEditEquiv" runat="server" DefaultButton="BtnEquivalencyUpdate_gv">
                                                    <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                                                    </div>
                                                    <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                                                        <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                                            <asp:ImageButton ID="ImgBtnEquivEditCancel" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" OnClick="ImgBtnEquivEditCancel_Click" />
                                                        </div>
                                                        <h3>Step 1: Choose Equivalency Trigger</h3>
                                                        How is this Equivalency going to be triggered?
                   
                                                        <asp:RadioButton ID="RbEquivPayroll_gv" Text="Payroll" runat="server" GroupName="question" Checked="true" AutoPostBack="true" OnCheckedChanged="RbEquivPayroll_gv_CheckedChanged" />
                                                        <br />
                                                        <asp:Panel ID="PnlgpDescription_gv" runat="server" Visible="true">
                                                            <h3>Step 2: Select Gross Pay Trigger</h3>
                                                            <label class="lbl4">Gross Pay Description :</label>
                                                            <asp:DropDownList ID="DdlEquivGrossPayDesc_gv" runat="server" CssClass="ddl" Width="310px"></asp:DropDownList>
                                                            <br />
                                                        </asp:Panel>

                                                        <h3>Step 3: Enter Equivalency Details</h3>
                                                        <label class="lbl4">Name :</label>
                                                        <asp:TextBox ID="TxtEquivName_gv" runat="server" CssClass="txt" Width="300px" Text='<%# Eval("EQUIV_NAME") %>'></asp:TextBox>
                                                        <br />
                                                        <label class="lbl4">Active :</label>
                                                        <asp:CheckBox ID="CbEquivActive_gv" runat="server" CssClass="largerCheckbox" Checked='<%# Eval("EQUIV_ACTIVE") %>' />
                                                        <br />
                                                        <label class="lbl4">Notes</label>
                                                        <asp:TextBox ID="TxtEquivNotes_gv" runat="server" Width="300px" TextMode="MultiLine" Height="100px" Text='<%# Eval("EQUIV_NOTES") %>'></asp:TextBox>
                                                        <br />
                                                        <h3>Step 5: Set the hourly equivalency</h3>
                                                        <label style="width: 100px; float: left; text-align: right; line-height: 18px;">For every&nbsp;</label>
                                                        <asp:TextBox ID="TxtEquivUnits2_gv" runat="server" Width="50px" Text='<%# Eval("EQUIV_EVERY") %>'></asp:TextBox>
                                                        &nbsp;&nbsp;
               
                                                        <asp:DropDownList ID="DdlEquivUnit_gv" runat="server"></asp:DropDownList>
                                                        worked, credit
               
                                                        <asp:TextBox ID="TxtEquivUnits_gv" runat="server" Width="50px" Text='<%# Eval("EQUIV_CREDIT") %>'></asp:TextBox>
                                                        hours
               
                                                        <br />
                                                        <br />

                                                        <label class="lbl3" style="background-color: white; color: white;">.</label>
                                                        <asp:Button ID="BtnEquivalencyUpdate_gv" CssClass="btn" runat="server" Text="Submit" CommandName="Update" />
                                                        <br />
                                                        <br />
                                                        <br />
                                                        <label class="lbl3">Message</label>
                                                        <asp:Label ID="LblEquivalencyUpdateMessage_gv" runat="server" ForeColor="Red" Font-Bold="true" Height="20px" Style="line-height: 20px"></asp:Label>
                                                        <br />
                                                        <br />
                                                        Last Modified by:&nbsp;&nbsp;
               
                                                        <asp:Literal ID="LiteqModBy" runat="server" Text='<%# Eval("EQUIV_MOD_BY") %>'></asp:Literal>
                                                        &nbsp;&nbsp;on&nbsp;&nbsp; 
               
                                                        <asp:Literal ID="LiteqModOn" runat="server" Text='<%# Eval("EQUIV_MOD_ON") %>'></asp:Literal>
                                                    </div>
                                                </asp:Panel>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Job Activity" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="LblEmployee" runat="server" Text='<%# Eval("EQUIV_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Activity Code" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="LblGvID" runat="server" Text='<%# Eval("EQUIV_GROSS_PAY_ID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Trigger" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="LblGvTypeID" runat="server" Text='<%# Eval("EQUIV_TYPE_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="LblGvDescription" runat="server" Text='<%# Eval("EQUIV_COMBINED") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>

                            <asp:Panel ID="PnlUsers" runat="server">
                                <div style="position: absolute; top: 22px; right: 85px;">
                                    <asp:Button ID="BtnNewUser" CssClass="btn" runat="server" Text="New" />
                                </div>
                                <h3>Users</h3>

                                <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" PopupControlID="PnlNewUser" TargetControlID="BtnNewUser"></asp:ModalPopupExtender>

                                <asp:Panel ID="PnlNewUser" runat="server" DefaultButton="BtnSaveNewUser">
                                    <div style="position: fixed; top: 0; left: 0; background-color: gray; opacity: 0.8; width: 100%; height: 100%;">
                                    </div>
                                    <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 800px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                                        <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                            <asp:ImageButton ID="ImageButton1" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" />
                                        </div>
                                        <h3>Create New User</h3>
                                        <label class="lbl3">First Name:</label>
                                        <asp:TextBox ID="TxtNewFName" runat="server" CssClass="txtLong"></asp:TextBox>
                                        <br />
                                        <label class="lbl3">Last Name:</label>
                                        <asp:TextBox ID="TxtNewLName" runat="server" CssClass="txtLong"></asp:TextBox>
                                        <br />
                                        <label class="lbl3">Email:</label>
                                        <asp:TextBox ID="TxtNewEmail" runat="server" CssClass="txtLong"></asp:TextBox>
                                        <br />
                                        <label class="lbl3">Phone:</label>
                                        <asp:TextBox ID="TxtNewPhone" runat="server" CssClass="txtLong"></asp:TextBox>
                                        <br />
                                        <label class="lbl3">UserName:</label>
                                        <asp:TextBox ID="TxtNewUserName" runat="server" CssClass="txtLong"></asp:TextBox>
                                        <br />
                                        <label class="lbl3">Password:</label>
                                        <asp:TextBox ID="TxtNewPassword" runat="server" TextMode="Password" CssClass="txtLong"></asp:TextBox>

                                        <asp:PasswordStrength ID="PasswordStrength1" runat="server" Enabled="true" TargetControlID="TxtNewPassword" DisplayPosition="RightSide"
                                            MinimumNumericCharacters="1"
                                            MinimumSymbolCharacters="1" HelpStatusLabelID="LblPassword" BarBorderCssClass="border"
                                            MinimumLowerCaseCharacters="1"
                                            MinimumUpperCaseCharacters="1" PreferredPasswordLength="6" CalculationWeightings="25;25;15;35"
                                            StrengthIndicatorType="BarIndicator" TextStrengthDescriptions="Very Poor; Weak; Average; Strong; Excellent" StrengthStyles="VeryPoor; Weak; Average; Strong; Excellent">
                                        </asp:PasswordStrength>
                                        <asp:Label ID="LblPassword" runat="server" Font-Size="10px"></asp:Label>
                                        <br />
                                        <label class="lbl3">Re-enter Password:</label>
                                        <asp:TextBox ID="TxtNewPassword2" runat="server" TextMode="Password" CssClass="txtLong"></asp:TextBox>
                                        <br />
                                        <br />
                                        <label class="lbl3" style="background-color: white; color: white;">.</label>
                                        <asp:Button ID="BtnSaveNewUser" CssClass="btn" runat="server" Text="Submit" OnClick="BtnSaveNewUser_Click" />
                                        <br />
                                        <br />
                                        <br />
                                        <label class="lbl3">Message</label>
                                        <asp:Label ID="LblNewUserMessage" runat="server" ForeColor="Red" Font-Bold="true" Height="20px" Style="line-height: 20px"></asp:Label>
                                        <br />
                                        <br />
                                        .
                   
                                    </div>
                                </asp:Panel>
                                <br />
                                <i>You are viewing page
                   
                                    <%=GvDistrictUsers.PageIndex + 1%>
                    of
                   
                                    <%=GvDistrictUsers.PageCount%>
                                </i>
                                <br />
                                <i>Showing 
                   
                                    <asp:Literal ID="LitUserShow" runat="server"></asp:Literal>
                                    Users of 
                   
                                    <asp:Literal ID="LitUserTotal" runat="server"></asp:Literal>
                                </i>

                                <asp:GridView ID="GvDistrictUsers" runat="server" CssClass="gridviewHeader" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" CellPadding="0" ForeColor="#333333" AllowPaging="true" PageSize="15" EmptyDataText="There are currently no alerts." GridLines="None" Width="765px" Font-Size="14px" OnRowDeleting="GvDistrictUsers_RowDeleting" OnRowUpdating="GvDistrictUsers_RowUpdating" OnPageIndexChanging="GvDistrictUsers_PageIndexChanging">
                                    <AlternatingRowStyle BackColor="White" />
                                    <EditRowStyle BackColor="yellow" />
                                    <FooterStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#eb0029" Font-Bold="True" ForeColor="black" />
                                    <PagerStyle BackColor="#eb0029" HorizontalAlign="Center" />
                                    <PagerSettings FirstPageImageUrl="~/design/first.png" Mode="NextPreviousFirstLast" LastPageImageUrl="/design/last.png" NextPageImageUrl="/design/next.png" PreviousPageImageUrl="/design/prev.png" PageButtonCount="25" Position="TopAndBottom" />
                                    <RowStyle BackColor="#EFF3FB" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                    <SortedAscendingHeaderStyle BackColor="#eb0029" />
                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                    <SortedDescendingHeaderStyle BackColor="#eb0029" />
                                    <EmptyDataTemplate>
                                        <asp:Label ID="LblEmptyDataSource" runat="server" Text="There are currently no users."></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgBtnEdit" runat="server" ImageUrl="~/images/edit_notes.png" Height="30px" ToolTip="Edit User" />
                                                <asp:ModalPopupExtender ID="ModalPopupExtender3" runat="server" PopupControlID="PnlEditUser" TargetControlID="ImgBtnEdit"></asp:ModalPopupExtender>
                                                <asp:Panel ID="PnlEditUser" runat="server" DefaultButton="BtnUserUpdate">
                                                    <div style="position: fixed; top: 0; left: 0; background-color: gray; opacity: 0.8; width: 100%; height: 100%;">
                                                    </div>
                                                    <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                                                        <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                                            <asp:ImageButton ID="ImageButton1" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" />
                                                        </div>
                                                        <h3>Edit User</h3>
                                                        <asp:HiddenField ID="HfUserID" runat="server" Value='<%# Eval("User_ID") %>' />
                                                        <asp:HiddenField ID="HfUserName" runat="server" Value='<%# Eval("User_UserName") %>' />
                                                        <label class="lbl3">First Name</label>
                                                        <asp:TextBox ID="TxtmpFName" runat="server" CssClass="txtLong" Text='<%# Eval("User_First_Name") %>'></asp:TextBox>
                                                        <br />
                                                        <label class="lbl3">Last Name</label>
                                                        <asp:TextBox ID="TxtmpLName" runat="server" CssClass="txtLong" Text='<%# Eval("User_Last_Name") %>'></asp:TextBox>
                                                        <br />
                                                        <label class="lbl3">Email</label>
                                                        <asp:TextBox ID="TxtmpEmail" runat="server" CssClass="txtLong" Text='<%# Eval("User_Email") %>'></asp:TextBox>
                                                        <br />
                                                        <label class="lbl3">Phone</label>
                                                        <asp:TextBox ID="TxtmpPhone" runat="server" CssClass="txtLong" Text='<%# Eval("User_Phone") %>'></asp:TextBox>
                                                        <br />
                                                        <label class="lbl3">Data Contact</label>
                                                        <asp:CheckBox ID="CbtmpPowerUser" runat="server" Checked='<%# Eval("User_Power") %>' />
                                                        <br style="clear: both;" />
                                                        <p>
                                                            * Note: Only a Power User can add/edit records. 
                           
                                                        </p>
                                                        <label class="lbl3">Billing Contact</label>
                                                        <asp:CheckBox ID="CbtmpBillingUser" runat="server" Checked='<%# Eval("User_Billing") %>' />
                                                        <br />
                                                        <p>
                                                            * Note: Check the Billing for each user that will be involved in handling the Invoices. 
                           
                                                        </p>
                                                        <br />
                                                        <label class="lbl3">IRS Contact</label>
                                                        <asp:CheckBox ID="CbtmpIRSContact" runat="server" Checked='<%# Eval("User_IRS_CONTACT") %>' />
                                                        <br />
                                                        <asp:ModalPopupExtender ID="ModalPopupExtenderIRSContactChange" runat="server" CancelControlID="btnCancelIRSContactChange" PopupControlID="pnlIRSContactChange" TargetControlID="CbtmpIRSContact"></asp:ModalPopupExtender>
                                                        <asp:Panel ID="pnlIRSContactChange" runat="server" DefaultButton="btnYesIRSContactChange" visable="false">
                                                            <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                                                            </div>
                                                            <div style="position: relative; margin-left: auto; padding: 10px; margin-right: auto; border-radius: 20px; width: 300px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                                                                <div style="position: absolute; width: 15px; height: 15px; opacity: 1.0; line-height: 30px; right: -10px; top: -10px; text-align: center;">
                                                                    <asp:ImageButton ID="ImageButton7" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" />
                                                                </div>
                                                                <p>
                                                                    Only one IRS contact may be selected. Are you sure you want to change permission of this contact?                            
                                                                </p>
                                                                <asp:Button ID="btnYesIRSContactChange" CssClass="btn" runat="server" Text="Yes" OnClick="btnYesIRSContactChange_Click" />
                                                                <asp:Button ID="btnCancelIRSContactChange" CssClass="btn" runat="server" Text="Cancel" />
                                                            </div>
                                                        </asp:Panel>
                                                        <p>
                                                            * Note: If checked, this name will appear as the contact on the IRS reporting. 
                           
                                                        </p>
                                                        <br />
                                                        <label class="lbl3" style="background-color: white; color: white;">.</label>
                                                        <asp:Button ID="BtnUserUpdate" CssClass="btn" runat="server" Text="Submit" CommandName="Update" />
                                                        &nbsp;&nbsp;
                           
                                                        <asp:Button ID="BtnUserDelete" CssClass="btn" runat="server" Text="Delete" CommandName="Delete" />
                                                        <asp:ConfirmButtonExtender ID="ConfirmButtonExtender1" runat="server" ConfirmText="Are you sure you want to remove this user?" TargetControlID="BtnUserDelete"></asp:ConfirmButtonExtender>
                                                        <br />
                                                        <br />
                                                        <br />
                                                        <label class="lbl3">Message</label>
                                                        <asp:Label ID="LblUserUpdateMessage" runat="server" ForeColor="Red" Font-Bold="true" Height="20px" Style="line-height: 20px"></asp:Label>
                                                        <br />
                                                        <br />
                                                        Last Modified by:&nbsp;&nbsp;
                           
                                                        <asp:Literal ID="LitModBy" runat="server" Text='<%# Eval("LAST_MOD_BY") %>'></asp:Literal>
                                                        &nbsp;&nbsp;on&nbsp;&nbsp; 
                           
                                                        <asp:Literal ID="LitModOn" runat="server" Text='<%# Eval("LAST_MOD") %>'></asp:Literal>
                                                    </div>
                                                </asp:Panel>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Username" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="LblGvUsername" runat="server" Text='<%# Eval("User_UserName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="First Name" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="LblGvFirstName" runat="server" Text='<%# Eval("User_First_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Last Name" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="LblGvLastName" runat="server" Text='<%# Eval("User_Last_Name") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Email" HeaderStyle-Width="175px" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="LblGvEmail" runat="server" Text='<%# Eval("User_Email") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Data" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CbPowerUser" runat="server" Checked='<%# Eval("User_Power") %>' Enabled="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bill" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CbBillingUser" runat="server" Checked='<%# Eval("User_Billing") %>' Enabled="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="IRS" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CbIrsContact" runat="server" Checked='<%# Eval("User_IRS_CONTACT") %>' Enabled="false" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>

                            <asp:Panel ID="PnlDistrictProfile" runat="server">
                                <div style="position: absolute; top: 22px; right: 62px;">
                                    <asp:Button ID="BtnSaveProfile" runat="server" CssClass="btn" Text="Update" OnClick="BtnSaveProfile_Click" />
                                </div>
                                <div style="float: left; min-height: 400px; width: 450px;">

                                    <h3>Profile</h3>
                                    <p>
                                        <label class="lbl">EIN:</label>
                                        <asp:TextBox ID="TxtEmployerEIN" runat="server" CssClass="txt3"></asp:TextBox>

                                    </p>
                                    <p>
                                        <label class="lbl">Legal Name:</label>
                                        <asp:TextBox ID="TxtEmployerIrsName" runat="server" CssClass="txt3"></asp:TextBox>
                                    </p>
                                    <p>
                                        <label class="lbl">DBA Name:</label>
                                        <asp:TextBox ID="TxtEmployerDbaName" runat="server" CssClass="txt3"></asp:TextBox>
                                    </p>
                                    <p>
                                        <label class="lbl">Address:</label>
                                        <asp:TextBox ID="TxtEmployerAddress" runat="server" CssClass="txt3"></asp:TextBox>
                                    </p>
                                    <p>
                                        <label class="lbl">City:</label>
                                        <asp:TextBox ID="TxtEmployerCity" runat="server" CssClass="txt3"></asp:TextBox>
                                    </p>
                                    <p>
                                        <label class="lbl">State:</label>
                                        <asp:DropDownList ID="DdlEmployerState" runat="server" CssClass="ddl2"></asp:DropDownList>
                                    </p>
                                    <p>

                                        <label class="lbl">Zip</label>
                                        <asp:TextBox ID="TxtEmployerZip" runat="server" CssClass="txt3"></asp:TextBox>
                                    </p>
                                </div>
                            </asp:Panel>

                            <asp:Panel ID="PnlAlerts" runat="server">
                                <h3>Alert Management</h3>
                                <asp:GridView ID="GvAlerts" runat="server" CssClass="gridviewHeader" AutoGenerateColumns="false" EmptyDataText="Contact <%= Branding.CompanyShortName %> to setup your ALERTS." CellPadding="4" ForeColor="#333333" GridLines="None" Width="512px" Font-Size="15px" AllowPaging="true" PageSize="20" OnPageIndexChanging="GvAlerts_PageIndexChanging">
                                    <AlternatingRowStyle BackColor="White" />
                                    <EditRowStyle BackColor="#eb0029" />
                                    <FooterStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#eb0029" Font-Bold="True" ForeColor="black" />
                                    <PagerStyle BackColor="#eb0029" HorizontalAlign="Center" />
                                    <PagerSettings FirstPageImageUrl="~/design/first.png" Mode="NextPreviousFirstLast" LastPageImageUrl="/design/last.png" NextPageImageUrl="/design/next.png" PreviousPageImageUrl="/design/prev.png" PageButtonCount="25" Position="TopAndBottom" />
                                    <RowStyle BackColor="#EFF3FB" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                    <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                    <SortedDescendingHeaderStyle BackColor="#4870BE" />
                                    <EmptyDataTemplate>
                                        <br />
                                        <br />
                                        <asp:Label ID="LblEmptyDataSource" runat="server" Text="No Alerts have been created for your account yet."></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderText="Alert Name" HeaderStyle-Width="225px" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="LitAlertName" runat="server" Text='<%# Eval("ALERT_TYPE_NAME") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Alert Count" HeaderStyle-Width="225px" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="LitAlertCount" runat="server" Text='<%# Eval("ALERT_COUNT") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>

                            <asp:Panel ID="PnlInsurance" runat="server" Visible="false">
                                <div style="position: absolute; top: 15px; right: 85px;">
                                    <asp:Button ID="BtnNewInsurance" CssClass="btn" runat="server" Text="New" OnClick="BtnNewInsurance_Click" />
                                </div>
                                <asp:Panel ID="Pnl_I_Insurance" runat="server" DefaultButton="Btn_I_Update">
                                    <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                                    </div>
                                    <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                                        <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                            <asp:ImageButton ID="ImgBtn_epy_Cancel" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" OnClick="ImgBtnEquivEditCancel_Click" />
                                        </div>
                                        <h3>
                                            <asp:Literal ID="Lit_I_function" runat="server"></asp:Literal>
                                            - Medical Plan</h3>
                                        <asp:HiddenField ID="Hf_I_id" runat="server" />
                                        <label class="lbl3">Name</label>
                                        <asp:TextBox ID="Txt_I_Name" runat="server" CssClass="txt3"></asp:TextBox>
                                        <br />
                                        <label class="lbl3">Select Stability Period</label>
                                        <asp:DropDownList ID="Ddl_I_PlanYear" runat="server" CssClass="ddl" Width="310px"></asp:DropDownList>
                                        <br />
                                        <label class="lbl3">Select Insurance Type</label>
                                        <asp:DropDownList ID="Ddl_I_Type" runat="server" CssClass="ddl" Width="310px" AutoPostBack="true" OnSelectedIndexChanged="Ddl_I_Type_SelectedIndexChanged"></asp:DropDownList>
                                        <br />
                                        <asp:Panel ID="Pnl_I_FullyPlusSelfInsured" runat="server" Visible="false">
                                            Do you offer Self-Insured plans as well?
                                            <br />
                                            <asp:DropDownList ID="Ddl_I_SelfPlusFullyInsured" runat="server" CssClass="ddl">
                                                <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="false"></asp:ListItem>
                                                <asp:ListItem Text="Select" Selected="True"></asp:ListItem>
                                            </asp:DropDownList>
                                        </asp:Panel>
                                        <br />
                                        <h3>Monthly Premium</h3>
                                        * The TOTAL MONTHLY PREMIUM is based on <b>12 calendar months</b>
                                        <br />
                                        <label class="lbl3">Total Monthly Premium</label>
                                        <asp:TextBox ID="Txt_I_Cost" runat="server" CssClass="txt3"></asp:TextBox>*Single Coverage for Lowest Cost Medical Plan Only
                                        <br />
                                        <br />
                                        Does this plan meet MEC requirements?
                                        <br />
                                        <asp:DropDownList ID="Ddl_I_mec" runat="server" CssClass="ddl">
                                            <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="false"></asp:ListItem>
                                            <asp:ListItem Text="Select" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                        <br />
                                        <br />
                                        Does this plan meet minimum value?
                       
                                        <br />
                                        <asp:DropDownList ID="Ddl_I_MinValue" runat="server" CssClass="ddl">
                                            <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="false"></asp:ListItem>
                                            <asp:ListItem Text="Select" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                        <br />
                                        <br />
                                        Is the plan offered to spouses?
                   
                                        <br />
                                        <asp:DropDownList ID="Ddl_I_Spouse" runat="server" CssClass="ddl">
                                            <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                                            <asp:ListItem Text="Conditional Offer" Value="Conditionally"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="false"></asp:ListItem>
                                            <asp:ListItem Text="Select" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                        <br />
                                        <br />
                                        Is the plan offered to dependents?
               
                                        <br />
                                        <asp:DropDownList ID="Ddl_I_Dependant" runat="server" CssClass="ddl">
                                            <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                                            <asp:ListItem Text="No" Value="false"></asp:ListItem>
                                            <asp:ListItem Text="Select" Selected="True"></asp:ListItem>
                                        </asp:DropDownList>
                                        <br />
                                        <br />
                                        <br />
                                        <label class="lbl3" style="background-color: white; color: white;">.</label>
                                        <asp:Button ID="Btn_I_Update" CssClass="btn" runat="server" Text="Submit" OnClick="Btn_I_Update_Click" />
                                        <asp:ConfirmButtonExtender ID="ConfirmButtonExtender2" runat="server" TargetControlID="Btn_I_Update" ConfirmText="Was the TOTAL MONTHLY PREMIUM based on 12 calendar months?"></asp:ConfirmButtonExtender>
                                        <br />
                                        <br />
                                        <label class="lbl3">Message</label>
                                        <asp:Label ID="Lbl_I_Message" runat="server" ForeColor="Red" Font-Bold="true" Height="20px" Style="line-height: 20px"></asp:Label>
                                        <br />
                                        <br />
                                    </div>
                                </asp:Panel>
                                <asp:HiddenField ID="HfIDummy" runat="server" />
                                <asp:ModalPopupExtender ID="Mpe_I_insurance" runat="server" TargetControlID="HfIDummy" PopupControlID="Pnl_I_Insurance"></asp:ModalPopupExtender>
                                <h3>Medical Plan</h3>
                                *The lowest cost qualified medical plan is specific to each Stability Period. If you have multiple Stability Periods, Please use the drop down list in the upper right hand corner to view the medical plans(s) for a different Stability Period.
    
   

                                <p>
                                    Enter the monthly premium for single coverage in your lowest cost “qualified” employer provided plan.Qualified plans must meet or exceed the 60% minimum value calculation required under the Affordable Care Act.
   
                                </p>
                                <i>You are viewing page
       
                                    <%=GvInsurance.PageIndex + 1%>
        of
       
                                    <%=GvInsurance.PageCount%>
                                </i>
                                <br />
                                <i>Showing 
       
                                    <asp:Literal ID="LitInsuranceDisplay" runat="server"></asp:Literal>
                                    Insurance Plans of 
       
                                    <asp:Literal ID="LitInsuranceCount" runat="server"></asp:Literal>
                                </i>
                                <asp:GridView ID="GvInsurance" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" CssClass="gridviewHeader" CellPadding="0" ForeColor="#333333" EmptyDataText="There are currently no medical plans." GridLines="None" Width="514px" Font-Size="15px" OnRowEditing="GvInsurance_RowEditing" OnRowDeleting="GvInsurance_RowDeleting" OnSelectedIndexChanged="GvInsurance_SelectedIndexChanged" OnPageIndexChanging="GvInsurance_PageIndexChanging" AllowPaging="true" PageSize="15">
                                    <AlternatingRowStyle BackColor="White" />
                                    <EditRowStyle BackColor="white" />
                                    <FooterStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#eb0029" Font-Bold="True" ForeColor="black" />
                                    <PagerStyle BackColor="#eb0029" HorizontalAlign="Center" />
                                    <PagerSettings FirstPageImageUrl="~/design/first.png" Mode="NextPreviousFirstLast" LastPageImageUrl="/design/last.png" NextPageImageUrl="/design/next.png" PreviousPageImageUrl="/design/prev.png" PageButtonCount="25" Position="TopAndBottom" />
                                    <RowStyle BackColor="#EFF3FB" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                    <SortedAscendingHeaderStyle BackColor="#eb0029" />
                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                    <SortedDescendingHeaderStyle BackColor="#eb0029" />
                                    <EmptyDataTemplate>
                                        <br />
                                        <br />
                                        <asp:Label ID="LblEmptyDataSource" runat="server" Text="There are currently no Lowest Cost medical plans for the selected Stability Period."></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgBtnInsuranceDelete" runat="server" ImageUrl="~/images/close_box_red.png" Height="25px" CommandName="Delete" ToolTip="Delete Record" />
                                                <asp:ImageButton ID="ImgBtnInsuranceEdit" runat="server" ImageUrl="~/images/edit_notes.png" Height="25px" CommandName="Edit" ToolTip="Edit Record" />
                                                <asp:ImageButton ID="ImgBtnInsuranceHistory" runat="server" ImageUrl="~/images/history.png" Height="25px" CommandName="Select" ToolTip="View Record History" />
                                                <asp:ConfirmButtonExtender ID="ConfirmButtonExtender4" runat="server" TargetControlID="ImgBtnInsuranceDelete" ConfirmText="Are you sure you want to DELETE this record?"></asp:ConfirmButtonExtender>
                                                <asp:HiddenField ID="HfDummyTrigger2" runat="server" />
                                                <asp:HiddenField ID="HfInsuranceID" runat="server" Value='<%# Eval("INSURANCE_ID") %>' />
                                                <asp:Panel ID="Pnl_h_insurance" runat="server">
                                                    <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                                                    </div>
                                                    <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                                                        <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                                            <asp:ImageButton ID="ImageButton3" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" />
                                                        </div>
                                                        <h3>Stability Period History</h3>
                                                        <asp:TextBox ID="Txt_h_insurance" runat="server" Width="575px" Height="300px" TextMode="MultiLine" Font-Size="12px" BorderStyle="None" ReadOnly="true" Text='<%# Eval("INSURANCE_HISTORY") %>'></asp:TextBox>
                                                    </div>
                                                </asp:Panel>
                                                <asp:ModalPopupExtender ID="Mpe_h_insurance" runat="server" TargetControlID="HfDummyTrigger2" PopupControlID="Pnl_h_insurance"></asp:ModalPopupExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description" HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="LblInsuranceName" runat="server" Text='<%# Eval("INSURANCE_NAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Monthly Cost" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="LblInsuranceCost" runat="server" Text='<%# Eval("INSURANCE_COST", "{0:C}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>

                            <asp:Panel ID="PnlInsuranceContribution" runat="server" Visible="false">
                                <div style="position: absolute; top: 15px; right: 85px;">
                                    <asp:Button ID="Btn_nec_InsuranceContribution" CssClass="btn" runat="server" Text="New" />
                                </div>
                                <h3>Medical Plan Contributions</h3>
                                <h5><i>Please Enter the monthly employer contribution amount for any employee classes made an offer of coverage</i></h5>
                                Select medical plan:
                                <asp:DropDownList ID="Ddl_ic_InsurancePlans" runat="server" CssClass="ddl2" AutoPostBack="true" OnSelectedIndexChanged="Ddl_ic_InsurancePlans_SelectedIndexChanged"></asp:DropDownList>
                                <br />
                                <br />
                                <i>You are viewing page
                       
                                    <%=Gv_employerContributions.PageIndex + 1%>
                        of
                       
                                    <%=Gv_employerContributions.PageCount%>
                                </i>
                                <br />
                                <i>Showing 
                       
                                    <asp:Literal ID="LitInsContShowing" runat="server"></asp:Literal>
                                    Pay Description Filters of 
                       
                                    <asp:Literal ID="LitInsContTotal" runat="server"></asp:Literal>
                                </i>
                                <asp:GridView ID="Gv_employerContributions" runat="server" AutoGenerateColumns="false" CssClass="gridviewHeader" CellPadding="0" ForeColor="#333333" GridLines="None" Width="500px" Font-Size="15px" OnRowDeleting="Gv_employerContributions_RowDeleting" OnRowEditing="Gv_employerContributions_RowEditing" OnRowUpdating="Gv_employerContributions_RowUpdating" OnPageIndexChanging="Gv_employerContributions_PageIndexChanging" AllowPaging="true" PageSize="17">
                                    <AlternatingRowStyle BackColor="White" />
                                    <EditRowStyle BackColor="white" />
                                    <FooterStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#eb0029" Font-Bold="True" ForeColor="black" />
                                    <PagerStyle BackColor="#eb0029" HorizontalAlign="Center" />
                                    <PagerSettings FirstPageImageUrl="~/design/first.png" Mode="NextPreviousFirstLast" LastPageImageUrl="/design/last.png" NextPageImageUrl="/design/next.png" PreviousPageImageUrl="/design/prev.png" PageButtonCount="25" Position="TopAndBottom" />
                                    <RowStyle BackColor="#EFF3FB" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                    <SortedAscendingHeaderStyle BackColor="#eb0029" />
                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                    <SortedDescendingHeaderStyle BackColor="#eb0029" />
                                    <EmptyDataTemplate>
                                        <br />
                                        <br />
                                        <asp:Label ID="LblEmptyDataSource" runat="server" Text="There are currently no Contributions set-up for the current medical plan."></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="75px">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgBtnContributionDelete" runat="server" ImageUrl="~/images/close_box_red.png" Height="25px" CommandName="Delete" ToolTip="Delete Record" />
                                                <asp:ConfirmButtonExtender ID="ConfirmButtonExtender5" runat="server" TargetControlID="ImgBtnContributionDelete" ConfirmText="Are you sure you want to DELETE this Contribution?"></asp:ConfirmButtonExtender>
                                                <asp:ImageButton ID="ImgBtnContributionEdit" runat="server" ImageUrl="~/images/edit_notes.png" Height="25px" CommandName="Edit" ToolTip="Edit Record" />
                                                <asp:ImageButton ID="ImgBtnContributionHistory" runat="server" ImageUrl="~/images/history.png" Height="25px" CommandName="Select" ToolTip="View Record History" />
                                                <asp:HiddenField ID="HfContributionID" runat="server" Value='<%# Eval("INS_CONT_ID") %>' />
                                                <asp:HiddenField ID="HfDummyTrigger22" runat="server" />
                                                <asp:Panel ID="Pnl_e_contribution" runat="server" DefaultButton="Btn_e_saveContribution">
                                                    <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                                                    </div>
                                                    <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                                                        <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                                            <asp:ImageButton ID="ImageButton9" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" />
                                                        </div>
                                                        <h3>Edit Employer Contribution</h3>
                                                        <label class="lbl3">Lowest Cost Medical Plan Option</label>
                                                        <asp:DropDownList ID="Ddl_e_InsurancePlans" runat="server" CssClass="ddl2"></asp:DropDownList>
                                                        <br />
                                                        <br />
                                                        <label class="lbl3">Contribution Method</label>
                                                        <asp:DropDownList ID="Ddl_e_ContributionMethod" runat="server" CssClass="ddl2"></asp:DropDownList>
                                                        <br />
                                                        <br />
                                                        <label class="lbl3">Employee Class</label>
                                                        <asp:DropDownList ID="Ddl_e_EmployeeClass" runat="server" CssClass="ddl2"></asp:DropDownList>
                                                        <br />
                                                        <br />
                                                        <label class="lbl3">Contribution Amount</label>
                                                        <asp:TextBox ID="Txt_e_contribution" runat="server" CssClass="txt3"></asp:TextBox>
                                                        <br />
                                                        <br />
                                                        <br />
                                                        <br />
                                                        <label class="lbl3" style="background-color: white; color: white;">.</label>
                                                        <asp:Button ID="Btn_e_saveContribution" CssClass="btn" runat="server" Text="Submit" CommandName="Update" />
                                                        <br />
                                                        <br />
                                                        <label class="lbl3">Message</label>
                                                        <asp:Label ID="Lbl_e_Message" runat="server" ForeColor="Red" Font-Bold="true" Height="20px" Style="line-height: 20px"></asp:Label>
                                                        <br />
                                                        <br />
                                                    </div>
                                                </asp:Panel>
                                                <asp:ModalPopupExtender ID="Mpe_e_contribution" runat="server" TargetControlID="HfDummyTrigger22" PopupControlID="Pnl_e_contribution"></asp:ModalPopupExtender>
                                                <asp:Panel ID="Pnl_h_contribution" runat="server">
                                                    <div style="position: fixed; top: 0; left: 0; background-color: white; opacity: 0.8; width: 100%; height: 100%;">
                                                    </div>
                                                    <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                                                        <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                                            <asp:ImageButton ID="ImageButton3" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" />
                                                        </div>
                                                        <h3>Medical plan Contribution History</h3>
                                                        <asp:TextBox ID="Txt_h_contribution" runat="server" Width="575px" Height="300px" TextMode="MultiLine" Font-Size="12px" BorderStyle="None" ReadOnly="true" Text='<%# Eval("INS_CONT_HISTORY") %>'></asp:TextBox>
                                                    </div>
                                                </asp:Panel>
                                                <asp:ModalPopupExtender ID="Mpe_h_contribution" runat="server" TargetControlID="ImgBtnContributionHistory" PopupControlID="Pnl_h_contribution"></asp:ModalPopupExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Employee Class" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="LblContributionName" runat="server" Text='<%# Eval("INS_CONT_CLASSNAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Contribution" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                            <ItemTemplate>
                                                <asp:Label ID="LblContributionType" runat="server" Text='<%# Eval("INS_CONT_CONTRIBUTION_ID") %>'></asp:Label>
                                                <asp:Label ID="LblContributionAmount" runat="server" Text='<%# Eval("INS_CONT_AMOUNT", "{0:n}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:Panel ID="Pnl_nec_newContribution" runat="server" DefaultButton="Btn_nec_saveContribution">
                                    <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                                    </div>
                                    <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                                        <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                            <asp:ImageButton ID="ImageButton8" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" />
                                        </div>
                                        <h3>Add Employer Contribution</h3>
                                        <label class="lbl3">Medical plan</label>
                                        <asp:DropDownList ID="Ddl_nec_InsurancePlans" runat="server" CssClass="ddl2"></asp:DropDownList>
                                        <br />
                                        <br />
                                        <label class="lbl3">Contribution Method</label>
                                        <asp:DropDownList ID="Ddl_nec_ContributionMethod" runat="server" CssClass="ddl2"></asp:DropDownList>
                                        <br />
                                        <br />
                                        <label class="lbl3">Employee Class</label>
                                        <asp:DropDownList ID="Ddl_nec_EmployeeClass" runat="server" CssClass="ddl2"></asp:DropDownList>
                                        <br />
                                        <br />
                                        <label class="lbl3">Contribution Amount</label>
                                        <asp:TextBox ID="Txt_nec_contribution" runat="server" CssClass="txt3"></asp:TextBox>
                                        *
                                        <asp:Literal ID="Lit_nec_ContributionType" runat="server"></asp:Literal>
                                        <br />
                                        <br />
                                        <label class="lbl3" style="background-color: white; color: white;">.</label>
                                        <asp:Button ID="Btn_nec_saveContribution" CssClass="btn" runat="server" Text="Submit" OnClick="Btn_ic_saveContribution_Click" />
                                        <br />
                                        <br />
                                        <label class="lbl3">Message</label>
                                        <asp:Label ID="Lbl_nec_Message" runat="server" ForeColor="Red" Font-Bold="true" Height="20px" Style="line-height: 20px"></asp:Label>
                                        <br />
                                        <br />
                                    </div>
                                </asp:Panel>
                                <asp:ModalPopupExtender ID="Mpe_nec_InsuranceContribution" runat="server" TargetControlID="Btn_nec_InsuranceContribution" PopupControlID="Pnl_nec_newContribution"></asp:ModalPopupExtender>
                            </asp:Panel>

                            <asp:Panel ID="PnlPlanYear" runat="server">
                                <%if (Feature.SelfMeasurementPeriodsEnabled)
                                    {%>
                                <div style="position: absolute; top: 15px; right: 85px;">
                                    <asp:Button ID="BtnNewPlanYear" CssClass="btn" runat="server" Text="New" />
                                </div>
                                <%} %><h3>Stability Period</h3>
                                <h3>Stability Period</h3>
                                <i>You are viewing page
                       
                                    <%=GvPlanYears.PageIndex + 1%>
                        of
                       
                                    <%=GvPlanYears.PageCount%>
                                </i>
                                <br />
                                <i>Showing 
                       
                                    <asp:Literal ID="LitPyShow" runat="server"></asp:Literal>
                                    Stability Periods of 
                       
                                    <asp:Literal ID="LitPyTotal" runat="server"></asp:Literal>
                                </i>
                                <asp:GridView ID="GvPlanYears" runat="server" AutoGenerateColumns="false" CssClass="gridviewHeader" ShowHeaderWhenEmpty="true" CellPadding="0" ForeColor="#333333" EmptyDataText="There are currently no active Stability Periods." GridLines="None" Width="500px" Font-Size="15px" AllowPaging="true" PageSize="10" OnRowUpdating="GvPlanYears_RowUpdating" OnRowEditing="GvPlanYears_RowEditing" OnSelectedIndexChanged="GvPlanYears_SelectedIndexChanged" OnPageIndexChanging="GvPlanYears_PageIndexChanging">
                                    <AlternatingRowStyle BackColor="White" />
                                    <EditRowStyle BackColor="#eb0029" />
                                    <FooterStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#eb0029" Font-Bold="True" ForeColor="black" />
                                    <PagerStyle BackColor="#eb0029" HorizontalAlign="Center" />
                                    <PagerSettings FirstPageImageUrl="~/design/first.png" Mode="NextPreviousFirstLast" LastPageImageUrl="/design/last.png" NextPageImageUrl="/design/next.png" PreviousPageImageUrl="/design/prev.png" PageButtonCount="25" Position="TopAndBottom" />
                                    <RowStyle BackColor="#EFF3FB" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                    <SortedAscendingHeaderStyle BackColor="#eb0029" />
                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                    <SortedDescendingHeaderStyle BackColor="#eb0029" />
                                    <EmptyDataTemplate>
                                        <asp:Label ID="LblEmptyDataSource" runat="server" Text="There are currently no active Stability Periods."></asp:Label>
                                    </EmptyDataTemplate>
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="75px">
                                            <ItemTemplate>
                                                <%if (Feature.SelfMeasurementPeriodsEnabled)
                                                    {%>
                                                <asp:ImageButton ID="ImgBtn_epy_edit" runat="server" ImageUrl="~/images/edit_notes.png" Height="25px" CommandName="Edit" />
                                                <%} %>

                                                <asp:ImageButton ID="ImgBtn_hpy_view" runat="server" ImageUrl="~/images/history.png" Height="25px" CommandName="Select" />
                                                <asp:HiddenField ID="HfDummyTrigger" runat="server" />
                                                <asp:HiddenField ID="HfDummyTrigger2" runat="server" />
                                                <asp:HiddenField ID="HfPlanYearID" runat="server" Value='<%# Eval("PLAN_YEAR_ID") %>' />
                                                <asp:Panel ID="Pnl_hpy" runat="server">
                                                    <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                                                    </div>
                                                    <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                                                        <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                                            <asp:ImageButton ID="ImageButton3" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" />
                                                        </div>
                                                        <h3>Stability Period History</h3>
                                                        <asp:TextBox ID="TxtPlanYearHistory" runat="server" Width="575px" Height="300px" TextMode="MultiLine" Font-Size="12px" BorderStyle="None" ReadOnly="true" Text='<%# Eval("PLAN_YEAR_HISTORY") %>'></asp:TextBox>
                                                    </div>
                                                </asp:Panel>
                                                <asp:ModalPopupExtender ID="MpePlanYearHistory" runat="server" TargetControlID="HfDummyTrigger" PopupControlID="Pnl_hpy"></asp:ModalPopupExtender>

                                                <asp:Panel ID="Pnl_epy" runat="server" DefaultButton="Btn_epy_Update">
                                                    <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                                                    </div>
                                                    <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                                                        <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                                            <asp:ImageButton ID="ImgBtn_epy_Cancel" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" OnClick="ImgBtnEquivEditCancel_Click" />
                                                        </div>
                                                        <h3>Edit Stability Period</h3>
                                                        <label class="lbl3">Name</label>
                                                        <asp:TextBox ID="Txt_epy_Name" runat="server" CssClass="txt3" Text='<%# Eval("PLAN_YEAR_DESCRIPTION") %>'></asp:TextBox>
                                                        <br />
                                                        <label class="lbl3">Start Date</label>
                                                        <asp:TextBox ID="Txt_epy_StartDate" runat="server" CssClass="txt3" Text='<%# Eval("PLAN_YEAR_START", "{0:MM/dd/yyyy}") %>'></asp:TextBox>
                                                        <br />
                                                        <label class="lbl3">End Date</label>
                                                        <asp:TextBox ID="Txt_epy_EndDate" runat="server" CssClass="txt3" Text='<%# Eval("PLAN_YEAR_END", "{0:MM/dd/yyyy}") %>'></asp:TextBox>

                                                        <br />
                                                        <label class="lbl3">Notes</label>
                                                        <asp:TextBox ID="Txt_epy_Notes" runat="server" Width="400px" Height="100px" TextMode="MultiLine" Text='<%# Eval("PLAN_YEAR_NOTES") %>'></asp:TextBox>
                                                        <br />
                                                        <br />
                                                        <label class="lbl3" style="background-color: white; color: white;">.</label>
                                                        <asp:Button ID="Btn_epy_Update" CssClass="btn" runat="server" Text="Submit" CommandName="Update" />
                                                        <br />
                                                        <br />
                                                        <br />
                                                        <label class="lbl3">Message</label>
                                                        <asp:Label ID="Lbl_npy_Message" runat="server" ForeColor="Red" Font-Bold="true" Height="20px" Style="line-height: 20px"></asp:Label>
                                                        <br />
                                                        <br />
                                                    </div>
                                                </asp:Panel>
                                                <asp:ModalPopupExtender ID="Mpe_epy" runat="server" PopupControlID="Pnl_epy" TargetControlID="HfDummyTrigger2"></asp:ModalPopupExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Stability Period Name" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="LblGvPlanName" runat="server" Text='<%# Eval("PLAN_YEAR_DESCRIPTION") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Stability Start Date" HeaderStyle-Width="125px" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="LblGvRenewalDate" runat="server" Text='<%# Eval("PLAN_YEAR_START", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Stability End Date" HeaderStyle-Width="125px" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="LblGvEndDate" runat="server" Text='<%# Eval("PLAN_YEAR_END", "{0:MM/dd/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <%if (Feature.SelfMeasurementPeriodsEnabled)
                                    {%>
                                <asp:Panel ID="PnlNewPlanYear" runat="server" DefaultButton="BtnPlanYearUpdate">
                                    <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                                    </div>
                                    <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                                        <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                            <asp:ImageButton ID="ImgBtnEquivEditCancel" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" OnClick="ImgBtnEquivEditCancel_Click" />
                                        </div>
                                        <h3>New Stability Period</h3>
                                        <label class="lbl3">Name</label>
                                        <asp:TextBox ID="Txt_npy_Name" runat="server" CssClass="txt3"></asp:TextBox>
                                        <br />
                                        <label class="lbl3">Start Date</label>
                                        <asp:TextBox ID="Txt_npy_StartDate" runat="server" CssClass="txt3"></asp:TextBox>
                                        <br />
                                        <label class="lbl3">Notes</label>
                                        <asp:TextBox ID="Txt_npy_Notes" runat="server" Width="400px" Height="100px" TextMode="MultiLine"></asp:TextBox>
                                        <br />
                                        <br />
                                        <label class="lbl3" style="background-color: white; color: white;">.</label>
                                        <asp:Button ID="BtnPlanYearUpdate" CssClass="btn" runat="server" Text="Submit" OnClick="BtnPlanYearUpdate_Click" />
                                        <br />
                                        <br />
                                        <br />
                                        <label class="lbl3">Message</label>
                                        <asp:Label ID="Lbl_npy_Message" runat="server" ForeColor="Red" Font-Bold="true" Height="20px" Style="line-height: 20px"></asp:Label>
                                        <br />
                                        <br />
                                    </div>
                                </asp:Panel>
                                <%} %>
                                <asp:ModalPopupExtender ID="MpeNewPlanYear" runat="server" TargetControlID="BtnNewPlanYear" PopupControlID="PnlNewPlanYear"></asp:ModalPopupExtender>
                            </asp:Panel>



                            <asp:Panel ID="PnlGrossPayExclusion" runat="server">
                                <div style="position: absolute; top: 15px; right: 85px;">
                                    <asp:Button ID="BtnNewGPFilter" runat="server" CssClass="btn" Text="New" OnClick="BtnNewGPFilter_Click" />
                                </div>
                                <h3>Gross Pay Description Exclusion</h3>
                                <p>
                                    By creating this Exclusion rule, the system will ignore all payroll file records that contain the specified Gross Pay Description. 
                   
                                </p>
                                <i>You are viewing page
                       
                                    <%=GvGrossPayExclusion.PageIndex + 1%>
                        of
                       
                                    <%=GvGrossPayExclusion.PageCount%>
                                </i>
                                <br />
                                <i>Showing 
                       
                                    <asp:Literal ID="LitGpExcShown" runat="server"></asp:Literal>
                                    Pay Description Filters of 
                       
                                    <asp:Literal ID="LitGpExcCount" runat="server"></asp:Literal>
                                </i>
                                <asp:GridView ID="GvGrossPayExclusion" runat="server" CssClass="gridviewHeader" AutoGenerateColumns="false" EmptyDataText="Currently no PAY DESCRIPTION filters can be found." CellPadding="0" ForeColor="#333333" GridLines="None" Width="500px" Font-Size="10px" OnRowDeleting="GvGrossPayExclusion_RowDeleting" AllowPaging="true" PageSize="17" OnPageIndexChanging="GvGrossPayExclusion_PageIndexChanging">
                                    <AlternatingRowStyle BackColor="White" />
                                    <EditRowStyle BackColor="#eb0029" />
                                    <FooterStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#eb0029" Font-Bold="True" ForeColor="black" />
                                    <PagerStyle BackColor="#eb0029" HorizontalAlign="Center" />
                                    <PagerSettings FirstPageImageUrl="~/design/first.png" Mode="NextPreviousFirstLast" LastPageImageUrl="/design/last.png" NextPageImageUrl="/design/next.png" PreviousPageImageUrl="/design/prev.png" PageButtonCount="25" Position="TopAndBottom" />
                                    <RowStyle BackColor="#EFF3FB" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                    <SortedAscendingHeaderStyle BackColor="#eb0029" />
                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                    <SortedDescendingHeaderStyle BackColor="#eb0029" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgBtn_gp_delete" runat="server" ImageUrl="~/images/close_box_red.png" Height="25px" CommandName="Delete" />
                                                <asp:ConfirmButtonExtender ID="CbeDeleteGP" runat="server" TargetControlID="ImgBtn_gp_delete" ConfirmText="Are you sure you want to remove this filter?"></asp:ConfirmButtonExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pay Code" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="HfFilterID" runat="server" Value='<%# Eval("GROSS_PAY_ID") %>' />
                                                <asp:Literal ID="LitExtGPID" runat="server" Text='<%# Eval("GROSS_PAY_EXTERNAL_ID") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Pay Code Description" HeaderStyle-Width="300px" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Literal ID="LitGPDescription" runat="server" Text='<%# Eval("GROSS_PAY_DESCRIPTION") %>'></asp:Literal>

                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:Panel ID="PnlNewGPFilter" runat="server" DefaultButton="BtnSaveGpFilter">
                                    <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                                    </div>
                                    <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                                        <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                            <asp:ImageButton ID="ImageButton4" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" OnClick="ImgBtnEquivEditCancel_Click" />
                                        </div>
                                        <h3>New Gross Pay Filter</h3>
                                        <label class="lbl3">Select Gross Pay</label>
                                        <asp:DropDownList ID="DdlGrossPayFilter" runat="server" CssClass="ddl2"></asp:DropDownList>

                                        <br />
                                        <br />
                                        <label class="lbl3" style="background-color: white; color: white;">.</label>
                                        <asp:Button ID="BtnSaveGpFilter" CssClass="btn" runat="server" Text="Submit" OnClick="BtnSaveGpFilter_Click" />
                                        <br />
                                        <br />
                                        <br />
                                        <label class="lbl3">Message</label>
                                        <asp:Label ID="LblGpFilterMessage" runat="server" ForeColor="Red" Font-Bold="true" Height="20px" Style="line-height: 20px"></asp:Label>
                                        <br />
                                        <br />
                                    </div>
                                </asp:Panel>
                                <asp:HiddenField ID="HfGPDummy" runat="server" />
                                <asp:ModalPopupExtender ID="MpeGPFilter" runat="server" TargetControlID="HfGPDummy" PopupControlID="PnlNewGPFilter"></asp:ModalPopupExtender>

                            </asp:Panel>

                            <asp:Panel ID="PnlEmployeeClassification" runat="server">
                                <div style="position: absolute; top: 15px; right: 85px;">
                                    <asp:Button ID="BtnNewEmpClassification" runat="server" CssClass="btn" Text="New" OnClick="BtnNewEmpClassification_Click" />
                                </div>
                                <h3>Employee Classifications</h3>
                                <p>
                                    By setting up Employee Classifications, this will allow you to mass update your insurance offerings. 
                   
                                </p>
                                <i>You are viewing page
                       
                                    <%=GvEmployeeClassifications.PageIndex + 1%>
                        of
                       
                                    <%=GvEmployeeClassifications.PageCount%>
                                </i>
                                <br />
                                <i>Showing 
                       
                                    <asp:Literal ID="litClassesShown" runat="server"></asp:Literal>
                                    Employee Classifications of 
                       
                                    <asp:Literal ID="litClassesCount" runat="server"></asp:Literal>
                                </i>
                                <asp:GridView ID="GvEmployeeClassifications" runat="server" CssClass="gridviewHeader" AutoGenerateColumns="false" EmptyDataText="Currently no Employee Classificatons can be found." CellPadding="0" ForeColor="#333333" GridLines="None" Width="526px" Font-Size="16px" OnRowDeleting="GvEmployeeClassifications_RowDeleting" OnRowEditing="GvEmployeeClassifications_RowEditing" OnSelectedIndexChanged="GvEmployeeClassifications_SelectedIndexChanged" OnPageIndexChanging="GvEmployeeClassifications_PageIndexChanging" AllowPaging="True" PageSize="17">
                                    <AlternatingRowStyle BackColor="White" />
                                    <EditRowStyle BackColor="white" />
                                    <FooterStyle BackColor="#eb0029" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#eb0029" Font-Bold="True" ForeColor="black" />
                                    <PagerStyle BackColor="#eb0029" HorizontalAlign="Center" />
                                    <PagerSettings FirstPageImageUrl="~/design/first.png" Mode="NextPreviousFirstLast" LastPageImageUrl="/design/last.png" NextPageImageUrl="/design/next.png" PreviousPageImageUrl="/design/prev.png" PageButtonCount="25" Position="TopAndBottom" />
                                    <RowStyle BackColor="#EFF3FB" />
                                    <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                    <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                    <SortedAscendingHeaderStyle BackColor="#eb0029" />
                                    <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                    <SortedDescendingHeaderStyle BackColor="#eb0029" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ImgBtn_ec_delete" runat="server" ImageUrl="~/images/close_box_red.png" Height="25px" CommandName="Delete" />
                                                <asp:ImageButton ID="ImgBtn_ec_edit" runat="server" ImageUrl="~/images/edit_notes.png" Height="25px" CommandName="Edit" />
                                                <asp:ImageButton ID="ImgBtn_ec_history" runat="server" ImageUrl="~/images/history.png" Height="25px" CommandName="Select" />
                                                <asp:ConfirmButtonExtender ID="CbeDeleteEc" runat="server" TargetControlID="ImgBtn_ec_delete" ConfirmText="Are you sure you want to remove this Employee Class?"></asp:ConfirmButtonExtender>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Employee Class Description" HeaderStyle-Width="300px" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="HfClassID" runat="server" Value='<%# Eval("CLASS_ID") %>' />
                                                <asp:Literal ID="Lit_ec_Description" runat="server" Text='<%# Eval("CLASS_DESC") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <asp:Panel ID="PnlNewEmployeeClass" runat="server" DefaultButton="Btn_ec_Save">
                                    <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                                    </div>
                                    <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                                        <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                            <asp:ImageButton ID="ImageButton6" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" OnClick="ImgBtnEquivEditCancel_Click" />
                                        </div>
                                        <h3>
                                            <asp:Literal ID="Lit_ec_function" runat="server"></asp:Literal>
                                            -  Employee Classification</h3>
                                        <label class="lbl3">Enter Description</label>
                                        <asp:TextBox ID="Txt_ec_EmployeeClass" runat="server" CssClass="txt"></asp:TextBox>
                                        <asp:HiddenField ID="Hf_ec_id" runat="server" />
                                        <br />
                                        <br />
                                        <label class="lbl3">Affordability Safe Harbor</label>
                                        <asp:DropDownList ID="Ddl_ec_ASH" runat="server" CssClass="ddl2"></asp:DropDownList>
                                        <br />
                                        <br />
                                        <label class="lbl3">Waiting Period</label>
                                        <asp:DropDownList ID="Ddl_ec_WaitingPeriod" runat="server" CssClass="ddl2"></asp:DropDownList>
                                        <br />
                                        <br />
                                        <label class="lbl3">Default Offer Code</label>
                                        <asp:DropDownList ID="Ddl_ec_offerCode" runat="server" CssClass="ddl2"></asp:DropDownList>
                                        <br />
                                        <br />
                                        <label class="lbl3" style="background-color: white; color: white;">.</label>
                                        <asp:Button ID="Btn_ec_Save" CssClass="btn" runat="server" Text="Submit" OnClick="Btn_ec_Save_Click" />
                                        <br />
                                        <br />
                                        <br />
                                        <label class="lbl3">Message</label>
                                        <asp:Label ID="LblEmpClassMessage" runat="server" ForeColor="Red" Font-Bold="true" Height="20px" Style="line-height: 20px"></asp:Label>
                                        <br />
                                        <br />
                                    </div>
                                </asp:Panel>
                                <asp:HiddenField ID="HfDummyTrigger1" runat="server" />
                                <asp:ModalPopupExtender ID="MpeNewEmployeeClass" runat="server" TargetControlID="HfDummyTrigger1" PopupControlID="PnlNewEmployeeClass"></asp:ModalPopupExtender>

                                <asp:Panel ID="Pnl_h_employeeClass" runat="server">
                                    <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                                    </div>
                                    <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                                        <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                            <asp:ImageButton ID="ImageButton3" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" />
                                        </div>
                                        <h3>Employee Class History</h3>
                                        <asp:TextBox ID="Txt_h_employeeClass" runat="server" Width="575px" Height="300px" TextMode="MultiLine" Font-Size="12px" BorderStyle="None" ReadOnly="true"></asp:TextBox>
                                    </div>
                                </asp:Panel>
                                <asp:HiddenField ID="Hf_h_DummyTrigger2" runat="server" />
                                <asp:ModalPopupExtender ID="Mpe_h_employeeClass" runat="server" TargetControlID="Hf_h_DummyTrigger2" PopupControlID="Pnl_h_employeeClass"></asp:ModalPopupExtender>
                            </asp:Panel>
                        </div>

                        <asp:HiddenField ID="HfDummyTrigger" runat="server" />
                        <asp:Panel ID="PnlWebPageMessage" runat="server">
                            <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                            </div>
                            <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                                <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                    <asp:ImageButton ID="ImageButton5" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" OnClick="ImgBtnEquivEditCancel_Click" />
                                </div>
                                <h3>Webpage Message</h3>
                                <asp:Literal ID="LitMessage" runat="server"></asp:Literal>
                                <br />
                                <br />
                            </div>
                        </asp:Panel>

                        <asp:ModalPopupExtender ID="MpeWebPageMessage" runat="server" TargetControlID="HfDummyTrigger" PopupControlID="PnlWebPageMessage"></asp:ModalPopupExtender>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="BtnSaveProfile" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
        </div>

        <div id="bottomcontainer">
            <div id="footer">
                Copyright &copy; <%= Branding.CopyrightYears %> <a href="<%= Branding.CompanyWebSite %>"><%= Branding.CompanyName %></a> - All Rights Reserved   
            <br />
                <div style="clear: both;">&nbsp;</div>
            </div>
        </div>
    </form>

    <script>
        setTimeout(AutoLogout, <%= Feature.AutoLogoutTime %> );

        function AutoLogout() {
            alert("<%= Branding.AutoLogoutMessage %>");
            window.location = window.location.href;
        }
    </script>
</body>
</html>
