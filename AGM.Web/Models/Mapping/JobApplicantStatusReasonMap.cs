using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace AGM.Web.Models.Mapping
{
    public class JobApplicantStatusReasonMap : EntityTypeConfiguration<JobApplicantStatusReason>
    {
        public JobApplicantStatusReasonMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("candidatimotivi");
            this.Property(t => t.Id).HasColumnName("idmotivo");
            this.Property(t => t.Name).HasColumnName("nome");
        }
    }
}