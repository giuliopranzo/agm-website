using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace AGM.Web.Models.Mapping
{
    public class MHReportLockMap : EntityTypeConfiguration<MHReportLock>
    {
        public MHReportLockMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("mhreportlock");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Month).HasColumnName("Month");
            this.Property(t => t.LockDate).HasColumnName("LockDate");
            this.Property(t => t.UnlockDate).HasColumnName("UnlockDate");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
        }
    }
}