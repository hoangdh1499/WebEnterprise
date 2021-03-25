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
        [Authorize(Roles = "MarketingCoordinator")]
        public ActionResult Index()
        {
            var contentAssign = db.ContentAssigns.Include(t => t.Content).Include(t => t.MarketingCoordinator).Include(t => t.Topic);
            return View(contentAssign.ToList());
        }
        public ActionResult Details(int? id)
        {
            ViewBag.Comments = (from c in db.Comments where c.CTassignID == id select c).ToList();

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
        public ActionResult ehee([Bind(Include = "TopicID,CommentA,CTID,MCID,StatusID,CTassignID")] ContentAssign contentAssign, int id)
        {
            contentAssign.CTassignID = contentAssign.CTassignID;
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "GiveStatus");

            var tpid = (from tp in db.Topics join t in db.ContentAssigns on tp.TopicID equals t.TopicID where t.CTassignID == id select new { Name = tp.TopicID }).FirstOrDefault();
            contentAssign.TopicID = tpid.Name;

            var cid = (from c in db.Contents join t in db.ContentAssigns on c.CTID equals t.CTID where t.CTassignID == id select new { ID = c.CTID }).FirstOrDefault();
            contentAssign.CTID = cid.ID;

            var m = (from s in db.MarketingCoordinators
                     where s.UserName.Equals(User.Identity.Name)
                     select s).FirstOrDefault();
            contentAssign.MCID = m.MCID;
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
                .Where(s => s.Content.Student.UserName == User.Identity.Name)
                .ToList();

            return View(AcceptContent.ToList());
        }
        

        [Authorize(Roles = "MarketingCoordinator,Admin")]
        public ActionResult ehee(int? id)
        {
            var tpid = (from tp in db.Topics join t in db.ContentAssigns on tp.TopicID equals t.TopicID where t.CTassignID == id select new { Name = tp.TopicName }).FirstOrDefault();
            ViewBag.TName = tpid.Name;
            ViewBag.StatusID = new SelectList(db.Status, "StatusID", "GiveStatus");

            var cid = (from c in db.Contents join t in db.ContentAssigns on c.CTID equals t.CTID where t.CTassignID == id select new { Name = c.CTName }).FirstOrDefault();
            ViewBag.CName = cid.Name;

            var m = (from s in db.MarketingCoordinators
                     where s.UserName.Equals(User.Identity.Name)
                     select s).FirstOrDefault();
            ViewBag.MCName = m.MCName; //LINQ+ ENtityframework

            ContentAssign contentAssign = db.ContentAssigns.Find(id);

            return View(contentAssign);
        }

        public ActionResult WriteCMT(int id)
        {
            var cmt = (from c in db.ContentAssigns where c.CTassignID == id select c).FirstOrDefault();
            ViewBag.wrtc = cmt.Content.CTName;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult WriteCMT(Comment cmt, int id)
        {

            var cmmt = (from c in db.ContentAssigns where c.CTassignID == id select c).FirstOrDefault();
            
            if (ModelState.IsValid)
            {
                db.Comments.Add(new Comment
                {
                    CommentText = cmt.CommentText,
                    CommentDate = DateTime.Now,
                    AuthorName = User.Identity.Name,
                    CTassignID = cmmt.CTassignID,
                });
                db.SaveChanges();

                return RedirectToAction("Details/" + id);
            }

            return RedirectToAction("Details/" + id);
        }
        public ActionResult ViewTopic(string id)
        {
            var TopicContent = db.ContentAssigns
                .Where(s => s.Status.GiveStatus.Contains("Accept") && s.Topic.TopicID == id)
                .ToList();

            return View(TopicContent.ToList());
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