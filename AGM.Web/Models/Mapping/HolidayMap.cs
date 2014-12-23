using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace AGM.Web.Models.Mapping
{
    public class HolidayMap: EntityTypeConfiguration<Holiday>
    {
        public HolidayMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("rappfestivi");
            this.Property(t => t.Id).HasColumnName("idgiorno");
            this.Property(t => t.DateRaw).HasColumnName("giorno");

            // Ignore
            this.Ignore(p => p.Date);
        }
    }
}