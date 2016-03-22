using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WMS.Controllers
{
    public class JobCardController : Controller
    {
        //
        // GET: /JobCard/
        public ActionResult Index()
        {
            return RedirectToAction("JobCardList");
        }
        public ActionResult JobCardList()
        {
            return View();
        }
	}
}