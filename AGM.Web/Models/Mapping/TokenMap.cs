using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace AGM.Web.Models.Mapping
{
    public class TokenMap : EntityTypeConfiguration<Token>
    {
        public TokenMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("token");
            this.Property(t => t.IsConsumed).HasColumnName("IsConsumed");
            this.Property(t => t.ExpirationDate).HasColumnName("ExpirationDate");
        }
    }
}