<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IRSVerification.aspx.cs" Inherits="IRSVerification" %>

<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>




<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" type="text/css" href="/default.css" />
    <link rel="stylesheet" type="text/css" href="/menu.css" />
    <link rel="stylesheet" type="text/css" href="/v_menu.css" />
    <title>IRS Verification Dashboard</title>







            <style>
                p { font-size:14px; margin-bottom:10px;margin-left:10px;}
                .p-flush {margin-left:0;}
                .irs-ver-para > p.irs-ver-para > br {margin-bottom:10px; line-height: 2.0;}
                #Gv_SafeHarbor {background-color:rgba(255,242,1,.25)!important;}
                .irs-button { background-color:#0088B5; padding: 7px 10px; border-radius:5px; width:300px; text-align:center; margin-bottom:0; margin-top:15px;}
                .irs-button > a { color:#ffffff; text-decoration:none;}
            </style>
</head>






<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
        <div id="container">



           
            
            
            
            
             <div id="header" style =" height:90px">
                <a href="default.aspx">
                    <img src="<%= Page.ResolveUrl(Branding.LogoUri) %>" style="height: 70px" alt="<%= Branding.ProductName %> Logo" class="logo" />
                </a>
                <ul id="toplinks">
                    <li>Stability Period: 
                        <asp:HiddenField ID="HfEmployerTypeID" runat="server" />
                        <asp:HiddenField ID="HfUserName" runat="server" />
                    </li>
                    <li>User Name:<asp:Literal ID="LitUserName" runat="server"></asp:Literal></li>
                    <li>
                        <asp:Button ID="BtnLogout" CssClass="btn" runat="server" Text="Log Out" OnClick="BtnLogout_Click" UseSubmitBehavior="false" /></li>

                     </ul>
                <asp:HiddenField ID="HfDistrictID" runat="server" />
            </div>








            <div id="nav2">
                <nav>
                    <ul>
                        <li><a href="default.aspx">Home</a></li>
                        <li><a href="e_find.aspx">Employee</a></li>
                        <li><a href="r_reporting.aspx">Reporting</a></li>
                        <li><a href="s_setup.aspx">Employer Setup</a></li>
                        <li><a href="t_terms.aspx">ACA Terms</a></li>
                        <li><a href="contact.aspx">Help</a></li>
                    </ul>
                </nav>
                <ul class="right">
                    <li></li>
                </ul>
            </div>























            
            <div id="content">
                
                
                
                
                <table border="0" style="border-width: 1px; border-color: white;" >
                    <tr>
                        <td style="width:78%">
                            <h1>STATUS AND ACTION PORTAL</h1>
                        </td>
                        <td style="width:100%" >
<%
 if (null != Session["CurrentDistrict"] && ((employer)Session["CurrentDistrict"]).IrsEnabled == true)
{
%>
 <a href="../Reporting/Verification">2017 STATUS AND ACTION PORTAL</a>
<%
                        
}
%>
<%= demo.getLeftLinks(null != Session["CurrentDistrict"] && ((employer)Session["CurrentDistrict"]).IrsEnabled) %> 





                        </td>
                    </tr>
                </table>






               <h2 style="font-size:18px;">Completing your 1095-C and 1094-C Reporting Process </h2>





                <p>
                    There are three steps left to review, approve, and have your 1095-C Forms mailed to employees and copies filed with the IRS.
                    You can return to this page to check the status as you move through the steps below. 
                </p>







                <p>
                    <span style="font-weight:bold">Helpful Guide:</span> To get started, we strongly encourage you to download and print our 
                    <a href="<%= Feature.IrsInstructionsLink %>">detailed instruction guide</a> and keep it at hand as you complete each of the following steps.
                </p>



                
                
                
                <p>
                    <span  style="font-weight:bold">Important Note about Timing:</span> After Step 2 below, an overnight process must run before you can continue with your certification.
                    All steps must be completed no later than <%= Branding.IrsDeadlineCertify %> in order to ensure your 1095-C forms are mailed by the IRS deadline.
                    As such, we recommend you complete the first two steps (through completion of the Questionnaire linked below) as soon as possible and no later than <%= Branding.IrsDeadlineSetup %>.
                    If you do not complete Steps 1 and 2 by <%= Branding.IrsDeadlineSetup %>, and Step 3 by <%= Branding.IrsDeadlineCertify %>, we cannot guarantee your forms will be mailed by the IRS deadline, 
                    which can result in late filing penalties for your organization.  
                </p>





                
                <table>
                    <colgroup>
                        <col style="width:25%" />
                        <col style="width:25%; border-left:1px solid black; border-right:1px solid black;" />
                        <col style="width:50%" />
                    </colgroup>
                    <thead>
                        <tr style="border-bottom: 1px solid black">
                            <td><span style="font-weight:bold">Step</span></td>
                            <td><span style="font-weight:bold">Status</span></td>
                            <td><span style="font-weight:bold">Instructions</span></td>
                        </tr>
                    </thead>
                    <tbody>




                        <tr style="border-bottom: 1px solid black">

                            <td><span style="font-weight:bold">1. Confirm Data is Correct</span></td>

                            <td><span><asp:Label ID="lblConfirmDataStatus" runat="server" ForeColor="Red" Text="Not Complete"></asp:Label></span></td>


                            <td><p>Go to the <a href="/securepages/IRSConfirmation.aspx">Confirmation Page</a> and follow the instructions to confirm key data is correct and address any open issues.  </p></td>
                        </tr>





                        <tr style="border-bottom: 1px solid black">
                            <td><span style="font-weight:bold">2. Complete Questionnaire</span></td>
                            <td><span><asp:Label ID="lblCompleteQuestionnaireStatus" runat="server" ForeColor="Red" Text="placeholder"></asp:Label></span></td>
                            <td><p>Complete the <a href="/irs_submission/step1.aspx">Questionnaire</a> and submit.  A process must run overnight before you may continue to Step 3.</p></td>
                        </tr>







                        <tr style="border-bottom: 1px solid black">
                            <td><span style="font-weight:bold">3. Certify 1095-C Information</span></td>
                            <td>
                                <span>
                                    <asp:Label ID="lblReviewAndApproveStatus" runat="server" ForeColor="Red" Text="placeholder"></asp:Label>
                                    <br />
                                    <asp:Label ID="lblEtlBuildProcess" runat="server" ForeColor="Red" Visible="false" > 
                                        We are still processing your request to move to Step 3. <br /> 
                                        Once it is complete a link will appear to the right for you to review the information and codes
                                    </asp:Label>
                                </span>
                                <br />
                                <asp:HyperLink ID="lkViewPdfOfCompleted1095C" Visible="false" NavigateUrl="/securepages/1095_PDF_display.aspx" runat="server">View PDF of completed 1095-C Forms</asp:HyperLink>
                            </td>


                            <td>
                                <p>
                                    <asp:Literal ID="litReviewInformationAndCodes" runat="server" Visible="true">Review the information and codes</asp:Literal> that will be shown on your 1095-C forms and correct as necessary, then certify with your approval. After you approve, your 1095-C forms will be sent to print and mailed to employees.<asp:HyperLink ID="lkReviewInformationAndCodes" runat="server" NavigateUrl="/securepages/1095_approval.aspx" Visible="false">Review the information and codes</asp:HyperLink>
                                    </p><p>You can see your list of finalized employees and change their 1095 approval status on <asp:HyperLink ID="lkUnapproval" runat="server" NavigateUrl="/securepages/1095_unapproval.aspx" Visible="false">the detailed approval page</asp:HyperLink><asp:Literal ID="litUnapproval" runat="server" Visible="true">the detailed approval page</asp:Literal>.</p>
                                <% if (Branding.IrsReprintFeeEnabled) { %>
                                <p style="color:yellow; background-color:darkgray"> <span style="color:black;font-weight:bold">Important:</span> If you need to make any changes after submitting the approval for this step, you will be charged a reprocessing and reprinting fee of $2.00 per form per employee.</p>
                                <% } %>

                                <p>
                                    A few days after you submit your certification, a link will be provided on this page for you to view the PDFs of your completed 1095-C Forms.
                                    This is optional and will not impact the mailing of your forms.  
                                </p>
                            
                            </td>




                        </tr>




                        <tr>
                            <td><span style="font-weight:bold">IRS Submission of 1094-C and 1095-C Forms </span></td>
                            <td>
                                <p>Must notify us of any corrections prior to <%= Branding.IrsDeadlineTransmit %></p>
                                <p><asp:HyperLink ID="lkTransmit" NavigateUrl="/securepages/irs_transmit_send.aspx" runat="server">Transmit my forms to the IRS</asp:HyperLink></p>
                            </td>
                            <td>
                                <% if (Branding.Irs1094Enabled) { %>
                                    <p>You can review your <asp:HyperLink ID="hyprlnk1094" NavigateUrl="/securepages/1094_approval.aspx" runat="server">1094-C information</asp:HyperLink> here.</p>
                               <% } %>
                                <p>
                                   Unless we hear from you, 
                                   we will automatically submit your 1094-C transmittal form and copies of your 1095-C forms to the IRS on your behalf prior to the March 31 deadline.
                                   If you have any corrections that need to be made, you MUST notify us by <%= Branding.IrsDeadlineTransmit %> by completing <a href="/securepages/irs_transmit_hold.aspx">this form</a>. 
                                </p>

                                <p>Once your forms have been submitted to the IRS, additional status information will be <asp:HyperLink ID="lkTransmissionStatus" runat="server" NavigateUrl="/securepages/10941095_submission_status.aspx" Visible="false">provided here</asp:HyperLink><asp:Literal ID="litTransmissionStatus" runat="server" Visible="true">provided here</asp:Literal>.</p>
                            
                            </td>
                        </tr>



                    </tbody>
                </table>

        </div>

        <asp:HiddenField ID="HfDummyTrigger" runat="server" />
        <asp:Panel ID="PnlMessage" runat="server">
            <div style="position: fixed; top: 0; left: 0; background-color: black; opacity: 0.8; width: 100%; height: 100%;">
            </div>
            <div style="position: relative; margin-left: auto; padding: 5px; margin-right: auto; border-radius: 10px; width: 600px; height: auto; margin-top: 0px; font-size: 12px; background-color: white; z-index: 10">
                <div style="position: absolute; width: 30px; height: 30px; opacity: 1.0; line-height: 20px; right: -10px; top: -10px; text-align: center;">
                    <asp:ImageButton ID="ImgBtnClose" runat="server" Height="30px" ImageUrl="~/images/close_box_red.png" Width="30px" BorderStyle="None" />
                </div>
                <h3 style="color: black;">Webpage Message</h3>
                <p style="color: darkgray">
                    <asp:Literal ID="LitMessage" runat="server"></asp:Literal>
                </p>
                <br />
            </div>
        </asp:Panel>

        <asp:ModalPopupExtender ID="MpeWebMessage" runat="server" TargetControlID="HfDummyTrigger" OkControlID="ImgBtnClose" PopupControlID="PnlMessage"></asp:ModalPopupExtender>

        <div style="clear: both;">&nbsp;</div>
        <div id="footer">
            Copyright &copy; <%= Branding.CopyrightYears %> <a href="<%= Branding.CompanyWebSite %>"><%= Branding.CompanyName %></a> - All Rights Reserved   
            <br />
            <div style="clear: both;">&nbsp;</div>
        </div>
    </form>

    <script>
        setTimeout(AutoLogout, <%= Feature.AutoLogoutTime %> );
        
        function AutoLogout() {
            alert("<%= Branding.AutoLogoutMessage %>");
            window.location = window.location.href;
        }
    </script>

    <style>
        table{
            width:100%; 
            border: 1px solid black; 
            border-collapse:collapse
        }
        table td, table td * {
            vertical-align: top;
        }
    </style>
</body>
</html>
