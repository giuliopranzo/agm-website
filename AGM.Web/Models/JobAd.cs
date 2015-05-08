using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace AGM.Web.Models
{
    public class JobAd
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public string DateFromRaw { get; set; }
        public string DateToRaw { get; set; }
        public string RefCode { get; set; }

        public DateTime DateFrom
        {
            get
            {
                var cultureIt = CultureInfo.GetCultureInfo("it-IT");
                return DateTime.Parse(DateFromRaw, cultureIt);
            }
            set
            {
                DateFromRaw = value.ToString("dd/MM/yyyy");
            }
        }

        public DateTime DateTo
        {
            get
            {
                var cultureIt = CultureInfo.GetCultureInfo("it-IT");
                return DateTime.Parse(DateToRaw, cultureIt);
            }
            set
            {
                DateToRaw = value.ToString("dd/MM/yyyy");
            }
        }

        public bool Expired
        {
            get
            {
                return DateTo < DateTime.Today;
            }
        }

        public bool AlmostExpired
        {
            get
            {
                return DateTo.AddDays(-3) <= DateTime.Today;
            }
        }
    }
}