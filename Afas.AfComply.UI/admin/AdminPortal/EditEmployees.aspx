<%@ Page EnableSessionState="ReadOnly" Title="Edit Employees" Language="C#" 
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="EditEmployees.aspx.cs" 
    Inherits="Afas.AfComply.UI.admin.AdminPortal.EditEmployees" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    

    <h2>Update Information</h2>
    <br />
    <label class="lbl3">Select Employer</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    <br />
    <label class="lbl3">Search for Employee</label>
    <asp:TextBox ID="TxtEmployee" runat="server" CssClass="txt3"></asp:TextBox> 
    <br />
    <br />
    <asp:Button ID="BtnFindEmployees" runat="server" Text="Search for Employee" CssClass="btn" Width="125px" OnClick="BtnFindEmployees_Click" />
    <br />
    <br />
    <asp:Button ID="BtnResetEmployees" runat="server" Text="Reset Employees" CssClass="btn" Width="125px"  OnClick="BtnResetEmployees_Click" />
    <br />
    <br />
    <label class="lbl3">Select Employee</label>
    <asp:DropDownList ID="DdlEmployee" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployee_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    <br />
    <asp:LinkButton ID="LbViewDependents" runat="server" Text="View Dependents" OnClick="LbViewDependents_Click" Visible="false"></asp:LinkButton>
    <br />
   
     <h3>Profile</h3>
                <div class="left">
                <asp:UpdatePanel ID="PnlEmployeeInfo" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <label class="lbl">First Name</label>
                        <asp:TextBox ID="TxtFirstName" runat="server" CssClass="txt3"></asp:TextBox>                  
                        <br />
                        <label class="lbl">Middle Name</label>
                        <asp:TextBox ID="TxtMiddleName" runat="server" CssClass="txt3"></asp:TextBox>                  
                        <br />
                        <label class="lbl">Last Name:</label>
                        <asp:TextBox ID="TxtLastName" runat="server" CssClass="txt3"></asp:TextBox>
                        <br />
                        <label class="lbl">Address</label>
                        <asp:TextBox ID="TxtAddress" runat="server" CssClass="txt3"></asp:TextBox>
                        <br />
                        <label class="lbl">City</label>
                        <asp:TextBox ID="TxtCity" runat="server" CssClass="txt3"></asp:TextBox>
                        <br />
                        <label class="lbl">State</label>
                        <asp:DropDownList ID="DdlState" runat="server" CssClass="ddl2" AutoPostBack="true"></asp:DropDownList>
                        <br />
                        <label class="lbl">Zip</label>
                        <asp:TextBox ID="TxtZip" runat="server" CssClass="txt3" MaxLength="5" ></asp:TextBox>
                        <asp:RegularExpressionValidator ID="regTxtZip" ControlToValidate="TxtZip" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+"></asp:RegularExpressionValidator>
                        <br />
                        <label class="lbl">SSN</label>
                        <asp:TextBox ID="TxtSSN" runat="server" CssClass="txt3" MaxLength="9" ></asp:TextBox>
                        <asp:RegularExpressionValidator ID="regTxtSSN" ControlToValidate="TxtSSN" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+"></asp:RegularExpressionValidator>
                        <br />
                        <label class="lbl">DOB</label>
                        <asp:TextBox ID="TxtDOB" runat="server" CssClass="txt3" placeholder="mm/dd/yyyy" MaxLength="10"></asp:TextBox>
                        <br />
                        <label class="lbl">Current Date</label>
                        <asp:TextBox ID="TxtCurrDate" runat="server" CssClass="txt3" placeholder="mm/dd/yyyy" MaxLength="10" ReadOnly="true" Enabled="false"></asp:TextBox>
                        <br />
                        <label class="lbl">Hire Date</label>
                        <asp:TextBox ID="TxtHireDate" runat="server" CssClass="txt3" placeholder="mm/dd/yyyy" MaxLength="10"  ReadOnly="true" Enabled="false"></asp:TextBox>
                        <br />
                        <label class="lbl">Term Date</label>
                        <asp:TextBox ID="TxtTermDate" runat="server" CssClass="txt3" placeholder="mm/dd/yyyy" MaxLength="10" ></asp:TextBox>
                        <br />
                        <label class="lbl">IMP End</label>
                        <asp:TextBox ID="TxtImpEnd" runat="server" CssClass="txt3" placeholder="mm/dd/yyyy" MaxLength="10"  ReadOnly="true" Enabled="false"></asp:TextBox>
                        <br />
                        <label class="lbl">Employee Type</label>
                        <asp:DropDownList ID="DdlType" runat="server" CssClass="ddl2" AutoPostBack="true"></asp:DropDownList>
                        <br />
                        <label class="lbl">HR Status</label>
                        <asp:DropDownList ID="DdlHireStatus" runat="server" CssClass="ddl2" AutoPostBack="true"></asp:DropDownList>
                        <br />
                        <label class="lbl">Classification</label>
                        <asp:DropDownList ID="DdlClassification" runat="server" CssClass="ddl2"  AutoPostBack="true"></asp:DropDownList>
                        <br />
                        <label class="lbl">ACA Status</label>
                        <asp:DropDownList ID="DdlAcaStatus" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlAcaStatus_SelectedIndexChanged"  AutoPostBack="true"></asp:DropDownList>
                        <br />
                        <label class="lbl">Employee #</label>
                        <asp:TextBox ID="TxtExtID" runat="server" CssClass="txt3"></asp:TextBox>
                        <br />
                        <br />
                        <br />

                        <asp:Button ID="BtnSave" runat="server" CssClass="btn" Text="Save" OnClick="BtnSave_Click" />
                        <br />
                        <asp:Label ID="lblMsg" runat="server"></asp:Label>
                        <br />
                        <asp:HiddenField ID="HfAcaStatus" runat="server" />
                         <asp:ModalPopupExtender ID="mpAcaStatus" PopupControlID="pnlMessage" TargetControlID="HfAcaStatus" OkControlID="btnOK" runat="server">
                        </asp:ModalPopupExtender>
                        <asp:Panel id="pnlMessage" runat="server">
                            <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                                    </div>
                            <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white; z-index: 10">
                             <p >
                                 Changing the ACA Status alone will not terminate the employee from the system unless a termination date is provided.  Please enter the termination date in the Term Date field above.

                             </p>
                            <p><asp:Button ID="btnOK" Text="OK" runat="server" /></p>
                               </div>
                        </asp:Panel>
                    </ContentTemplate>

                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="DdlState" />
                    </Triggers>

                  </asp:UpdatePanel>
                  </div>
                    <div class="middle">
                  <asp:Panel ID="PnlDependents" runat="server" Visible="false">
                                    <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                                    </div>
                                    <div style="position: fixed; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white; z-index: 10; top: 161px; left: 448px;">
                                        <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                                            <asp:ImageButton ID="ImgBtnCloseDependents" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" BorderStyle="None" />
                                        </div>
                                        <h3 style="color: black;">Employee Dependents</h3>
                                        <p style="color: darkgray">
                                            <asp:Literal ID="Lit_dep_employeeName" runat="server"></asp:Literal>
                                        </p>
                                        <br />
                                        <asp:GridView ID="GvDependents" runat="server" AutoGenerateColumns="false" Width="590px" EmptyDataText="No Dependents could be found." CellPadding="4" ForeColor="#333333" GridLines="None" OnRowCancelingEdit="GvDependents_RowCancelingEdit" OnRowEditing="GvDependents_RowEditing" OnRowDeleting="GvDependents_RowDeleting" OnRowUpdating="GvDependents_RowUpdating">
                                            <AlternatingRowStyle BackColor="White" />
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="45px" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="ImgBtnEditDependent" runat="server" ImageUrl="~/images/edit_notes.png" CommandName="Edit" Width="20px" ToolTip="EDIT dependent record." />
                                                        <asp:ImageButton ID="ImgBtnDeleteDependent" runat="server" ImageUrl="~/images/close_box_red.png" CommandName="Delete" Width="20px" ToolTip="Delete dependent record." />
                                                        <asp:ConfirmButtonExtender ID="CbeDelete" runat="server" TargetControlID="ImgBtnDeleteDependent" ConfirmText="Are you sure you want to DELETE this dependent record?"></asp:ConfirmButtonExtender>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:ImageButton ID="ImgBtnCancelDependent" runat="server" ImageUrl="~/images/back-icon.png" Width="20px" CommandName="Cancel" ToolTip="CANCEL editing." />
                                                        <asp:ImageButton ID="ImgBtnSaveDependent" runat="server" ImageUrl="~/images/disk-save.png" Width="20px" CommandName="Update" ToolTip="SAVE changes." />
                                                    </EditItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="45px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="First" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Lbl_dep_Fname" runat="server" Text='<%# Bind("DEPENDENT_FIRST_NAME") %>'></asp:Label>
                                                        <asp:HiddenField ID="Hf_dep_id" runat="server" Value='<%# Bind("DEPENDENT_ID") %>' />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="Txt_dep_Fname" runat="server" Text='<%# Bind("DEPENDENT_FIRST_NAME") %>' Width="75px"></asp:TextBox>
                                                        <asp:HiddenField ID="Hf_dep_id2" runat="server" Value='<%# Bind("DEPENDENT_ID") %>' />
                                                    </EditItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Middle" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Lbl_dep_Mname" runat="server" Text='<%# Bind("DEPENDENT_MIDDLE_NAME") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="Txt_dep_Mname" runat="server" Text='<%# Bind("DEPENDENT_MIDDLE_NAME") %>' Width="75px"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Last" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Lbl_dep_Lname" runat="server" Text='<%# Bind("DEPENDENT_LAST_NAME") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="Txt_dep_Lname" runat="server" Text='<%# Bind("DEPENDENT_LAST_NAME") %>' Width="75px"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="SSN" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Lbl_dep_SSN" runat="server" Text='<%# Bind("DEPENDENT_SSN_MASKED") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="Txt_dep_SSN" runat="server" Text='<%# Bind("DEPENDENT_SSN") %>' Width="75px"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="75px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="DOB" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Lbl_dep_DOB" runat="server" Text='<%# Bind("DEPENDENT_DOB", "{0:MM-dd-yyyy}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="Txt_dep_DOB" runat="server" Text='<%# Bind("DEPENDENT_DOB", "{0:MM-dd-yyyy}") %>' Width="75px"></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="75px" />
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
                                        <br />
                                        <p style="color: darkgray">
                                            <asp:Literal ID="Lit_dep_message" runat="server"></asp:Literal>
                                        </p>
                                    </div>
                                </asp:Panel>
                        
                                <asp:HiddenField ID="HfDummyDep" runat="server" />
                                <asp:ModalPopupExtender ID="MpeDependents" runat="server" TargetControlID="HfDummyDep" OkControlID="ImgBtnCloseDependents" PopupControlID="PnlDependents"></asp:ModalPopupExtender>
                                </div>
       


</asp:Content>
