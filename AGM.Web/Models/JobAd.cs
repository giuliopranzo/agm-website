using System;
using System.Collections.Generic;
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
                return DateTime.Parse(DateFromRaw);
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
                return DateTime.Parse(DateToRaw);
            }
            set
            {
                DateToRaw = value.ToString("dd/MM/yyyy");
            }
        }
    }
}