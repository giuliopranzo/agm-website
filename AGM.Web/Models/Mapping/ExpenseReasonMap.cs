using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace AGM.Web.Models.Mapping
{
    public class ExpenseReasonMap : EntityTypeConfiguration<ExpenseReason>
    {
        public ExpenseReasonMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("rappcausalispese");
            this.Property(t => t.Id).HasColumnName("idspesa");
            this.Property(t => t.Name).HasColumnName("nome");
        }
    }
}