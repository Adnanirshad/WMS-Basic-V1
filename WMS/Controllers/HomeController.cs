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

        //public ActionResult TestData()
        //{
        //    short Code = 7;

        //    using (var db = new TAS2013Entities())
        //    {
        //        db.Configuration.ProxyCreationEnabled = false;
        //        List<DailySumSection> secs = db.DailySumSections.Where(aa => aa.SectionID == Code).ToList();
        //        List<Section> _secList = db.Sections.ToList();
        //        //return Json(movies, JsonRequestBehavior.AllowGet);
        //        //string output = SerializeObject(secs);
        //        if (HttpContext.Request.IsAjaxRequest())
        //            return Json(ConvertToJsonList(secs, _secList), JsonRequestBehavior.AllowGet);
        //    }
        //    return RedirectToAction("Index");
        //}

        //private object ConvertToJsonList(List<DailySumSection> _sec, List<Section> _secList)
        //{
        //    var data = new List<object>();
        //    foreach (var item in _sec)
        //    {
        //        string SecName = _secList.Where(aa => aa.SectionID == _sec.FirstOrDefault().SectionID).FirstOrDefault().SectionName;
        //        data.Add(new
        //        {
        //            SectionName = SecName,
        //            Date = item.SummaryDate.Value.ToString("dd-MMM-yyyy"),
        //            TotalEmp = item.TotalEmp,
        //        });
        //    }
        //    return data;
        //}


        // Usage:
    }
}