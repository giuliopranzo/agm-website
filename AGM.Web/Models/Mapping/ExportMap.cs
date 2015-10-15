using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace AGM.Web.Models.Mapping
{
    public class ExportMap : EntityTypeConfiguration<Export>
    {
        public ExportMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("export");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Month).HasColumnName("month");
            this.Property(t => t.MHFileName).HasColumnName("mhFileName");
            this.Property(t => t.RIFileName).HasColumnName("riFileName");
            this.Property(t => t.UsersCount).HasColumnName("usersCount");
            this.Property(t => t.UsersMax).HasColumnName("usersMax");
            this.Property(t => t.CalculateDate).HasColumnName("calculateDate");
            this.Property(t => t._hourReport).HasColumnName("HourReport");
            this.Property(t => t._retributionItems).HasColumnName("RetributionItems");

            this.Ignore(t => t.HourReport);
            this.Ignore(t => t.RetributionItems);
        }
    }
}