using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WebEnterprise.Models;

namespace WebEnterprise.Controllers
{
    public class ContentAssignController : Controller
    {
        private G5EnterpriseDBEntities db = new G5EnterpriseDBEntities();
        [Authorize(Roles = "MarketingCoordinator,Student")]
        public ActionResult Index()
        {
            var contentAssign = db.ContentAssigns.Include(t => t.Content).Include(t => t.MarketingCoordinator).Include(t => t.Topic);
            return View(contentAssign.ToList());
        }
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContentAssign contentAssign = db.ContentAssigns.Find(id);
            if (contentAssign == null)
            {
                return HttpNotFound();
            }
            return View(contentAssign);
        }
        [Authorize(Roles = "Student")]
        public ActionResult Create()
        {
            
            ViewBag.TopicID = new SelectList(db.Topics, "TopicID", "TopicName");
            ViewBag.CTID = new SelectList(db.Contents, "CTID", "CTName");
           
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( ContentAssign contentAssign)
        {
            using (MailMessage mm = new MailMessage(contentAssign.Email, contentAssign.To))
            {
                mm.Subject = contentAssign.Subject;
                mm.Body = contentAssign.Body;
                mm.IsBodyHtml = false;
                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential(contentAssign.Email, contentAssign.Password);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                    ViewBag.Message = "Email sent.";
                }
            }
            if (ModelState.IsValid)
                {
                    db.ContentAssigns.Add(contentAssign);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            
            ViewBag.TopicID = new SelectList(db.Topics, "TopicID", "TopicName", contentAssign.TopicID);
            ViewBag.CTID = new SelectList(db.Contents, "CTID", "CTName", contentAssign.CTID);
            
            return View(contentAssign);
        }
        [Authorize(Roles = "MarketingCoordinator,Admin")]
        public ActionResult Edit(int id)
        {
            ViewBag.TopicID = new SelectList(db.Topics, "TopicID", "TopicName");
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "GiveStatus");
            ViewBag.CTID = new SelectList(db.Contents, "CTID", "CTName");
            ViewBag.MCID = new SelectList(db.MarketingCoordinators, "MCID", "MCName");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContentAssign contentAssign = db.ContentAssigns.Find(id);
            if (contentAssign == null)
            {
                return HttpNotFound();
            }
            
            return View(contentAssign);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TopicID,Comment,CTID,MCID,StatusID,CTassignID")] ContentAssign contentAssign)
        {
            contentAssign.CTassignID = contentAssign.CTassignID;
            ViewBag.TopicID = new SelectList(db.Topics, "TopicID", "TopicName");
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "GiveStatus");
            ViewBag.CTID = new SelectList(db.Contents, "CTID", "CTName");
            ViewBag.MCID = new SelectList(db.MarketingCoordinators, "MCID", "MCName");
            if (ModelState.IsValid)
            {
                db.Entry(contentAssign).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(contentAssign);
        }
        [Authorize(Roles = "MarketingCoordinator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ContentAssign contentAssign = db.ContentAssigns.Find(id);
            if (contentAssign == null)
            {
                return HttpNotFound();
            }
            return View(contentAssign);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            ContentAssign contentAssign = db.ContentAssigns.Find(id);
            db.ContentAssigns.Remove(contentAssign);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult ViewALL()
        {
            var AcceptContent = db.ContentAssigns
                .Where(s => s.Status.GiveStatus.Contains("Accept"))
                .ToList();
            
            return View(AcceptContent.ToList());
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