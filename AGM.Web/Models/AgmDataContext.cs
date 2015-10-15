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
        public DbSet<RetributionItem> RetributionItems { get; set; }
        public DbSet<HourReason> HourReasons { get; set; }
        public DbSet<ExpenseReason> ExpenseReasons { get; set; }
        public DbSet<Festivity> Festivities { get; set; }
        public DbSet<JobAd> JobAds { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Version> Versions { get; set; }
        public DbSet<Export> Exports { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<MHReportLock> MHReportLocks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new MonthlyReportHourMap());
            modelBuilder.Configurations.Add(new MonthlyReportExpenseMap());
            modelBuilder.Configurations.Add(new MonthlyReportNoteMap());
            modelBuilder.Configurations.Add(new RetributionItemMap());
            modelBuilder.Configurations.Add(new HourReasonMap());
            modelBuilder.Configurations.Add(new ExpenseReasonMap());
            modelBuilder.Configurations.Add(new FestivityMap());
            modelBuilder.Configurations.Add(new JobAdMap());
            modelBuilder.Configurations.Add(new OptionMap());
            modelBuilder.Configurations.Add(new VersionMap());
            modelBuilder.Configurations.Add(new ExportMap());
            modelBuilder.Configurations.Add(new TokenMap());
            modelBuilder.Configurations.Add(new MHReportLockMap());
        }
    }
}