using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AGM.Web.Models
{
    public class MHReportLock
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Month { get; set; }
        public DateTime LockDate { get; set; }
        public DateTime UnlockDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}