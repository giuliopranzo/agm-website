using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace AGM.Web.Models.Mapping
{
    public class CandidateMap  : EntityTypeConfiguration<Candidate>
    {
        public CandidateMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("candidati");
            this.Property(t => t.Id).HasColumnName("idcandidato");
            this.Property(t => t.FirstName).HasColumnName("nome");
            this.Property(t => t.LastName).HasColumnName("cognome");
            this.Property(t => t.InterviewDateRaw).HasColumnName("colloquio");
            this.Property(t => t.BirthDateRaw).HasColumnName("anno");
            this.Property(t => t.Description).HasColumnName("profilo");
            this.Property(t => t.Language1).HasColumnName("lingua1");
            this.Property(t => t.Language2).HasColumnName("lingua2");
            this.Property(t => t.Language2Level).HasColumnName("lingua2livello");
            this.Property(t => t.Language3).HasColumnName("lingua3");
            this.Property(t => t.Language3Level).HasColumnName("lingua3livello");
            this.Property(t => t.ActualSalaryRaw).HasColumnName("contrattoimporto");
            this.Property(t => t.ContractType).HasColumnName("contrattotipo");
            this.Property(t => t.JobCategory).HasColumnName("idcategoria");
            this.Property(t => t.StatusId).HasColumnName("idstato");
            this.Property(t => t.InterviewerId).HasColumnName("idselezionatore");
            this.Property(t => t.ResidenceTown).HasColumnName("idluogo");
            this.Property(t => t.UpdateDateRaw).HasColumnName("aggiornamento");
            this.Property(t => t.AvailableIn).HasColumnName("disponibilita");
            this.Property(t => t.BirthPlace).HasColumnName("luogonascita");
            this.Property(t => t.EnglishTest).HasColumnName("testinglese");
            this.Property(t => t.ReasonId).HasColumnName("idmotivo");
            this.Property(t => t.WorkTown).HasColumnName("idluogolavoro");
            this.Property(t => t.Straniero).HasColumnName("straniero");
            this.Property(t => t.Idpermesso).HasColumnName("idpermesso");
            this.Property(t => t.DataScPermessoRaw).HasColumnName("datascpermesso");



            // Ignore
            this.Ignore(t => t.Name);
            this.Ignore(t => t.ActualSalary);
            this.Ignore(t => t.UpdateDate);
            this.Ignore(t => t.InterviewDate);
            this.Ignore(t => t.BirthDate);
            //this.Ignore<Candidate>(p => p.);
            //this.Ignore<Candidate>(p => p.HoursCount);
            //this.Ignore<Candidate>(p => p.Reason);
        }
    }
}