using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace AGM.Web.Models
{
    public class Candidate
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string InterviewDateRaw { get; set; }
        public string BirthDateRaw { get; set; }
        public string Description { get; set; }
        public int Language1 { get; set; }
        public int Language2 { get; set; }
        public int Language2Level { get; set; }
        public int Language3 { get; set; }
        public int Language3Level { get; set; }
        public string ActualSalaryRaw { get; set; }
        public int ContractType { get; set; }
        public int JobCategory { get; set; }
        public int StatusId { get; set; }
        public int InterviewerId { get; set; }
        public int ResidenceTown { get; set; }
        public string UpdateDateRaw { get; set; }
        public string AvailableIn { get; set; }
        public string BirthPlace { get; set; }
        public int EnglishTest { get; set; }
        public int ReasonId { get; set; }
        public int WorkTown { get; set; }

        public DateTime InterviewDate
        {
            get
            {
                var cultureIt = CultureInfo.GetCultureInfo("it-IT");
                return DateTime.Parse(InterviewDateRaw, cultureIt);
            }
            set
            {
                InterviewDateRaw = value.ToString("dd/MM/yyyy");
            }
        }

        public DateTime BirthDate
        {
            get
            {
                var cultureIt = CultureInfo.GetCultureInfo("it-IT");
                return DateTime.Parse(BirthDateRaw, cultureIt);
            }
            set
            {
                BirthDateRaw = value.ToString("dd/MM/yyyy");
            }
        }

        public DateTime UpdateDate
        {
            get
            {
                var cultureIt = CultureInfo.GetCultureInfo("it-IT");
                return DateTime.Parse(UpdateDateRaw, cultureIt);
            }
            set
            {
                UpdateDateRaw = value.ToString("dd/MM/yyyy");
            }
        }

        public float ActualSalary
        {
            get
            {
                var cultureIt = CultureInfo.GetCultureInfo("it-IT");
                return float.Parse(ActualSalaryRaw, cultureIt);
            }
            set
            {
                var cultureIt = CultureInfo.GetCultureInfo("it-IT");
                ActualSalaryRaw = value.ToString(cultureIt);
            }
        }

        public string Name
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }
    }
}