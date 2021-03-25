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

        public ActionResult Topic()
        {
            var tp = from m in db.Topics select m;
            return View(tp);
        }

        public ActionResult Uploaded()
        {
            var ct = db.Contents
                .Where(s => s.Student.UserName == User.Identity.Name)
                .ToList();

            return View(ct);
        }
        [Authorize(Roles = "Student")]
        public ActionResult Create(string id)
        {

            //ViewBag.CTTagID = new SelectList(db.CTTags, "CTTagID", "CTTagName");
            //ViewBag.TopicID = new SelectList(db.Topics, "TopicID", "TopicName");
            //ViewBag.StudentID = new SelectList(db.Students, "StudentID", "StudentName");
            var student = (from s in db.Students
                           where s.UserName.Equals(User.Identity.Name)
                           select s).FirstOrDefault();
            ViewBag.StudentName = student.StudentName; //LINQ+ ENtityframework
            ViewBag.StudentID = student.StudentID;
            ViewBag.Faculty = student.Faculty.FacultyName;

            var tpid = (from t in db.Topics
                        where t.TopicID == id
                        select t).FirstOrDefault();
            ViewBag.TName = tpid.TopicName;

            return View();

        }
        
        [HttpPost]
        public ActionResult Create(Content ct, HttpPostedFileBase postedImg, HttpPostedFileBase postedPDF, string id, ContentAssign contentAssign)
        {
            var student = (from s in db.Students
                           where s.UserName.Equals(User.Identity.Name)
                           select s).FirstOrDefault();
            ct.StudentID = student.StudentID; //LINQ+ ENtityframework
            //ct.StudentID = ViewBag.StudentID;

            var tpid = (from t in db.Topics
                        where t.TopicID == id
                        select t).FirstOrDefault();
            ct.FacultyID = tpid.FacultyID;

            byte[] bytes;
            byte[] byte2s;
            using (BinaryReader br = new BinaryReader(postedImg.InputStream))
            {
                bytes = br.ReadBytes(postedImg.ContentLength);
               
            }
            using (BinaryReader br2 = new BinaryReader(postedPDF.InputStream))
            {
                byte2s = br2.ReadBytes(postedPDF.ContentLength);
            }
            if (postedPDF.ContentLength > 0)
            {
                var fileName = Path.GetFileName(postedPDF.FileName);
                var filePath = Path.Combine(Server.MapPath("~/Files"), fileName);
                postedPDF.SaveAs(filePath);
            }
            db.Contents.Add(new Content
            {
                Name2 = Path.GetFileName(postedImg.FileName),
                ContentType2 = postedImg.ContentType,
                Data2 = bytes,
                CTName = ct.CTName,
                CTDescription = ct.CTDescription,
                FacultyID = ct.FacultyID,
                StudentID = ct.StudentID,
                Name = Path.GetFileName(postedPDF.FileName),
                ContentType = postedPDF.ContentType,
                Data = byte2s,
            });

            using (MailMessage mm = new MailMessage("sysgwww@gmail.com", "daipngch18721@fpt.edu.vn"))
            {
                //ct.To = ViewBag.McMail;
                mm.Subject = "Grade content";
                mm.Body = "You have a new content from student: ";
                mm.IsBodyHtml = false;
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential("sysgww@gmail.com", "tsuna2000");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                    ViewBag.Message = "Email sent.";
                }
            }

            //ViewBag.CTTagID = new SelectList(db.CTTags, "CTTagID", "CTTagName", ct.CTTagID);
            //ViewBag.TopicID = new SelectList(db.Topics, "TopicID", "TopicName", ct.TopicID);
            //ViewBag.StudentID = new SelectList(db.Students, "StudentID", "StudentName", ct.StudentID);
            db.ContentAssigns.Add(new ContentAssign 
            {
                CTID = ct.CTID,
                TopicID = tpid.TopicID
            });
            db.SaveChanges();
            return RedirectToAction("Create");

        }
        [Authorize(Roles = "Student,MarketingCoordinator,ManagerMarketing,Guest")]
        [HttpPost]
        public FileResult DownloadFile(int? fileId)
        {
            G5EnterpriseDBEntities entities = new G5EnterpriseDBEntities();
            Content file = entities.Contents.ToList().Find(p => p.CTID == fileId.Value);
            return File(file.Data, file.ContentType, file.Name);
        }
        [Authorize(Roles = "Student")]
        public ActionResult Edit(int id)
        {
            Content ct = db.Contents.Find(id);
            ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "FacultyName");
            ViewBag.StudentID = new SelectList(db.Students, "StudentID", "StudentName");
            return View(ct);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Content ct, HttpPostedFileBase postedFile, HttpPostedFileBase postedPDF)
        {
            ViewBag.FacultyID = new SelectList(db.Faculties, "FacultyID", "FacultyName", ct.FacultyID);
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
            Content ct = db.Contents.Find(id);
            return View(ct);
        }
        [HttpPost, ActionName("Delete")]

        public ActionResult DeleteConfirmed(int id)
        {
            Content ct = db.Contents.Find(id);
            db.Contents.Remove(ct);
            db.SaveChanges();
            return RedirectToAction("Uploaded");
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
            Content ct = db.Contents.Find(id);
            if (ct == null)
            {
                return HttpNotFound();
            }
            return View(ct);
        }
    }
}