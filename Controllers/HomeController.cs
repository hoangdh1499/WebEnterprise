using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebEnterprise.Models;

namespace WebEnterprise.Controllers
{
    public class HomeController : Controller
    {
        private G5EnterpriseDBEntities db = new G5EnterpriseDBEntities();
        public ActionResult Index(string searchString)
        {
            var ct = from m in db.CTents select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                ct = ct.Where(s => s.CTname.Contains(searchString));
            }
            return View(ct);

        }

        public ActionResult About()
        {
            ViewBag.Message = "Application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact page.";

            return View();
        }
    }
}