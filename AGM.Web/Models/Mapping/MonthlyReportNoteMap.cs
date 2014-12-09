using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace AGM.Web.Models.Mapping
{
    public class MonthlyReportNoteMap : EntityTypeConfiguration<MonthlyReportNote>
    {
        public MonthlyReportNoteMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("rappdescrizioni");
            this.Property(t => t.Id).HasColumnName("iddescrizione");
            this.Property(t => t.UserId).HasColumnName("idutente");
            this.Property(t => t.Day).HasColumnName("giorno");
            this.Property(t => t.Month).HasColumnName("mese");
            this.Property(t => t.Year).HasColumnName("anno");
            this.Property(t => t.Note).HasColumnName("campo");

            // Ignore
            this.Ignore(p => p.Date);
        }
    }
}