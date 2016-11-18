using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AGM.Web.Models
{
    public class Notice
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date     { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public bool IsDeleted { get; set; }
    }
}