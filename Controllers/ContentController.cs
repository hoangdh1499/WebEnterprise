using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data;
using WebEnterprise.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.IO;
using System.Net.Mail;

namespace WebEnterprise.Controllers
{
    //[Authorize(Roles = "Student,Guest")]
    public class ContentController : Controller
    {
        private G5EnterpriseDBEntities db = new G5EnterpriseDBEntities();
        // GET: Content
        [Authorize(Roles = "Student,MarketingCoordinator")]
        public ActionResult Index(string searchString)
        {
            var ct = from m in db.Contents select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                ct = ct.Where(s => s.CTName.Contains(searchString));
            }
            return View(ct);
        }
        [Authorize(Roles = "Student")]
        public ActionResult Create()
        {

            ViewBag.CTTagID = new SelectList(db.CTTags, "CTTagID", "CTTagName");
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "StudentName");

            return View();

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Content ct, HttpPostedFileBase postedFile)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    db.Contents.Add(ct);
                    db.SaveChanges();
                    return RedirectToAction("index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error inserting Content. ID is already existed");
                    return View(ct);
                }

            }
            ViewBag.CTTagID = new SelectList(db.CTTags, "CTTagID", "CTTagName", ct.CTTagID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "StudentName", ct.StudentID);

            return RedirectToAction("Create");

        }

        [HttpPost]
        public ActionResult UpImage(Content ct, HttpPostedFileBase postedFile, HttpPostedFileBase postedPDF)
        {
           /* DateTime SubmissionDate = DateTime.Now;
            DateTime DeadlineDate = DateTime.Now.AddMinutes(1);
            if (DeadlineDate < DateTime.Now)
            {
                TempData["msg"] = "<script>alert('UP CC HET GIO!');</script>";
               
            }
            else
            { */
                //SEND EMAIL
                //Create

                byte[] bytes;
                byte[] byte2s;
                using (BinaryReader br = new BinaryReader(postedFile.InputStream))
                {
                    bytes = br.ReadBytes(postedFile.ContentLength);
                    byte2s = br.ReadBytes(postedPDF.ContentLength);
                }

                db.Contents.Add(new Content
                {
                    Name2 = Path.GetFileName(postedPDF.FileName),
                    ContentType2 = postedPDF.ContentType,
                    Data2 = byte2s,
                    CTName = ct.CTName,
                    CTDescription = ct.CTDescription,
                    CTTagID = ct.CTTagID,
                    StudentID = ct.StudentID,
                    Name = Path.GetFileName(postedFile.FileName),
                    ContentType = postedFile.ContentType,
                    Data = bytes

                });
            
            ViewBag.CTTagID = new SelectList(db.CTTags, "CTTagID", "CTTagName", ct.CTTagID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "StudentName", ct.StudentID);
            db.SaveChanges();
            return RedirectToAction("Create");

        }
        [Authorize(Roles = "Student,MarketingCoordinator,ManagerMarketing,Guest")]
        [HttpPost]
        public FileResult DownloadFile(int? fileId)
        {

            Content file = db.Contents.ToList().Find(p => p.CTID == fileId.Value);
            return File(file.Data2, file.ContentType2, file.Name2);
        }
        [Authorize(Roles = "Student")]
        public ActionResult Edit(int id)
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
            ViewBag.CTTagID = new SelectList(db.CTTags, "CTTagID", "CTTagName");
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "StudentName");
            return View(ct);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Content ct, HttpPostedFileBase postedFile, HttpPostedFileBase postedPDF)
        {
            ViewBag.CTTagID = new SelectList(db.CTTags, "CTTagID", "CTTagName", ct.CTTagID);
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "StudentName", ct.StudentID);
            if (ModelState.IsValid)
            {
                db.Entry(postedFile).State = EntityState.Modified;
                db.Entry(postedPDF).State = EntityState.Modified;
                db.Entry(ct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ct);
        }
        [Authorize(Roles = "Student")]
        public ActionResult Delete(int id)
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

        public ActionResult DeleteConfirmed(int id)
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
        [Authorize(Roles = "Student")]
        public ActionResult Details(int id)
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