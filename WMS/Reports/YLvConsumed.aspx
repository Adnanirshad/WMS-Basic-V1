<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="YLvConsumed.aspx.cs" Inherits="WMS.Reports.YLvConsumed" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script src="../Scripts/modernizr-2.6.2.js"></script>
    <script src="../Scripts/jquery-2.1.1.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="report-container">
           <div class="report-filter">
             <div style="height:20px;"></div>
             <div class="button-divDate">
                 <span>From: </span><input id="dateFrom" type="date" runat="server" style="height:30px;" />
                 <span>To: </span><input id="dateTo" type="date" runat="server" style="height:30px;" />
             </div>
            <div class="button-div">
                <asp:Button ID="btnGenerateReport" CssClass="btn btn-success" 
                    runat="server" Text="Generate Report" onclick="btnGenerateReport_Click" Width="190px" />

            </div>
            <div class="button-div"><asp:Button ID="btnEmployeeGrid" CssClass="btn btn-primary" runat="server" Text="Select Employee" onclick="btnEmployeeGrid_Click" Width="180px"/>
                <div><asp:Label ID="lbEmpCount" runat="server" Text="Selected Employees: 0" Font-Bold="False" Font-Italic="True"  Font-Size="10pt" ForeColor="#F9F9F9"></asp:Label></div>
             </div>

            <div class="button-div"><asp:Button ID="btnSectionGrid" CssClass="btn btn-primary" runat="server" Text="Select Section" onclick="btnSectionGrid_Click" Width="180px"/>
                <div><asp:Label ID="lbSectionCount" runat="server" Text="Selected Sections: 0" Font-Bold="False" Font-Italic="True" Font-Size="10pt" ForeColor="#F9F9F9"></asp:Label></div>
            </div>

            <div class="button-div"><asp:Button ID="btnDepartmentGrid" CssClass="btn btn-primary" runat="server" 
                Text="Select Department" onclick="btnDepartmentGrid_Click" Width="180px"/>
                <div><asp:Label ID="lbDeptCount" runat="server" Text="Selected Departments: 0" Font-Bold="False" Font-Italic="True" Font-Size="10pt" ForeColor="#F9F9F9"></asp:Label></div>
            </div>

            <div class="button-div"><asp:Button ID="btnCrewGrid" CssClass="btn btn-primary" runat="server" 
                Text="Select Crew" onclick="btnCrewGrid_Click" Width="180px" />
                <div><asp:Label ID="lbCrewCount" runat="server" Text="Selected Crews: 0" Font-Bold="False" Font-Italic="True" Font-Size="10pt" ForeColor="#F9F9F9"></asp:Label></div>
            </div>

            <div class="button-div"><asp:Button ID="btnShiftGrid" CssClass="btn btn-primary" runat="server" 
                Text="Select Shift" onclick="btnShiftGrid_Click" Width="180px"/>
                <div><asp:Label ID="lbShiftCount" runat="server" Text="Selected Shifts: 0" Font-Bold="False" Font-Italic="True" Font-Size="10pt" ForeColor="#F9F9F9"></asp:Label></div>
            </div>

            <div class="button-div">
                <asp:Button ID="btnLoc" CssClass="btn btn-primary" runat="server" 
                Text="Select Locations" onclick="btnLoc_Click" Width="180px"/>
                <div><asp:Label ID="lbLocCount" runat="server" Text="Selected Locations: 0" Font-Bold="False" Font-Italic="True" Font-Size="10pt" ForeColor="#F9F9F9"></asp:Label></div>
            </div>

            <div class="button-div">
                <asp:Button ID="btnEmpType" CssClass="btn btn-primary" runat="server" 
                Text="Select Employee Type" onclick="btnEmpType_Click" Width="180px"/>
                <div><asp:Label ID="lbCatCount" runat="server" Text="Selected Types: 0" Font-Bold="False" Font-Italic="True" Font-Size="10pt" ForeColor="#F9F9F9"></asp:Label></div>
            </div>


            <div class="button-div"></div>

         </div> <%--End div Report-filter--%>

           <div class="report-viewer-container" style="margin: 0 auto;">
             <%-- Emp Grid --%>
             <div runat="server" id="DivGridEmp" style="margin: 20px;">
                 <div style="font-size: 15px;margin: 10px;font-weight: bold;">Press Ctrl+F to Find a Employee </div>
             <asp:GridView ID="grid_Employee" runat="server" OnRowDataBound="grid_Employee_RowDataBound" AutoGenerateColumns="False" DataKeyNames="EmpID">
                 <Columns>
                     <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkCtrlEmp" runat="server" oncheckedchanged="chkCtrlEmp_CheckedChanged"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                     <asp:BoundField DataField="EmpID" HeaderText="Emp ID" ReadOnly="True" SortExpression="EmpID"  />
                     <asp:BoundField DataField="EmpNo" HeaderText="Emp No" SortExpression="EmpNo" />
                     <asp:BoundField DataField="EmpName" HeaderText="Name" SortExpression="EmpName"  />
                     <asp:BoundField DataField="CardNo" HeaderText="Card No" SortExpression="CardNo" />
                     <asp:BoundField DataField="DesignationName" HeaderText="Designation" SortExpression="DesignationName" />
                     <asp:BoundField DataField="SectionName" HeaderText="Section" SortExpression="SectionName"  />
                     <asp:BoundField DataField="ShiftName" HeaderText="Shift" SortExpression="ShiftName" />
                     
                 </Columns>
             </asp:GridView>
            </div>

  <%-- Section Grid --%>
             <div runat="server" id="DivGridSection" style="margin: 20px;">
                 <div style="font-size: 15px;margin: 10px;font-weight: bold;">Press Ctrl+F to Find a Section </div>
             <asp:GridView ID="grid_Section" runat="server" AutoGenerateColumns="False" DataKeyNames="SectionID">
                 <Columns>
                     <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkCtrlSection" runat="server" oncheckedchanged="chkCtrlSection_CheckedChanged"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                     <asp:BoundField DataField="SectionID" HeaderText="SectionID" InsertVisible="False" ReadOnly="True" SortExpression="SectionID" />
                     <asp:BoundField DataField="SectionName" HeaderText="SectionName" SortExpression="SectionName" />
                     <asp:BoundField DataField="DeptID" HeaderText="DeptID" SortExpression="DeptID" />
                 </Columns>
             </asp:GridView>
             </div>

             <%-- Department Grid --%>
             <div runat="server" id="DivGridDept" style="margin: 20px;">
                 <div style="font-size: 15px;margin: 10px;font-weight: bold;">Press Ctrl+F to Find a Department </div>
                 <asp:GridView ID="grid_Dept" runat="server" AutoGenerateColumns="False" DataKeyNames="DeptID" >
                     <Columns>
                         <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkCtrlDept" runat="server"  oncheckedchanged="chkCtrlDept_CheckedChanged"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:BoundField DataField="DeptID" HeaderText="DeptID" InsertVisible="False" ReadOnly="True" SortExpression="DeptID" />
                         <asp:BoundField DataField="DeptName" HeaderText="DeptName" SortExpression="DeptName" />
                     </Columns>
                 </asp:GridView>
             </div>

             <%-- Location Grid --%>
             <div runat="server" id="DivLocGrid" style="margin: 20px;">
                 <div style="font-size: 15px;margin: 10px;font-weight: bold;">Press Ctrl+F to Find a Location </div>
                 <asp:GridView ID="grid_Location" runat="server" AutoGenerateColumns="False" DataKeyNames="LocID">
                     <Columns>
                         <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkCtrlLoc" runat="server"  oncheckedchanged="chkCtrlLoc_CheckedChanged"/>
                                </ItemTemplate>
                            </asp:TemplateField>
                         <asp:BoundField DataField="LocID" HeaderText="LocID" InsertVisible="False" ReadOnly="True" SortExpression="LocID" />
                         <asp:BoundField DataField="LocName" HeaderText="LocName" SortExpression="LocName" />
                     </Columns>
                 </asp:GridView>
             </div>

             <%-- Crew Grid --%>
             <div runat="server" id="DivGridCrew" style="margin: 20px;">
                 <div style="font-size: 15px;margin: 10px;font-weight: bold;">Press Ctrl+F to Find a Crew </div>
                 <asp:GridView ID="grid_Crew" runat="server" AutoGenerateColumns="False" DataKeyNames="CrewID" >
                     <Columns>
                         <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkCtrlCrew" runat="server"  oncheckedchanged="chkCtrlCrew_CheckedChanged" />
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:BoundField DataField="CrewID" HeaderText="CrewID" InsertVisible="False" ReadOnly="True" SortExpression="CrewID" />
                         <asp:BoundField DataField="CrewName" HeaderText="CrewName" SortExpression="CrewName" />
                     </Columns>
                 </asp:GridView>
             </div>

             <%-- EmpType Grid --%>
             <div runat="server" id="DivTypeGrid" style="margin: 20px;">
                 <div style="font-size: 15px;margin: 10px;font-weight: bold;">Press Ctrl+F to Find a Type </div>
                 <asp:GridView ID="grid_EmpType" runat="server" AutoGenerateColumns="False" DataKeyNames="TypeID">
                     <Columns>
                         <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkCtrlType" runat="server"  oncheckedchanged="chkCtrlType_CheckedChanged"/>
                                </ItemTemplate>
                        </asp:TemplateField>
                         <asp:BoundField DataField="TypeID" HeaderText="TypeID" ReadOnly="True" SortExpression="TypeID" />
                         <asp:BoundField DataField="TypeName" HeaderText="TypeName" SortExpression="TypeName" />
                         <asp:BoundField DataField="CatID" HeaderText="CatID" SortExpression="CatID" />
                     </Columns>
                 </asp:GridView>
             </div>

             <%-- Shift Grid --%>
             <div runat="server" id="DivShiftGrid" style="margin: 20px;">
                 <div style="font-size: 15px;margin: 10px;font-weight: bold;">Press Ctrl+F to Find a Shift </div>
                 <asp:GridView ID="grid_Shift" runat="server" AutoGenerateColumns="False" DataKeyNames="ShiftID">
                     <Columns>
                         <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkCtrlShift" runat="server"  oncheckedchanged="chkCtrlShift_CheckedChanged"/>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:BoundField DataField="ShiftID" HeaderText="ShiftID" ReadOnly="True" SortExpression="ShiftID" />
                         <asp:BoundField DataField="ShiftName" HeaderText="ShiftName" SortExpression="ShiftName" />
                         <asp:BoundField DataField="StartTime" HeaderText="StartTime" SortExpression="StartTime" />
                     </Columns>
                 </asp:GridView>
             </div>
            <div>
                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt">
                    <LocalReport ReportPath="Reports\RDLC\YLvConsumed.rdlc">
                        <DataSources>
                            <rsweb:ReportDataSource DataSourceId="ObjectDataSource1" Name="DataSet1" />
                        </DataSources>
                    </LocalReport>
                </rsweb:ReportViewer>
                 <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="WMS.Models.TASReportDataSetTableAdapters.ViewLvConsumedTableAdapter"></asp:ObjectDataSource>
                 <asp:ScriptManager ID="ScriptManager1" runat="server">
                 </asp:ScriptManager>
             </div>
         </div> <%--End div Report-viewer-container--%>
           <div class="clearfix">
             
         </div> 
    </div><%--End div Report-container--%>
</asp:Content>


