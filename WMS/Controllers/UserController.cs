using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WMS.Models;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using WMS.Controllers.Filters;
using WMS.CustomClass;

namespace WMS.Controllers
{
    [CustomControllerAttributes]
    public class UserController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();

        // GET: /User/
        public ActionResult Index()
        {
            int NoOfUsres = Convert.ToInt32(GlobalVaribales.NoOfUsers);
            var users = db.Users.Where(aa=>aa.Deleted==false);
            var usr = users.Take(NoOfUsres);
            return View(usr.ToList());
        }

        // GET: /User/Details/5
        [CustomActionAttribute]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [CustomActionAttribute]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [CustomActionAttribute]
        [ValidateAntiForgeryToken]
        public ActionResult Create( [Bind(Include = "UserID,UserName,Password,EmpID,DateCreated,Name,Status,Department,CanEdit,CanDelete,CanAdd,CanView,RoleID,MHR,MDevice,MLeave,MDesktop,MEditAtt,MUser,MOption,MRoster,MRDailyAtt,MRLeave,MRMonthly,MRAudit,MRManualEditAtt,MREmployee,MRDetail,MRSummary,MRGraph,ViewPermanentStaff,ViewPermanentMgm,ViewContractual,ViewLocation,LocationID,MProcess")] User user)
        {
            if (db.Users.Where(aa => aa.Status == true && aa.Deleted==false).Count() >= Convert.ToInt32(GlobalVaribales.NoOfUsers))
                ModelState.AddModelError("UserName", "Your Users has exceeded from License, Please upgrade your license");
            user.CanAdd = (bool)ValueProvider.GetValue("CanAdd").ConvertTo(typeof(bool));
            user.CanEdit = (bool)ValueProvider.GetValue("CanEdit").ConvertTo(typeof(bool));
            user.CanDelete = (bool)ValueProvider.GetValue("CanDelete").ConvertTo(typeof(bool));
            user.CanView = (bool)ValueProvider.GetValue("CanView").ConvertTo(typeof(bool));
            user.MHR = (bool)ValueProvider.GetValue("MHR").ConvertTo(typeof(bool));
            user.MOption = (bool)ValueProvider.GetValue("MOption").ConvertTo(typeof(bool));
            user.MLeave = (bool)ValueProvider.GetValue("MLeave").ConvertTo(typeof(bool));
            user.MRoster = (bool)ValueProvider.GetValue("MRoster").ConvertTo(typeof(bool));
            user.MUser = (bool)ValueProvider.GetValue("MUser").ConvertTo(typeof(bool));
            user.MDevice = (bool)ValueProvider.GetValue("MDevice").ConvertTo(typeof(bool));
            user.MDesktop = (bool)ValueProvider.GetValue("MDesktop").ConvertTo(typeof(bool));
            user.MEditAtt = (bool)ValueProvider.GetValue("MEditAtt").ConvertTo(typeof(bool));
            user.MProcess = (bool)ValueProvider.GetValue("MProcess").ConvertTo(typeof(bool));
            user.MRLeave = (bool)ValueProvider.GetValue("MRLeave").ConvertTo(typeof(bool));
            user.MRDailyAtt = (bool)ValueProvider.GetValue("MRDailyAtt").ConvertTo(typeof(bool));
            user.MRMonthly = (bool)ValueProvider.GetValue("MRMonthly").ConvertTo(typeof(bool));
            user.MRAudit = (bool)ValueProvider.GetValue("MRAudit").ConvertTo(typeof(bool));
            user.MRManualEditAtt = (bool)ValueProvider.GetValue("MRManualEditAtt").ConvertTo(typeof(bool));
            user.MRDetail = (bool)ValueProvider.GetValue("MRDetail").ConvertTo(typeof(bool));
            if (Request.Form["UserType"].ToString() == "true")
                user.UserType = "Admin";
            else
                user.UserType = "Restricted";
            user.DateCreated = DateTime.Today;
            if (ModelState.IsValid)
            {
                user.Deleted = false;
                db.Users.Add(user);
                db.SaveChanges();
                if (user.UserType == "Restricted")
                {
                    List<Section> secs = new List<Section>();
                    secs = db.Sections.ToList();
                    int count = Convert.ToInt32(Request.Form["uSectionCount"]);
                    for (int i = 1; i <= count; i++)
                    {
                        string uSecID = "uSection" + i;
                        string secName = Request.Form[uSecID].ToString();
                        int locID = secs.Where(aa => aa.SectionName == secName).FirstOrDefault().SectionID;
                        UserSection uSec = new UserSection();
                        uSec.UserID = user.UserID;
                        uSec.SecID = (short)locID;
                        db.UserSections.Add(uSec);
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Index");
            }
            return View(user);
        }


        // GET: /User/Edit/5
        [CustomActionAttribute]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            //ViewBag.RoleIDL = new SelectList(db.UserRoles, "RoleID", "RoleName", user.UserRoleL);

            ViewBag.LocationID = new SelectList(db.Locations, "LocID", "LocName");
            


            return View(user);
        }

        // POST: /User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [CustomActionAttribute]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserID,UserName,Password,EmpID,DateCreated,Name,Status,Department,CanEdit,CanDelete,CanAdd,CanView,RoleID,MHR,MDevice,MLeave,MDesktop,MEditAtt,MUser,MOption,MRDailyAtt,MRLeave,MRMonthly,MRAudit,MRManualEditAtt,MREmployee,MRDetail,MRSummary,MRGraph,ViewPermanentStaff,ViewPermanentMgm,ViewContractual,ViewLocation,LocationID,MProcess")] User user)
        {
            bool check = false;
            user.CanAdd = (bool)ValueProvider.GetValue("CanAdd").ConvertTo(typeof(bool));
            user.CanEdit = (bool)ValueProvider.GetValue("CanEdit").ConvertTo(typeof(bool));
            user.CanDelete = (bool)ValueProvider.GetValue("CanDelete").ConvertTo(typeof(bool));
            user.CanView = (bool)ValueProvider.GetValue("CanView").ConvertTo(typeof(bool));
            user.MHR = (bool)ValueProvider.GetValue("MHR").ConvertTo(typeof(bool));
            user.MOption = (bool)ValueProvider.GetValue("MOption").ConvertTo(typeof(bool));
            user.MLeave = (bool)ValueProvider.GetValue("MLeave").ConvertTo(typeof(bool));
            user.MRoster = (bool)ValueProvider.GetValue("MRoster").ConvertTo(typeof(bool));
            user.MUser = (bool)ValueProvider.GetValue("MUser").ConvertTo(typeof(bool));
            user.MDevice = (bool)ValueProvider.GetValue("MDevice").ConvertTo(typeof(bool));
            user.MDesktop = (bool)ValueProvider.GetValue("MDesktop").ConvertTo(typeof(bool));
            user.MEditAtt = (bool)ValueProvider.GetValue("MEditAtt").ConvertTo(typeof(bool));
            user.MProcess = (bool)ValueProvider.GetValue("MProcess").ConvertTo(typeof(bool));
            user.MRLeave = (bool)ValueProvider.GetValue("MRLeave").ConvertTo(typeof(bool));
            user.MRDailyAtt = (bool)ValueProvider.GetValue("MRDailyAtt").ConvertTo(typeof(bool));
            user.MRMonthly = (bool)ValueProvider.GetValue("MRMonthly").ConvertTo(typeof(bool));
            user.MRAudit = (bool)ValueProvider.GetValue("MRAudit").ConvertTo(typeof(bool));
            user.MRManualEditAtt = (bool)ValueProvider.GetValue("MRManualEditAtt").ConvertTo(typeof(bool));
            user.MRDetail = (bool)ValueProvider.GetValue("MRDetail").ConvertTo(typeof(bool));
            if (Request.Form["UserType"].ToString() == "true")
                user.UserType = "Admin";
            else
                user.UserType = "Restricted";
            user.DateCreated = DateTime.Today;
            user.Deleted = false;
            // db.Entry(user).State = EntityState.Modified;
            db.Entry(user).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            //User Section
            int count = Convert.ToInt32(Request.Form["uSectionCount"]);
            List<Section> secs = new List<Section>();
            List<UserSection> userLocs = db.UserSections.Where(aa => aa.UserID == user.UserID).ToList();
            secs = db.Sections.ToList();
            List<int> currentLocIDs = new List<int>();
            foreach (var uloc in userLocs)
            {
                UserSection ul = db.UserSections.First(aa => aa.SecID == uloc.SecID);
                db.UserSections.Remove(ul);
                db.SaveChanges();
            }
            userLocs = new List<UserSection>();
            if (user.UserType=="Restricted")
            {
                for (int i = 1; i <= count; i++)
                {
                    string uLocID = "uSection" + i;
                    string LocName = Request.Form[uLocID].ToString();
                    int locID = secs.Where(aa => aa.SectionName == LocName).FirstOrDefault().SectionID;
                    currentLocIDs.Add(locID);
                    if (userLocs.Where(aa => aa.SecID == locID).Count() > 0)
                    {

                    }
                    else
                    {
                        UserSection uloc = new UserSection();
                        uloc.UserID = user.UserID;
                        uloc.SecID = (short)locID;
                        db.UserSections.Add(uloc);
                        userLocs.Add(uloc);
                        db.SaveChanges();
                    }
                } 
            }
           return RedirectToAction("Index");
        }
        // GET: /User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: /User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            user.Deleted = true;
            //db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult UserSectionList()
        {
            var states = db.Sections.ToList();
            return Json(new SelectList(
                            states.ToArray(),
                            "SectionID",
                            "SectionName")
                       , JsonRequestBehavior.AllowGet);
        }

        public ActionResult SelectedUserSecList(int id)
        {
            List<UserSection> userLoc = db.UserSections.Where(aa => aa.UserID == id).ToList();
            List<Section> _locs = db.Sections.ToList();
            List<Section> locs = new List<Section>();
            var uloc = new List<UserSection>();

            foreach (var loc in userLoc)
            {
                Section ll = db.Sections.FirstOrDefault(aa => aa.SectionID == loc.SecID);
                locs.Add(ll);
            }
            return Json(new SelectList(
                           locs.ToArray(),
                           "SectionID",
                           "SectionName")
                      , JsonRequestBehavior.AllowGet);
        }

    }



    public class ADUsersAttributes
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string PrincipleName { get; set; }
        public string SAMName { get; set; }
        public string DistingushedName { get; set; }
        public bool Checked { get; set; }

    }
    public class ADUsersModel
    {
        public List<ADUsersAttributes> _ADUsersAttributes { get; set; }
    }

}
