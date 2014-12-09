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

        public string Company { get { return "COMPANY"; } }

        public string Name
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }

        }

        public string Picture
        {
            get { return string.Format("{0}{1}", "https://unsplash.it/48/48?image=", Id); }
        }
    }
}