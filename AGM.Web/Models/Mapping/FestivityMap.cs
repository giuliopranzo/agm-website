using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace AGM.Web.Models.Mapping
{
    public class FestivityMap: EntityTypeConfiguration<Festivity>
    {
        public FestivityMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("rappfestivi");
            this.Property(t => t.Id).HasColumnName("idgiorno");
            this.Property(t => t.Date).HasColumnName("date");
        }
    }
}