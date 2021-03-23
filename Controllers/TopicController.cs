﻿using System;
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
    public class TopicController : Controller
    {
        private G5EnterpriseDBEntities db = new G5EnterpriseDBEntities();
        [Authorize(Roles = "Admin,Guest")]// GET: Topic
        public ActionResult Index(string searchString)
        {
            var tp = from m in db.Topics select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                tp = tp.Where(s => s.TopicID.Contains(searchString));
            }
            return View(tp);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Topic tp)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Topics.Add(tp);
                    db.SaveChanges();
                    return RedirectToAction("index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error inserting Content. ID is already existed");
                    return View(tp);
                }

            }
            return RedirectToAction("index");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Details(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic tp = db.Topics.Find(id);
            if (tp == null)
            {
                return HttpNotFound();
            }
            return View(tp);
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic tp = db.Topics.Find(id);
            if (tp == null)
            {
                return HttpNotFound();
            }
            return View(tp);
        }
        [HttpPost, ActionName("Delete")]

        public ActionResult DeleteConfirmed(string id)
        {
            Topic tp = db.Topics.Find(id);
            db.Topics.Remove(tp);
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
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Topic tp = db.Topics.Find(id);
            if (tp == null)
            {
                return HttpNotFound();
            }
           
            return View(tp);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Topic tp)
        {        
            if (ModelState.IsValid)
            {
                db.Entry(tp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tp);
        }
    }
}