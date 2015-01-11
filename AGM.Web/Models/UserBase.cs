using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AGM.Web.Models
{
    public class UserBase
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string Company { get { return "COMPANY"; } }

        public string Name
        {
            get { return string.Format("{0} {1}", LastName, FirstName); }

        }

        public string Picture
        {
            get { return string.Format("http://robohash.org/bgset_bg1/agmuser_{0}?size=48x48", Id); }
        }
    }
}