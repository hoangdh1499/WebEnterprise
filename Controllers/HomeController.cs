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
        private G5EnterpriseDBEntities3 db = new G5EnterpriseDBEntities3();
        public ActionResult Index(string searchString)
        {
            var ct = from m in db.ConTents select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                ct = ct.Where(s => s.CTName.Contains(searchString));
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