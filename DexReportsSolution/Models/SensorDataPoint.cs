using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DexReportsSolution.Models
{
    public class SensorDataPoint
    {
        public long Timestamp { get; set; }
        public double Value { get; set; }

        public SensorDataPoint()
        { }

        public SensorDataPoint(long dateTime, double value)
        {
            Timestamp = dateTime;
            Value = value;
        }
    }
}