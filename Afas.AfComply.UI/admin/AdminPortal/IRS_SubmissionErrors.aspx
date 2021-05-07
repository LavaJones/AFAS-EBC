<%@ Page EnableSessionState="ReadOnly" Title="Clear Offer Alerts" Language="C#"
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="IRS_SubmissionErrors.aspx.cs"
    Inherits="Afas.AfComply.UI.admin.AdminPortal.IRS_SubmissionErrors" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">


    <h2>Clear Offer Alerts</h2>
    <br />
    <label class="lbl3">Select Employer</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    <br />
    <asp:Label ID="cofein" runat="server"></asp:Label>
    <br />
    <label class="lbl3">Select Transmission Date</label>
    <asp:DropDownList ID="DdlSubmissions" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlSubmissions_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
    
    <br />
    <br />
    <asp:Label ID="lblMsg" runat="server"></asp:Label>

    <br />
    <br />

    <div id="topbox">
        <div id="tbleft" style="padding-bottom: 20px;">
            <h3>Left Column</h3>
        </div>
        <div id="tbright">
            <h3>Export Errors (.csv)</h3>
            <asp:ImageButton ID="ImgBtnExport" runat="server" ImageUrl="~/design/csv-file-icon.png" Height="25px" Width="25px" OnClick="ImgBtnExport_Click" />
        </div>
    </div>
    <br />
    <hr />
    <asp:GridView ID="GvErrors" runat="server" AutoGenerateColumns="false" Width="1025px" AllowPaging="true" AllowSorting="true" PageSize="50" OnSorting="GvErrors_Sorting" OnPageIndexChanging="GvErrors_PageIndexChanging">
        <EditRowStyle BackColor="Yellow" />
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <AlternatingRowStyle BackColor="White" ForeColor="Black" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#EFF3FB" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <SortedAscendingCellStyle BackColor="#F5F7FB" />
        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
        <SortedDescendingCellStyle BackColor="#E9EBEF" />
        <SortedDescendingHeaderStyle BackColor="#4870BE" />
        <Columns>
            <asp:TemplateField HeaderText="ID" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="50px" SortExpression="RE_ID">
                <ItemTemplate>
                    <asp:Label ID="Lbl_rpt_id" runat="server" Text='<%# Eval("RE_ID") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="First Name" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" SortExpression="RE_ERROR_FIRST_NAME">
                <ItemTemplate>
                    <asp:Label ID="Lbl_rpt_fname" runat="server" Text='<%# Eval("RE_ERROR_FIRST_NAME") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Last Name" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" SortExpression="RE_ERROR_LAST_NAME">
                <ItemTemplate>
                    <asp:Label ID="Lbl_rpt_lname" runat="server" Text='<%# Eval("RE_ERROR_LAST_NAME") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Error Code" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" SortExpression="RE_ERROR_CODE">
                <ItemTemplate>
                    <asp:Label ID="Lbl_rpt_error_code" runat="server" Text='<%# Eval("RE_ERROR_CODE") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Message" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="650px" SortExpression="RE_ERROR_TEXT">
                <ItemTemplate>
                    <asp:Label ID="Lbl_rpt_error_text" runat="server" Text='<%# Eval("RE_ERROR_TEXT") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

</asp:Content>
