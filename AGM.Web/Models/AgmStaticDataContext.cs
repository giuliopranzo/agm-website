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
        public static List<UserType> UserTypes = GetUserTypes();

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

        private static List<UserType> GetUserTypes()
        {
            using (var context = new AgmDataContext())
            {
                return context.UserTypes.ToList();
            }
        }
    }

    
}