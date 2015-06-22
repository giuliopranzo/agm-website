using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace AGM.Web.Models.Mapping
{
    public class HourReasonMap : EntityTypeConfiguration<HourReason>
    {
        public HourReasonMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("rappcausali");
            this.Property(t => t.Id).HasColumnName("idcausale");
            this.Property(t => t.Name).HasColumnName("nome");
            this.Property(t => t.IsDeleted).HasColumnName("isDeleted");
        }
    }
}