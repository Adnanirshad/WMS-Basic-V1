<%@ Page Title="" Language="C#" MasterPageFile="~/ReportingEngine.Master" AutoEventWireup="true" CodeBehind="Graphs.aspx.cs" Inherits="WMS.Reports.Graphs.Graphs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<style>
    .selected {
        background-color:black;
        color:white;
        font-weight:bold;
    }
</style>
    <div ng-controller="DashboardController" ng-Init="InitFunction()" style="height:100vh;" class="jumbotron" style="background:blue !important" >
        <div class="row" style="margin-bottom:20px;">
            <div class="col-md-2">
                <form name="myForm" class=" form-group">
                    <label for="repeatSelect"> Criteria: </label>
                    <select name="repeatSelect" id="repeatSelect" ng-model="Criteria.repeatSelect" placeholder="Criteria...">
                      <option ng-repeat="option in Criteria.availableOptions" value="{{option.id}}">{{option.name}}</option>
                    </select>
                </form>
                
            </div>
           <div class="col-md-4">
               <div class="col-md-8">
               <form name="myForm">
                    <label for="ValueSelect"> Value: </label>
                    <select name="ValueSelect" id="ValueSelect" ng-model="Value.repeatSelect">
                        <option ng-repeat="option in Value.availableOptions" value="{{option.id}}">{{option.name}}</option>
                    </select>
                </form>
                   </div>
               <div class="col-md-4">
                Select All:   <input type="checkbox" ng-model="MultipleSelect"/>
           </div></div>
            <div class="col-md-6">
               
                DateFrom : <input id="dateFrom" ng-model="DateFrom"  class="input-sm"  type="date" />
                 DateTo : <input id="date1" ng-model="DateTo"  class="input-sm"  type="date" />
             
            </div>
        </div>
        <div class="row">
            <div class="col-md-2">
                <div class="row">
                    <div style="font-size:16px; background-color:wheat;">Select Graph Type</div>
                     <div class="table-responsive">  
                    <table class="table table-bordered table-striped"> 
                      <tr ng-repeat="x in names" ng-class="{'selected':$index == selectedRow}" ng-click="setClickedRow($index)">
                        <td>{{ x }}</td>
   
                      </tr>
                    </table>
                        </div>
                </div>
               
                <div class="row">
                    <asp:button CssClass="btn btn-small btn-primary" ng-click="GetBestCriteria()" OnClientClick="false">Evaluation for the past 20 days</asp:button>
                </div>
            </div>
            
            <div class="col-md-10" >
                <div class ="row"> 
                    <div class="col-md-6" >
                        <highchart id="chart1" config="highchartsNG"></highchart>
                    </div> 
                    <div class="col-md-6" >
                        <highchart id="HighchartforSameDate" config="highchartsBG"></highchart>
                    </div> 
                </div> 
            </div>
        </div>
    </div>
  
</asp:Content>
