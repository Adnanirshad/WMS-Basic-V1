using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WMS.Models;

namespace WMS.CustomClass
{
    public class QueryBuilder
    {
        public DataTable GetValuesfromDB(string query)
        {
            DataTable dt = new DataTable();
            SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TAS2013ConnectionString"].ConnectionString);

            using (SqlCommand cmdd = Conn.CreateCommand())
            using (SqlDataAdapter sda = new SqlDataAdapter(cmdd))
            {
                cmdd.CommandText = query;
                cmdd.CommandType = CommandType.Text;
                Conn.Open();
                sda.Fill(dt);
                Conn.Close();
            }
            return dt;
        }
        public string MakeCustomizeQuery(User _user)
        {
            string RoleQuery = "";
            string CatQuery = "";
            TAS2013Entities db = new TAS2013Entities();
            List<UserRoleData> userRoleData = new List<UserRoleData>();
            List<string> UserRoleString = new List<string>();
            List<string> CategoryUser = new List<string>();
            //CategoryUser.Add(" where (CatID=1 ");
            //if (_user.ViewContractual == true)
            //{
            //    CategoryUser.Add(" CatID = 4 ");
            //}
            //if (_user.ViewPermanentMgm == true)
            //{
            //    CategoryUser.Add(" CatID = 2  ");
            //}
            //if (_user.ViewPermanentStaff == true)
            //{
            //    CategoryUser.Add(" CatID = 3  ");
            //}
            userRoleData = db.UserRoleDatas.Where(aa => aa.RoleUserID == _user.UserID).ToList();
            switch (_user.UserRoleD)
            {
                case "A"://Admin

                    break;
                case "C"://City
                    foreach (var urd in userRoleData)
                    {
                        UserRoleString.Add(" CityID = " + urd.RoleDataValue + " ");
                    }
                    break;
                case "D"://Department
                    foreach (var urd in userRoleData)
                    {
                        UserRoleString.Add(" DeptID = " + urd.RoleDataValue + " ");
                    }
                    break;
                case "E"://Single Employee
                    foreach (var urd in userRoleData)
                    {
                        UserRoleString.Add(" EmpID = " + urd.RoleDataValue + " ");
                    }
                    break;
                case "L"://Location
                    foreach (var urd in userRoleData)
                    {
                        UserRoleString.Add(" LocID = " + urd.RoleDataValue + " ");
                    }
                    break;
                case "R"://Region
                    foreach (var urd in userRoleData)
                    {
                        UserRoleString.Add(" RegionID = " + urd.RoleDataValue + " ");
                    }
                    break;
                case "S"://Section
                    foreach (var urd in userRoleData)
                    {
                        UserRoleString.Add(" SecID = " + urd.RoleDataValue + " ");
                    }
                    break;
                case "V"://Division
                    foreach (var urd in userRoleData)
                    {
                        UserRoleString.Add(" DivID = " + urd.RoleDataValue + " ");
                    }
                    break;
                case "W"://Crew
                    foreach (var urd in userRoleData)
                    {
                        UserRoleString.Add(" CrewID = " + urd.RoleDataValue + " ");
                    }
                    break;
            }
            if (UserRoleString.Count == 1)
            {
                RoleQuery = " and (" + RoleQuery + UserRoleString[0] + " ) ";
            }
            else if(UserRoleString.Count>1)
            {
                RoleQuery = RoleQuery + " and ( ";
                for (int i = 0; i < UserRoleString.Count - 1; i++)
                {
                    RoleQuery = RoleQuery + UserRoleString[i] + " or ";
                }
                RoleQuery = RoleQuery + UserRoleString[UserRoleString.Count - 1] + " ) ";
            }
            if (CategoryUser.Count == 1)
                CatQuery = CatQuery + CategoryUser[0]+" ) ";
            else if(CategoryUser.Count>1)
            {
                for (int i = 0; i < CategoryUser.Count-1; i++)
                {
                    CatQuery = CatQuery + CategoryUser[i] + " or ";
                }
                CatQuery = CatQuery + CategoryUser[CategoryUser.Count - 1] + " ) ";
            }

            return CatQuery + RoleQuery;
        }

