using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DexReportsSolution.Models
{
    public class Period
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Period(DateTime startdate, DateTime enddate)
        {
            StartDate = startdate;
            EndDate = enddate;
        }
    }
}