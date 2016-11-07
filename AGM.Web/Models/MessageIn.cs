using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AGM.Web.Models
{
    public class MessageIn
    {
        public string Subject { get; set; }
        public string Text { get; set; }
        public int SendToAll { get; set; }
        public int[] ToUserIds { get; set; }
    }
}