        public string QueryForCompanySegeration(User _user)
        {
            string query = "";
            if (query != "")
            {
                query = " where " + query;
            }
            return query;
        }
        public string QueryForLocationSegeration(User _user)
        {
            TAS2013Entities db = new TAS2013Entities();
            //List<UserLocation> ulocs = new List<UserLocation>();
            //List<string> _CriteriaForOrLoc = new List<string>();
            //ulocs = db.UserLocations.Where(aa => aa.UserID == _user.UserID).ToList();
            string query = "";
            //foreach (var uloc in ulocs)
            //{
            //    _CriteriaForOrLoc.Add(" LocationID = " + uloc.LocationID + " ");
            //}
            //for (int i = 0; i < _CriteriaForOrLoc.Count - 1; i++)
            //{
            //    query = query + _CriteriaForOrLoc[i] + " or ";
            //}
            //query = query + _CriteriaForOrLoc[_CriteriaForOrLoc.Count - 1];
            return query;
        }
        public string QueryForLocationTableSegeration(User _user)
        {
            TAS2013Entities db = new TAS2013Entities();
            //List<UserLocation> ulocs = new List<UserLocation>();
            //List<string> _CriteriaForOrLoc = new List<string>();
            //ulocs = db.UserLocations.Where(aa => aa.UserID == _user.UserID).ToList();
            string query = " where ";
            //foreach (var uloc in ulocs)
            //{
            //    _CriteriaForOrLoc.Add(" LocID = " + uloc.LocationID + " ");
            //}
            //for (int i = 0; i < _CriteriaForOrLoc.Count - 1; i++)
            //{
            //    query = query + _CriteriaForOrLoc[i] + " or ";
            //}
            //query = query + _CriteriaForOrLoc[_CriteriaForOrLoc.Count - 1];
            return query;
        }
        public string QueryForLocationFilters(User _user)
        {
            TAS2013Entities db = new TAS2013Entities();
            //List<UserLocation> ulocs = new List<UserLocation>();
            //List<string> _CriteriaForOrLoc = new List<string>();
            //ulocs = db.UserLocations.Where(aa => aa.UserID == _user.UserID).ToList();
            string query = "";
            //foreach (var uloc in ulocs)
            //{
            //    _CriteriaForOrLoc.Add(" LocID = " + uloc.LocationID + " ");
            //}
            //for (int i = 0; i < _CriteriaForOrLoc.Count - 1; i++)
            //{
            //    query = query + _CriteriaForOrLoc[i] + " or ";
            //}
            //query = query + _CriteriaForOrLoc[_CriteriaForOrLoc.Count - 1];
            return query;
        }
        public string QueryForCompanyView(User _User)
        {
            string query = "";
            //switch (_User.RoleID)
            //{
            //    case 1:
            //        break;
            //    case 2:
            //        query = " where CompID= 1 or CompID = 2 ";
            //        break;
            //    case 3:
            //        query = " where  CompID>= 3";
            //        break;
            //    case 4:
            //        query = " where  CompID = " + _User.ToString();
            //        break;
            //    case 5:
            //        break;
            //}
            return query;
        }
        public string QueryForCompanyFilters(User _User)
        {
            string query = "";
            //switch (_User.RoleID)
            //{
            //    case 1: 
            //        break;
            //    case 2:
            //        query = " where CompanyID= 1 or CompanyID = 2 ";
            //        break;
            //    case 3:
            //        query = " where  CompanyID>= 3";
            //        break;
            //    case 4:
            //        query = " where  CompanyID = " + _User.ToString();
            //        break;
            //    case 5:
            //        break;
            //}
            return query;
        }

