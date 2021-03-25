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
    public class MarketingCoordinatorActionController : Controller
    {
        // GET: MarketingCoordinatorAction
        private G5EnterpriseDBEntities db = new G5EnterpriseDBEntities();
        public ActionResult MCProfile()
        {
            var UserName = User.Identity.Name;
            var userId = UserName;

            var MC = (from a in db.MarketingCoordinators where a.UserName.ToString().Equals(userId) select a).FirstOrDefault();
            return View(MC);
        }

        [Authorize(Roles = "Admin,MarketingCoordinator")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MarketingCoordinator marketingCoordinator = db.MarketingCoordinators.Find(id);
            if (marketingCoordinator == null)
            {
                return HttpNotFound();
            }

            ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "FacultyName", marketingCoordinator.FacultyID);
            return View(marketingCoordinator);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit([Bind(Include = "MCEmail,MCID,MCName,MCAddress,MCPhone,FacultyID,UserName")] MarketingCoordinator marketingCoordinator)
        {
            if (ModelState.IsValid)
            {
                db.Entry(marketingCoordinator).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Profile");
            }

            ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "FacultyName", marketingCoordinator.FacultyID);
            return View(marketingCoordinator);
        }
    }
}