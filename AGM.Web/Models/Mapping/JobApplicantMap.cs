using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace AGM.Web.Models.Mapping
{
    public class JobApplicantMap : EntityTypeConfiguration<JobApplicant>
    {
        public JobApplicantMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Table & Column Mappings
            this.ToTable("candidati");
            /*
            public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string InterviewDateRaw { get; set; }
        public string BirthDateRaw { get; set; }
        public string Notes { get; set; }
        public int Language1Id { get; set; }
        public int Language2Id { get; set; }
        public int Language2Level { get; set; }
        public int Language3Id { get; set; }
        public int Language3Level { get; set; }
        public string ContractPriceNotes { get; set; }
        public int ContractTypeId { get; set; }
        public int CategoryId { get; set; }
        public int StatusId { get; set; }
        public int LocationId { get; set; }
        public int WorkLocationId { get; set; }
        public string UpdateDateRaw { get; set; }
        public string AvailabilityNotes { get; set; }
        public string BirthPlace { get; set; }
        public int EnglishTestResult { get; set; }
        public int StatusReasonId { get; set; }
            */
            this.Property(t => t.Id).HasColumnName("idcandidato");
            this.Property(t => t.FirstName).HasColumnName("nome");
            this.Property(t => t.LastName).HasColumnName("cognome");
            this.Property(t => t.InterviewDate).HasColumnName("InterviewDate");
            this.Property(t => t.UserId).HasColumnName("idselezionatore");
            this.Property(t => t.BirthDate).HasColumnName("BirthDate");
            this.Property(t => t.Notes).HasColumnName("profilo");
            this.Property(t => t.Language1Id).HasColumnName("lingua1");
            this.Property(t => t.Language2Id).HasColumnName("lingua2");
            this.Property(t => t.Language2Level).HasColumnName("lingua2livello");
            this.Property(t => t.Language3Id).HasColumnName("lingua3");
            this.Property(t => t.Language3Level).HasColumnName("lingua3livello");
            this.Property(t => t.ContractPriceNotes).HasColumnName("contrattoimporto");
            this.Property(t => t.JobCategoryId).HasColumnName("idcategoria");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
            this.Property(t => t.StatusId).HasColumnName("idstato");
            this.Property(t => t.StatusReasonId).HasColumnName("idmotivo");
            this.Property(t => t.WorkLocationId).HasColumnName("idluogolavoro");
            this.Property(t => t.AvailabilityNotes).HasColumnName("disponibilita");

            HasRequired(t => t.JobCategory).WithMany().HasForeignKey(e => e.JobCategoryId);
            HasOptional(t => t.Status).WithMany().HasForeignKey(e => e.StatusId);
            HasOptional(t => t.StatusReason).WithMany().HasForeignKey(e => e.StatusReasonId);
            HasOptional(t => t.WorkLocation).WithMany().HasForeignKey(e => e.WorkLocationId);
            HasOptional(t => t.Language1).WithMany().HasForeignKey(e => e.Language1Id);
            HasOptional(t => t.Language2).WithMany().HasForeignKey(e => e.Language2Id);
            HasOptional(t => t.Language3).WithMany().HasForeignKey(e => e.Language3Id);
            HasOptional(t => t.User).WithMany().HasForeignKey(e => e.UserId);

            this.Ignore(t => t.DisplayName);
            this.Ignore(t => t.StatusCalculated);
        }
    }
}