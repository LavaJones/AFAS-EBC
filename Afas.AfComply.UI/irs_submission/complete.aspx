<%@ Page Title="" Language="C#" MasterPageFile="~/irs_submission/irs_submission.Master"  AutoEventWireup="true" CodeBehind="complete.aspx.cs" Inherits="complete" %>
<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="bodycontent" runat="server">


    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
  <link rel="stylesheet" type="text/css" href="/questionnaire.css" />
    
    <asp:HiddenField ID="HfUserName" runat="server" />
    <asp:HiddenField ID="HfDistrictID" runat="server" />


   
        <div id="container">
            
           
            <div id="content" style="padding-left:25%;">
                <h3>Review Steps</h3>
                <p>This summary screen displays all of your responses to the Questionnaire.<br />
                   Review and confirm your responses,
                   then choose the  “Complete Questionnaire” button.</p>
                Step A:
               
                 Are you a Designated Government Entity?
     
                <br />
               
                <asp:DropDownList ID="Ddl_step1" runat="server" CssClass="ddl2" AutoPostBack="true" Enabled="false">
                    <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                    <asp:ListItem Text="No" Value="false"></asp:ListItem>
                    <asp:ListItem Text="Select" Value="select" Selected="True"></asp:ListItem>
                </asp:DropDownList>
                <br />
                <br />
                Step B: Are you a member of an Aggregated ALE Group?
     
                <br />
                <asp:DropDownList ID="Ddl_step2" runat="server" CssClass="ddl2" AutoPostBack="true" Enabled="false">
                    <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                    <asp:ListItem Text="No" Value="false"></asp:ListItem>
                    <asp:ListItem Text="Select" Value="select" Selected="True"></asp:ListItem>
                </asp:DropDownList>
                <br />
                <br />

                Step C: Did you verify all data has been entered in the system?
     
                <br />
                <asp:DropDownList ID="Ddl_step5" runat="server" CssClass="ddl2" AutoPostBack="true" Enabled="false">
                    <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                    <asp:ListItem Text="No" Value="false"></asp:ListItem>
                    <asp:ListItem Text="Select" Value="select" Selected="True"></asp:ListItem>
                </asp:DropDownList>
                <br />
                <br />
                Step D: Did you read and understand the Affordability Safe Harbor defaults?
     
                <br />
                <asp:DropDownList ID="Ddl_step6" runat="server" CssClass="ddl2" AutoPostBack="true" Enabled="false">
                    <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                    <asp:ListItem Text="No" Value="false"></asp:ListItem>
                    <asp:ListItem Text="Select" Value="select" Selected="True"></asp:ListItem>
                </asp:DropDownList>
                <br />
                <br />
                 <asp:Button ID="Btn_Prev" runat="server" Text="PREVIOUS" CssClass="btn" Font-Bold="true" Width="10%" Height="30px" Enabled="true" OnClick="Btn_Previous_Click" />
                <asp:Button ID="Btn_Next" runat="server" Text="COMPLETE QUESTIONNARIE" CssClass="btn" Font-Bold="true" Width="18%" Height="30px" Enabled="false" OnClick="Btn_Next_Click" />
                <br />
                <asp:Label ID="LblMessage" runat="server" Font-Bold="true" Font-Size="15px"></asp:Label>

                 <asp:HiddenField ID="HfDummyTrigger" runat="server" />
                <asp:Panel ID="PnlMessage" runat="server">
                    <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
                    </div>
                    <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white; z-index: 10">
                        <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                          <asp:ImageButton ID="ImgBtnClose" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" BorderStyle="None" />
                        </div>
                        <h3 style="color: black;">Complete Message</h3>
                        <p style="color: darkgray">
                           Thank you for completing the Questionaire. Please give us 24 hours to process your answers.
                        </p>
                        <br />
                    </div>
                </asp:Panel>

                <asp:ModalPopupExtender ID="MpeWebMessage" runat="server" TargetControlID="HfDummyTrigger" OkControlID="ImgBtnClose" PopupControlID="PnlMessage"></asp:ModalPopupExtender>
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
