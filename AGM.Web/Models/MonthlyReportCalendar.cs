using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AGM.Web.Models
{
    public class MonthlyReportCalendar
    {
        public List<MonthlyReportDay> Days;

        public MonthlyReportCalendar()
        {
            Days = new List<MonthlyReportDay>();
        }

        public MonthlyReportCalendar(List<MonthlyReportDay> days)
        {
            Days = days;
        }
    }
}