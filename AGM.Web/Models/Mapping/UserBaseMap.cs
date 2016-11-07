using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using System.Web;

namespace AGM.Web.Models.Mapping
{
    public class UserBaseMap: EntityTypeConfiguration<UserBase>
    {
        public UserBaseMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("Utenti");
            this.Property(t => t.Id).HasColumnName("idutente");
            this.Property(t => t.Username).HasColumnName("utente");
            this.Property(t => t.FirstName).HasColumnName("nome");
            this.Property(t => t.LastName).HasColumnName("cognome");
            this.Property(t => t.Email).HasColumnName("email");
            this.Property(t => t._image).HasColumnName("image");

            // Ignore
            this.Ignore(p => p.Name);
            this.Ignore(p => p.Company);
            this.Ignore(p => p.Image);
        }
    }
}