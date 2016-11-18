using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace AGM.Web.Models.Mapping
{
    public class NoticeMap : EntityTypeConfiguration<Notice>
    {
        public NoticeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("comunicazioni");
            this.Property(t => t.Id).HasColumnName("idcomunicazione");
            this.Property(t => t.UserId).HasColumnName("idutente");
            this.Property(t => t.Date).HasColumnName("data");
            this.Property(t => t.Subject).HasColumnName("oggetto");
            this.Property(t => t.Text).HasColumnName("testo");
            this.Property(t => t.IsDeleted).HasColumnName("isDeleted");

        }
    }
}