using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CS.WebApplication;

namespace CS.WebApplication.Controllers
{
    public class CareersController : Controller
    {
        private ConsSoftEntities db = new ConsSoftEntities();

        // GET: Careers
        public ActionResult Index()
        {
            return View(db.Career.ToList());
        }

        // GET: Careers/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Career career = db.Career.Find(id);
            if (career == null)
            {
                return HttpNotFound();
            }
            return View(career);
        }

        // GET: Careers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Careers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Seq,Name,Description,Disabled")] Career career)
        {
            if (ModelState.IsValid)
            {
                career.Id = Guid.NewGuid();
                career.CreatedDate = DateTime.Now;

                if (Request.Files.Count > 0)
                {
                    var file = Request.Files[0];
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = $"{career.Id}_{Path.GetFileName(file.FileName)}";
                        var filePath = Path.Combine(Server.MapPath("/Public/Careers/"), fileName);
                        file.SaveAs(filePath);

                        career.LogoUrl = fileName;
                    }
                }

                db.Career.Add(career);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(career);
        }

        // GET: Careers/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Career career = db.Career.Find(id);
            if (career == null)
            {
                return HttpNotFound();
            }
            return View(career);
        }

        // POST: Careers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Seq,Name,Description,Disabled")] Career career)
        {
            if (ModelState.IsValid)
            {
                career.CreatedDate = DateTime.Now;
                db.Entry(career).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(career);
        }

        // GET: Careers/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Career career = db.Career.Find(id);
            if (career == null)
            {
                return HttpNotFound();
            }
            return View(career);
        }

        // POST: Careers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Career career = db.Career.Find(id);
            db.Career.Remove(career);
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
    }
}
