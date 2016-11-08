using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace AGM.Web.Models.Mapping
{
    public class MonthlyReportAvailabilityMap : EntityTypeConfiguration<MonthlyReportAvailability>
    {
        public MonthlyReportAvailabilityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("rappreperibilita");
            this.Property(t => t.Id).HasColumnName("idreperibilita");
            this.Property(t => t.UserId).HasColumnName("idutente");
            this.Property(t => t.Day).HasColumnName("giorno");
            this.Property(t => t.Month).HasColumnName("mese");
            this.Property(t => t.Year).HasColumnName("anno");
            this.Property(t => t.Availability).HasColumnName("campo");

            // Ignore
            this.Ignore(p => p.Date);
        }
    }
}