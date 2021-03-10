using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebEnterprise.Models;

namespace WebEnterprise.Controllers
{
    public class MarketingCoordinatorsController : Controller
    {
        private G5EnterpriseDBEntities db = new G5EnterpriseDBEntities();

        // GET: MarketingCoordinators
        public ActionResult Index()
        {
            var marketingCoordinators = db.MarketingCoordinators.Include(m => m.Content).Include(m => m.CTTag1);
            return View(marketingCoordinators.ToList());
        }

        // GET: MarketingCoordinators/Details/5
        public ActionResult Details(string id)
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
            return View(marketingCoordinator);
        }

        // GET: MarketingCoordinators/Create
        public ActionResult Create()
        {
            ViewBag.CTTag = new SelectList(db.Contents, "CTID", "CTName");
            ViewBag.CTTag = new SelectList(db.CTTags, "CTTagID", "CTTag1");
            return View();
        }

        // POST: MarketingCoordinators/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "MCID,MCName,MCAddress,MCPhone,CTTag")] MarketingCoordinator marketingCoordinator)
        {
            if (ModelState.IsValid)
            {
                db.MarketingCoordinators.Add(marketingCoordinator);
                db.SaveChanges();

                AuthenController.CreateAccount(marketingCoordinator.MCID, "123456", "MarketingCoordinator");

                return RedirectToAction("Index");
            }

            ViewBag.CTTag = new SelectList(db.Contents, "CTID", "CTName", marketingCoordinator.CTTag);
            ViewBag.CTTag = new SelectList(db.CTTags, "CTTagID", "CTTag1", marketingCoordinator.CTTag);
            return View(marketingCoordinator);
        }

        // GET: MarketingCoordinators/Edit/5
        [Authorize(Roles = "MarketingCoordinator")]
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
            ViewBag.CTTag = new SelectList(db.Contents, "CTID", "CTName", marketingCoordinator.CTTag);
            ViewBag.CTTag = new SelectList(db.CTTags, "CTTagID", "CTTag1", marketingCoordinator.CTTag);
            return View(marketingCoordinator);
        }

        // POST: MarketingCoordinators/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "MarketingCoordinator")]
        public ActionResult Edit([Bind(Include = "MCID,MCName,MCAddress,MCPhone,CTTag")] MarketingCoordinator marketingCoordinator)
        {
            if (ModelState.IsValid)
            {
                db.Entry(marketingCoordinator).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CTTag = new SelectList(db.Contents, "CTID", "CTName", marketingCoordinator.CTTag);
            ViewBag.CTTag = new SelectList(db.CTTags, "CTTagID", "CTTag1", marketingCoordinator.CTTag);
            return View(marketingCoordinator);
        }

        // GET: MarketingCoordinators/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
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
            return View(marketingCoordinator);
        }

        // POST: MarketingCoordinators/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(string id)
        {
            MarketingCoordinator marketingCoordinator = db.MarketingCoordinators.Find(id);
            db.MarketingCoordinators.Remove(marketingCoordinator);
            db.SaveChanges();
            AuthenController.DeleteAccount(marketingCoordinator.MCID);
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
    }
}
