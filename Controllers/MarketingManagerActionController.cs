using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebEnterprise.Models;
namespace WebEnterprise.Controllers
{
    public class MarketingManagerActionController : Controller
    {
        private G5EnterpriseDBEntities db = new G5EnterpriseDBEntities();
        public ActionResult MMprofile()
        {
            var UserName = User.Identity.Name;
            var userId = UserName;

            var MM = (from a in db.MarketingManagers where a.UserName.ToString().Equals(userId) select a).FirstOrDefault();
            return View(MM);
        }
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarketingManager marketingManager = db.MarketingManagers.Find(id);
            if (marketingManager == null)
            {
                return HttpNotFound();
            }
            ViewBag.MMID = (from i in db.MarketingManagers where i.UserName == User.Identity.Name select i.MMID).FirstOrDefault();
            ViewBag.uName = (from m in db.MarketingManagers where m.UserName == User.Identity.Name select m.UserName).FirstOrDefault();
            
            //ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "FacultyName", marketingCoordinator.FacultyID);
            return View(marketingManager);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MarketingManager marketingManager)
        {
            var mID = (from i in db.MarketingManagers where i.UserName == User.Identity.Name select i.MMID).FirstOrDefault();
            var uName = (from m in db.MarketingManagers where m.UserName == User.Identity.Name select m.UserName).FirstOrDefault();
            if (ModelState.IsValid)
            {
                marketingManager.MMID = mID;
               
                marketingManager.UserName = uName;
                db.Entry(marketingManager).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MMProfile");
            }

            
            return View(marketingManager);
        }
    }
}