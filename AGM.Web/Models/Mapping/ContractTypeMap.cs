using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace AGM.Web.Models.Mapping
{
    public class ContractTypeMap : EntityTypeConfiguration<ContractType>
    {
        public ContractTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("candidaticontratti");
            this.Property(t => t.Id).HasColumnName("idcontratto");
            this.Property(t => t.Name).HasColumnName("nome");
        }
    }
}