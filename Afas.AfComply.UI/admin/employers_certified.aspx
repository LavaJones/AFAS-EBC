<%@ Page EnableSessionState="ReadOnly" Title="Release 1095 Edits For CASS/Print" Language="C#" 
    MasterPageFile="AdminPortal/AdminPortal.Master" AutoEventWireup="true" CodeBehind="employers_certified.aspx.cs" 
    Inherits="Afas.AfComply.UI.admin.employers_certified" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    
    <h2>Release 1095 Edits For CASS/Print: <%= DateTime.Now.AddYears(-1).Year %></h2>
    <p>Select the checkbox of the employers that you want approve for release to the CASS and/or Print Process. Click the Approve Companies button once you are done</p>
    <br />

    <asp:GridView ID="gvEmployersCertified" runat="server"  AutoGenerateColumns="False" DataKeyNames="EmployerTaxYearTransmissionId"
        BorderWidth="1px" ForeColor="White" BackColor="#00ACE6" CellPadding="3" CellSpacing="2" BorderStyle="None" BorderColor="Black" Width="100%" PageSize="100">
        <Columns>
            <asp:TemplateField ItemStyle-Width="40px">
                <HeaderTemplate>
                    <asp:CheckBox ID="chkboxSelectAll" runat="server" onclick="CheckAllEmp(this);" />
                </HeaderTemplate>
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                <ItemTemplate>
                    <asp:CheckBox ID="chkEmp" runat="server"></asp:CheckBox>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:BoundField  DataField="EmployerTaxYearTransmissionId" Visible="false"/>
            <asp:BoundField HeaderText="Name" DataField="name" SortExpression="name"></asp:BoundField>
            <asp:BoundField HeaderText="Address" DataField="address" SortExpression="address"></asp:BoundField>
            <asp:BoundField HeaderText="City" DataField="city" SortExpression="city"></asp:BoundField>
            <asp:BoundField HeaderText="State" DataField="state" SortExpression="state"></asp:BoundField>
            <asp:BoundField HeaderText="Zip" DataField="zip" SortExpression="zip"></asp:BoundField>
                    
            <asp:BoundField HeaderText="Billing Address" DataField="bill_address" SortExpression="bill_address"></asp:BoundField>
            <asp:BoundField HeaderText="Billing City" DataField="bill_city" SortExpression="bill_city"></asp:BoundField>
            <asp:BoundField HeaderText="Billing State" DataField="bill_state" SortExpression="bill_state"></asp:BoundField>
            <asp:BoundField HeaderText="Billing Zip" DataField="bill_zip" SortExpression="bill_zip"></asp:BoundField>
        </Columns>
        <SelectedRowStyle ForeColor="White" Font-Size="Small" Font-Bold="True" BackColor="#008DBC"></SelectedRowStyle>
        <RowStyle ForeColor="Black" Font-Size="Small" BackColor="White" ></RowStyle>
    </asp:GridView>
    
    <br />
    <asp:Button ID="btnApprove" runat="server" Text="Approve Companies" OnClick="btnApprove_Click" />
    <asp:Label ID="lblMsg" runat="server" Text=""></asp:Label>

    <script type="text/javascript" >
        function CheckAllEmp(Checkbox) {
            var GridVwHeaderChckbox = document.getElementById("<%= gvEmployersCertified.ClientID %>");
            for (i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
                GridVwHeaderChckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
            }
        }
    </script>

</asp:Content>

