using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace AGM.Web.Models.Mapping
{
    public class MonthlyReportHourMap : EntityTypeConfiguration<MonthlyReportHour>
    {
        public MonthlyReportHourMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            //this.Property(t => t.Title)
            //    .HasMaxLength(50);

            //this.Property(t => t.ImageUrl)
            //    .HasMaxLength(255);

            //this.Property(t => t.Location)
            //    .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("rappore");
            this.Property(t => t.Id).HasColumnName("idrappore");
            this.Property(t => t.UserId).HasColumnName("idutente");
            this.Property(t => t.ReasonId).HasColumnName("idcausale");
            this.Property(t => t.Day).HasColumnName("giorno");
            this.Property(t => t.Month).HasColumnName("mese");
            this.Property(t => t.Year).HasColumnName("anno");
            this.Property(t => t.HoursRaw).HasColumnName("ore");
 
            // Ignore
            this.Ignore(p => p.Date);
            this.Ignore(p => p.HoursCount);
            this.Ignore(p => p.Reason);
        }
    }
}