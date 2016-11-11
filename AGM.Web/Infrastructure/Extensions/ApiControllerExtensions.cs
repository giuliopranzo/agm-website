using AGM.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace AGM.Web.Infrastructure.Extensions
{
    public static class ApiControllerExtensions
    {
        public static void CheckCurrentUserPermission(this ApiController o, int id, Func<User,bool> checkFunction)
        {
            using (var context = new AgmDataContext())
            {
                var currentEmail = (Thread.CurrentPrincipal as CustomPrincipal).User.Split('$').GetValue(0) as string;
                var currentUser = context.Users.Single(u => u.Email.ToLower() == currentEmail.ToLower() && !u._isDeleted && u._isActive == 1);
                if (id != currentUser.Id && !checkFunction(currentUser))
                    throw new Exception("Operazione non autorizzata");
            }
        }

        public static void CheckCurrentUserPermission(this ApiController o, Func<User, bool> checkFunction)
        {
            using (var context = new AgmDataContext())
            {
                var currentEmail = (Thread.CurrentPrincipal as CustomPrincipal).User.Split('$').GetValue(0) as string;
                var currentUser = context.Users.Single(u => u.Email.ToLower() == currentEmail.ToLower() && !u._isDeleted && u._isActive == 1);
                if (!checkFunction(currentUser))
                    throw new Exception("Operazione non autorizzata");
            }
        }

        public static User GetCurrentUser(this ApiController o)
        {
            using (var context = new AgmDataContext())
            {
                var currentEmail = (Thread.CurrentPrincipal as CustomPrincipal).User.Split('$').GetValue(0) as string;
                var currentUser =
                    context.Users.Single(
                        u => u.Email.ToLower() == currentEmail.ToLower() && !u._isDeleted && u._isActive == 1);

                return currentUser;
            }
        }

        public static MonthlyReportCalendar GetUserMonthlyCalendar(this ApiController o, int userId, string month)
        {
            using (var context = new AgmDataContext())
            {
                return new MonthlyReportCalendar(context.MonthlyReportDays(userId, month).ToList());
            }
        }
    }
}