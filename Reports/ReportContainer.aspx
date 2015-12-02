<%@ Page Title="" Language="C#" MasterPageFile="~/ReportingEngine.Master" AutoEventWireup="true" CodeBehind="ReportContainer.aspx.cs" Inherits="WMS.Reports.ReportContainer" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <rsweb:ReportViewer ID="ReportViewer1" Height="1500px" Width="1000px" SizeToReportContent="true" runat="server"></rsweb:ReportViewer>
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="3600">
                 </asp:ScriptManager>
</asp:Content>
