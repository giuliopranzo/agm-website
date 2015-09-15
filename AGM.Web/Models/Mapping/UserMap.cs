using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using System.Web;

namespace AGM.Web.Models.Mapping
{
    public class UserMap: EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            //this.Property(t => t.Title)
            //    .HasMaxLength(50);

            //this.Property(t => t.ImageUrl)
            //    .HasMaxLength(255);

            //this.Property(t => t.Location)
            //    .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Utenti");
            this.Property(t => t.Id).HasColumnName("idutente");
            this.Property(t => t.IdExport).HasColumnName("idExport");
            this.Property(t => t.IdExport).IsOptional();
            this.Property(t => t.Username).HasColumnName("utente");
            this.Property(t => t.FirstName).HasColumnName("nome");
            this.Property(t => t.LastName).HasColumnName("cognome");
            this.Property(t => t.Email).HasColumnName("email");
            this.Property(t => t._image).HasColumnName("image");

            this.Property(t => t.Password).HasColumnName("pass");
            this.Property(t => t._sectionUsersVisible).HasColumnName("utenti");
            this.Property(t => t._sectionJobAdsVisible).HasColumnName("annunci");
            this.Property(t => t._sectionMonthlyReportsVisible).HasColumnName("rapportini");
            this.Property(t => t._sectionJobApplicantsVisible).HasColumnName("candidati");
            this.Property(t => t._isActive).HasColumnName("attivo");
            this.Property(t => t._isDeleted).HasColumnName("isDeleted");
            this.Property(t => t.RetributionItemConfSerialized).HasColumnName("retributionItemConf");

            // Ignore
            this.Ignore(p => p.Name);
            this.Ignore(p => p.Company);
            this.Ignore(p => p.Image);
            this.Ignore(p => p.SectionUsersVisible);
            this.Ignore(p => p.SectionJobAdsVisible);
            this.Ignore(p => p.SectionJobApplicantsVisible);
            this.Ignore(p => p.SectionMonthlyReportsVisible);
            this.Ignore(p => p.IsActive);
            this.Ignore(p => p.IsDeleted);
            this.Ignore(p => p.RetributionItemConfiguration);
        }
    }
}