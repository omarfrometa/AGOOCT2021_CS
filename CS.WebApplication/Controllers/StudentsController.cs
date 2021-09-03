using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CS.WebApplication;

namespace CS.WebApplication.Controllers
{
    public class StudentsController : Controller
    {
        private ConsSoftEntities db = new ConsSoftEntities();

        // GET: Students
        public ActionResult Index()
        {
            var student = db.Student.Include(s => s.Career);
            return View(student.ToList());
        }

        // GET: Students/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Student.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            ViewBag.CareerId = new SelectList(db.Career, "Id", "Name");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Seq,Enrollment,FirstName,LastName,Email,Phone,CareerId,Disabled")] Student student)
        {
            if (ModelState.IsValid)
            {
                student.Id = Guid.NewGuid();
                student.CreatedDate = DateTime.Now;

                db.Student.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CareerId = new SelectList(db.Career, "Id", "Name", student.CareerId);
            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Student.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.CareerId = new SelectList(db.Career, "Id", "Name", student.CareerId);
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Seq,Enrollment,FirstName,LastName,Email,Phone,CareerId,Disabled")] Student student)
        {
            if (ModelState.IsValid)
            {
                student.CreatedDate = DateTime.Now;
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CareerId = new SelectList(db.Career, "Id", "Name", student.CareerId);
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Student.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Student student = db.Student.Find(id);
            db.Student.Remove(student);
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
