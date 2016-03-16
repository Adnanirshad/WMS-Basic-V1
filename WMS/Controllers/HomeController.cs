using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using System.Xml.Serialization;
using WMS.Controllers.Filters;
using WMS.HelperClass;
using WMS.Models;
using System.DirectoryServices;
using System.Linq.Dynamic;
using System.DirectoryServices.AccountManagement;
using System.Data;
using System.Drawing;

namespace WMS.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        public ActionResult Index()
        {
            try
            {
                if (Session["LogedUserID"] == null)
                {
                    Session["LogedUserID"] = "";
                    Session["Role"] = "";
                    Session["MHR"] = "0";
                    Session["MDevice"] = "0";
                    Session["MLeave"] = "0";
                    Session["MEditAtt"] = "0";
                    Session["MUser"] = "0";
                    Session["LogedUserFullname"] = "";
                    Session["UserCompany"] = "";
                    Session["MRDailyAtt"] = "0";
                    Session["MRLeave"] = "0";
                    Session["MRMonthly"] = "0";
                    Session["MRAudit"] = "0";
                    Session["MRManualEditAtt"] = "0";
                    Session["MREmployee"] = "0";
                    Session["MRoster"] = "0";
                    Session["MRDetail"] = "0";
                    Session["MRSummary"] = "0";
                    Session["MProcess"] = "0";
                    return View();
                }
                else if (Session["LogedUserID"].ToString() == "")
                {
                    return View();
                }
                else
                {
                    return View("AfterLogin");
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        private void SaveImage()
        {
            //image to byteArray
            Image img = Image.FromFile("E:\\air.png");
            byte[] bArr = imgToByteArray(img);
            //byte[] bArr = imgToByteConverter(img);
            //Again convert byteArray to image and displayed in a picturebox
            TAS2013Entities ctx = new TAS2013Entities();
            Option oo = new Option();
            oo = ctx.Options.First(aa=>aa.ID==1);
            oo.CompanyLogo = bArr;
            ctx.SaveChanges();
        }
        //convert image to bytearray
        public byte[] imgToByteArray(Image img)
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                img.Save(mStream, img.RawFormat);
                return mStream.ToArray();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User u)
        {
            try
            {
                      if (ModelState.IsValid) // this is check validity
                      {
                          using (TAS2013Entities dc = new TAS2013Entities())
                          {
                              var v = dc.Users.Where(a => a.UserName.Equals(u.UserName) && a.Password==u.Password && a.Status == true).FirstOrDefault();
                              //login for emplioyee
                              if (v != null && v.UserRoleD == "E" && v.UserRoleL == null)
                             {
                                 Session["LogedUserID"] = v.UserID.ToString();
                                 Session["LogedUserFullname"] = v.UserName;
                                 Session["LoggedUser"] = v;
                                 return RedirectToAction("Create", "ESSLeave", new { area = "ESS" });
                              }
                              else
                              if (v != null)
                              {

                                  Session["MDevice"] = "0";
                                  Session["MHR"] = "0";
                                  Session["MDevice"] = "0";
                                  Session["MLeave"] = "0";
                                  Session["MEditAtt"] = "0";
                                  Session["MUser"] = "0";
                                  Session["LogedUserFullname"] = "";
                                  Session["UserCompany"] = "";
                                  Session["MRDailyAtt"] = "0";
                                  Session["MRLeave"] = "0";
                                  Session["MRMonthly"] = "0";
                                  Session["MRAudit"] = "0";
                                  Session["MRManualEditAtt"] = "0";
                                  Session["MREmployee"] = "0";
                                  Session["MRDetail"] = "0";
                                  Session["MRSummary"] = "0";
                                  Session["LogedUserID"] = v.UserID.ToString();
                                  Session["LogedUserFullname"] = v.UserName;
                                  Session["LoggedUser"] = v;
                                  if (v.MHR == true)
                                      Session["MHR"] = "1";
                                  if (v.MDevice == true)
                                      Session["MDevice"] = "1";
                                  if (v.MLeave == true)
                                      Session["MLeave"] = "1";
                                  if (v.MEditAtt == true)
                                      Session["MEditAtt"] = "1";
                                  if (v.MUser == true)
                                      Session["MUser"] = "1";
                                  if (v.MRDailyAtt == true)
                                      Session["MRDailyAtt"] = "1";
                                  if (v.MRLeave == true)
                                      Session["MRLeave"] = "1";
                                  if (v.MRMonthly == true)
                                      Session["MRMonthly"] = "1";
                                  if (v.MRAudit == true)
                                      Session["MRAudit"] = "1";
                                  if (v.MRManualEditAtt == true)
                                      Session["MRManualEditAtt"] = "1";
                                  if (v.MProcess == true)
                                      Session["MProcess"] = "1";
                                  if (v.MREmployee == true)
                                      Session["MREmployee"] = "1";
                                  if (v.MRDetail == true)
                                      Session["MRDetail"] = "1";
                                  if (v.MRSummary == true)
                                      Session["MRSummary"] = "1";
                                  else
                                      Session["MRSummary"] = "0";
                                  if (v.MRoster == true)
                                      Session["MRoster"] = "1";
                                  if(v.MRGraph==true)
                                      Session["MGraph"] = "1";
                                  else
                                      Session["MGraph"] = "0";
                                  HelperClass.MyHelper.SaveAuditLog(v.UserID, (byte)MyEnums.FormName.LogIn, (byte)MyEnums.Operation.LogIn, DateTime.Now);
                                  return RedirectToAction("AfterLogin");
                              }
                              else
                              {
                                  int LoginCount = 0;
                                  bool successOnConversion = int.TryParse(Session["LoginCount"] as string, out LoginCount);
                                  if (successOnConversion == true)
                                  {
                                      LoginCount++;
                                      Session["LoginCount"] = LoginCount + "";
                                  }
                                  else
                                  {
                                      Session["LoginCount"] = "1";
                                  }


                              }
                          }
                      }
                  return RedirectToAction("index");

            }
            catch (Exception ex)
            {
                ViewBag.Message = "There seems to be a problem with the network. Please contact your network administrator";
                return RedirectToAction("Index");
            }
        }
        public ActionResult AfterLogin()
        {
            try
            {
                if (Session["LogedUserID"] != null)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                return View("Index");
            }
        }
        public ActionResult Logout()
        {
            try
            {
                int _userID = Convert.ToInt32(Session["LogedUserID"].ToString());
                HelperClass.MyHelper.SaveAuditLog(_userID, (byte)MyEnums.FormName.LogIn, (byte)MyEnums.Operation.LogOut, DateTime.Now);
                Session["LogedUserID"] = "";
                Session["LogedUserFullname"] = null;
                Session["LogedUserRegion"] = null;
                Session["LoggedUser"] = null;
                Session["MHR"] = null;
                Session["MDevice"] = null;
                Session["MLeave"] = null;
                Session["MEditAtt"] = null;
                Session["MUser"] = null;
                Session["MRDailyAtt"] = null;
                Session["MRLeave"] = null;
                Session["MRMonthly"] = null;
                Session["MRAudit"] = null;
                Session["MRManualEditAtt"] = null;
                Session["MREmployee"] = null;
                Session["MRDetail"] = null;
                Session["MRSummary"] = null;
                Session["FiltersModel"] = new WMSLibrary.FiltersModel();
                Session["LoginCount"] = null;
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Index");
            }
        }
       
        private string SerializeObject(object myObject)
        {
            var stream = new MemoryStream();
            var xmldoc = new XmlDocument();
            var serializer = new XmlSerializer(myObject.GetType());
            using (stream)
            {
                serializer.Serialize(stream, myObject);
                stream.Seek(0, SeekOrigin.Begin);
                xmldoc.Load(stream);
            }

            return xmldoc.InnerXml;
        }

        private object DeSerializeObject(object myObject, Type objectType)
        {
            var xmlSerial = new XmlSerializer(objectType);
            var xmlStream = new StringReader(myObject.ToString());
            return xmlSerial.Deserialize(xmlStream);
        }

        public ActionResult GetDahboard()
        {
            DateTime dt = DateTime.Today.AddDays(-1);
            //DateTime dt = new DateTime(2016, 02, 02);
            DashboardValues dv = new DashboardValues();
            TAS2013Entities db = new TAS2013Entities();
            List<DailySummary> ds = new List<DailySummary>();
            //List<JobCardEmp> jcEmp = new List<JobCardEmp>();
            //if (dt.DayOfWeek == DayOfWeek.Saturday)
            //    dt = dt.AddDays(-1);
            //if (dt.DayOfWeek == DayOfWeek.Sunday)
            //    dt = dt.AddDays(-2);
            //jcEmp = db.JobCardEmps.Where(aa => aa.Dated == dt).ToList();
            //List<JobCardTime> jcEmpT = new List<JobCardTime>();
            //jcEmpT = db.JobCardTimes.Where(aa => aa.DutyDate == dt).ToList();
            ds = db.DailySummaries.Where(aa => aa.Date == dt && aa.Criteria == "C").ToList();
            if (ds.Count > 0)
            {
                dv.DateTime = dt.ToString("dd-MMM-yyy");
                dv.TotalEmps = (short)ds.FirstOrDefault().TotalEmps;
                dv.Present = (short)ds.FirstOrDefault().PresentEmps;
                dv.Absent = (short)ds.FirstOrDefault().AbsentEmps;
                dv.Leaves = (short)ds.FirstOrDefault().LvEmps;
                dv.LateIn = (short)ds.FirstOrDefault().LIEmps;
                dv.LateOut = (short)ds.FirstOrDefault().LOEmps;
                dv.EarlyIn = (short)ds.FirstOrDefault().EIEmps;
                dv.EarlyOut = (short)ds.FirstOrDefault().EOEmps;
                dv.OverTime = (short)ds.FirstOrDefault().OTEmps;
                dv.ShortLeaves = (short)ds.FirstOrDefault().ShortLvEmps;
                //dv.JCOfficalAssignment = jcEmp.Where(aa => aa.WrkCardID == 11).ToList().Count + jcEmpT.Where(aa => aa.JobCardID == 11).Count();
                //dv.JCTour = jcEmp.Where(aa => aa.WrkCardID == 9).ToList().Count + jcEmpT.Where(aa => aa.JobCardID == 9).Count();
                //dv.JCTraining = jcEmp.Where(aa => aa.WrkCardID == 8).ToList().Count + jcEmpT.Where(aa => aa.JobCardID == 8).Count();
                //dv.JCVisit = jcEmp.Where(aa => aa.WrkCardID == 10).ToList().Count + jcEmpT.Where(aa => aa.JobCardID == 10).Count();
                dv.EWork = (int)(ds.FirstOrDefault().ExpectedWorkMins / 60);
                dv.AWork = (int)(ds.FirstOrDefault().ActualWorkMins / 60);
                dv.LWork = (int)(ds.FirstOrDefault().LossWorkMins / 60);
            }
            else
            {
                dv.DateTime = dt.ToString("dd-MMM-yyy");
                dv.TotalEmps = 0;
                dv.Present = 0;
                dv.Absent = 0;
                dv.Leaves = 0;
                dv.LateIn = 0;
                dv.LateOut = 0;
                dv.EarlyIn = 0;
                dv.EarlyOut = 0;
                dv.OverTime = 0;
                dv.ShortLeaves = 0;
                //dv.JCOfficalAssignment = jcEmp.Where(aa => aa.WrkCardID == 11).ToList().Count;
                //dv.JCTour = jcEmp.Where(aa => aa.WrkCardID == 9).ToList().Count;
                //dv.JCTraining = jcEmp.Where(aa => aa.WrkCardID == 8).ToList().Count;
                //dv.JCVisit = jcEmp.Where(aa => aa.WrkCardID == 10).ToList().Count;
            }
            if (HttpContext.Request.IsAjaxRequest())
                return Json(dv
                           , JsonRequestBehavior.AllowGet);

            return RedirectToAction("Index");
        }
        public ActionResult GetDahboardGraph()
        {
            //DateTime dtE =  new DateTime(2016,02,02);
            DateTime dtE = DateTime.Today.AddDays(-1);
            DateTime dtS = dtE.AddDays(-31);
            TAS2013Entities db = new TAS2013Entities();
            List<DailySummary> ds = new List<DailySummary>();
            List<DailySummary> dsTemp = new List<DailySummary>();
            DashboardGraph dg = new DashboardGraph();
            ds = db.DailySummaries.Where(aa => aa.Date >= dtS && aa.Date <= dtE && aa.Criteria == "C").ToList();
            if (ds.Count > 0)
            {
                dg.DateTime1 = ds[0].Date.Value.ToString("dd-MMM");
                dg.LateIn1 = (int)ds[0].LIEmps;

                dg.DateTime2 = ds[2].Date.Value.ToString("dd-MMM");
                dg.LateIn2 = (int)ds[2].LIEmps;

                dg.DateTime3 = ds[4].Date.Value.ToString("dd-MMM");
                dg.LateIn3 = (int)ds[4].LIEmps;

                dg.DateTime4 = ds[7].Date.Value.ToString("dd-MMM");
                dg.LateIn4 = (int)ds[7].LIEmps;

                dg.DateTime5 = ds[10].Date.Value.ToString("dd-MMM");
                dg.LateIn5 = (int)ds[10].LIEmps;

                dg.DateTime6 = ds[12].Date.Value.ToString("dd-MMM");
                dg.LateIn6 = (int)ds[12].LIEmps;

                dg.DateTime7 = ds[15].Date.Value.ToString("dd-MMM");
                dg.LateIn7 = (int)ds[15].LIEmps;

                dg.DateTime8 = ds[17].Date.Value.ToString("dd-MMM");
                dg.LateIn8 = (int)ds[17].LIEmps;

                dg.DateTime9 = ds[19].Date.Value.ToString("dd-MMM");
                dg.LateIn9 = (int)ds[19].LIEmps;

                dg.DateTime10 = ds[ds.Count - 1].Date.Value.ToString("dd-MMM");
                dg.LateIn10 = (int)ds[ds.Count - 1].LIEmps;
            }
            else
            {

            }
            if (HttpContext.Request.IsAjaxRequest())
                return Json(dg
                           , JsonRequestBehavior.AllowGet);

            return RedirectToAction("Index");
        }
    }
    public class DashboardValues
    {
        public string DateTime { get; set; }
        public int TotalEmps { get; set; }
        public int Present { get; set; }
        public int Absent { get; set; }
        public int Leaves { get; set; }
        public int LateIn { get; set; }
        public int LateOut { get; set; }
        public int EarlyIn { get; set; }
        public int EarlyOut { get; set; }
        public int OverTime { get; set; }
        public int ShortLeaves { get; set; }
        public int JCTour { get; set; }
        public int JCVisit { get; set; }
        public int JCTraining { get; set; }
        public int JCOfficalAssignment { get; set; }
        public int EWork { get; set; }
        public int AWork { get; set; }
        public int LWork { get; set; }
    }

    public class DashboardGraph
    {
        public string DateTime1 { get; set; }
        public int LateIn1 { get; set; }
        public string DateTime2 { get; set; }
        public int LateIn2 { get; set; }
        public string DateTime3 { get; set; }
        public int LateIn3 { get; set; }
        public string DateTime4 { get; set; }
        public int LateIn4 { get; set; }
        public string DateTime5 { get; set; }
        public int LateIn5 { get; set; }
        public string DateTime6 { get; set; }
        public int LateIn6 { get; set; }
        public string DateTime7 { get; set; }
        public int LateIn7 { get; set; }
        public string DateTime8 { get; set; }
        public int LateIn8 { get; set; }
        public string DateTime9 { get; set; }
        public int LateIn9 { get; set; }
        public string DateTime10 { get; set; }
        public int LateIn10 { get; set; }
    }
}