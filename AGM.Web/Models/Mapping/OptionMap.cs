using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace AGM.Web.Models.Mapping
{
    public class OptionMap: EntityTypeConfiguration<Option>
    {
        public OptionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("Options");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Section).HasColumnName("Section");
            this.Property(t => t.SerializedValue).HasColumnName("SerializedValue");

            this.Ignore(t => t.Value);
        }
    }
}