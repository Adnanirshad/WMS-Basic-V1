<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="ReportHome.aspx.cs" Inherits="WMS.Reports.ReportHome" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/modernizr-2.6.2.js"></script>
    <script src="../Scripts/jquery-2.1.1.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <div class="col-md-12">
     <div class="jumbotron">
    <h1>Fatima Group</h1>
    <h2>Reporting System</h2>
</div>

        <div>
            <div class="col-md-4">
                <div class="panel panel-info">
                    <div class="panel-heading"><h3>Daily Reports</h3></div>
                    <div class="panel-body">
                        Our completely automated time and attendance solutions reduce labor costs by enforcing pay and work rules consistently and accurately across the organization. Labor-intensive timecard tracking, data entry. And that reduces the administrative time associated with attendance exceptions and employee inquiries.
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="panel panel-warning">
                    <div class="panel-heading"><h3>Monthly Reports</h3></div>
                    <div class="panel-body">
                        Leave Tracker enables you to apply for leave online, view leave balances and track all your employee leave information from one central place. Employees' leave transactions are accurately tracked and leave balances are automatically updated. You can import/export leave summary for integrating data with other applications.
                    </div>
                </div>
            </div>
             <div class="col-md-4">
                <div class="panel panel-success">
                    <div class="panel-heading"><h3>Trends</h3></div>
                    <div class="panel-body">
                       Flexible shift patterns should work well for any environment. The easiest to manage should be for early and late starters.The trick is to start to look at the call profile and to look at your staffing profile. You may find that your resource forecasting software may be able to help you modelling the call profiles against the number of people needed.
                    </div>
                </div>
            </div>
        </div>
    </div>

    </asp:Content>
