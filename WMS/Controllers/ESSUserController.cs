using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Models;

namespace WMS.Controllers
{
    public class ESSUserController : Controller
    {
        private TAS2013Entities db = new TAS2013Entities();
        
        // GET: /ESSUser/ ----- returns the ESSUser View
        public ActionResult Index()
        {
            return View();
        }

        // GET: /ESSUser/ESSEmpsList -- Returns list of emps acc to the user info
        public JsonResult ESSEmpsList()
        {
            var collection = db.Emps.Select(x => new
            {
                EmpID = x.EmpID,
                EmpName = x.EmpName,
                EmpNo = x.EmpNo,
                //HasAccess = x.HasAccess
            });
            return Json(collection, JsonRequestBehavior.AllowGet);
        }

        // POST: /ESSUser -- post with selected Emps Data -- post back the selected emps to generate Users
        [HttpPost]
        public ActionResult Index(List<Emp> emps)
        {
            if (emps == null)
                return View();
            foreach (Emp emp in emps)
            {
                ManageSingleEmpUserInDB(emp.EmpID, true);
            }
            return View();
        }

        // GET: /ESSUser/  -- Get request to generate all users
        public ActionResult GenerateAll()
        {
            using (var context = new TAS2013Entities())
            {
                foreach (Emp emp in context.Emps)
                {
                    ManageSingleEmpUserInDB(emp.EmpID, true);
                } 
            }
            return Json("success", JsonRequestBehavior.AllowGet);
        }

        // Get: /ESSUser/GenerateESSUser -- Request to generate single user with ID
        public JsonResult GenerateESSUser(int EmpID)
        {
            string result = ManageSingleEmpUserInDB(EmpID, true);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // Get: /ESSUser/RestrictESSUser -- Request to restrict single user with ID
        public JsonResult RestrictESSUser(int EmpID)
        {
            string result = ManageSingleEmpUserInDB(EmpID, false);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private string ManageSingleEmpUserInDB(int EmpID, Boolean status)
        {
            User user;
            Emp emp = db.Emps.First(ep => ep.EmpID == EmpID);
            if(db.Users.Where(usr => usr.EmpID == EmpID).Count() > 0)
                user = db.Users.First(us => us.EmpID == EmpID);
            else
            {
                user = new User()
                {
                    UserName = emp.EmpNo,
                    Password = "Password",
                    EmpID = EmpID
                };
                db.Users.Add(user);
            }
            user.Status = status;
            //emp.HasAccess = status;
            if (db.SaveChanges() > 0)
                return "success";
            else
                return "error";
        }            
	}
}