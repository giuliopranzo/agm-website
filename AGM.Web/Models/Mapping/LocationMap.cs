using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace AGM.Web.Models.Mapping
{
    public class LocationMap : EntityTypeConfiguration<Location>
    {
        public LocationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("candidatiluoghi");

            this.Property(t => t.Id).HasColumnName("idluogo");
            this.Property(t => t.Name).HasColumnName("nome");
        }
    }
}