        public string QueryForCompanyViewLinq(User _User)
        {
            string query = "";
           //switch (_User.RoleID)
           // {
           //     case 1: query ="CompID > 0";
           //         break;
           //     case 2:
           //         query = "CompID= 1 or CompID = 2 ";
           //         break;
           //     case 3:
           //         query = "CompID>= 3";
           //         break;
           //     case 4:
           //         query = "CompID = " + _User.ToString();
           //         break;
           //     case 5:
           //         break;
           // }
            return query;
        }

        public string QueryForCompanyViewForLinq(User _User)
        {
            string query = "";
            //switch (_User.RoleID)
            //{
            //    case 1: query = "CompanyID > 0";
            //        break;
            //    case 2:
            //        query = "CompanyID= 1 or CompanyID = 2 ";
            //        break;
            //    case 3:
            //        query = "CompanyID>= 3";
            //        break;
            //    case 4:
            //        query = "CompanyID = " + _User.ToString();
            //        break;
            //    case 5:
            //        break;
            //}
            return query;
        }

        internal string QueryForLocationTableSegerationForLinq(User LoggedInUser)
        {
            TAS2013Entities db = new TAS2013Entities();
            //List<UserLocation> ulocs = new List<UserLocation>();
            //List<string> _CriteriaForOrLoc = new List<string>();
            //ulocs = db.UserLocations.Where(aa => aa.UserID == LoggedInUser.UserID).ToList();
            String query = "";
            //foreach (var uloc in ulocs)
            //{
            //    _CriteriaForOrLoc.Add(" LocID = " + uloc.LocationID + " ");
            //}
            //for (int i = 0; i < _CriteriaForOrLoc.Count - 1; i++)
            //{
            //    query = query + _CriteriaForOrLoc[i] + " or ";
            //}
            //if (_CriteriaForOrLoc.Count != 0)
            //    query = query + _CriteriaForOrLoc[_CriteriaForOrLoc.Count - 1];
            //else
            //    query = "LocID > 0";
            return query;
        }

        

        internal string QueryForShiftForLinq(User LoggedInUser)
        {
            TAS2013Entities db = new TAS2013Entities();
            //List<UserLocation> ulocs = new List<UserLocation>();
            //List<string> _CriteriaForOrLoc = new List<string>();
            //ulocs = db.UserLocations.Where(aa => aa.UserID == LoggedInUser.UserID).ToList();
            string query = "";
            //foreach (var uloc in ulocs)
            //{
            //    _CriteriaForOrLoc.Add(" LocationID = " + uloc.LocationID + " ");
            //}
            //for (int i = 0; i < _CriteriaForOrLoc.Count - 1; i++)
            //{
            //    query = query + _CriteriaForOrLoc[i] + " or ";
            //}
            //query = query + _CriteriaForOrLoc[_CriteriaForOrLoc.Count - 1];
            return query;
        }

