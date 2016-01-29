using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AGM.Web.Models
{
    public class JobApplicant
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? InterviewDate { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Notes { get; set; }
        public int? Language1Id { get; set; }
        public Language Language1 { get; set; }
        public int? Language2Id { get; set; }
        public Language Language2 { get; set; }
        public int? Language2Level { get; set; }
        public int? Language3Id { get; set; }
        public Language Language3 { get; set; }
        public int? Language3Level { get; set; }
        public string ContractPriceNotes { get; set; }
        //public int ContractTypeId { get; set; }
        public int JobCategoryId { get; set; }
        public JobCategory JobCategory { get; set; }
        public int? StatusId { get; set; }
        public JobApplicantStatus Status { get; set; }
        //public int LocationId { get; set; }
        public int? WorkLocationId { get; set; }
        public Location WorkLocation { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string AvailabilityNotes { get; set; }
        //public string BirthPlace { get; set; }
        //public int EnglishTestResult { get; set; }
        public int? StatusReasonId { get; set; }
        public JobApplicantStatusReason StatusReason { get; set; }

        public string DisplayName
        {
            get
            {
                return string.Format("{0} {1}", LastName, FirstName);
            }
        }

        public string StatusCalculated
        {
            get
            {
                if (StatusReason != null && StatusReason.Id != 1)
                    return StatusReason.Name;
                return (Status != null) ? Status.Name : string.Empty;
            }
        }
    }
}