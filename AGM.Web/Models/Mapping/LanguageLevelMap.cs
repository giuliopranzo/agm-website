using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace AGM.Web.Models.Mapping
{
    public class LanguageLevelMap : EntityTypeConfiguration<LanguageLevel>
    {
        public LanguageLevelMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("candidatilinguelivelli");
            this.Property(t => t.Id).HasColumnName("idlivello");
            this.Property(t => t.Name).HasColumnName("nome");
        }
    }
}