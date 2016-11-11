using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AGM.Web.Models
{
    public class MonthlyReportDay
    {
        public DateTime Date { get; set; }
        public bool Festivity { get; set; }
        public Decimal OrdinaryHours { get; set; }
        public Decimal OvertimeHours { get; set; }
    }
}