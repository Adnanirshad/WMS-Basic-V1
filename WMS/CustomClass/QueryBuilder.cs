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
            CategoryUser.Add(" where (CatID=1 ");
            if (_user.ViewContractual == true)
            {
                CategoryUser.Add(" CatID = 4 ");
            }
            if (_user.ViewPermanentMgm == true)
            {
                CategoryUser.Add(" CatID = 2  ");
            }
            if (_user.ViewPermanentStaff == true)
            {
                CategoryUser.Add(" CatID = 3  ");
            }
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

        internal string QueryForRegionFromCitiesForLinq(IEnumerable<City> cities)
        {

            String query = "";
            int d=1;
            foreach (var city in cities)
            {
              if(d <cities.Count())
                query = query + "RegionID="+city.RegionID+" or ";
            d++;
            }
            query = query + "RegionID=" + cities.Last().RegionID;
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
    }
}