        #region -- Reports Filters Data Seggregation according to User Role--
        internal string QueryForRegionInFilters(User LoggedInUser)
        {
        //    string query = " where ";
        //    TAS2013Entities db = new TAS2013Entities();
        //    List<UserRoleData> roleDataL = new List<UserRoleData>();
        //    roleDataL = db.UserRoleDatas.Where(aa => aa.RoleUserID == LoggedInUser.UserID && aa.UserRoleLegend=="L").ToList();
        //    List<Region> regions = db.Regions.ToList();
        //    List<City> cities = db.Cities.ToList();
        //    List<Location> locs = db.Locations.ToList();
        //    List<string> queryList = new List<string>();
        //    foreach (var access in roleDataL)
        //    {
        //        switch (LoggedInUser.UserRoleL)
        //        {
        //            case "A"://Super ADmin
        //                query = "";
        //                break;
        //            case "R"://REgion
        //                queryList.Add(" RegionID =" + access.RoleDataValue.ToString());
        //                break;
        //            case "C"://City
        //                string regionID = cities.Where(aa => aa.CityID == access.RoleDataValue).FirstOrDefault().RegionID.ToString();
        //                queryList.Add(" RegionID =" + regionID);
        //                break;
        //            case "L"://Location
        //                string regionIDForLoc = locs.Where(aa => aa.LocID == access.RoleDataValue).FirstOrDefault().City.Region.RegionID.ToString();
        //                queryList.Add(" RegionID =" + regionIDForLoc);
        //                break;
        //        }
        //    }
        //    if (queryList.Count == 1)
        //    {
        //        query = query + queryList[0];
        //    }
        //    else if (queryList.Count > 1)
        //    {
        //        for (int i = 0; i < queryList.Count - 1; i++)
        //        {
        //            query = query + queryList[i] + " or ";
        //        }
        //        query = query + queryList[queryList.Count - 1];
        //    }


        //    return query;
        //}

        //internal string QueryForReportsCity(User LoggedInUser)
        //{
        //    string query = " where ";
        //    TAS2013Entities db = new TAS2013Entities();
        //    List<UserRoleData> uAcc = new List<UserRoleData>();
        //    uAcc = db.UserRoleDatas.Where(aa => aa.RoleUserID == LoggedInUser.UserID && aa.UserRoleLegend=="L").ToList();
        //    List<City> cities = db.Cities.ToList();
        //    List<Location> locs = db.Locations.ToList();
        //    List<string> queryList = new List<string>();
        //    foreach (var access in uAcc)
        //    {
        //        switch (LoggedInUser.UserRoleL)
        //        {
        //            case "A"://Super ADmin
        //                query = "";
        //                break;
        //            case "R"://REgion
        //                //List<City> city = cities.Where(aa => aa.RegionID == access.RoleDataValue).ToList();
        //                //foreach (var c in city)
        //                //{
        //                //    queryList.Add(" CityID =" + c.CityID);
        //                //}
        //                //break;
        //            case "C"://City
        //                //string cityID = cities.Where(aa => aa.CityID == access.RoleDataValue).FirstOrDefault().CityID.ToString();
        //                //queryList.Add(" CityID =" + cityID);
        //                //break;
        //            case "L"://Location
        //                //string cityIDForLoc = locs.Where(aa => aa.LocID == access.RoleDataValue).FirstOrDefault().CityID.ToString();
        //                queryList.Add(" CityID =" + cityIDForLoc);
        //                break;
        //        }
        //    }
        //    if (queryList.Count == 1)
        //    {
        //        query = query + queryList[0];
        //    }
        //    else if (queryList.Count > 1)
        //    {
        //        for (int i = 0; i < queryList.Count - 1; i++)
        //        {
        //            query = query + queryList[i] + " or ";
        //        }
        //        query = query + queryList[queryList.Count - 1];
        //    }


            return "";
        }

        internal string QueryForLocReport(User LoggedInUser)
        {
            //string query = " where ";
            //TAS2013Entities db = new TAS2013Entities();
            //List<UserRoleData> uAcc = new List<UserRoleData>();
            //uAcc = db.UserRoleDatas.Where(aa => aa.RoleUserID == LoggedInUser.UserID && aa.UserRoleLegend == "L").ToList();
            ////List<Region> regions = db.Regions.ToList();
            //List<Location> locss = db.Locations.ToList();
            //List<string> queryList = new List<string>();
            //foreach (var access in uAcc)
            //{
            //    switch (LoggedInUser.UserRoleL)
            //    {
            //        case "A"://Super ADmin
            //            query = "";
            //            break;
            //        case "R"://REgion
            //            List<Location> locs = locss.Where(aa => aaCity.RegionID == access.RoleDataValue).ToList();
            //            foreach (var c in locs)
            //            {
            //                queryList.Add(" LocID =" + c.LocID);
            //            }
            //            break;
            //        case "C"://City
            //            //locs = locss.Where(aa => aa.CityID == access.RoleDataValue).ToList();
            //            foreach (var c in locs)
            //            {
            //                queryList.Add(" LocID =" + c.LocID);
            //            }
            //            break;
            //        case "L"://Location
            //            string cityIDForLoc = locss.Where(aa => aa.LocID == access.RoleDataValue).FirstOrDefault().LocID.ToString();
            //            queryList.Add(" LocID =" + cityIDForLoc);
            //            break;
            //    }
            //}
            //if (queryList.Count == 1)
            //{
            //    query = query + queryList[0];
            //}
            //else if (queryList.Count > 1)
            //{
            //    for (int i = 0; i < queryList.Count - 1; i++)
            //    {
            //        query = query + queryList[i] + " or ";
            //    }
            //    query = query + queryList[queryList.Count - 1];
            //}


            return "";
        }

