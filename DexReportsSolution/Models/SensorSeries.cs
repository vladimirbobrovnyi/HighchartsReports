using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace DexReportsSolution.Models
{

    [Table(Name = "SeriesParams")]
    public class SensorSeries
    {
        [Column(IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id { get; set; }

        [Column(Name = "Tagname")]
        public string TagName { get; set; }

        [Required]
        [Column(Name = "Label")]
        public string Label { get; set; }

        [Required]
        [Column(Name = "Server")]
        public string Server { get; set; }

        [Column(Name = "Port")]
        public int Port { get; set; }

        [Column(Name = "Security")]
        public bool IntegratedSecurity { get; set; }

        [Column(Name = "Login")]
        public string Login { get; set; }

        [Column(Name = "Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Column(Name = "DBName")]
        public string DatabaseName { get; set; }

        [Column(Name = "TableName")]
        public string TableName { get; set; }

        [Column(Name = "xFieldName")]
        public string xFieldName { get; set; }

        [Column(Name = "yFieldName")]
        public string yFieldName { get; set; }

        [Column(Name = "sp1Name")]
        public string sp1Name { get; set; }

        [Column(Name = "sp2Name")]
        public string sp2Name { get; set; }

        [Column(Name = "Query")]
        private string Query { get; set; }

        private string StartTime { get; set; }
        private string EndTime { get; set; }

        private List<SensorDataPoint> Data { get; set; }

        public void InitialSeries(int id)
        {
            string connectionString = @"Data Source = BOBROVNY-VA; Initial Catalog = Series; Integrated Security = True";
            DataContext db = new DataContext(connectionString);

            var query = from f in db.GetTable<SensorSeries>()
                        where f.Id == id
                        select f;

            foreach (var f in query)
            {
                Id = f.Id;
                TagName = f.TagName;
                Label = f.Label;
                Server = f.Server;
                Port = f.Port;
                IntegratedSecurity = f.IntegratedSecurity;
                Login = f.Login;
                Password = f.Password;
                DatabaseName = f.DatabaseName;
                TableName = f.TableName;
                xFieldName = f.xFieldName;
                yFieldName = f.yFieldName;
                sp1Name = f.sp1Name;
                sp2Name = f.sp2Name;
            }
        }

        public IEnumerable<object> GetSeries(string startdate, string enddate)
        {
            StartTime = startdate;
            EndTime = enddate;

            string connectionString = ParseConnectionString();
            string queryString = ParseQueryString();

            var data = new List<SensorDataPoint>();

            SqlConnection con = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(queryString, con);

            con.Open();
            SqlDataReader dr = command.ExecuteReader();



            if (dr.HasRows)
            {
                dr.Read();
                var dt = dr.GetValue(0);
                var prevVal = Math.Round(Convert.ToDouble(dr.GetValue(1)), 2);

                while (dr.Read())
                {
                    dt = dr.GetValue(0);

                    if (prevVal != Convert.ToDouble(dr.GetValue(1)))
                    {
                        data.Add(new SensorDataPoint(GetJavascriptTimestamp((DateTime)dt), prevVal));
                        prevVal = Math.Round(Convert.ToDouble(dr.GetValue(1)), 2);
                    }
                }
            }
            con.Close();


            var dataforchart = data.Select(x => new { x = x.Timestamp, y = x.Value });
            return dataforchart;
        }

        private static long GetJavascriptTimestamp(DateTime input)
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

        private string ParseConnectionString()
        {
            string connectionString = @"Data Source = ";

            if (Server != "") connectionString = connectionString + Server;
            if (Port != 0) connectionString = connectionString + ", " + Port;
            if (DatabaseName != "") connectionString = connectionString + "; Initial Catalog = " + DatabaseName + "";
            if (!IntegratedSecurity) connectionString = connectionString + "; Integrated Security = false; User ID = " + Login + "; Password = " + Password + "";
            else connectionString = connectionString + "; Integrated Security = true";

            return connectionString;
        }

        private string ParseQueryString()
        {
            string querystring = "SELECT";

            if (xFieldName != null) querystring = querystring + " " + xFieldName;
            if (yFieldName != null) querystring = querystring + ", " + yFieldName;
            if (sp1Name != null) querystring = querystring + ", " + sp1Name;
            if (sp2Name != null) querystring = querystring + ", " + sp2Name;
            if (DatabaseName != "") querystring = querystring + " FROM [" + DatabaseName + "].[dbo].";
            if (TableName != "") querystring = querystring + "[" + TableName + "] WHERE " + xFieldName + " between '" + StartTime + "' and '" + EndTime + "' ORDER BY " + xFieldName + "";
            return querystring;
        }


    }
}