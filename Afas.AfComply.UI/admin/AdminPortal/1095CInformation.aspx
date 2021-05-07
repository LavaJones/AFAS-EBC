<%@ Page EnableSessionState="ReadOnly" Title="Certify 1095-C Information" Language="C#" 
    MasterPageFile="~/admin/AdminPortal/AdminPortal.Master" AutoEventWireup="true" CodeBehind="1095CInformation.aspx.cs" 
    Inherits="Afas.AfComply.UI.admin.AdminPortal._1095CInformation" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <h2>Step 3. Certify 1095-C Information</h2>
    <br />
      <table style="width: 100%; ">
        <tr>
          <td><label class="lbl3" style="font-weight: bold;">Percent Reviewed</label></td>
          <td><label class="lbl3" style="font-weight: bold;">Total Employees</label></td>
          <td><label class="lbl3" style="font-weight: bold; width: 175px;"># Employees Receiving 1095-c</label></td>
          <td><label class="lbl3" style="color:red; font-weight: bold; align-items:flex-start; width: 171px;" >#1095-c Forms Left to Review</label></td>      
        </tr>
        <tr>
          <td><asp:Label id="lblCurrentUserScore" runat="server"></asp:Label></td>
          <td><asp:Label id="lblTotalEmployees" runat="server"></asp:Label></td>
          <td><asp:Label id="lblEmployeesReceiving" runat="server"></asp:Label></td>
          <td><asp:Label id="lbl1095CFormsLefToReview" runat="server" ForeColor="Red"></asp:Label></td>      
        </tr>
      </table>
    <br />
    <br />
    <br />
    <div style="text-align:right">
    <asp:Label ID="myLabel" runat="server" Text="To edit your 1095-c information in a CSV file, use the links below"></asp:Label>
         <br />
         <table align="right">
            <tr>
                <td><asp:Button runat="server" id="btnExport" Text="Export"/></td>
                <td><asp:Button runat="server" id="btnImport" Text="Import"/></td>
            </tr>
        </table>
    
    </div>
    <br />
    <br />
    <asp:Label id="lblEmployer" Font-Bold="true" Font-Size="X-Large" Text="ABC Employer" runat="server" />
    <br />
    <br />
    <asp:Label id="lblSearchAndFilter"  Font-Size="Large" Text="Search and Filter" runat="server" />
    <br />
    <br />
     <table style="width: 100%; ">
         <tr>
             <td style="width: 458px"><label style="font-size: medium">Employee Search</label></td>
             <td><label style="font-size: medium">Filter By</label></td>
         </tr>
         <tr>
             <td style="width: 458px">
                  <table>
                      <tr>
                          <td style="width: 292px"><label style="font-size: small">Enter Employee's Last Name or SSN</label></td>
                          <td></td>
                      </tr>
                      <tr>
                          <td style="width: 292px"><asp:TextBox runat="server" ID="txtFilterBy" Width="227px" ></asp:TextBox></td>
                          <td><asp:Button runat="server" ID="btnSearchEmployees" Text="Search Employees" /></td>
                      </tr>
                  </table>
             </td>
             <td >
                  <table style="width: 558px; height: 69px;">
                      <tr>
                          <td style="width: 149px"><label style="font-size: small">Calender Year</label></td>
                          <td style="width: 252px"><label style="font-size: small">Preliminary 1095-C Results</label></td>
                          <td><label style="font-size: small">Employee Class</label></td>
                      </tr>
                      <tr>
                          <td style="width: 149px"><asp:DropDownList runat="server" ID="ddlCalenderYear" Height="16px" Width="124px"></asp:DropDownList></td>
                          <td style="width: 252px"><asp:DropDownList runat="server" ID="ddlPreliminary1095CResults" Height="16px" Width="210px"></asp:DropDownList></td>
                          <td><asp:DropDownList runat="server" ID="ddlEmployeeClass" Height="16px" Width="146px"></asp:DropDownList></td>
                      </tr>
                     
                  </table>
             </td>
         </tr>
         </table>
     <div style="text-align:right">
          <asp:Button runat="server" ID="btnFilter" Text="Filter" Width="141px" />
     </div>
   

     <asp:GridView ID="GridView1" runat="server" ShowHeaderWhenEmpty="true" AllowPaging="True" 
            AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="CustomerID" 
           style="width: 100%;" PageSize="20" > 
            <AlternatingRowStyle BackColor="White" />  
            <Columns>  
                <asp:TemplateField HeaderText="Reviewed">  
                    <EditItemTemplate>  
                        <asp:CheckBox ID="CheckBox1" runat="server" />  
                    </EditItemTemplate>  
                    <ItemTemplate>  
                        <asp:CheckBox ID="CheckBox1" runat="server" />  
                    </ItemTemplate>  
                </asp:TemplateField> 
                <asp:TemplateField HeaderText="Receiving 1095-c">  
                    <EditItemTemplate>  
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Receiving1095c") %>'></asp:TextBox>  
                    </EditItemTemplate>  
                    <ItemTemplate>  
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("Receiving1095c") %>'></asp:Label>  
                    </ItemTemplate>  
                </asp:TemplateField>  
                <asp:TemplateField HeaderText="Name">  
                    <EditItemTemplate>  
                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("name") %>'></asp:TextBox>  
                    </EditItemTemplate>  
                    <ItemTemplate>  
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("name") %>'></asp:Label>  
                    </ItemTemplate>  
                </asp:TemplateField>  
                <asp:TemplateField HeaderText="SSN">  
                    <EditItemTemplate>  
                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("SSN") %>'></asp:TextBox>  
                    </EditItemTemplate>  
                    <ItemTemplate>  
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("SSN") %>'></asp:Label>  
                    </ItemTemplate>  
                </asp:TemplateField>  
                <asp:TemplateField HeaderText="Address">  
                    <EditItemTemplate>  
                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Address") %>'></asp:TextBox>  
                    </EditItemTemplate>  
                    <ItemTemplate>  
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("Address") %>'></asp:Label>  
                    </ItemTemplate>  
                </asp:TemplateField>  
                <asp:TemplateField HeaderText="City">  
                    <EditItemTemplate>  
                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("city") %>'></asp:TextBox>  
                    </EditItemTemplate>  
                    <ItemTemplate>  
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("city") %>'></asp:Label>  
                    </ItemTemplate>  
                </asp:TemplateField>  
                 <asp:TemplateField HeaderText="State">  
                    <EditItemTemplate>  
                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("State") %>'></asp:TextBox>  
                    </EditItemTemplate>  
                    <ItemTemplate>  
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("State") %>'></asp:Label>  
                    </ItemTemplate>  
                </asp:TemplateField> 
                 <asp:TemplateField HeaderText="Zip">  
                    <EditItemTemplate>  
                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("Zip") %>'></asp:TextBox>  
                    </EditItemTemplate>  
                    <ItemTemplate>  
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("Zip") %>'></asp:Label>  
                    </ItemTemplate>  
                </asp:TemplateField> 
                 <asp:TemplateField HeaderText="Hire Date">  
                    <EditItemTemplate>  
                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("HireDate") %>'></asp:TextBox>  
                    </EditItemTemplate>  
                    <ItemTemplate>  
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("HireDate") %>'></asp:Label>  
                    </ItemTemplate>  
                </asp:TemplateField> 
                 <asp:TemplateField HeaderText="Term Date">  
                    <EditItemTemplate>  
                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("TermDate") %>'></asp:TextBox>  
                    </EditItemTemplate>  
                    <ItemTemplate>  
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("TermDate") %>'></asp:Label>  
                    </ItemTemplate>  
                </asp:TemplateField> 
                 
            </Columns>  
           <EmptyDataTemplate>No Record Available</EmptyDataTemplate>  
            <FooterStyle BackColor="#CCCC99" />  
            <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />  
            <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />  
            <RowStyle BackColor="#F7F7DE" />  
            <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />  
            <SortedAscendingCellStyle BackColor="#FBFBF2" />  
            <SortedAscendingHeaderStyle BackColor="#848384" />  
            <SortedDescendingCellStyle BackColor="#EAEAD3" />  
            <SortedDescendingHeaderStyle BackColor="#575357" />  
        </asp:GridView>  
        <asp:CheckBox ID="CheckBox1" runat="server" Text="Check ALL"/>
    <br />
    <br />
    <label style="font-size: small">DISCLAIMER: As Agreed upon in the Service Agreement as an Employer You Are Ultimately Responsible for the Final Review and Approval of the Data Collected via the System. AFAS Is Entitled to Rely upon the Accuracy and Completeness of Information Provided to AFAS by the Employer, or on Behalf of Employer, Regardless of the Form of the Information (E.G., Oral, Written, Electronic, Etc.). AFAS Is Not Responsible for Negative Consequences Resulting from Inaccurate, Incomplete, or Voluntary Overrides, Etc. Information Provided to AFAS by the Employer, or on Behalf of Employer.</label>
     <br />
     <br />
     <asp:CheckBox ID="CheckBox2" runat="server"/>
     <label style="font-size: small">As an Authorized Agent of the Employer, I Attest That I Have Reviewed and I Approve the Submission of This Data for 1095 Production and Submission to the IRS.</label>
     <br />
    <asp:Button runat="server" ID="Button1" Text="Finalize 1095-C Forms" />

</asp:Content>
