<%@ Page Title="" Language="C#" MasterPageFile="~/ReportingEngine.Master" AutoEventWireup="true" CodeBehind="ReportContainer.aspx.cs" Inherits="WMS.Reports.ReportContainer" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="../../DHA/Scripts/jquery-2.1.1.min.js" type="text/javascript"></script>
    <script src="../../DHA/Scripts/bootstrap.min.js" type="text/javascript"></script>
    <script src="../../DHA/Scripts/jquery.maskedinput.js" type="text/javascript"></script>
   
    <script src="../../DHA/Scripts/angular.min.js"></script>
<script src="../../DHA/Scripts/highcharts.js"></script>
    <script src="../../DHA/Scripts/highcharts-3d.js"></script>
        <script src="../../DHA/Scripts/exporting.js"></script>
<script src="../../DHA/Scripts/angular/highcharts-ng.js"></script>
     <script src="../../DHA/Scripts/slimscroll.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <br />

    <rsweb:ReportViewer ID="ReportViewer1" Height="1500px" Width="1000px" SizeToReportContent="true" runat="server">
    </rsweb:ReportViewer>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
</asp:Content>
