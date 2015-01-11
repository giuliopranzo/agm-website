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
                var currentUser = context.Users.Single(u => u.Email == currentEmail);
                if (id != currentUser.Id && !checkFunction(currentUser))
                    throw new Exception("Operazione non autorizzata");
            }
        }
    }
}