using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace AGM.Web.Infrastructure
{
    public class CustomPrincipal : IPrincipal
    {
        public string User { get; private set; }
        public string[] roles { get; set; }
        public IIdentity Identity { get; private set; }

        public CustomPrincipal(string user)
        {
            User = user;
            this.Identity = new GenericIdentity(user);
        }

        public bool IsInRole(string role)
        {
            if (roles.Any(r => role.Contains(r)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}