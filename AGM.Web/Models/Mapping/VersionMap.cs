using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace AGM.Web.Models.Mapping
{
    public class VersionMap : EntityTypeConfiguration<Version>
    {
        public VersionMap()
        {
            // Primary Key
            this.HasKey(t => t.Code);

            // Table & Column Mappings
            this.ToTable("Version");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.UpdateSucceeded).HasColumnName("UpdateSucceeded");
            this.Property(t => t.LastUpdateTryDate).HasColumnName("LastUpdateTryDate");
        }
    }
}