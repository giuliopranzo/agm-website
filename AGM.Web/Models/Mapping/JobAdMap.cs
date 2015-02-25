using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace AGM.Web.Models.Mapping
{
    public class JobAdMap: EntityTypeConfiguration<JobAd>
    {
        public JobAdMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("annunci");
            this.Property(t => t.Id).HasColumnName("idannuncio");
            this.Property(t => t.Title).HasColumnName("titolo");
            this.Property(t => t.Description).HasColumnName("indirizzo");
            this.Property(t => t.Location).HasColumnName("dove");
            this.Property(t => t.DateFromRaw).HasColumnName("datainizio");
            this.Property(t => t.DateToRaw).HasColumnName("datafine");
            this.Property(t => t.RefCode).HasColumnName("riferimento");

            // Ignore
            this.Ignore(p => p.DateFrom);
            this.Ignore(p => p.DateTo);
        }
    }
}