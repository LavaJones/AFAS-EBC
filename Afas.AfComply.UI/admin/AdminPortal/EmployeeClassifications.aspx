<%@ Page EnableSessionState="ReadOnly" Title="Create Employer Classification" Language="C#"
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="EmployeeClassifications.aspx.cs"
    Inherits="Afas.AfComply.UI.admin.AdminPortal.EmployeeClassifications" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">

    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="UpClass" runat="server">
        <ContentTemplate>
    <h2>Manage Employee Classifications</h2>
    <br />
    <label class="lbl3">Filter Employer List</label>
    <asp:panel ID="PnlFilter" runat="server" DefaultButton="BtnSearchEmployer">
        <asp:TextBox ID="TxtEmployerSearch" runat="server" CssClass="txt3"></asp:TextBox>
        <asp:Button ID="BtnSearchEmployer" runat="server" Text="Filter" CssClass="btn" OnClientClick="this.focus()" OnClick="BtnSearchEmployer_Click" />
    </asp:panel>
    <br />
    <label class="lbl3">Select Employer</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" AutoPostBack="true" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged"></asp:DropDownList>
    <br />
    <br />
    <hr />
    <br />
    <h3>New Classification</h3>
    <label class="lbl3">Enter Description</label>
    <asp:TextBox ID="TxtEmployeeClass" runat="server" CssClass="txt3"></asp:TextBox>
    <br />
    <label class="lbl3">Affordability Safe Harbor</label>
    <asp:DropDownList ID="DdlASH" runat="server" CssClass="ddl2"></asp:DropDownList>
    <br />
    <label class="lbl3">Waiting Period</label>
    <asp:DropDownList ID="DdlWaitingPeriod" runat="server" CssClass="ddl2"></asp:DropDownList>
    <br />
    <label class="lbl3">Default Offer of Coverage</label>
    <asp:DropDownList ID="DdlOoc" runat="server" CssClass="ddl2"></asp:DropDownList>
    <br />
    <br />
    <asp:Button ID="BtnSaveNew" runat="server" CssClass="btn" Text="Submit" OnClick="BtnSaveNew_Click" />
    <br />
    <asp:Label ID="lblMsg" runat="server"></asp:Label>
    <br />
    <hr />
    <br />
    <asp:GridView ID="GvClassifications" runat="server" AutoGenerateColumns="false"
        BackColor="White" BorderColor="#336666" BorderStyle="Double" BorderWidth="3px" CellPadding="4" GridLines="Horizontal"
        EmptyDataText="No Errors Exist"
        OnRowEditing="GvClassifications_RowEditing"
        OnRowCancelingEdit="GvClassifications_RowCancelingEdit"
        OnRowUpdating="GvClassifications_RowUpdating" OnRowDataBound="GvClassifications_RowDataBound" AlternatingRowStyle-HorizontalAlign="Left" RowStyle-HorizontalAlign="Left" EditRowStyle-HorizontalAlign="Left">
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
            <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="75px">
                <ItemTemplate>
                    <asp:ImageButton ID="ImgBtnEdit" runat="server" CommandName="Edit" ImageUrl="~/images/edit_notes.png" Height="25px" Style="float: right;" />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:ImageButton ID="ImgBtnUpdate" runat="server" CommandName="Update" ImageUrl="~/images/disk-save.png" Height="25px" />
                    <asp:ImageButton ID="ImgBtnCancel" runat="server" CommandName="Cancel" ImageUrl="~/images/error.png" Height="25px" />
                </EditItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Classification Name" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:Literal ID="Lit_gv_name" runat="server" Text='<%# Eval("CLASS_DESC") %>'></asp:Literal>
                    <asp:HiddenField ID="Hf_gv_id" runat="server" Value='<%# Eval("CLASS_ID") %>' />
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:HiddenField ID="Hf_Edit_id" runat="server" Value='<%# Eval("CLASS_ID") %>' />
                    <asp:TextBox ID="Txt_Edit_name" runat="server" Text='<%# Eval("CLASS_DESC")  %>' CssClass="txt"></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Safe Harbor" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:Literal ID="Lit_gv_harbor" runat="server" Text='<%# Eval("CLASS_AFFORDABILITY_CODE") %>'></asp:Literal>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:HiddenField ID="Hf_Edit_ASH" runat="server" Value='<%# Eval("CLASS_AFFORDABILITY_CODE") %>'></asp:HiddenField>
                    <asp:DropDownList ID="Ddl_Edit_ASH" runat="server" CssClass="ddl"></asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Waiting Period" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:HiddenField ID="Hf_gv_waitingPeriod" runat="server" Value='<%# Eval("CLASS_WAITING_PERIOD_ID") %>' />
                    <asp:DropDownList ID="Ddl_gv_waitingPeriod" runat="server" CssClass="ddl2" Enabled="false"></asp:DropDownList>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:HiddenField ID="Hf_Edit_waitingPeriod" runat="server" Value='<%# Eval("CLASS_WAITING_PERIOD_ID") %>' />
                    <asp:DropDownList ID="Ddl_Edit_waitingPeriod" runat="server" CssClass="ddl2"></asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="OOC" HeaderStyle-Width="75px" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:Literal ID="Lit_gv_ooc" runat="server" Text='<%# Eval("CLASS_DEFAULT_OOC") %>'></asp:Literal>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:HiddenField ID="Hf_Edit_ooc" runat="server" Value='<%# Eval("CLASS_DEFAULT_OOC") %>' />
                    <asp:DropDownList ID="Ddl_Edit_ooc" runat="server" CssClass="ddl"></asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Entity Status" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:HiddenField ID="Hf_gv_entityStatus" runat="server" Value='<%# Eval("CLASS_ENTITY_STATUS") %>' />
                    <asp:DropDownList ID="Ddl_gv_entityStatus" runat="server" CssClass="ddl" Enabled="false"></asp:DropDownList>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:HiddenField ID="Hf_Edit_entityStatus" runat="server" Value='<%# Eval("CLASS_ENTITY_STATUS") %>' />
                    <asp:DropDownList ID="Ddl_Edit_entityStatus" runat="server" CssClass="ddl"></asp:DropDownList>
                </EditItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpClass" DynamicLayout="true" DisplayAfter="500">
                    <ProgressTemplate>
                        <div style="position: fixed; top: 0; left: 0; background-color: white; width: 100%; height: 100%; opacity: .85; filter: alpha(opacity=85); -moz-opacity: 0.85; text-align: center;">
                            <div style="position: relative; margin-left: auto; margin-right: auto; background-color: white; padding-top: 100px;">
                                <h4>Processing your data.</h4>
                                <asp:Image ID="ImgSearching" runat="server" ImageUrl="~/design/icon-loading-animated.gif" />
                            </div>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
</asp:Content>
