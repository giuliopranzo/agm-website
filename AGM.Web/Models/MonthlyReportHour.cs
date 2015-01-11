using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace AGM.Web.Models
{
    public class MonthlyReportHour
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ReasonId { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string HoursRaw { get; set; }

        public double HoursCount
        {
            get
            {
                var cultureIt = CultureInfo.GetCultureInfo("it-IT");
                double o;
                double.TryParse(HoursRaw, NumberStyles.Any, cultureIt, out o);
                return Math.Round(o,2);
            }
        }

        public string HoursCountString
        {
            get
            {
                var cultureIt = CultureInfo.GetCultureInfo("it-IT");
                return HoursCount.ToString("N2", cultureIt);
            }
        }

        public DateTime Date
        {
            get { return new DateTime(Year, Month, Day); }
        }

        public string Reason
        {
            get
            {
                if (AgmStaticDataContext.HourReasons.Any(r => r.Id == ReasonId))
                    return AgmStaticDataContext.HourReasons.First(r => r.Id == ReasonId).Name;
                return null;
            }
        }
    }
}