using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace DexReportsSolution.Models
{
    public class SeriesParamContext : DbContext
    {
        public SeriesParamContext() : base("SeriesParamContext")
        {}

        public DbSet<SensorSeries> Series { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SensorSeries>().ToTable("SeriesParams");
            modelBuilder.Entity<SensorSeries>().Property(p => p.IntegratedSecurity).HasColumnName("Security");
            modelBuilder.Entity<SensorSeries>().Property(p => p.DatabaseName).HasColumnName("DBName");
            base.OnModelCreating(modelBuilder);
        }
    }

}