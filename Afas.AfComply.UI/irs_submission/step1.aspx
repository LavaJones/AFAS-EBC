<%@ Page Title="" Language="C#" MasterPageFile="~/irs_submission/irs_submission.Master" AutoEventWireup="true" CodeBehind="step1.aspx.cs" Inherits="step1" %>
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
                        <h3>Step A: Designated Government Entity</h3>
                        <p> A Designated Government Entity (DGE) is an entity that is willing and has agreed to perform ACA reporting on behalf of another entity.<br />
                            Designating another government entity to perform ACA reporting must be done in writing and requires that you follow very specific rules.<br />
                            Here you will answer "Yes" or "No" to the question “Is your group a designated government entity?”<br />
                            For most employers, including most government employers, this answer is a “No”.<br />
                            If you have not gone through the process to agree to be a DGE and complete reporting on behalf of other entities, answer “No” here.<br />
                            <br />
                          <b>  If you believe you may be a Designated Government Entity, contact your consultant before proceeding with this step.</b>
</p>












                        Is your group a designated government entity?
             
                        <asp:DropDownList ID="Ddl_step1_DGE" runat="server" CssClass="ddl2" AutoPostBack="true" OnSelectedIndexChanged="Ddl_step1_DGE_SelectedIndexChanged">
                            <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                            <asp:ListItem Text="No" Value="false"></asp:ListItem>
                            <asp:ListItem Text="Select" Value="select" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                        <br style="clear: both" />
                        <asp:Panel ID="PnlDGE" runat="server" Visible="false">
                            <h4>Enter DGE Info</h4>
                               <p>
                            <asp:Label ID="lblName" runat="server" Text="Name:" CssClass="lbl"></asp:Label>
                            <asp:TextBox ID="Txt_step1_DGEName" runat="server" CssClass="txt3"></asp:TextBox>
                            </p>
                            <p>
                            <asp:Label ID="Label1" runat="server" Text="EIN:" CssClass="lbl"></asp:Label>
                            <asp:TextBox ID="Txt_step1_EIN" runat="server" CssClass="txt3"></asp:TextBox>
                             </p>
                            <p>
                            <asp:Label ID="Label2" runat="server" Text="Address:" CssClass="lbl"></asp:Label>
                            <asp:TextBox ID="Txt_step1_Addresss" runat="server" CssClass="txt3"></asp:TextBox>

                            </p>
                            <p>
                            <asp:Label ID="Label3" runat="server" Text="City:" CssClass="lbl"></asp:Label>
                            <asp:TextBox ID="Txt_step1_City" runat="server" CssClass="txt3"></asp:TextBox>
                            </p>
                            <p>
                            <asp:Label ID="Label4" runat="server" Text="State:" CssClass="lbl"></asp:Label>
                            <asp:DropDownList ID="Ddl_step1_State" runat="server" CssClass="ddl2"></asp:DropDownList>
                            </p>
                            <p>
                            <asp:Label ID="Label5" runat="server" Text="Zip:" CssClass="lbl"></asp:Label>
                            <asp:TextBox ID="Txt_step1_Zip" runat="server" CssClass="txt3"></asp:TextBox>
                            </p>
                            <p>
                            <asp:Label ID="Label6" runat="server" Text="First Name:" CssClass="lbl"></asp:Label>
                            <asp:TextBox ID="Txt_step1_Fname" runat="server" CssClass="txt3"></asp:TextBox>
                             </p>
                            <p>
                            <asp:Label ID="Label8" runat="server" Text="Last Name:" CssClass="lbl"></asp:Label>
                            <asp:TextBox ID="Txt_step1_Lname" runat="server" CssClass="txt3"></asp:TextBox>
                            </p>
                            <p>
                            <asp:Label ID="Label7" runat="server" Text="Phone:" CssClass="lbl"></asp:Label>
                            <asp:TextBox ID="Txt_step1_Phone" runat="server" CssClass="txt3"></asp:TextBox>
                             </p>
                        </asp:Panel>
                        <br />
                        <br />
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

    