        //internal string QueryForEmployeeReports(User LoggedInUser)
        //{
        //    string query = " where ";
        //    TAS2013Entities db = new TAS2013Entities();
        //    List<UserAccess> uAcc = new List<UserAccess>();
        //    uAcc = db.UserAccesses.Where(aa => aa.UserID == LoggedInUser.UserID).ToList();
        //    List<Region> regions = db.Regions.ToList();
        //    List<City> cities = db.Cities.ToList();
        //    List<Location> locs = db.Locations.ToList();
        //    List<string> queryList = new List<string>();
        //    foreach (var access in uAcc)
        //    {
        //        switch (LoggedInUser.RoleID)
        //        {
        //            case 1://Super ADmin
        //                query = "";
        //                break;
        //            case 4://Zone
        //                queryList.Add(" ZoneID =" + access.CriteriaData.ToString());
        //                break;
        //            case 5://REgion
        //                queryList.Add(" RegionID =" + access.CriteriaData.ToString());
        //                break;
        //            case 6://City

        //                queryList.Add(" CityID =" + access.CriteriaData.ToString());
        //                break;
        //            case 7://Location
        //                queryList.Add(" LocID =" + access.CriteriaData.ToString());
        //                break;
        //        }
        //    }
        //    if (queryList.Count == 1)
        //    {
        //        query = query + queryList[0];
        //    }
        //    else if (queryList.Count > 1)
        //    {
        //        for (int i = 0; i < queryList.Count - 1; i++)
        //        {
        //            query = query + queryList[i] + " or ";
        //        }
        //        query = query + queryList[queryList.Count - 1];
        //    }


