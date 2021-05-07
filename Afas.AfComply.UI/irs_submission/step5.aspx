<%@ Page Language="C#" MasterPageFile="~/irs_submission/irs_submission.Master" AutoEventWireup="true" CodeBehind="step5.aspx.cs" Inherits="step5" %>
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
                        <h3>Step C: Data Verification</h3>
                        <p>You have already confirmed during the Confirmation stage that your data files are complete and loaded into AFcomply.<br />
                           This step is simply where you are confirming that you completed that review.<br />
                           Please note that if you do not confirm the above information is complete before continuing with this process, your IRS forms may be incorrect or incomplete.<br />
Here you will answer "Yes" or "No" to the statement “I verify that I have loaded all necessary data files into AFcomply for the measurement/reporting period(s) applicable to my organization”.


                        </p>
                       


                        I verify that I have loaded all necessary data files into <%= Branding.ProductName %> for the measurement/reporting period(s) applicable to my organization.<br />
             
                        <asp:DropDownList ID="Ddl_step6_unpaid" runat="server" CssClass="ddl2" AutoPostBack="true" OnSelectedIndexChanged="Ddl_step6_unpaid_SelectedIndexChanged">
                            <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                            <asp:ListItem Text="No" Value="false"></asp:ListItem>
                            <asp:ListItem Text="Select" Value="select" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                        <br />
                        <br />
                        <span style="background-color: yellow;">
                            <asp:Literal ID="lit_step5_message" runat="server"></asp:Literal>
                        </span>
                        <br />
                        <br />
                         <asp:Button ID="Btn_Prev" runat="server" Text="PREVIOUS" CssClass="btn" Font-Bold="true" Width="10%" Height="30px" Enabled="true" OnClick="Btn_Previous_Click" />
                        <asp:Button ID="Btn_Next" runat="server" Text="NEXT" CssClass="btn" Font-Bold="true" Width="15%" Height="30px" Enabled="false" OnClick="Btn_Next_Click" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upDGE" DynamicLayout="true" DisplayAfter="500">
                    <ProgressTemplate>
                        <div style="position: fixed; top: 0; left: 0; background-color: white; width: 100%; height: 100%; opacity: .85; filter: alpha(opacity=85); -moz-opacity: 0.85; text-align: center;">
                            <div style="position: relative; margin-left: auto; margin-right: auto; background-color: white; padding-top: 100px;">
                                <h4>Saving the form data .....</h4>
                                <asp:Image ID="ImgSearching" runat="server" ImageUrl="~/design/icon-loading-animated.gif" />
                            </div>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
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
