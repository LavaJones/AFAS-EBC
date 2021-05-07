<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="step3.aspx.cs" Inherits="step3" %>
<%@ Import Namespace="Afas.AfComply.Domain" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="stylesheet" type="text/css" href="/default.css" />
    <link rel="stylesheet" type="text/css" href="/menu.css" />
    <link rel="stylesheet" type="text/css" href="/v_menu.css" />
    <title>Step C</title>

</head>
<body>
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
        <div id="container">
            <div id="header">
                <a href="/securepages/Default.aspx">
                    <img src="<%= Page.ResolveUrl(Branding.LogoUri) %>" style="height: 70px" alt="<%= Branding.ProductName %> Logo" class="logo" />
                </a>
                <ul id="toplinks">
                    <li>Need Help? Call <%= Branding.PhoneNumber %></li> 
                    <li>
                        <asp:Literal ID="LitUserName" runat="server"></asp:Literal></li>
                    <li>
                        <asp:Button ID="BtnLogout" CssClass="btn" runat="server" Text="Log Out" OnClick="BtnLogout_Click" UseSubmitBehavior="false" /></li>
                </ul>
                <asp:HiddenField ID="HfDistrictID" runat="server" />
            </div>
            <div id="nav2">
                <nav>
                    <ul>
                        <li><a href="step1.aspx">Step A</a></li>
                        <li><a href="step2.aspx">Step B</a></li>
                        <li><a href="step3.aspx">Step C</a></li>
                        <li><a href="step5.aspx">Step D</a></li>
                        <li><a href="step6.aspx">Step E</a></li>
                        <li><a href="complete.aspx">Final Review</a></li>
                    </ul>
                </nav>
                <ul class="right">
                    <li>D</li>
                </ul>
            </div>
            <div id="content">
                <asp:UpdatePanel ID="upDGE" runat="server">
                    <ContentTemplate>
                        <h3>Step C: 2015 Plan Year Transition Relief for Non-Calendar Year Plans</h3>

                        <p>
                            Limited transition relief continues to apply in 2016 to employers with non-calendar year plans who meet specific requirements.
                            For the 2016 calendar year, the transition relief described below is applicable only if the employer offered coverage under 
                            a health plan with a plan year beginning on a date other than January 1 (a non-calendar year plan year),
                            and only for the calendar months in 2016 that fall within that 2015 plan year. 
                        </p>

                        <p>
                            Two forms of transition relief are available and the rules to qualify for each are listed below. If you sponsor a non-calendar year plan, you should closely review these rules to determine if you qualify to claim the transition relief. An employer can claim only one (not both) form of transition relief.
                        </p>

                        <ul>
                            <li>
                                <h3>
                                    1.	Non-calendar year plan employers with 50-99 full-time employees:
                                </h3>
                                <p>
                                    To certify that an employer with at least one plan with a non-calendar year plan year is eligible for 2015 Plan Year 50-99 (“Mid-Size Employer”)
                                    Transition Relief transition relief it must have met the following conditions:
                                </p>
                                <ul style="margin-left:15px">
                                    <li>
                                        <p>
                                            •	The employer is an ALE or is part of an Aggregated ALE Group that had 50 to 99 full-time employees, including full-time equivalent employees, on business days in 2014;
                                        </p>
                                    </li>
                                    <li>
                                        <p>
                                            •	During the period of February 9, 2014, through December 31, 2014, the ALE or the Aggregated ALE Group of which the employer is 
                                            a member did not reduce the size of its workforce or reduce the overall hours of service of its employees in order to qualify for the transition relief; and
                                        </p>
                                    </li>
                                    <li>
                                        <p>
                                            •	During the period of February 9, 2014, through the last day of the 2015 plan year, 
                                            the ALE or Aggregated ALE Group of which the employer is a member does not eliminate or materially reduce the health coverage, 
                                            if any, it offered as of February 9, 2014
                                        </p>
                                    </li>
                                </ul>
                            </li>
                            <li>
                                <h3>
                                    2.	Non-calendar year plan employers with 100 or more full-time employees
                                </h3>
                                <p>
                                    To certify that an employer with at least one plan with a non-calendar year plan year is eligible for 2015 Plan Year 100 
                                    or More Transition Relief it must have met the following conditions:
                                </p>
                                <ul style="margin-left:15px">
                                    <li>
                                        <p>
                                            •	Have 100 or more full-time employees (including full-time equivalent employees) on business days in 2014.
                                        </p>
                                    </li>
                                </ul>
                            </li>
                        </ul>

                         <h4>Transition Relief Status</h4>

                       Did you sponsor a non-calendar year plan qualifying for 2015 Plan Year 50-99 (“Mid-Size Employer”) Transition Relief?
             
                        <br />
                        <asp:DropDownList ID="Ddl_step3_quest_1" runat="server" CssClass="ddl2" AutoPostBack="true" OnSelectedIndexChanged="Ddl_step3_quest_1_SelectedIndexChanged">
                            <asp:ListItem Text="YES" Value="true"></asp:ListItem>
                            <asp:ListItem Text="No" Value="false"></asp:ListItem>
                            <asp:ListItem Text="Select" Value="select" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                        <br />
                        <br />
                        <asp:Panel ID="Pnl_tr_final" runat="server" Visible="false">
                            Did you sponsor a non-calendar year plan qualifying for 2015 Plan Year 100 or More Transition Relief?
                 
                            <br />
                            <asp:DropDownList ID="Ddl_step3_quest_2" runat="server" CssClass="ddl2" AutoPostBack="true" OnSelectedIndexChanged="Ddl_step3_quest_2_SelectedIndexChanged">
                                <asp:ListItem Text="YES" Value="true"></asp:ListItem>
                                <asp:ListItem Text="No" Value="false"></asp:ListItem>
                                <asp:ListItem Text="Select" Value="select" Selected="True"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:Panel ID="Pnl_message" runat="server" Visible="false">
                                <p style="color: blue;">
                                    Your company may treat a full-time employee (including their dependents) as having been offered coverage for months prior to their effective date provided the employee was offered coverage as of the first day of the 2015 plan year.  
                     
                                </p>
                            </asp:Panel>
                        </asp:Panel>
                        <br />
                        
                        
                        <br />
                        <br />
                        <asp:Button ID="Btn_Next" runat="server" Text="NEXT" Font-Bold="true" Width="95%" Height="30px" Enabled="false" OnClick="Btn_Next_Click" />
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

            <div style="clear: both;">&nbsp;</div>
            <div id="footer">
                Copyright &copy; <%= Branding.CopyrightYears %> <a href="<%= Branding.CompanyWebSite %>"><%= Branding.CompanyName %></a> - All Rights Reserved   
                <br />
                <div style="clear: both;">&nbsp;</div>
            </div>
        </div>
    </form>
</body>
</html>
