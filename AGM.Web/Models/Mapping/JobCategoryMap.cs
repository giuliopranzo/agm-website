using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace AGM.Web.Models.Mapping
{
    public class JobCategoryMap : EntityTypeConfiguration<JobCategory>
    {
        public JobCategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("candidaticategorie");
            this.Property(t => t.Id).HasColumnName("idcategoria");
            this.Property(t => t.Name).HasColumnName("nome");
        }
    }
}