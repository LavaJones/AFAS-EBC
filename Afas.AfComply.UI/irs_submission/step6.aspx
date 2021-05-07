<%@ Page Title="" Language="C#" MasterPageFile="~/irs_submission/irs_submission.Master" AutoEventWireup="true" CodeBehind="step6.aspx.cs" Inherits="step6" %>
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
                <asp:UpdatePanel ID="upDGE" runat="server" style="padding-left:25%;">
                    <ContentTemplate>

                        <h3>Step D: Affordability Safe Harbor</h3>
                        <p>
                            Employees receiving a 1095-C form will be assigned an affordability safe harbor code, if applicable, using the employee class information stored in <%= Branding.ProductName %>.
                            The three affordability safe harbors are:
                        </p>
                        <p style="padding-left: 20px">
                            1)	The Form W-2 wages safe harbor – Code 2F
                 
                            <br />
                            2)	The Federal Poverty Line safe harbor – Code 2G
                 
                            <br />
                            3)	The Rate of Pay safe harbor – Code 2H 
             
                        </p>
                        <p>
                            (More information about eligibility to claim the affordability safe harbors is available in the detailed instruction guide.) By selecting “Yes” below, you verify that you have reviewed and confirmed the affordability safe harbors assigned to each employee classification in <%= Branding.ProductName %>, and that all employees have been assigned to the correct employee class.
                        </p>
                        <br />

                        I verify that I have reviewed and confirmed the affordability safe harbor information stored in <%= Branding.ProductName %>. <br />
             
                        <asp:DropDownList ID="Ddl_step6_ash" runat="server" CssClass="ddl2" AutoPostBack="true" OnSelectedIndexChanged="Ddl_step6_ash_SelectedIndexChanged">
                            <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                            <asp:ListItem Text="No" Value="false"></asp:ListItem>
                            <asp:ListItem Text="Select" Value="select" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                        <br />
                        <span style="background-color: yellow">
                            <asp:Literal ID="lit_step6_message" runat="server" Text=""></asp:Literal>
                        </span>
                        <br />
                        <br />
                          <asp:Button ID="Btn_Prev" runat="server" Text="PREVIOUS" CssClass="btn" Font-Bold="true" Width="10%" Height="30px" Enabled="true" OnClick="Btn_Previous_Click" />
                        <asp:Button ID="Btn_Next" runat="server" Text="NEXT" CssClass="btn" Font-Bold="true" Width="10%" Height="30px" Enabled="false" OnClick="Btn_Next_Click" />
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

