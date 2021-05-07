<%@ Page Title="" Language="C#" MasterPageFile="~/admin/AdminPortal/AdminPortal.Master" AutoEventWireup="true" CodeBehind="Finalize1094.aspx.cs" Inherits="Afas.AfComply.UI.admin.AdminPortal.Finalize1094" %>


<%@ Import Namespace="Afas.AfComply.Domain" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server"></asp:ToolkitScriptManager>
    <h2>Finalize 1094</h2>
    <br />
      <table style="width: 100%; ">
           <tr>
            <td>
                Select Employer
            </td>
            <td>
                <asp:DropDownList ID="DdlEmployer" runat="server" CssClass="ddl2" OnSelectedIndexChanged="DdlEmployer_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
             </td>
            </tr>

         <tr>
             <td>

             </td>
             <td>
                <asp:Button ID="BtnPlanYearUpdate" CssClass="btn" runat="server" Text="Submit" OnClick="BtnFinalize1094_Click" />
            </td>
        </tr>

      </table>

    </asp:Content>