using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace AGM.Web.Models.Mapping
{
    public class MonthlyReportExpenseMap : EntityTypeConfiguration<MonthlyReportExpense>
    {
        public MonthlyReportExpenseMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("rappspese");
            this.Property(t => t.Id).HasColumnName("idrappspese");
            this.Property(t => t.UserId).HasColumnName("idutente");
            this.Property(t => t.ReasonId).HasColumnName("idspesa");
            this.Property(t => t.Day).HasColumnName("giorno");
            this.Property(t => t.Month).HasColumnName("mese");
            this.Property(t => t.Year).HasColumnName("anno");
            this.Property(t => t.AmountRaw).HasColumnName("importo");

            // Ignore
            this.Ignore(p => p.Date);
            this.Ignore(p => p.Amount);
            this.Ignore(p => p.Reason);
            this.Ignore(p => p.CompleteDescription);
        }
    }
}