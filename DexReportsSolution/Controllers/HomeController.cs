using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Mvc;
using Highsoft.Web.Mvc.Stocks;
using Highsoft.Web.Mvc.Charts;
using DexReportsSolution.Models;
using System.Data.Linq;
using DexReportsSolution.Classes;

namespace DexReportsSolution.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string connectionString = @"Data Source = BOBROVNY-VA; Initial Catalog = Series; Integrated Security = True";
            DataContext db = new DataContext(connectionString);

            IEnumerable<SensorSeries> series = db.GetTable<SensorSeries>();

            ViewBag.Series = series;

            return View();
        }

        public ActionResult GetChartData(string parameter)
        {
            string startdate = Convert.ToDateTime(epoch2string(Convert.ToInt32(parameter.Substring(0, 10)))).ToLocalTime().ToString("yyyy-MM-dd hh:mm:ss");
            string enddate = Convert.ToDateTime(epoch2string(Convert.ToInt32(parameter.Substring(10, 10)))).ToLocalTime().ToString("yyyy-MM-dd hh:mm:ss");
            int bnum = Convert.ToInt32(parameter.Substring(20));

            SensorSeries ss = new SensorSeries();
            ss.InitialSeries(bnum);
            ss.GetSeries(startdate, enddate);

            ViewBag.Label = ss.Label;

            LargeJsonResult json = new LargeJsonResult();

            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            json.Data = ss.GetSeries(startdate, enddate);

            return json;
        }

        public ActionResult GetPieData()
        {

            var data = new List<Borehole>()
            {
                new Borehole() {boreholeNumber = "Скважина №181", atWork = 32 },
                new Borehole() {boreholeNumber = "Скважина №132", atWork = 15 },
                new Borehole() {boreholeNumber = "Скважина №46", atWork = 53 }

            };

            var dataforchart = data.Select(x => new { name = x.boreholeNumber, y = x.atWork });
            return Json(dataforchart, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public static long GetJavascriptTimestamp(DateTime input)
        {
            TimeSpan span = new TimeSpan(DateTime.Parse("1/1/1970").Ticks);
            DateTime time = input.Subtract(span);
            return (time.Ticks / 10000);
        }

        private string epoch2string(int epoch)
        {
            string date = "" + new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local).AddSeconds(epoch).ToShortDateString() + " " + new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local).AddSeconds(epoch).ToLongTimeString() + "";
            return date;
        }

    }
}