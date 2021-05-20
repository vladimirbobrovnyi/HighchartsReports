using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Web;
using System.Web.Mvc;
using DexReportsSolution.Models;
using System.Data.Entity;

namespace DexReportsSolution.Controllers
{
    public class SeriesController : Controller
    {

        SeriesParamContext db = new SeriesParamContext();

        public ActionResult All()
        {
            string connectionString = @"Data Source = BOBROVNY-VA; Initial Catalog = Series; Integrated Security = True";
            DataContext db = new DataContext(connectionString);

            IEnumerable<SensorSeries> series = db.GetTable<SensorSeries>();

            ViewBag.Series = series;
            return View();
        }

        [HttpGet]
        public ActionResult Add()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Add(SensorSeries s)
        {
            string connectionString = @"Data Source = BOBROVNY-VA; Initial Catalog = Series; Integrated Security = True";
            DataContext db = new DataContext(connectionString);

            db.GetTable<SensorSeries>().InsertOnSubmit(s);
            db.SubmitChanges();
            return Redirect("/Series/All");
        }

        [HttpGet]
        public ActionResult Edit(int? parameter)
        {
            //  string connectionString = @"Data Source = BOBROVNY-VA; Initial Catalog = Series; Integrated Security = True";
            int? Id = parameter;
            SensorSeries s = db.Series.Find(Id);
            
            //ViewBag.Series = series;
            return PartialView(s);
        }

        [HttpPost]
        public ActionResult Edit(SensorSeries s)
        {
            db.Entry(s).State = EntityState.Modified;
            db.SaveChanges();
            return Redirect("/Series/All");
        }

        [HttpGet]
        public ActionResult Delete(int? parameter)
        {
            int? Id = parameter;
            SensorSeries s = db.Series.Find(Id);

            return PartialView(s);
        }

        [HttpPost]
        public ActionResult Delete(SensorSeries s)
        {
            db.Entry(s).State = EntityState.Deleted;
            db.SaveChanges();
            return Redirect("/Series/All");
        }
    }
}