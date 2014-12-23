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
        public DbSet<MonthlyReportExpense> MonthlyReportExpenses { get; set; }
        public DbSet<MonthlyReportNote> MonthlyReportNotes { get; set; }
        public DbSet<HourReason> HourReasons { get; set; }
        public DbSet<ExpenseReason> ExpenseReasons { get; set; }
        public DbSet<Holiday> Holidays { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new MonthlyReportHourMap());
            modelBuilder.Configurations.Add(new MonthlyReportExpenseMap());
            modelBuilder.Configurations.Add(new MonthlyReportNoteMap());
            modelBuilder.Configurations.Add(new HourReasonMap());
            modelBuilder.Configurations.Add(new ExpenseReasonMap());
            modelBuilder.Configurations.Add(new HolidayMap());
        }
    }
}