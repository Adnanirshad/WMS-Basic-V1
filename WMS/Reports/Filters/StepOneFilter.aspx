<%@ Page Title="" Language="C#" MasterPageFile="~/ReportingEngine.Master" AutoEventWireup="true" CodeBehind="StepOneFilter.aspx.cs" Inherits="WMS.Reports.Filters.StepOneFilter" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
  .hiddencol
  {
    display: none;
  }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
  .hiddencol
  {
    display: none;
  }
</style>
    <section class="container" style="margin-left:0;margin-right:0;">
        <div class="col-sm-3 col-md-3 col-lg-3" >
            <!-- Sidebar -->
            <div id="sidebar-wrapper">
                <ul class="sidebar-nav">
                    <li class="sidebar-brand">
                        <h4>Filters Navigation</h4>
                    </li>
                    <li >
                        <asp:LinkButton ID="btnStepOne" runat="server" CssClass="active-link" OnClick="btnStepOne_Click" >Step One<p>Units, Locations</p></asp:LinkButton>
                    </li>
                    <li>
                        <asp:LinkButton ID="btnStepFour" runat="server" CssClass="inactive-link" OnClick="btnStepFour_Click" >Step Two<p>Types, Shifts</p></asp:LinkButton>
                    </li>
                    <li>
                        <asp:LinkButton ID="btnStepFive" runat="server"  CssClass="inactive-link" OnClick="btnStepFive_Click" >Step Three<p>Employee</p></asp:LinkButton>
                    </li>
                    <li>
                        <asp:LinkButton ID="btnStepSix" runat="server" CssClass="inactive-link" OnClick="btnStepSix_Click" >Finish<p>Generate Report</p></asp:LinkButton>

                    </li>
                </ul>
                
            <!-- /#sidebar-wrapper -->
        </div>
        </div>
        <div class="col-sm-9 col-md-9 col-lg-9">
                <div class="row">
                    <div class="col-md-8">
                        <div class="row"> 
                            <div class="col-md-8">
                                <h3>Choose Units or Location</h3>
                                 </div>
                            
                               <div class="col-md-3">
                              <asp:Button ID="Button3" runat="server" style="margin-top:18px" Text="Clear All Filters" CssClass="btn-warning" OnClick="ButtonDeleteAll_Click" />
                                </div>
                                 </div>
                       
                        <div class="row">
                            <div class="col-md-6">
                                From : <input id="dateFrom"  class="input-sm"  runat="server" type="date" />
                            </div>
                            <div class="col-md-6">
                                To : <input id="dateTo" class="input-sm"  runat="server" type="date" />
                            </div>
                        </div>
                        <hr />
                         <div class="row">
                             <div class="filterHeader"><span class="FilterNameHeading">Units</span>
                                 <span style="margin-left:10px"><asp:TextBox ID="TextBoxSearch" CssClass="input-field" runat="server" /> <asp:Button ID="Button1" runat="server" Text="Search" CssClass="btn-primary" OnClick="ButtonSearchSection_Click" /></span>
                        </div>
                             <section>
                            <asp:GridView ID="GridViewSection" runat="server" Width="350px" AutoGenerateColumns="False" PagerStyle-CssClass="pgr" CssClass="Grid"                              GridLines="None" AllowPaging="True" AllowSorting="True"                                                OnPageIndexChanging="GridViewSection_PageIndexChanging" BorderColor="#0094FF" BorderStyle="None" OnRowDataBound="GridViewSection_RowDataBound" ShowFooter="True" BorderWidth="1px"  >
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="SectionID" HeaderText="ID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <%--<asp:CheckBox ID="CheckAll" runat="server" />--%>
                                            <input style="margin-left:6px" id="chkAll" onclick="javascript: SelectAllCheckboxes(this, 'GridViewSection');" 
                                            runat="server" type="checkbox" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox style="margin-left:6px"  ID="CheckOne" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                        
                                        <asp:BoundField DataField="SectionName" HeaderText="Units" />
                                        <asp:BoundField DataField ="DeptName" HeaderText ="Groups" />  
                                </Columns>
                                <EditRowStyle BackColor="#999999" />
                                <FooterStyle BackColor="#EEEEEE" Font-Bold="False" ForeColor="Black" Wrap="False" />
                                <HeaderStyle BackColor="#EEEEEE" Font-Bold="False" ForeColor="Black" BorderColor="#0094FF" BorderStyle="None" BorderWidth="1px" />
                                <PagerSettings FirstPageText="First" LastPageText="Last" NextPageText="Next" PreviousPageText="Prev" Mode="NextPreviousFirstLast" />
                                <PagerStyle BackColor="White" ForeColor="#0094FF" HorizontalAlign="Center" />
                                <RowStyle BackColor="White" ForeColor="#333333" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                            </asp:GridView>
                        </section>
                        </div>
                        <hr />
                        <div class="row">
                             <div class="filterHeader"><span class="FilterNameHeading">Locations</span>
                                 <span style="margin-left:10px"><asp:TextBox ID="tbSearch_Location" CssClass="input-field" runat="server" /> <asp:Button ID="Button2" runat="server" Text="Search" CssClass="btn-primary" OnClick="ButtonSearchLoc_Click" /></span>
                        </div>
                             <section>
                            <asp:GridView ID="GridViewLocation" runat="server" Width="350px" AutoGenerateColumns="False" PagerStyle-CssClass="pgr" CssClass="Grid"                              GridLines="None" AllowPaging="True" AllowSorting="True"                                                OnPageIndexChanging="GridViewLocation_PageIndexChanging" BorderColor="#0094FF" BorderStyle="None" OnRowDataBound="GridViewLocation_RowDataBound" ShowFooter="True" BorderWidth="1px"  >
                                <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                <Columns>
                                    <asp:BoundField DataField="LocID" HeaderText="ID" ItemStyle-CssClass="hiddencol" HeaderStyle-CssClass="hiddencol" />
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <%--<asp:CheckBox ID="CheckAll" runat="server" />--%>
                                            <input style="margin-left:6px" id="chkAll" onclick="javascript: SelectAllCheckboxes(this, 'GridViewLocation');" 
                                            runat="server" type="checkbox" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox style="margin-left:6px"  ID="CheckOne" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle Width="10%" />
                                    </asp:TemplateField>
                                        
                                        <asp:BoundField DataField="LocName" HeaderText="Name" />
                                        <%-- <asp:BoundField DataField="City.CityName" HeaderText="CityName" /> --%>   
                                </Columns>
                                <EditRowStyle BackColor="#999999" />
                                <FooterStyle BackColor="#EEEEEE" Font-Bold="False" ForeColor="Black" Wrap="False" />
                                <HeaderStyle BackColor="#EEEEEE" Font-Bold="False" ForeColor="Black" BorderColor="#0094FF" BorderStyle="None" BorderWidth="1px" />
                                <PagerSettings FirstPageText="First" LastPageText="Last" NextPageText="Next" PreviousPageText="Prev" Mode="NextPreviousFirstLast" />
                                <PagerStyle BackColor="White" ForeColor="#0094FF" HorizontalAlign="Center" />
                                <RowStyle BackColor="White" ForeColor="#333333" />
                                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                            </asp:GridView>
                        </section>
                        </div>
                    </div>
                   <section class="col-md-4 selected-filters-wrapper">
                    <h2>Selected Filters...</h2><hr />
                    <div class="panel-group" id="accordion">

	


                    
                    <% if (((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).SectionFilter.Count > 0)
                       {
                           {
                               int d = ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).SectionFilter.Count;
                               Response.Write("<div class='panel panel-default'><div class='panel-heading'><h4 class='panel-title'><a style = 'text-decoration: none !important;' data-toggle='collapse' data-parent='#accordion' href='#collapseSection'>Units</a>  <span style ='float:right;' class='badge' id='SectionSpan'>" + d + "</span></h4></div><div id='collapseSection' class='panel-collapse collapse out'><div class='list-group'>");
                           }
                           foreach (var item in ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).SectionFilter)
                           {
                               { Response.Write("<a class='list-group-item' id='Section'>" + item.FilterName + "<button type='button' id='" + item.ID + "' onclick = 'deleteFromFilters(this)' class='btn btn-danger btn-sm' style='float:right;'>[X]</button></a> "); }
                           }
                           { Response.Write("</div></div></div>"); }
                       }%>
                       
                   </div>
                          <div class="panel-group" id="Div1">

                         <% if (((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).LocationFilter.Count > 0)
                       {
                           {
                               int d = ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).LocationFilter.Count;
                               Response.Write("<div class='panel panel-default'><div class='panel-heading'><h4 class='panel-title'><a style = 'text-decoration: none !important;' data-toggle='collapse' data-parent='#Div1' href='#collapseLocation'>Locations</a>  <span style ='float:right;' class='badge' id ='LocationSpan'>" + d + "</span></h4></div><div id='collapseLocation' class='panel-collapse collapse out'><div class='list-group'>");
                           }
                           foreach (var item in ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).LocationFilter)
                           {
                               { Response.Write("<a class='list-group-item' id ='Location'>" + item.FilterName + "<button type='button' id='" + item.ID + "' onclick = 'deleteFromFilters(this)' class='btn btn-danger btn-sm' style='float:right;'>[X]</button></a>"); }
                           }
                           { Response.Write("</div></div></div>"); }
                    }%>
                               </div>
                       
                       <div class="panel-group" id="Div3">
                         <% if (((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).ShiftFilter.Count > 0)
                       {
                           {
                               int d = ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).ShiftFilter.Count;
                               Response.Write("<div class='panel panel-default'><div class='panel-heading'><h4 class='panel-title'><a style = 'text-decoration: none !important;' data-toggle='collapse' data-parent='#Div3' href='#collapseShift'>Shifts</a>  <span style ='float:right;' class='badge' id ='ShiftSpan'>" + d + "</span></h4></div><div id='collapseShift' class='panel-collapse collapse out'><div class='list-group'>");
                           }
                           foreach (var item in ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).ShiftFilter)
                           {
                               { Response.Write("<a class='list-group-item' id='Shift'>" + item.FilterName + "<button type='button' id='" + item.ID + "' onclick = 'deleteFromFilters(this)' class='btn btn-danger btn-sm' style='float:right;'>[X]</button></a>"); }
                           } 
                                { Response.Write("</div></div></div>"); }
                    }%>

                       </div>
                         <div class="panel-group" id="Div4">
                         <% if (((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).DepartmentFilter.Count > 0)
                       {
                           {
                               int d = ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).DepartmentFilter.Count;
                               Response.Write("<div class='panel panel-default'><div class='panel-heading'><h4 class='panel-title'><a style = 'text-decoration: none !important;'  data-toggle='collapse' data-parent='#Div4' href='#collapseDepartment'>Departments<span  style ='float:right;' class='badge' id ='DepartmentSpan'>" + d + "</span></a></h4></div><div id='collapseLocation' class='panel-collapse collapse out'><div class='list-group'>");
                           }
                           foreach (var item in ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).DepartmentFilter)
                           {
                               { Response.Write("<a class='list-group-item' id='Department'>" + item.FilterName + "<button type='button' id='" + item.ID + "' onclick = 'deleteFromFilters(this)' class='btn btn-danger btn-sm' style='float:right;'>[X]</button></a>"); }
                           }
                           { Response.Write("</div></div></div>"); }
                    }%>
                             </div>
                         <div class="panel-group" id="Div5">
                         <% if (((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).TypeFilter.Count > 0)
                       {
                           {
                               int d = ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).TypeFilter.Count;
                               Response.Write("<div class='panel panel-default'><div class='panel-heading'><h4 class='panel-title'><a data-toggle='collapse' data-parent='#Div5' href='#collapseType'>Employee Type</a><span style ='float:right;' class='badge' id ='TypeSpan'>" + d + "</span></h4></div><div id='collapseType' class='panel-collapse collapse out'><div class='list-group'>");
                           }
                           foreach (var item in ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).TypeFilter)
                           {
                               { Response.Write("<a class='list-group-item' id='Type'>" + item.FilterName + "<button type='button' id='" + item.ID + "' onclick = 'deleteFromFilters(this)' class='btn btn-danger btn-sm' style='float:right;'>[X]</button></a>"); }
                           }
                           { Response.Write("</div></div></div>"); }
                    }%>

                         </div>

                        <% if (((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).EmployeeFilter.Count > 0)
                       {
                           {
                               int d = ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).EmployeeFilter.Count;
                               Response.Write("<div class='panel panel-default'><div class='panel-heading'><h4 class='panel-title'><a data-toggle='collapse' data-parent='#Div7' href='#collapseEmployee'>Employees</a><span style ='float:right;' class='badge' id ='EmployeeSpan'>" + d + "</span></h4></div><div id='collapseEmployee' class='panel-collapse collapse out'><div class='list-group'>");
                           }
                           foreach (var item in ((WMSLibrary.FiltersModel)HttpContext.Current.Session["FiltersModel"]).EmployeeFilter)
                           {
                               { Response.Write("<a class='list-group-item' id='Employee'>" + item.FilterName + "<button type='button' id='" + item.ID + "' onclick = 'deleteFromFilters(this)' class='btn btn-danger btn-sm' style='float:right;'>[X]</button></a>"); }
                           }  { Response.Write("</div></div></div><div>"); }
                    }%>


                </section>
                </div>
                <div class="row">
                    
                </div>
        </div>
    </section>

    <asp:scriptmanager id="ScriptManager1" runat="server" EnablePartialRendering="true"  enablepagemethods="true" />

   <%-- <script type="text/javascript">

        $(document).ready(function () {

            window.addEventListener('beforeunload', clearSession);

        });

        function clearSession() {
            PageMethods.ClearSession();
            //$.ajax({
            //    type: "POST",

            //    url: "StepOneFilter.aspx/ClearSession",
            //});

        }

        
    </script>--%>
   <script src="../../Scripts/Filters/DeleteSingleFilters.js"></script>
    <%--<script src="../../Scripts/Filters/ClearSessionOnPageClose.js" />--%>

</asp:Content>

