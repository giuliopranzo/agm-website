using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace AGM.Web.Models.Mapping
{
    public class JobApplicantStatusMap : EntityTypeConfiguration<JobApplicantStatus>
    {
        public JobApplicantStatusMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("candidatistati");
            this.Property(t => t.Id).HasColumnName("idstato");
            this.Property(t => t.Name).HasColumnName("nome");
        }
    }
}