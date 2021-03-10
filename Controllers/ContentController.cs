using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data;
using WebEnterprise.Models;

namespace WebEnterprise.Controllers
{
  //[Authorize(Roles = "Student,Guest")]
    public class ContentController : Controller
    {
        private G5EnterpriseDBEntities db = new G5EnterpriseDBEntities();
        // GET: Content
        
        public ActionResult Index(string searchString)
        {
            var ct= from m in db.Contents select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                ct = ct.Where(s => s.CTName.Contains(searchString));
            }
            return View(ct);
        }
        public ActionResult Create()
        {
            /* ViewBag.CTTagID = new SelectList(db.CTTags, "CTTag");
             ViewBag.StudentID = new SelectList(db.Students, "StudentName");
              var CTTaglist = db.CTTags.ToList();
              ViewBag.CTTag = new SelectList(CTTaglist, dataValueField: "CTTagID", dataTextField: "CTTag");*/
            ViewBag.CTTag = new SelectList(db.CTTags, "CTTagID", "CTTag1");
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "StudentName");
            
            return View();

        }
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Create([Bind(Include = "CTID,CTName,CTDescription,StudentID,CTTag")] Content ct)
            {
            /*var CTTaglist = db.CTTags.ToList();
            ViewBag.CTTag = new SelectList(CTTaglist, dataValueField: "CTTagID", dataTextField: "CTTag");
                ViewBag.CTTagID = new SelectList(db.CTTags, "CTTag", ct.CTTag);
                ViewBag.StudentID = new SelectList(db.Students, "StudentName", ct.StudentID);*/

            if (ModelState.IsValid)
            {
                try{ 
                db.Contents.Add(ct);
                db.SaveChanges();
                return RedirectToAction("Index");
                  }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Error inserting Content. ID is already existed");
                    return View(ct);
                    }
                }
            ViewBag.CTTag = new SelectList(db.CTTags, "CTTagID", "CTTag1", ct.CTTag);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "StudentName", ct.StudentID);

            return View(ct);
        }
            public ActionResult Edit(string id)
            {
            ViewBag.CTTag = new SelectList(db.CTTags, "CTTagID", "CTTag1");
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "StudentName");

            if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Content ct = db.Contents.Find(id);
                if (ct == null)
                {
                    return HttpNotFound();
                }
                return View(ct);
            }
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Edit([Bind(Include = "CTID,CTName,CTDescription,CTTag,StudentID")] Content ct)
        {
            ViewBag.CTTag = new SelectList(db.CTTags, "CTTagID", "CTTag1");
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "StudentName");
            if (ModelState.IsValid)
                {
                    db.Entry(ct).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            return View(ct);
        }

            public ActionResult Delete(string id)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Content ct = db.Contents.Find(id);
                if (ct == null)
                {
                    return HttpNotFound();
                }
                return View(ct);
            }
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public ActionResult DeleteConfirmed(string id)
            {
                Content ct = db.Contents.Find(id);
                db.Contents.Remove(ct);
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
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content ct = db.Contents.Find(id);
            if (ct == null)
            {
                return HttpNotFound();
            }
            return View(ct);
        }
    }
}