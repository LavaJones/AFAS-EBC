<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="1094_approval.aspx.cs" Inherits="securepages_1094_approval" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" type="text/css" href="../default.css" />
    <link rel="stylesheet" type="text/css" href="../menu.css" />
    <link rel="stylesheet" type="text/css" href="../v_menu.css" />
    <title>1094 Approval</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
        <div id="container">
            <div id="header" style="height: 90px">
                <a href="default.aspx">
                    <img src="<%= Page.ResolveUrl(Branding.LogoUri) %>" style="height: 70px" alt="<%= Branding.ProductName %> Logo" class="logo" />
                </a>
                <ul id="toplinks">
                    <li>Stability Period: 
               
                        <asp:HiddenField ID="HfEmployerTypeID" runat="server" />
                        <asp:HiddenField ID="HfUserName" runat="server" />
                    </li>
                    <li>User Name:<asp:Literal ID="LitUserName" runat="server"></asp:Literal></li>
                    <li>
                        <asp:Button ID="BtnLogout" CssClass="btn" runat="server" Text="Log Out" OnClick="BtnLogout_Click" UseSubmitBehavior="false" /></li>

                </ul>
                <asp:HiddenField ID="HfDistrictID" runat="server" />
            </div>
            <div id="nav2">
                <nav>
                    <ul>
                        <li><a href="default.aspx">Home</a></li>
                        <li><a href="e_find.aspx">Employee</a></li>
                        <li><a href="r_reporting.aspx">Reporting</a></li>
                        <li><a href="s_setup.aspx">Employer Setup</a></li>
                        <li><a href="t_terms.aspx">ACA Terms</a></li>
                        <li><a href="contact.aspx">Help</a></li>
                    </ul>
                </nav>
                <ul class="right">
                    <li></li>
                </ul>
            </div>
            <div id="content">
                <h3>1094-C Review</h3>
                <p>
                    Please review the 1094-C information below.              
                </p>
                Select the Calendar Year:
               
                        <asp:DropDownList ID="DdlCalendarYear" runat="server" CssClass="ddl" OnSelectedIndexChanged="DdlCalendarYear_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Text="select" Value="select" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="2015" Value="2015"></asp:ListItem>
                            <asp:ListItem Text="2016" Value="2016"></asp:ListItem>
                            <asp:ListItem Text="2017" Value="2017"></asp:ListItem>
                        </asp:DropDownList>
                <br />
                <br />

                <asp:Panel ID="Document1094c" runat="server" Visible="false" ScrollBars="Auto">
                    <label>1. Name of ALE Member (Employer)</label>
                    <asp:Literal ID="Doc1094Field1" runat="server"></asp:Literal>
                    <br />

                    <label>2. Employer identification number (EIN)</label>
                    <asp:Literal ID="Doc1094Field2" runat="server"></asp:Literal>
                    <br />

                    <label>3. Street address (including room or suite no.)</label>
                    <asp:Literal ID="Doc1094Field3" runat="server"></asp:Literal>
                    <br />

                    <label>4. City or town</label>
                    <asp:Literal ID="Doc1094Field4" runat="server"></asp:Literal>
                    <br />

                    <label>5. State or province</label>
                    <asp:Literal ID="Doc1094Field5" runat="server"></asp:Literal>
                    <br />

                    <label>6. Country and ZIP or foreign postal code</label>
                    <asp:Literal ID="Doc1094Field6" runat="server"></asp:Literal>
                    <br />

                    <label>7. Name of person to contact</label>
                    <asp:Literal ID="Doc1094Field7" runat="server"></asp:Literal>
                    <br />

                    <label>8. Contact telephone number</label>
                    <asp:Literal ID="Doc1094Field8" runat="server"></asp:Literal>
                    <br />

                    <label>9. Name of Designated Government Entity (only if applicable)</label>
                    <asp:Literal ID="Doc1094Field9" runat="server"></asp:Literal>
                    <br />

                    <label>10. Employer identification number (EIN)</label>
                    <asp:Literal ID="Doc1094Field10" runat="server"></asp:Literal>
                    <br />

                    <label>11. Street address (including room or suite no.)</label>
                    <asp:Literal ID="Doc1094Field11" runat="server"></asp:Literal>
                    <br />

                    <label>12. City or town</label>
                    <asp:Literal ID="Doc1094Field12" runat="server"></asp:Literal>
                    <br />

                    <label>13. State or province</label>
                    <asp:Literal ID="Doc1094Field13" runat="server"></asp:Literal>
                    <br />

                    <label>14. Country and ZIP or foreign postal code</label>
                    <asp:Literal ID="Doc1094Field14" runat="server"></asp:Literal>
                    <br />

                    <label>15. Name of person to contact</label>
                    <asp:Literal ID="Doc1094Field15" runat="server"></asp:Literal>
                    <br />

                    <label>16. Contact telephone number</label>
                    <asp:Literal ID="Doc1094Field16" runat="server"></asp:Literal>
                    <br />

                    <label>17. Reserved</label>
                    <br />

                    <label>18. Total number of Forms 1095-C submitted with this transmittal</label>
                    <asp:Literal ID="Doc1094Field18" runat="server"></asp:Literal>
                    <br />

                    <label>19. Is this the authoritative transmittal for this ALE Member? If "Yes," check the box and continue. If "No," see instructions</label>
                    <asp:CheckBox ID="Doc1094Field19" Checked="true" Enabled="false" runat="server" />
                    <br />
                    <br />

                    <h3>Part II  ALE Member Information</h3>
                    <br />

                    <label>20. Total number of Forms 1095-C filed by and/or on behalf of ALE Member</label>
                    <asp:Literal ID="Doc1094Field20" runat="server"></asp:Literal>
                    <br />

                    <label>21. Is ALE Member a member of an Aggregate ALE Group?</label>
                    <asp:CheckBox ID="Doc1094Field21Yes" Enabled="false" runat="server" Text="Yes" />
                    <asp:CheckBox ID="Doc1094Field21No" Enabled="false" runat="server" Text="No" />
                    <br />

                    <label>22. Certifications of Eligibility (select all that apply):</label>
                    <asp:CheckBox ID="Doc1094Field22A" Enabled="false" runat="server" Text="A. Qualifying Offer Method" />
                    <asp:CheckBox ID="Doc1094Field22B" Enabled="false" runat="server" Text="B. Reserved" />
                    <asp:CheckBox ID="Doc1094Field22C" Enabled="false" runat="server" Text="C. Section 4980H Transition Relief" />
                    <asp:CheckBox ID="Doc1094Field22D" Enabled="false" runat="server" Text="D. 98% Offer Method" />
                    <br />
                    <br />

                    <h3>Part III  ALE Member Information--Monthly</h3>
                    <br />

                    <asp:GridView ID="Gv_ALE_Monthly" runat="server" AutoGenerateColumns="false"
                        EmptyDataText="No Monthly data found, please contact IT" BackColor="White" BorderColor="#336666"
                        BorderStyle="Double" BorderWidth="3px" CellPadding="4" GridLines="Horizontal">
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
                            <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:HiddenField ID="HiddenId" runat="server" />
                                    <asp:Literal ID="Doc1094MonthlyLineNum" runat="server" Text='<%# Eval("Doc1094MonthlyLineNum") %>'></asp:Literal>
                                    <asp:Literal ID="Doc1094MonthlyMonthLabel" runat="server" Text='<%# Eval("Doc1094MonthlyMonthLabel") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="(a) Minimum Essential Coverage Offer Indicator" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:CheckBox ID="Doc1094MonthlyAYes" Enabled="false" runat="server" Text="Yes" Checked='<%# Eval("Doc1094MonthlyAYes") %>' />
                                    <asp:CheckBox ID="Doc1094MonthlyANo" Enabled="false" runat="server" Text="No" Checked='<%# Eval("Doc1094MonthlyANo") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="(b) Section 4980H Full-Time Employee Count for ALE Member" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Literal ID="Doc1094MonthlyB" runat="server" Text='<%# Eval("Doc1094MonthlyB") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="(c) Total Employee Count for ALE Member" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Literal ID="Doc1094MonthlyC" runat="server" Text='<%# Eval("Doc1094MonthlyC") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="(d) Aggregated Group Indicator" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:CheckBox ID="Doc1094MonthlyD" Enabled="false" runat="server" Checked='<%# Eval("Doc1094MonthlyD") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="(e) Section 4980H Transition Relief Indicator" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Literal ID="Doc1094MonthlyE" runat="server" Text='<%# Eval("Doc1094MonthlyE") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <br />

                    <h3>Part IV  Other ALE Members of Aggregate ALE Group</h3>
                    <p>Enter the names and EINs of Other ALE Members of the Aggregated ALE Group (who were members at any time during the calendar year).</p>
                    <br />
                    <asp:GridView ID="DV_OtherAleMembers" runat="server" AutoGenerateColumns="false"
                        EmptyDataText="No Aggregate ALE data found or you are not an Aggregated ALE." BackColor="White" BorderColor="#336666"
                        BorderStyle="Double" BorderWidth="3px" CellPadding="4" GridLines="Horizontal">
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
                            <asp:TemplateField HeaderText="Name" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Literal ID="Doc1094OtherLineNum" runat="server"></asp:Literal>
                                    <asp:Literal ID="Doc1094OtherName" runat="server" Text='<%# Eval("OtherALEBusinessNameLine1Txt") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="EIN" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Literal ID="Doc1094OtherEIN" runat="server" Text='<%# Eval("OtherALEEIN") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                    <br />
                    <br />
                    <br />

                    <h3>Additional information</h3>
                    <p>All employees getting a 1095C</p>
                    <br />
                    <asp:GridView ID="Gv1095C" runat="server" AutoGenerateColumns="false"
                        EmptyDataText="No 1095 data found, please contact IT" BackColor="White" BorderColor="#336666"
                        BorderStyle="Double" BorderWidth="3px" CellPadding="4" GridLines="Horizontal">
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
                            <asp:TemplateField HeaderText="First Name" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Literal ID="Doc1095FirstName" runat="server" Text='<%# Eval("OtherCompletePersonFirstNm") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Last Name" HeaderStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:Literal ID="Doc1095LastName" runat="server" Text='<%# Eval("OtherCompletePersonLastNm") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>

                <asp:HiddenField ID="HfDummyTrigger" runat="server" />
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
            </div>
        </div>

        <div style="clear: both;">&nbsp;</div>
        <div id="footer">
            Copyright &copy; <%= Branding.CopyrightYears %> <a href="<%= Branding.CompanyWebSite %>"><%= Branding.CompanyName %></a> - All Rights Reserved   
            <br />
            <div style="clear: both;">&nbsp;</div>
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

