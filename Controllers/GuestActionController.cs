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
    public class GuestActionController : Controller
    {
        private G5EnterpriseDBEntities db = new G5EnterpriseDBEntities();
        // GET: StudentAction
        public ActionResult GProfile()
        {
            var UserName = User.Identity.Name;
            var userId = UserName;

            var guest = (from b in db.Guests where b.UserName.ToString().Equals(userId) select b).FirstOrDefault();
            return View(guest);
        }
        [Authorize(Roles = "Guest")]
        public ActionResult Edit(int? id)
        {
            ViewBag.GID = (from i in db.Guests where i.UserName == User.Identity.Name select i.GuestID).FirstOrDefault();
            ViewBag.Fac = (from i in db.Guests where i.UserName == User.Identity.Name select i.Faculty.FacultyName).FirstOrDefault();

            var Name = (from m in db.Guests where m.UserName == User.Identity.Name select m).FirstOrDefault();
            ViewBag.uName = Name.UserName;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guest guest = db.Guests.Find(id);
            if (guest == null)
            {
                return HttpNotFound();
            }
            return View(guest);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(Guest guest)
        {
            var GID = (from i in db.Guests where i.UserName == User.Identity.Name select i.GuestID).FirstOrDefault();
            var uName = (from m in db.Guests where m.UserName == User.Identity.Name select m.UserName).FirstOrDefault();
            var Fac = (from i in db.Guests where i.UserName == User.Identity.Name select i.FacultyID).FirstOrDefault();
            if (ModelState.IsValid)
            {
                guest.GuestID = GID;
                guest.FacultyID = Fac;
                guest.UserName = uName;
                db.Entry(guest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GProfile");
            }

            return View(guest);
        }

    }
}
