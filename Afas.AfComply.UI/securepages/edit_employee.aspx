<%@ Page Title="" Language="C#" MasterPageFile="~/securepages/SecurePages.Master" AutoEventWireup="true" CodeBehind="edit_employee.aspx.cs" Inherits="edit_employee" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="bodycontent" runat="server">
    <link rel="stylesheet" type="text/css" href="/edit_employee.css" />
    <link rel="stylesheet" type="text/css" href="/leftnav.css" />
    <asp:HiddenField ID="HfUserName" runat="server" />
    <asp:HiddenField ID="HfDistrictID" runat="server" />
    <div id="content" style="padding-left: 50%; padding-right: 50%;">
        <h2>Update Employee Information</h2>
        <br />

        <label class="lbl3">Search for Employee</label>
        <asp:TextBox ID="TxtEmployee" runat="server" CssClass="txt3"></asp:TextBox>
        <br />
        <br />

        <asp:Button ID="BtnFindEmployees" runat="server" Text="Search for Employee" CssClass="btn" Width="135px" OnClick="BtnFindEmployees_Click" />
        <br />
        <br />

        <asp:Button ID="BtnResetEmployees" runat="server" Text="Reset Employees" CssClass="btn" Width="135px" OnClick="BtnResetEmployees_Click" />
        <br />
        <br />

        <label class="lbl3">Select An Employee</label>
        <asp:DropDownList ID="DdlEmployee" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployee_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
        <br />
        <br />

        <h3>Profile</h3>

        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <p>
                    <label class="lbl">First Name:</label>
                    <asp:TextBox ID="TxtFirstName" runat="server" CssClass="txt3"></asp:TextBox>
                </p>
                <p>
                    <label class="lbl">Middle Name:</label>
                    <asp:TextBox ID="TxtMiddleName" runat="server" CssClass="txt3"></asp:TextBox>
                </p>
                <p>
                    <label class="lbl">Last Name:</label>
                    <asp:TextBox ID="TxtLastName" runat="server" CssClass="txt3"></asp:TextBox>
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
                    <asp:DropDownList ID="DdlEmployeeState" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployeeState_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                </p>
                <p>
                    <label class="lbl">Zip:</label>
                    <asp:TextBox ID="TxtEmployerZip" runat="server" CssClass="txt3" MaxLength="5"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="regTxtEmployerZip" ControlToValidate="TxtEmployerZip" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+"></asp:RegularExpressionValidator>
                </p>
                <p>
                    <label class="lbl">SSN:</label>
                    <asp:TextBox ID="TxtSSN" runat="server" CssClass="txt3" MaxLength="9"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="regTxtSSN" ControlToValidate="TxtSSN" runat="server" ErrorMessage="Only Numbers allowed" ValidationExpression="\d+"></asp:RegularExpressionValidator>
                </p>
                <p>
                    <label class="lbl">DOB:</label>
                    <asp:TextBox ID="TxtDOB" runat="server" CssClass="txt3" placeholder="mm/dd/yyyy" MaxLength="10"></asp:TextBox>
                </p>
                <p>
                    <label class="lbl">Current Date:</label>
                    <asp:TextBox ID="TxtCurrDate" runat="server" CssClass="txt3" placeholder="mm/dd/yyyy" MaxLength="10" ReadOnly="true" Enabled="false"></asp:TextBox>
                </p>
                <p>
                    <label class="lbl">Hire Date:</label>
                    <asp:TextBox ID="TxtHireDate" runat="server" CssClass="txt3" placeholder="mm/dd/yyyy" MaxLength="10" ReadOnly="true" Enabled="false"></asp:TextBox>
                </p>
                <p>
                    <label class="lbl">Term Date:</label>
                    <asp:TextBox ID="TxtTermDate" runat="server" CssClass="txt3" placeholder="mm/dd/yyyy" MaxLength="10"></asp:TextBox>
                </p>
                <p>
                    <label class="lbl">IMP End:</label>
                    <asp:TextBox ID="TxtImpEnd" runat="server" CssClass="txt3" placeholder="mm/dd/yyyy" MaxLength="10" ReadOnly="true" Enabled="false"></asp:TextBox>
                </p>
                <p>
                    <label class="lbl">Employee Type:</label>
                    <asp:DropDownList ID="DdlEmployeeType" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployeeType_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                </p>
                <p>
                    <label class="lbl">HR Status:</label>
                    <asp:DropDownList ID="DdlHireStatus" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlHireStatus_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                </p>
                <p>
                    <label class="lbl">Classification:</label>
                    <asp:DropDownList ID="DdlClassification" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlClassification_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                </p>
                <p>
                    <label class="lbl">ACA Status:</label>
                    <asp:DropDownList ID="DdlAcaStatus" runat="server" CssClass="ddl2" AutoPostBack="true" OnSelectedIndexChanged="DdlAcaStatus_SelectedIndexChanged"></asp:DropDownList>
                </p>

                <label class="lbl">Employee #:</label>
                <asp:TextBox ID="TxtExtID" runat="server" CssClass="txt3"></asp:TextBox>
                <br />

                <asp:HiddenField ID="HfAcaStatus" runat="server" />

                <asp:ModalPopupExtender ID="mpAcaStatus" PopupControlID="pnlMessage" TargetControlID="HfAcaStatus" OkControlID="btnOK" runat="server">
                </asp:ModalPopupExtender>

                <asp:Panel ID="pnlMessage" runat="server">
                    <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                    </div>
                    <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white; z-index: 10">
                        <p>
                            Changing the ACA Status alone will not terminate the employee from the system unless a termination date is provided.  Please enter the termination date in the Term Date field above.

                        </p>
                        <p>
                            <asp:Button ID="btnOK" Text="OK" runat="server" /></p>
                    </div>

                </asp:Panel>

            </ContentTemplate>

            <Triggers>

                <asp:AsyncPostBackTrigger ControlID="DdlEmployeeState" />
            </Triggers>

        </asp:UpdatePanel>

        <br />
        <br />

        <asp:Button ID="BtnSave" runat="server" CssClass="btn" Text="Save" OnClick="BtnSave_Click" />
        <br />

        <asp:Label ID="lblMsg" runat="server"></asp:Label>

    </div>
    <div id="footer">
        Copyright &copy; <%= Branding.CopyrightYears %> <a href="<%= Branding.CompanyWebSite %>"><%= Branding.CompanyName %></a> - All Rights Reserved
                <br />
    </div>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
</asp:Content>
