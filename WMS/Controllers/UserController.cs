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

namespace WMS.Controllers
{
    [CustomControllerAttributes]
    public class UserController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();

        // GET: /User/
        public ActionResult Index()
        {
            User LoggedInUser = Session["LoggedUser"] as User;
            var users = db.Users;
            return View(users.ToList());
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


        private ADUsersModel GetADUsers()
        {
            ADUsersModel _objstudentmodel = new ADUsersModel();
            _objstudentmodel._ADUsersAttributes = new List<ADUsersAttributes>();
            //using (var context = new PrincipalContext(ContextType.Domain, "fatima-group.com", "ffl.ithelpdesk@fatima-group.com", "fatima@0202"))
            using (var context = new PrincipalContext(ContextType.Domain, "fatima-group.com", "wms.ffl@fatima-group.com", "fflWMS.net"))
            {
                using (var searcher = new PrincipalSearcher(new UserPrincipal(context)))
                {
                    int i = 1;
                    foreach (var result in searcher.FindAll())
                    {
                        DirectoryEntry de = result.GetUnderlyingObject() as DirectoryEntry;
                        string name = result.Name;
                        string displayName = result.DisplayName;
                        string userPrincipleName = result.UserPrincipalName;
                        string samAccountName = result.SamAccountName;
                        string distinguishedName = result.DistinguishedName;
                        //label1.Text += "Name:    " + result.Name;
                        //label1.Text += "      account name   :    " + result.UserPrincipalName;
                        //label1.Text += "      Server:    " + result.Context.ConnectedServer + "\r";
                        _objstudentmodel._ADUsersAttributes.Add(new ADUsersAttributes
                        {
                            ID = i,
                            UserName = name,
                            DisplayName = displayName,
                            PrincipleName = userPrincipleName,
                            DistingushedName = distinguishedName,
                            SAMName = samAccountName
                        });
                        i++;
                    }
                }
            }
            return _objstudentmodel;
        }
        // POST: /User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [CustomActionAttribute]
        [ValidateAntiForgeryToken]
        public ActionResult Create( [Bind(Include = "UserID,UserName,Password,EmpID,DateCreated,Name,Status,Department,CanEdit,CanDelete,CanAdd,CanView,RoleID,MHR,MDevice,MLeave,MDesktop,MEditAtt,MUser,MOption,MRoster,MRDailyAtt,MRLeave,MRMonthly,MRAudit,MRManualEditAtt,MREmployee,MRDetail,MRSummary,MRGraph,ViewPermanentStaff,ViewPermanentMgm,ViewContractual,ViewLocation,LocationID,MProcess")] User user)
        {
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
            user.DateCreated = DateTime.Today;

                db.Users.Add(user);
                db.SaveChanges();
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
            
            
            //user.UserRoleD = Request.Form["RoleID"].ToString();


            if (Request.Form["Status"].ToString() == "true")
                user.Status = true;
            else
                user.Status = false;

            if (Request.Form["CanEdit"].ToString() == "true")
                user.CanEdit = true;
            else
                user.CanEdit = false;

            if (Request.Form["CanDelete"].ToString() == "true")
                user.CanDelete = true;
            else
                user.CanDelete = false;

            if (Request.Form["CanAdd"].ToString() == "true")
                user.CanAdd = true;
            else
                user.CanAdd = false;

            if (Request.Form["CanView"].ToString() == "true")
                user.CanView = true;
            else
                user.CanView = false;
            if (Request.Form["MUser"].ToString() == "true")
                user.MUser = true;
            else
                user.MUser = false;
            if (Request.Form["MRoster"].ToString() == "true")
                user.MRoster = true;
            else
                user.MRoster = false;
            if (Request.Form["MHR"].ToString() == "true")
                user.MHR = true;
            else
                user.MHR = false;
            //if (Request.Form["MOption"].ToString() == "true")
            //    user.MOption = true;
            //else
            //    user.MOption = false;
            if (Request.Form["MDevice"].ToString() == "true")
                user.MDevice = true;
            else
                user.MDevice = false;
            if (Request.Form["MDesktop"].ToString() == "true")
                user.MDesktop = true;
            else
                user.MDesktop = false;
            if (Request.Form["MEditAtt"].ToString() == "true")
                user.MEditAtt = true;
            else
                user.MEditAtt = false;
            if (Request.Form["MLeave"].ToString() == "true")
                user.MLeave = true;
            else
                user.MLeave = false;
            if (Request.Form["MRLeave"].ToString() == "true")
                user.MRLeave = true;
            else
                user.MRLeave = false;
            if (Request.Form["MRDailyAtt"].ToString() == "true")
                user.MRDailyAtt = true;
            else
                user.MRDailyAtt = false;
            if (Request.Form["MRMonthly"].ToString() == "true")
                user.MRMonthly = true;
            else
                user.MRMonthly = false;
            if (Request.Form["MRAudit"].ToString() == "true")
                user.MRAudit = true;
            else
                user.MRAudit = false;
            if (Request.Form["MRManualEditAtt"].ToString() == "true")
                user.MRManualEditAtt = true;
            else
                user.MRManualEditAtt = false;
            if (Request.Form["MProcess"].ToString() == "true")
                user.MProcess = true;
            else
                user.MProcess = false;
            if (Request.Form["MREmployee"].ToString() == "true")
                user.MREmployee = true;
            else
                user.MREmployee = false;
            if (Request.Form["MRDetail"].ToString() == "true")
                user.MRDetail = true;
            else
                user.MRDetail = false;
            
            if (check == false)
            {

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                int count = Convert.ToInt32(Request.Form["uLocationCount"]);
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
                for (int i = 1; i <= count; i++)
                {
                    string uLocID = "uLocation" + i;
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
                
                return RedirectToAction("Index");

            }

            //ViewBag.RoleID = new SelectList(db.UserRoles, "RoleID", "RoleName", user.UserRole);
             

            // todo to be verified - user data missing
            ViewBag.LocationID = new SelectList(db.Locations, "LocID", "LocName");
            
            return View(user);
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
            db.Users.Remove(user);
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

        public ActionResult SelectedUserLocList(int id)
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
