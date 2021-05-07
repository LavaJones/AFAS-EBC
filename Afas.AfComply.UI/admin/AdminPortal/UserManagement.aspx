<%@ Page EnableSessionState="ReadOnly" Title="Manage Users" Language="C#"
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="UserManagement.aspx.cs"
    Inherits="Afas.AfComply.UI.admin.AdminPortal.UserManagement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>

    <asp:ImageButton ID="ImgBtnExportCSV" align="right" runat="server" OnClick="ImgBtnExportCSV_Click" ImageUrl="~/images/csv.png" Height="30px" ToolTip="Export to .CSV file" />

    <br />
    <label class="lbl3">Select Employer</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    <br />

    <asp:HiddenField ID="HfEmployerTypeID" runat="server" />
    <asp:HiddenField ID="HfUserName" runat="server" />
    <asp:HiddenField ID="HfDistrictID" runat="server" />


    <asp:Panel ID="PnlUsers" runat="server">

        <asp:Button ID="BtnNewUser" CssClass="btn" runat="server" Text="New" />

        <h3>Users</h3>

        <asp:ModalPopupExtender ID="ModalPopupExtender2" runat="server" PopupControlID="PnlNewUser" TargetControlID="BtnNewUser"></asp:ModalPopupExtender>

        <asp:Panel ID="PnlNewUser" runat="server" DefaultButton="BtnSaveNewUser">
            <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
            </div>
            <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 800px; height: auto; margin-top: 0px; font-size: 12px; background-color: white;">
                <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                    <asp:ImageButton ID="ImageButton1" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" />
                </div>
                <h3>Create New User</h3>
                <label class="lbl3">First Name</label>
                <asp:TextBox ID="TxtNewFName" runat="server" CssClass="txtLong"></asp:TextBox>
                <br />
                <label class="lbl3">Last Name</label>
                <asp:TextBox ID="TxtNewLName" runat="server" CssClass="txtLong"></asp:TextBox>
                <br />
                <label class="lbl3">Email</label>
                <asp:TextBox ID="TxtNewEmail" runat="server" CssClass="txtLong"></asp:TextBox>
                <br />
                <label class="lbl3">Phone</label>
                <asp:TextBox ID="TxtNewPhone" runat="server" CssClass="txtLong"></asp:TextBox>
                <br />
                <label class="lbl3">UserName</label>
                <asp:TextBox ID="TxtNewUserName" runat="server" CssClass="txtLong"></asp:TextBox>
                <br />
                <label class="lbl3">Password</label>
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
                <label class="lbl3">Re-enter Password</label>
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

        <asp:GridView ID="GvDistrictUsers" runat="server" CssClass="gridviewHeader" AutoGenerateColumns="false"
            ShowHeaderWhenEmpty="true" CellPadding="0" ForeColor="#333333" AllowPaging="true" PageSize="15"
            EmptyDataText="There are currently no alerts." GridLines="None" Width="500px" Font-Size="10px"
            OnRowDeleting="GvDistrictUsers_RowDeleting"
            OnRowUpdating="GvDistrictUsers_RowUpdating"
            OnPageIndexChanging="GvDistrictUsers_PageIndexChanging">
            <AlternatingRowStyle BackColor="White" />
            <EditRowStyle BackColor="yellow" />
            <FooterStyle BackColor="LightGray" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="LightGray" Font-Bold="True" ForeColor="black" />
            <PagerStyle BackColor="#FF0083AF" HorizontalAlign="Center" />
            <PagerSettings FirstPageImageUrl="~/design/first.png" Mode="NextPreviousFirstLast" LastPageImageUrl="/design/last.png" NextPageImageUrl="/design/next.png" PreviousPageImageUrl="/design/prev.png" PageButtonCount="25" Position="TopAndBottom" />
            <RowStyle BackColor="#EFF3FB" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F5F7FB" />
            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
            <SortedDescendingCellStyle BackColor="#E9EBEF" />
            <SortedDescendingHeaderStyle BackColor="#4870BE" />
            <EmptyDataTemplate>
                <asp:Label ID="LblEmptyDataSource" runat="server" Text="There are currently no users."></asp:Label>
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="ImgBtnEdit" runat="server" ImageUrl="~/images/edit_notes.png" Height="30px" ToolTip="Edit User" />
                        <asp:ModalPopupExtender ID="ModalPopupExtender3" runat="server" PopupControlID="PnlEditUser" TargetControlID="ImgBtnEdit"></asp:ModalPopupExtender>
                        <asp:Panel ID="PnlEditUser" runat="server" DefaultButton="BtnUserUpdate">
                            <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
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
                                <label class="lbl3">Power User</label>
                                <asp:CheckBox ID="CbtmpPowerUser" runat="server" Checked='<%# Eval("User_Power") %>' />
                                <br style="clear: both;" />
                                <p>
                                    * Note: Only a Power User can add/edit records. 
                                </p>
                                <label class="lbl3">Billing User</label>
                                <asp:CheckBox ID="CbtmpBillingUser" runat="server" Checked='<%# Eval("User_Billing") %>' />
                                <br />
                                <p>
                                    * Note: Check the Billing for each user that will be involved in handling the Invoices. 
                                </p>
                                <br />
                                <label class="lbl3">IRS Contact</label>
                                <asp:CheckBox ID="CbtmpIRSContact" runat="server" Checked='<%# Eval("User_IRS_CONTACT") %>' />
                                <br />
                                <p>
                                    * Note: If checked, this name will appear as the contact on the IRS reporting. 
                                </p>
                                <br />
                                <label class="lbl3">Floating User</label>
                                <asp:CheckBox ID="CbtmpFloating" runat="server" Checked='<%# Eval("User_Floater") %>' />
                                <br style="clear: both;" />

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
                <asp:TemplateField HeaderText="Admin" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Left">
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
                <asp:TemplateField HeaderText="Floating" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:CheckBox ID="CbFloating" runat="server" Checked='<%# Eval("User_Floater") %>' Enabled="false" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
</asp:Content>
