<%@ Page Title="" Language="C#"  MasterPageFile="~/irs_submission/irs_submission.Master" AutoEventWireup="true" CodeBehind="step2.aspx.cs" Inherits="step2" %>
<%@ Import Namespace="Afas.AfComply.Domain" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="bodycontent" runat="server">

    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" type="text/css" href="/questionnaire.css" />
   
    <asp:HiddenField ID="HfUserName" runat="server" />
    <asp:HiddenField ID="HfDistrictID" runat="server" />

        <div id="container">
           
         
            <div id="content">
                <asp:UpdatePanel ID="upALE" runat="server" style="padding-left:25%;">
                    <ContentTemplate>
                        <h3>Step B: Aggregated ALE Group</h3>
                        <p>An Aggregated ALE Group is a group of employers (referred to by the IRS as ALE Members) under common control, sometimes referred to as a “controlled group.”
                          One example might be a group of auto dealers with separate locations and different federal tax IDs, but the same owner who owns 80% or more of each dealership.<br /><br />
                          The IRS uses rules under Internal Revenue Code sections 414(b), 414(c), 414(m), or 414(o) to determine aggregated ALE status.<br /><br />
                          These rules can be complex, and you should consult your tax or legal counsel if you are unsure of your controlled group status.<br /><br />
                          Here you will answer "Yes" or "No" to the question “Are you a member of an Aggregated ALE Group?”<br /><br />
                          This situation occurs occasionally in some industries, such as automotive, but will not apply to many other employers, including most government employers.


                        </p>










                        Are you a member of an Aggregated ALE Group?
             
                        <asp:DropDownList ID="Ddl_step2_ALE" runat="server" CssClass="ddl2" AutoPostBack="true" OnSelectedIndexChanged="Ddl_step2_ALE_SelectedIndexChanged">
                            <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                            <asp:ListItem Text="No" Value="false"></asp:ListItem>
                            <asp:ListItem Text="Select" Value="select" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                        <br style="clear: both" />
                        <asp:Panel ID="PnlALE" runat="server" Visible="false">
                            <h4>Enter ALE Members</h4>
                            <table>
                                <tr>
                                    <td>
                                        <asp:ImageButton ID="ImgBtn_add" runat="server" ImageUrl="~/images/disk-save.png" Height="30px" Width="30px" OnClick="ImgBtn_add_Click" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblName" runat="server" Text="Name" CssClass="lbl"></asp:Label>
                                        <asp:TextBox ID="Txt_step2_DGEName" runat="server" CssClass="txt3"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="EIN" CssClass="lbl"></asp:Label>
                                        <asp:TextBox ID="Txt_step2_EIN" runat="server" CssClass="txt3"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td style="padding-left:25%">format: 12-1234567
                          </td>
                                </tr>
                            </table>

                            <br style="clear: both" />
                            <br />
                            <h4>ALE Members</h4>
                            <asp:GridView ID="GvALE" runat="server" EmptyDataText="No ALE members have been added." OnRowDeleting="GvALE_RowDeleting" AutoGenerateColumns="false" Width="450px" BorderStyle="None">
                                <AlternatingRowStyle BackColor="White" />
                                <Columns>
                                    <asp:TemplateField HeaderStyle-Width="30px" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="ImgBtnDeletePayroll" runat="server" ImageUrl="~/images/close_box_red.png" Width="20px" CommandName="Delete" ToolTip="Delete payroll record." />
                                            <asp:ConfirmButtonExtender ID="CbeDelete" runat="server" TargetControlID="ImgBtnDeletePayroll" ConfirmText="Are you sure you want to DELETE this ALE Member?"></asp:ConfirmButtonExtender>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="30px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Name" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="Lbl_gv_step2_name" runat="server" Text='<%# Bind("ALE_NAME") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="EIN" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="Lbl_gv_step2_ein" runat="server" Text='<%# Bind("ALE_EIN") %>'></asp:Label>
                                            <asp:HiddenField ID="Hf_gv_step2_id" runat="server" Value='<%# Bind("ALE_ID") %>' />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="200px" />
                                    </asp:TemplateField>
                                </Columns>
                                <EditRowStyle BackColor="Yellow" />
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="#EFF3FB" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                            </asp:GridView>
                        </asp:Panel>
                        <br />
                        <br />
                        <asp:Button ID="Btn_Prev" runat="server" Text="PREVIOUS" CssClass="btn" Font-Bold="true" Width="10%" Height="30px" Enabled="true" OnClick="Btn_Previous_Click" />
                        <asp:Button ID="Btn_Next" runat="server" Text="NEXT" Font-Bold="true" CssClass="btn" Width="10%" Height="30px" Enabled="false" OnClick="Btn_Next_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>

           
        </div>
        <script>
        setTimeout(AutoLogout, <%= Feature.AutoLogoutTime %> );

        function AutoLogout() {
            alert("<%= Branding.AutoLogoutMessage %>");
            window.location = window.location.href;
        }
    </script>
</asp:Content>