        //    return query;
        //}

       
        internal string QueryForReportsDepartment(User LoggedInUser)
        {
            string query = " where ";
            TAS2013Entities db = new TAS2013Entities();
            List<UserRoleData> uAcc = new List<UserRoleData>();
            uAcc = db.UserRoleDatas.Where(aa => aa.RoleUserID == LoggedInUser.UserID && aa.UserRoleLegend == "D").ToList();
            List<Department> depts = db.Departments.ToList();
            List<Section> secs = db.Sections.ToList();
            List<string> queryList = new List<string>();
            foreach (var access in uAcc)
            {
                switch (LoggedInUser.UserRoleD)
                {
                    case "G"://Super ADmin
                        query = "";
                        break;
                    case "v"://division
                        //list<department> dept = depts.where(aa => aa.divid == access.roledatavalue).tolist();
                        //foreach (var c in dept)
                        {
                            //querylist.add(" deptid =" + c.deptid);
                        }
                        break;
                    case "D"://dept
                        string deptID = depts.Where(aa => aa.DeptID == access.RoleDataValue).FirstOrDefault().DeptID.ToString();
                        queryList.Add(" DeptID =" + deptID);
                        break;
                    case "S"://section
                        deptID = secs.Where(aa => aa.SectionID == access.RoleDataValue).FirstOrDefault().DeptID.ToString();
                        queryList.Add(" DeptID =" + deptID);
                        break;
                }
            }
            if (queryList.Count == 1)
            {
                query = query + queryList[0];
            }
            else if (queryList.Count > 1)
            {
                for (int i = 0; i < queryList.Count - 1; i++)
                {
                    query = query + queryList[i] + " or ";
                }
                query = query + queryList[queryList.Count - 1];
            }
            return query;
        }
        #endregion
        public string MakeCustomizeQueryForEmpView(User _user)
        {
            string RoleQuery = "";
            string CatQuery = "";
            TAS2013Entities db = new TAS2013Entities();
            List<UserRoleData> userRoleDataD = new List<UserRoleData>();
            List<UserRoleData> userRoleDataL = new List<UserRoleData>();
            List<string> UserRoleString = new List<string>();
            //List<string> CategoryUser = new List<string>();
            //CategoryUser.Add(" where (CatID=1 ");
            //if (_user.ViewContractual == true)
            //{
            //    CategoryUser.Add(" CatID = 4 ");
            //}
            //if (_user.ViewPermanentMgm == true)
            //{
            //    CategoryUser.Add(" CatID = 2  ");
            //}
            //if (_user.ViewPermanentStaff == true)
            //{
            //    CategoryUser.Add(" CatID = 3  ");
            //}
            userRoleDataD = db.UserRoleDatas.Where(aa => aa.RoleUserID == _user.UserID && aa.UserRoleLegend=="D").ToList();
            userRoleDataL = db.UserRoleDatas.Where(aa => aa.RoleUserID == _user.UserID && aa.UserRoleLegend == "L").ToList();
            switch (_user.UserRoleD)
            {
                case "G"://Admin
                   
                    break;
                case "D"://Department
                    foreach (var urd in userRoleDataD)
                    {
                        UserRoleString.Add(" DeptID = " + urd.RoleDataValue + " ");
                    }
                    break;
                case "S"://Section
                    foreach (var urd in userRoleDataD)
                    {
                        UserRoleString.Add(" SecID = " + urd.RoleDataValue + " ");
                    }
                    break;
                case "V"://Division
                    foreach (var urd in userRoleDataD)
                    {
                        UserRoleString.Add(" DivID = " + urd.RoleDataValue + " ");
                    }
                    break;
            }
            switch (_user.UserRoleL)
            {
                case "A"://Admin

                    break;
                case "C"://City
                    foreach (var urd in userRoleDataL)
                    {
                        UserRoleString.Add(" CityID = " + urd.RoleDataValue + " ");
                    }
                    break;
                case "L"://Location
                    foreach (var urd in userRoleDataL)
                    {
                        UserRoleString.Add(" LocID = " + urd.RoleDataValue + " ");
                    }
                    break;
                case "R"://Region
                    foreach (var urd in userRoleDataL)
                    {
                        UserRoleString.Add(" RegionID = " + urd.RoleDataValue + " ");
                    }
                    break;
            }
            if (UserRoleString.Count == 1)
            {
                RoleQuery = " and (" + RoleQuery + UserRoleString[0] + " ) ";
            }
            else if (UserRoleString.Count > 1)
            {
                RoleQuery = RoleQuery + " and ( ";
                for (int i = 0; i < UserRoleString.Count - 1; i++)
                {
                    RoleQuery = RoleQuery + UserRoleString[i] + " or ";
                }
                RoleQuery = RoleQuery + UserRoleString[UserRoleString.Count - 1] + " ) ";
            }
            //if (CategoryUser.Count == 1)
            //    CatQuery = CatQuery + CategoryUser[0] + " ) ";
            //else if (CategoryUser.Count > 1)
            //{
            //    for (int i = 0; i < CategoryUser.Count - 1; i++)
            //    {
            //        CatQuery = CatQuery + CategoryUser[i] + " or ";
            //    }
            //    CatQuery = CatQuery + CategoryUser[CategoryUser.Count - 1] + " ) ";
            //}

            return CatQuery + RoleQuery;
        }
        
    }
}