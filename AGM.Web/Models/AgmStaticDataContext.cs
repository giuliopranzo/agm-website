using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AGM.Web.Models
{
    public static class AgmStaticDataContext
    {
        public static List<HourReason> HourReasons = GetHourReasons();
        public static List<ExpenseReason> ExpenseReasons = GetExpenseReasons();

        private static List<HourReason> GetHourReasons()
        {
            using (var context = new AgmDataContext())
            {
                return context.HourReasons.ToList();
            }
        }

        private static List<ExpenseReason> GetExpenseReasons()
        {
            using (var context = new AgmDataContext())
            {
                return context.ExpenseReasons.ToList();
            }
        }
    }

    
}