using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using AGM.Web.Models.Mapping;

namespace AGM.Web.Models
{
    public class AgmDataContext: DbContext
    {
        static AgmDataContext()
        {
            Database.SetInitializer<AgmDataContext>(null);
        }

        public AgmDataContext()
            : base("Name=AgmDataContext")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<MonthlyReportHour> MonthlyReportHours { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new MonthlyReportHourMap());
        }
    }
}