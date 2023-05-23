using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UIux.Models;

namespace UIux.Controllers
{
    public class SpecializationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /*// GET: Specializations
        public ActionResult Index()
        {
            return View(db.Specializations.ToList());
        }

        // GET: Specializations/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Specializations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "SpecializationID,Price,Name")] Specialization specialization)
        {
            if (ModelState.IsValid)
            {
                db.Specializations.Add(specialization);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(specialization);
        }

        // GET: Specializations/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Specialization specialization = db.Specializations.Find(id);
            if (specialization == null)
            {
                return HttpNotFound();
            }
            return View(specialization);
        }

        // POST: Specializations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "SpecializationID,Price,Name")] Specialization specialization)
        {
            if (ModelState.IsValid)
            {
                db.Entry(specialization).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(specialization);
        }

        // POST: Specializations/Delete/5
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            Specialization specialization = db.Specializations.Find(id);
            db.Specializations.Remove(specialization);
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
    


        public ActionResult Show(int id)
                {
                    try
                    {
                        Specialization specialization = db.Specializations.Find(id);
                        var doctors = from doctor in db.Doctors
                                         where doctor.SpecializationID == specialization.SpecializationID
                                      select doctor;
                        //Console.WriteLine(doctors);
                        if (doctors != null)
                        {
                            ViewBag.Categories = specialization;
                            ViewBag.Fresheners = doctors;
                            return View(specialization);
                        }
                        else
                        {
                            throw new NullReferenceException("You can't check a specialization that has no doctors!");
                        }
                    }
                    catch (Exception e)
                    {
                        TempData["message"] = e;
                        return Redirect("/Specializations/Index");
                    }
                }*/
        public ActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.message = TempData["message"].ToString();
            }

            var categories = from category in db.Specializations
                             orderby category.Name
                             select category;
            ViewBag.Specializations = categories;
            return View();
        }

        public ActionResult Show(int id)
        {
            try
            {
                Specialization category = db.Specializations.Find(id);
                var fresheners = from freshener in db.Doctors
                                 where freshener.SpecializationID == category.SpecializationID
                                 select freshener;
                //Console.WriteLine(fresheners);
                if (fresheners != null)
                {
                    ViewBag.Specializations = category;
                    ViewBag.Doctors = fresheners;
                    return View(category);
                }
                else
                {
                    throw new NullReferenceException("You can't check a category that has no doctors!");
                }
            }
            catch (Exception e)
            {
                TempData["message"] = e;
                return Redirect("/Categories/Index");
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(Specialization cat)
        {
            try
            {
                db.Specializations.Add(cat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View();
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            Specialization category = db.Specializations.Find(id);
            return View(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public ActionResult Edit(int id, Specialization requestCategory)
        {
            try
            {
                Specialization category = db.Specializations.Find(id);
                if (TryUpdateModel(category))
                {
                    category.Name = requestCategory.Name;
                    db.SaveChanges();
                    TempData["message"] = "The SPECIALIZATION has been modified!";
                    return RedirectToAction("Index");
                }

                return View(requestCategory);
            }
            catch (Exception)
            {
                return View(requestCategory);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            Specialization category = db.Specializations.Find(id);
            List<int> freshenersToDelete = db.Doctors.Where(fr => fr.SpecializationID == id).Select(f => f.DoctorId).ToList();

            foreach (var freshenerDelete in freshenersToDelete)
            {
                Doctor fresh = db.Doctors.Find(freshenerDelete);

                db.Doctors.Remove(fresh);
            }
            db.Specializations.Remove(category);
            TempData["message"] = "The specialization has been deleted!";
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
