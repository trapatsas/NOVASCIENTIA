using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using se.Models;
using Newtonsoft.Json;
using se.ViewModels;

namespace se.Controllers
{
    public class AirfoilsController : Controller
    {
        private AirFoilDBEntities db = new AirFoilDBEntities();

        // GET: /Airfoils/
        public ActionResult Index(string id)
        {
            var airfoils = from a in db.Dat
                           select a;


            if (!String.IsNullOrEmpty(id) && id.Length > 1)
            {
                airfoils = airfoils.Where(s => s.Id.Contains(id) || s.Description.Contains(id));
            }
            else if (!String.IsNullOrEmpty(id) && id.Length == 1) 
            {
                airfoils = airfoils.Where(s => s.Id.ToLower().StartsWith(id));
            }
            else
                airfoils = airfoils.Where(s => s.Id.ToLower().StartsWith("a"));



            return View(airfoils.ToList());
        }


        public FileContentResult ExportCSV(string name, string csv)
        {
            return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/plain", name + ".txt");

        }

        // GET: /Airfoils/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dat dat = db.Dat.Find(id);
            if (dat == null)
            {
                return HttpNotFound();
            }
            return View(dat);
        }

        // POST: /Airfoils/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public JsonResult Create(DatJson Airfoil)
        {
            Dat dat = db.Dat.Find(Airfoil.Id);
            if ((dat == null) && !String.IsNullOrEmpty(Airfoil.Id) && !String.IsNullOrEmpty(Airfoil.Description))
            {
                if (ModelState.IsValid)
                {
                    Dat dto = new Dat();
                    // Manual Mapping
                    dto.Id = Airfoil.Id;
                    dto.Path = Airfoil.Path;
                    dto.Description = Airfoil.Description;
                    db.Dat.Add(dto);

                    foreach (var point in Airfoil.Points)
                    {
                        Coordinates newPoint = new Coordinates();
                        newPoint.DatId = point.DatId;
                        newPoint.Latitude = point.Latitude;
                        newPoint.Longitude = point.Longitude;
                        db.Coordinates.Add(newPoint);
                    }
                    db.SaveChanges();
                    return Json(new { msg = "Airfoil " + Airfoil.Id + " was successfully saved!" });
                }
            }
            return Json(new { msg = "Saved Failed! Maybe you should change the Title ;)" });
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
