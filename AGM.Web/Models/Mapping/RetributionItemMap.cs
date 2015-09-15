using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace AGM.Web.Models.Mapping
{
    public class RetributionItemMap : EntityTypeConfiguration<RetributionItem>
    {
        public RetributionItemMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("rappvociretributive");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Month).HasColumnName("Month");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Qty).HasColumnName("Qty");
            this.Property(t => t.Amount).HasColumnName("Amount");
            this.Property(t => t.Total).HasColumnName("Total");
        }
    }
}