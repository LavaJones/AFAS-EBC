<%@ Page EnableSessionState="ReadOnly" Title="Stage 1095 Data" Language="C#" 
    MasterPageFile="AdminPortal.Master" AutoEventWireup="true" CodeBehind="IRSStaging1095.aspx.cs" 
    Inherits="Afas.AfComply.UI.admin.AdminPortal.IRSStaging1095" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" AsyncPostBackTimeout="3600"></asp:ToolkitScriptManager>
    
    <h2>Stage 1095 Data</h2>
    <br />
    <label class="lbl3">Select Employer</label>
    <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged"  AutoPostBack="true"></asp:DropDownList>
    <asp:DropDownList ID="DdlCalendarYear" runat="server" CssClass="ddl">
                                        <asp:ListItem Text="2015" Value="2015" ></asp:ListItem>
                                        <asp:ListItem Text="2016" Value="2016" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="2017" Value="2017" ></asp:ListItem>
                                        <asp:ListItem Text="2018" Value="2018" ></asp:ListItem>
                                    </asp:DropDownList>
    <br />
   <br />

    <h3>IRS Contacts</h3>
    <asp:GridView ID="gvContacts" runat="server" AutoGenerateColumns="false"
        EmptyDataText="There are currently NO IRS Contacts for this employer!" BackColor="White" BorderColor="#336666"
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
            <asp:TemplateField HeaderText="First Name" HeaderStyle-Width="225px" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:Literal ID="LitFName" runat="server" Text='<%# Eval("User_First_Name") %>'></asp:Literal>
                </ItemTemplate>

            </asp:TemplateField>
            <asp:TemplateField HeaderText="Last Name" HeaderStyle-Width="225px" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:Literal ID="LitLName" runat="server" Text='<%# Eval("User_Last_Name") %>'></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Phone" HeaderStyle-Width="225px" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:Literal ID="LitPhone" runat="server" Text='<%# Eval("User_Phone") %>'></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <h3>Finalized Forms</h3>
    <asp:GridView ID="Gv_gv_Finalized" runat="server" AutoGenerateColumns="false"
        EmptyDataText="There are currently NO finalized forms for this employer, safe to push!" BackColor="White" BorderColor="#336666"
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
            <asp:TemplateField HeaderText="Employee Name" HeaderStyle-Width="225px" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:Literal ID="LitFinalizedEmployeeName" runat="server" Text='<%# Eval("EMPLOYEE_FULL_NAME") %>'></asp:Literal>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <h3>Alerts</h3>
    <asp:GridView ID="Gv_gv_Alerts" runat="server" AutoGenerateColumns="false"
        EmptyDataText="There are currently NO ALERTS setup for this employer!" BackColor="White" BorderColor="#336666"
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
            <asp:TemplateField HeaderText="Alert Name" HeaderStyle-Width="225px" HeaderStyle-HorizontalAlign="Left">
                <ItemTemplate>
                    <asp:HiddenField ID="HiddenAlertId" runat="server" Value='<%# Eval("ALERT_TYPE_ID") %>' />
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
     <br />
     <br />
    <h3>Employer's Answers to Questionnaire</h3>
    Step A: Are you a Designated Government Entity?
     
    <br />
    <asp:DropDownList ID="Ddl_step1" runat="server" CssClass="ddl2" Enabled="false">
        <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
        <asp:ListItem Text="No" Value="false"></asp:ListItem>
        <asp:ListItem Text="Select" Value="select" Selected="True"></asp:ListItem>
    </asp:DropDownList>
    <br />
    Step B: Are you a member of an Aggregated ALE Group?
     
    <br />
    <asp:DropDownList ID="Ddl_step2" runat="server" CssClass="ddl2" Enabled="false">
        <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
        <asp:ListItem Text="No" Value="false"></asp:ListItem>
        <asp:ListItem Text="Select" Value="select" Selected="True"></asp:ListItem>
    </asp:DropDownList>
    <br />
    Step C: Did you qualify for Non-Calendar Year Plan Transition Relief?
     
    <br />
    <asp:DropDownList ID="Ddl_step3" runat="server" CssClass="ddl2" Enabled="false">
        <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
        <asp:ListItem Text="No" Value="false"></asp:ListItem>
        <asp:ListItem Text="Select" Value="select" Selected="True"></asp:ListItem>
    </asp:DropDownList>
    <br />
    Step D: Did you verify all data has been entered in the system?
     
    <br />
    <asp:DropDownList ID="Ddl_step5" runat="server" CssClass="ddl2" Enabled="false">
        <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
        <asp:ListItem Text="No" Value="false"></asp:ListItem>
        <asp:ListItem Text="Select" Value="select" Selected="True"></asp:ListItem>
    </asp:DropDownList>
    <br />
    Step E: Did you read and understand the Affordability Safe Harbor defaults?
     
    <br />
    <asp:DropDownList ID="Ddl_step6" runat="server" CssClass="ddl2" Enabled="false">
        <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
        <asp:ListItem Text="No" Value="false"></asp:ListItem>
        <asp:ListItem Text="Select" Value="select" Selected="True"></asp:ListItem>
    </asp:DropDownList>
    <br />

    <asp:Label ID="cofein" runat="server"></asp:Label>
    <br />
    <asp:CheckBox ID="chkConfirm" runat="server" Checked="false" Text="This stages the 1095 data for this client, any previous changes made to the 1095 data is lost!"/>
    <br />

    <asp:Button ID="BtnTransmit" runat="server" Text="Transmit All" OnClick="BtnTransmit_Click" />
    <br />

    <asp:Label ID="lblMsg" runat="server"></asp:Label>    

</asp:Content>